using System;
using System.Collections.Generic;
using System.IO;

namespace RemoteJar.QTask
{
    /// <summary>
    /// 프로그램에서 파일을 사용하는 동안 수정할 수 없도록 잠급니다.
    /// </summary>
    public class QTaskFileLocker : IDisposable
    {
        private readonly Dictionary<string, FileRefCounter> lockedFiles = new Dictionary<string, FileRefCounter>();
        public event Action<string> LockFileEvent;
        public event Action<string> UnLockFileEvent;
        private readonly QTaskManager qTaskManager;
        private bool isDisposed = false;

        public QTaskFileLocker(QTaskManager qTaskManager)
        {
            this.qTaskManager = qTaskManager;
            qTaskManager.TaskEndEvent += QTaskManager_TaskEndEvent;
            qTaskManager.TaskErrorEvent += QTaskManager_TaskErrorEvent;
        }

        private void QTaskManager_TaskEndEvent(QTaskModel obj)
        {
            if (obj is TaskJarUpload taskJarUpload)
            {
                MinusLock(taskJarUpload.LocalJarPath);
            }
        }

        private void QTaskManager_TaskErrorEvent(QTaskModel errTask, Exception ex, bool nextRetry)
        {
            if (nextRetry)
                return;

            switch (errTask)
            {
                case TaskBlockAndWait blockAndWait:
                    MinusLock(blockAndWait.TaskJarUpload.LocalJarPath);
                    break;
                case TaskJarUpload jarUpload:
                    MinusLock(jarUpload.LocalJarPath);
                    break;
                default:
                    throw new NotImplementedException($"구현되지 않은 state 종류: {errTask.GetType().FullName}");
            }
        }

        private void MinusLock(string path)
        {
            if (isDisposed)
                return;

            lock (lockedFiles)
            {
                if (lockedFiles.TryGetValue(path, out FileRefCounter fileRefCounterVal))
                {
                    if (--fileRefCounterVal.RefCount <= 0)
                    {
                        fileRefCounterVal.Stream.Dispose();
                        lockedFiles.Remove(path);
                        UnLockFileEvent?.Invoke(path);
                    }
                }
            }
        }

        public void AddLock(string path, int refCnt)
        {
            if (isDisposed)
                return;

            if (refCnt <= 0)
                return;

            lock (lockedFiles)
            {
                if (lockedFiles.TryGetValue(path, out FileRefCounter fileRefCounterVal))
                {
                    fileRefCounterVal.RefCount += refCnt;
                }
                else
                {
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1);
                    fileRefCounterVal = new FileRefCounter() { Stream = stream, RefCount = refCnt };
                    lockedFiles.Add(path, fileRefCounterVal);
                    LockFileEvent?.Invoke(path);
                }
            }
        }

        private class FileRefCounter
        {
            public int RefCount { get; set; }
            public FileStream Stream { get; set; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                lock (lockedFiles)
                {
                    DisposeAllStream();
                }
            }
            else
            {
                DisposeAllStream();
            }
            isDisposed = true;

            void DisposeAllStream()
            {
                qTaskManager.TaskEndEvent -= QTaskManager_TaskEndEvent;
                qTaskManager.TaskErrorEvent -= QTaskManager_TaskErrorEvent;

                foreach (FileRefCounter val in lockedFiles.Values)
                {
                    val.Stream.Dispose();
                }
            }
        }

        ~QTaskFileLocker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
