using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using TestExample.Common;
using TestExample.Model;

namespace TestExample.ViewModel.ComboBox
{
    public class NewComboBoxViewModel : ViewModelBase
    {
        private StudentForNewComboBoxViewModel _student;
        private ObservableCollection<IdNameObject> _grades;
        private ObservableCollection<StudentForNewComboBoxViewModel> _students;

        public NewComboBoxViewModel()
        {
            FillSources();
        }

        public StudentForNewComboBoxViewModel Student
        {
            get => _student;
            set
            {
                if (_student == value) return;
                _student = value;
                RaisePropertyChanged(nameof(Student));
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

        public ObservableCollection<StudentForNewComboBoxViewModel> Students
        {
            get => _students;
            set
            {
                if (_students == value) return;
                _students = value;
                RaisePropertyChanged(nameof(Students));
            }
        }

        public void FillSources()
        {
            Grades = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(null,"",true),
                new IdNameObject(1,"一年级",true),
                new IdNameObject(2,"二年级",true),
                new IdNameObject(3,"三年级",true),
                new IdNameObject(4,"四年级",true),
                new IdNameObject(5,"五年级",false),
                new IdNameObject(6,"六年级",true)
            };
            Students = new ObservableCollection<StudentForNewComboBoxViewModel>
            {
                new StudentForNewComboBoxViewModel{Id = 01,Name = "张一",Sex = 0,Grade = 1,GradeName = "一年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 02,Name = "张二",Sex = 0,Grade = 5,GradeName = "五年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 03,Name = "张三",Sex = 1,Grade = 2,GradeName = "二年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 04,Name = "张四",Sex = 0,Grade = 3,GradeName = "三年级",IsUse = false},
                new StudentForNewComboBoxViewModel{Id = 05,Name = "张五",Sex = 1,Grade = 4,GradeName = "四年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 06,Name = "张六",Sex = 0,Grade = 1,GradeName = "一年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 07,Name = "张七",Sex = 0,Grade = 5,GradeName = "五年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 08,Name = "张八",Sex = 1,Grade = 2,GradeName = "二年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 09,Name = "张九",Sex = 0,Grade = 3,GradeName = "三年级",IsUse = false},
                new StudentForNewComboBoxViewModel{Id = 10,Name = "李一",Sex = 1,Grade = 4,GradeName = "四年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 11,Name = "李二",Sex = 0,Grade = 1,GradeName = "一年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 12,Name = "李三",Sex = 0,Grade = 5,GradeName = "五年级",IsUse = true},



                //new StudentForNewComboBoxViewModel{Id = 13,Name = "李四",Sex = 1,Grade = 2,GradeName = "二年级",IsUse = true},
                //new StudentForNewComboBoxViewModel{Id = 14,Name = "李五",Sex = 0,Grade = 3,GradeName = "三年级",IsUse = false},
                //new StudentForNewComboBoxViewModel{Id = 15,Name = "李六",Sex = 1,Grade = 4,GradeName = "四年级",IsUse = true},
                //new StudentForNewComboBoxViewModel{Id = 16,Name = "李七",Sex = 0,Grade = 1,GradeName = "一年级",IsUse = true},
                //new StudentForNewComboBoxViewModel{Id = 17,Name = "李八",Sex = 0,Grade = 5,GradeName = "五年级",IsUse = true},
                //new StudentForNewComboBoxViewModel{Id = 18,Name = "李九",Sex = 1,Grade = 2,GradeName = "二年级",IsUse = true},
                //new StudentForNewComboBoxViewModel{Id = 19,Name = "王一",Sex = 0,Grade = 3,GradeName = "三年级",IsUse = false},
                //new StudentForNewComboBoxViewModel{Id = 20,Name = "王二",Sex = 1,Grade = 4,GradeName = "四年级",IsUse = true}

            };

            Student = new StudentForNewComboBoxViewModel
            {
                Id = 1,
                Name = "张三",
                Sex = 0,
                Grade = 7,
                GradeName = "七年级"
            };
        }

        public void ChangeSources()
        {
            Grades = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(null,"",true),
                //new IdNameObject(1,"一年级",true),
                new IdNameObject(2,"二年级",true),
                new IdNameObject(5,"五年级",false),
                new IdNameObject(6,"六年级",true)
            };
            Students = new ObservableCollection<StudentForNewComboBoxViewModel>
            {
                new StudentForNewComboBoxViewModel{Id = 1,Name = "张三",Sex = 0,Grade = 1,GradeName = "一年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 2,Name = "李四",Sex = 1,Grade = 2,GradeName = "二年级",IsUse = true},
                new StudentForNewComboBoxViewModel{Id = 4,Name = "张飞",Sex = 1,Grade = 4,GradeName = "四年级",IsUse = true}
            };
            Student = new StudentForNewComboBoxViewModel
            {
                Id = 1,
                Name = "张三",
                Sex = 0,
                Grade = 8,
                GradeName = "八年级"
            };
        }
    }

    public class StudentForNewComboBoxViewModel:ModelBase
    {
        private int _id;
        private string _name;
        private int _sex;
        private int? _grade;
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

        public int? Grade
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
