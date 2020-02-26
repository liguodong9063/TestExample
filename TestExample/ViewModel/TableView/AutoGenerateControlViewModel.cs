using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using TestExample.Common;
using TestExample.Model;
using TestExample.Utilities;
using WpfControls.Editors;

namespace TestExample.ViewModel.TableView
{
    public class AutoGenerateControlViewModel: ViewModelBase
    {
        private ObservableCollection<AccountingItemDto> _accountingItems = new ObservableCollection<AccountingItemDto>
        {
            new AccountingItemDto
            {
                TypeId = 1,
                TypeName = "部门",
                DataItems = new ObservableCollection<AccountingItemDataDto>
                {
                    new AccountingItemDataDto
                    {
                        Id = 1,No = "001001",Name = "管理部管理部管理部管理部"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 2,No = "002",Name = "财务部财务部"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 3,No = "003",Name = "开发部"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 4,No = "004",Name = "业务部"
                    }
                }
            },
            new AccountingItemDto
            {
                TypeId = 2,
                TypeName = "员工",
                DataItems = new ObservableCollection<AccountingItemDataDto>
                {
                    new AccountingItemDataDto
                    {
                        Id = 1,No = "001",Name = "颜照"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 2,No = "002",Name = "龚尧"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 3,No = "003",Name = "李国栋"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 4,No = "004",Name = "王丹"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 3,No = "005",Name = "李荣"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 3,No = "006",Name = "李四"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 3,No = "007",Name = "李五"
                    },
                },
                InputedText = "李四"
            },
            new AccountingItemDto
            {
                TypeId = 3,
                TypeName = "工程项目",
                DataItems = new ObservableCollection<AccountingItemDataDto>
                {
                    new AccountingItemDataDto
                    {
                        Id = 1,No = "001",Name = "五局地铁"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 2,No = "002",Name = "成贵项目"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 3,No = "003",Name = "望安高速"
                    },
                    new AccountingItemDataDto
                    {
                        Id = 4,No = "004",Name = "金房墅间"
                    }
                }
            }
        };

        private string _tipInfo;

        private ICommand _customSelectionChangedCommand;

        public ICommand CustomSelectionChangedCommand => _customSelectionChangedCommand;


#region 临时
        private TestValidationDto _validationDto=new TestValidationDto();
        private AccountingItemSuggestionProvider _suggestion;

        public AccountingItemSuggestionProvider Suggestion
        {
            get => _suggestion;
            set
            {
                if (_suggestion == value) return;
                _suggestion = value;
                RaisePropertyChanged(nameof(Suggestion));
            }
        }

        public TestValidationDto ValidationDto
        {
            get => _validationDto;
            set
            {
                if (_validationDto == value) return;
                _validationDto = value;
                RaisePropertyChanged(nameof(ValidationDto));
            }
        }
#endregion

        public AutoGenerateControlViewModel()
        {
            foreach (var accountingItemDto in AccountingItems)
            {
                accountingItemDto.SuggestionProvider = new AccountingItemSuggestionProvider(accountingItemDto.DataItems.ToList());
            }


            _customSelectionChangedCommand=new RelayCommand<object>(message =>
            {
                var control = message as AutoCompleteTextBox;
                TipInfo = (control?.SelectedItem as AccountingItemDataDto)?.Name;
            });

            _suggestion = new AccountingItemSuggestionProvider(AccountingItems[1].DataItems.ToList());
        }



        public ObservableCollection<AccountingItemDto> AccountingItems
        {
            get => _accountingItems;
            set
            {
                if (_accountingItems == value) return;
                _accountingItems = value;
                RaisePropertyChanged(nameof(AccountingItems));
            }
        }

        public string TipInfo
        {
            get => _tipInfo;
            set
            {
                if (_tipInfo == value) return;
                _tipInfo = value;
                RaisePropertyChanged(nameof(TipInfo));
            }
        }
    }

    public class TestValidationDto:ModelBase,IDataErrorInfo
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
                RaisePropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
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
                    if (Id==null)
                        return "不能为空！";
                    break;
            }
            return null;
        }
    }
}
