using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using TestExample.Common;

namespace TestExample.ViewModel.ComboBox
{
    public class ScrollViewWithComboBoxViewModel: ViewModelBase
    {
        private ObservableCollection<IdNameObject> _treeViewItemSource;
        private ObservableCollection<IdNameObject> _comboBoxItemSource;

        public ScrollViewWithComboBoxViewModel()
        {
            _treeViewItemSource = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"学校一"),
                new IdNameObject(2,"学校二"),
                new IdNameObject(3,"学校三"),
                new IdNameObject(4,"学校四"),
                new IdNameObject(5,"学校五"),
                new IdNameObject(6,"学校六"),
                new IdNameObject(7,"学校七"),
                new IdNameObject(8,"学校八"),
                new IdNameObject(9,"学校九")
            };
            _comboBoxItemSource = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"张三"),
                new IdNameObject(2,"李四"),
                new IdNameObject(3,"王五"),
                new IdNameObject(4,"张勇"),
                new IdNameObject(5,"汤龙"),
                new IdNameObject(6,"龚尧"),
                new IdNameObject(7,"王亚"),
                new IdNameObject(8,"刘雅"),
                new IdNameObject(9,"喻涵"),
                new IdNameObject(10,"王学欢"),
                new IdNameObject(11,"张靖"),
                new IdNameObject(12,"张三丰"),
                new IdNameObject(13,"张无忌"),
                new IdNameObject(14,"李白"),
                new IdNameObject(15,"张奎"),
                new IdNameObject(16,"武松"),
                new IdNameObject(17,"王丹")
            };
        }

        public ObservableCollection<IdNameObject> TreeViewItemSource
        {
            get => _treeViewItemSource;
            set
            {
                if (_treeViewItemSource == value) return;
                _treeViewItemSource = value;
                RaisePropertyChanged(nameof(TreeViewItemSource));
            }
        }

        public ObservableCollection<IdNameObject> ComboBoxItemSource
        {
            get => _comboBoxItemSource;
            set
            {
                if (_comboBoxItemSource == value) return;
                _comboBoxItemSource = value;
                RaisePropertyChanged(nameof(ComboBoxItemSource));
            }
        }
    }
}
