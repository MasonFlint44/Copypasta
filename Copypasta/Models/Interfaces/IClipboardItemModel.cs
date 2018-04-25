using System;
using System.IO;
using System.Windows.Input;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardItemModel
    {
        event EventHandler<IPropertyUpdatedEventArgs<Key>> KeyUpdated;
        Key Key { get; set; }

        event EventHandler<IPropertyUpdatedEventArgs<IOrderedDictionary<string, MemoryStream>>> ClipboardDataUpdated;
        IOrderedDictionary<string, MemoryStream> ClipboardData { get; set; }
    }
}
