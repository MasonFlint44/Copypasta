using System;
using System.Runtime.InteropServices;

namespace PaperClip.Hooks.KeyboardHook
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967(v=vs.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT
    {
        public int VkCode { get; set; }
        public uint ScanCode { get; set; }
        public uint Flags { get; set; }
        public uint Time { get; set; }
        public UIntPtr ExtraInfo { get; set; }
    }
}