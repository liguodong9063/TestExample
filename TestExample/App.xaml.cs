using System;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Utils.Filtering.Internal;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using TestExample.Infrastructure.Localizers;

namespace TestExample
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeDevControlTheme();
        }

        private static void InitializeDevControlTheme()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo("zh-CHS");
            }));
            GridControlLocalizer.Active = new CustomerGridControlLoaclizer();
            EditorLocalizer.Active = new CustomerEditorLocalizer();

            FilterUIElementLocalizer.Active = new CustomerFilterUIElementLocalizer();
        }
    }
}
