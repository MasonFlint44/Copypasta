﻿using System;
using System.Windows;
using System.Windows.Interop;

namespace Copypasta.Notifications
{
    public class ClipboardNotification
    {
        public delegate void ClipboardUpdatedEventHandler(object sender, ClipboardUpdatedEventArgs e);

        public event ClipboardUpdatedEventHandler ClipboardUpdated;

        private Window _window;
        private IntPtr _handle;

        public ClipboardNotification(Window window)
        {
            _window = window;
            _handle = new WindowInteropHelper(window).EnsureHandle();

            NativeMethods.AddClipboardFormatListener(_handle);

            HwndSource source = HwndSource.FromHwnd(_handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == NativeMethods.WM_CLIPBOARDUPDATE)
            {
                ClipboardUpdated?.Invoke(_window, new ClipboardUpdatedEventArgs());
            }

            return IntPtr.Zero;
        }

        ~ClipboardNotification()
        {
            NativeMethods.RemoveClipboardFormatListener(_handle);
        }
    }
}