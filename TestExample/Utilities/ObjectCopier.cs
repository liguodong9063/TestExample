using System;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace TestExample.Utilities
{
    public static class ObjectCopier
    {

        public static T Clone<T>(this T source)
        {
            //if (source is ConditionBase)
            //{
            //    var condition = source as ConditionBase;
            //    condition.OrganizationChangeAction = null;
            //}

            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(source);

            //解决反序列化后日期变化问题（UTC和LOCAL）
            json = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
            {
                var dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });

            return serializer.Deserialize<T>(json);
        }

        //public static T Clone<T>(this T source)
        //{
        //    string json = JsonConvert.SerializeObject(source);
        //    return JsonConvert.DeserializeObject<T>(json);
        //}
    }
}
