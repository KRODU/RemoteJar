using RemoteJar.Model;
using System.Collections.Generic;

namespace RemoteJar.QTask
{
    public class TaskBlockAndWait : QTaskModel
    {
        public IEnumerable<JarDir> TaskDir { get; set; } = new HashSet<JarDir>();
        public bool ForceKillTask { get; set; }
        public TaskJarUpload TaskJarUpload { get; set; }
    }
}
