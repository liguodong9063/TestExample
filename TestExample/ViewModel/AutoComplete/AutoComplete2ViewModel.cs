using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using TestExample.Common;
using TestExample.Model.AutoCompleteTextBox;
using TestExample.View.AutoComplete;

namespace TestExample.ViewModel.AutoComplete
{
    public class AutoComplete2ViewModel : ViewModelBase
    {
        public List<IdNameObject> AllDataItems=new List<IdNameObject>
        {
            new IdNameObject(1,"湖南卓鑫贸易有限公司"),
            new IdNameObject(2,"广州市广源物资有限公司"),
            new IdNameObject(3,"重庆嘉奥斯物资有限公司"),
            new IdNameObject(4,"重庆卡通五金制品有限公司"),
            new IdNameObject(5,"成都德盼隆贸易有限公司"),
            new IdNameObject(6,"攀枝花市大兴工贸有限责任公司"),
            new IdNameObject(7,"重庆威福莱物资有限公司"),
            new IdNameObject(8,"湖南卓鑫贸易有限公司"),
            new IdNameObject(9,"广州昌和钢铁商贸有限公司"),
            new IdNameObject(10,"重庆领犇物资有限公司"),
            new IdNameObject(11,"广州晋跃贸易有限公司"),
            new IdNameObject(12,"湖南鸿聚供应链管理有限公司"),
            new IdNameObject(13,"成都宏通顺达贸易有限公司"),
            new IdNameObject(14,"上海铭兰贸易有限公司"),
            new IdNameObject(15,"衡阳市世亚工贸有限责任公司"),
            new IdNameObject(16,"肇庆市宏志建设有限公司"),
            new IdNameObject(17,"湖北东方瑞诚建设工程有限公司"),
            new IdNameObject(18,"中铁物贸集团昆明有限公司"),
            new IdNameObject(19,"中电建（湖南）拓海实业有限公司"),
            new IdNameObject(20,"重庆建亚远景钢结构有限公司"),
            new IdNameObject(21,"重庆七标机械有限公司"),
            new IdNameObject(22,"湖南天铁科技有限公司"),
            new IdNameObject(23,"湖南大盛供应链管理有限公司"),
            new IdNameObject(24,"武汉市佳力金属物资有限责任公司"),
            new IdNameObject(25,"湖南承运环境建设集团有限公司"),
            new IdNameObject(26,"惠州市金盈建材有限公司"),
            new IdNameObject(27,"成都好钢物资贸易有限公司"),
            new IdNameObject(28,"成都勇成伟业商贸有限公司"),
            new IdNameObject(29,"长沙闽帆钢材贸易有限公司"),
            new IdNameObject(30,"长沙汇鼎钢铁贸易有限公司"),
            new IdNameObject(31,"浙商中拓集团(湖南)有限公司"),
            new IdNameObject(32,"四川仁恒佳合贸易有限公司"),
            new IdNameObject(33,"重庆榕胜商贸有限公司"),
            new IdNameObject(34,"长沙市宁乡经济技术开发区长建钢材经营部"),
        };
        private ObservableCollection<IdNameObject> _grades=new ObservableCollection<IdNameObject>();


        private AutoCompleteIdNameDto _testObject;

        public AutoCompleteIdNameDto TestObject
        {
            get => _testObject;
            set
            {
                if (_testObject == value) return;
                _testObject = value;
                RaisePropertyChanged(nameof(TestObject));
            }
        }

        public AutoComplete2ViewModel()
        {
            _provider=new FilesystemSuggestionProvider();
            _testObject=new AutoCompleteIdNameDto
            {
                Id=11,Name = "广州晋跃贸易有限公司"
            };
        }

        public ObservableCollection<IdNameObject> Grades
        {
            get => _grades;
            set
            {
                if (_grades == value) return;
                _grades = value;
                RaisePropertyChanged(nameof(Grades));
            }
        }

        private IdNameObject _selectedGrade;
        public IdNameObject SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                if (_selectedGrade == value) return;
                _selectedGrade = value;
                RaisePropertyChanged(nameof(SelectedGrade));
            }
        }

        private FilesystemSuggestionProvider _provider;

        public FilesystemSuggestionProvider Provider
        {
            get => _provider;
            set
            {
                if (_provider == value) return;
                _provider = value;
                RaisePropertyChanged(nameof(Provider));
            }
        }
    }
}
