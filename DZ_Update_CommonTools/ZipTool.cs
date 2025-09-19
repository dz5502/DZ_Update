using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace DZ_Update_CommonTools
{
    public class ZipTool
    {

        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <returns>解压是否成功</returns>  
        public static bool UnZip(string zipFilePath, string unZipDir)
        {
            try
            {
                if (zipFilePath == string.Empty)
                {
                    throw new Exception("压缩文件不能为空！");
                }
                if (!File.Exists(zipFilePath))
                {
                    throw new FileNotFoundException("压缩文件不存在！");
                }
                //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
                if (unZipDir == string.Empty)
                    unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
                if (!unZipDir.EndsWith("/"))
                    unZipDir += "/";
                if (!Directory.Exists(unZipDir))
                    Directory.CreateDirectory(unZipDir);
                using (var s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (!string.IsNullOrEmpty(directoryName))
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (directoryName != null && !directoryName.EndsWith("/"))
                        {
                        }
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {

                                int size;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool UnZip(string zipFile, string unZipDir, string password)
        {
            using (FileStream fileStreamIn = new FileStream(zipFile, FileMode.Open, FileAccess.Read))
            {
                using (ZipInputStream zipInStream = new ZipInputStream(fileStreamIn))
                {
                    zipInStream.Password = password;
                    ZipEntry entry = zipInStream.GetNextEntry();
                    do
                    {
                        using (FileStream fileStreamOut = new FileStream(Path.Combine(unZipDir, entry.Name), FileMode.Create, FileAccess.Write))
                        {

                            int size = 2048;
                            byte[] buffer = new byte[2048];
                            do
                            {
                                size = zipInStream.Read(buffer, 0, buffer.Length);
                                fileStreamOut.Write(buffer, 0, size);
                            } while (size > 0);
                        }
                    } while ((entry = zipInStream.GetNextEntry()) != null);
                }
            }

            return true;
        }

        /// <summary>
        /// 压缩所有的文件
        /// </summary>
        /// <param name="filesPath"></param>
        /// <param name="zipFilePath"></param>
        public static void CreateZipFile(String[] filenames, string zipFile)
        {
            byte[] buffer = new byte[1024 * 1024]; //缓冲区大小

            FileStream fss =  File.Create(zipFile);
            ZipOutputStream stream = new ZipOutputStream(fss);
            stream.SetLevel(9); // 压缩级别 0-9
            foreach (string file in filenames)
            {
                FileInfo fileInfo = new FileInfo(file);
                ZipEntry entry = new ZipEntry(Path.GetFileName(file)); //压缩包中展示的文件名  Path.GetFileName(file)
                entry.DateTime = fileInfo.LastWriteTime;  //影响md5？
                stream.PutNextEntry(entry);
                using (FileStream fs = File.Open(file, FileMode.Open))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);

                    fs.Close();
                }
            }
            stream.Finish();

            fss.Close();
            stream.Close();

        }

        public static void CreateZipDir(String rootDir, string zipFile)
        {
            var files = Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories);
            byte[] buffer = new byte[1024 * 1024]; //缓冲区大小

            FileStream fss = File.Create(zipFile);
            ZipOutputStream stream = new ZipOutputStream(fss);
            stream.SetLevel(9); // 压缩级别 0-9
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                String entryName = file.Replace(rootDir, "").Trim(Path.DirectorySeparatorChar);
                ZipEntry entry = new ZipEntry(entryName); //压缩包中展示的文件名  Path.GetFileName(file)
                entry.DateTime = fileInfo.LastWriteTime;  //影响md5？
                stream.PutNextEntry(entry);
                using (FileStream fs = File.Open(file, FileMode.Open))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);

                    fs.Close();
                }
            }
            stream.Finish();

            fss.Close();
            stream.Close();

        }
    }
}
