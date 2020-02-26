using System.Collections.ObjectModel;

namespace TestExample.Utilities.HotKey
{
    /// <summary>
    /// 快捷键设置管理器
    /// </summary>
    public class HotKeySettingsManager
    {
        private static HotKeySettingsManager _mInstance;

        /// <summary>
        /// 通知注册系统快捷键委托
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        public delegate bool RegisterGlobalHotKeyHandler(ObservableCollection<HotKeyModel> hotKeyModelList);
        /// <summary>
        /// 注册系统快捷键事件
        /// </summary>
        public event RegisterGlobalHotKeyHandler RegisterGlobalHotKeyEvent;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static HotKeySettingsManager Instance => _mInstance ?? (_mInstance = new HotKeySettingsManager());

        /// <summary>
        /// 注册全局快捷键
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        public bool RegisterGlobalHotKey(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return RegisterGlobalHotKeyEvent != null && RegisterGlobalHotKeyEvent(hotKeyModelList);
        }
    }
}
