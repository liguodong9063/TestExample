using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Xpf.Grid;

namespace TestExample.Converter
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value?.ToString(), out decimal width))
            {
                return width - 10;
            }
            else
            {
                return 0;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
