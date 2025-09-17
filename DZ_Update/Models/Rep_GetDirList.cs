using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update.Models
{
    public class Rep_GetDirList
    {
        /// <summary>
        /// 目录
        /// </summary>
        public String href { get; set; }
        public bool allow_upload { get; set; }
        public bool allow_delete { get; set; }
        public bool allow_search { get; set; }
        public bool allow_archive { get; set; }
        public bool dir_exists { get; set; }
        public List<PathInfo> paths { get; set; } = new List<PathInfo>();
    }

    public class PathInfo
    {
        public String path_type { get; set; }
        public String name { get; set; }
        public Int64 mtime { get; set; }
        public Int64? size { get; set; }

        public PathType GetPathType()
        {
            return (PathType)Enum.Parse(typeof(PathType), path_type);
        }
    }

    public enum PathType
    {
        Dir = 0,
        File
    }
}
