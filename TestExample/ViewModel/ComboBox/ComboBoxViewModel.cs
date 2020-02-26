using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using TestExample.Common;
using TestExample.Model;

namespace TestExample.ViewModel.ComboBox
{
    public class ComboBoxViewModel:ViewModelBase
    {
        private StudentForComboBoxViewModel _student1;
        private StudentForComboBoxViewModel _student2;
        private StudentForComboBoxViewModel _student3;
        private ObservableCollection<IdNameObject> _grades;
        private ObservableCollection<StudentForComboBoxViewModel> _students;

        public ComboBoxViewModel()
        {
            _student1=new StudentForComboBoxViewModel
            {
                Id = 1,
                Name = "张三",
                Sex = 0,
                Grade = 2,
                GradeName = "二年级"
            };
            _student2 = new StudentForComboBoxViewModel
            {
                Id = 1,
                Name = "张三",
                Sex = 0,
                Grade = 7,
                GradeName = "初一"
            };
            _student3 = new StudentForComboBoxViewModel
            {
                Id = 1,
                Name = "张三",
                Sex = 0,
                Grade = 7,
                GradeName = "五年级"
            };
            _grades =new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"一年级",true),
                new IdNameObject(2,"二年级",true),
                new IdNameObject(3,"三年级",true),
                new IdNameObject(4,"四年级",true),
                new IdNameObject(5,"五年级",false),
                new IdNameObject(6,"六年级",true),
            };
            _students=new ObservableCollection<StudentForComboBoxViewModel>
            {
                new StudentForComboBoxViewModel{Id = 1,Name = "张三",Sex = 0,Grade = 1,GradeName = "一年级",IsUse = true},
                new StudentForComboBoxViewModel{Id = 2,Name = "李四",Sex = 1,Grade = 2,GradeName = "二年级",IsUse = true},
                new StudentForComboBoxViewModel{Id = 3,Name = "王五",Sex = 0,Grade = 3,GradeName = "三年级",IsUse = false},
                new StudentForComboBoxViewModel{Id = 4,Name = "张飞",Sex = 1,Grade = 4,GradeName = "四年级",IsUse = true},
            };
        }

        public StudentForComboBoxViewModel Student1
        {
            get => _student1;
            set
            {
                if (_student1 == value) return;
                _student1 = value;
                RaisePropertyChanged(nameof(Student1));
            }
        }

        public StudentForComboBoxViewModel Student2
        {
            get => _student2;
            set
            {
                if (_student2 == value) return;
                _student2 = value;
                RaisePropertyChanged(nameof(Student2));
            }
        }

        public StudentForComboBoxViewModel Student3
        {
            get => _student3;
            set
            {
                if (_student3 == value) return;
                _student3 = value;
                RaisePropertyChanged(nameof(Student3));
            }
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

        public ObservableCollection<StudentForComboBoxViewModel> Students
        {
            get => _students;
            set
            {
                if (_students == value) return;
                _students = value;
                RaisePropertyChanged(nameof(Students));
            }
        }
    }

    public class StudentForComboBoxViewModel:ModelBase
    {
        private int _id;
        private string _name;
        private int _sex;
        private int _grade;
        private string _gradeName;
        private bool _isUse;

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

        public int Sex
        {
            get => _sex;
            set
            {
                if (_sex == value) return;
                _sex = value;
                RaisePropertyChanged();
            }
        }

        public int Grade
        {
            get => _grade;
            set
            {
                if (_grade == value) return;
                _grade = value;
                RaisePropertyChanged();
            }
        }

        public string GradeName
        {
            get => _gradeName;
            set
            {
                if (_gradeName == value) return;
                _gradeName = value;
                RaisePropertyChanged();
            }
        }

        public bool IsUse
        {
            get => _isUse;
            set
            {
                if (_isUse == value) return;
                _isUse = value;
                RaisePropertyChanged();
            }
        }
    }
}
