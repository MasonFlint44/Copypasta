using System;
using System.Windows.Input;
using Copypasta.Models;
using Copypasta.Models.Interfaces;
using IClipboard = Copypasta.Domain.Interfaces.IClipboard;

namespace Copypasta.Domain
{
    public class Clipboard: IClipboard
    {
        private readonly PaperClip.Clipboard.Interfaces.IClipboard _clipboard;

        public Clipboard(PaperClip.Clipboard.Interfaces.IClipboard clipboard)
        {
            _clipboard = clipboard;
            _clipboard.ClipboardUpdated += (sender, args) => ClipboardUpdated?.Invoke(this, new PropertyUpdatedEventArgs());
        }

        public event EventHandler<PropertyUpdatedEventArgs> ClipboardUpdated; 

        public IClipboardItemModel ClipboardData
        {
            get => new ClipboardItemModel(Key.None, _clipboard.GetClipboardData());
            set => _clipboard.SetClipboardData(value.ClipboardData);
        }
    }
}
