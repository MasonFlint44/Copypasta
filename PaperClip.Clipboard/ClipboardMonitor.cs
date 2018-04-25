using System;
using System.Windows;
using System.Windows.Interop;
using PaperClip.Clipboard.Interfaces;

namespace PaperClip.Clipboard
{
    public class ClipboardMonitor: IClipboardMonitor
    {
        public event EventHandler<ClipboardUpdatedEventArgs> ClipboardUpdated;

        private readonly Window _window;
        private readonly IntPtr _handle;

        public ClipboardMonitor(Window window = null)
        {
            _window = window ?? new Window();
            _handle = new WindowInteropHelper(_window).EnsureHandle();

            if(window == null)
            {
                // Create new message-only window to monitor clipboard
                NativeMethods.SetParent(_handle, NativeMethods.HWND_MESSAGE);
            }

            NativeMethods.AddClipboardFormatListener(_handle);

            var source = HwndSource.FromHwnd(_handle);
            source?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == NativeMethods.WM_CLIPBOARDUPDATE)
            {
                ClipboardUpdated?.Invoke(_window, new ClipboardUpdatedEventArgs());
            }

            return IntPtr.Zero;
        }

        ~ClipboardMonitor()
        {
            NativeMethods.RemoveClipboardFormatListener(_handle);
        }
    }
}
