using System;

namespace Hooks
{
    public class HookEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}
