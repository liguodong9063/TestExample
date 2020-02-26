using System.Windows;

namespace TestExample.View.Other
{
    /// <summary>
    /// FileMultiSelectView.xaml 的交互逻辑
    /// </summary>
    public partial class FileMultiSelectView : Window
    {
        public FileMultiSelectView()
        {
            InitializeComponent();
        }

        private void FileUpload_OnClick(object sender, RoutedEventArgs e)
        {
            var openFile = new System.Windows.Forms.OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                //Filter = "All files (*.*)|*.*"
                Filter = "Excel (*.xlsx)|*.xlsx"
            };
            openFile.ShowDialog();
        }
    }
}
