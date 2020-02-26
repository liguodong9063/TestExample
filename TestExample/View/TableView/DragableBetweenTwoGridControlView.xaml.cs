using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid.DragDrop;

namespace TestExample.View.TableView
{
    /// <summary>
    /// DragableView.xaml 的交互逻辑
    /// </summary>
    public partial class DragableBetweenTwoGridControlView : Window
    {
        public DragableBetweenTwoGridControlView()
        {
            InitializeComponent();
        }

        private void GridDragDropManager1_OnDropped(object sender, GridDroppedEventArgs e)
        {
            //var manager = (GridDragDropManager)sender;
            //var draggingRow = (Student)manager.DraggingRows[0];
            //var dataIndex = ((DragableViewModel)DataContext).Students.IndexOf(draggingRow);
            //if (draggingRow.Sort != dataIndex)
            //{
            //    //draggingRow.OperationType = 3;
            //    //draggingRow.Sort = dataIndex;

            //    TestGrid.SetCellValue(dataIndex, "OperationType", 3);
            //    TestGrid.SetCellValue(dataIndex, "Sort", dataIndex);
            //}



            for (var i = 0; i < TestGrid1.VisibleRowCount; i++)
            {
                var rowHandle = TestGrid1.GetRowHandleByVisibleIndex(i);
                if (TestGrid1.IsGroupRowHandle(rowHandle))
                    throw new NotImplementedException();
                if (int.TryParse(TestGrid1.GetCellValue(i, "Sort")?.ToString(), out int oldIndex))
                {
                    if (oldIndex != i)
                    {
                        TestGrid1.SetCellValue(i, "OperationType", 3);
                        TestGrid1.SetCellValue(i, "Sort", i);
                    }
                }
                
            }
        }

        private void GridDragDropManager2_OnDropped(object sender, GridDroppedEventArgs e)
        {
            //var manager = (GridDragDropManager)sender;
            //var draggingRow = (Student)manager.DraggingRows[0];
            //var dataIndex = ((DragableViewModel)DataContext).Students.IndexOf(draggingRow);
            //if (draggingRow.Sort != dataIndex)
            //{
            //    //draggingRow.OperationType = 3;
            //    //draggingRow.Sort = dataIndex;

            //    TestGrid.SetCellValue(dataIndex, "OperationType", 3);
            //    TestGrid.SetCellValue(dataIndex, "Sort", dataIndex);
            //}



            for (var i = 0; i < TestGrid2.VisibleRowCount; i++)
            {
                var rowHandle = TestGrid2.GetRowHandleByVisibleIndex(i);
                if (TestGrid2.IsGroupRowHandle(rowHandle))
                    throw new NotImplementedException();
                if (int.TryParse(TestGrid2.GetCellValue(i, "Sort")?.ToString(), out int oldIndex))
                {
                    if (oldIndex != i)
                    {
                        TestGrid2.SetCellValue(i, "OperationType", 3);
                        TestGrid2.SetCellValue(i, "Sort", i);
                    }
                }

            }
        }
    }

    [POCOViewModel]
    public class DragableBetweenTwoGridControlViewModel : ViewModelBase
    {
        private ObservableCollection<Student> _students1;
        private ObservableCollection<Student> _students2;

        public ObservableCollection<Student> Students1
        {
            get => _students1;
            set
            {
                if (_students1 == value) return;
                _students1 = value;
                RaisePropertyChanged("Students1");
            }
        }

        public ObservableCollection<Student> Students2
        {
            get => _students2;
            set
            {
                if (_students2 == value) return;
                _students2 = value;
                RaisePropertyChanged("Students2");
            }
        }

        public DragableBetweenTwoGridControlViewModel()
        {
            var students = new List<Student>
            {
                new Student{Id=1,Name = "汤龙",Sort = 0,OperationType = 0},
                new Student{Id=2,Name = "王丹",Sort = 1,OperationType = 0},
                new Student{Id=3,Name = "张勇",Sort = 2,OperationType = 0},
                new Student{Id=4,Name = "李国栋",Sort = 3,OperationType = 0},
                new Student{Id=5,Name = "刘文浩",Sort = 4,OperationType = 0},
            };
            Students1 = new ObservableCollection<Student>(students);
            Students2=new ObservableCollection<Student>();
        }
    }
}
