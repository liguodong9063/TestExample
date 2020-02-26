using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using TestExample.Model;
using TestExample.Utilities;

namespace TestExample.Infrastructure
{
    public abstract class CustomViewModelBase<T>:ViewModelBase where T : ModelBase
    {
        private ObservableCollectionHelper<T> _virtualItems;
        private ObservableCollection<object> _selectedVirtualItems = new ObservableCollection<object>();

        private object _selectedVirtualItem;
        private IList<T> _dataItems = new List<T>();
        private IList<T> _selectedDataItems = new List<T>();
        private T _selectedDataItem;
        private IDictionary<string, CustomField> _customFields;
        private IDictionary<string, bool> _columnDisplayConfigurations;
        private bool _isSearching;
        private bool _hasItems;
        private string _displayTime;

        public virtual string CustomColumnDisplayTypeKey { get; set; }

        public virtual string BillTypeKey { get; set; }

        public virtual bool IsAutoBind { get; set; } = true;

        /// <summary>
        /// 是否为窗体（用于区分UserControl，配合自动绑定使用）
        /// </summary>
        public virtual bool IsWindow => false;

        public CustomViewModelBase()
        {
            
        }

        #region 列表控件直接绑定的ItemsSource、SelectedItems、SelectedItem（列表控制只需要设置为AutoBind即可。如无必要，请勿直接手工操作）
        /// <summary>
        /// ItemsSource
        /// </summary>
        public ObservableCollectionHelper<T> VirtualItems
        {
            get => _virtualItems;
            set
            {
                if (_virtualItems == value) return;
                _virtualItems = value;
                RaisePropertyChanged("VirtualItems");
            }
        }

        /// <summary>
        /// SelectedItems
        /// </summary>
        public ObservableCollection<object> SelectedVirtualItems
        {
            get => _selectedVirtualItems;
            set
            {
                if (_selectedVirtualItems == value) return;
                _selectedVirtualItems = value;
                FillSelectedDataItems();
                RaisePropertyChanged("SelectedVirtualItems");
            }
        }

        /// <summary>
        /// SelectedItem
        /// </summary>
        public object SelectedVirtualItem
        {
            get => _selectedVirtualItem;
            set
            {
                if (_selectedVirtualItem == value) return;
                _selectedVirtualItem = value;
                SelectedDataItem = _selectedVirtualItem == null ? null : VirtualItems.Obs[(int)_selectedVirtualItem];
                RaisePropertyChanged("SelectedVirtualItem");
            }
        }
        #endregion

        #region 提供给用户使用的ItemsSource、SelectedItems、SelectedItem（该3个对象为转换之后可供方便使用的对象）
        /// <summary>
        /// ItemsSource对应的用户可见的简单对象集合
        /// </summary>
        public IList<T> DataItems
        {
            get { return _dataItems; }
            set
            {
                if (Equals(_dataItems, value)) return;
                _dataItems = value;
            }
        }

        /// <summary>
        /// SelectedItems对应的用户可见的简单对象集合
        /// </summary>
        public IList<T> SelectedDataItems
        {
            get { return _selectedDataItems ?? new List<T>(); }
            set
            {
                if (Equals(_selectedDataItems, value)) return;
                _selectedDataItems = value;
            }
        }

        /// <summary>
        /// SelectedItem对应的用户可见的简单对象
        /// </summary>
        public T SelectedDataItem
        {
            get => _selectedDataItem;
            set
            {
                if (Equals(_selectedDataItem, value)) return;
                _selectedDataItem = value;
            }
        }
        #endregion

        public IDictionary<string, CustomField> CustomFields
        {
            get => _customFields;
            set
            {
                if (_customFields == value) return;
                _customFields = value;
                RaisePropertyChanged("CustomFields");
            }
        }

