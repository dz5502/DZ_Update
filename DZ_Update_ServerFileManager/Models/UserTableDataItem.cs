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
