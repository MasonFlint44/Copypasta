using System;
using System.Windows.Input;
using Copypasta.Models.Interfaces;
using PaperClip.Clipboard.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardModel: IClipboardModel
    {
        private readonly IClipboard _clipboard;

        public ClipboardModel(IClipboard clipboard)
        {
            _clipboard = clipboard;
            _clipboard.ClipboardUpdated += (sender, args) => ClipboardUpdated?.Invoke(this, new PropertyUpdatedEventArgs());
        }

        public event EventHandler<IPropertyUpdatedEventArgs> ClipboardUpdated; 

        public IClipboardItemModel ClipboardData
        {
            get => new ClipboardItemModel(Key.None, _clipboard.GetClipboardData());
            set => _clipboard.SetClipboardData(value.ClipboardData);
        }
    }
}
