using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RemoteJar
{
    public static class Util
    {
        public static readonly char[] trimChars = new char[] { ' ', '\r', '\n' };

        public static string TrimChars(this string str)
        {
            if (str == null)
                return null;

            return str.Trim(trimChars);
        }

        public static string Combine(this IEnumerable<string> str, string paddingStr, string splitStr)
        {
            if (str == null)
                return string.Empty;

            if (paddingStr == null)
                paddingStr = string.Empty;

            if (splitStr == null)
                splitStr = string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (string eachStr in str)
            {
                sb.Append(paddingStr);
                sb.Append(eachStr);
                sb.Append(paddingStr);
                sb.Append(splitStr);
            }

            if (sb.Length > 0)
            {
                sb.Length -= splitStr.Length;
            }

            return sb.ToString();
        }
    }
}
