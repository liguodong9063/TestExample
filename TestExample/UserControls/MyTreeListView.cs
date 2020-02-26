using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Native;
using DevExpress.Xpf.Grid.TreeList;
using TestExample.View.TableView;
using TestExample.View.TreeListView;
using TestExample.ViewModel.TreeListView;

namespace TestExample.UserControls
{

    public class CanceledEventArgs : EventArgs
    {
        public object NewRow { get; private set; }
        public object OldRow { get; private set; }
        public bool Cancel { get; set; }

        public CanceledEventArgs(object oldRow, object newRow)
        {
            OldRow = oldRow;
            NewRow = newRow;
            Cancel = false;
        }
    }

    public delegate void CanceledEventHandler(object sender, CanceledEventArgs e);

    public class MyTreeListView: TreeListView
    {
        public event CanceledEventHandler FocusedRowChanging;

        static MyTreeListView()
        {
            FocusedRowHandleProperty.OverrideMetadata(typeof(MyTreeListView), new FrameworkPropertyMetadata(GridControl.InvalidRowHandle, null,(d, e) => ((MyTreeListView)d).CoerceFocusedRowHandle((int)e)));
        }

        object CoerceFocusedRowHandle(int value)
        {
            if (FocusedRowHandle == value) return value;
            if (FocusedRowChanging != null)
            {
                TreeListNode affectedNode = GetNodeByRowHandle(value);
                object newRow = null;
                if (affectedNode != null)
                {
                    newRow = affectedNode.Content;
                }
                CanceledEventArgs e = new CanceledEventArgs(FocusedRow, newRow);
                FocusedRowChanging(this, e);
                if (e.Cancel)
                    return FocusedRowHandle;
            }
            return value;
        }

        protected override SelectionStrategyBase CreateSelectionStrategy()
        {
            var result = base.CreateSelectionStrategy();
            return result is TreeListSelectionStrategyRow ? new TreeListSelectionStrategyRowEx(this) : result;
        }
    }

    public class TreeListSelectionStrategyRowEx : TreeListSelectionStrategyRow
    {
        public TreeListSelectionStrategyRowEx(TreeListView view) : base(view)
        {
        }

        protected override void OnAfterMouseLeftButtonDownCore(IDataViewHitInfo hitInfo)
        {
            if (hitInfo.InRow)
            {
                //屏蔽选了一条明细后按Ctrl可以同时选中ParentId为null的记录,即按Ctrl点选时不允许选中Parent行
                var rowControl = (RowControl) view.GetRowElementByRowHandle(hitInfo.RowHandle);
                var treeListRowData = (TreeListRowData) rowControl.DataContext;
                var rowData = treeListRowData.Row;
                if (rowData.GetType() == typeof(TreeListView4.TestObject))
                {
                    var testObject = (TreeListView4.TestObject)treeListRowData.Row;
                    if (testObject.ParentId == null)
                    {
                        return;
                    }
                }
            }
            //屏蔽Shift连选
            if (Keyboard2.IsShiftPressed) return;

            base.OnAfterMouseLeftButtonDownCore(hitInfo);
        }

        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (Keyboard2.IsShiftPressed)
                base.OnAfterMouseLeftButtonDownCore(((TreeListView)view).CalcHitInfo((DependencyObject)e.OriginalSource));

            base.OnMouseLeftButtonUp(e);
        }
    }
}
