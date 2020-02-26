using System.Collections.Generic;

namespace TestExample.Common
{
    /// <summary>
    /// 列类型宽度
    /// </summary>
    public class ColumnTypeWidth
    {
        public static Dictionary<ColumnTypes, int[]> ColumnWidths { get; } =
            new Dictionary<ColumnTypes, int[]>
            {
                {ColumnTypes.Default, new[] {0, 100, 0}},
                {ColumnTypes.BillNo, new[] {0, 100, 0}},
                {ColumnTypes.Name, new[] {0, 120, 0}},
                {ColumnTypes.Special, new[] {0, 300, 0}}
            };
    }
}
