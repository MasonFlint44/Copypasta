using System;
using PaperClip.Hooks.Interfaces;

namespace PaperClip.Hooks.MouseHook.Interfaces
{
    public interface IMouseHookEventArgs: IHookBaseEventArgs
    {
        Point Point { get; }
        int ScrollValue { get; }
        XButton XButton { get; }
        MouseHookFlags Flag { get; }
        TimeSpan Time { get; }
        MouseInputNotifications MouseMessage { get; }
        ulong ExtraInfo { get; }
    }
}
