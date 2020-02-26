using System.Windows;

namespace TestExample.UserControls
{
    /// <summary>
    /// SlidingControl.xaml 的交互逻辑
    /// </summary>
    public partial class SlidingControl
    {
        public SlidingControl()
        {
            InitializeComponent();
        }

        private double? _oldWidth;

        private void ViewPresenter_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ExecuteSizeChanged();
        }

        public void ExecuteSizeChanged()
        {
            var view = ViewPresenter;
            var actualWidth = view.ActualWidth;
            var dataContenxt = (SlidingControlViewModel)DataContext;
            if (dataContenxt.InitializeViewsAction == null || dataContenxt.ShowDoubleColumnViewAction == null ||
                dataContenxt.ShowThreeColumnViewAction == null)
            {
                return;
            }

            if (_oldWidth != null && ((!(_oldWidth < 1100) || !(actualWidth >= 1100 - 40)) && (!(_oldWidth >= 1100) || !(actualWidth < 1100 - 40)))) return;
            _oldWidth = actualWidth;
            view.StoryboardSelector = null;
            if (actualWidth < 1060)
            {
                //显示一页2个的页面
                dataContenxt.ShowDoubleColumnViewAction();
            }
            else
            {
                //显示一页三个的页面
                dataContenxt.ShowThreeColumnViewAction();
            }
            dataContenxt.ActiveView = dataContenxt.Views[0];
            view.StoryboardSelector = dataContenxt.StoryboardSelector;
        }
    }
}
