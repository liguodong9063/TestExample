using System.Collections.Generic;
using System.Linq;
using TestExample.Common;
using WpfControls.Editors;

namespace TestExample.Utilities
{
    public class IdNameSuggestionProvider : ISuggestionProvider
    {
        private readonly List<IdNameObject> _dataItems;

        public IdNameSuggestionProvider(List<IdNameObject> dataItems)
        {
            _dataItems = dataItems;
        }

        public int? AccountingTypeId { get; set; }

        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            return _dataItems.Where(a => a.Name.Contains(filter)).Take(5).ToList();
         }
    }
}
