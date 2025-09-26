using System;
using System.IO;
using System.Linq;
using System.Text;
using DZ_Update_CommonTools;
using DZ_Update_Models;
using Newtonsoft.Json;
using RestSharp;

namespace DZ_Update.Control
{
    public class HttpFileUtil
    {
        public static Rep_GetDirList GetDirList(String dir)
        {
            var result = HttpTool.ClientConnect(VersionTool.GetHttpServer() + $"/{dir}" + "?json", Method.GET, null, GetToken());
            if ((result != null) && (!result.IsSuccessful))
                throw new Exception(result.Content + result.ErrorMessage);

            Rep_GetDirList re = JsonConvert.DeserializeObject<Rep_GetDirList>(result.Content);
            return re;
        }

        public static bool DownloadFile(String localFile, String remoteFile)
        {
            Rep_GetDirList fileInfos = HttpFileUtil.GetDirList(Path.GetDirectoryName(remoteFile));
            //找到对应文件大小
            Int64 fileSize = fileInfos.paths.FirstOrDefault(a => a.name.Equals(Path.GetFileName(remoteFile))).size.Value;
            //存到本地
            //判断文件夹是否存在
            String dir = Path.GetDirectoryName(localFile);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            Action<Int64> progressAction = (currentSize) =>
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"文件{Path.GetFileName(localFile)}下载进度 {(currentSize * 100.0 / fileSize).ToString("F2")}%");
            };

            try
            {
                HttpTool.DownloadFile(VersionTool.GetHttpServer() + $"/{remoteFile}", localFile, progressAction, GetToken());
            }
            catch (Exception ex)
            {
                throw new Exception($"从服务器下载文件失败：{ex.Message}");
            }

            Console.WriteLine();
            return true;
        }


        private static String GetToken()
        {
            String s = $"{VersionTool.GetLocalVersionInfo().UserName}:{VersionTool.GetLocalVersionInfo().Pwd}";
            var bytes = Encoding.UTF8.GetBytes(s);
            String token = Convert.ToBase64String(bytes);
            return token;
        }
    }
}
