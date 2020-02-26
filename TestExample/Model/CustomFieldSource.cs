using System.ComponentModel;
using Newtonsoft.Json;

namespace TestExample.Model
{
    public class CustomFieldSource : ModelBase, IDataErrorInfo
    {
        private int? _id;
        private string _name;
        private int _operationType;
        //private int? _isUse;
        private int? _itmTypeId;
        private string _remark;
        private int? _customFieldId;
        private int _version;
        private string _itemNo;
        private string _itemTypeName;
        private bool _isChecked;
        public CustomFieldSource()
        {
            //IsUse = 1;
        }

        [JsonIgnore]
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
        /// <summary>
        ///     主键
        /// </summary>
        [JsonProperty("id")]
        public int? Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        ///     数据源名称
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        ///     操作类型
        /// </summary>
        [JsonProperty("operationType")]
        public int OperationType
        {
            get { return _operationType; }
            set
            {
                if (_operationType == value) return;
                _operationType = value;
                RaisePropertyChanged("OperationType");
            }
        }

        ///// <summary>
        /////     是否启用
        ///// </summary>
        //[JsonProperty("isUse")]
        //public int? IsUse
        //{
        //    get { return _isUse; }
        //    set
        //    {
        //        if (_isUse == value) return;
        //        _isUse = value;
        //        RaisePropertyChanged("IsUse");
        //    }
        //}

        /// <summary>
        ///     产品类别ID
        /// </summary>
        [JsonProperty("itmTypeId")]
        public int? ItmTypeId
        {
            get { return _itmTypeId; }
            set
            {
                if (_itmTypeId == value) return;
                _itmTypeId = value;
                RaisePropertyChanged("ItmTypeId");
            }
        }

        /// <summary>
        ///     备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark == value) return;
                _remark = value;
                RaisePropertyChanged("Remark");
            }
        }

        /// <summary>
        ///     预留字段ID
        /// </summary>
        [JsonProperty("reserveField")]
        public int? CustomFieldId
        {
            get { return _customFieldId; }
            set
            {
                if (_customFieldId == value) return;
                _customFieldId = value;
                RaisePropertyChanged("CustomFieldId");
            }
        }

        /// <summary>
        ///     版本号
        /// </summary>
        [JsonProperty("version")]
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version == value) return;
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        /// <summary>
        ///     项号（仅用于.net端显示）
        /// </summary>
        [JsonIgnore]
        public string ItemNo
        {
            get { return _itemNo; }
            set
            {
                if (_itemNo == value) return;
                _itemNo = value;
                RaisePropertyChanged("ItemNo");
            }
        }

        /// <summary>
        ///     产品类型名称
        /// </summary>
        [JsonIgnore]
        public string ItemTypeName
        {
            get { return _itemTypeName; }
            set
            {
                if (_itemTypeName == value) return;
                _itemTypeName = value;
                RaisePropertyChanged("ItemNo");
            }
        }

        [JsonIgnore]
        public string Error
        {
            get { return null; }
        }

        public string this[string propertyName]
        {
            get { return IsValid(propertyName); }
        }

        private string IsValid(string propertyName)
        {
            switch (propertyName)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(Name))
                        return "*-必填项不能为空";
                    break;
            }
            return null;
        }
    }
}
