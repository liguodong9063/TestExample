using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TestExample.ViewModel.HotKey
{
    public class ShortCutViewModel:ViewModelBase
    {
        private ICommand _helloCommand;
        private ICommand _hello2Command;

        private KeyGesture _helloCommandKeyGesture;

        public ShortCutViewModel()
        {
            _helloCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Hello");
            }, () => true);

            _hello2Command=new RelayCommand(() =>
            {
                MessageBox.Show("Hello2");
            });

            _helloCommandKeyGesture = new KeyGesture(Key.H , ModifierKeys.Control | ModifierKeys.Alt);
            //ModifierKeys.Control + "+" + ModifierKeys.Alt + "+" + Key.H;
        }

        public ICommand HelloCommand => _helloCommand;

        public ICommand Hello2Command => _hello2Command;

        public KeyGesture HelloCommandKeyGesture
        {
            get => _helloCommandKeyGesture;
            set
            {
                if (_helloCommandKeyGesture == value) return;
                _helloCommandKeyGesture = value;
                RaisePropertyChanged(nameof(HelloCommandKeyGesture));
            }
        }
    }
}
