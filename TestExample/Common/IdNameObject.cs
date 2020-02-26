using Newtonsoft.Json;
using System.ComponentModel;
using TestExample.Model;

namespace TestExample.Common
{
    public class IdNameObject:ModelBase, IDataErrorInfo
    {
        public IdNameObject()
        {
        }

        public IdNameObject(int? id, string name)
        {
            Id = id;
            Name = name;
            IsUse = true;
        }

        public IdNameObject(int? id, string name ,bool isUse)
        {
            Id = id;
            Name = name;
            IsUse = isUse;
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsUse { get; set; }

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
