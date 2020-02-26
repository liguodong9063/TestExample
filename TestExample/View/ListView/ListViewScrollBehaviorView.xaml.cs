using System.Collections.ObjectModel;
using System.Windows;
using TestExample.Model;

namespace TestExample.View.ListView
{
    /// <summary>
    /// ListViewScrollBehaviorView.xaml 的交互逻辑
    /// </summary>
    public partial class ListViewScrollBehaviorView : Window
    {
        public ListViewScrollBehaviorView()
        {
            InitializeComponent();

            ListView.ItemsSource = new ObservableCollection<TestCalss>
            {
                new TestCalss{Content="1",Color = "Red"},
                new TestCalss{Content="2",Color = "Yellow"},
                new TestCalss{Content="3",Color = "Blue"},
                new TestCalss{Content="4",Color = "Green"},
                new TestCalss{Content="5",Color = "Black"},
                new TestCalss{Content="6",Color = "Brown"},
                new TestCalss{Content="7",Color = "Teal"}
            };
        }

        private class TestCalss:ModelBase
        {
            private string _content;
            private string _color;

            public string Content
            {
                get => _content;
                set
                {
                    if (_content == value) return;
                    _content = value;
                    RaisePropertyChanged();
                }
            }

            public string Color
            {
                get => _color;
                set
                {
                    if (_color == value) return;
                    _color = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RequestBringIntoViewHandler_OnHandler(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}