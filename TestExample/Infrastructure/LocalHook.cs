using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TestExample.Infrastructure
{
    /// <summary>
    /// 键盘钩子类
    /// </summary>
    public class LocalHook
    {
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        public enum WH_CODE : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            /// <summary>
            /// 进程钩子
            /// </summary>
            WH_KEYBOARD = 2,

            /// <summary>
            /// 底层键盘钩子 全局钩子就是用这个

            /// </summary>
            WH_KEYBOARD_LL = 13,
        }

        public enum HC_CODE : int
        {
            HC_ACTION = 0,
            HC_GETNEXT = 1,
            HC_SKIP = 2,
            HC_NOREMOVE = 3,
            HC_NOREM = 3,
            HC_SYSMODALON = 4,
            HC_SYSMODALOFF = 5
        }

        public struct BarCodes
        {
            public int VirtKey; //虚拟码
            public int ScanCode; //扫描码
            public string KeyName; //键名
            public uint Ascll; //Ascll
            public char Chr; //字符
            public string OriginalChrs; //原始 字符
            public string OriginalAsciis; //原始 ASCII
            public string OriginalBarCode; //原始数据条码
            public string BarCode; //条码信息 保存最终的条码
            public bool IsValid; //条码是否有效
            public DateTime Time; //扫描时间,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EventMsg
        {
            public Int32 message;
            public Int32 paramL;
            public Int32 paramH;
            public Int32 Time;
            public Int32 hwnd;
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(WH_CODE idHook, HookProc lpfn, IntPtr pInstance, uint threadId);

        /// <summary>
        /// 卸载钩子
        /// </summary>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);
        /// <summary>
        /// 传递钩子
        /// </summary>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int pHookHandle, int nCodem, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// 获取全部按键状态
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns>非0表示成功</returns>
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// 获取程序集模块的句柄
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// 获取当前进程中的当前线程ID
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32", EntryPoint = "ToAscii")]
        private static extern bool ToAscii(int virtualKey, int scanCode, byte[] lpKeySate, ref uint lpChar, int uFlags);

        [DllImport("user32", EntryPoint = "GetKeyNameText")]
        private static extern int GetKeyNameText(int param, StringBuilder lpBuffer, int nSize);

        #region 私有变量

        private byte[] mKeyState = new byte[256];
        private Keys mKeyData = Keys.None; //专门用于判断按键的状态

        /// <summary>
        /// 键盘钩子句柄
        /// </summary>
        private IntPtr mKetboardHook = IntPtr.Zero;

        /// <summary>
        /// 键盘钩子委托实例
        /// </summary>
        private HookProc mKeyboardHookProcedure;

        #endregion

        #region 键盘事件

        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;

        public delegate void BardCodeDeletegate(BarCodes barCode);
        public event BardCodeDeletegate BarCodeEvent;
        #endregion

        private BarCodes _barCode;
        private int _hKeyboardHook;

        public readonly StringBuilder SbBarCode = new StringBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public LocalHook()
        {
            GetKeyboardState(this.mKeyState);
        }

        ~LocalHook()
        {
            UnInstallHook();
        }

        /// <summary>
        /// 键盘钩子处理函数
        /// </summary>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode != 0) return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
            var msg = (EventMsg)Marshal.PtrToStructure(lParam, typeof(EventMsg));
            if (wParam != 0x100)
            {
                //0x100标识键盘按下
                return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
            }
            _barCode.VirtKey = msg.message & 0xff; //虚拟吗
            _barCode.ScanCode = msg.paramL & 0xff; //扫描码
            var strKeyName = new StringBuilder(225);
            _barCode.KeyName = GetKeyNameText(_barCode.ScanCode * 65536, strKeyName, 255) > 0 ? strKeyName.ToString().Trim(' ', '\0') : "";
            var kbArray = new byte[256];
            uint uKey = 0;
            GetKeyboardState(kbArray);
            if (ToAscii(_barCode.VirtKey, _barCode.ScanCode, kbArray, ref uKey, 0))
            {
                _barCode.Ascll = uKey;
                _barCode.Chr = Convert.ToChar(uKey);
            }
            var ts = DateTime.Now.Subtract(_barCode.Time);
            if (ts.TotalMilliseconds > 50)
            {
                //时间戳，大于50 毫秒表示手动输入
                SbBarCode.Remove(0, SbBarCode.Length);
                SbBarCode.Append(_barCode.Chr.ToString());
                _barCode.OriginalChrs = " " + Convert.ToString(_barCode.Chr);
                _barCode.OriginalAsciis = " " + Convert.ToString(_barCode.Ascll);
                _barCode.OriginalBarCode = Convert.ToString(_barCode.Chr);
            }
            else
            {
                SbBarCode.Append(_barCode.Chr.ToString());
                if ((msg.message & 0xff) == 13 && SbBarCode.Length > 3)
                {
                    //回车
                    _barCode.BarCode = SbBarCode.Replace("\r", "").ToString();
                    _barCode.IsValid = true;
                    SbBarCode.Remove(0, SbBarCode.Length);
                }
            }
            _barCode.Time = DateTime.Now;
            try
            {
                if (BarCodeEvent != null && _barCode.IsValid)
                {
                    var callback = new AsyncCallback(AsyncBack);
                    var delArray = BarCodeEvent.GetInvocationList();
                    foreach (var del in delArray)
                    {
                        var barcodeDelegate = del as BardCodeDeletegate;
                        //异步调用防止界面卡死
                        barcodeDelegate?.BeginInvoke(_barCode, callback, del);
                    }
                    _barCode.BarCode = "";
                    _barCode.OriginalChrs = "";
                    _barCode.OriginalAsciis = "";
                    _barCode.OriginalBarCode = "";
                }
            }
            finally
            {
                //最后一定要 设置barCode无效
                _barCode.IsValid = false;
                _barCode.Time = DateTime.Now;
            }
            return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
        }

        //异步返回方法
        public void AsyncBack(IAsyncResult ar)
        {
            try
            {
                var barcodeDelegate = ar.AsyncState as BardCodeDeletegate;
                barcodeDelegate?.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <returns></returns>
        public bool InstallHook()
        {
            //线程钩子时一定要通过这个取得的值才是操作系统下真实的线程
            uint currentThreadId = GetCurrentThreadId();

            if (this.mKetboardHook == IntPtr.Zero)
            {
                this.mKeyboardHookProcedure = new HookProc(this.KeyboardHookProc);
                //注册线程钩子时第三个参数是空

                //GetModuleHandle 函数 替代 Marshal.GetHINSTANCE
                //防止在 framework4.0中 注册钩子不成功
                var modulePtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);

                this.mKetboardHook = SetWindowsHookEx(WH_CODE.WH_KEYBOARD, this.mKeyboardHookProcedure, modulePtr, currentThreadId);
                /*
                如果是全局钩子应该这样使用
                this.mKetboardHook = SetWindowsHookEx(WH_CODE.WH_KEYBOARD_LL, mKeyboardHookProcedure,GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                */
                if (this.mKetboardHook == IntPtr.Zero)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <returns>true表示成功 </returns>
        public bool UnInstallHook()
        {
            bool result = true;
            if (this.mKetboardHook != IntPtr.Zero)
            {
                result = UnhookWindowsHookEx(this.mKetboardHook) && result;
                this.mKetboardHook = IntPtr.Zero;
            }
            return result;
        }
    }
}