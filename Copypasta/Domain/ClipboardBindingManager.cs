using System.Collections.Generic;
using System.Windows.Input;
using Copypasta.Domain.Interfaces;
using Copypasta.Domain.Notifications;
using Copypasta.Models;
using PaperClip.Reactive;

namespace Copypasta.Domain
{
    public class ClipboardBindingManager: Subscription<ClipboardBindingNotification>, IClipboardBindingManager
    {
        private readonly IDictionary<Key, ClipboardDataModel> _clipboardBindings = new Dictionary<Key, ClipboardDataModel>();

        public void AddBinding(Key key, ClipboardDataModel clipboardData)
        {
            _clipboardBindings[key] = clipboardData;

            Broadcast(new ClipboardBindingNotification(key, clipboardData));
        }

        public ClipboardDataModel GetBindingData(Key key)
        {
            if(!_clipboardBindings.TryGetValue(key, out var clipboardItem)) { return null; }
            return clipboardItem;
        }
    }
}
