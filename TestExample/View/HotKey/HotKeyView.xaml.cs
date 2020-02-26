using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using TestExample.Utilities;
using TestExample.Utilities.HotKey;
using TestExample.ViewModel.HotKey;

namespace TestExample.View.HotKey
{
    /// <summary>
    /// HotKeyView.xaml 的交互逻辑
    /// </summary>
    public partial class HotKeyView
    {
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr _mHwnd;

        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> _mHotKeySettings = new Dictionary<EHotKeySetting, int>();

        public HotKeyView()
        {
            InitializeComponent();
            Loaded += HotKeyView_Loaded;
        }

        private void HotKeyView_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
        }

        /// <summary>
        /// WPF窗体的资源初始化完成，并且可以通过WindowInteropHelper获得该窗体的句柄用来与Win32交互后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 获取窗体句柄
            _mHwnd = new WindowInteropHelper(this).Handle;
            var hWndSource = HwndSource.FromHwnd(_mHwnd);
            // 添加处理程序
            hWndSource?.AddHook(WndProc);
        }

        /// <summary>
        /// 所有控件初始化完成后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }

        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }

        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? LoadDefaultHotKey();
            // 注册全局快捷键
            var failList = HotKeyHelper.RegisterGlobalHotKey(list, _mHwnd, out _mHotKeySettings);
            if (string.IsNullOrEmpty(failList))
                return true;
            MessageBox.Show($"无法注册下列快捷键\n\r{failList}是否要改变这些快捷键？", "提示", MessageBoxButton.YesNo);
            return false;
        }

        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            switch (msg)
            {
                case HotKeyManager.WmHotkey:
                    var sid = wideParam.ToInt32();
                    if (_mHotKeySettings.ContainsKey(EHotKeySetting.FullScreen) && sid == _mHotKeySettings[EHotKeySetting.FullScreen])
                    {
                        //全屏
                        WindowState = WindowState.Maximized;
                        
                    }else if (_mHotKeySettings.ContainsKey(EHotKeySetting.NormalScreen) &&  sid == _mHotKeySettings[EHotKeySetting.NormalScreen])
                    {
                        //标准屏
                        WindowState = WindowState.Normal;
                    }
                    else if (_mHotKeySettings.ContainsKey(EHotKeySetting.NewRow) && sid == _mHotKeySettings[EHotKeySetting.NewRow])
                    {
                        //新增行
                        var focusedElement = Keyboard.FocusedElement;
                        if (focusedElement is TextBoxBase)
                        {
                            var tableView = VisualSearchHelper.GetParentObject<DevExpress.Xpf.Grid.TableView>((Control)focusedElement, string.Empty);
                            tableView?.AddNewRow();
                        }
                        else if (focusedElement is InplaceBaseEdit)
                        {
                            var tableView = VisualSearchHelper.GetParentObject<DevExpress.Xpf.Grid.TableView>((InplaceBaseEdit)focusedElement, string.Empty);
                            tableView?.AddNewRow();
                        }
                    }
                    else if (_mHotKeySettings.ContainsKey(EHotKeySetting.DragIn) && sid == _mHotKeySettings[EHotKeySetting.DragIn])
                    {
                        //自动引入值
                        var focusedElement=Keyboard.FocusedElement;

                        if (focusedElement is TextBoxBase)
                        {
                            var gridControl = VisualSearchHelper.GetParentObject<GridControl>((Control)focusedElement, string.Empty);
                            if (gridControl != null)
                            {
                                var focusedRow = gridControl.GetFocusedRow();
                                var itemSources = ((HotKeyViewModel)gridControl.DataContext).ItemSources;
                                var index = itemSources.IndexOf((HotKeyDto)focusedRow);
                                var currentColumn = gridControl.CurrentColumn;
                                if (index > 0)
                                {
                                    var preValue = gridControl.GetCellValue(index - 1, currentColumn.FieldName);
                                    gridControl.SetCellValue(index, currentColumn.FieldName, preValue);
                                    ((TextBox)focusedElement).Text = preValue?.ToString() ?? string.Empty;
                                    ((TextBox)focusedElement).SelectAll();
                                }
                            }
                        }
                    }
                    else if (_mHotKeySettings.ContainsKey(EHotKeySetting.Entry) && sid == _mHotKeySettings[EHotKeySetting.Entry])
                    {
                        //进入
                        MessageBox.Show("我已经进入了弹出框了！");
                    }
                    else if (_mHotKeySettings.ContainsKey(EHotKeySetting.MakeLevel) && sid == _mHotKeySettings[EHotKeySetting.MakeLevel])
                    {
                        //进入
                        MessageBox.Show("我触发了找平！");
                    }
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        private void TableView_OnInitNewRow(object sender, InitNewRowEventArgs e)
        {
            var newRow = TestGrid.GetRow(e.RowHandle);
            var obj= (HotKeyDto)newRow;
            obj.Name = "New";
        }

        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<HotKeyModel> LoadDefaultHotKey()
        {

            return new ObservableCollection<HotKeyModel>(XmlHelper.LoadHotKeys());

            //var hotKeyList = new ObservableCollection<HotKeyModel>
            //{
            //    new HotKeyModel
            //    {
            //        Name = EHotKeySetting.FullScreen.ToString(),
            //        IsUsable = true,
            //        IsSelectCtrl = true,
            //        IsSelectAlt = false,
            //        IsSelectShift = false,
            //        SelectKey = EKey.F
            //    },new HotKeyModel
            //    {
            //        Name = EHotKeySetting.NormalScreen.ToString(),
            //        IsUsable = true,
            //        IsSelectCtrl = true,
            //        IsSelectAlt = false,
            //        IsSelectShift = false,
            //        SelectKey = EKey.N
            //    },new HotKeyModel
            //    {
            //        Name = EHotKeySetting.DragIn.ToString(),
            //        IsUsable = true,
            //        IsSelectCtrl = true,
            //        IsSelectAlt = false,
            //        IsSelectShift = false,
            //        SelectKey = EKey.Down
            //    },new HotKeyModel
            //    {
            //        Name = EHotKeySetting.NewRow.ToString(),
            //        IsUsable = true,
            //        IsSelectCtrl = true,
            //        IsSelectAlt = false,
            //        IsSelectShift = true,
            //        SelectKey = EKey.N
            //    },new HotKeyModel
            //    {
            //        Name = EHotKeySetting.Entry.ToString(),
            //        IsUsable = true,
            //        IsSelectCtrl = true,
            //        IsSelectAlt = false,
            //        IsSelectShift = true,
            //        SelectKey = EKey.Space
            //    },new HotKeyModel
            //    {
            //        Name = EHotKeySetting.MakeLevel.ToString(),
            //        IsUsable = true,
            //        IsSelectCtrl = true,
            //        IsSelectAlt = false,
            //        IsSelectShift = false,
            //        SelectKey = EKey.Decimal
            //    }
            //};
            //return hotKeyList;
        }
    }
}