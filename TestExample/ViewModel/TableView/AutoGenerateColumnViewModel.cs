using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using TestExample.Common;
using TestExample.Model.AutoGenerateColumn;
using TestExample.Utilities;

namespace TestExample.ViewModel.TableView
{
    public class AutoGenerateColumnViewModel : ViewModelBase
    {
        private AutoGenerateColumnSuggestionProvider _suggestionProvider;

        public AutoGenerateColumnSuggestionProvider SuggestionProvider
        {
            get => _suggestionProvider;
            set
            {
                if (_suggestionProvider == value) return;
                _suggestionProvider = value;
                RaisePropertyChanged(nameof(SuggestionProvider));
            }
        }

        private readonly ObservableCollection<AccountingItemDto> _accountingItems = new ObservableCollection<AccountingItemDto>
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

        private ObservableCollection<CustomHeader> _headers;
        private ObservableCollection<CustomRow> _rows;

        public AutoGenerateColumnViewModel()
        {

            _headers=new ObservableCollection<CustomHeader>
            {
                new CustomHeader
                {
                    HeaderName = "工程项目",
                    FieldName = "PrjId",
                    AccountingTypeId=3
                },
                new CustomHeader
                {
                    HeaderName = "部门",
                    FieldName = "DptId",
                    AccountingTypeId=1
                },
                new CustomHeader
                {
                    HeaderName = "员工",
                    FieldName = "EmpId",
                    AccountingTypeId=2
                },
                new CustomHeader
                {
                    HeaderName = "余额",
                    FieldName = "TaxAmount"
                }
            };
            _rows=new ObservableCollection<CustomRow>
            {
                new CustomRow
                {
                    Columns = new List<CustomColumn>
                    {
                        new CustomColumn
                        {
                            FieldName = "PrjId",
                            FieldValue = new CustomColumnValue
                            {
                                Id = 1,
                                Value = "成贵项目",
                                AccountingTypeId=3
                            }
                        },
                        new CustomColumn
                        {
                            FieldName = "DptId",
                            FieldValue = new CustomColumnValue
                            {
                                Id = 1,
                                Value = "业务部",
                                AccountingTypeId=1
                            }
                        },
                        new CustomColumn
                        {
                            FieldName = "EmpId",
                            FieldValue = new CustomColumnValue
                            {
                                Id = 1,
                                Value = "张三",
                                AccountingTypeId=2
                            }
                        },
                        new CustomColumn
                        {
                            FieldName = "TaxAmount",
                            FieldValue = new CustomColumnValue
                            {
                                Value = "100"
                            }
                        }
                    }
                },
                new CustomRow
                {
                    Columns = new List<CustomColumn>
                    {
                        new CustomColumn
                        {
                            FieldName = "PrjId",
                            FieldValue = new CustomColumnValue
                            {
                                Id = 2,
                                Value = "五局地铁",
                                AccountingTypeId=3
                            }
                        },
                        new CustomColumn
                        {
                            FieldName = "DptId",
                            FieldValue = new CustomColumnValue
                            {
                                Id = 2,
                                Value = "财务部",
                                AccountingTypeId=1
                            }
                        },
                        new CustomColumn
                        {
                            FieldName = "EmpId",
                            FieldValue = new CustomColumnValue
                            {
                                Id = 2,
                                Value = "李四",
                                AccountingTypeId=2
                            }
                        },
                        new CustomColumn
                        {
                            FieldName = "TaxAmount",
                            FieldValue = new CustomColumnValue
                            {
                                Value = "200"
                            }
                        }
                    }
                }
            };

            SuggestionProvider=new AutoGenerateColumnSuggestionProvider(_accountingItems.ToList());
        }

        public ObservableCollection<CustomHeader> Headers
        {
            get => _headers;
            set
            {
                if (_headers == value) return;
                _headers = value;
                RaisePropertyChanged(nameof(Headers));
            }
        }

        public ObservableCollection<CustomRow> Rows
        {
            get => _rows;
            set
            {
                if (_rows == value) return;
                _rows = value;
                RaisePropertyChanged(nameof(Rows));
            }
        }
    }
}
