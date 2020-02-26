using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using TestExample.UserControls;
using TestExample.ViewModel.TreeListView;
using Control = System.Windows.Forms.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace TestExample.View.TreeListView
{
    /// <summary>
    /// TreeListView4.xaml 的交互逻辑
    /// </summary>
    public partial class TreeListView4 : Window
    {
        public TreeListView4()
        {
            InitializeComponent();
            //var itemsSource = new List<TestObject>
            //{
            //    new TestObject {Id = 1, ParentId = null, Type = 0, Name = "一年级"},
            //    new TestObject {Id = 2, ParentId = null, Type = 0, Name = "二年级"},
            //    new TestObject {Id = 3, ParentId = null, Type = 0, Name = "三年级"},
            //    new TestObject {Id = 4, ParentId = 1, Type = 1, Name = "0101"},
            //    new TestObject {Id = 5, ParentId = 1, Type = 1, Name = "0102"},
            //    new TestObject {Id = 6, ParentId = 2, Type = 1, Name = "0201"},
            //    new TestObject {Id = 7, ParentId = 2, Type = 1, Name = "0202"},
            //    new TestObject {Id = 8, ParentId = 3, Type = 1, Name = "0301"},
            //    new TestObject {Id = 9, ParentId = 3, Type = 1, Name = "0302"}
            //};
            //GridControl.ItemsSource = new ObservableCollection<TestObject>(itemsSource);
            //GridControl.SelectedItem = null;
        }

        public class TestObject
        {
            public int Index { get; set; }
            public int Id { get; set; }
            public int? ParentId { get; set; }
            public string Name { get; set; }
            public int Type { get; set; } //0:年级；1：班级；2：学生
        }

        private void MyTreeListView_OnFocusedRowChanging(object sender, CanceledEventArgs e)
        {
            var selectedItems=GridControl.SelectedItems==null||GridControl.SelectedItems.Count==0?new List<TestObject>(): (GridControl.SelectedItems as ObservableCollection<TestObject>).ToList();
            var data = (TestObject)e.NewRow;

            if (data.ParentId == null)
            {
                //避免按Shift时鼠标点击ParentId为null的行导致可以联系问题
                e.Cancel = true;
            }
            else
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                    //避免背景色与进入编辑状态的框不在同一行问题，即让选中行为鼠标选中的新行
                    ((TreeListView4ViewModel) DataContext).SelectedItems = new ObservableCollection<TestObject> { data };
                }
                else
                {
                    if (selectedItems.Count > 0 && data.ParentId != selectedItems.Max(a => a.ParentId))
                    {
                        //避免跨Parent选明细
                        ((TreeListView4ViewModel)DataContext).SelectedItems = new ObservableCollection<TestObject>();
                    }
                }
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridControl.SelectedItem == null)
                return;
            var selectedItem=(TestObject) GridControl.SelectedItem;
            MessageBox.Show(selectedItem.Name);
        }
    }

    public class RowTemplateSelector3 : DataTemplateSelector
    {
        public DataTemplate EvenRowTemplate { get; set; }
        public DataTemplate OddRowTemplate { get; set; }
        public DataTemplate ParentRowTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var row = item as RowData;
            var rowData = (TreeListView4.TestObject)row.Row;
            return rowData.ParentId == null ? ParentRowTemplate : base.SelectTemplate(item, container);
        }
    }
}
