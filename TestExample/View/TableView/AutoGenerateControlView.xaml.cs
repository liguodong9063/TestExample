using System.Windows;
using TestExample.Common;
using TestExample.ViewModel.TableView;
using WpfControls.Editors;

namespace TestExample.View.TableView
{
    /// <summary>
    /// AutoGenerateControlView.xaml 的交互逻辑
    /// </summary>
    public partial class AutoGenerateControlView : Window
    {
        public AutoGenerateControlView()
        {
            InitializeComponent();

            TT.CanTrigger = () => !_isLoading;
        }

        //private void AccountingItemGridControl_OnItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        //{
        //    AccountingItemGridControl.Height = (AccountingItemGridControl.ItemsSource as ObservableCollection<AccountingItemDto>)?.Count * 40 ?? 0;
        //}

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as AutoGenerateControlViewModel;
            var info = string.Empty;
            //foreach (var dataContextAccountingItem in dataContext.AccountingItems)
            //{
            //    info += dataContextAccountingItem.TypeName + ":" + dataContextAccountingItem.SelectedDataItem?.Name +";";
            //}

            MessageBox.Show(dataContext.ValidationDto.Name);
        }

        private bool _isLoading;

        //private void AutoComplete_OnCustomSelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    var control = sender as AutoCompleteTextBox;
        //    TestTip.Text = control.Editor.Text;
        //}

        private void ChangeSource_OnClick(object sender, RoutedEventArgs e)
        {
            _isLoading = true;
            var dataContext = DataContext as AutoGenerateControlViewModel;
            dataContext.AccountingItems[1].InputedText = "李四";

            dataContext.ValidationDto.Name = "张三";
            dataContext.ValidationDto.Name = "李四";
            
            //dataContext.ValidationDto = new TestValidationDto
            //{
            //    Name = "李四";
            //};

            _isLoading = false;
        }

        /// <summary>
        /// 注：该事件请配合Bind方法使用，再Bind中触发的不要执行该时间，看需求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoComplete_OnCustomSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!_isLoading)
            {
                var control = sender as AutoCompleteTextBox;
                TestTip.Text = $"name:{(control.SelectedItem as AccountingItemDataDto)?.Name};id:{(control.SelectedItem as AccountingItemDataDto)?.Id};";
            }
        }
    }
}
