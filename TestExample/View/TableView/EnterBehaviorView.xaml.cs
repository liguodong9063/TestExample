using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using TestExample.Common;
using WpfControls.Editors;

namespace TestExample.View.TableView
{
    /// <summary>
    /// EnterBehaviorView.xaml 的交互逻辑
    /// </summary>
    public partial class EnterBehaviorView : Window
    {
        public EnterBehaviorView()
        {
            InitializeComponent();

            var itemsSource = new List<ChangeRowColorView.TestObject>
            {
                new ChangeRowColorView.TestObject{Id = 1,ParentId = null,Type = 0,Name = "一年级"},
                new ChangeRowColorView.TestObject{Id = 2,ParentId = null,Type = 0,Name = "二年级"},
                new ChangeRowColorView.TestObject{Id = 3,ParentId = null,Type = 0,Name = "三年级"},
                new ChangeRowColorView.TestObject{Id = 4,ParentId = 1,Type = 1,Name = "010101010101010101010101"},
                new ChangeRowColorView.TestObject{Id = 5,ParentId = 1,Type = 1,Name = "01020102010201020102"},
                new ChangeRowColorView.TestObject{Id = 6,ParentId = 2,Type = 1,Name = "02010201"},
                new ChangeRowColorView.TestObject{Id = 7,ParentId = 2,Type = 1,Name = "020202020202"},
                new ChangeRowColorView.TestObject{Id = 8,ParentId = 3,Type = 1,Name = "0301030103010301"},
                new ChangeRowColorView.TestObject{Id = 9,ParentId = 3,Type = 1,Name = "03020302030203020302"},
                new ChangeRowColorView.TestObject{Id = 10,ParentId = 4,Type = 2,Name = "张三"},
                new ChangeRowColorView.TestObject{Id = 11,ParentId = 4,Type = 2,Name = "李四"},
                new ChangeRowColorView.TestObject{Id = 12,ParentId = 5,Type = 2,Name = "王二"},
                new ChangeRowColorView.TestObject{Id = 13,ParentId = 6,Type = 2,Name = "陈驰"},
                new ChangeRowColorView.TestObject{Id = 14,ParentId = 8,Type = 2,Name = "蔡为民"},
                new ChangeRowColorView.TestObject{Id = 15,ParentId = 8,Type = 2,Name = "汤龙"},
            };
            for (var i = 0; i < itemsSource.Count; i++)
            {
                var t = (int)Math.Floor(i / 2d);
                itemsSource[i].Index = t % 2 == 0 ? 0 : 1;
            }
            GridControl.ItemsSource = new ObservableCollection<ChangeRowColorView.TestObject>(itemsSource);
            GridControl.SelectedItem = null;
        }

        private void GridControl_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                TableView.MoveNextCell();
                TableView.ShowEditor(true);
            }
            if (e.Key == Key.F1)
            {
                e.Handled = true;
                TableView.MoveNextRow();
                TableView.ShowEditor(true);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var t = GridControl.ItemsSource;
        }

        private void PART_Editor_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as AutoCompleteTextBox).SelectedItem as IdNameObject;
            if (selectedItem != null)
            {
                var select = GridControl.SelectedItem as ChangeRowColorView.TestObject;
                select.Id = selectedItem.Id??0;
            }
        }
    }
}