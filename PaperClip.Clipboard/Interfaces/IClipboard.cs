using System;
using System.Collections.Generic;
using System.IO;
using PaperClip.Collections;
using PaperClip.Collections.Interfaces;

namespace PaperClip.Clipboard.Interfaces
{
    public interface IClipboard
    {
        MemoryStream this[string format] { get; }

        IEnumerable<string> Formats { get; }

        event EventHandler<ClipboardUpdatedEventArgs> ClipboardUpdated;

        bool ContainsFormat(string format);
        OrderedDictionary<string, MemoryStream> GetClipboardData();
        MemoryStream GetClipboardData(string format);
        void SetClipboardData(IOrderedDictionary<string, MemoryStream> data);
    }
}