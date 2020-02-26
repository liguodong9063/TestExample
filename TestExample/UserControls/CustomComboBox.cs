using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DevExpress.Xpf.CodeView;
using NLog;
using TestExample.Infrastructure.Messages;
using TestExample.Infrastructure.Messages.Token;
using TestExample.Utilities;

namespace TestExample.UserControls
{
    /// <summary>
    /// 下拉控件（不能应用到列表控件中）
    /// </summary>
    public class CustomComboBox : ComboBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 控件下拉数据源
        /// </summary>
        public static readonly DependencyProperty CustomItemsSourceProperty = DependencyProperty.Register("CustomItemsSource", typeof(IEnumerable), typeof(CustomComboBox), new PropertyMetadata(CustomItemsSourceProertyChangedCallBack));
        /// <summary>
        /// 控件选中值(切记使用Mode=TwoWay）
        /// </summary>
        public static readonly DependencyProperty CustomSelectedValueProperty = DependencyProperty.Register("CustomSelectedValue", typeof(object), typeof(CustomComboBox), new PropertyMetadata(CustomSelectedValueProertyChangedCallBack));
        /// <summary>
        /// 初始绑定显示值（用于数据项已经被删除,但是仍然需要显示）
        /// </summary>
        public static readonly DependencyProperty CustomInitialDisplayTextProperty = DependencyProperty.Register("CustomInitialDisplayText", typeof(object), typeof(CustomComboBox), new PropertyMetadata(null, null));
        /// <summary>
        /// 用于显示的属性名
        /// </summary>
        public string CustomDisplayMemberPath { private get; set; } = "Name";
        /// <summary>
        /// 选中值的属性名
        /// </summary>
        public string CustomSelectedValuePath { private get; set; } = "Id";
        /// <summary>
        /// 数据项是否显示
        /// 1(int)或true(bool)显示，其它隐藏
        /// </summary>
        public string CustomIsNeedDisplayPropertyPath { private get; set; }

        public CustomComboBox()
        {
            Loaded += (sender, e) =>
            {
                //设置DisplayMemberPath、SelectedValuePath
                DisplayMemberPath = CustomDisplayMemberPath;
                SelectedValuePath = CustomSelectedValuePath;
                //设置值为默认双向绑定
                SetBindingModeToTwoWay();
                CustomNotificationMessenger.Register<string>(this, NotificationMessageToken.CloseComboBoxItemArea, message =>
                {
                    //页面滚动条滚动时自动关闭打开的ComboBox下拉项
                    IsDropDownOpen = false;
                });
            };
            Unloaded += (sender, e) =>
            {
                CustomNotificationMessenger.Unregister<string>(this);
            };
            SelectionChanged += CustomComboBox_SelectionChanged;
        }

        public IEnumerable CustomItemsSource
        {
            private get => (IEnumerable)GetValue(CustomItemsSourceProperty);
            set => SetValue(CustomItemsSourceProperty, value);
        }

        public object CustomSelectedValue
        {
            private get => GetValue(CustomSelectedValueProperty);
            set => SetValue(CustomSelectedValueProperty, value);
        }

        public object CustomInitialDisplayText
        {
            private get => GetValue(CustomInitialDisplayTextProperty);
            set => SetValue(CustomInitialDisplayTextProperty, value);
        }

