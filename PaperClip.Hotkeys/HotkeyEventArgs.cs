using System;
using PaperClip.Hotkeys.Interfaces;

namespace PaperClip.Hotkeys
{
    public class HotkeyEventArgs: EventArgs, IHotkeyEventArgs
    {
        public bool Handled { get; set; }
    }
}
