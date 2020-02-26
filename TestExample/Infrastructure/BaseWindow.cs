using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestExample.Infrastructure
{
    public partial class BaseWindow : Window
    {
        public BaseWindow()
        {
            Closed += Window_Closed;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
        }

        protected void Window_Closed(object sender, EventArgs e)
        {
            //容器Grid
            var grid = Owner.Content as Grid;
            //父级窗体原来的内容 
            var original = VisualTreeHelper.GetChild(grid, 1) as UIElement;
            //将父级窗体原来的内容在容器Grid中移除 
            grid.Children.Remove(original);
            //赋给父级窗体 
            Owner.Content = grid;
        }

        public void ShowDialog2(Window owner)
        {
            //蒙板 
            var layer = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0))
            };
            //父级窗体原来的内容 
            var original = owner.Content as UIElement;
            owner.Content = null;
            //容器Grid 
            var container = new Grid();
            container.Children.Add(original);
            //放入原来的内容 
            container.Children.Add(layer);
            //在上面放一层蒙板 
            //将装有原来内容和蒙板的容器赋给父级窗体 
            owner.Content = container;
            Owner = owner;
            ShowDialog();
        }
    }
}
