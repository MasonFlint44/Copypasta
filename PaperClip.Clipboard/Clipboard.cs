using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using PaperClip.Clipboard.Helpers;
using PaperClip.Clipboard.Interfaces;
using PaperClip.Collections;
using PaperClip.Collections.Interfaces;

namespace PaperClip.Clipboard
{
    // TODO: Clipboard does not support fetching data from sythesized clipboard formats after getting all available formats
    public class Clipboard : IClipboard
    {
        private readonly Window _window;
        private readonly IntPtr _windowHandle;
        private readonly IClipboardMonitor _clipboardMonitor;
        private readonly ClipboardHelper _clipboardHelper;
        private readonly ClipboardFormatsEnumerable _clipboardFormatsEnumerable;

        private IDictionary<string, MemoryStream> _clipboardData = new Dictionary<string, MemoryStream>();
        private OrderedDictionary<string, int> _formats;
        private bool _formatsCurrent;

        public event EventHandler<IClipboardUpdatedEventArgs> ClipboardUpdated;

        // Ordered by priority - Most descriptive format first
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IEnumerable<string> Formats
        {
            get
            {
                if (!_formatsCurrent) { GetClipboardFormats(); }
                return _formats.Keys;
            }
        }

        public Clipboard(Window window = null)
        {
            _window = window ?? new Window();
            _windowHandle = new WindowInteropHelper(_window).EnsureHandle();

            if (window == null)
            {
                // Create message-only window
                NativeMethods.SetParent(_windowHandle, NativeMethods.HWND_MESSAGE);
            }

            _clipboardMonitor = new ClipboardMonitor(_window);
            _clipboardMonitor.ClipboardUpdated += (sender, args) =>
            {
                _formatsCurrent = false;
                _clipboardData.Clear();
                ClipboardUpdated?.Invoke(this, args);
            };

            _clipboardHelper = new ClipboardHelper(_windowHandle);
            _clipboardFormatsEnumerable = new ClipboardFormatsEnumerable(_clipboardHelper);
        }

        public MemoryStream this[string format] => GetClipboardData(format);

        // TODO: Need to add support for delayed rendering in case null is passed in
        public void SetClipboardData(IOrderedDictionary<string, MemoryStream> data)
        {
            try
            {
                _clipboardHelper.OpenClipboard();
                _clipboardHelper.EmptyClipboard();

                var formats = new OrderedDictionary<string, int>();
                var clipboardData = new Dictionary<string, MemoryStream>();
                foreach (var format in data)
                {
                    var dataFormat = DataFormats.GetDataFormat(format.Key);
                    try
                    {
                        _clipboardHelper.SetClipboardData(dataFormat.Id, format.Value);

                        // Store format names as lowercase
                        formats[format.Key.ToLower()] = dataFormat.Id;
                        clipboardData[format.Key.ToLower()] = format.Value;
                    }
                    catch
                    {
                        // Do nothing
                    }
                }

                _formats = formats;
                _formatsCurrent = true;
                _clipboardData = clipboardData;
            }
            finally
            {
                _clipboardHelper.CloseClipboard();
            }
        }

        //public void SetClipboardData(OrderedDictionary<string, object> data)
        //{
        //    var formatter = new BinaryFormatter();
        //    var clipboardData = new OrderedDictionary<string, MemoryStream>();
        //    foreach (var format in data)
        //    {
        //        if (format.Value == null)
        //        {
        //            clipboardData[format.Key] = null;
        //            break;
        //        }
                
        //        if (format.Value is MemoryStream stream)
        //        {
        //            clipboardData[format.Key] = stream;
        //            break;
        //        }

        //        // Serialize data
        //        stream = new MemoryStream();
        //        formatter.Serialize(stream, format.Value);

        //        clipboardData[format.Key] = stream;
        //    }
        //    SetClipboardData(clipboardData);
        //}

        public bool ContainsFormat(string format)
        {
            if (!_formatsCurrent) { GetClipboardFormats(); }
            return _formats.ContainsKey(format.ToLower());
        }

        private void GetClipboardFormats()
        {
            var formats = new OrderedDictionary<string, int>();
            foreach (var format in _clipboardFormatsEnumerable)
            {
                formats[format.Name.ToLower()] = format.Id;
            }

            // Get synthesized formats
            var synthesizedFormats = formats.Keys.SelectMany(formatName => _clipboardHelper.GetSynthesizedFormats(formatName));
            foreach (var synthesizedFormat in synthesizedFormats)
            {
                var lowerSynthFormatName = synthesizedFormat.Name.ToLower();
                if (!formats.ContainsKey(lowerSynthFormatName))
                {
                    formats[lowerSynthFormatName] = synthesizedFormat.Id;
                }
            }

            _formats = formats;
            _formatsCurrent = true;
        }

        public MemoryStream GetClipboardData(string format)
        {
            if (!_formatsCurrent) { GetClipboardFormats(); }
            var lowerFormat = format.ToLower();
            if (!_formats.ContainsKey(lowerFormat))
            {
                throw new FormatException($"Clipboard does not contain {format} data.");
            }
            if (!_clipboardData.TryGetValue(lowerFormat, out var data))
            {
                try
                {
                    _clipboardHelper.OpenClipboard();
                    var formatId = _formats[lowerFormat];
                    data = _clipboardHelper.GetClipboardData(formatId);
                    _clipboardData[lowerFormat] = data;
                }
                finally
                {
                    _clipboardHelper.CloseClipboard();
                }
            }
            return data;
        }

        public OrderedDictionary<string, MemoryStream> GetClipboardData()
        {
            if (!_formatsCurrent) { GetClipboardFormats(); }
            var clipboardData = new OrderedDictionary<string, MemoryStream>();
            try
            {
                _clipboardHelper.OpenClipboard();

                foreach (var format in _formats)
                {
                    if (!_clipboardData.TryGetValue(format.Key.ToLower(), out var data))
                    {
                        try
                        {
                            data = _clipboardHelper.GetClipboardData(format.Value);
                        }
                        catch
                        {
                            break;
                        }
                    }
                    clipboardData[format.Key.ToLower()] = data;
                }
            }
            finally
            {
                _clipboardHelper.CloseClipboard();
            }
            
            _clipboardData = clipboardData;

            return clipboardData;
        }
    }
}
