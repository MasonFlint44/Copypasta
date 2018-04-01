using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using PaperClip.Hooks.KeyboardHook.Interfaces;

namespace PaperClip.Hooks.KeyboardHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644985(v=vs.85).aspx
    public class KeyboardHook : HookBase, IKeyboardHook
    {
        public event EventHandler<IKeyboardHookEventArgs> KeyUp;
        public event EventHandler<IKeyboardHookEventArgs> KeyDown;

        public KeyboardHook() : base(HookType.WH_KEYBOARD_LL) { }

        protected override int HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHook(nCode, wParam, lParam);
            }

            var keyAttributes = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            var e = new KeyboardHookBaseEventArgs
            {
                Key = KeyInterop.KeyFromVirtualKey(keyAttributes.VkCode)
            };

            if (wParam == (IntPtr)WPARAM.WM_KEYDOWN || wParam == (IntPtr)WPARAM.WM_SYSKEYDOWN)
            {
                KeyDown?.Invoke(this, e);
            }
            else if (wParam == (IntPtr)WPARAM.WM_KEYUP || wParam == (IntPtr)WPARAM.WM_SYSKEYUP)
            {
                KeyUp?.Invoke(this, e);
            }

            if (e.Handled)
            {
                // Do not pass Go.  Do not collect $200.
                return -1;
            }

            return CallNextHook(nCode, wParam, lParam);
        }
    }
}
