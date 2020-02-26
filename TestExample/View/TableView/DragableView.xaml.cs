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
    public partial class DragableView : Window
    {
        public DragableView()
        {
            InitializeComponent();
        }

        private void GridDragDropManager_OnDropped(object sender, GridDroppedEventArgs e)
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



            for (var i = 0; i < TestGrid.VisibleRowCount; i++)
            {
                var rowHandle = TestGrid.GetRowHandleByVisibleIndex(i);
                if (TestGrid.IsGroupRowHandle(rowHandle))
                    throw new NotImplementedException();
                if (int.TryParse(TestGrid.GetCellValue(i, "Sort")?.ToString(), out int oldIndex))
                {
                    if (oldIndex != i)
                    {
                        TestGrid.SetCellValue(i, "OperationType", 3);
                        TestGrid.SetCellValue(i, "Sort", i);
                    }
                }
                
            }
        }
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public int OperationType { get; set; }
    }

    [POCOViewModel]
    public class DragableViewModel:ViewModelBase
    {
        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                if (_students == value) return;
                _students = value;
                RaisePropertyChanged("Students");
            }
        }

        public DragableViewModel()
        {
            var students = new List<Student>
            {
                new Student{Id=1,Name = "汤龙",Sort = 0,OperationType = 0},
                new Student{Id=2,Name = "王丹",Sort = 1,OperationType = 0},
                new Student{Id=3,Name = "张勇",Sort = 2,OperationType = 0},
                new Student{Id=4,Name = "李国栋",Sort = 3,OperationType = 0},
                new Student{Id=5,Name = "刘文浩",Sort = 4,OperationType = 0},
            };
            Students = new ObservableCollection<Student>(students);
        }
    }
}
