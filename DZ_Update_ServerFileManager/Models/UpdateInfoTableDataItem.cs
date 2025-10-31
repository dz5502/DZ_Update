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
