using Copypasta.Domain.Notifications;
using Copypasta.Models;
using IClipboardDataAccess = Copypasta.Domain.Interfaces.IClipboard;
using PaperClip.Reactive;

namespace Copypasta.Domain
{
    public class Clipboard: Subscription<ClipboardNotification>, IClipboardDataAccess
    {
        private readonly PaperClip.Clipboard.Interfaces.IClipboard _clipboard;

        public Clipboard(PaperClip.Clipboard.Interfaces.IClipboard clipboard)
        {
            _clipboard = clipboard;
            _clipboard.ClipboardUpdated += (sender, args) =>
            {
                foreach (var observer in Subscribers)
                {
                    observer.OnNext(new ClipboardNotification());
                }
            };
        }

        public ClipboardDataModel ClipboardData
        {
            get => new ClipboardDataModel(_clipboard.GetClipboardData());
            set => _clipboard.SetClipboardData(value.ClipboardData);
        }
    }
}
