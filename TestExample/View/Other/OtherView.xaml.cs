using System.Windows;

namespace TestExample.View.Other
{
    /// <summary>
    /// OtherView.xaml 的交互逻辑
    /// </summary>
    public partial class OtherView : Window
    {
        public OtherView()
        {
            InitializeComponent();
        }

        private void ConvertPinyinButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pinyin = ConvertToPinYin("长高");
        }

        private void OpenCaculatorButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
            Info.FileName = "calc.exe ";//"calc.exe"为计算器，"notepad.exe"为记事本
            System.Diagnostics.Process Proc = System.Diagnostics.Process.Start(Info);
        }

        private void OpenNotepadButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
            Info.FileName = "notepad.exe ";//"calc.exe"为计算器，"notepad.exe"为记事本
            System.Diagnostics.Process Proc = System.Diagnostics.Process.Start(Info);
        }

        private (string Pinyin, string PinyinSimple) ConvertToPinYin(string chineseStr)
        {
            var pinYin = "";
            var pinYinSimple = "";
            foreach (var str in chineseStr)
            {
                if (Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar(str))
                {
                    var cc = new Microsoft.International.Converters.PinYinConverter.ChineseChar(str);
                    var pinYinFull = cc.Pinyins[0];
                    pinYin += pinYinFull.Substring(0, pinYinFull.Length - 1);
                    pinYinSimple += pinYinFull.Substring(0, 1);
                }
                else
                {
                    pinYin += str.ToString();
                    pinYinSimple += str.ToString().Substring(0, 1);
                }
            }
            return (pinYin, pinYinSimple);
        }
    }
}
