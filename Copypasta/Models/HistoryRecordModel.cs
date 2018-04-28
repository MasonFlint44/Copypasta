using System;
using System.Windows.Input;
using Copypasta.Models.Notifications;
using PaperClip.Reactive;

namespace Copypasta.Models
{
    public class HistoryRecordModel: IObservable<HistoryRecordNotification>
    {
        private readonly Subscription<HistoryRecordNotification> _subscription = new Subscription<HistoryRecordNotification>();

        private Key _key;
        public Key Key
        {
            get => _key;
            set
            {
                _key = value;
                foreach (var observer in _subscription.Subscribers)
                {
                    observer.OnNext(new HistoryRecordNotification(value, ClipboardData));
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
                    observer.OnNext(new HistoryRecordNotification(Key, value));
                }
            }
        }

        public HistoryRecordModel(Key key, ClipboardDataModel clipboardData)
        {
            Key = key;
            ClipboardData = clipboardData;
        }

        public IDisposable Subscribe(IObserver<HistoryRecordNotification> observer)
        {
            var unsubscriber = _subscription.Subscribe(observer);
            // Send notification of current state
            observer.OnNext(new HistoryRecordNotification(Key, ClipboardData));
            return unsubscriber;
        }
    }
}
