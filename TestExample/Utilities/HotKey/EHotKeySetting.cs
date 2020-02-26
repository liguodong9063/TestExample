using System.ComponentModel;

namespace TestExample.Utilities.HotKey
{
    /// <summary>
    /// 快捷键设置项枚举
    /// </summary>
    public enum EHotKeySetting
    {
        [Description("全屏")]
        FullScreen = 0,
        [Description("恢复普通大小")]
        NormalScreen=1,
        [Description("引入上行值")]
        DragIn=2,
        [Description("新增行")]
        NewRow=3,
        [Description("进入")]
        Entry=4,
        [Description("自动找平")]
        MakeLevel
    }
}
