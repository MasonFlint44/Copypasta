namespace PaperClip.Hooks.KeyboardHook
{
    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms644985(v=vs.85).aspx
    internal enum WPARAM
    {
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105
    }
}
