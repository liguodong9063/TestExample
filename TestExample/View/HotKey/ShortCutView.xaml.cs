using System.Windows;
using System.Windows.Input;
using TestExample.ViewModel.HotKey;

namespace TestExample.View.HotKey
{
    /// <summary>
    /// ShortCutView.xaml 的交互逻辑
    /// </summary>
    public partial class ShortCutView : Window
    {
        public ShortCutView()
        {
            InitializeComponent();

            var dataContext = (ShortCutViewModel) DataContext;
            //模式4
            var helloKeyGesture = new KeyGesture(Key.Decimal, ModifierKeys.Control);
            var helloKeyBinding = new KeyBinding(dataContext.Hello2Command, helloKeyGesture);
            InputBindings.Add(helloKeyBinding);
        }

        private void CommandBinding1_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding1_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.FontSize += 5;
        }

        private void CommandBinding2_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.FontSize >= 5;
        }

        private void CommandBinding2_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.FontSize -= 5;
        }
    }
}
