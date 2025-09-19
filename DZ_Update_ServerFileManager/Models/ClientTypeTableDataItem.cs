/*
 * 文件名称(File Name)：ClientTypeTableDataItem
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/19 15:01:17
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/19 15:01:17
 *          修改理由：创建 DZ_Update_ServerFileManager.Models.ClientTypeTableDataItem 类。
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_ServerFileManager.Models
{
    public class ClientTypeTableDataItem : BaseTableDataItem
    {
        public String _clientType;
        [Description("客户端版本")]
        public String ClientType
        {
            get { return this._clientType; }
            set { this._clientType = value; NotifyPropertyChanged(); }
        }
    }
}
