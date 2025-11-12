using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using DZ_Update_CommonTools;
using DZ_Update_Models.update;
using DZ_Update_Models;

namespace DZ_Update.Control
{
    public class DZUpdateHelper
    {
        private String _updateDir = Path.Combine(Environment.CurrentDirectory, UpdateDefine.UpdateFileDir);
        private String _updateBackupDir = Path.Combine(Environment.CurrentDirectory, UpdateDefine.UpdateBackupDir);

        private MainUpdateJson _mainUpdateInfo = null;
        private List<SubUpdateJson> _subUpdateInfoList = new List<SubUpdateJson>();

        public void GetServerInfo()
        {
            //获取服务器最新版本
            String dir = _updateDir;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            String localUpdateAllFile = Path.Combine(dir, UpdateDefine.MainUpdateJsonFileName);

            HttpFileUtil.DownloadFile(localUpdateAllFile, UpdateDefine.MainUpdateJsonFileName);//updateAll.json
            //解析最新版本号
            _mainUpdateInfo = JsonConvert.DeserializeObject<MainUpdateJson>(File.ReadAllText(localUpdateAllFile));
            if (_mainUpdateInfo == null)
                throw new Exception("从服务器获取更新信息失败！");

            //下载具体的更新文件   判断是否需要更新

            //下载所有新的版本信息
            int index = _mainUpdateInfo.VersionList.IndexOf(GetClientVersion());
            for (int i = index; i < _mainUpdateInfo.VersionList.Count; i++)
            {
                String versionStr = _mainUpdateInfo.VersionList[i];
                //创建具体版本文件夹
                String lastestDir = Path.Combine(_updateDir, versionStr);
                if (!Directory.Exists(lastestDir))
                    Directory.CreateDirectory(lastestDir);

                String localUpdateFile = Path.Combine(lastestDir, UpdateDefine.SubUpdateJsonFileName);
                HttpFileUtil.DownloadFile(localUpdateFile, _mainUpdateInfo.GetSubUpdateJsonPath(versionStr));//updateAll.json
                SubUpdateJson subUpdateInfo = JsonConvert.DeserializeObject<SubUpdateJson>(File.ReadAllText(localUpdateFile));
                if (subUpdateInfo == null)
                    throw new Exception("从服务器获取更新信息失败！！");

                _subUpdateInfoList.Add(subUpdateInfo);
            }
        }

        public String GetServerLatestVersion()
        {
            return _mainUpdateInfo.LatestVersion;
        }
        public List<String> GetServerUpdateMsg()
        {
            List<String> re = new List<string>();

            for (int i = 1; i < _subUpdateInfoList.Count; i++)
            {
                re.AddRange(_subUpdateInfoList[i].UpdateInfo);
            }
            return re;
        }
        public String GetClientVersion()
        {
            return VersionTool.GetClientVersion();
        }

        /// <summary>
        /// 是否存在更新
        /// </summary>
        /// <returns></returns>
        public bool IsExistUpdate()
        {
            //获取本地版本
            String currentVersion = VersionTool.GetClientVersion();
            //比对最新版本
            if (!_mainUpdateInfo.LatestVersion.BiggerThanVersion(currentVersion))
            {
                return false;
            }

            return true;
        }

        public bool IsFoceToUpdate()
        {
            //下载具体的更新文件   判断是否需要更新
            if (_subUpdateInfoList.Last().ForceUpdate)
                return true;

            return false;
        }

