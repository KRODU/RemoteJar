using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RemoteJar
{
    public class Logger : IDisposable
    {
        private StreamWriter writer;
        public event Action<string> NewLogEvent;

        public void OpenLogFile()
        {
            try
            {
                writer?.Close();
                writer = new StreamWriter("remoteJar.log", false, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                WriteLine($"remoteJar.log 파일을 여는 중 오류 발생{Environment.NewLine}{ex.Message}");
            }
        }

        public void WriteLine(string str)
        {
            string logMsg = $"[{DateTime.Now.ToString("yy.MM.dd-HH:mm:ss")}]{str}";
            Debug.WriteLine(logMsg);
            writer?.WriteLine(logMsg);
            NewLogEvent?.Invoke(logMsg + Environment.NewLine);
        }

        public void Flush()
        {
            writer?.Flush();
        }

        public void Dispose()
        {
            writer?.Close();
            GC.SuppressFinalize(this);
        }

        ~Logger()
        {
            Dispose();
        }
    }
}
