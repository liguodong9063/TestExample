using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestExample.Common;
using WpfControls.Editors;

namespace TestExample.Utilities
{
    public class AutoGenerateColumnSuggestionProvider : ISuggestionProvider
    {
        private readonly List<AccountingItemDto> _dataItems;

        public AutoGenerateColumnSuggestionProvider(List<AccountingItemDto> dataItems)
        {
            _dataItems = dataItems;
        }

        public int? AccountingTypeId { get; set; }

        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            var dataItems = _dataItems.FirstOrDefault(a => a.TypeId == AccountingTypeId) ??new AccountingItemDto {DataItems = new ObservableCollection<AccountingItemDataDto>()};
            return dataItems.DataItems.Where(a => a.Name.Contains(filter));
        }
    }
}
