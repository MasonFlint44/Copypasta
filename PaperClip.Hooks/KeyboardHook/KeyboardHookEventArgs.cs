using System.Windows.Input;
using PaperClip.Hooks.KeyboardHook.Interfaces;

namespace PaperClip.Hooks.KeyboardHook
{
    public class KeyboardHookBaseEventArgs : IKeyboardHookEventArgs
    {
        public Key Key { get; protected internal set; }
        public bool Handled { get; set; }
    }
}
