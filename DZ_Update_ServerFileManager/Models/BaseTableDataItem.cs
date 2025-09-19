/*
 * 文件名称(File Name)：BaseTableDataItem
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/19 12:29:05
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/19 12:29:05
 *          修改理由：创建 DZ_Update_ServerFileManager.Models.BaseTableDataItem 类。
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_ServerFileManager.Models
{
    public class BaseTableDataItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String p = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        public int _index = 0;
        [Description("序号")]
        public int AIndex
        {
            get { return this._index; }
            set { this._index = value; NotifyPropertyChanged(); }
        }
    }
}
