using System;
using System.Diagnostics;

namespace Hooks
{
    // References:
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx
    public enum HookType
    {
        WH_MSGFILTER = -1,
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    // References:
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx
    public abstract class Hook
    {
        private readonly HookType _hookType;
        private IntPtr _hookHandle;
        private NativeMethods.HookProc _hookProc;

        public Hook(HookType hookType)
        {
            _hookType = hookType;
            _hookProc = HookProc;
            Install();
        }

        ~Hook()
        {
            Uninstall();
        }

        protected abstract int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        protected int CallNextHook(int code, IntPtr wParam, IntPtr lParam)
        {
            return NativeMethods.CallNextHookEx(_hookHandle, code, wParam, lParam);
        }

        private void Install()
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule)
            {
                var moduleHandle = NativeMethods.GetModuleHandle(currentModule.ModuleName);
                _hookHandle = NativeMethods.SetWindowsHookEx(_hookType, _hookProc, moduleHandle, 0);
            }
        }

        private void Uninstall()
        {
            NativeMethods.UnhookWindowsHookEx(_hookHandle);
        }
    }
}
