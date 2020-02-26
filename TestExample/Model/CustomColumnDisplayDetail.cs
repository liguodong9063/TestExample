namespace TestExample.Model
{
    public class CustomColumnDisplayDetail:ModelBase
    {
        private bool _isSelected;
        private string _columnChinaName;
        private string _columnEnglishName;
        private int _displayIndex;

        public int DisplayIndex
        {
            get { return _displayIndex; }
            set
            {
                if (_displayIndex == value) return;
                _displayIndex = value;
                RaisePropertyChanged("DisplayIndex");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public string ColumnChinaName
        {
            get { return _columnChinaName; }
            set
            {
                if (_columnChinaName == value) return;
                _columnChinaName = value;
                RaisePropertyChanged("ColumnChinaName");
            }
        }

        public string ColumnEnglishName
        {
            get { return _columnEnglishName; }
            set
            {
                if (_columnEnglishName == value) return;
                _columnEnglishName = value;
                RaisePropertyChanged("ColumnEnglishName");
            }
        }
    }
}
