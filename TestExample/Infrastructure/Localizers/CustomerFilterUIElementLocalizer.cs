using DevExpress.Utils.Filtering.Internal;

namespace TestExample.Infrastructure.Localizers
{
    public class CustomerFilterUIElementLocalizer : FilterUIElementLocalizer
    {
        protected override void PopulateStringTable()
        {
            base.PopulateStringTable();
            AddString(FilterUIElementLocalizerStringId.CustomUIFilterBeginsWithName, "开始");
            AddString(FilterUIElementLocalizerStringId.CustomUIFilterBeginsWithName, "开始");
            AddString(FilterUIElementLocalizerStringId.CustomUIFilterDoesNotEqualName, "和={0:0.##}");
            //AddString(EditorStringId.Clear, Resource.EditorLocalizerResource.Clear);
            //AddString(EditorStringId.OK, Resource.EditorLocalizerResource.OK);
            //AddString(EditorStringId.Cancel, Resource.EditorLocalizerResource.Cancel);
        }
    }
}