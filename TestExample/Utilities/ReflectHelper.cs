using System;
using System.Linq;
using System.Reflection;

namespace TestExample.Utilities
{
    public class ReflectHelper
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object detail, string columnName)
        {
            var targetProperty = detail.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(a => a.Name == columnName);
            return targetProperty != null ? targetProperty.GetValue(detail, null) : null;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="columnName"></param>ConvertValueToSpecifyType
        /// <param name="value"></param>
        public static void SetPropertyValue(object detail, string columnName, object value)
        {
            var targetProperty = detail.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(a => a.Name.ToLower() == columnName.ToLower());
            if (targetProperty != null)
            {
                //targetProperty.PropertyType
                targetProperty.SetValue(detail, ConvertValueToSpecifyType(targetProperty, value), null);
            }
        }

        /// <summary>
        /// 将值转换成对应类型
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="value">目标值</param>
        /// <returns></returns>
        public static object ConvertValueToSpecifyType(PropertyInfo property, object value)
        {
            object propertyValue = null;
            if (!property.PropertyType.IsGenericType)
            {
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    //非泛型
                    if (property.PropertyType == typeof(decimal) && value.ToString().ToLower().Contains("e"))
                    {
                        propertyValue = Convert.ToDecimal(decimal.Parse(value.ToString(), System.Globalization.NumberStyles.Float));
                    }
                    else
                    {
                        propertyValue = value.ToString().ToUpper() == "NULL" ? null : Convert.ChangeType(value, property.PropertyType);
                    }
                }
                else
                {
                    if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?) || property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    {
                        propertyValue = 0;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    //泛型Nullable<>
                    var genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        if ((property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?)) && value.ToString().Contains("E"))
                        {
                            propertyValue = Convert.ToDecimal(Decimal.Parse(value.ToString().ToLower(), System.Globalization.NumberStyles.Float));
                        }
                        else
                        {
                            propertyValue = value.ToString().ToUpper() == "NULL" ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType));
                        }
                    }
                    else if (property.PropertyType.ToString()
                        .StartsWith("System.Collections.ObjectModel.ObservableCollection"))
                    {
                        propertyValue = Convert.ChangeType(value, property.PropertyType);
                    }
                    else
                    {

                    }
                }
            }
            return propertyValue;
        }
    }
}
