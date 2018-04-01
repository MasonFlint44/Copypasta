using System;

namespace PaperClip.Hooks.KeyboardHook.Interfaces
{
    public interface IKeyboardHook
    {
        event EventHandler<IKeyboardHookEventArgs> KeyUp;
        event EventHandler<IKeyboardHookEventArgs> KeyDown;
    }
}
