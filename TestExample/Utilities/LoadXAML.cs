using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace TestExample.Utilities
{
    public class LoadXAML: Window
    {
        public LoadXAML(string xamlFile)
        {
            Width = Height = 285;
            Left = Top = 100;
            Title = "动态加载XAML";
            DependencyObject rootElement;

            using (var fs = new FileStream(xamlFile, FileMode.Open))
            {
                rootElement = (DependencyObject)XamlReader.Load(fs);
            }
            Content = rootElement;
            //查找控件(方法一)
            //button1 = (Button) LogicalTreeHelper.FindLogicalNode(rootElement, "button1");
            //查找控件(方法二)
            //FrameworkElement frameworkElement = (FrameworkElement)rootElement;
            //button1 = (Button)frameworkElement.FindName("button1");
            //button1.Click += button1_Click;
        }
    }
}
