using System;
using System.Runtime.InteropServices;

namespace PaperClip.Clipboard
{
    internal static class NativeMethods
    {
        // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public const int ERROR_SUCCESS = 0x0;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);

        // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        // See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
        // See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms649038(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint EnumClipboardFormats(uint format);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms649048(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms649035(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseClipboard();

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms649051(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms649039(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetClipboardData(uint uFormat);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms649039(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EmptyClipboard();

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/aa366584(v=vs.85).aspx
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/aa366593(v=vs.85).aspx
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GlobalSize(IntPtr hMem);

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/aa366595(v=vs.85).aspx
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalUnlock(IntPtr hMem);
    }
}
