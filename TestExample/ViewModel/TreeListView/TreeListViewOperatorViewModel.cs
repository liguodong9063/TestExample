using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TestExample.View.TreeListView;

namespace TestExample.ViewModel.TreeListView
{
    public class TreeListViewOperatorViewModel: ViewModelBase
    {
        private TreeListObject _selectedItem;
        private ObservableCollection<TreeListObject> _itemsSources;

        private ICommand _addCommand;
        private ICommand _deleteCommand;

        public TreeListViewOperatorViewModel()
        {
            _addCommand = new RelayCommand(() =>
            {
                SelectedItem.Childrens.Add(new TreeListObject{PathCode = "001.001.001",Name = "苹果"});
            },()=>SelectedItem!=null);
            _deleteCommand = new RelayCommand(() =>
            {
                var parentCode = SelectedItem.PathCode.Substring(0, SelectedItem.PathCode.LastIndexOf("."));
                var parent = FindParent(ItemsSources, parentCode);
                parent?.Childrens.Remove(SelectedItem);
            }, () => SelectedItem != null);


            var itemSources = new List<TreeListObject>
            {
                new TreeListObject{PathCode = "001",Name = "食物",Childrens = new ObservableCollection<TreeListObject>()},
                new TreeListObject{PathCode = "001.001",Name = "水果",Childrens = new ObservableCollection<TreeListObject>()},
                new TreeListObject{PathCode = "001.002",Name = "零食",Childrens = new ObservableCollection<TreeListObject>()}
            };

            var origDatasources = itemSources.OrderByDescending(a => (a.PathCode ?? "").Split('.').Length).ThenByDescending(a => a.PathCode).ToList();
            var totalCount = origDatasources.Count;
            for (var index = 0; index < totalCount; index++)
            {
                var lastIndexDecimal = origDatasources[index].PathCode.LastIndexOf(".", StringComparison.Ordinal);
                if (lastIndexDecimal < 0) continue;
                var parent = origDatasources.FirstOrDefault(a => a.PathCode == origDatasources[index].PathCode.Substring(0, lastIndexDecimal));
                if (parent == null) continue;
                parent.Childrens.Add(origDatasources[index]);
                origDatasources.Remove(origDatasources[index]);
                totalCount--;
                index--;
            }

            var datas = ResetOrder(new ObservableCollection<TreeListObject>(origDatasources));
            ItemsSources = datas;
        }

        private TreeListObject FindParent(ObservableCollection<TreeListObject> itemSources,string parentCode)
        {

            foreach (var obj in itemSources)
            {
                if (obj.PathCode == parentCode)
                {
                    return obj;
                }
                if (obj.Childrens.Count > 0)
                {
                    return FindParent(obj.Childrens, parentCode);
                }
            }
            return null;
        }

        public ICommand AddCommand => _addCommand;
        public ICommand DeleteCommand => _deleteCommand;

        public TreeListObject SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public ObservableCollection<TreeListObject> ItemsSources
        {
            get { return _itemsSources; }
            set
            {
                if (_itemsSources == value) return;
                _itemsSources = value;
                RaisePropertyChanged(nameof(ItemsSources));
            }
        }

        private ObservableCollection<TreeListObject> ResetOrder(ObservableCollection<TreeListObject> accountingInitializations)
        {
            foreach (var data in accountingInitializations)
            {
                if (data.Childrens != null && data.Childrens.Count > 0)
                {
                    data.Childrens = ResetOrder(data.Childrens);
                }
            }
            return new ObservableCollection<TreeListObject>(accountingInitializations.OrderBy(a => (a.PathCode ?? "").Split('.').Length).ThenBy(a => a.PathCode).ToList());
        }
    }
}
