namespace PaperClip.Hooks.KeyboardHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ff468861(v=vs.85).aspx
    public enum KeyboardInputNotifications
    {
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSDEADCHAR = 0x0107,
        WM_UNICHAR = 0x0109,
        WM_HOTKEY = 0x0312,
        WM_APPCOMMAND = 0x0319
    }
}