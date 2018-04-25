using System;
using Copypasta.Models.Interfaces;

namespace Copypasta.Domain.Interfaces
{
    public interface IClipboard
    {
        event EventHandler<PropertyUpdatedEventArgs> ClipboardUpdated;
        IClipboardItemModel ClipboardData { get; set; }
    }
}
