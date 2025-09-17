using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update.Models.update
{
    public class SubUpdateJson
    {
        public String CurrentVersion { get; set; }
        public bool ForceUpdate { get; set; }
        public List<String> TargetClientType { get; set; }
        public List<String> TargetClientUser { get; set; }
        public List<String> UpdateInfo { get; set; }
        public List<RemoteFileInfo> FileList { get; set; }

    }
}
