using RemoteJar.Model;
using RemoteJar.QTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RemoteJar
{
    static class Program
    {
        public static Logger Log { get; private set; } = new Logger();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmRemoteJarMain());
        }
    }
}
