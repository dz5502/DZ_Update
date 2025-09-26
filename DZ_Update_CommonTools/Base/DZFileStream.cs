/*
 * 文件名称(File Name)：DZFileStream
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/26 16:49:08
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/26 16:49:08
 *          修改理由：创建 DZ_Update.CommonTools.Base.DZFileStream 类。
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update.CommonTools.Base
{
    public class DZFileStream : FileStream
    {
        public DZFileStream(String saveFile)
            : base(saveFile, FileMode.Create)
        {
        }

        public long CurrentSize { get; private set; }


        public event EventHandler Progress;
        //private List<byte> _data = new List<byte>();
        public override void Write(byte[] array, int offset, int count)
        {
            base.Write(array, offset, count);

            //byte[] temp = new byte[count];
            //Array.Copy(array, offset, temp, 0, count);
            //_data.AddRange(temp);

            CurrentSize += count;
            Progress?.Invoke(this, EventArgs.Empty);
        }


        //public byte[] GetBytes()
        //{ 
        //    return _data.ToArray();
        //}
    }
}
