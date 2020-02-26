using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TestExample.UserControls;

namespace TestExample.View.SlideView
{
    /// <summary>
    /// SlideView1.xaml 的交互逻辑
    /// </summary>
    public partial class SlideView1
    {
        public SlideView1()
        {
            InitializeComponent();
        }

        private void SlidingControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataContext = ((SlidingControlViewModel)SlidingControl.DataContext);
            dataContext.InitializeViewsAction = InitializeDataSources;
            dataContext.ShowDoubleColumnViewAction = () =>
            {
                dataContext.Views = new List<UserControl>
                {
                    dataContext.AllViews[1],
                    dataContext.AllViews[2]
                };
                dataContext.PrevNextButtonVisibility = Visibility.Visible;
                dataContext.PrevButtonEnable = false;
                dataContext.NextButtonEnable = true;
            };
            dataContext.ShowThreeColumnViewAction = () =>
            {
                dataContext.Views = new List<UserControl>()
                {
                    dataContext.AllViews[0]
                };
                dataContext.PrevNextButtonVisibility = Visibility.Collapsed;
                dataContext.PrevButtonEnable = false;
                dataContext.NextButtonEnable = false;
            };
            //初始化View
            dataContext.InitializeViewsAction();
            //执行SizeChanged对应的方法
            SlidingControl.ExecuteSizeChanged();

        }

        private void InitializeDataSources()
        {
            var dataContext = ((SlidingControlViewModel)SlidingControl.DataContext);
            dataContext.AllViews = new List<UserControl>()
            {
                new UserControl0(),
                new UserControl1(),
                new UserControl2()
            };
            var orderInfo = new OrderInfoDto
            {
                DepartmentName = "市场一部",
                EntName = "湖南联创控股集团有限公司",
                PaymentType = "电汇",
                SaleType = "市场零售",
                SettleWay = "到期付款"
            };
            dataContext.AllViews[0].DataContext = orderInfo;
            dataContext.AllViews[1].DataContext = orderInfo;
            dataContext.AllViews[2].DataContext = orderInfo;
        }
    }

    public class OrderInfoDto
    {
        public string EntName { get; set; }
        public string SettleWay { get; set; }
        public string PaymentType { get; set; }
        public string DepartmentName { get; set; }
        public string SaleType { get; set; }
    }
}
