using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_ServerFileManager.Base
{
    public interface IBaseViewModel : INotifyPropertyChanged
    {
        void NotifyPropertyChanged([CallerMemberName] String p = "");
    }
}
