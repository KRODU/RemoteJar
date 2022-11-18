using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RemoteJar.Model
{
    public class JarProc : IEquatable<JarProc>
    {
        public string Host { get; set; }
        public string PID { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JarProc);
        }

        public bool Equals(JarProc other)
        {
            return other != null &&
                   Host == other.Host &&
                   Name == other.Name;
        }

        public static bool operator ==(JarProc left, JarProc right)
        {
            return EqualityComparer<JarProc>.Default.Equals(left, right);
        }

        public static bool operator !=(JarProc left, JarProc right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            var hashCode = 1752210925;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Host);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
