using System;

namespace PaperClip.Hooks.MouseHook.Interfaces
{
    public interface IMouseHook
    {
        event EventHandler<IMouseHookEventArgs> MouseEvent;
    }
}
