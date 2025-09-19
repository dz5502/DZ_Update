/*
 * 文件名称(File Name)：UserTableDataItem
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/19 11:31:20
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/19 11:31:20
 *          修改理由：创建 DZ_Update_ServerFileManager.Models.UserTableDataItem 类。
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DZ_Update_ServerFileManager.Base;

namespace DZ_Update_ServerFileManager.Models
{
    public class UserTableDataItem : BaseTableDataItem
    {
        public String _userName;
        [Description("用户名")]
        public String UserName
        {
            get { return this._userName; }
            set { this._userName = value; NotifyPropertyChanged(); }
        }
    }
}
