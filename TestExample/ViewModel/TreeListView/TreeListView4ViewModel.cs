using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using TestExample.View.TreeListView;

namespace TestExample.ViewModel.TreeListView
{
    public class TreeListView4ViewModel: ViewModelBase
    {
        private ObservableCollection<TreeListView4.TestObject> _items;
        private ObservableCollection<TreeListView4.TestObject> _selectedItems;

        public TreeListView4ViewModel()
        {
            var itemsSource = new List<TreeListView4.TestObject>
            {
                new TreeListView4.TestObject {Id = 1, ParentId = null, Type = 0, Name = "一年级"},
                new TreeListView4.TestObject {Id = 2, ParentId = null, Type = 0, Name = "二年级"},
                new TreeListView4.TestObject {Id = 3, ParentId = null, Type = 0, Name = "三年级"},
                new TreeListView4.TestObject {Id = 4, ParentId = 1, Type = 1, Name = "0101"},
                new TreeListView4.TestObject {Id = 5, ParentId = 1, Type = 1, Name = "0102"},
                new TreeListView4.TestObject {Id = 6, ParentId = 2, Type = 1, Name = "0201"},
                new TreeListView4.TestObject {Id = 7, ParentId = 2, Type = 1, Name = "0202"},
                new TreeListView4.TestObject {Id = 8, ParentId = 3, Type = 1, Name = "0301"},
                new TreeListView4.TestObject {Id = 9, ParentId = 3, Type = 1, Name = "0302"}
            };
            Items=new ObservableCollection<TreeListView4.TestObject>(itemsSource);
            SelectedItems=new ObservableCollection<TreeListView4.TestObject>();
        }

        public ObservableCollection<TreeListView4.TestObject> Items
        {
            get => _items;
            set
            {
                if (_items == value) return;
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public ObservableCollection<TreeListView4.TestObject> SelectedItems
        {
            get => _selectedItems;
            set
            {
                if (_selectedItems == value) return;
                _selectedItems = value;
                RaisePropertyChanged(nameof(SelectedItems));
            }
        }
    }
}
