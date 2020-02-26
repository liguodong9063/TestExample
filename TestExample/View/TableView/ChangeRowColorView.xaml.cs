using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using TestExample.Model;

namespace TestExample.View.TableView
{
    /// <summary>
    /// ChangeRowColorView.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeRowColorView:Window
    {
        public ChangeRowColorView()
        {
            InitializeComponent();
            var itemsSource = new List<TestObject>
            {
                new TestObject{Id = 1,ParentId = null,Type = 0,Name = "一年级"},
                new TestObject{Id = 2,ParentId = null,Type = 0,Name = "二年级"},
                new TestObject{Id = 3,ParentId = null,Type = 0,Name = "三年级"},
                new TestObject{Id = 4,ParentId = 1,Type = 1,Name = "010101010101010101010101"},
                new TestObject{Id = 5,ParentId = 1,Type = 1,Name = "01020102010201020102"},
                new TestObject{Id = 6,ParentId = 2,Type = 1,Name = "02010201"},
                new TestObject{Id = 7,ParentId = 2,Type = 1,Name = "020202020202"},
                new TestObject{Id = 8,ParentId = 3,Type = 1,Name = "0301030103010301"},
                new TestObject{Id = 9,ParentId = 3,Type = 1,Name = "03020302030203020302"},
                new TestObject{Id = 10,ParentId = 4,Type = 2,Name = "张三"},
                new TestObject{Id = 11,ParentId = 4,Type = 2,Name = "李四"},
                new TestObject{Id = 12,ParentId = 5,Type = 2,Name = "王二"},
                new TestObject{Id = 13,ParentId = 6,Type = 2,Name = "陈驰"},
                new TestObject{Id = 14,ParentId = 8,Type = 2,Name = "蔡为民"},
                new TestObject{Id = 15,ParentId = 8,Type = 2,Name = "汤龙"},
            };
            for (var i = 0; i < itemsSource.Count; i++)
            {
                var t = (int)Math.Floor(i / 2d);
                itemsSource[i].Index = t % 2 == 0 ? 0 : 1;
            }
            GridControl.ItemsSource = new ObservableCollection<TestObject>(itemsSource);
            GridControl.SelectedItem = null;

            GridControl2.ItemsSource = new ObservableCollection<TestObject>(itemsSource);
            GridControl2.SelectedItem = null;

            GridControl3.ItemsSource = new ObservableCollection<TestObject>(itemsSource);
            GridControl3.SelectedItem = null;
        }

        public class TestObject:ModelBase
        {
            private int _index;
            private int _id;
            private int? _parentId;
            private string _name;
            private int _type;

            public int Index
            {
                get => _index;
                set
                {
                    if (_index == value) return;
                    _index = value;
                    RaisePropertyChanged();
                }
            }
            public int Id
            {
                get => _id;
                set
                {
                    if (_id == value) return;
                    _id = value;
                    RaisePropertyChanged();
                }
            }
            public int? ParentId
            {
                get => _parentId;
                set
                {
                    if (_parentId == value) return;
                    _parentId = value;
                    RaisePropertyChanged();
                }
            }
            public string Name
            {
                get => _name;
                set
                {
                    if (_name == value) return;
                    _name = value;
                    RaisePropertyChanged();
                }
            }

            //0:年级；1：班级；2：学生
            public int Type
            {
                get => _type;
                set
                {
                    if (_type == value) return;
                    _type = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void GridControl3_OnItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            TableView3.BestFitColumns();
        }

        private void GridControl3_OnLoaded(object sender, RoutedEventArgs e)
        {
            GridControl3.ItemsSourceChanged += GridControl3_OnItemsSourceChanged;
        }

        private void ChangeRowColorView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            {
                var textBox = e.OriginalSource as TextBox;
                if (textBox == null) return;
                var dataContext=textBox.DataContext as EditGridCellData;
                var columnName=dataContext.Column.FieldName;
            }
        }
    }
}
