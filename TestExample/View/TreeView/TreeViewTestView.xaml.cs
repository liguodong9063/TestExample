using System.Collections.ObjectModel;
using System.Windows;
using TestExample.Common;

namespace TestExample.View.TreeView
{
    /// <summary>
    /// TreeViewTestView.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewTestView : Window
    {
        public TreeViewTestView()
        {
            InitializeComponent();
            TreeView.ItemsSource = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"张三"),
                new IdNameObject(2,"李四")
            };
        }
    }
}
