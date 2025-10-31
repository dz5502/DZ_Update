using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using DZ_Update_ServerFileManager.Base;
using DZ_Update_ServerFileManager.Models;
using DZ_Update_ServerFileManager.Views.ViewHelper;

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
