using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_CommonTools
{
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public class CalcMD5Tool
    {
        /// <summary>
        /// 计算byte数组的md5
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static String CalcMD5(byte[] buff)
        {
            if (buff == null || buff.Length < 1)
                return null;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();

            md5.TransformFinalBlock(buff, 0, buff.Length);

            byte[] result = md5.Hash;
            md5.Clear();
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("X2"));

            return sb.ToString();

        }

        public static String CalcStrMD5(String str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return CalcMD5(bytes);
        }

        private static String CalcMD5(String file)
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            int bufferSize = 1048576;
            byte[] buff = new byte[bufferSize];
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();
            long offset = 0;
            while (offset < fs.Length)
            {
                long readSize = bufferSize;
                if (offset + readSize > fs.Length)
                    readSize = fs.Length - offset;
                fs.Read(buff, 0, Convert.ToInt32(readSize));
                if (offset + readSize < fs.Length)
                    md5.TransformBlock(buff, 0, Convert.ToInt32(readSize), buff, 0);
                else
                    md5.TransformFinalBlock(buff, 0, Convert.ToInt32(readSize));
                offset += bufferSize;
            }
            if (offset >= fs.Length)
            {
                fs.Close();
                byte[] result = md5.Hash;
                md5.Clear();
                StringBuilder sb = new StringBuilder(32);
                for (int i = 0; i < result.Length; i++)
                    sb.Append(result[i].ToString("X2"));


               return sb.ToString();

            }
            else
            {
                fs.Close();

            }

            return null;
        }

        public static String CalcFileMD5(String file)
        {
            return CalcMD5(file);
        }

    }
}
