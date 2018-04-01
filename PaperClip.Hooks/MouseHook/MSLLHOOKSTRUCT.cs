using System;
using System.Runtime.InteropServices;

namespace PaperClip.Hooks.MouseHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644970(v=vs.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    internal struct MSLLHOOKSTRUCT
    {
        public POINT Point { get; set; }
        public MOUSEDATA MouseData { get; set; }
        public uint Flags { get; set; }
        public uint Time { get; set; }
        public UIntPtr ExtraInfo { get; set; }
    }
}
