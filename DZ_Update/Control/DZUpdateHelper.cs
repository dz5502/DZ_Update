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
        private SubUpdateJson _subUpdateInfo = null;

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
            //创建具体版本文件夹
            String lastestDir = Path.Combine(_updateDir, _mainUpdateInfo.LatestVersion);
            if (!Directory.Exists(lastestDir))
                Directory.CreateDirectory(lastestDir);

            String localUpdateFile = Path.Combine(lastestDir, UpdateDefine.SubUpdateJsonFileName);
            HttpFileUtil.DownloadFile(localUpdateFile, _mainUpdateInfo.GetLatestUpdateJsonPath());//updateAll.json
            _subUpdateInfo = JsonConvert.DeserializeObject<SubUpdateJson>(File.ReadAllText(localUpdateFile));

            if (_subUpdateInfo.ForceUpdate)
                return true;

            return false;
        }

        public bool IsNeedUpdate()
        {
            //判断针对版本的更新  
            if (_subUpdateInfo.TargetClientType.ExistData())
            {
                bool needUpdate = false;
                ClientType currentType = VersionTool.GetClientType();
                //存在当前版本 则提示更新
                foreach (var item in _subUpdateInfo.TargetClientType)
                {
                    if (currentType.ToString().Equals(item))
                    {
                        needUpdate = true;
                        break;
                    }
                }

                if (!needUpdate)
                    return false;

            }

            //判断针对用户的更新    没有则不判断
            if (_subUpdateInfo.TargetClientUser.ExistData())
            {
                bool needUpdate = false;
                String userName = VersionTool.GetClientUser();

                //存在当前版本 则提示更新
                foreach (var item in _subUpdateInfo.TargetClientUser)
                {
                    if (item.ToString().Equals(userName))
                    {
                        needUpdate = true;
                        break;
                    }
                }

                if (!needUpdate)
                    return false;

            }

            return true;
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        public void Update()
        {
            String currentClientVersion = VersionTool.GetClientVersion();

            //跨版本更新
            //按版本顺序 执行下面的操作
            int currentVersionIndex = _mainUpdateInfo.VersionList.IndexOf(currentClientVersion);
            if (currentVersionIndex < 0)
            {
                RepairClientAllFile();
                return;
            }

            for (int i = currentVersionIndex + 1; i < _mainUpdateInfo.VersionList.Count; i++)
            {
                String needVersion = _mainUpdateInfo.VersionList[i];
                Console.WriteLine($"更新版本{needVersion}中...");
                DoUpdate(needVersion);
            }

        }
        private void DoUpdate(String needVersion)
        {
            //_subUpdateInfo  需重新生成
            //下载具体的更新文件   判断是否需要更新
            //创建具体版本文件夹
            String lastestDir = Path.Combine(_updateDir, needVersion);
            if (!Directory.Exists(lastestDir))
                Directory.CreateDirectory(lastestDir);

            String localUpdateFile = Path.Combine(lastestDir, UpdateDefine.SubUpdateJsonFileName);
            HttpFileUtil.DownloadFile(localUpdateFile, _mainUpdateInfo.GetSubUpdateJsonPath(needVersion));//updateAll.json
            _subUpdateInfo = JsonConvert.DeserializeObject<SubUpdateJson>(File.ReadAllText(localUpdateFile));



            String currentClientVersion = VersionTool.GetClientVersion();
            //备份文件
            String currentVersionBackupDir = Path.Combine(_updateBackupDir, currentClientVersion);
            if (!Directory.Exists(currentVersionBackupDir))
                Directory.CreateDirectory(currentVersionBackupDir);

            //从更新配置中获取 哪些文件需要备份
            foreach (var item in _subUpdateInfo.FileList)
            {
                //拷贝文件
                //判断是否在二级目录
                if (item.Path.Contains(Path.DirectorySeparatorChar))
                {
                    String dir = Path.Combine(currentVersionBackupDir, Path.GetDirectoryName(item.Path));
                    Directory.CreateDirectory(dir);
                }

                //复制文件
                String sourceFile = Path.Combine(Environment.CurrentDirectory, item.Path);
                String destFile = Path.Combine(currentVersionBackupDir, item.Path);

                if (File.Exists(sourceFile))
                    File.Copy(sourceFile, destFile, true);
            }

            //下载文件   整个压缩包
            String updateVersionDir = Path.Combine(_updateDir, _subUpdateInfo.CurrentVersion);
            if (!Directory.Exists(updateVersionDir))
                Directory.CreateDirectory(updateVersionDir);

            //尝试三次
            bool downloadSuc = false;
            String localZipFile = Path.Combine(updateVersionDir, $"{_subUpdateInfo.CurrentVersion}.zip");
            String remoteZipFile = Path.Combine(_subUpdateInfo.CurrentVersion, $"{_subUpdateInfo.CurrentVersion}.zip");
            for (int i = 0; i < 3; i++)
            {
                HttpFileUtil.DownloadFile(localZipFile, remoteZipFile);
                //解压文件  比对压缩包MD5
                String zipMD5 = CalcMD5Tool.CalcFileMD5(localZipFile);
                RemoteFileInfo zipInfo = _subUpdateInfo.FileList.FirstOrDefault(a => a.FileName.Equals($"{_subUpdateInfo.CurrentVersion}.zip"));
                if (zipInfo != null)
                {
                    String remoteMD5 = zipInfo.MD5;
                    if (remoteMD5.ToLower().Equals(zipMD5.ToLower()))
                    {
                        downloadSuc = true;
                        break;
                    }

                }
            }
            if (downloadSuc == false)
                throw new Exception("更新文件下载失败，请1小时后重试！");

            //解压文件
            ZipTool.UnZip(localZipFile, updateVersionDir);
            //删除zip文件
            File.Delete(localZipFile);
            //覆盖文件
            DirFileOperateTool.CopyDirectory(updateVersionDir, Environment.CurrentDirectory);
            //删除运行目录多余的 update.json
            String jsonFile = Path.Combine(Environment.CurrentDirectory, UpdateDefine.SubUpdateJsonFileName);
            if (File.Exists(jsonFile))
                File.Delete(jsonFile);
            //修改本地版本
            VersionTool.UpdateLocalVersion(_subUpdateInfo.CurrentVersion);
        }
        /// <summary>
        /// 回退到上一版本
        /// </summary>
        public void RevertToPreviousVersion()
        {
            //找到备份文件夹中的最新版本
            var dirArray = Directory.GetDirectories(_updateBackupDir);
            if (!dirArray.ExistData())
                throw new Exception("没有版本备份，无法执行回退操作！");
            //排序
            var dirList = dirArray.ToList();
            dirList.Sort();

            String lastDir = dirList.Last();

            //直接覆盖文件
            DirFileOperateTool.CopyDirectory(lastDir, Environment.CurrentDirectory);
            //修改本地版本
            VersionTool.UpdateLocalVersion(Path.GetFileName(lastDir));
        }
        /// <summary>
        /// 修复本地文件
        /// </summary>
        public void RepairClientAllFile()
        {
            //文件名-文件路径
            List<RemoteFileInfo> needDownloadFileList = new List<RemoteFileInfo>();

            //获取所有文件 计算 MD5
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                String MD5 = CalcMD5Tool.CalcFileMD5(file);

                //从服务器获取MD5
                RemoteFileInfo fileInfo = _mainUpdateInfo.FileList.FirstOrDefault(a => a.FileName.Equals(Path.GetFileName(file)));
                if (fileInfo != null)
                {
                    //判断MD5是否一致
                    if (!MD5.ToLower().Equals(fileInfo.MD5.ToLower()))
                    {
                        needDownloadFileList.Add(fileInfo);
                    }
                }
            }
            //找到本地文件没有  但是远程有的文件
            foreach (var item in _mainUpdateInfo.FileList)
            {
                //压缩包跳过
                if (item.FileName.Contains("zip"))
                    continue;

                String file = files.FirstOrDefault(a => Path.GetFileName(a).Equals(item.FileName));
                if (String.IsNullOrEmpty(file))
                {
                    needDownloadFileList.Add(item);
                }
            }

            //下载文件

            if (!needDownloadFileList.ExistData())
                return;

            String allUpdateDir = Path.Combine(_updateDir, "allUpdate");
            if (Directory.Exists(allUpdateDir))
                Directory.Delete(allUpdateDir, true);

            Directory.CreateDirectory(allUpdateDir);

            foreach (var item in needDownloadFileList)
            {
                String localFile = Path.Combine(allUpdateDir, item.Path);
                String remoteFile = item.GetRemoteFilePath();

                HttpFileUtil.DownloadFile(localFile, remoteFile);
                //计算MD5是否一致
                String md5 = CalcMD5Tool.CalcFileMD5(localFile);
                if (!md5.Equals(item.MD5))
                {
                    //不相等  重下一次
                    HttpFileUtil.DownloadFile(localFile, remoteFile);
                }
            }

            //覆盖文件
            DirFileOperateTool.CopyDirectory(allUpdateDir, Environment.CurrentDirectory);
            Directory.Delete(allUpdateDir, true);
            //修改本地版本
            VersionTool.UpdateLocalVersion(_mainUpdateInfo.LatestVersion);
        }
    }

}
