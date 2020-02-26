using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TestExample.Infrastructure
{
    /// <summary>
    /// 获取键盘输入或者USB扫描枪数据 可以是没有焦点 应为使用的是全局钩子
    /// USB扫描枪 是模拟键盘按下
    /// 这里主要处理扫描枪的值，手动输入的值不太好处理
    /// </summary>
    public class GlobalBarCodeHook
    {
        public delegate void BardCodeDeletegate(BarCodes barCode);
        public event BardCodeDeletegate BarCodeEnterEvent;

        //定义成静态，这样不会抛出回收异常
        private static HookProc _hookProc;

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

        private struct EventMsg
        {
            public int message;
            public int paramL;
            public int paramH;
            public int time;
            public int hwnd;
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <param name="idHook">钩子类型 13键盘和14鼠标,用来对底层输入事件监视</param>
        /// <param name="lpfn">函数指针</param>
        /// <param name="hInstance">包含SetWindowsHookEx函数的模块地址,user32.dll入口</param>
        /// <param name="threadId">0表示系统钩子</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "GetKeyNameText")]
        private static extern int GetKeyNameText(int param, StringBuilder lpBuffer, int nSize);

        [DllImport("user32", EntryPoint = "GetKeyboardState")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32", EntryPoint = "ToAscii")]
        private static extern bool ToAscii(int virtualKey, int scanCode, byte[] lpKeySate, ref uint lpChar, int uFlags);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        private BarCodes _barCode;
        private int _hKeyboardHook;

        public readonly StringBuilder SbBarCode = new StringBuilder();

        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode != 0) return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
            var msg = (EventMsg)Marshal.PtrToStructure(lParam, typeof(EventMsg));
            if (wParam != 0x100) return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
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
                if (BarCodeEnterEvent != null && _barCode.IsValid)
                {
                    var callback = new AsyncCallback(AsyncBack);
                    Debug.Assert(BarCodeEnterEvent != null, nameof(BarCodeEnterEvent) + " != null");
                    var delArray = BarCodeEnterEvent.GetInvocationList();
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
        private void AsyncBack(IAsyncResult ar)
        {
            try
            {
                var barcodeDelegate = ar.AsyncState as BardCodeDeletegate;
                barcodeDelegate?.EndInvoke(ar);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        //安装钩子
        public bool Start()
        {
            if (_hKeyboardHook != 0) return (_hKeyboardHook != 0);
            _hookProc = KeyboardHookProc;
            //GetModuleHandle 函数 替代 Marshal.GetHINSTANCE
            //防止在 framework4.0中 注册钩子不成功
            var modulePtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            //WH_KEYBOARD_LL=13
            //全局钩子 WH_KEYBOARD_LL
            _hKeyboardHook = SetWindowsHookEx(13, _hookProc, modulePtr, 0);
            return (_hKeyboardHook != 0);
        }

        //卸载钩子
        public bool Stop()
        {
            return _hKeyboardHook == 0 || UnhookWindowsHookEx(_hKeyboardHook);
        }
    }
}