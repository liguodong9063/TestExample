using System.Windows;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using TestExample.Common;

namespace TestExample.UserControls
{
    public class CustomGridColumn:GridColumn
    {
        public bool CustomBestFit { get; set; }

        public bool IsColumnEnabled { get; set; }

        public static readonly DependencyProperty CustomColumnTypeProperty = DependencyProperty.Register("CustomColumnType", typeof(ColumnTypes), typeof(CustomGridColumn), new PropertyMetadata(ColumnTypes.Default, OnColumnTypePropertyChangedCallback));
        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly DependencyProperty CustomIsVisibleProperty = DependencyProperty.Register("CustomIsVisible", typeof(bool), typeof(CustomGridColumn), new PropertyMetadata(true, OnIsVisiblePropertyChangedCallback));
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(CustomGridColumn), new PropertyMetadata(0));

        public CustomGridColumn()
        {
            FilterPopupMode = FilterPopupMode.Excel;
            ImmediateUpdateColumnFilter = true;
            ShowAllTableValuesInCheckedFilterPopup = DefaultBoolean.False;

            IsColumnEnabled = true;
            Width = ColumnTypeWidth.ColumnWidths[ColumnTypes.Default][1];
            Loaded += CustomGridColumn_Loaded;
        }

        private void CustomGridColumn_Loaded(object sender, RoutedEventArgs e)
        {
            var column = sender as CustomGridColumn;
            if (column?.Index > 0)
            {
                column.VisibleIndex = column.Index;
            }
        }

        public ColumnTypes CustomColumnType
        {
            get => (ColumnTypes)GetValue(CustomColumnTypeProperty);
            set => SetValue(CustomColumnTypeProperty, value);
        }

        public bool CustomIsVisible
        {
            get => (bool)GetValue(CustomIsVisibleProperty);
            set => SetValue(CustomIsVisibleProperty, value);
        }

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set
            {
                SetValue(IndexProperty, value);
            }
        }

        private static void OnColumnTypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((ColumnTypes)e.NewValue == ColumnTypes.Default) return;
            var column = d as CustomGridColumn;
            if (column != null)
            {
                column.Width = ColumnTypeWidth.ColumnWidths[column.CustomColumnType][1];
            }
        }

        private static void OnIsVisiblePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isVisiable = (bool)e.NewValue;
            var column = d as CustomGridColumn;
            if (column != null)
            {
                column.Visible = isVisiable;
            }
        }
    }
}