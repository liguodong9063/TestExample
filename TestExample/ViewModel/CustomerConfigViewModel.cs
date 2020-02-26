using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TestExample.Infrastructure.Messages;
using TestExample.Infrastructure.Messages.Token;
using TestExample.Model;

namespace TestExample.ViewModel
{
    public class CustomerConfigViewModel : ViewModelBase
    {
        //private readonly ICustomColumnDisplayService _customColumnDisplayService;
        private ObservableCollection<CustomColumnDisplayDetail> _columnDetailConfigs= new ObservableCollection<CustomColumnDisplayDetail>();

        //public CustomerConfigViewModel(IDialogService dialogService, IContextService contextService, ICustomColumnDisplayService customColumnDisplayService)
        //    : base(dialogService, contextService)
        //{
        //    _customColumnDisplayService = customColumnDisplayService;
        //}

        public ObservableCollection<CustomColumnDisplayDetail> ColumnDetailConfigs
        {
            get => _columnDetailConfigs;
            set
            {
                if (_columnDetailConfigs == value) return;
                _columnDetailConfigs = value;
                RaisePropertyChanged("ColumnDetailConfigs");
            }
        }

        /// <summary>
        /// 页面对应的DataContext
        /// </summary>
        public object TargetViewModel { get; set; }

        public ICommand AllCheckChangeCommand => new RelayCommand(AllCheckChange);

        public ICommand SubmitCommand => new RelayCommand(Submit, CanSubmit);

        public ICommand CancelCommand => new RelayCommand(Cancel);

        public string OperationType { get; set; }

        private bool CanSubmit()
        {
            var customColumnDisplayDetails = new ObservableCollection<CustomColumnDisplayDetail>(_columnDetailConfigs.Where(a => a.IsSelected == false));
            return customColumnDisplayDetails.Count > 0 || customColumnDisplayDetails.Count == 0;
        }

        public void AllCheckChange()
        {
            foreach (CustomColumnDisplayDetail customColumnDisplayDetail in _columnDetailConfigs)
            {
                if (customColumnDisplayDetail.IsSelected) continue;
                customColumnDisplayDetail.IsSelected = true;
            }
        }

        //todo:临时注释
        private void Submit()
        {
            //var customColumnDisplayDetails = new ObservableCollection<CustomColumnDisplayDetail>(_columnDetailConfigs.Where(a => a.IsSelected == false));
            //var noShowColu = customColumnDisplayDetails.Aggregate(string.Empty, (current, customColumnDisplayDetail) => current + customColumnDisplayDetail.ColumnEnglishName + ",");
            //if (noShowColu.Length > 0)
            //    noShowColu = noShowColu.Substring(0, noShowColu.Length - 1);

            //try
            //{
            //    var newColumnConfig = new CustomColumnDispaly();
            //    var customColumnDispaly = _customColumnDisplayService.GetCurrentUserCustomColumnDispaly(OperationType);
            //    if (customColumnDispaly != null)
            //    {
            //        newColumnConfig = customColumnDispaly;
            //        newColumnConfig.NoShowColu = noShowColu;
            //    }
            //    else
            //    {
            //        newColumnConfig.ColuType = OperationType;
            //        newColumnConfig.NoShowColu = noShowColu;
            //        Debug.Assert(UserPrincipal.Identity.User.Id != null);
            //        newColumnConfig.UserId = UserPrincipal.Identity.User.Id.Value;
            //    }

            //    var configInformation = _customColumnDisplayService.SaveCustomColumnDispaly(newColumnConfig);
            //    Cancel();
            //    var method = TargetViewModel.GetType().GetMethod("SetColumnDisplayConfigurations");
            //    if (method == null)
            //    {
            //        DialogService.ShowMessage("未找到公共的自定义列显示设置方法！", ShowType.Warning);
            //        return;
            //    }
            //    method.Invoke(TargetViewModel, new object[] { configInformation });

            //}
            //catch (Exception)
            //{
            //    DialogService.ShowMessage(NotificationMessageResource.OperationFailed, ShowType.Warning);
            //}
        }

        private void Cancel()
        {
            CustomNotificationMessenger.Send(string.Empty,string.Empty, NotificationMessageToken.CancelCustomerConfigView);
        }
    }
}
