using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using TestExample.Common;
using TestExample.Model;
using TestExample.Utilities;
using Binding = System.Windows.Data.Binding;
using UserControl = System.Windows.Controls.UserControl;

namespace TestExample.UserControls
{
    public class CustomGridControl:GridControl
    {
        public bool CustomAllowMultiSelect { get; set; }

        public GridUniqueNames UniqueName { get; set; }

        /// <summary>
        /// 预留字段
        /// </summary>
        public static readonly DependencyProperty CustomFieldConfigurationsProperty = DependencyProperty.Register("CustomFieldConfigurations", typeof(IDictionary<string, CustomField>), typeof(CustomGridControl), new PropertyMetadata(new Dictionary<string, CustomField>(), OnCustomFieldConfigurationsPropertyChangedCallback));
        /// <summary>
        /// 自定义列显示
        /// </summary>
        public static readonly DependencyProperty ColumnDisplayConfigurationsProperty = DependencyProperty.Register("ColumnDisplayConfigurations", typeof(IDictionary<string, bool>), typeof(CustomGridControl), new PropertyMetadata(new Dictionary<string, bool>(), OnColumnDisplayConfigurationsPropertyChangedCallback));

        public CustomGridControl()
        {
            ImplyNullLikeEmptyStringWhenFiltering = true;

            Loaded += CustomGridControl_Loaded;
        }

        public IDictionary<string, CustomField> CustomFieldConfigurations
        {
            get => (IDictionary<string, CustomField>)GetValue(CustomFieldConfigurationsProperty);
            set => SetValue(CustomFieldConfigurationsProperty, value);
        }

        public IDictionary<string, bool> ColumnDisplayConfigurations
        {
            get => (IDictionary<string, bool>)GetValue(ColumnDisplayConfigurationsProperty);
            set => SetValue(ColumnDisplayConfigurationsProperty, value);
        }

        private void CustomGridControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ItemsSourceChanged += CustomGridControl_ItemsSourceChanged;

            if (DataContext == null)
            {
                //无DataContext时则执行ItemsSourceChagned事件后返回
                CustomGridControl_ItemsSourceChanged(null, null);
                return;
            }
            var isAutoBindProperty = DataContext.GetType().GetProperty("IsAutoBind");
            if (isAutoBindProperty == null)
            {
                //无IsAutoBind属性时则执行ItemsSourceChagned事件后返回
                CustomGridControl_ItemsSourceChanged(null, null);
                return;
            }
            var isAutoBindValue = isAutoBindProperty.GetValue(DataContext, null);
            var isAutoBind = false;
            if (isAutoBindValue != null)
            {
                bool.TryParse(isAutoBindValue.ToString(), out isAutoBind);
            }
            if (!isAutoBind)
            {
                //IsAutoBind=false时则执行ItemsSourceChagned事件后返回
                CustomGridControl_ItemsSourceChanged(null, null);
                return;
            }
            RelativeSource relativeSource;
            if (bool.TryParse(ReflectHelper.GetPropertyValue(DataContext, "IsWindow")?.ToString(), out bool isWindow))
            {
                relativeSource = isWindow ? new RelativeSource { Mode = RelativeSourceMode.FindAncestor, AncestorType = typeof(Window) } : new RelativeSource { Mode = RelativeSourceMode.FindAncestor, AncestorType = typeof(UserControl) };
            }
            else
            {
                relativeSource = new RelativeSource { Mode = RelativeSourceMode.FindAncestor, AncestorType = typeof(UserControl) };
            }

            var itemsSourceBinding = new Binding("DataContext.VirtualItems") { RelativeSource = relativeSource };
            SetBinding(ItemsSourceProperty, itemsSourceBinding);

            CustomGridControl_ItemsSourceChanged(null, null);

            var selectedItemBinding = new Binding("DataContext.SelectedVirtualItem") { RelativeSource = relativeSource };
            SetBinding(SelectedItemProperty, selectedItemBinding);
            var selectedColumnDisplayConfigurationsBinding = new Binding("DataContext.ColumnDisplayConfigurations") { RelativeSource = relativeSource };
            SetBinding(ColumnDisplayConfigurationsProperty, selectedColumnDisplayConfigurationsBinding);
            var selectedCustomFiledsBinding = new Binding("DataContext.CustomFields") { RelativeSource = relativeSource };
            SetBinding(CustomFieldConfigurationsProperty, selectedCustomFiledsBinding);
            //注：通过绑定的方式数据量大的时候速度慢
            //if (!AllowMultiSelect) return;
            //var selectedItemsBinding = new Binding("DataContext.SelectedVirtualItems") { RelativeSource = relativeSource };
            //SetBinding(SelectedItemsProperty, selectedItemsBinding);
            if (!CustomAllowMultiSelect) return;
            SelectionChanged += CustomNewGridControl_SelectionChanged;
        }

