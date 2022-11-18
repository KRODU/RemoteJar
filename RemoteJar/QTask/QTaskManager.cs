using RemoteJar.Model;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteJar.QTask
{
    public class QTaskManager : IDisposable
    {
        private readonly LimitedLevelTaskScheduler ts;
        private readonly TaskFactory tf;
        private readonly SortedList<DateTime, QTaskModel> taskBook;
        private readonly System.Timers.Timer taskBookTimer;
        /// <summary>
        /// 모든 작업이 완료되었을 경우 실행되는 이벤트
        /// </summary>
        public event Action CompleteTaskEvent;

        /// <summary>
        /// 작업이 시작할 때 호출
        /// </summary>
        public event Action<QTaskModel> TaskStartEvent;

        /// <summary>
        /// 작업이 끝날 때 호출
        /// </summary>
        public event Action<QTaskModel> TaskEndEvent;

        /// <summary>
        /// 에러 발생시 처리하기 위한 delegate
        /// </summary>
        /// <param name="errTask">에러가 발생한 Task</param>
        /// <param name="ex">발생한 예외</param>
        /// <param name="nextRetry">작업이 retry를 시도하는지 여부를 나타냄</param>
        public delegate void TaskErrorEventHandler(QTaskModel errTask, Exception ex, bool nextRetry);

        /// <summary>
        /// 작업 에러가 발생할 때 호출
        /// </summary>
        public event TaskErrorEventHandler TaskErrorEvent;

        /// <summary>
        /// 예약 작업을 포함한 모든 작업이 다 끝난 경우에 true 입니다.
        /// </summary>
        public bool CompleteTask
        {
            get
            {
                lock (ts)
                {
                    lock (taskBook)
                    {
                        return taskBook.Count == 0 && ts.RunningThreadCnt == 0 && ts.RemainingTaskCnt == 0;
                    }
                }
            }
        }

        public QTaskManager()
        {
            ts = new LimitedLevelTaskScheduler(3);
            ts.CompleteTaskEvent += TS_CompleteTaskEvent;
            tf = new TaskFactory(ts);
            taskBook = new SortedList<DateTime, QTaskModel>(new DuplicateDateTimeComparer());
            taskBookTimer = new System.Timers.Timer();
            taskBookTimer.Elapsed += TaskBookTimerEvent;
            ResetTaskBookTimer();
        }

        private void TS_CompleteTaskEvent()
        {
            if (CompleteTask)
            {
                CompleteTaskEvent?.Invoke();
            }
        }

        private void TaskBookTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            TaskBookWork(DateTime.Now, true);
        }

        private void TaskBookWork(DateTime stanDate, bool resetTimer)
        {
            lock (taskBook)
            {
                while (taskBook.Count > 0)
                {
                    if (stanDate.CompareTo(taskBook.Keys[0]) >= 0)
                    {
                        QTaskModel iQTask = taskBook.Values[0];
                        taskBook.RemoveAt(0);
                        AddNewTask(iQTask);
                    }
                    else
                    {
                        break;
                    }
                }

                if (resetTimer)
                {
                    ResetTaskBookTimer();
                }
            }
        }

        public void AddTaskBook(DateTime time, QTaskModel qTask)
        {
            if (qTask == null)
                return;

            if (DateTime.Now.CompareTo(time) >= 0)
            {
                AddNewTask(qTask);
                return;
            }

            lock (taskBook)
            {
                taskBook.Add(time, qTask);
                ResetTaskBookTimer();
            }
        }

        private void ResetTaskBookTimer()
        {
            lock (taskBook)
            {
                while (true)
                {
                    if (taskBook.Count == 0)
                    {
                        taskBookTimer.Stop();
                    }
                    else
                    {
                        int interval;
                        DateTime now = DateTime.Now;
                        double secFrom = (taskBook.Keys[0] - now).TotalSeconds;

                        if (secFrom <= 0)
                        {
                            TaskBookWork(now, false);
                            continue;
                        }
                        else if (secFrom > 60)
                            interval = 60 * 1000;
                        else
                            interval = (int)(secFrom * 1000);

                        if (interval < 100)
                            interval = 100;

                        taskBookTimer.Interval = interval;
                        taskBookTimer.Start();
                    }
                    break;
                }
            }
        }

        public void AddNewTask(QTaskModel qTask)
        {
            if (qTask == null)
                return;

            lock (ts)
            {
                tf.StartNew(WorkPulse, qTask);
            }
        }

        private void WorkPulse(object state)
        {
            QTaskModel qTask = (QTaskModel)state;
            try
            {
                TaskStartEvent?.Invoke(qTask);
                switch (qTask)
                {
                    case TaskBlockAndWait blockAndWait:
                        TaskBlockAndWait(blockAndWait);
                        break;
                    case TaskJarUpload jarUpload:
                        JarUpdate(jarUpload);
                        break;
                    default:
                        Program.Log.WriteLine($"구현되지 않은 state 종류: {state.GetType().FullName}");
                        break;
                }
                TaskEndEvent?.Invoke(qTask);
            }
            catch (SshConnectionException ex)
            {
                Program.Log.WriteLine($"서버 접속 에러 발생. IP 및 접속 정보가 올바른지 확인해주세요: {qTask.Client.Host}{Environment.NewLine}{ex.Message}");
                TaskErrorEvent?.Invoke(qTask, ex, false);
            }
            catch (SshAuthenticationException ex)
            {
                Program.Log.WriteLine($"서버 접속 에러 발생. IP 및 접속 정보가 올바른지 확인해주세요: {qTask.Client.Host}{Environment.NewLine}{ex.Message}");
                TaskErrorEvent?.Invoke(qTask, ex, false);
            }
            catch (Exception ex)
            {
                // 오류 발생시 로깅 및 재시도
                Program.Log.WriteLine($"작업 중 에러 발생. 현재 재시도 횟수:{qTask.CurRetryCnt}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                if (qTask.AddRetryCnt())
                {
                    TaskErrorEvent?.Invoke(qTask, ex, true);
                    AddNewTask(qTask);
                }
                else
                {
                    TaskErrorEvent?.Invoke(qTask, ex, false);
                }
            }
        }

        private void TaskBlockAndWait(TaskBlockAndWait task)
        {
            task.Client.BlockCrontab(from dir in task.TaskDir select dir.CrontabPath);

            List<JarProc> procList = task.Client.GetJarProcs();

            // 작업이 가능한 JarDir 목록
            IEnumerable<JarDir> jarTaskList;

            if (task.ForceKillTask)
            {
                // 작업중인 프로세스를 모두 kill하고 모든 jar파일을 바로 업데이트
                List<string> taskNameList = (from dir in task.TaskDir select dir.ProcName).ToList();

                task.Client.KillProcessList(from proc in procList where taskNameList.Contains(proc.Name) select proc.PID);
                jarTaskList = task.TaskDir;
            }
            else
            {
                // 작업중인 프로세스는 내버려두고 실행중이지 않은 파일만 업데이트
                List<string> procNameList = (from proc in procList select proc.Name).ToList();

                jarTaskList = from dir in task.TaskDir where !procNameList.Contains(dir.ProcName) select dir;

                // 나중에 다시 재확인하도록 예약 작업에 추가
                HashSet<JarDir> newTaskDirSet = (from dir in task.TaskDir where procNameList.Contains(dir.ProcName) select dir).ToHashSet();
                if (newTaskDirSet.Count > 0)
                {
                    TaskBlockAndWait bookTask = (TaskBlockAndWait)task.Clone();
                    bookTask.TaskDir = newTaskDirSet;
                    AddTaskBook(DateTime.Now + TimeSpan.FromSeconds(60), bookTask);
                    Program.Log.WriteLine($"{bookTask.Client.Host} 에서 프로세스가 아직 실행 중. 1분 뒤에 재시도: {(from dir in bookTask.TaskDir select dir.ServerJarPath).Combine("\"", ",")} ");
                }
            }

            foreach (JarDir taskDir in jarTaskList)
            {
                TaskJarUpload newTask = (TaskJarUpload)task.TaskJarUpload.Clone();
                newTask.JarDir = taskDir;
                AddNewTask(newTask);
            }
        }

        private void JarUpdate(TaskJarUpload task)
        {
            task.Client.UploadFile(task.LocalJarPath, task.JarDir.ServerJarPath);
            task.Client.RollbackCrontab(task.JarDir);

            if (task.ForceRun)
            {
                task.Client.RunSh(task.JarDir.CrontabPath);
            }
        }

        public void Dispose()
        {
            taskBookTimer.Dispose();
            GC.SuppressFinalize(this);
        }

        ~QTaskManager()
        {
            Dispose();
        }

        private class DuplicateDateTimeComparer : IComparer<DateTime>
        {
            public int Compare(DateTime x, DateTime y)
            {
                int result = x.CompareTo(y);
                return result == 0 ? 1 : result;
            }
        }
    }
}
