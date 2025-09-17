/*
 * 文件名称(File Name)：LocalUpdateConfig
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/17 14:17:41
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/17 14:17:41
 *          修改理由：创建 DZ_Update.Models.update.LocalUpdateConfig 类。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update.Models.update
{
    public class LocalUpdateConfig
    {
        public String Version { get; set; }
        public String ClientType { get; set; }
        public String UserName { get; set; }
        public String Pwd { get; set; }
        public String HttpServer { get; set; }
    }
}
