using System;
using System.Collections.Generic;
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
    public class Hook
    {
        public delegate int HookProc(WH_CODE nCode, Int32 wParam, IntPtr lParam);
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
        public static extern int CallNextHookEx(IntPtr pHookHandle, WH_CODE nCodem, Int32 wParam, IntPtr lParam);

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

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public Hook()
        {
            GetKeyboardState(this.mKeyState);
        }

        ~Hook()
        {
            UnInstallHook();
        }

        /// <summary>
        /// 键盘钩子处理函数
        /// </summary>
        private int KeyboardHookProc(WH_CODE nCode, Int32 wParam, IntPtr lParam)
        {
            /*全局钩子应该这样设定
             KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
            */
            // 定义为线程钩子时，wParam的值是击打的按键，与Keys里的对应按键相同
            if ((nCode == (int)HC_CODE.HC_ACTION) && (this.OnKeyDown != null || this.OnKeyUp != null))
            {
                mKeyData = (Keys)wParam;
                KeyEventArgs keyEvent = new KeyEventArgs(mKeyData);
                //这里简单的通过lParam的值的正负情况与按键的状态相关联
                if (lParam.ToInt32() > 0 && this.OnKeyDown != null)
                {
                    this.OnKeyDown(this, keyEvent);
                }
                else if (lParam.ToInt32() < 0 && this.OnKeyUp != null)
                {
                    this.OnKeyUp(this, keyEvent);
                }
            }

            return CallNextHookEx(this.mKetboardHook, nCode, wParam, lParam);
        }
        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <returns></returns>
        public bool InstallHook()
        {
            //线程钩子时一定要通过这个取得的值才是操作系统下真实的线程
            uint result = GetCurrentThreadId();

            if (this.mKetboardHook == IntPtr.Zero)
            {
                this.mKeyboardHookProcedure = new HookProc(this.KeyboardHookProc);
                //注册线程钩子时第三个参数是空
                this.mKetboardHook = SetWindowsHookEx(WH_CODE.WH_KEYBOARD, this.mKeyboardHookProcedure, IntPtr.Zero, result);
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