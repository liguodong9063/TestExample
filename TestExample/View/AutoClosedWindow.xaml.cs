using System;
using System.Windows;
using System.Windows.Threading;

namespace TestExample.View
{
    /// <summary>
    /// AutoClosedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AutoClosedWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public AutoClosedWindow(string msg)
        {
            InitializeComponent();
            this.msg.Text = msg;
            timer.Interval = new TimeSpan(0, 0, 0,1,500);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }
    }
}
