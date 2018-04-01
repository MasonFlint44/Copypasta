using System;
using PaperClip.Hooks.MouseHook.Interfaces;

namespace PaperClip.Hooks.MouseHook
{
    public class MouseHookBaseEventArgs : IMouseHookEventArgs
    {
        public Point Point { get; protected  internal set; }
        public int ScrollValue { get; protected internal set; }
        public XButton XButton { get; protected internal set; }
        public MouseHookFlags Flag { get; protected internal set; }
        public TimeSpan Time { get; protected internal set; }
        public MouseInputNotifications MouseMessage { get; protected internal set; }
        public ulong ExtraInfo { get; protected internal set; }
        public bool Handled { get; set; }
    }
}
