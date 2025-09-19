/*
 * 文件名称(File Name)：UCDateGrid
 * 
 * 版权所有(Copyright)：天佑智隧
 *
 * 功能描述(Description)：
 * 
 * 作者(Author)：ZDX
 * 
 * 日期(Create Date)：2025/9/19 12:27:12
 * 
 * 修改记录(Revision History)：
 *      R1：
 *          修改作者：ZDX
 *          修改日期：2025/9/19 12:27:12
 *          修改理由：创建 DZ_Update_ServerFileManager.Views.Controls.UCDateGrid 类。
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using DZ_Update_ServerFileManager.Base;
using DZ_Update_ServerFileManager.Models;
using DZ_Update_ServerFileManager.Views.ViewHelper;
using static DZ_Update_ServerFileManager.Views.Controls.UCDateGrid;

namespace DZ_Update_ServerFileManager.Views.Controls
{
    /// <summary>
    /// UCDateGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UCDateGrid : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String p = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
        public UCDateGrid()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private Action _addNewItemAction;
        private Action _deleteItemAction;

        public void SetTableData<T>(ObservableCollection<T> tableDataList) where T : BaseTableDataItem, new()
        {
            // 动态生成列
            var propertys = typeof(T).GetProperties();
            var list = propertys.ToList();
            list.Sort((a, b)=>a.Name.CompareTo(b.Name));
            foreach (var property in list)
            {
                var column = new DataGridTextColumn
                {
                    Header = GetDescription(property),
                    Binding = new System.Windows.Data.Binding(property.Name)
                };
                dataGrid.Columns.Add(column);
            }

            dataGrid.ItemsSource = tableDataList;

            _addNewItemAction = () => DataGridOperatHelper.AddNewDataItem(tableDataList);
            _deleteItemAction = () => DataGridOperatHelper.DeleteDataItem<T>(tableDataList, SelecedItem);

        }


        private object _selecedItem;
        public object SelecedItem
        {
            get { return this._selecedItem; }
            set { this._selecedItem = value; NotifyPropertyChanged(); }
        }


        public BaseCommand AddNewItemCommand { get { return new BaseCommand(AddNewItem); } }
        public BaseCommand DeleteItemCommand { get { return new BaseCommand(DeleteItem); } }


        private void AddNewItem(object obj)
        {
            _addNewItemAction?.Invoke();
        }
        private void DeleteItem(object obj)
        {
            _deleteItemAction?.Invoke();
        }



























        public string GetDescription(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? property.Name; // 如果没有 Description，则使用属性名
        }


    }
}
