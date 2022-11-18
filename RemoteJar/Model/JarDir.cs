using System;
using System.Collections.Generic;

namespace RemoteJar.Model
{
    public class JarDir : IEquatable<JarDir>
    {
        public Client Client { get; set; }
        public string ProcName { get; set; }
        public string JarId { get; set; }
        public string ServerJarPath { get; set; }
        public string CrontabPath { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JarDir);
        }

        public bool Equals(JarDir other)
        {
            return other != null &&
                   EqualityComparer<Client>.Default.Equals(Client, other.Client) &&
                   ProcName == other.ProcName &&
                   JarId == other.JarId &&
                   ServerJarPath == other.ServerJarPath &&
                   CrontabPath == other.CrontabPath;
        }

        public override int GetHashCode()
        {
            var hashCode = 332277370;
            hashCode = hashCode * -1521134295 + EqualityComparer<Client>.Default.GetHashCode(Client);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProcName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(JarId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ServerJarPath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CrontabPath);
            return hashCode;
        }

        public static bool operator ==(JarDir left, JarDir right)
        {
            return EqualityComparer<JarDir>.Default.Equals(left, right);
        }

        public static bool operator !=(JarDir left, JarDir right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{Client.Host} : {ServerJarPath}";
        }
    }
}
