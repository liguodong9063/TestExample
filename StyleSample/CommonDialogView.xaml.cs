using System;
using System.Windows;
using System.Windows.Input;

namespace StyleSample
{
    /// <summary>
    /// CommonDialogView.xaml 的交互逻辑
    /// </summary>
    public partial class CommonDialogView
    {
        public CommonDialogView(Uri uri)
        {
            InitializeComponent();
            MainContent.NavigationService.Navigate(uri);
        }

        private void WindowHeader_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void YesButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}