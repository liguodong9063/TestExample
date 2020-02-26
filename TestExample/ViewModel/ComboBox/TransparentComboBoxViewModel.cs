using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using TestExample.Common;
using TestExample.Model;

namespace TestExample.ViewModel.ComboBox
{
    public class TransparentComboBoxViewModel : ViewModelBase
    {
        private StudentForTransparentComboBoxViewModel _student;
        private ObservableCollection<IdNameObject> _grades;

        public TransparentComboBoxViewModel()
        {
            _student=new StudentForTransparentComboBoxViewModel
            {
                Id = 1,
                Name = "张三",
                Sex = 0,
                Grade = 2,
                GradeName = "二年级"
            };
            _grades =new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"一年级",true),
                new IdNameObject(2,"二年级二年级二年级二年级",true),
                new IdNameObject(3,"三（年级）",true),
                new IdNameObject(4,"四年级四年级",true),
                new IdNameObject(5,"五年级",false),
                new IdNameObject(6,"六年级",true)
            };
        }

        public StudentForTransparentComboBoxViewModel Student
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
    }

    public class StudentForTransparentComboBoxViewModel : ModelBase
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
