/*
 * 文件名称(File Name)：HistoryFileRecord
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/11/12 15:28:54
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/11/12 15:28:54
 *          修改理由：创建 DZ_Update.ServerFileManager.Models.HistoryFileRecord 类。
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DZ_Update_ServerFileManager.Base;

namespace DZ_Update.ServerFileManager.Models
{
    public class SelectedPathRecordModel: IBaseViewModel
    {
        //#region 单例模式

        //private static object _lock = new object();

        //private static SelectedPathRecordModel _instance;
        //public static SelectedPathRecordModel Instance
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            if (_instance == null)
        //                _instance = new SelectedPathRecordModel();

        //            return _instance;
        //        }
        //    }
        //}

        //#endregion

        /// <summary>
        /// 发布目录
        /// </summary>
        private String publishPath;
        public String PublishPath
        {
            get { return this.publishPath; }
            set { this.publishPath = value; NotifyPropertyChanged(); }
        }

        private String _latestVersion;
        /// <summary>
        /// 上一次生成的最新版本
        /// </summary>
        public String LatestVersion
        {
            get { return this._latestVersion; }
            set { this._latestVersion = value; NotifyPropertyChanged(); }
        }

        

        private String _sourceFileDir;
        public String SourceFileDir
        {
            get { return this._sourceFileDir; }
            set { this._sourceFileDir = value; NotifyPropertyChanged(); }
        }

        private String _ignoreFile;
        public String IgnoreFile
        {
            get { return this._ignoreFile; }
            set { this._ignoreFile = value; NotifyPropertyChanged(); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string p = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
    }
}
