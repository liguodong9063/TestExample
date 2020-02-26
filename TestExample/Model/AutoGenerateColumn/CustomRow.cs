using System.Collections.Generic;

namespace TestExample.Model.AutoGenerateColumn
{

    public class CustomRow : ModelBase
    {
        private List<CustomColumn> _columns;

        public List<CustomColumn> Columns
        {
            get => _columns;
            set
            {
                if (_columns == value) return;
                _columns = value;
                RaisePropertyChanged();
            }
        }
    }
}
