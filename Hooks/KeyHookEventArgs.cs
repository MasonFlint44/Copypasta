using System.Windows.Input;

namespace Hooks
{
    public class KeyHookEventArgs : HookEventArgs
    {
        public Key KeyCode { get; set; }
    }
}
