using System;

namespace PaperClip.Hotkeys.Interfaces
{
    public interface IHotkey
    {
        event EventHandler<IHotkeyEventArgs> HotkeyPressed;
        bool Handled { get; set; }
        bool IsPressed { get; }
    }
}
