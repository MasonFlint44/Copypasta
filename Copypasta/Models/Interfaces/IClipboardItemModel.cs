using System;
using System.Windows;
using System.Windows.Input;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardItemModel
    {
        event EventHandler<IPropertyUpdatedEventArgs<Key>> KeyUpdated;
        Key Key { get; set; }

        event EventHandler<IPropertyUpdatedEventArgs<IDataObject>> ClipboardDataUpdated;
        IDataObject ClipboardData { get; set; }
    }
}
