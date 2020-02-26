using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using TestExample.Model;

namespace TestExample.ViewModel.LookUpEdit
{
    public class LookUpEditViewModel:ViewModelBase
    {

        private ObservableCollection<Staff> _staffs;
        private Staff _selectedStaff;

        public LookUpEditViewModel()
        {
            _staffs=new ObservableCollection<Staff>
            {
                new Staff{Id = 2,Name = "张三1"},
                new Staff{Id = 1,Name = "李四2"},
                new Staff{Id = 3,Name = "王五3"}
            };
        }

        public ObservableCollection<Staff> Staffs
        {
            get => _staffs;
            set
            {
                if (_staffs == value) return;
                _staffs = value;
                RaisePropertyChanged("Staffs");
            }
        }

        public Staff SelectedStaff
        {
            get => _selectedStaff;
            set
            {
                if (_selectedStaff == value) return;
                _selectedStaff = value;
                RaisePropertyChanged();
            }
        }
    }

    public class Staff: ModelBase
    {
        private int _id;
        private string _name;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
    }
}
