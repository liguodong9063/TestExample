using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Xpf.Grid;

namespace TestExample.Converter
{
    public class MyConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rowData = (RowData)value;
            Debug.Assert(rowData!=null);
            var index=rowData.RowHandle;
            return (int)Math.Floor(index.Value / 2d) % 2==0? "Green" : "Blue";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
