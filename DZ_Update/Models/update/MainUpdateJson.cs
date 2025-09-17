using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update.Models.update
{
    public class MainUpdateJson
    {
        public String LatestVersion { get; set; }
        /// <summary>
        /// 用于判断是否跨版本更新
        /// </summary>
        public List<String> VersionList { get; set; }

        public List<RemoteFileInfo> FileList { get; set; }

        /// <summary>
        /// 暂时停用
        /// </summary>
        /// <returns></returns>
        private String GetUpdateAllJsonPath()
        {
            return UpdateDefine.MainUpdateJsonFileName;
        }
        public String GetLatestUpdateJsonPath()
        {
            return $"{LatestVersion}/" + UpdateDefine.SubUpdateJsonFileName;
        }

        public String GetSubUpdateJsonPath(String version)
        {
            return $"{version}/" + UpdateDefine.SubUpdateJsonFileName;
        }
    }

    public class RemoteFileInfo
    {
        /// <summary>
        /// 文件名  仅名字
        /// </summary>
        public String FileName { get; set; }
       /// <summary>
       /// 最新版本号
       /// </summary>
        public String Version { get; set; }
        public String MD5 { get; set; }
        /// <summary>
        /// 当前文件最新地址   Version / Path  为实际路径
        /// </summary>
        public String Path { get; set; }


        public String GetRemoteFilePath()
        {
            return $"{Version}/{Path}";
        }
    }
}
