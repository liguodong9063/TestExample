using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using GalaSoft.MvvmLight.Command;

namespace TestExample.UserControls
{
    public class SlidingControlViewModel : ViewModelBase
    {
        private UserControl _activeView;
        private UserControl _prevView;
        private bool _prevButtonEnable;
        private bool _nextButtonEnable;
        private Visibility _prevNextButtonVisibility;

        public UserControl ActiveView
        {
            get => _activeView;

            set
            {
                _prevView = _activeView;
                _activeView = value;
                RaisePropertyChanged("ActiveView");
            }
        }

        public bool PrevButtonEnable
        {
            get => _prevButtonEnable;
            set
            {
                if (_prevButtonEnable == value) return;
                _prevButtonEnable = value;
                RaisePropertyChanged("PrevButtonEnable");
            }
        }

        public bool NextButtonEnable
        {
            get => _nextButtonEnable;
            set
            {
                if (_nextButtonEnable == value) return;
                _nextButtonEnable = value;
                RaisePropertyChanged("NextButtonEnable");
            }
        }

        public Visibility PrevNextButtonVisibility
        {
            get => _prevNextButtonVisibility;
            set
            {
                if (_prevNextButtonVisibility == value) return;
                _prevNextButtonVisibility = value;
                RaisePropertyChanged("PrevNextButtonVisibility");
            }
        }

        public Action InitializeViewsAction;
        public Action ShowDoubleColumnViewAction;
        public Action ShowThreeColumnViewAction;

        public List<UserControl> AllViews = new List<UserControl>();

        public List<UserControl> Views = new List<UserControl>();

        public ICommand NextCommand => new RelayCommand<object>((p) =>
            {
                ActiveView = Views[(Views.IndexOf(ActiveView) + 1) % 2];
                ResetNextPrevButtonEnableProperty();
            }
        );

        public ICommand PrevCommand => new RelayCommand<object>((p) =>
        {
            ActiveView = Views[(Views.IndexOf(ActiveView) - 1) == -1 ? 0 : (Views.IndexOf(ActiveView) - 1)];
            ResetNextPrevButtonEnableProperty();
        });

        public Func<object, string> StoryboardSelector => (newModuleView) => Views.IndexOf((UserControl)newModuleView) < Views.IndexOf(_prevView) ? "FromLeft" : "FromRight";

        private void ResetNextPrevButtonEnableProperty()
        {
            PrevButtonEnable = Views.IndexOf(ActiveView) != 0;
            NextButtonEnable = Views.IndexOf(ActiveView) != 1;
        }
    }
}
