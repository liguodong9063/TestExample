using System;
using System.Windows.Threading;

namespace TestExample.View
{
    /// <summary>
    /// BaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBarWindow
    {

        public ProgressBarWindow()
        {
            InitializeComponent();
        }

        public static void SetProgressBarValue(ProgressBarWindow window,double theoryValue,string toolTip)
        {
            //window.Dispatcher.Invoke(() =>
            //{
            //    window.TestProgressBar.Value = theoryValue;
            //    window.TxtTip.Text = toolTip;
            //    if (theoryValue >= 100)
            //    {
            //        window.Close();
            //    }
            //},DispatcherPriority.Background);



            window.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                window.TestProgressBar.Value = theoryValue;
                window.TxtTip.Text = toolTip;
                if (theoryValue >= 100)
                {
                    window.Close();
                }
            }));
        }
    }
}
