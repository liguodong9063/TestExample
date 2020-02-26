using System;
using System.Collections;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfControls.Editors
{
    [TemplatePart(Name = PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartSelector, Type = typeof(Selector))]
    public class AutoCompleteTextBox : Control
    {
        #region "Fields"

        public const string PartEditor = "PART_Editor";
        public const string PartPopup = "PART_Popup";
        public const string PartSelector = "PART_Selector";
        public const string PartScrollViewer = "Part_ScrollViewer";

        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(200));
        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register("DisplayMember", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register("IconPlacement", typeof(IconPlacement), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(IconPlacement.Left));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(AutoCompleteTextBox));
        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register("LoadingContent", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(ISuggestionProvider), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// 特殊功能（核算项类别）
        /// </summary>
        public static readonly DependencyProperty AccountingTypeIdProperty = DependencyProperty.Register("AccountingTypeId", typeof(int?), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 是否列表控件中使用
        /// </summary>
        public static readonly DependencyProperty IsInColumnProperty = DependencyProperty.Register("IsInColumn", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// 是否自动宽度（数据源弹出框）
        /// </summary>
        public static readonly DependencyProperty IsPopupAutoWidthProperty = DependencyProperty.Register("IsPopupAutoWidth", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// 最大宽度（数据源弹出框）
        /// </summary>
        public static readonly DependencyProperty MaxPopupAutoWidthProperty = DependencyProperty.Register("MaxPopupAutoWidth", typeof(double), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(0d));


        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));

        private BindingEvaluator _bindingEvaluator;

        private TextBox _editor;

        private DispatcherTimer _fetchTimer;

        private string _filter;

        private bool _isUpdatingText;

        private Selector _itemsSelector;

        private ScrollViewer _scrollViewer;

        private Popup _popup;

        private SelectionAdapter _selectionAdapter;

        private bool _selectionCancelled;

        private SuggestionsAdapter _suggestionsAdapter;

        /// <summary>
        /// 是否初始（在TextChanged时改成false）
        /// </summary>
        private bool _isInitial = true;


        #endregion

        #region "Constructors"

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        public AutoCompleteTextBox()
        {
            Loaded += AutoCompleteTextBox_Loaded;
        }

        #endregion

        #region "Properties"

        public BindingEvaluator BindingEvaluator
        {
            get => _bindingEvaluator;
            set => _bindingEvaluator = value;
        }

        public int Delay
        {
            get => (int)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        public string DisplayMember
        {
            get => (string)GetValue(DisplayMemberProperty);
            set => SetValue(DisplayMemberProperty, value);
        }

        public TextBox Editor
        {
            get => _editor;
            set => _editor = value;
        }

        public DispatcherTimer FetchTimer
        {
            get => _fetchTimer;
            set => _fetchTimer = value;
        }

        public string Filter
        {
            get => _filter;
            set => _filter = value;
        }

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public IconPlacement IconPlacement
        {
            get => (IconPlacement)GetValue(IconPlacementProperty);
            set => SetValue(IconPlacementProperty, value);
        }

        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public Selector ItemsSelector
        {
            get => _itemsSelector;
            set => _itemsSelector = value;
        }

        public ScrollViewer ScrollViewer
        {
            get => _scrollViewer;
            set => _scrollViewer = value;
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get => ((DataTemplateSelector)(GetValue(ItemTemplateSelectorProperty)));
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        public object LoadingContent
        {
            get => GetValue(LoadingContentProperty);
            set => SetValue(LoadingContentProperty, value);
        }

        public Popup Popup
        {
            get => _popup;
            set => _popup = value;
        }

        public ISuggestionProvider Provider
        {
            get => (ISuggestionProvider)GetValue(ProviderProperty);
            set => SetValue(ProviderProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public SelectionAdapter SelectionAdapter
        {
            get => _selectionAdapter;
            set => _selectionAdapter = value;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// 核算项类别ID（该参数将会额外窗体给SuggestionProvider）
        /// </summary>
        public int? AccountingTypeId
        {
            get => (int?)GetValue(AccountingTypeIdProperty);
            set => SetValue(AccountingTypeIdProperty, value);
        }

        /// <summary>
        /// 是否再列表EditTemplate中使用
        /// </summary>
        public bool IsInColumn
        {
            get => (bool)GetValue(IsInColumnProperty);
            set => SetValue(IsInColumnProperty, value);
        }

        /// <summary>
        /// 是否自动宽度（数据源弹出框）
        /// </summary>
        public bool IsPopupAutoWidth
        {
            get => (bool)GetValue(IsPopupAutoWidthProperty);
            set => SetValue(IsPopupAutoWidthProperty, value);
        }

        /// <summary>
        /// 最大宽度（弹出框）
        /// </summary>
        public double MaxPopupAutoWidth
        {
            get => (double)GetValue(MaxPopupAutoWidthProperty);
            set => SetValue(MaxPopupAutoWidthProperty, value);
        }

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public Func<bool> CanTrigger;

        /// <summary>
        /// 声明路由事件
        /// 参数:要注册的路由事件名称，路由事件的路由策略，事件处理程序的委托类型(可自定义)，路由事件的所有者类类型
        /// </summary>
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChangedEvent", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventArgs<object>), typeof(AutoCompleteTextBox));

        /// <summary>
        /// 处理各种路由事件的方法 
        /// </summary>
        public event RoutedEventHandler SelectionChanged
        {
            //将路由事件添加路由事件处理程序
            add => AddHandler(SelectionChangedEvent, value);
            //从路由事件处理程序中移除路由事件
            remove => RemoveHandler(SelectionChangedEvent, value);
        }
        #endregion

        #region "Methods"

        public static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AutoCompleteTextBox;
            if (control == null) return;
            if (!(control.Editor != null & !control._isUpdatingText)) return;
            control._isUpdatingText = true;
            control.Editor.Text = control.BindingEvaluator.Evaluate(e.NewValue);
            control._isUpdatingText = false;
        }

        private void ScrollToSelectedItem()
        {
            var listBox = ItemsSelector as ListBox;
            if (listBox?.SelectedItem != null)
                listBox.ScrollIntoView(listBox.SelectedItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Editor = Template.FindName(PartEditor, this) as TextBox;
            Popup = Template.FindName(PartPopup, this) as Popup;
            ItemsSelector = Template.FindName(PartSelector, this) as Selector;
            ScrollViewer = Template.FindName(PartScrollViewer, this) as ScrollViewer;

            BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            if (Editor != null)
            {
                Editor.TextChanged += OnEditorTextChanged;
                Editor.PreviewKeyDown += OnEditorKeyDown;
                Editor.LostFocus += OnEditorLostFocus;

                if (SelectedItem != null)
                {
                    Editor.Text = BindingEvaluator.Evaluate(SelectedItem);
                }
            }

            if (Popup != null)
            {
                Popup.StaysOpen = false;
                Popup.Opened += OnPopupOpened;
                Popup.Closed += OnPopupClosed;
            }

            if (ScrollViewer != null)
            {
                ScrollViewer.Loaded += ScrollViewer_Loaded;
            }

            if (ItemsSelector == null) return;
            ItemsSelector.MouseMove += ItemsSelector_MouseMove;

            SelectionAdapter = new SelectionAdapter(ItemsSelector);
            SelectionAdapter.Commit += OnSelectionAdapterCommit;
            SelectionAdapter.Cancel += OnSelectionAdapterCancel;
            SelectionAdapter.SelectionChanged += OnSelectionAdapterSelectionChanged;
        }

        private ListBoxItem _lastSelectedListBoxItem;

        private void ItemsSelector_MouseMove(object sender, MouseEventArgs e)
        {
            //仅在列表控件中支持鼠标移动时自动选择对象
            if (!IsInColumn) return;

            var size = ItemsSelector.Items.Count;
            if (size <= 0) return;
            var index = 0;
            foreach (var itemsSelectorItem in ItemsSelector.Items)
            {
                if (ItemsSelector.ItemContainerGenerator.ContainerFromItem(itemsSelectorItem) is ListBoxItem listBoxItem&&listBoxItem.IsMouseOver)
                {
                    if (_lastSelectedListBoxItem != listBoxItem)
                    {
                        _lastSelectedListBoxItem = listBoxItem;

                        if (SelectionAdapter == null) return;
                        if (IsDropDownOpen)
                        {
                            SelectionAdapter.SelectorControl.SelectedIndex = index;

                            SetSelectedItem(ItemsSelector.SelectedItem, true);
                            OnSelectionAdapterSelectionChanged();
                        }
                    }
                }
                index++;
            }
        }

        private string GetDisplayText(object dataItem)
        {
            if (BindingEvaluator == null)
            {
                BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            return string.IsNullOrEmpty(DisplayMember) ? dataItem.ToString() : BindingEvaluator.Evaluate(dataItem);
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (SelectionAdapter == null) return;
            if (IsDropDownOpen)
            {
                SelectionAdapter.HandleKeyDown(e);
            }
            else
            {
                //键盘方向键的上与下将触发下拉框打开
                IsDropDownOpen = e.Key == Key.Down || e.Key == Key.Up;

                var canTrigger = CanTrigger?.Invoke() ?? true;
                if (_isUpdatingText||!canTrigger)
                    return;

                if (FetchTimer == null)
                {
                    FetchTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(Delay)};
                    FetchTimer.Tick += OnFetchTimerTick;
                }
                FetchTimer.IsEnabled = false;
                FetchTimer.Stop();


                IsLoading = true;
                ItemsSelector.ItemsSource = null;
                FetchTimer.IsEnabled = true;
                FetchTimer.Start();
            }
        }

        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                IsDropDownOpen = false;
            }
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            var canTrigger = CanTrigger?.Invoke() ?? true;
            if (_isUpdatingText||!canTrigger)
                return;
            if (FetchTimer == null)
            {
                FetchTimer = new DispatcherTimer();
                FetchTimer.Interval = TimeSpan.FromMilliseconds(Delay);
                FetchTimer.Tick += OnFetchTimerTick;
            }
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();

            //todo:
            //var hasChanged = false;
            //if (ItemsSelector.SelectedItem == null && SelectedItem == null)
            //{
            //    if (_isInitial)
            //    {
            //        hasChanged = true;
            //    }
            //}
            //else
            //{
            //    if (ItemsSelector.SelectedItem != SelectedItem)
            //    {
            //        hasChanged = true;
            //    }
            //}
            //SetSelectedItem(ItemsSelector.SelectedItem, hasChanged);

            SetSelectedItem(ItemsSelector.SelectedItem, true);

            if (Editor.Text.Length > 0)
            {
                IsLoading = true;
                IsDropDownOpen = true;
                ItemsSelector.ItemsSource = null;
                FetchTimer.IsEnabled = true;
                FetchTimer.Start();
            }
            else
            {
                IsDropDownOpen = false;
            }
            _isUpdatingText = false;

            //标记设置为非初始
            if (_isInitial)
            {
                _isInitial = false;
            }
        }

        private void OnFetchTimerTick(object sender, EventArgs e)
        {
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();
            if (Provider == null || ItemsSelector == null) return;
            Filter = Editor.Text;
            if (_suggestionsAdapter == null)
            {
                _suggestionsAdapter = new SuggestionsAdapter(this);
            }
            _suggestionsAdapter.GetSuggestions(Filter);
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            if (!_selectionCancelled)
            {
                OnSelectionAdapterCommit();
            }
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            _selectionCancelled = false;
            ItemsSelector.SelectedItem = SelectedItem;
        }

        private void OnSelectionAdapterCancel()
        {
            _isUpdatingText = true;
            Editor.Text = SelectedItem == null ? Filter : GetDisplayText(SelectedItem);
            Editor.SelectionStart = Editor.Text.Length;
            Editor.SelectionLength = 0;
            _isUpdatingText = false;
            IsDropDownOpen = false;
            _selectionCancelled = true;
        }

        private void OnSelectionAdapterCommit()
        {
            if (ItemsSelector.SelectedItem == null) return;
            var hasChanged = SelectedItem != ItemsSelector.SelectedItem;
            SelectedItem = ItemsSelector.SelectedItem;
            _isUpdatingText = true;
            Editor.Text = GetDisplayText(ItemsSelector.SelectedItem);

            SetSelectedItem(ItemsSelector.SelectedItem,hasChanged);

            _isUpdatingText = false;
            IsDropDownOpen = false;
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            _isUpdatingText = true;

            Editor.Text = ItemsSelector.SelectedItem == null ? Filter : GetDisplayText(ItemsSelector.SelectedItem);

            Editor.SelectionStart = Editor.Text.Length;
            Editor.SelectionLength = 0;
            ScrollToSelectedItem();

            _isUpdatingText = false;
        }

        /// <summary>
        /// 触发自定义选中改变事件
        /// </summary>
        private void TriggerCustomSelectionChangedEvent()
        {
            //引用自定义路由事件
            var args = new RoutedEventArgs(SelectionChangedEvent, this);
            RaiseEvent(args);
        }

        private void SetSelectedItem(object item,bool hasChanged)
        {
            _isUpdatingText = true;
            if (SelectedItem != item)
            {
                SelectedItem = item;
            }
            if (hasChanged)
            {
                TriggerCustomSelectionChangedEvent();
            }
            _isUpdatingText = false;
        }
        #endregion

        #region "Nested Types"

        private class SuggestionsAdapter
        {

            #region "Fields"

            private readonly AutoCompleteTextBox _control;

            private string _filter;
            #endregion

            #region "Constructors"

            public SuggestionsAdapter(AutoCompleteTextBox control)
            {
                _control = control;
            }

            #endregion

            #region "Methods"

            public void GetSuggestions(string searchText)
            {
                _filter = searchText;
                _control.IsLoading = true;
                ParameterizedThreadStart thInfo = GetSuggestionsAsync;
                var thread = new Thread(thInfo);

                _control.Provider.AccountingTypeId = _control.AccountingTypeId;

                thread.Start(new object[] {
                    searchText,
                    _control.Provider
                });
            }

            private void DisplaySuggestions(IEnumerable suggestions, string filter)
            {
                if (_filter != filter)
                {
                    return;
                }
                if (!_control.IsDropDownOpen) return;
                _control.IsLoading = false;
                _control.ItemsSelector.ItemsSource = suggestions;


                


                //todo:默认选中第一项
                //_control.ItemsSelector.SelectedIndex = 0;
                _control.IsDropDownOpen = _control.ItemsSelector.HasItems;
            }

            private void GetSuggestionsAsync(object param)
            {
                if (!(param is object[] args)) return;

                var searchText = Convert.ToString(args[0]);
                var provider = args[1] as ISuggestionProvider;
                var list = provider?.GetSuggestions(searchText);
                _control.Dispatcher.BeginInvoke(new Action<IEnumerable, string>(DisplaySuggestions), DispatcherPriority.Background, list, searchText);
            }
            #endregion
        }
        #endregion

        private void AutoCompleteTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsInColumn) return;
            var editor = (sender as AutoCompleteTextBox)?.Editor;
            if (editor == null) return;
            editor.Focus();
            Keyboard.Focus(editor);
            editor.SelectAll();
        }

        /// <summary>
        /// ScrollViewer加载完成事件（控制Popup宽度）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            //自动宽度时只需限制最大宽度，非自动宽度时Popup宽度永远为当前控件宽度
            if (!IsPopupAutoWidth)
            {
                //数据源弹出框非自动宽度
                var binding = new Binding("ActualWidth")
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
                };
                BindingOperations.SetBinding(Popup, WidthProperty, binding);
            }
            else
            {
                //数据源弹出框自动宽度
                var actualWidthBinding = new Binding("ActualWidth")
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
                };
                BindingOperations.SetBinding(Popup, MinWidthProperty, actualWidthBinding);
                //控制弹出框最大宽度
                if (MaxPopupAutoWidth > 0)
                {
                    Popup.MaxWidth = MaxPopupAutoWidth;
                }
            }
        }

        /// <summary>
        /// 清除控件值
        /// </summary>
        public void ClearText()
        {
            _isUpdatingText = true;
            ItemsSelector.SelectedItem = null;
            SetSelectedItem(ItemsSelector.SelectedItem, false);
            Editor.Text = BindingEvaluator.Evaluate(null);
            _isUpdatingText = false;
        }
    }
}