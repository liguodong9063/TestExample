using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TestExample.Model
{
    public class CustomField : ModelBase, IDataErrorInfo
    {
        private int _controlType;
        private string _field;
        private string _fieldName;
        private int? _id;
        private int? _orgId;
        private int _isSpecial;
        private int? _isUse;
        private int? _no;
        private List<CustomFieldSource> _reserveSrcDtoList;
        private string _itemNo;
        private int _operationType;
        private int? _isUnique;
        private int? _version;

        /// <summary>
        ///     是否唯一
        /// </summary>
        [JsonProperty("isUnique")]
        public int? IsUnique
        {
            get { return _isUnique; }
            set
            {
                if (_isUnique == value) return;
                _isUnique = value;
                RaisePropertyChanged("IsUnique");
            }
        }


        /// <summary>
        ///     控件类型（1：下拉框；1以外：文本框）
        /// </summary>
        [JsonProperty("controlType")]
        public int ControlType
        {
            get { return _controlType; }
            set
            {
                if (_controlType == value) return;
                _controlType = value;
                RaisePropertyChanged("ControlType");
            }
        }

        /// <summary>
        ///     字段的英文名
        /// </summary>
        [JsonProperty("field")]
        public string Field
        {
            get { return _field; }
            set
            {
                if (_field == value) return;
                _field = value;
                RaisePropertyChanged("Field");
            }
        }

        /// <summary>
        ///     字段的中文名
        /// </summary>
        [JsonProperty("fieldName")]
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                if (_fieldName == value) return;
                _fieldName = value;
                RaisePropertyChanged("FieldName");
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
        ///     组织机构ID
        /// </summary>
        [JsonProperty("orgId")]
        public int? OrgId
        {
            get { return _orgId; }
            set
            {
                if (_orgId == value) return;
                _orgId = value;
                RaisePropertyChanged("OrgId");
            }
        }

        /// <summary>
        ///     1、固定字段，2、预留字段
        /// </summary>
        [JsonProperty("isSpecial")]
        public int IsSpecial
        {
            get { return _isSpecial; }
            set
            {
                if (_isSpecial == value) return;
                _isSpecial = value;
                RaisePropertyChanged("IsSpecial");
            }
        }

        /// <summary>
        ///     是否启用
        /// </summary>
        [JsonProperty("isUse")]
        public int? IsUse
        {
            get { return _isUse; }
            set
            {
                if (_isUse == value) return;
                _isUse = value;
                RaisePropertyChanged("IsUse");
            }
        }

        /// <summary>
        ///     编号
        /// </summary>
        [JsonProperty("no")]
        public int? No
        {
            get { return _no; }
            set
            {
                if (_no == value) return;
                _no = value;
                RaisePropertyChanged("No");
            }
        }

        //预留字段数据源
        [JsonProperty("reserveSrcDtoList")]
        public List<CustomFieldSource> ReserveSrcDtoList
        {
            get { return _reserveSrcDtoList; }
            set
            {
                if (_reserveSrcDtoList == value) return;
                _reserveSrcDtoList = value;
                RaisePropertyChanged("ReserveSrcDtoList");
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
        ///     操作类型(仅用于前台识别)
        /// </summary>
        [JsonIgnore]
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

        [JsonProperty("Version")]
        public int? Version
        {
            get { return _version; }
            set
            {
                if (_version == value) return;
                _version = value;
                RaisePropertyChanged("Version");
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
                case "FieldName":
                    if (string.IsNullOrWhiteSpace(FieldName) && IsUse == 1)
                        return "*-必填项不能为空";
                    break;
            }
            return null;
        }
    }
}
