using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using TestExample.Common;
using TestExample.Model.AutoGenerateColumn;
using TestExample.UserControls;
using TestExample.Utilities;
using TestExample.ViewModel.TableView;
using WpfControls.Editors;

namespace TestExample.View.TableView
{
    /// <summary>
    /// AutoGenerateColumnView.xaml 的交互逻辑
    /// </summary>
    public partial class AutoGenerateColumnView : Window
    {
        public AutoGenerateColumnView()
        {
            InitializeComponent();

            GridControl.Loaded += GridControl_Loaded;
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public readonly ObservableCollectionHelperForAutoGenerate ObservableCollectionHelperList = new ObservableCollectionHelperForAutoGenerate();

        private void LoadData()
        {
            var dataContext = DataContext as AutoGenerateColumnViewModel;
            if (dataContext == null) return;
            var rows = new ObservableCollection<CustomRow>();
            foreach (var columnData in dataContext.Rows)
            {
                rows.Add(columnData);
            }
            ObservableCollectionHelperList.Ini(rows, dataContext.Headers);
            foreach (var header in dataContext.Headers)
            {
                var column = new CustomGridColumn
                {
                    FieldName = header.FieldName,
                    Header = header.HeaderName,
                    HorizontalHeaderContentAlignment = HorizontalAlignment.Center,
                };

                if (header.AccountingTypeId != null)
                {
                    //column.CellTemplate= FindResource("AutoCompleteColumnCellTemplate") as DataTemplate;
                    column.DisplayTemplate = FindResource("AutoCompleteColumnDisplayTemplate") as ControlTemplate;
                    column.EditTemplate = FindResource("AutoCompleteColumnEditTemplate") as ControlTemplate;
                }else
                {
                    column.DisplayTemplate = FindResource("CommonColumnDisplayTemplate") as ControlTemplate;
                    column.EditTemplate = FindResource("CommonColumnEditTemplate") as ControlTemplate;
                }

                GridControl.Columns.Add(column);
            }
            GridControl.Columns.EndUpdate();
            GridControl.ItemsSource = ObservableCollectionHelperList;

        }

        private void AutoComplete_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var autoCompleteTextBox = sender as AutoCompleteTextBox;
            if (autoCompleteTextBox == null) return;
            var cellData = autoCompleteTextBox?.DataContext as EditGridCellData;
            var idValueDto=cellData?.Value as CustomColumnValue;
            if (idValueDto == null) return;
            idValueDto.OperationType = idValueDto.OperationType == 1 ? 1 : 3;
            idValueDto.Id = (autoCompleteTextBox.SelectedItem as AccountingItemDataDto)?.Id;
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var list = ObservableCollectionHelperList;
        }

        private void AddDetailButton_OnClick(object sender, RoutedEventArgs e)
        {
            var index=ObservableCollectionHelperList.Add(new CustomRow
            {
                Columns = new List<CustomColumn>
                {
                    new CustomColumn
                    {
                        FieldName = "PrjId",
                        FieldValue = new CustomColumnValue
                        {
                            Id = 3,
                            Value = "望安高速",
                            AccountingTypeId=3
                        }
                    },
                    new CustomColumn
                    {
                        FieldName = "DptId",
                        FieldValue = new CustomColumnValue
                        {
                            Id = 3,
                            Value = "管理部",
                            AccountingTypeId=1
                        }
                    },
                    new CustomColumn
                    {
                        FieldName = "EmpId",
                        FieldValue = new CustomColumnValue
                        {
                            Id = 3,
                            Value = "王五",
                            AccountingTypeId=2
                        }
                    },
                    new CustomColumn
                    {
                        FieldName = "TaxAmount",
                        FieldValue = new CustomColumnValue
                        {
                            Value = "300"
                        }
                    }
                }
            });
            GridControl.RefreshData();
        }

        private void DeleteDetailButton_OnClick(object sender, RoutedEventArgs e)
        {
            var rowHandle= (int)GridControl.SelectedItem;
            var customRow = ObservableCollectionHelperList.Obs[rowHandle] as CustomRow;
            ObservableCollectionHelperList.Remove(customRow);
            GridControl.RefreshData();
        }
    }
}