        public bool IsNeedUpdate(String currentType, String userName)
        {
            if (IsExistUpdate() == false)
                return false;

            //暂不实现对版本及用户的筛选  在跨版本更新时逻辑复杂
            ////判断针对版本的更新  
            //if (_subUpdateInfo.TargetClientType.ExistData())
            //{
            //    bool needUpdate = false;
            //    //存在当前版本 则提示更新
            //    foreach (var item in _subUpdateInfo.TargetClientType)
            //    {
            //        if (currentType.Equals(item))
            //        {
            //            needUpdate = true;
            //            break;
            //        }
            //    }

            //    if (!needUpdate)
            //        return false;

            //}

            ////判断针对用户的更新    没有则不判断
            //if (_subUpdateInfo.TargetClientUser.ExistData())
            //{
            //    bool needUpdate = false;
            //    //存在当前版本 则提示更新
            //    foreach (var item in _subUpdateInfo.TargetClientUser)
            //    {
            //        if (item.ToString().Equals(userName))
            //        {
            //            needUpdate = true;
            //            break;
            //        }
            //    }

            //    if (!needUpdate)
            //        return false;

            //}

            return true;
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        public void Update(Action<int> progressAction)
        {
            String currentClientVersion = VersionTool.GetClientVersion();

            //跨版本更新
            //按版本顺序 执行下面的操作
            int currentVersionIndex = _mainUpdateInfo.VersionList.IndexOf(currentClientVersion);
            if (currentVersionIndex < 0)
            {
                RepairClientAllFile(progressAction);
                return;
            }

            //计算当前更新占的比例   也可以更复杂用所有需要下载文件的大小?
            double oneProgressRatio = 1.0 / (_mainUpdateInfo.VersionList.Count - (currentVersionIndex + 1));
            int versionCount = 0;
            for (int i = currentVersionIndex + 1; i < _mainUpdateInfo.VersionList.Count; i++)
            {
                String needVersion = _mainUpdateInfo.VersionList[i];
                Console.Write(Environment.NewLine + $"更新版本{needVersion}中...");
                DoUpdate(needVersion, a =>
                {
                    int currentProgress = Convert.ToInt32((a * oneProgressRatio) + (oneProgressRatio * versionCount * 100));
                    //Console.WriteLine(aa);
                    progressAction?.Invoke(currentProgress);
                });

                versionCount++;
                Console.WriteLine();
            }

        }
        /// <summary>
        /// 返回 进度100比例
        /// </summary>
        /// <param name="needVersion"></param>
        /// <param name="progressAction"></param>
        private void DoUpdate(String needVersion, Action<int> progressAction)
        {
            //_subUpdateInfo  需重新生成
            //下载具体的更新文件   判断是否需要更新
            //创建具体版本文件夹
            String lastestDir = Path.Combine(_updateDir, needVersion);
            if (!Directory.Exists(lastestDir))
                Directory.CreateDirectory(lastestDir);

            String localUpdateFile = Path.Combine(lastestDir, UpdateDefine.SubUpdateJsonFileName);
            HttpFileUtil.DownloadFile(localUpdateFile, _mainUpdateInfo.GetSubUpdateJsonPath(needVersion));//updateAll.json
            SubUpdateJson subUpdateInfo = JsonConvert.DeserializeObject<SubUpdateJson>(File.ReadAllText(localUpdateFile));

            progressAction?.Invoke(2);

            String currentClientVersion = VersionTool.GetClientVersion();
            //备份文件
            String currentVersionBackupDir = Path.Combine(_updateBackupDir, currentClientVersion);
            if (!Directory.Exists(currentVersionBackupDir))
                Directory.CreateDirectory(currentVersionBackupDir);

            //从更新配置中获取 哪些文件需要备份
            int fileCount = 0;
            foreach (var item in subUpdateInfo.FileList)
            {
                String sourceFile = Path.Combine(Environment.CurrentDirectory, item.Path);
                if (!File.Exists(sourceFile))
                    continue;

                //拷贝文件
                //判断是否在二级目录
                if (item.Path.Contains(Path.DirectorySeparatorChar))
                {
                    String dir = Path.Combine(currentVersionBackupDir, Path.GetDirectoryName(item.Path));
                    Directory.CreateDirectory(dir);
                }

                //复制文件
                String destFile = Path.Combine(currentVersionBackupDir, item.Path);
                File.Copy(sourceFile, destFile, true);

                //计算比例
                fileCount++;
                progressAction?.Invoke(Convert.ToInt32((fileCount * 1.0 / subUpdateInfo.FileList.Count * 100 * (10 / 100.0))));
            }

            progressAction?.Invoke(10);

            //下载文件   整个压缩包
            String updateVersionDir = Path.Combine(_updateDir, subUpdateInfo.CurrentVersion);
            if (!Directory.Exists(updateVersionDir))
                Directory.CreateDirectory(updateVersionDir);

            //尝试三次
            bool downloadSuc = false;

            String localZipFile = Path.Combine(updateVersionDir, $"{subUpdateInfo.CurrentVersion}.zip");
            String remoteZipFile = Path.Combine(subUpdateInfo.CurrentVersion, $"{subUpdateInfo.CurrentVersion}.zip");

            HttpFileUtil.DownloadFile(localZipFile, remoteZipFile, (ratio) =>
            {
                progressAction?.Invoke(10 + Convert.ToInt32(ratio * (87 / 100.0)));
            });

            //解压文件  比对压缩包MD5
            String zipMD5 = CalcMD5Tool.CalcFileMD5(localZipFile);
            RemoteFileInfo zipInfo = subUpdateInfo.FileList.FirstOrDefault(a => a.FileName.Equals($"{subUpdateInfo.CurrentVersion}.zip"));
            if (zipInfo != null)
            {
                String remoteMD5 = zipInfo.MD5;
                if (remoteMD5.ToLower().Equals(zipMD5.ToLower()))
                {
                    downloadSuc = true;
                }
            }

            if (downloadSuc == false)
                throw new Exception("更新文件下载失败，请重试！");

            progressAction?.Invoke(98);
            //解压文件
            ZipTool.UnZip(localZipFile, updateVersionDir);
            //删除zip文件
            File.Delete(localZipFile);
            this.SelfUpdate(updateVersionDir);

            progressAction?.Invoke(99);
            //覆盖文件
            DirFileOperateTool.CopyDirectory(updateVersionDir, Environment.CurrentDirectory);
            //删除运行目录多余的 update.json
            String jsonFile = Path.Combine(Environment.CurrentDirectory, UpdateDefine.SubUpdateJsonFileName);
            if (File.Exists(jsonFile))
                File.Delete(jsonFile);
            //修改本地版本
            VersionTool.UpdateLocalVersion(subUpdateInfo.CurrentVersion);
            progressAction?.Invoke(100);

        }
        /// <summary>
        /// 回退到上一版本
        /// </summary>
        public void RevertToPreviousVersion(Action<int> progressAction)
        {
            //找到备份文件夹中的最新版本
            if (!Directory.Exists(_updateBackupDir))
                throw new Exception("没有版本备份，无法执行回退操作！");

            var dirArray = Directory.GetDirectories(_updateBackupDir);
            if (!dirArray.ExistData())
                throw new Exception("没有版本备份，无法执行回退操作！");

            progressAction?.Invoke(5);
            //删除更新文件  再用旧文件覆盖
            //找到最新版本更新的文件列表
            foreach (var item in _subUpdateInfoList.Last().FileList)
            {
                String localFile = Path.Combine(Environment.CurrentDirectory, item.Path);
                if (File.Exists(localFile))
                    File.Delete(localFile);
            }
            progressAction?.Invoke(10);

            //排序
            var dirList = dirArray.ToList();
            dirList.Sort();

            String lastDir = dirList.Last();

            //直接覆盖文件
            int totalFileCount = Directory.GetFiles(lastDir, "*", SearchOption.AllDirectories).Length;
            int copyFileCount = 0;

            this.SelfUpdate(lastDir);//自更新判断
            DirFileOperateTool.CopyDirectory(lastDir, Environment.CurrentDirectory, a =>
            {
                //计算 比例
                copyFileCount += a;
                progressAction?.Invoke(Convert.ToInt32((copyFileCount * 1.0 / totalFileCount) * 89));
            });

            progressAction?.Invoke(99);
            //修改本地版本
            VersionTool.UpdateLocalVersion(Path.GetFileName(lastDir));
            progressAction?.Invoke(100);

        }
        /// <summary>
        /// 修复本地文件
        /// </summary>
        public int RepairClientAllFile(Action<int> progressAction)
        {
            //文件名-文件路径
            List<RemoteFileInfo> needDownloadFileList = new List<RemoteFileInfo>();

            //直接以发布json为主
            foreach (var item in _mainUpdateInfo.FileList)
            {
                //压缩包跳过
                if (item.FileName.Contains("zip"))
                    continue;

                String localFile = Path.Combine(Environment.CurrentDirectory, item.Path);
                if (!File.Exists(localFile))
                {
                    needDownloadFileList.Add(item);
                    continue;
                }

                //比较md5
                String md5 = CalcMD5Tool.CalcFileMD5(localFile);
                //判断MD5是否一致
                if (!md5.ToLower().Equals(item.MD5.ToLower()))
                {
                    needDownloadFileList.Add(item);
                }
            }

            //下载文件
            if (!needDownloadFileList.ExistData())
            {
                progressAction?.Invoke(100);
                return 0;
            }

            String allUpdateDir = Path.Combine(_updateDir, "allUpdate");
            if (Directory.Exists(allUpdateDir))
                Directory.Delete(allUpdateDir, true);

            Directory.CreateDirectory(allUpdateDir);

            progressAction?.Invoke(10);

            double progressRatio = 0.89 / needDownloadFileList.Count;
            for (int i = 0; i < needDownloadFileList.Count; i++)
            {
                var item = needDownloadFileList[i];
                String localFile = Path.Combine(allUpdateDir, item.Path);
                String remoteFile = item.GetRemoteFilePath();

                HttpFileUtil.DownloadFile(localFile, remoteFile, a =>
                {
                    int aa = Convert.ToInt32(progressRatio * a + (progressRatio * i * 100));
                    Console.WriteLine(aa);
                    progressAction?.Invoke(aa);
                });
                //计算MD5是否一致
                String md5 = CalcMD5Tool.CalcFileMD5(localFile);
                if (!md5.Equals(item.MD5))
                {
                    //不相等
                    throw new Exception("文件下载失败，请重试！");
                }
            }

            progressAction?.Invoke(99);
            //覆盖文件
            DirFileOperateTool.CopyDirectory(allUpdateDir, Environment.CurrentDirectory);
            Directory.Delete(allUpdateDir, true);
            //修改本地版本
            VersionTool.UpdateLocalVersion(_mainUpdateInfo.LatestVersion);
            progressAction?.Invoke(100);
            return needDownloadFileList.Count;
        }

        /// <summary>
        /// 自更新
        /// </summary>
        private void SelfUpdate(String updateDir)
        {
            //判断更新文件中 是否包含本程序
            List<String> filterFileList = new List<String>() { "DZ_Update.exe", "DZ_Update.Models.dll", "DZ_Update.CommonTools.dll" };

            var files = Directory.GetFiles(updateDir, "*", SearchOption.AllDirectories);

            foreach (String file in filterFileList)
            {
                String needUpdateFile = files.FirstOrDefault(f => Path.GetFileName(f).Equals(file));
                if (!String.IsNullOrEmpty(needUpdateFile))
                {
                    //将运行目录的程序改名
                    String file_source = Path.Combine(Environment.CurrentDirectory, file);
                    String file_back = Path.Combine(Environment.CurrentDirectory, file + ".back");
                    if(File.Exists(file_back))
                        File.Delete(file_back);

                    File.Move(file_source, file_back);
                }
            }

            //继续执行其他正常复制即可
            return;
        }
    }

}
