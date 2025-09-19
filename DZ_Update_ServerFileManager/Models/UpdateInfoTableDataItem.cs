/*
 * 文件名称(File Name)：UpdateInfoTableDataItem
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/19 11:59:16
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/19 11:59:16
 *          修改理由：创建 DZ_Update_ServerFileManager.Models.UpdateInfoTableDataItem 类。
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
    public class UpdateInfoTableDataItem : BaseTableDataItem
    {

        public String _info ;
        [Description("更新日志")]
        public String Info
        {
            get { return this._info; }
            set { this._info = value; NotifyPropertyChanged(); }
        }
    }
}
