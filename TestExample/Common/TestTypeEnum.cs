using System.ComponentModel;

namespace TestExample.Common
{
    public enum TestTypeEnum
    {
        [Description("表格")]
        TableView,
        [Description("树")]
        TreeListView,
        [Description("滑动")]
        Slide,
        [Description("容器")]
        PlaceHolder,
        [Description("下载器")]
        Downloader,
        [Description("快捷键")]
        HotKey,
        [Description("模糊搜索")]
        LookUpEdit,
        [Description("CustomUnBind")]
        CustomUnBind,
        [Description("ComboBox")]
        ComboBox,
        [Description("ComboBox")]
        DockPanel,
        [Description("TreeView")]
        TreeView,
        [Description("自动完成")]
        AutoCompleteTextBox,
        [Description("其他")]
        Other,
        [Description("ListView")]
        ListView
    }
}
