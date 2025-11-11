using System;
using DZ_Update.Control;

namespace DZ_Update
{
    internal class Program
    {
        static void Main(string[] args)
        {
            String currentUserName = "dz";

            DZUpdateHelper dZUpdateHelper = new DZUpdateHelper();
            dZUpdateHelper.GetServerInfo();

            bool needUpdate = dZUpdateHelper.IsExistUpdate();
            if (!needUpdate)
            {
                Console.WriteLine("不存在更新");
                return;
            }

            Console.WriteLine("存在更新");
            //是否强制更新
            needUpdate = dZUpdateHelper.IsFoceToUpdate();
            if (!needUpdate)
            {
                Console.WriteLine("不是强制更新，判断是否是针对版本用户的更新");
                //不是则判断针对版本用户的更新
                needUpdate = dZUpdateHelper.IsNeedUpdate( dZUpdateHelper.GetClientVersion(), currentUserName);
                if (!needUpdate)
                {
                    Console.WriteLine("当前版本或用户不需要更新， 退出更新操作");
                    return;
                }
            }
            Console.WriteLine("需要更新， 执行更新操作");
            //执行更新
            dZUpdateHelper.Update(a=>
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"更新总进度：{a}%");
            });

            Console.WriteLine();
            Console.WriteLine("更新操作完成， 输入任意字符结束！");
            Console.ReadKey();
        }
    }   
}
