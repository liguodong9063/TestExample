using System;
using System.Globalization;
using System.Windows.Data;

namespace TestExample.Converter
{
    /// <summary>
    /// 测试Converter，用于协助判断上下文类型
    /// </summary>
    public class ObjectToItSelfTestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
