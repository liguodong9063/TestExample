using Newtonsoft.Json;
using System.ComponentModel;

namespace TestExample.Model.AutoCompleteTextBox
{
    public class AutoCompleteIdNameDto: ModelBase,IDataErrorInfo
    {
        private int? _id;
        private string _name;

        public int? Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
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

        [JsonIgnore]
        public string Error => null;

        public string this[string propertyName] => IsValid(propertyName);

        private string IsValid(string propertyName)
        {
            switch (propertyName)
            {
                case "Name":
                    if (string.IsNullOrEmpty(Name))
                        return "名称不能为空！";
                    break;
            }
            return null;
        }
    }
}
