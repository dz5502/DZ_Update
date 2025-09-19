using System;
using Newtonsoft.Json;
using System.IO;
using DZ_Update_Models;
using DZ_Update_Models.update;

namespace DZ_Update.Control
{
    public static class VersionTool
    {
        private static LocalUpdateConfig _localUpdateConfig = null;
        static VersionTool()
        {
            try
            {
                String str = File.ReadAllText(UpdateDefine.VersionConfigFile);
                _localUpdateConfig = JsonConvert.DeserializeObject<LocalUpdateConfig>(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine("本地版本文件读取失败：" + ex.Message);
            }
        }
        public static void SaveVersionInfo()
        {
            File.WriteAllText(UpdateDefine.VersionConfigFile, JsonConvert.SerializeObject(_localUpdateConfig));
        }

        public static void UpdateLocalVersion(String version)
        {
            _localUpdateConfig.Version = version;
            SaveVersionInfo();
        }

        public static bool BiggerThanVersion(this String str1, String str2)
        {
            var str1s = str1.Split('.');
            var str2s = str2.Split('.');

            for (int i = 0; i < str1s.Length; i++)
            {
                int val1 = int.Parse(str1s[i]);
                int val2 = int.Parse(str2s[i]);
                if (val1 > val2)
                    return true;
            }

            return false;
        }

        public static ClientType GetClientType()
        {
            //从ini读取
            if (String.IsNullOrEmpty(_localUpdateConfig.ClientType))
                return ClientType.one;

            return (ClientType)Enum.Parse(typeof(ClientType), _localUpdateConfig.ClientType);
        }

        public static LocalUpdateConfig GetLocalVersionInfo()
        {
            return _localUpdateConfig;
        }

        public static String GetClientUser()
        {
            return _localUpdateConfig.UserName;
        }

        public static String GetClientVersion()
        {
            return _localUpdateConfig.Version;
        }

        public static String GetHttpServer()
        {
            return _localUpdateConfig.HttpServer;
        }
    }

}
