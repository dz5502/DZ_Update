using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DZ_Update
{
    public static class StringTool
    {
        /// <summary>
        /// 首字母小写写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }
        /// <summary>
        /// 格式化字符串  简化在vs2015中的操作
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static String StringFormat(params object[] list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }

        public static byte[] Base64StringToBytes(this string input)
        {
            return Convert.FromBase64String(input);
        }

        /// <summary>
        /// 判断字符串中是否包含中文
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <returns>判断结果</returns>
        public static bool ExistChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        public static bool IsTelePhone(this string str)
        {
            string pattern = @"^1[3-9]\d{9}$";
            return Regex.IsMatch(str, pattern);
        }


    }
}
