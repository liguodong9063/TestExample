using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using TestExample.Common;
using TestExample.Model;

namespace TestExample.ViewModel.TableView
{
    public class CustomUnBoundViewModel:ViewModelBase
    {
        private ObservableCollection<IdNameObject> _sexTypes;
        private ObservableCollection<IdNameObject> _grades;
        private ObservableCollection<StudentForCustomUnBound> _students;

        public CustomUnBoundViewModel()
        {
            _sexTypes=new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"男"),
                new IdNameObject(2,"女")
            };
            _grades = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"一年级"),
                new IdNameObject(2,"二年级"),
                new IdNameObject(3,"三年级"),
                new IdNameObject(4,"四年级"),
                new IdNameObject(5,"五年级"),
                new IdNameObject(6,"六年级")
            };
            _students=new ObservableCollection<StudentForCustomUnBound>
            {
                new StudentForCustomUnBound{Id = 1,Sex = 1,Grade = 1,Name = "张三",BillState = 1},
                new StudentForCustomUnBound{Id = 2,Sex = 1,Grade = 2,Name = "李四",BillState = 0},
                new StudentForCustomUnBound{Id = 3,Sex = 2,Grade = 3,Name = "王五",BillState = 1},
                new StudentForCustomUnBound{Id = 4,Sex = 2,Grade = 4,Name = "张飞",BillState = 0}
            };
        }

        public ObservableCollection<IdNameObject> SexTypes
        {
            get => _sexTypes;
            set
            {
                if (_sexTypes == value) return;
                _sexTypes = value;
                RaisePropertyChanged(nameof(SexTypes));
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

        public ObservableCollection<StudentForCustomUnBound> Students
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

    public class StudentForCustomUnBound:ModelBase
    {
        private int _id;
        private int _sex;
        private int _grade;
        private string _name;
        private int _billState;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        public int Sex
        {
            get => _sex;
            set
            {
                if (_sex == value) return;
                _sex = value;
                RaisePropertyChanged(nameof(Sex));
            }
        }

        public int Grade
        {
            get => _grade;
            set
            {
                if (_grade == value) return;
                _grade = value;
                RaisePropertyChanged(nameof(Grade));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public int BillState
        {
            get => _billState;
            set
            {
                if (_billState == value) return;
                _billState = value;
                RaisePropertyChanged(nameof(BillState));
            }
        }
    }
}
