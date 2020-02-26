using System.Collections.ObjectModel;
using System.Windows;

namespace TestExample.View.TableView
{
    /// <summary>
    /// GridControlBorderView.xaml 的交互逻辑
    /// </summary>
    public partial class GridControlBorderView : Window
    {
        public GridControlBorderView()
        {
            InitializeComponent();

            GridControl.ItemsSource = new ObservableCollection<TestClass>
            {
                new TestClass{Column1 = "列1",Column2 = "列2",Column3 = "列3"},
                new TestClass{Column1 = "列1",Column2 = "列2",Column3 = "列3"},
                new TestClass{Column1 = "列1",Column2 = "列2",Column3 = "列3"},
                new TestClass{Column1 = "列1",Column2 = "列2",Column3 = "列3"}
            };
        }

        private class TestClass
        {
            public string Column1 { get; set; }
            public string Column2 { get; set; }
            public string Column3 { get; set; }
        }
    }
}
