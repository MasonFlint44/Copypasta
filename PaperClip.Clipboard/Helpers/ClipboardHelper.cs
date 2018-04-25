using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace PaperClip.Clipboard.Helpers
{
    internal class ClipboardHelper
    {
        private readonly IntPtr _windowHandle;

        // Synthesized clipboard formats: https://msdn.microsoft.com/en-us/library/windows/desktop/ms649013(v=vs.85).aspx#_win32_Synthesized_Clipboard_Formats
        private readonly Dictionary<string, List<string>> _sythesizedFormats = new Dictionary<string, List<string>>
        {
            { DataFormats.Bitmap.ToLower(), new List<string>{ DataFormats.Dib.ToLower(), DataFormats.GetDataFormat(17).Name.ToLower() } },
            { DataFormats.Dib.ToLower(), new List<string>{ DataFormats.Bitmap.ToLower(), DataFormats.Palette.ToLower(), DataFormats.GetDataFormat(17).Name.ToLower() } },
            { DataFormats.GetDataFormat(17).Name.ToLower(), new List<string>{ DataFormats.Bitmap.ToLower(), DataFormats.Palette.ToLower(), DataFormats.Dib.ToLower() } },
            { DataFormats.EnhancedMetafile.ToLower(), new List<string>{ DataFormats.MetafilePicture.ToLower() } },
            { DataFormats.MetafilePicture.ToLower(), new List<string>{ DataFormats.EnhancedMetafile.ToLower() } },
            { DataFormats.OemText.ToLower(), new List<string>{ DataFormats.Text.ToLower(), DataFormats.UnicodeText.ToLower() } },
            { DataFormats.Text.ToLower(), new List<string>{ DataFormats.OemText.ToLower(), DataFormats.UnicodeText.ToLower() } },
            { DataFormats.UnicodeText.ToLower(), new List<string>{ DataFormats.Text.ToLower(), DataFormats.OemText.ToLower() } }
        };

        internal ClipboardHelper(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public List<DataFormat> GetSynthesizedFormats(string format)
        {
            var lowerFormat = format.ToLower();
            if (!_sythesizedFormats.TryGetValue(lowerFormat, out var availableFormats))
            {
                return new List<DataFormat>();
            }
            return availableFormats.Select(x => DataFormats.GetDataFormat(x)).ToList();
        }

        public bool IsSynthesizedFormatAvailable(string format)
        {
            var lowerFormat = format.ToLower();
            if (!_sythesizedFormats.TryGetValue(lowerFormat, out var availableFormats))
            {
                return false;
            }
            if (!availableFormats.Contains(lowerFormat))
            {
                return false;
            }
            return true;
        }

        public MemoryStream GetClipboardData(int format)
        {
            var dataHandle = NativeMethods.GetClipboardData((uint)format);
            if (dataHandle == IntPtr.Zero)
            {
                GetLastError();

                // If no error, return null as there is no data stored for this format
                return null;
            }

            try
            {
                var dataPtr = GlobalLock(dataHandle);

                var size = GlobalSize(dataHandle);
                var buffer = new byte[size];
                Marshal.Copy(dataPtr, buffer, 0, size);

                return new MemoryStream(buffer);
            }
            finally
            {
                GlobalUnlock(dataHandle);
            }
        }

        public void SetClipboardData(int format, MemoryStream data)
        {
            var unmanagedDataPtr = IntPtr.Zero;
            if (data != null)
            {
                // Allocate unmanaged memory
                var size = (int)data.Length;
                unmanagedDataPtr = Marshal.AllocHGlobal(size);

                // Copy data to unmanaged memory
                Marshal.Copy(data.ToArray(), 0, unmanagedDataPtr, size);
            }

            var dataPtr = NativeMethods.SetClipboardData((uint)format, unmanagedDataPtr);

            if (dataPtr == IntPtr.Zero)
            {
                GetLastError();
            }
        }

        public int EnumClipboardFormats(int currentFormat)
        {
            uint nextFormat;
            if ((nextFormat = NativeMethods.EnumClipboardFormats((uint)currentFormat)) == 0)
            {
                GetLastError();
            }
            return (int)nextFormat;
        }

        public void EmptyClipboard()
        {
            if (!NativeMethods.EmptyClipboard())
            {
                GetLastError();
            }
        }

        public void CloseClipboard()
        {
            if (!NativeMethods.CloseClipboard())
            {
                GetLastError();
            }
        }

        public void OpenClipboard()
        {
            if (!NativeMethods.OpenClipboard(_windowHandle))
            {
                GetLastError();
            }
        }

        private static bool GlobalUnlock(IntPtr handle)
        {
            var result = NativeMethods.GlobalUnlock(handle);
            GetLastError();
            return result;
        }

        private static int GlobalSize(IntPtr handle)
        {
            var size = NativeMethods.GlobalSize(handle);
            if (size == 0)
            {
                GetLastError();
            }
            return size;
        }

        private static IntPtr GlobalLock(IntPtr handle)
        {
            var ptr = NativeMethods.GlobalLock(handle);
            if (ptr == IntPtr.Zero)
            {
                GetLastError();
            }
            return ptr;
        }

        private static int GetLastError()
        {
            int error;
            if ((error = Marshal.GetLastWin32Error()) != NativeMethods.ERROR_SUCCESS)
            {
                throw new Win32Exception(error);
            }
            return error;
        }
    }
}
