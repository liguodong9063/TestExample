using System.ComponentModel;
using Newtonsoft.Json;

namespace TestExample.Model.AutoGenerateColumn
{
    public class CustomColumnValue : IDataErrorInfo
    {
        public int? Id { get; set; }
        public string Value { get; set; }
        public int? AccountingTypeId { get; set; }
        public int OperationType { get; set; }

        [JsonIgnore]
        public string Error => null;

        public string this[string propertyName] => IsValid(propertyName);

        private string IsValid(string propertyName)
        {
            switch (propertyName)
            {
                case "Value":
                    if (Id == null && AccountingTypeId != null)
                        return "无效的类别！";
                    break;
            }
            return null;
        }
    }
}
