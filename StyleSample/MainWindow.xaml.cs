using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StyleSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void JumbPage_OnClick(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("Pages/Page1.xaml", UriKind.Relative);
            var window = new CommonDialogView(uri);
            window.ShowDialog();
        }

        private void JumbPage2_OnClick(object sender, RoutedEventArgs e)
        {
            //var window = new CommonDialogView("Page2.xaml");
            //window.ShowDialog();
        }
    }
}