        /// <summary>
        /// 选中项改动事件（目前主要用于更改CustomSelectValue）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CustomSelectedValuePath)) return;
            if (e.AddedItems.Count <= 0) return;
            var itemType = e.AddedItems[0].GetType();
            var selectedValuePathProperty = itemType.GetProperty(CustomSelectedValuePath);
            if (selectedValuePathProperty == null)
            {
                Logger.Error($"SelectedValuePath is not valid(itemType:{itemType};selectedValuePath:{CustomSelectedValuePath}!");
                return;
            }
            var selectedValue = selectedValuePathProperty.GetValue(e.AddedItems[0], null);
            if (selectedValue == null)
            {
                if (CustomSelectedValue != null)
                {
                    CustomSelectedValue = null;
                }
            }
            else
            {
                if (selectedValue.Equals(CustomSelectedValue)) return;
                CustomSelectedValue = selectedValue;
            }
        }

        private static void CustomItemsSourceProertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as CustomComboBox;
            if (sender == null) return;
            sender.InitializeItemsSource();
            sender.SelectedValue = sender.CustomSelectedValue;
        }

        private static void CustomSelectedValueProertyChangedCallBack(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as CustomComboBox;
            if (sender?.CustomItemsSource == null) return;
            if (Equals(sender.SelectedValue, dependencyPropertyChangedEventArgs.NewValue)) return;
            sender.InitializeItemsSource();
            sender.SelectedValue = sender.CustomSelectedValue;
        }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void InitializeItemsSource()
        {
            var filtedDataSource = new List<object>();
            if (CustomItemsSource == null) return;

            PropertyInfo needDisplayProperty = null;
            PropertyInfo selectedValueProperty = null;

            foreach (var item in CustomItemsSource)
            {
                if (CustomIsNeedDisplayPropertyPath == null)
                {
                    //无是否需要显示属性
                    filtedDataSource.Add(item);
                }
                else
                {
                    if (needDisplayProperty == null)
                    {
                        needDisplayProperty = item.GetType().GetProperty(CustomIsNeedDisplayPropertyPath);
                    }

                    if (needDisplayProperty != null)
                    {
                        var propertyType = needDisplayProperty.PropertyType;
                        var needDisplay = ObjectToIsNeedDisplayConverter(needDisplayProperty.GetValue(item, null), propertyType);
                        if (needDisplay)
                        {
                            //数据项未被禁用
                            filtedDataSource.Add(item);
                        }
                        else
                        {
                            //数据项已被禁用
                            //当前选中的数据源本身就是被禁用的情况需要显示出来（主要解决数据源被禁用后显示空白的问题）
                            if (selectedValueProperty == null)
                            {
                                selectedValueProperty = item.GetType().GetProperty(CustomSelectedValuePath);
                            }

                            if (selectedValueProperty == null)
                            {
                                Logger.Error($"选中值属性名无效(Method:InitializeItemsSource,SelectedValuePath:{CustomSelectedValuePath}！");
                            }
                            else
                            {
                                var value = selectedValueProperty.GetValue(item, null);
                                if ((CustomSelectedValue ?? string.Empty).Equals(value ?? string.Empty))
                                {
                                    filtedDataSource.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        filtedDataSource.Add(item);
                    }
                }
            }
            //自动填充数据源
            FillDefaultSourceItem(filtedDataSource);
            ItemsSource = filtedDataSource;
        }

        /// <summary>
        /// 自动填充数据源（当绑定的数据在数据源中不存在时）
        /// </summary>
        /// <param name="filtedDataSource"></param>
        private void FillDefaultSourceItem(List<object> filtedDataSource)
        {
            try
            {
                if (CustomSelectedValuePath == null || CustomDisplayMemberPath == null || CustomInitialDisplayText == null) return;
                Type sourceItemType = null;
                var itemsSourceType = CustomItemsSource.GetType();
                if (itemsSourceType.IsGenericType)
                {
                    var genericArgTypes = itemsSourceType.GetGenericArguments();
                    if (genericArgTypes.Length == 1)
                    {
                        sourceItemType = genericArgTypes[0];
                    }
                }
                if (sourceItemType == null) return;
                var hasDefaultItem = filtedDataSource.Any(item =>
                {
                    var propertyInfo = item.GetType().GetProperty(CustomSelectedValuePath);
                    if (propertyInfo == null) return true;
                    var propertyValue = propertyInfo.GetValue(item, null);
                    return (propertyValue ?? string.Empty) == (CustomSelectedValue ?? string.Empty) ||
                           propertyValue != null && propertyValue.Equals(CustomSelectedValue ?? string.Empty);
                });
                if (hasDefaultItem) return;
                var instance = Activator.CreateInstance(sourceItemType, null);
                ReflectHelper.SetPropertyValue(instance, CustomSelectedValuePath, CustomSelectedValue);
                if (CustomDisplayMemberPath != CustomSelectedValuePath)
                {
                    ReflectHelper.SetPropertyValue(instance, CustomDisplayMemberPath, CustomInitialDisplayText);
                }
                filtedDataSource.Add(instance);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 将Int/Bool类型的值转换成Bool类型（用于判断该数据源是否需要显示在数据源中）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyType">属性类型</param>
        /// <returns></returns>
        private static bool ObjectToIsNeedDisplayConverter(object value, Type propertyType)
        {
            if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                if (int.TryParse(value?.ToString(), out int intValue))
                {
                    return intValue == 1;
                }
                return false;
            }
            if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return bool.TryParse(value?.ToString(), out bool boolValue) && boolValue;
            }
            Logger.Info("ObjectToIsNeedDisplayConverter:只支持布尔或整型！");
            return false;
        }

        /// <summary>
        /// 设置CustomSelectedValue绑定方式为TwoWay
        /// </summary>
        private void SetBindingModeToTwoWay()
        {
            //重置Binding，设置成TwoWay
            var bindingExpression = GetBindingExpression(CustomSelectedValueProperty);
            if (bindingExpression == null) return;
            var originBinding = bindingExpression.ParentBinding;
            var newBinding = new Binding(originBinding.Path.Path)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = originBinding.Converter,
                Mode = BindingMode.TwoWay,
                RelativeSource = originBinding.RelativeSource,
                NotifyOnValidationError = originBinding.NotifyOnValidationError
            };
            newBinding.ValidationRules.AddRange(originBinding.ValidationRules);
            BindingOperations.SetBinding(this, CustomSelectedValueProperty, newBinding);
        }
    }
}
