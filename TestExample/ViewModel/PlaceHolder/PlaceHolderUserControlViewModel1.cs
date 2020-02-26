using GalaSoft.MvvmLight;

namespace TestExample.ViewModel.PlaceHolder
{
    public class PlaceHolderUserControlViewModel1: ViewModelBase
    {
        private string _title="我是Title1";

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
    }
}
