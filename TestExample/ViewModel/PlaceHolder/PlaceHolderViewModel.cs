using GalaSoft.MvvmLight;

namespace TestExample.ViewModel.PlaceHolder
{
    public class PlaceHolderViewModel: ViewModelBase
    {
        private PlaceHolderUserControlViewModel1 _viewModel1;
        private PlaceHolderUserControlViewModel2 _viewModel2;

        public PlaceHolderUserControlViewModel1 ViewModel1
        {
            get => _viewModel1;
            set
            {
                if (_viewModel1 == value) return;
                _viewModel1 = value;
                RaisePropertyChanged("ViewModel1");
            }
        }

        public PlaceHolderUserControlViewModel2 ViewModel2
        {
            get => _viewModel2;
            set
            {
                if (_viewModel2 == value) return;
                _viewModel2 = value;
                RaisePropertyChanged("ViewModel2");
            }
        }

        public PlaceHolderViewModel()
        {
            _viewModel1=new PlaceHolderUserControlViewModel1();
            _viewModel2=new PlaceHolderUserControlViewModel2();
        } 
    }
}