        /// <summary>
        /// 数据源改变时重新计算列宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomGridControl_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            if (ItemsSource == null) return;
            //清除选中行
            SelectedItem = null;
            //处理列宽
            if (ItemsSource != null && !GridControlColumnInformationHelper.ExistCustomerSetting(this))
            {
                foreach (var column in Columns)
                {
                    var customColumn = column as CustomGridColumn;
                    if (customColumn == null || !customColumn.CustomBestFit || customColumn.FixedWidth) continue;

                    if (View is CustomTableView view)
                    {
                        column.BestFitArea = BestFitArea.All;
                        column.BestFitMode = BestFitMode.VisibleRows;
                        column.Width = view.CalcColumnBestFitWidth(column);
                    }
                    var maxWidth = ColumnTypeWidth.ColumnWidths[customColumn.CustomColumnType][2];
                    var minWidth = ColumnTypeWidth.ColumnWidths[customColumn.CustomColumnType][0];
                    //实际宽度与列类型限制的最小、最大宽度比较，不能超过限制
                    var needSetToActualWidth = true;
                    var actualDataWidth = column.ActualDataWidth;
                    if (maxWidth < actualDataWidth && maxWidth != 0)
                    {
                        column.Width = maxWidth;
                        needSetToActualWidth = false;
                    }
                    if (minWidth > actualDataWidth && minWidth != 0)
                    {
                        column.Width = minWidth;
                        needSetToActualWidth = false;
                    }
                    column.FixedWidth = false;
                    var width = column.Width;
                    column.Width = 0;
                    if (customColumn.CustomBestFit)
                    {
                        column.Width = !needSetToActualWidth ? width : actualDataWidth;
                    }
                    else
                    {
                        column.Width = width;
                    }
                }
            }
            //自动绑定相关，去掉选中项
            if (DataContext == null) return;
            var isAutoBindProperty = DataContext.GetType().GetProperty("IsAutoBind");
            if (isAutoBindProperty == null) return;
            var isAutoBindValue = isAutoBindProperty.GetValue(DataContext, null);
            var isAutoBind = false;
            if (isAutoBindValue != null)
            {
                bool.TryParse(isAutoBindValue.ToString(), out isAutoBind);
            }
            if (!isAutoBind || ItemsSource == null || !CustomAllowMultiSelect) return;
            //去掉默认选中行
            SelectedItems = new ObservableCollection<object>();
            ReflectHelper.SetPropertyValue(DataContext, "SelectedVirtualItems", new ObservableCollection<object>());
            //RefreshData();
        }

        private void CustomNewGridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (ItemsSource == null) return;
            var propertyInfo = ItemsSource.GetType().GetProperty("Obs");
            if (propertyInfo == null) return;
            var rows = (IList)propertyInfo.GetValue(ItemsSource, null);
            var selectedVirtualItems = ((ObservableCollection<object>)ReflectHelper.GetPropertyValue(DataContext, "SelectedVirtualItems")).Clone() ?? new ObservableCollection<object>();
            var selectedItems = SelectedItems;
            #region 开始处理选择图标刷新
            var toRefreshRowHandles = new List<int>();

            //获取可见行
            var currentVisibleRowHandles = GetVisibleRowHandles();

            if (selectedItems != null && selectedItems.Count > 0)
            {
                for (var i = 0; i < selectedVirtualItems.Count; i++)
                {
                    if (i >= selectedVirtualItems.Count) break;

                    var selectedVirtualItem = selectedVirtualItems[i];

                    if ((int)selectedVirtualItem >= rows.Count) break;

                    if (selectedItems.Contains(selectedVirtualItem)) continue;
                    var selectedRowData = rows[(int)selectedVirtualItem];
                    ReflectHelper.SetPropertyValue(selectedRowData, "IsChecked", false);
                    //设置哪些数据需要刷新
                    if (currentVisibleRowHandles != null && currentVisibleRowHandles.Contains((int)selectedVirtualItem))
                    {
                        toRefreshRowHandles.Add(GetRowHandleByListIndex((int)selectedVirtualItem));
                    }
                    selectedVirtualItems.Remove(selectedVirtualItem);
                    i--;
                }
                foreach (var selectedItem in selectedItems)
                {
                    if (selectedVirtualItems.Contains(selectedItem)) continue;
                    var selectedRowData = rows[(int)selectedItem];
                    if (currentVisibleRowHandles != null)
                    {
                        ReflectHelper.SetPropertyValue(selectedRowData, "IsChecked", true);
                    }
                    //设置哪些数据需要刷新
                    if (currentVisibleRowHandles != null && currentVisibleRowHandles.Contains((int)selectedItem))
                    {
                        toRefreshRowHandles.Add(GetRowHandleByListIndex((int)selectedItem));
                    }
                    selectedVirtualItems.Add(selectedItem);
                }
            }
            else
            {
                //初次进入时解决默认勾选上了第一条记录问题或则点击全部选按钮时
                if (selectedVirtualItems.Count > 0)
                {
                    foreach (var selectedVirtualItem in selectedVirtualItems)
                    {
                        if (rows.Count <= (int)selectedVirtualItem) continue;
                        var selectedRowData = rows[(int)selectedVirtualItem];
                        ReflectHelper.SetPropertyValue(selectedRowData, "IsChecked", false);
                    }
                    toRefreshRowHandles.AddRange(from i in selectedVirtualItems where currentVisibleRowHandles == null || currentVisibleRowHandles.Contains((int)i) select GetRowHandleByListIndex((int)i));
                    selectedVirtualItems = new ObservableCollection<object>();
                }

            }
            try
            {
                //刷新数据
                foreach (var rowIndex in toRefreshRowHandles)
                {
                    RefreshRow(rowIndex);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            #endregion
            ReflectHelper.SetPropertyValue(DataContext, "SelectedVirtualItems", selectedVirtualItems);
        }

        private static void OnCustomFieldConfigurationsPropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var source = s as CustomGridControl;
            if (source == null) return;
            UpdateColumns(source);
        }

        private static void OnColumnDisplayConfigurationsPropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var source = s as CustomGridControl;
            if (source == null) return;
            UpdateColumns(source);
        }

        private static void UpdateColumns(CustomGridControl source)
        {
            ResetColumns(source.Columns);
            UpdateColumnsByCustomFieldConfigurations(source);
            UpdateColumnsByColumnDisplayConfigurations(source);
        }

        private static void ResetColumns(IEnumerable<GridColumn> columns)
        {
            foreach (var column in columns)
            {
                if (column is CustomGridColumn customColumn)
                {
                    //修复设置IsVisible=false无效问题
                    if (!customColumn.CustomIsVisible) continue;
                    column.Visible = true;
                    column.IsEnabled = true;
                }
                else
                {
                    column.Visible = true;
                    column.IsEnabled = true;
                }
            }
        }

        private static void UpdateColumnsByCustomFieldConfigurations(CustomGridControl source)
        {
            if (source.CustomFieldConfigurations == null) return;
            foreach (var gridColumn in source.Columns)
            {
                var column = (CustomGridColumn)gridColumn;
                column.IsColumnEnabled = true;
                var key = StringHelper.LowerInitial(column.FieldName);
                if (key == null || !source.CustomFieldConfigurations.ContainsKey(key)) continue;
                var customFieldConfiguration = source.CustomFieldConfigurations[key];
                column.Header = customFieldConfiguration.FieldName;
                if (customFieldConfiguration.IsUse != 0) continue;
                column.IsColumnEnabled = false;
                column.Visible = false;
            }
        }

        private static void UpdateColumnsByColumnDisplayConfigurations(CustomGridControl source)
        {
            if (source.ColumnDisplayConfigurations == null) return;
            foreach (var column in source.Columns)
            {
                string key = column.FieldName;
                if (key != null && source.ColumnDisplayConfigurations.ContainsKey(key))
                {
                    bool isDisplayed = source.ColumnDisplayConfigurations[key];
                    if (!isDisplayed)
                    {
                        column.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前可见区域ListIndex
        /// </summary>
        /// <returns></returns>
        private List<int> GetVisibleRowHandles()
        {
            List<int> currentVisibleRowHandles;
            var tableView = View as TableView;
            if (tableView == null) return new List<int>();
            var startIndex = GetRowHandleByVisibleIndex(tableView.TopRowIndex);
            var endIndex = GetRowHandleByVisibleIndex(Convert.ToInt32(GetScrollInfo().ViewportHeight + GetScrollInfo().VerticalOffset));
            if (IsValidRowHandle(endIndex))
            {
                currentVisibleRowHandles = new List<int>();
                for (var i = startIndex; i <= endIndex; i++)
                {
                    currentVisibleRowHandles.Add((int)GetRow(i));
                }
                //额外多将一部分数据视作可见区域（否则会导致临界点数据刷新不及时）
                for (var i = 1; i <= 2; i++)
                {
                    if (!IsValidRowHandle(endIndex + i)) break;
                    currentVisibleRowHandles.Add((int)GetRow(endIndex + i));
                }
            }
            else
            {
                endIndex = GetRowHandleByVisibleIndex(Convert.ToInt32(GetScrollInfo().ViewportHeight + GetScrollInfo().VerticalOffset) - 1);
                if (!IsValidRowHandle(endIndex) || endIndex != VisibleRowCount - 1) return new List<int>();
                currentVisibleRowHandles = new List<int>();
                for (var i = startIndex; i <= VisibleRowCount - 1; i++)
                {
                    currentVisibleRowHandles.Add((int)GetRow(i));
                }
            }
            return currentVisibleRowHandles;
        }

        /// <summary>
        /// 获取滚轮信息
        /// </summary>
        /// <returns></returns>
        private IScrollInfo GetScrollInfo()
        {
            foreach (DependencyObject item in new VisualTreeEnumerable(View))
            {
                if (item is DataPresenter info)
                    return info;
            }
            throw new InvalidOperationException();
        }
    }
}
