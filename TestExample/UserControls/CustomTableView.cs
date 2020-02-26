using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using TestExample.Utilities;

namespace TestExample.UserControls
{
    public class CustomTableView:TableView
    {
        public CustomTableView()
        {
            AllowFilterEditor = false;

            AutoWidth = false;
            AllowBestFit = true;
            BestFitArea=BestFitArea.All;
            BestFitMode=BestFitMode.VisibleRows;
            GroupSummaryDisplayMode = GroupSummaryDisplayMode.AlignByColumns;
            Loaded += CustomTableView_Loaded;
        }

        private void CustomTableView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //加载用户自定义列顺序及宽度
            bool hasCustomFormt;
            if (Grid is CustomGridControl grid)
            {
                hasCustomFormt = GridControlColumnInformationHelper.LoadGridSettingsByXml(grid);
            }
        }

        protected override PopupBaseEdit CreateExcelColumnFilterPopupEditor()
        {
            var p = base.CreateExcelColumnFilterPopupEditor();
            p.PopupMinWidth = 260;
            return p;
        }
    }
}
