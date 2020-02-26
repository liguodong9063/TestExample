using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestExample.View.TreeListView
{
    /// <summary>
    /// TreeListView3.xaml 的交互逻辑
    /// </summary>
    public partial class TreeListView3
    {
        public TreeListView3()
        {
            InitializeComponent();
            var itemsSource = new List<TestObject>
            {
                new TestObject {Id = 1, ParentId = null, Type = 0, Name = "一年级"},
                new TestObject {Id = 2, ParentId = null, Type = 0, Name = "二年级"},
                new TestObject {Id = 3, ParentId = null, Type = 0, Name = "三年级"},
                new TestObject {Id = 4, ParentId = 1, Type = 1, Name = "0101"},
                new TestObject {Id = 5, ParentId = 1, Type = 1, Name = "0102"},
                new TestObject {Id = 6, ParentId = 2, Type = 1, Name = "0201"},
                new TestObject {Id = 7, ParentId = 2, Type = 1, Name = "0202"},
                new TestObject {Id = 8, ParentId = 3, Type = 1, Name = "0301"},
                new TestObject {Id = 9, ParentId = 3, Type = 1, Name = "0302"},
                new TestObject {Id = 10, ParentId = 4, Type = 2, Name = "张三"},
                new TestObject {Id = 11, ParentId = 4, Type = 2, Name = "李四"},
                new TestObject {Id = 12, ParentId = 5, Type = 2, Name = "王二"},
                new TestObject {Id = 13, ParentId = 6, Type = 2, Name = "陈驰"},
                new TestObject {Id = 14, ParentId = 8, Type = 2, Name = "蔡为民"},
                new TestObject {Id = 15, ParentId = 8, Type = 2, Name = "汤龙"},
            };
            for (var i = 0; i < itemsSource.Count; i++)
            {
                var t = (int)Math.Floor(i / 2d);
                itemsSource[i].Index = t % 2 == 0 ? 0 : 1;
            }
            GridControl.ItemsSource = new ObservableCollection<TestObject>(itemsSource);
            GridControl.SelectedItem = null;
        }

        public class TestObject
        {
            public int Index { get; set; }
            public int Id { get; set; }
            public int? ParentId { get; set; }
            public string Name { get; set; }
            public int Type { get; set; } //0:年级；1：班级；2：学生
        }
    }
}
