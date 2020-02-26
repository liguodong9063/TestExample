using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using TestExample.Model;
using TestExample.UserControls;

namespace TestExample.ViewModel.TreeListView
{
    public class TreeListView5ViewModel : ViewModelBase
    {
        private ObservableCollection<SubjectObjectForTreeListView5> _subjects;
        private SubjectObjectForTreeListView5 _selectedSubject;
        private TreeListExpandHeaderControlViewModel _explandHeaderControlViewModel;

        public TreeListView5ViewModel()
        {

            var itemsSource = new ObservableCollection<SubjectObjectForTreeListView5>
            {
                new SubjectObjectForTreeListView5{TypeCode = "",SubjectCode = "01",SubjectName = "资产类",IsExpanded=true},
                new SubjectObjectForTreeListView5{TypeCode = "01",SubjectCode = "01",SubjectName = "固定资产",IsExpanded = true},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002",SubjectName = "存款"},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002.01",SubjectName = "长沙银行"},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002.01.01",SubjectName = "长沙蔡锷路支行"},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002.01.02",SubjectName = "长沙朝阳支行"},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002.02",SubjectName = "工商银行"},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002.02.02",SubjectName = "长沙司门口支行"},
                new SubjectObjectForTreeListView5{TypeCode = "01.01",SubjectCode = "1002.02.04",SubjectName = "长沙湘雅支行"},
                new SubjectObjectForTreeListView5{TypeCode = "01",SubjectCode = "02",SubjectName = "非固定资产",IsExpanded = true},
                new SubjectObjectForTreeListView5{TypeCode = "01.02",SubjectCode = "1003",SubjectName = "非固定资产2"},
                new SubjectObjectForTreeListView5{TypeCode = "01.02",SubjectCode = "1003.01",SubjectName = "非固定资产2.1"},
                new SubjectObjectForTreeListView5{TypeCode = "01.02",SubjectCode = "1003.02",SubjectName = "非固定资产2.2"}
            };
            Subjects=new ObservableCollection<SubjectObjectForTreeListView5>(MakeUpDatasources(itemsSource));

            ExplandHeaderControlViewModel = new TreeListExpandHeaderControlViewModel
            {
                GetItemSourcesFunc = () => Subjects
            };
            ExplandHeaderControlViewModel.Init();
        }

        public ObservableCollection<SubjectObjectForTreeListView5> Subjects
        {
            get => _subjects;
            set
            {
                if (_subjects == value) return;
                _subjects = value;
                RaisePropertyChanged(nameof(Subjects));
            }
        }

        public SubjectObjectForTreeListView5 SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (_selectedSubject == value) return;
                _selectedSubject = value;
                RaisePropertyChanged(nameof(SelectedSubject));
            }
        }

        public TreeListExpandHeaderControlViewModel ExplandHeaderControlViewModel
        {
            get => _explandHeaderControlViewModel;
            set
            {
                if (_explandHeaderControlViewModel == value) return;
                _explandHeaderControlViewModel = value;
                RaisePropertyChanged(nameof(ExplandHeaderControlViewModel));
            }
        }

        private List<SubjectObjectForTreeListView5> MakeUpDatasources(IEnumerable<SubjectObjectForTreeListView5> dataSources)
        {
            var origDatasources = dataSources.OrderBy(a => (a.PathCode ?? "").Split('.').Length).ThenBy(a => a.PathCode);
            foreach (var subject in origDatasources)
            {
                var childrens = origDatasources.Where(a => a.IsInChild == false && (a.PathCode ?? "").IndexOf((subject.PathCode ?? "") + ".", StringComparison.Ordinal) == 0 && (a.PathCode ?? "").LastIndexOf(".", StringComparison.Ordinal) == (subject.PathCode ?? "").Length).ToList();
                childrens.ForEach(a => a.IsInChild = true);
                subject.Childrens = new List<NewTreeBase>(childrens);
            }
            return origDatasources.Where(a => a.IsInChild == false).OrderBy(a => (a.PathCode ?? "").Split('.').Length).ThenBy(a => a.PathCode).ToList();
        }
    }

    public class SubjectObjectForTreeListView5 : NewTreeBase
    {
        public string SubjectCode { get; set; }
        public string TypeCode { get; set; }
        public string SubjectName { get; set; }
        public string PathCode => string.IsNullOrEmpty(TypeCode) ? SubjectCode : TypeCode + "." + SubjectCode;
        public bool IsInChild { get; set; }
    }


    public class NewTreeBase : ModelBase
    {
        private IList<NewTreeBase> _childrens;
        private bool _isExpanded;

        public IList<NewTreeBase> Childrens
        {
            get => _childrens;
            set
            {
                if (_childrens == value) return;
                _childrens = value;
                RaisePropertyChanged("Childrens");
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }
    }
}
