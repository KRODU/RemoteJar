using RemoteJar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace RemoteJar
{
    public class Config
    {
        public Dictionary<string, HashSet<JarDir>> JarIdDic { get; private set; } = new Dictionary<string, HashSet<JarDir>>();
        public Dictionary<Client, HashSet<JarDir>> ClientDic { get; private set; } = new Dictionary<Client, HashSet<JarDir>>();

        public void Load(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            foreach (XmlNode clientNode in xmlDoc.SelectNodes("/remoteJar/clientList/client"))
            {
                var attr = clientNode.Attributes;
                Client client = new Client(attr["name"]?.Value?.TrimChars(), attr["host"]?.Value.TrimChars(), attr["user"]?.Value.TrimChars(), attr["pwd"]?.Value.TrimChars());

                if (string.IsNullOrWhiteSpace(client.Name))
                    throw new XmlConfigValidationException($"일부 client의 name 속성이 없습니다.");

                if (string.IsNullOrWhiteSpace(client.Host))
                    throw new XmlConfigValidationException($"일부 client의 host 속성이 없습니다.");

                if (string.IsNullOrWhiteSpace(client.User))
                    throw new XmlConfigValidationException($"일부 client의 user 속성이 없습니다.");

                if (string.IsNullOrWhiteSpace(client.Pwd))
                    throw new XmlConfigValidationException($"일부 client의 pwd 속성이 없습니다.");

                if (ClientDic.Keys.Contains(client))
                    throw new XmlConfigValidationException($"같은 HOST에 대하여 하나의 XML 설정만 허용됩니다: {client.Host}");

                foreach (XmlNode jarDirNode in clientNode.SelectNodes("./jarDir"))
                {
                    attr = jarDirNode.Attributes;
                    string jarId = attr["jarId"]?.Value?.TrimChars();
                    JarDir jarDir = new JarDir()
                    {
                        Client = client,
                        JarId = jarId,
                        ProcName = attr["procName"]?.Value?.TrimChars(),
                        ServerJarPath = attr["jarPath"]?.Value?.TrimChars(),
                        CrontabPath = attr["crontabPath"]?.Value?.TrimChars()
                    };

                    if (string.IsNullOrWhiteSpace(jarDir.JarId))
                        throw new XmlConfigValidationException($"{client.Host}의 일부 jarDir에서 jarId 속성이 없습니다.");

                    if (string.IsNullOrWhiteSpace(jarDir.ProcName))
                        throw new XmlConfigValidationException($"{client.Host}의 일부 jarDir에서 procName 속성이 없습니다.");

                    if (string.IsNullOrWhiteSpace(jarDir.ServerJarPath))
                        throw new XmlConfigValidationException($"{client.Host}의 일부 jarDir에서 jarPath 속성이 없습니다.");

                    if (string.IsNullOrWhiteSpace(jarDir.CrontabPath))
                        throw new XmlConfigValidationException($"{client.Host}의 일부 jarDir에서 crontabPath 속성이 없습니다.");

                    // jarIdDic에 넣음
                    if (JarIdDic.TryGetValue(jarId, out HashSet<JarDir> jarDirSet))
                    {
                        jarDirSet.Add(jarDir);
                    }
                    else
                    {
                        jarDirSet = new HashSet<JarDir>
                        {
                            jarDir
                        };
                        JarIdDic.Add(jarId, jarDirSet);
                    }

                    // clientDic에 넣음
                    if (ClientDic.TryGetValue(client, out jarDirSet))
                    {
                        if ((from dir in jarDirSet where dir.ProcName == jarDir.ProcName select dir).Count() > 0)
                            throw new XmlConfigValidationException($"같은 HOST 내에서 중복되는 procName 설정이 존재합니다: {jarDir.Client.Host}, {jarDir.ProcName}");

                        if ((from dir in jarDirSet where dir.ServerJarPath == jarDir.ServerJarPath select dir).Count() > 0)
                            throw new XmlConfigValidationException($"같은 HOST 내에서 중복되는 jarPath 설정이 존재합니다: {jarDir.Client.Host}, {jarDir.ServerJarPath}");

                        if ((from dir in jarDirSet where dir.CrontabPath == jarDir.CrontabPath select dir).Count() > 0)
                            throw new XmlConfigValidationException($"같은 HOST 내에서 중복되는 crontabPath 설정이 존재합니다: {jarDir.Client.Host}, {jarDir.CrontabPath}");

                        jarDirSet.Add(jarDir);
                    }
                    else
                    {
                        jarDirSet = new HashSet<JarDir>
                        {
                            jarDir
                        };
                        ClientDic.Add(client, jarDirSet);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Xml Config 검증 오류입니다.
    /// </summary>
    public class XmlConfigValidationException : Exception
    {
        public XmlConfigValidationException() { }

        public XmlConfigValidationException(string errMsg) : base(errMsg) { }
    }
}
