using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RemoteJar.Model
{
    public class Client : IEquatable<Client>
    {
        private static readonly Regex procRgex = new Regex(@"^[^\s]*\s+([0-9]+)[\S ]*?java[\S ]*?-Du=([^\s]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex crontabPath = new Regex(@"^[ ]*[^# \r\n]+[ ]+[^# \r\n]+[ ]+[^# \r\n]+[ ]+[^# \r\n]+[ ]+[^# \r\n]+[ ]+([^#\r\n]+)", RegexOptions.Compiled | RegexOptions.Multiline);
        private const string BlockingLabel = "#!BLOCK!";

        public string Name { get; private set; }
        public string Host { get; private set; }
        public string User { get; private set; }
        public string Pwd { get; private set; }

        public Client(string name, string host, string user, string pwd)
        {
            Name = name;
            Host = host;
            User = user;
            Pwd = pwd;
        }

        private SshClient GetSshClient()
        {
            SshClient sshClient = new SshClient(Host, User, Pwd);
            sshClient.Connect();
            return sshClient;
        }

        private SftpClient GetSftpClient()
        {
            SftpClient sftpClient = new SftpClient(Host, User, Pwd);
            sftpClient.Connect();
            return sftpClient;
        }

        /// <summary>
        /// ps -ef|grep java를 실행하여 PID와 Name 목록 반환
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<JarProc> GetJarProcs()
        {
            List<JarProc> procList = new List<JarProc>();
            string procResult;

            using (SshClient client = GetSshClient())
            {
                procResult = client.RunCommand("ps -ef|grep java").Result;
            }
            Match m = procRgex.Match(procResult);
            while (m.Success)
            {
                procList.Add(new JarProc() { Host = Host, PID = m.Groups[1].Value, Name = m.Groups[2].Value });
                m = m.NextMatch();
            }

            return procList;
        }

        /// <summary>
        /// pathList와 일치하는 경로의 Path에 대하여 crontab을 차단
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool BlockCrontab(IEnumerable<string> pathList)
        {
            string oldCrontab;
            using (SshClient client = GetSshClient())
            {
                oldCrontab = client.RunCommand("crontab -l").Result;
            }

            string newCrontab = crontabPath.Replace(oldCrontab, m => pathList.Contains(m.Groups[1].Value.TrimChars()) ? $"{BlockingLabel}{m.Value}" : m.Value);
            // 크론탭이 동일하여 바꿀 필요가 없을 경우 바로 return
            if (oldCrontab == newCrontab)
            {
                return false;
            }

            InstallCrontab(newCrontab);
            Program.Log.WriteLine($"{Host} 에서 crontab 차단 성공: {pathList.Combine("\"", ",")}");
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool RollbackCrontab(JarDir dir)
        {
            string oldCrontab;
            using (SshClient client = GetSshClient())
            {
                oldCrontab = client.RunCommand("crontab -l").Result;
            }

            string[] newCrontabSplit = oldCrontab.Split('\n');
            for (int i = newCrontabSplit.Length - 1; i >= 0; i--)
            {
                string curLine = newCrontabSplit[i];
                if (curLine.Contains(dir.CrontabPath) && curLine.StartsWith(BlockingLabel))
                {
                    curLine = curLine.Substring(BlockingLabel.Length);
                    newCrontabSplit[i] = curLine;
                }
            }

            string newCrontab = string.Join("\n", newCrontabSplit);
            // 크론탭이 동일하여 바꿀 필요가 없을 경우 바로 return
            if (oldCrontab == newCrontab)
            {
                return false;
            }
            InstallCrontab(newCrontab);
            Program.Log.WriteLine($"{Host} 에서 차단한 crontab 롤백 성공: {dir.CrontabPath}");

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InstallCrontab(string newCrontab)
        {
            using (SshClient client = GetSshClient())
            {
                client.RunCommand("cd ~");
                string absPath = client.RunCommand("pwd").Result.TrimChars();

                client.RunCommand("rm ~/newCrontab");
                using (SftpClient sftp = GetSftpClient())
                {
                    sftp.WriteAllText($"{absPath}/newCrontab", newCrontab);
                }
                client.RunCommand("crontab ~/newCrontab");
                client.RunCommand("rm ~/newCrontab");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void KillProcessList(IEnumerable<string> pidList)
        {
            using (SshClient client = GetSshClient())
            {
                foreach (string pid in pidList)
                {
                    string trimPid = pid.TrimChars();
                    client.RunCommand($"kill {trimPid}");
                    Program.Log.WriteLine($"{Host} 에서 pid {trimPid} 프로세스 kill 완료");
                }
            }
        }

        public void UploadFile(string localPath, string serverPath)
        {
            Program.Log.WriteLine($"{Host} 에서 {serverPath} 업로드 시작");
            using (SftpClient sftp = GetSftpClient())
            {
                if (!sftp.Exists(serverPath))
                {
                    Program.Log.WriteLine($"[WARN] {Host} 에서 \"{serverPath}\" 에 파일이 존재하지 않습니다.");
                }
                using (Stream fileInputStream = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
                {
                    sftp.UploadFile(fileInputStream, serverPath, true);
                }
            }
            Program.Log.WriteLine($"{Host} 에서 {serverPath} 업로드 완료");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RunSh(string path)
        {
            using (SshClient client = GetSshClient())
            {
                string trimPath = path.TrimChars();
                client.RunCommand($"nohup {trimPath} > /dev/null 2>&1 &");
                Program.Log.WriteLine($"{Host} 에서 \"{trimPath}\" 실행 완료");
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Client);
        }

        public bool Equals(Client other)
        {
            return other != null &&
                   Host == other.Host;
        }

        public override int GetHashCode()
        {
            return 1485843175 + EqualityComparer<string>.Default.GetHashCode(Host);
        }

        public static bool operator ==(Client left, Client right)
        {
            return EqualityComparer<Client>.Default.Equals(left, right);
        }

        public static bool operator !=(Client left, Client right)
        {
            return !(left == right);
        }
    }
}
