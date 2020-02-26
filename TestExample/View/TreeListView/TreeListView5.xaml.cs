using System.Collections;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using TestExample.Model;
using TestExample.ViewModel.TreeListView;

namespace TestExample.View.TreeListView
{
    /// <summary>
    /// TreeListView4.xaml 的交互逻辑
    /// </summary>
    public partial class TreeListView5 : Window
    {
        public TreeListView5()
        {
            InitializeComponent();
        }

        private void TreeListView_OnNodeExpanded(object sender, TreeListNodeEventArgs e)
        {
            var dataContext = (TreeListView5ViewModel) DataContext;
            dataContext.ExplandHeaderControlViewModel.ResetExpandState();
        }

        private void TreeListView_OnNodeCollapsed(object sender, TreeListNodeEventArgs e)
        {
            var dataContext = (TreeListView5ViewModel)DataContext;
            dataContext.ExplandHeaderControlViewModel.ResetExpandState();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = (TreeListView5ViewModel)DataContext;
            dataContext.Subjects.Remove(dataContext.Subjects[0]);
            dataContext.ExplandHeaderControlViewModel.ResetExpandState(true);
        }
    }

    public class SubjectNodeChildSelector : IChildNodesSelector
    {
        IEnumerable IChildNodesSelector.SelectChildren(object item)
        {
            return (item as SubjectObjectForTreeListView5)?.Childrens;
        }
    }

    public class TreeBase<T>: ModelBase
    {
        private IList<T> _childrens;
        private bool _isExpanded;

        public TreeBase()
        {
            _childrens = new List<T>();
        }

        public IList<T> Childrens
        {
            get { return _childrens; }
            set
            {
                if (_childrens == value) return;
                _childrens = value;
                RaisePropertyChanged("Childrens");
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }
    }

}
