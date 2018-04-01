using System;
using System.Windows.Input;
using Copypasta.DataAccess.Interfaces;
using Copypasta.Models.Interfaces;
using PaperClip.Clipboard.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardModel: IClipboardModel
    {
        private readonly IClipboardDataAccess _clipboardDataAccess;
        private readonly IClipboardMonitor _clipboardMonitor;

        public ClipboardModel(IClipboardDataAccess clipboardDataAccess, IClipboardMonitor clipboardMonitor)
        {
            _clipboardDataAccess = clipboardDataAccess;
            _clipboardDataAccess.PersistClipboardData = true;
            _clipboardMonitor = clipboardMonitor;
            _clipboardMonitor.ClipboardUpdated += (sender, args) => ClipboardUpdated?.Invoke(this, new PropertyUpdatedEventArgs());
        }

        public event EventHandler<IPropertyUpdatedEventArgs> ClipboardUpdated; 

        public IClipboardItemModel ClipboardData
        {
            get => new ClipboardItemModel(Key.None, _clipboardDataAccess.ClipboardData);
            set => _clipboardDataAccess.ClipboardData = value.ClipboardData;
        }
    }
}
