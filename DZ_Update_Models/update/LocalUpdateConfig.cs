using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_Models.update
{
    public class LocalUpdateConfig
    {
        public String Version { get; set; }
        public String ClientType { get; set; }
        public String UserName { get; set; } = "timtk";
        public String Pwd { get; set; } = "Tkry20@cz.zh";
        public String HttpServer { get; set; } = "https://dufs.tunnelkey.com/PCAS";
        public List<String> HttpServerUrlList = new List<string>();
    }
}
