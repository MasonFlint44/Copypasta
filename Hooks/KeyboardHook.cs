using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Hooks
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT
    {
        public int VkCode { get; set; }
        public uint ScanCode { get; set; }
        public uint Flags { get; set; }
        public uint Time { get; set; }
        public IntPtr DwExtraInfo { get; set; }
    }

    public class KeyboardHook : Hook
    {
        public delegate void KeyHookEventHandler(object sender, KeyHookEventArgs e);

        public event KeyHookEventHandler KeyUp;
        public event KeyHookEventHandler KeyDown;

        public KeyboardHook() : base(HookType.WH_KEYBOARD_LL) { }

        protected override int HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHook(nCode, wParam, lParam);
            }

            var keyAttributes = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            KeyHookEventArgs e = new KeyHookEventArgs
            {
                KeyCode = KeyInterop.KeyFromVirtualKey(keyAttributes.VkCode)
            };

            if (keyAttributes.Flags == 0 || keyAttributes.Flags == 1)
            {
                KeyDown?.Invoke(this, e);
            }
            else if (keyAttributes.Flags == 128 || keyAttributes.Flags == 129)
            {
                KeyUp?.Invoke(this, e);
            }

            if (e.Handled)
            {
                // Do not pass Go.  Do not collect $200.
                return -1;
            }
            else
            {
                return CallNextHook(nCode, wParam, lParam);
            }
        }
    }
}
