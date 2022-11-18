using RemoteJar.Model;
using RemoteJar.QTask;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RemoteJar
{
    public partial class FrmRemoteJarMain : Form
    {
        private readonly Config config = new Config();
        private readonly QTaskManager qTask = new QTaskManager();
        private readonly Mutex singleInstanceMutex = new Mutex(false, "{87EF359C-40D9-42BD-95C9-D2D214354F2F}");
        private readonly QTaskFileLocker fileLocker;
        private int logFlushTic = 0;

        public FrmRemoteJarMain()
        {
            InitializeComponent();
            Program.Log.NewLogEvent += Log_NewLogEvent;
            qTask.CompleteTaskEvent += QTask_CompleteTaskEvent;
            qTask.TaskStartEvent += QTask_TaskStartEvent;
            qTask.TaskEndEvent += QTask_TaskEndEvent;
            qTask.TaskErrorEvent += QTask_TaskErrorEvent;

            fileLocker = new QTaskFileLocker(qTask);
            fileLocker.LockFileEvent += FileLocker_LockFileEvent;
            fileLocker.UnLockFileEvent += FileLocker_UnLockFileEvent;
        }

        private void FileLocker_LockFileEvent(string str)
        {
            Program.Log.WriteLine($"파일 잠금: {str}");
        }
        private void FileLocker_UnLockFileEvent(string str)
        {
            Program.Log.WriteLine($"파일 잠금 해제: {str}");
        }

        private void QTask_TaskEndEvent(QTaskModel obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => QTask_TaskEndEvent(obj)));
                return;
            }

            lock (GridTask)
            {
                switch (obj)
                {
                    case TaskBlockAndWait blockAndWait:
                        foreach (DataGridViewRow curRow in from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && blockAndWait.TaskDir.Contains(taskDir) select row)
                        {
                            if (blockAndWait.ForceKillTask)
                            {
                                curRow.Cells[2].Value = "프로세스 종료 완료";
                            }
                            else
                            {
                                curRow.Cells[2].Value = "프로세스 종료 대기 중";
                            }
                        }
                        break;
                    case TaskJarUpload jarUpload:
                        foreach (DataGridViewRow curRow in from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && jarUpload.JarDir == taskDir select row)
                        {
                            curRow.Cells[2].Value = "작업 완료";
                            curRow.Tag = DateTime.Now + TimeSpan.FromSeconds(60);
                        }
                        break;
                }
            }
        }

        private void QTask_TaskStartEvent(QTaskModel obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => QTask_TaskStartEvent(obj)));
                return;
            }

            lock (GridTask)
            {
                switch (obj)
                {
                    case TaskBlockAndWait blockAndWait:
                        foreach (DataGridViewRow curRow in from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && blockAndWait.TaskDir.Contains(taskDir) select row)
                        {
                            if (blockAndWait.ForceKillTask)
                            {
                                curRow.Cells[2].Value = "프로세스 종료 중";
                            }
                            else
                            {
                                curRow.Cells[2].Value = "프로세스 실행여부 확인 중";
                            }
                        }
                        break;
                    case TaskJarUpload jarUpload:
                        foreach (DataGridViewRow curRow in from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && jarUpload.JarDir == taskDir select row)
                        {
                            curRow.Cells[2].Value = "파일 업로드 중";
                        }
                        break;
                }
            }
        }

        private void QTask_CompleteTaskEvent()
        {
            Program.Log.WriteLine("모든 작업이 완료되었습니다.");
        }

        private void QTask_TaskErrorEvent(QTaskModel errTask, Exception ex, bool nextRetry)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => QTask_TaskErrorEvent(errTask, ex, nextRetry)));
                return;
            }

            lock (GridTask)
            {
                switch (errTask)
                {
                    case TaskBlockAndWait blockAndWait:
                        foreach (DataGridViewRow curRow in from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && blockAndWait.TaskDir.Contains(taskDir) select row)
                        {
                            if (nextRetry)
                            {
                                curRow.Cells[2].Value = "오류 발생. 재시도 대기 중";
                            }
                            else
                            {
                                curRow.Cells[2].Value = "오류로 작업 중단";
                                curRow.Tag = DateTime.Now + TimeSpan.FromSeconds(60 * 10);
                            }
                        }
                        break;
                    case TaskJarUpload jarUpload:
                        foreach (DataGridViewRow curRow in from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && jarUpload.JarDir == taskDir select row)
                        {
                            if (nextRetry)
                            {
                                curRow.Cells[2].Value = "오류 발생. 재시도 대기 중";
                            }
                            else
                            {
                                curRow.Cells[2].Value = "오류로 작업 중단";
                                curRow.Tag = DateTime.Now + TimeSpan.FromSeconds(60 * 10);
                            }
                        }
                        break;
                }
            }
        }

        private void Log_NewLogEvent(string log)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log_NewLogEvent(log)));
                return;
            }
            txtLog.AppendText(log);
            while (txtLog.TextLength > 12000)
            {
                txtLog.Text = txtLog.Text.Remove(0, 2000);
            }
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
            txtLog.ClearUndo();
        }

        private void FrmRemoteJarMain_Load(object sender, EventArgs e)
        {
            if (!singleInstanceMutex.WaitOne(TimeSpan.Zero))
            {
                MessageBox.Show("프로그램이 이미 실행중이므로 종료합니다.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            Program.Log.OpenLogFile();
            try
            {
                config.Load("config.xml");
            }
            catch (XmlConfigValidationException ex)
            {
                string errMsg = $"config.xml 에서 검증 오류가 발생하였습니다.{Environment.NewLine}{Environment.NewLine}{ex.Message}";
                Program.Log.WriteLine(errMsg);
                MessageBox.Show(errMsg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            catch (Exception ex)
            {
                string errMsg = $"config.xml 파일을 로딩 중 오류가 발생하였습니다.{Environment.NewLine}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}";
                Program.Log.WriteLine(errMsg);
                MessageBox.Show(errMsg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

            List<string> sortList = new List<string>(config.JarIdDic.Keys);
            sortList.Sort();
            foreach (string jarId in sortList)
            {
                ListJarId.Items.Add(jarId);
            }
            Program.Log.WriteLine("프로그램 시작");
        }

        private void FrmRemoteJarMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.Log.WriteLine("프로그램 종료");
            Program.Log.Dispose();
            singleInstanceMutex.Dispose();
            fileLocker.Dispose();
            Application.Exit();
        }

        private void FrmRemoteJarMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (qTask.CompleteTask)
                return;

            if (MessageBox.Show("아직 모든 작업이 다 끝나지 않았습니다. 정말로 프로그램을 종료하시겠습니까?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ListJarId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListJarId.SelectedItem is string selJarid)
            {
                GridDir.SuspendLayout();
                try
                {
                    GridDir.Rows.Clear();
                    foreach (Client client in config.ClientDic.Keys)
                    {
                        foreach (JarDir selDir in from dir in config.ClientDic[client] where dir.JarId == selJarid select dir)
                        {
                            int rowCnt = GridDir.Rows.Add(false, selDir.Client.Name, selDir.Client.Host, selDir.ServerJarPath);
                            GridDir.Rows[rowCnt].Tag = selDir;
                        }
                    }
                }
                finally
                {
                    GridDir.ResumeLayout();
                }
            }
        }

        private void BtnPopupPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog opDia = new OpenFileDialog()
            {
                Filter = "모든파일|*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            })
            {
                if (opDia.ShowDialog() != DialogResult.OK)
                    return;
                txtLocalPath.Text = opDia.FileName;
            }
        }

        private void BtnTaskStart_Click(object sender, EventArgs e)
        {
            List<JarDir> selected = (from DataGridViewRow row in GridDir.Rows where (bool)row.Cells[0].Value select (JarDir)row.Tag).ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show($"하나 이상의 선택된 업로드 경로가 없습니다.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtLocalPath.TextLength == 0)
            {
                MessageBox.Show($"업로드 파일을 입력하지 않았습니다.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lock (GridTask)
            {
                // 이미 작업중인 목록과의 중복 체크
                List<JarDir> dupList = (from DataGridViewRow row in GridTask.Rows where row.Tag is JarDir taskDir && selected.Contains(taskDir) select (JarDir)row.Tag).ToList();
                if (dupList.Count > 0)
                {
                    MessageBox.Show($"현재 진행중인 작업과의 중복이 존재합니다.{Environment.NewLine}{(from dir in dupList select dir.ToString()).Combine(null, Environment.NewLine)}",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    fileLocker.AddLock(txtLocalPath.Text, selected.Count);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"선택한 파일을 여는 중 오류가 발생하였습니다.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (JarDir dir in selected)
                {
                    int rowNo = GridTask.Rows.Add(dir.Client.Host, dir.ServerJarPath, "대기중");
                    GridTask.Rows[rowNo].Tag = dir;
                }

                foreach (Client client in (from dir in selected select dir.Client).Distinct())
                {
                    List<JarDir> taskList = (from dir in selected where dir.Client == client select dir).ToList();

                    TaskJarUpload taskJarUpload = new TaskJarUpload() { LocalJarPath = txtLocalPath.Text, Client = client, ForceRun = ckRun.Checked };
                    qTask.AddNewTask(new TaskBlockAndWait() { Client = client, TaskDir = taskList, TaskJarUpload = taskJarUpload, ForceKillTask = ckForceKill.Checked });
                }

                Program.Log.WriteLine("새로운 작업이 등록되었습니다.");
            }
        }

        private void GridDir_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 체크박스 헤더 클릭시
            if (e.ColumnIndex == 0 && GridDir.Rows.Count > 0)
            {
                try
                {
                    GridDir.SuspendLayout();
                    bool newChk = !(bool)GridDir.Rows[0].Cells[0].Value;

                    List<DataGridViewRow> backupRow = new List<DataGridViewRow>();
                    foreach (DataGridViewRow row in GridDir.Rows)
                    {
                        backupRow.Add(row);
                    }
                    GridDir.Rows.Clear();

                    foreach (DataGridViewRow row in backupRow)
                    {
                        row.Cells[0].Value = newChk;
                        GridDir.Rows.Add(row);
                    }
                }
                finally
                {
                    GridDir.ResumeLayout();
                }
            }
        }

        private void ToolStripMenuItemLogClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            txtLog.ClearUndo();
        }

        private void ToolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            txtLog.Copy();
        }

        private void TimerSec_Tick(object sender, EventArgs e)
        {
            if (++logFlushTic >= 10)
            {
                logFlushTic = 0;
                Program.Log.Flush();
            }

            lock (GridTask)
            {
                List<DataGridViewRow> rows = (from DataGridViewRow row in GridTask.Rows where row.Tag is DateTime regDate && (DateTime.Now - regDate).Ticks >= 0 select row).ToList();

                foreach (DataGridViewRow delRow in rows)
                {
                    GridTask.Rows.Remove(delRow);
                }
            }
        }
    }
}
