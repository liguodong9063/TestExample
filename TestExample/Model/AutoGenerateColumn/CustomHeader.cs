namespace TestExample.Model.AutoGenerateColumn
{
    public class CustomHeader : ModelBase
    {
        private string _fieldName;
        private string _headerName;
        private int? _accountingTypeId;

        public string HeaderName
        {
            get => _headerName;
            set
            {
                if (_headerName == value) return;
                _headerName = value;
                RaisePropertyChanged();
            }
        }

        public string FieldName
        {
            get => _fieldName;
            set
            {
                if (_fieldName == value) return;
                _fieldName = value;
                RaisePropertyChanged();
            }
        }

        public int? AccountingTypeId
        {
            get => _accountingTypeId;
            set
            {
                if (_accountingTypeId == value) return;
                _accountingTypeId = value;
                RaisePropertyChanged();
            }
        }
    }
}