        public IDictionary<string, bool> ColumnDisplayConfigurations
        {
            get => _columnDisplayConfigurations;
            set
            {
                if (_columnDisplayConfigurations == value) return;
                _columnDisplayConfigurations = value;
                RaisePropertyChanged("ColumnDisplayConfigurations");
            }
        }

        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                if (_isSearching == value) return;
                _isSearching = value;
                RaisePropertyChanged("IsSearching");
            }
        }

        /// <summary>
        /// 是否需要显示无数据提示
        /// </summary>
        public bool HasItems
        {
            get => _hasItems;
            set
            {
                _hasItems = value;
                RaisePropertyChanged("HasItems");
            }
        }

        public string DisplayTime
        {
            get => _displayTime;
            set
            {
                if (_displayTime == value) return;
                _displayTime = value;
                RaisePropertyChanged("DisplayTime");
            }
        }

        public void RefreshAsync()
        {
            if (!ValidSearch()) return;
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                    IsSearching = true;
                    //ShowText = BusyShowTextType.Loading;
                }));
                HasItems = false;
                try
                {
                    e.Result = FindDataItems();
                }
                catch (OutOfMemoryException)
                {
                    e.Result = null;
                    //todo:
                    //DialogService.ShowMessage("查询信息量过大，请缩减查询范围！", ShowType.Information);
                }
            };
            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                var result = (PagedData<T>)e.Result;
                HasItems = result == null || result.Items == null || result.Items.Count == 0;
                if (HasItems)
                {
                    if (result == null)
                        result = new PagedData<T>(new List<T>(), 0);
                    else
                        result.Items = new List<T>();
                }
                BindDataItems(result);
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { IsSearching = false; }));
            };
            backgroundWorker.RunWorkerAsync();
        }

        protected virtual bool ValidSearch()
        {
            return true;
        }

        private PagedData<T> FindDataItems()
        {
            VirtualItems = null;
            return Find();
        }

        protected abstract PagedData<T> Find();

        private void BindDataItems(PagedData<T> result)
        {
            Bind(result);
            DisplayTime = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            if (DataItems == null || DataItems.Count == 0) return;
            VirtualItems = new ObservableCollectionHelper<T>().Ini(new ObservableCollection<T>(DataItems));
        }

        protected abstract void Bind(PagedData<T> result);

        protected void InvokeAsyncOnBackDispatcher(Action action)
        {
            if (action == null) return;
            var backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            backgroundWorker.DoWork += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { IsSearching = true; }));
                action.Invoke();
            };
            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { IsSearching = false; }));
            };
            backgroundWorker.RunWorkerAsync();
        }

        protected void InitializeColumnDisplayConfigurations()
        {
            //todo:temp code
            //ColumnDisplayConfigurations = _configFacadService.GetCurrentUserCustomColumnDisplays(CustomColumnDisplayTypeKey);

            //示例：ID列显示，Name列隐藏
            //ColumnDisplayConfigurations=new Dictionary<string, bool>()
            //{
            //    {"Id",true},
            //    {"Name",true }
            //};
        }

        protected void InitializeCustomFields()
        {
            //todo:temp code
            //Debug.Assert(UserOrganization.Id != null);
            //_customFields = _configFacadService.GetCustomFieldsWithoutSource(UserOrganization.Id.Value).ToDictionary(customField => customField.Field, customField => customField);

            //示例：FieldName="Id"，Header显示主键
            //_customFields = new Dictionary<string, CustomField>()
            //{
            //    {"id", new CustomField {Field = "Id", FieldName = "主键"}},
            //    {"name", new CustomField {Field = "Name", FieldName = "名称"}}
            //};
        }

        public virtual void SetColumnDisplayConfigurations(Dictionary<string, bool> columnDisplayConfigurations)
        {
            ColumnDisplayConfigurations = columnDisplayConfigurations;
        }

        /// <summary>
        /// 填充SelectedDataItems值
        /// </summary>
        private void FillSelectedDataItems()
        {
            var selectedDataItems = new ObservableCollection<T>();
            if (_selectedVirtualItems != null)
            {
                foreach (var selectedVirtualItem in _selectedVirtualItems)
                {
                    if (VirtualItems.Count > 0 && (int)selectedVirtualItem < VirtualItems.Count)
                    {
                        selectedDataItems.Add(VirtualItems.Obs[(int)selectedVirtualItem]);
                    }
                }
            }
            SelectedDataItems = selectedDataItems;
        }
    }
}
