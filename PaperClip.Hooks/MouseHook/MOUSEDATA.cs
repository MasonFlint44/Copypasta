using System.Runtime.InteropServices;

namespace PaperClip.Hooks.MouseHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644970(v=vs.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEDATA
    {
        public short Low { get; set; }
        public short High { get; set; }
    }
}
