using FastReport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TestExample.Common;
using TestExample.Infrastructure;
using TestExample.Model;
using TestExample.Utilities;
using TestExample.View;

namespace TestExample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HomeView
    {
        private readonly GlobalBarCodeHook listener = new GlobalBarCodeHook();

        readonly ObservableCollection<HomeNavigationModel> _navigations;

        public HomeView()
        {
            InitializeComponent();
            Loaded += HomeView_Loaded;
            Unloaded += HomeView_Unloaded;
            listener.BarCodeEnterEvent += ListenerBarCodeEnterEvent;


            _navigations = new ObservableCollection<HomeNavigationModel>()
            {
                new HomeNavigationModel
                {
                    Name = "整行变色|表头对齐方式|内容对齐方式",Url = "View/TableView/ChangeRowColorView.xaml",TestType = TestTypeEnum.TableView
                },
                new HomeNavigationModel
                {
                    Name = "整行变色（空白列除外）|列表边框|自定义表头",Url = "View/TableView/ChangeRowColorView2.xaml",TestType = TestTypeEnum.TableView
                },
                new HomeNavigationModel
                {
                    Name = "层级行模版|水平跟随滚动",Url = "View/TreeListView/TreeListView1.xaml",TestType = TestTypeEnum.TreeListView
                },new HomeNavigationModel
                {
                    Name = "层级行模版|水平不跟随滚动",Url = "View/TreeListView/TreeListView2.xaml",TestType = TestTypeEnum.TreeListView
                },new HomeNavigationModel
                {
                    Name = "普通行模板",Url = "View/TreeListView/TreeListView3.xaml",TestType = TestTypeEnum.TreeListView
                },new HomeNavigationModel
                {
                    Name = "树列表（Parent不可选中，控制Parent按钮IsEnable，不允许Shift连选，不允许跨Parent选明细）",Url = "View/TreeListView/TreeListView4.xaml",TestType = TestTypeEnum.TreeListView
                },new HomeNavigationModel
                {
                    Name = "滑动Demo",Url = "View/SlideView/SlideView1.xaml",TestType = TestTypeEnum.Slide
                },new HomeNavigationModel
                {
                    Name = "容器Demo",Url = "View/PlaceHolder/PlaceHolderView.xaml",TestType = TestTypeEnum.PlaceHolder
                },new HomeNavigationModel
                {
                    Name = "客户端升级相关要点",Url = "View/Dowloader/DowloaderView.xaml",TestType = TestTypeEnum.Downloader
                },new HomeNavigationModel
                {
                    Name = "树控件（包含可展开收起头部按钮）",Url = "View/TreeListView/TreeListView5.xaml",TestType = TestTypeEnum.TreeListView
                },new HomeNavigationModel
                {
                    Name = "行拖动（TableView）",Url = "View/TableView/DragableView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "多控件间行拖动（TableView）",Url = "View/TableView/DragableBetweenTwoGridControlView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "快捷键",Url = "View/HotKey/ShortCutView.xaml",TestType = TestTypeEnum.HotKey
                },new HomeNavigationModel
                {
                    Name = "热键【包含TextBlock内容换行并文本右对齐效果】",Url = "View/HotKey/HotKeyView.xaml",TestType = TestTypeEnum.HotKey
                },new HomeNavigationModel
                {
                    Name = "模糊搜索",Url = "View/LookUpEdit/LookUpEditView.xaml",TestType = TestTypeEnum.LookUpEdit
                },new HomeNavigationModel
                {
                    Name = "CustomUnBound的引用（包含表格中存在可编辑的下拉列表时的筛选效果|合计分组合计样式）",Url = "View/TableView/CustomUnBoundView.xaml",TestType = TestTypeEnum.CustomUnBind
                },new HomeNavigationModel
                {
                    Name = "自定义ComboBox的使用，以及在列表中的应用",Url = "View/ComboBox/ComboBoxView.xaml",TestType = TestTypeEnum.ComboBox
                },new HomeNavigationModel
                {
                    Name = "自定义ComboBox在列表中的使用",Url = "View/ComboBox/NewComboBoxView.xaml",TestType = TestTypeEnum.ComboBox
                },new HomeNavigationModel
                {
                    Name = "透明背景色ComboBox",Url = "View/ComboBox/TransparentComboBoxView.xaml",TestType = TestTypeEnum.ComboBox
                },new HomeNavigationModel
                {
                    Name = "DockPanel",Url = "View/DockPanel/DockPanelView.xaml",TestType = TestTypeEnum.DockPanel
                },new HomeNavigationModel
                {
                    Name = "GridControl列宽各种模式",Url = "View/TableView/ColumnWidthModeView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "TreeView中模版最后一列宽度自适应，显示补全时出现...",Url = "View/TreeView/TreeViewTestView.xaml",TestType = TestTypeEnum.TreeView
                },new HomeNavigationModel
                {
                    Name = "TreeView操作",Url = "View/TreeListView/TreeListViewOperatorView.xaml",TestType = TestTypeEnum.TreeListView
                },new HomeNavigationModel
                {
                    Name = "自动完成控件",Url = "View/AutoComplete/AutoCompleteView.xaml",TestType = TestTypeEnum.AutoCompleteTextBox
                },new HomeNavigationModel
                {
                    Name = "控件自动生成",Url = "View/TableView/AutoGenerateControlView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "大数据交互",Url = "View/TableView/BigDataView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "其它",Url = "View/Other/OtherView.xaml",TestType = TestTypeEnum.Other
                },new HomeNavigationModel
                {
                    Name = "自动生成列",Url = "View/TableView/AutoGenerateColumnView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "列表Enter行为",Url = "View/TableView/EnterBehaviorView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "文件多选",Url = "View/Other/FileMultiSelectView.xaml",TestType = TestTypeEnum.Other
                },new HomeNavigationModel
                {
                    Name = "GridControl边框",Url = "View/TableView/GridControlBorderView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name = "滚动行为",Url = "View/ListView/ListViewScrollBehaviorView.xaml",TestType = TestTypeEnum.ListView
                },new HomeNavigationModel
                {
                    Name="单元格获得焦点显示编辑按钮",Url="View/TableView/MouseOverShowEditorButtonView.xaml",TestType = TestTypeEnum.TableView
                },new HomeNavigationModel
                {
                    Name="可输可选单元格",Url="View/TableView/SelectableAndEditableColumnView.xaml",TestType = TestTypeEnum.TableView
                }
            };
            for (var i = 1; i <= _navigations.Count; i++)
            {
                _navigations[i - 1].Id = i;
            }
            GridControl.ItemsSource = _navigations.OrderBy(a => a.TestType).ThenBy(a => a.Id).ToList();
        }

        private void ListenerBarCodeEnterEvent(GlobalBarCodeHook.BarCodes barCode)
        {
            var isActive = ApplicationHelper.ApplicationIsActivated();


            if (isActive)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    var win = new AutoClosedWindow("报销单打印中。。。")
                    {
                        Owner = this
                    };
                    win.ShowDialog();


                    //TT.Text += isActive + ":" + DateTime.Now + "\r\n";




                    var report = new Report();
                    report.Load("E:\\TestPrint.frx");
                    //report.RegisterData(...);
                    report.PrintSettings.ShowDialog = false;
                    report.Print();
                }));
            }
        }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            listener.Start();
        }

        private void HomeView_Unloaded(object sender, RoutedEventArgs e)
        {
            listener.Stop();
        }

        private void ViewDetailButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var id = (int)button.Tag;
            var selectedNavigation = _navigations.First(a => a.Id == id);
            var finalName = selectedNavigation.Url.Substring(0, selectedNavigation.Url.Length - 5).Replace("/", ".");
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var type = Type.GetType($"{assemblyName}.{finalName}", true);
            var instance = type.Assembly.CreateInstance($"{assemblyName}.{finalName}");
            var window = (Window)instance;
            window?.ShowDialog();
        }

        private List<ProcessingStep> _steps = new List<ProcessingStep>()
        {
            new ProcessingStep{Id = 1,Desc = "进度1"},
            new ProcessingStep{Id = 2,Desc = "进度2"},
            new ProcessingStep{Id = 3,Desc = "进度3"},
            new ProcessingStep{Id = 4,Desc = "进度4"},
            new ProcessingStep{Id = 5,Desc = "进度5"},
            new ProcessingStep{Id = 6,Desc = "进度6"},
            new ProcessingStep{Id = 7,Desc = "进度7"},
            new ProcessingStep{Id = 8,Desc = "进度8"},
            new ProcessingStep{Id = 9,Desc = "进度9"},
            new ProcessingStep{Id = 10,Desc = "进度10"},
            new ProcessingStep{Id = 11,Desc = "进度11"},
            new ProcessingStep{Id = 12,Desc = "进度12"}
        };

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var box = new ProgressBarWindow();

            _steps.ForEach(a => a.IsDone = false);
            var backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            backgroundWorker.DoWork += (sender2, e2) =>
            {
                foreach (var step in _steps)
                {
                    if (step.IsDone) continue;
                    step.IsDone = true;
                    var _currentProgress = Math.Floor(_steps.Count(a => a.IsDone) * 100 / (double)_steps.Count);
                    ProgressBarWindow.SetProgressBarValue(box, _currentProgress, step.Desc);
                    Thread.Sleep(500);
                }
            };
            backgroundWorker.RunWorkerAsync();


            box.ShowDialog2(this);
        }
    }

    public class ProcessingStep
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public bool IsDone { get; set; }
    }
}