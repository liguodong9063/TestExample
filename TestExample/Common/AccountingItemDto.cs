using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using TestExample.Model;
using WpfControls.Editors;

namespace TestExample.Common
{
    public class AccountingItemDto: ModelBase, IDataErrorInfo
    {
        private int _typeId;
        private string _typeName;
        private ObservableCollection<AccountingItemDataDto> _dataItems;

        private string _inputedText;
        private AccountingItemDataDto _selectedDataItem;
        private int? _selectedId;

        /// <summary>
        /// 核算项类别ID
        /// </summary>
        public int TypeId
        {
            get => _typeId;
            set
            {
                if (_typeId == value) return;
                _typeId = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 核算项类别名
        /// </summary>
        public string TypeName
        {
            get => _typeName;
            set
            {
                if (_typeName == value) return;
                _typeName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 核算项数据源
        /// </summary>
        public ObservableCollection<AccountingItemDataDto> DataItems
        {
            get => _dataItems;
            set
            {
                if (_dataItems == value) return;
                _dataItems = value;
                RaisePropertyChanged();
            }
        }

        public string InputedText
        {
            get => _inputedText;
            set
            {
                if (_inputedText == value) return;
                _inputedText = value;
                RaisePropertyChanged();
            }
        }

        public int? SelectedId
        {
            get => _selectedId;
            set
            {
                if (_selectedId == value) return;
                _selectedId = value;
                RaisePropertyChanged();
            }
        }

        public ISuggestionProvider SuggestionProvider { get; set; }

        public AccountingItemDataDto SelectedDataItem
        {
            get => _selectedDataItem;
            set
            {
                if (_selectedDataItem == value) return;
                _selectedDataItem = value;
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
                case "InputedText":
                    if (SelectedId == null)
                        return "不能为空！";
                    break;
            }
            return null;
        }
    }

    public class AccountingItemDataDto{
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
    }
}
