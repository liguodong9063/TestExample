using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using TestExample.Common;

namespace TestExample.ViewModel.AutoComplete
{
    public class AutoCompleteViewModel: ViewModelBase
    {
        private List<IdNameObject> _allDataItems=new List<IdNameObject>
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
        private string _inputedText;

        public AutoCompleteViewModel()
        {
            InputedText = "九年";
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

        public string InputedText
        {
            get => _inputedText;
            set
            {
                if (_inputedText == value) return;
                _inputedText = value;

                if (_inputedText?.Trim().Length >= 2)
                {
                    var regex = new Regex("^(([\u4e00-\u9fa5]{2,30})|([\u4e00-\u9fa5]{2,26}（+[\u4e00-\u9fa5]{2,26}）[\u4e00-\u9fa5]{0,24})|([\u4e00-\u9fa5]{2,26}\\(+[\u4e00-\u9fa5]{2,26}\\)[\u4e00-\u9fa5]{0,24}))$");
                    if (regex.IsMatch(_inputedText))
                    {
                        if (_grades.Count(a => a.Name.Contains(value)) == 0)
                        {
                            Grades = new ObservableCollection<IdNameObject>(_allDataItems.Where(a => a.Name.Contains(value)).Take(5));
                        }
                    }
                }

                RaisePropertyChanged(nameof(InputedText));
            }
        }

        public AutoCompleteFilterPredicate<object> GradeFilter
        {
            get
            {
                return (searchText, obj) => ((IdNameObject) obj).Name.Contains(searchText) || ((IdNameObject) obj).Id.ToString().Contains(searchText);
            }
        }
    }
}
