using System;
using System.Runtime.InteropServices;
using PaperClip.Hooks.MouseHook.Interfaces;

namespace PaperClip.Hooks.MouseHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644986(v=vs.85).aspx
    public class MouseHook : HookBase, IMouseHook
    {
        private const int WheelDelta = 120;

        public event EventHandler<IMouseHookEventArgs> MouseEvent;

        public MouseHook() : base(HookType.WH_MOUSE_LL) { }

        protected override int HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHook(nCode, wParam, lParam);
            }

            var mouseAttributes = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

            var e = new MouseHookBaseEventArgs
            {
                Point = new Point
                {
                    X = mouseAttributes.Point.X,
                    Y = mouseAttributes.Point.Y
                },
                ScrollValue = (Enum.IsDefined(typeof(XButton), (int)mouseAttributes.MouseData.High) == false) ?
                    (mouseAttributes.MouseData.High / WheelDelta) : 0,
                XButton = Enum.IsDefined(typeof(XButton), (int)mouseAttributes.MouseData.High) ?
                    (XButton)mouseAttributes.MouseData.High :
                    XButton.NONE,
                Flag = (MouseHookFlags)mouseAttributes.Flags,
                // How to interpret time: 
                // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644939(v=vs.85).aspx
                Time = TimeSpan.FromMilliseconds(mouseAttributes.Time),
                MouseMessage = (MouseInputNotifications)wParam.ToInt32(),
                ExtraInfo = mouseAttributes.ExtraInfo.ToUInt64()
            };

            MouseEvent?.Invoke(this, e);

            if (e.Handled)
            {
                // Do not pass Go.  Do not collect $200.
                return -1;
            }

            return CallNextHook(nCode, wParam, lParam);
        }
    }
}
