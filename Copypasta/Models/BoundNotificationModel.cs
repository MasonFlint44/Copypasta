using System;
using System.Windows.Input;
using Copypasta.Models.Interfaces;
using Copypasta.Models.Notifications;
using PaperClip.Reactive;

namespace Copypasta.Models
{
    public class BoundNotificationModel : IBoundNotificationModel
    {
        private readonly Subscription<BoundNotification> _subscription = new Subscription<BoundNotification>();

        private Key _key;
        public Key Key
        {
            get => _key;
            set
            {
                _key = value;
                foreach (var observer in _subscription.Subscribers)
                {
                    observer.OnNext(new BoundNotification(value, ClipboardData));
                }
            }
        }

        private ClipboardDataModel _clipboardData;
        public ClipboardDataModel ClipboardData
        {
            get => _clipboardData;
            set
            {
                _clipboardData = value;
                foreach (var observer in _subscription.Subscribers)
                {
                    observer.OnNext(new BoundNotification(Key, value));
                }
            }
        }

        public IDisposable Subscribe(IObserver<BoundNotification> observer)
        {
            var unsubscriber = _subscription.Subscribe(observer);
            // Send notification of current state
            observer.OnNext(new BoundNotification(Key, ClipboardData));
            return unsubscriber;
        }
    }
}
