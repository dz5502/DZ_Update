using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_Models.update
{
    public class SubUpdateJson
    {
        public String CurrentVersion { get; set; }
        public bool ForceUpdate { get; set; }
        public List<String> TargetClientType { get; set; } = new List<string>();
        public List<String> TargetClientUser { get; set; } = new List<string>();
        public List<String> UpdateInfo { get; set; } = new List<string>();
        public List<RemoteFileInfo> FileList { get; set; } = new List<RemoteFileInfo>();

    }
}
