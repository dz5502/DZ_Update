using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using DZ_Update_CommonTools;
using DZ_Update_Models;
using DZ_Update_Models.update;
using DZ_Update_ServerFileManager.Base;
using DZ_Update_ServerFileManager.Models;
using Newtonsoft.Json;

namespace DZ_Update_ServerFileManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IBaseViewModel
    {
        private MainUpdateJson _mainUpdateJson = null;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;


            this.userTable.SetTableData(UserTableData);
            this.clientTypeTable.SetTableData(ClientTypeTableData);
            this.updateInfoTable.SetTableData(UpdateInfoData);            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string p = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }


        private String _httpFileManagerDir;
        public String HttpFileManagerDir
        {
            get { return this._httpFileManagerDir; }
            set { this._httpFileManagerDir = value; NotifyPropertyChanged(); }
        }

        private String _sourceFileDir;
        public String SourceFileDir
        {
            get { return this._sourceFileDir; }
            set { this._sourceFileDir = value; NotifyPropertyChanged(); }
        }

        private String _versionA;
        public String VersionA
        {
            get { return this._versionA; }
            set { this._versionA = value; NotifyPropertyChanged(); }
        }
        private String _versionB;
        public String VersionB
        {
            get { return this._versionB; }
            set { this._versionB = value; NotifyPropertyChanged(); }
        }
        private String _versionC;
        public String VersionC
        {
            get { return this._versionC; }
            set { this._versionC = value; NotifyPropertyChanged(); }
        }
        private String _versionD;
        public String VersionD
        {
            get { return this._versionD; }
            set { this._versionD = value; NotifyPropertyChanged(); }
        }

        private bool _enableGeneratePanel;
        public bool EnableGeneratePanel
        {
            get { return this._enableGeneratePanel; }
            set { this._enableGeneratePanel = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<UserTableDataItem> _userTableData = new ObservableCollection<UserTableDataItem>();
        public ObservableCollection<UserTableDataItem> UserTableData
        {
            get { return this._userTableData; }
        }

        private ObservableCollection<ClientTypeTableDataItem> _clientTypeTableData = new ObservableCollection<ClientTypeTableDataItem>();
        public ObservableCollection<ClientTypeTableDataItem> ClientTypeTableData
        {
            get { return this._clientTypeTableData; }
        }


        private ObservableCollection<UpdateInfoTableDataItem> _updateInfoData = new ObservableCollection<UpdateInfoTableDataItem>();
        public ObservableCollection<UpdateInfoTableDataItem> UpdateInfoData
        {
            get { return this._updateInfoData; }
        }


        private bool _foceToUpdate;
        public bool FoceToUpdate
        {
            get { return this._foceToUpdate; }
            set { this._foceToUpdate = value; NotifyPropertyChanged(); }
        }

        public BaseCommand OpenHttpFileDirCommand { get { return new BaseCommand(OpenHttpFileDir); } }
        public BaseCommand OpenSourceFileDirCommand { get { return new BaseCommand(OpenSourceFileDir); } }
        public BaseCommand GenerateUpdateFileCommand { get { return new BaseCommand(GenerateUpdateFile); } }

        private void GenerateUpdateFile(object obj)
        {
            try
            {
                //判断版本号是否正确
                if (String.IsNullOrEmpty(VersionA) || String.IsNullOrEmpty(VersionB) || String.IsNullOrEmpty(VersionC) ||
                    String.IsNullOrEmpty(VersionD))
                {
                    throw new Exception("请出入正确的版本号");
                }

                if (!Directory.Exists(HttpFileManagerDir))
                    throw new Exception("请先选择正确的需要更新的文件存放目录");
                if (!Directory.Exists(SourceFileDir))
                    throw new Exception("请先选择正确的需要更新的文件原始目录");


                //判断 updateAll 是否存在  不存在则自动生成
                String mainUpdateJsonFile = System.IO.Path.Combine(HttpFileManagerDir, UpdateDefine.MainUpdateJsonFileName);
                //一般执行这步在项目第一次创建  
                if (!File.Exists(mainUpdateJsonFile))
                {
                    _mainUpdateJson = new MainUpdateJson();
                }
                else
                {
                    _mainUpdateJson = JsonConvert.DeserializeObject<MainUpdateJson>(File.ReadAllText(mainUpdateJsonFile));
                }

                String versionDirName = $"{VersionA}.{VersionB}.{VersionC}.{VersionD}";
                //判断是否已经存在
                if (_mainUpdateJson.VersionList.Count(a => a.Equals(versionDirName)) > 0)
                {
                    throw new Exception($"当前版本 {versionDirName} 已存在！");
                }

                SubUpdateJson subUpdateJson = new SubUpdateJson();
                //从选择的源文件夹 生成
                var files = Directory.GetFiles(SourceFileDir, "*", SearchOption.AllDirectories);
                _mainUpdateJson.LatestVersion = versionDirName;
                _mainUpdateJson.VersionList.Add(_mainUpdateJson.LatestVersion);

                foreach (var file in files)
                {
                    //可能存在不同目录  相同文件名的情况
                    String relativePath = file.Replace(SourceFileDir, "").Trim(Path.DirectorySeparatorChar);
                    RemoteFileInfo remoteFileInfo = _mainUpdateJson.FileList.FirstOrDefault(a=>
                    {
                        return a.FileName.Equals(System.IO.Path.GetFileName(file)) && a.Path.Equals(relativePath);
                    });
                    if (remoteFileInfo == null)
                    {
                        remoteFileInfo = new RemoteFileInfo();
                        _mainUpdateJson.FileList.Add(remoteFileInfo);
                    }

                    remoteFileInfo.FileName = System.IO.Path.GetFileName(file);
                    remoteFileInfo.Version = _mainUpdateJson.LatestVersion;
                    remoteFileInfo.MD5 = CalcMD5Tool.CalcFileMD5(file);
                    remoteFileInfo.Path = relativePath;

                    subUpdateJson.FileList.Add(remoteFileInfo);

                }

                File.WriteAllText(mainUpdateJsonFile, JsonConvert.SerializeObject(_mainUpdateJson));
                //生成具体的更新文件
                //创建文件夹
                String versionDir = System.IO.Path.Combine(HttpFileManagerDir, versionDirName);
                if (Directory.Exists(versionDir))
                    Directory.Delete(versionDir);

                Directory.CreateDirectory(versionDir);

                subUpdateJson.CurrentVersion = _mainUpdateJson.LatestVersion;
                subUpdateJson.ForceUpdate = this.FoceToUpdate;

                foreach (var item in UserTableData)
                {
                    if (!String.IsNullOrEmpty(item.UserName))
                    {
                        subUpdateJson.TargetClientUser.Add(item.UserName);

                    }
                }
                foreach (var item in ClientTypeTableData)
                {
                    if (!String.IsNullOrEmpty(item.ClientType))
                    {
                        subUpdateJson.TargetClientType.Add(item.ClientType);

                    }
                }
                foreach (var item in UpdateInfoData)
                {
                    if (!String.IsNullOrEmpty(item.Info))
                    {
                        subUpdateJson.UpdateInfo.Add(item.Info);

                    }
                }

                //拷贝其他文件  到更讯目录
                DirFileOperateTool.CopyDirectory(SourceFileDir, versionDir);
                //生成压缩包
                var zipFiles = Directory.GetFiles(versionDir, "*", SearchOption.AllDirectories);
                String zipFile = Path.Combine(versionDir, $"{versionDirName}.zip");
                ZipTool.CreateZipDir(versionDir, zipFile);

                RemoteFileInfo zipFileInfo = new RemoteFileInfo();
                zipFileInfo.FileName = System.IO.Path.GetFileName(zipFile);
                zipFileInfo.Version = _mainUpdateJson.LatestVersion;
                zipFileInfo.MD5 = CalcMD5Tool.CalcFileMD5(zipFile);
                zipFileInfo.Path = zipFile.Replace(versionDir, "").Trim(Path.DirectorySeparatorChar);
                subUpdateJson.FileList.Add(zipFileInfo);

                String subUpdateJsonFile = Path.Combine(versionDir, UpdateDefine.SubUpdateJsonFileName);
                File.WriteAllText(subUpdateJsonFile, JsonConvert.SerializeObject(subUpdateJson, Formatting.Indented));

                MessageBox.Show("成功生成版本文件！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenSourceFileDir(object obj)
        {
            try
            {
                String dir = String.Empty;
                if (OpenFileHelper.OpenDir(out dir) == false)
                {
                    return;
                }

                //至少必须存在一个版本文件夹（包含所有文件的初始文件夹）
                var dirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
                if (dirs.ExistData() == false)
                    throw new Exception("所选文件夹为空，请重新选择！");

                SourceFileDir = dir;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenHttpFileDir(object obj)
        {
            try
            {
                String dir = String.Empty;
                if (OpenFileHelper.OpenDir(out dir) == false)
                {
                    return;
                }

                ////至少必须存在一个版本文件夹（包含所有文件的初始文件夹）
                //var dirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
                //if (dirs.ExistData() == false)
                //    throw new Exception("所选文件夹为空，没有版本路径文件，无法作为更新存放目录！");

                //读取updateall 
                //判断 updateAll 是否存在  
                String mainUpdateJson = System.IO.Path.Combine(dir, UpdateDefine.MainUpdateJsonFileName);
                //一般执行这步在项目第一次创建  
                if (File.Exists(mainUpdateJson))
                {
                    try
                    {
                        _mainUpdateJson = JsonConvert.DeserializeObject<MainUpdateJson>(File.ReadAllText(mainUpdateJson));
                    }
                    catch (Exception )
                    {

                        throw new Exception($"更新文件{mainUpdateJson}损坏！");
                    }
                }
                

                HttpFileManagerDir = dir;
                EnableGeneratePanel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
