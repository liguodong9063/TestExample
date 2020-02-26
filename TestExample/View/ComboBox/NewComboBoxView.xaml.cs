using System.Windows;
using TestExample.ViewModel.ComboBox;

namespace TestExample.View.ComboBox
{
    /// <summary>
    /// ComboBoxInGridColumnView.xaml 的交互逻辑
    /// </summary>
    public partial class NewComboBoxView : Window
    {
        public NewComboBoxView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = (NewComboBoxViewModel) DataContext;
            //dataContext.ChangeSources();
            dataContext.ChangeSources();
        }
    }
}
