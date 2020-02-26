using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TestExample.Utilities;

namespace TestExample.UserControls
{
    public enum SlidingModeType
    {
        [Description("像素")]
        Pixel,
        [Description("行")]
        Row
    }

    /// <summary>
    /// 自定义ListView
    /// ScrollViewer.CanContentScroll="False"：按像素滚动，不支持虚拟化，数据大时卡顿
    /// ScrollViewer.CanContentScroll="True"：按逻辑单元滚动（几行几行的滚）
    /// ScrollViewer.CanContentScroll="True"&& VirtualizingPanel.ScrollUnit="Pixel"：按像素滚动并支持虚拟化（建议方式）
    ///
    /// 如想禁用显示一半时选中自动滚动行为，需在ListViewItem设置：<EventSetter Event="RequestBringIntoView" Handler="RequestBringIntoViewHandler_OnHandler"/>
    /// </summary>
    public class CustomListView:ListView
    {
        /// <summary>
        /// 滑动模式（按像素或者按行）
        /// </summary>
        public SlidingModeType CustomSlidingMode { get; set; } = SlidingModeType.Pixel;
        /// <summary>
        /// 行模式单位（每次滑动多次行）
        /// </summary>
        public int CustomRowModeUnit { get; set; } = 2;
        /// <summary>
        /// 像素模式单位（每次滑动多少像素）
        /// </summary>
        public int CustomPixelModeUnit { get; set; } = 10;

        private ScrollViewer _scrollViewer;

        public CustomListView()
        {
            PreviewMouseWheel += CustomListView_PreviewMouseWheel;
            Loaded += CustomListView_Loaded;
        }

        private void CustomListView_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = VisualSearchHelper.GetChildObject<ScrollViewer>(this, string.Empty);
            ScrollViewer.SetHorizontalScrollBarVisibility(this,ScrollBarVisibility.Disabled);
            ScrollViewer.SetCanContentScroll(this,true);
            VirtualizingPanel.SetIsVirtualizing(this,true);
            //设置ScrollUnit
            ScrollUnit scrollUnit;
            switch (CustomSlidingMode)
            {
                case SlidingModeType.Pixel:
                    scrollUnit = ScrollUnit.Pixel;
                    break;
                case SlidingModeType.Row:
                    scrollUnit = ScrollUnit.Item;
                    break;
                default:
                    scrollUnit = ScrollUnit.Pixel;
                    break;
            }
            VirtualizingPanel.SetScrollUnit(this, scrollUnit);
        }

        private void CustomListView_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var isSlidingByPixel = false;
            var canContentScroll = ScrollViewer.GetCanContentScroll(this);
            if (canContentScroll)
            {
                var scrollUnit = VirtualizingPanel.GetScrollUnit(this);
                if (scrollUnit == ScrollUnit.Pixel)
                {
                    isSlidingByPixel = true;
                }
            }
            if (isSlidingByPixel)
            {
                if (e.Delta > 0)
                {
                    //向上
                    _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - CustomPixelModeUnit);
                }
                else if (e.Delta < 0)
                {
                    //向下
                    _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + CustomPixelModeUnit);
                }
            }
            else
            {
                if (e.Delta > 0)
                {
                    //向上
                    _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - CustomRowModeUnit);
                }
                else if (e.Delta < 0)
                {
                    //向下
                    _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + CustomRowModeUnit);
                }
            }
            e.Handled = true;
        }
    }
}