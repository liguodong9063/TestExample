using System.Collections.ObjectModel;
using System.Windows;
using TestExample.Common;

namespace TestExample.View.TableView
{
    /// <summary>
    /// SelectableAndEditableColumnView.xaml 的交互逻辑
    /// </summary>
    public partial class SelectableAndEditableColumnView : Window
    {
        public SelectableAndEditableColumnView()
        {
            InitializeComponent();
            GridControl.ItemsSource = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"张三"),
                new IdNameObject(2,"李四"),
                new IdNameObject(3,"王五")
            };
        }
    }
}