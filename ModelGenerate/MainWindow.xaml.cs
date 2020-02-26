using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using ModelGenerate.Model;
using Newtonsoft.Json;

namespace ModelGenerate
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

        private void LoadSource_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Title = "选择文件", Filter = "文件|*.txt" };
            if (openFileDialog.ShowDialog() != true)
            {
                e.Handled = true;
                return;
            }
            var fileName = openFileDialog.FileName.Trim();
            var json = string.Empty;

            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    string strLine = sr.ReadLine(); //  读取一行字符并返回
                    while (strLine != null)
                    {
                        json += strLine;
                        strLine = sr.ReadLine();
                    }
                }
                var classDto = JsonConvert.DeserializeObject<ClassDto>(json);
                if (classDto != null)
                {
                    var dataContext = (MainWindowViewModel) DataContext;
                    dataContext.ClassName = classDto.ClassName;
                    dataContext.NameSpace = classDto.NameSpace;
                    dataContext.ModelBaseChecked = classDto.BaseClassEnabled;
                    dataContext.DataErrorInfoChecked = classDto.InterfaceEnabled;
                    dataContext.Mode1Checked = classDto.Mode1Checked;
                    dataContext.Mode2Checked = classDto.Mode2Checked;
                    dataContext.Mode3Checked = classDto.Mode3Checked;
                    dataContext.SelectedProperty = null;
                    dataContext.Properties = new ObservableCollection<PropertyDto>(classDto.PropertyDtos);
                }
                else
                {
                    MessageBox.Show("源文件内容为空！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
