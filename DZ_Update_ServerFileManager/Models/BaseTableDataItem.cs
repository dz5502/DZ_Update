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
