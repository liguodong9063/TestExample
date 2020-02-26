using DevExpress.Xpf.Editors;

namespace TestExample.Infrastructure.Localizers
{
    public class CustomerEditorLocalizer : EditorLocalizer
    {
        protected override void PopulateStringTable()
        {
            base.PopulateStringTable();
            AddString(EditorStringId.SelectAll, Resource.EditorLocalizerResource.SelectAll);
            AddString(EditorStringId.Clear, Resource.EditorLocalizerResource.Clear);
            AddString(EditorStringId.OK, Resource.EditorLocalizerResource.OK);
            AddString(EditorStringId.Cancel, Resource.EditorLocalizerResource.Cancel);

            AddString(EditorStringId.FilterClauseBeginsWith, Resource.EditorLocalizerResource.Cancel);

        }
    }
}