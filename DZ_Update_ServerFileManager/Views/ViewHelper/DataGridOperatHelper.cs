/*
 * 文件名称(File Name)：DataGridOperatHelper
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/19 14:07:38
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/19 14:07:38
 *          修改理由：创建 DZ_Update_ServerFileManager.Views.ViewHelper.DataGridOperatHelper 类。
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZ_Update_ServerFileManager.Models;

namespace DZ_Update_ServerFileManager.Views.ViewHelper
{
    public class DataGridOperatHelper
    {
        public static void AddNewDataItem<T>(ObservableCollection<T> tableDataList) where T : BaseTableDataItem, new()
        {
            T newItem = new T();
            newItem.AIndex = tableDataList.Count;
            tableDataList.Add((T)newItem);
        }

        public static void DeleteDataItem<T>(ObservableCollection<T> tableDataList, object oldItem) 
        {
            tableDataList.Remove((T)oldItem);
            //重排序号
            int index = 0;
            foreach (var item in tableDataList)
            {
                (item as BaseTableDataItem).AIndex = index++;
            }
        }
    }
}
