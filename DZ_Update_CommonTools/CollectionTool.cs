using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Update_CommonTools
{
    public static class CollectionTool
    {
        public static bool ExistData<T>(this IList<T> list)
        {
            return HasData(list);
        }
        public static bool ExistData<T>(IEnumerable<T> list)
        {
            return HasData(list);
        }
        public static bool ExistData<T>(T[] array)
        {
            return HasData(array);
        }

        public static bool HasData<T>(IList<T> list)
        {
            return (list != null) && (list.Count > 0);
        }
        public static bool HasData<T>(IEnumerable<T> list)
        {
            return (list != null) && (list.Count() > 0);
        }
        public static bool HasData<T>(T[] array)
        {
            return (array != null) && (array.Length > 0);
        }
        /// <summary>
        /// 抽稀list  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="outCount">指定数量</param>
        /// <returns></returns>
        public static List<T> GetSubList<T>(List<T> sourceList, int outCount)
        {
            List<T> list = new List<T>();

            int interval = sourceList.Count / outCount + 1;
            for (int i = 0; i < sourceList.Count; i += interval)
            {
                list.Add(sourceList[i]);
            }
            //添加最后一个
            if(list.Contains(sourceList.Last()))
                list.Add(sourceList.Last());
            return list;
        }

        /// <summary>
        /// 抽稀list  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="outCount">指定间隔</param>
        /// <returns></returns>
        public static List<T> GetSubListUseInterval<T>(List<T> sourceList, int interval)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < sourceList.Count; i += interval)
            {
                list.Add(sourceList[i]);
            }
            //添加最后一个
            if (list.Contains(sourceList.Last()))
                list.Add(sourceList.Last());
            return list;
        }
    }

    //创建list<T>方法distinct比较器
    public delegate bool CompareDelegate<T>(T x, T y);
    public class Compare<T> : IEqualityComparer<T>
    {
        private CompareDelegate<T> _compare;
        public Compare(CompareDelegate<T> d)
        {
            this._compare = d;
        }

        public bool Equals(T x, T y)
        {
            if (_compare != null)
            {
                return this._compare(x, y);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
