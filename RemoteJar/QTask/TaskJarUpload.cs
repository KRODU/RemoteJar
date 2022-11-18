using RemoteJar.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteJar.QTask
{
    public class TaskJarUpload : QTaskModel
    {
        public string LocalJarPath { get; set; }
        public JarDir JarDir { get; set; }
        public bool ForceRun { get; set; }
    }
}
