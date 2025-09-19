using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_Models
{
    public class UpdateDefine
    {
        public static String VersionConfigFile = Environment.CurrentDirectory + "\\version.json";

        public static String MainUpdateJsonFileName = "updateAll.json";
        public static String SubUpdateJsonFileName = "update.json";
        /// <summary>
        /// 更新文件下载后 存放目录
        /// </summary>
        public static String UpdateFileDir = "update";
        /// <summary>
        /// 版本文件备份处  用于版本回退使用
        /// </summary>
        public static String UpdateBackupDir = "update_backup";
    }
}
