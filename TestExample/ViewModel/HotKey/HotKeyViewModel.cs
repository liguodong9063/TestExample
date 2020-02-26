using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using TestExample.Model;

namespace TestExample.ViewModel.HotKey
{
    public class HotKeyViewModel:ViewModelBase
    {
        private ObservableCollection<HotKeyDto> _itemSources;
        private string _testContent;

        public HotKeyViewModel()
        {
            _testContent = "ABC\r\nDEFG";
            _itemSources=new ObservableCollection<HotKeyDto>
            {
                new HotKeyDto{Id = 1,Name = "张三"},
                new HotKeyDto{Id = 2,Name = "李四"},
                new HotKeyDto{Id = 3,Name = "王五"},
            };
        }

        public ObservableCollection<HotKeyDto> ItemSources
        {
            get => _itemSources;
            set
            {
                if (_itemSources == value) return;
                _itemSources = value;
                RaisePropertyChanged("ItemSources");
            }
        }

        public string TestContent
        {
            get => _testContent;
            set
            {
                if (_testContent == value) return;
                _testContent = value;
                RaisePropertyChanged("TestContent");
            }
        }
    }

    public class HotKeyDto: ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}