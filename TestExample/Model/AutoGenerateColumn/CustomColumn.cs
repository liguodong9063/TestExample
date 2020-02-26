namespace TestExample.Model.AutoGenerateColumn
{
    public class CustomColumn : ModelBase
    {
        private string _fieldName;
        private CustomColumnValue _fieldValue;

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

        public CustomColumnValue FieldValue
        {
            get => _fieldValue;
            set
            {
                if (_fieldValue == value) return;
                _fieldValue = value;
                RaisePropertyChanged();
            }
        }
    }
}
