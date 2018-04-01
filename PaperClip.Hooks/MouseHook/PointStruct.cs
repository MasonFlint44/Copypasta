using System.Runtime.InteropServices;

namespace PaperClip.Hooks.MouseHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162805(v=vs.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
