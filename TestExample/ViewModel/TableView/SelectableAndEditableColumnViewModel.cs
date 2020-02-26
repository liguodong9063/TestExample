using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using TestExample.Common;

namespace TestExample.ViewModel.TableView
{
    public class SelectableAndEditableColumnViewModel:ViewModelBase
    {
        private ObservableCollection<IdNameObject> _items;

        public SelectableAndEditableColumnViewModel()
        {
            Items=new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"张三"),
                new IdNameObject(2,"李四"),
                new IdNameObject(3,"王五"),
                new IdNameObject(4,"李白"),
                new IdNameObject(5,"宋江"),
                new IdNameObject(6,"王羲之"),
                new IdNameObject(7,"杜甫"),
                new IdNameObject(8,"武松"),
                new IdNameObject(9,"武"),
                new IdNameObject(10,"王五")
            };
        }

        public ObservableCollection<IdNameObject> Items
        {
            get => _items;
            set
            {
                if (_items == value) return;
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }
    }
}