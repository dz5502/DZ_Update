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
