using System.Collections.ObjectModel;
using System.Windows;
using TestExample.Infrastructure.Messages;
using TestExample.Infrastructure.Messages.Token;
using TestExample.Model;
using TestExample.ViewModel;

namespace TestExample.View
{
    /// <summary>
    /// CustomerConfigView.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerConfigView : Window
    {
        public CustomerConfigView(string operationType, ObservableCollection<CustomColumnDisplayDetail> columnDetailConfigs, object targetViewModel = null)
        {
            InitializeComponent();
            ((CustomerConfigViewModel)DataContext).OperationType = operationType;
            ((CustomerConfigViewModel)DataContext).ColumnDetailConfigs = columnDetailConfigs;
            ((CustomerConfigViewModel)DataContext).TargetViewModel = targetViewModel;
            Loaded += CustomerConfigView_Loaded;
            Unloaded += CustomerConfigView_Unloaded;
        }

        private void CustomerConfigView_Unloaded(object sender, RoutedEventArgs e)
        {
            UnregisterMessages();
        }

        private void CustomerConfigView_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterMessages();
        }

        protected void RegisterMessages()
        {
            CustomNotificationMessenger.Register<string>(this, NotificationMessageToken.CancelCustomerConfigView, message => Close());
        }

        protected void UnregisterMessages()
        {
            CustomNotificationMessenger.Unregister<string>(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = ((CustomerConfigViewModel)DataContext);
            dataContext.AllCheckChange();
            ColumnsConfig.RefreshData();
        }
    }
}