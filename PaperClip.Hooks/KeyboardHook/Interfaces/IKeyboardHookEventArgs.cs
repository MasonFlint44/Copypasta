using System.Windows.Input;
using PaperClip.Hooks.Interfaces;

namespace PaperClip.Hooks.KeyboardHook.Interfaces
{
    public interface IKeyboardHookEventArgs : IHookBaseEventArgs
    {
        Key Key { get; }
    }
}
