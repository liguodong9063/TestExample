using System.Collections;

namespace WpfControls.Editors
{
    public interface ISuggestionProvider
    {
        int? AccountingTypeId { get; set; }

        #region Public Methods

        IEnumerable GetSuggestions(string filter);

        #endregion Public Methods
    }
}
