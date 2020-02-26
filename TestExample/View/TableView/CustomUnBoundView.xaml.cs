using System.Linq;
using System.Windows;
using DevExpress.Xpf.Grid;
using TestExample.Common;
using TestExample.ViewModel.TableView;

namespace TestExample.View.TableView
{
    /// <summary>
    /// CustomUnBoundView.xaml 的交互逻辑
    /// </summary>
    public partial class CustomUnBoundView : Window
    {
        public CustomUnBoundView()
        {
            InitializeComponent();
        }

        private void GridControl_OnCustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            var dataContext = (CustomUnBoundViewModel)DataContext;
            var row = (StudentForCustomUnBound)e.Source.GetRowByListIndex(e.ListSourceRowIndex);
            switch (e.Column.FieldName)
            {
                case "UnBoundGrade":
                    //注意事项：
                    //1、UnBoundType=Object
                    //2、ColumnFilterMode=DisplayText，否则筛选无值
                    //3、一定不要设置ValueMember，否则ComboBox无显示
                    if (e.IsGetData)
                    {
                        var selectedItem = dataContext.Grades.FirstOrDefault(x => x.Id == row.Grade);
                        e.Value = selectedItem;
                    }
                    else if (e.IsSetData)
                    {
                        var selectedItem = (IdNameObject)e.Value;
                        row.Grade = selectedItem.Id??0;
                    }
                    break;
                case "UnBoundBillState":
                    if (e.IsGetData)
                    {
                        switch (row.BillState)
                        {
                            case 0:
                                e.Value = "禁用";
                                break;
                            case 1:
                                e.Value = "正常";
                                break;
                            default:
                                e.Value = "N/A";
                                break;
                        }
                    }
                    break;
            }
        }

        private void GridViewBase_OnCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "UnBoundGrade") return;
            var item=(StudentForCustomUnBound)e.Cell.Row;
            var idNameObject = (IdNameObject) e.Cell.Value;
            if (int.TryParse(idNameObject?.Id.ToString(), out int gradeId))
            {
                item.Grade = gradeId;
            }
            else
            {
                item.Grade = -1;
            }
        }
    }
}
