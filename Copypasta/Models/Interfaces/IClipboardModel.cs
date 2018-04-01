using System;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardModel
    {
        event EventHandler<IPropertyUpdatedEventArgs> ClipboardUpdated;
        IClipboardItemModel ClipboardData { get; set; }
    }
}
