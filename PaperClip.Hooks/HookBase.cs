using System;
using System.Diagnostics;

namespace PaperClip.Hooks
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx
    public abstract class HookBase
    {
        private readonly HookType _hookType;
        private readonly NativeMethods.HookProc _hookProc;
        private IntPtr _hookHandle;

        protected HookBase(HookType hookType)
        {
            _hookType = hookType;
            _hookProc = HookProc;
            Install();
        }

        ~HookBase()
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
            using (var currentProcess = Process.GetCurrentProcess())
            using (var currentModule = currentProcess.MainModule)
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
