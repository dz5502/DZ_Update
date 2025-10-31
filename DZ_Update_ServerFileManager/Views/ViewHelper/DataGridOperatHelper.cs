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
