using System;
using System.Globalization;
using System.Windows.Data;
using TestExample.View.TreeListView;

namespace TestExample.Converter
{
    public class TestObject4GetParentIdConverter: IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedRow= (TreeListView4.TestObject) value[0];
            if (selectedRow == null) return false;
            var parentId = (int)value[1];
            return selectedRow.ParentId == parentId;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
