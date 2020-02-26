using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using TestExample.Model;
using TestExample.Utilities;
using TestExample.View;

namespace TestExample.UserControls
{
    public class ColumnDisplayConfigurationButton:Button
    {
        public static readonly DependencyProperty CustomGridControlProperty = DependencyProperty.Register("CustomGridControl", typeof(GridControl), typeof(ColumnDisplayConfigurationButton), new PropertyMetadata(null));

        public ColumnDisplayConfigurationButton()
        {
            Click += ColumnDisplayConfiguration_Click;
        }

        public CustomGridControl CustomGridControl
        {
            get => (CustomGridControl)GetValue(CustomGridControlProperty);
            set => SetValue(CustomGridControlProperty, value);
        }

        private void ColumnDisplayConfiguration_Click(object sender, RoutedEventArgs e)
        {
            var customColumnDisplayType = string.Empty;
            var customColumnDisplayTypeKeyProperty = DataContext.GetType().GetProperty("CustomColumnDisplayTypeKey");
            if (customColumnDisplayTypeKeyProperty != null)
            {
                customColumnDisplayType = customColumnDisplayTypeKeyProperty.GetValue(DataContext, null).ToString();
            }
            var columnDetailConfigs=new ObservableCollection<CustomColumnDisplayDetail>();
            try
            {
                columnDetailConfigs = GridControlColumnInformationHelper.GetCustomColumnDisplayDetails(CustomGridControl);
            }
            catch (InvalidCastException)
            {
            }
            var view = new CustomerConfigView(customColumnDisplayType, columnDetailConfigs, DataContext) { Owner = Window.GetWindow(this) };
            view.ShowDialog();
        }
    }
}
