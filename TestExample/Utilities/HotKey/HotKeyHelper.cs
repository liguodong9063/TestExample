﻿using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TestExample.Utilities.HotKey
{
    /// <summary>
    /// 热键注册帮助
    /// </summary>
    public static class HotKeyHelper
    {
        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private static readonly Dictionary<EHotKeySetting, int> MHotKeySettingsDic = new Dictionary<EHotKeySetting, int>();

        /// <summary>
        /// 注册全局快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册快捷键项</param>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKeySettingsDic">快捷键注册项的唯一标识符字典</param>
        /// <returns>返回注册失败项的拼接字符串</returns>
        public static string RegisterGlobalHotKey(IEnumerable<HotKeyModel> hotKeyModelList, IntPtr hwnd, out Dictionary<EHotKeySetting, int> hotKeySettingsDic)
        {
            var failList = string.Empty;
            foreach (var item in hotKeyModelList)
            {
                if (RegisterHotKey(item, hwnd)) continue;
                var str = string.Empty;
                if (item.IsSelectCtrl && !item.IsSelectShift && !item.IsSelectAlt)
                {
                    str = ModifierKeys.Control.ToString();
                }
                else if (!item.IsSelectCtrl && item.IsSelectShift && !item.IsSelectAlt)
                {
                    str = ModifierKeys.Shift.ToString();
                }
                else if (!item.IsSelectCtrl && !item.IsSelectShift && item.IsSelectAlt)
                {
                    str = ModifierKeys.Alt.ToString();
                }
                else if (item.IsSelectCtrl && item.IsSelectShift && !item.IsSelectAlt)
                {
                    str = $"{ModifierKeys.Control}+{ModifierKeys.Shift}";
                }
                else if (item.IsSelectCtrl && !item.IsSelectShift && item.IsSelectAlt)
                {
                    str = $"{ModifierKeys.Control}+{ModifierKeys.Alt}";
                }
                else if (!item.IsSelectCtrl && item.IsSelectShift && item.IsSelectAlt)
                {
                    str = $"{ModifierKeys.Shift}+{ModifierKeys.Alt}";
                }
                else if (item.IsSelectCtrl && item.IsSelectShift && item.IsSelectAlt)
                {
                    str = $"{ModifierKeys.Control}+{ModifierKeys.Shift}+{ModifierKeys.Alt}";
                }
                if (string.IsNullOrEmpty(str))
                {
                    str += item.SelectKey;
                }
                else
                {
                    str += $"+{item.SelectKey}";
                }
                str = $"{item.Name} ({str})\n\r";
                failList += str;
            }
            hotKeySettingsDic = MHotKeySettingsDic;
            return failList;
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hotKeyModel">热键待注册项</param>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns>成功返回true，失败返回false</returns>
        private static bool RegisterHotKey(HotKeyModel hotKeyModel, IntPtr hWnd)
        {
            var fsModifierKey = new ModifierKeys();
            var hotKeySetting = (EHotKeySetting)Enum.Parse(typeof(EHotKeySetting), hotKeyModel.Name);

            if (!MHotKeySettingsDic.ContainsKey(hotKeySetting))
            {
                // 全局原子不会在应用程序终止时自动删除。每次调用GlobalAddAtom函数，必须相应的调用GlobalDeleteAtom函数删除原子。
                if (HotKeyManager.GlobalFindAtom(hotKeySetting.ToString()) != 0)
                {
                    HotKeyManager.GlobalDeleteAtom(HotKeyManager.GlobalFindAtom(hotKeySetting.ToString()));
                }
                // 获取唯一标识符
                MHotKeySettingsDic[hotKeySetting] = HotKeyManager.GlobalAddAtom(hotKeySetting.ToString());
            }
            else
            {
                // 注销旧的热键
                HotKeyManager.UnregisterHotKey(hWnd, MHotKeySettingsDic[hotKeySetting]);
            }
            if (!hotKeyModel.IsUsable)
                return true;

            // 注册热键
            if (hotKeyModel.IsSelectCtrl && !hotKeyModel.IsSelectShift && !hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Control;
            }
            else if (!hotKeyModel.IsSelectCtrl && hotKeyModel.IsSelectShift && !hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Shift;
            }
            else if (!hotKeyModel.IsSelectCtrl && !hotKeyModel.IsSelectShift && hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Alt;
            }
            else if (hotKeyModel.IsSelectCtrl && hotKeyModel.IsSelectShift && !hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Control | ModifierKeys.Shift;
            }
            else if (hotKeyModel.IsSelectCtrl && !hotKeyModel.IsSelectShift && hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Control | ModifierKeys.Alt;
            }
            else if (!hotKeyModel.IsSelectCtrl && hotKeyModel.IsSelectShift && hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Shift | ModifierKeys.Alt;
            }
            else if (hotKeyModel.IsSelectCtrl && hotKeyModel.IsSelectShift && hotKeyModel.IsSelectAlt)
            {
                fsModifierKey = ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt;
            }
            return HotKeyManager.RegisterHotKey(hWnd, MHotKeySettingsDic[hotKeySetting], fsModifierKey, (int)hotKeyModel.SelectKey);
        }
    }
}
