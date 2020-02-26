using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using DevExpress.Xpf.Grid;
using TestExample.Model;

namespace TestExample.View.TreeListView
{
    /// <summary>
    /// TreeListViewOperatorView.xaml 的交互逻辑
    /// </summary>
    public partial class TreeListViewOperatorView : Window
    {
        public TreeListViewOperatorView()
        {
            InitializeComponent();
        }
    }

    public class TreeListObject:ModelBase
    {
        private string _pathCode;
        private string _name;
        private ObservableCollection<TreeListObject> _childrens;

        public string PathCode
        {
            get => _pathCode;
            set
            {
                if (_pathCode == value) return;
                _pathCode = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TreeListObject> Childrens
        {
            get => _childrens;
            set
            {
                if (_childrens == value) return;
                _childrens = value;
                RaisePropertyChanged();
            }
        }
    }

    public class TreeListViewChildNodeSelector : IChildNodesSelector
    {
        IEnumerable IChildNodesSelector.SelectChildren(object item)
        {
            return (item as TreeListObject)?.Childrens;
        }
    }
}
