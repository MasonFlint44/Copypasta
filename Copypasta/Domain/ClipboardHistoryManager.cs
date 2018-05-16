using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Copypasta.Domain.Interfaces;
using Copypasta.Domain.Notifications;
using Copypasta.Models;
using PaperClip.Reactive;

namespace Copypasta.Domain
{
    public class ClipboardHistoryManager: IClipboardHistoryManager
    {
        private readonly Subscription<ClipboardHistoryNotification> _subscription = new Subscription<ClipboardHistoryNotification>();
        private readonly List<ClipboardHistoryNotification> _notifications;
        private readonly List<HistoryRecordModel> _history;

        public int RecordCount { get; }

        public ClipboardHistoryManager(int recordCount)
        {
            RecordCount = recordCount;
            // TODO: wrap the replay observable logic in a class in PaperClip.Reactive or look into using Subjects
            _notifications = new List<ClipboardHistoryNotification>(RecordCount);
            _history = new List<HistoryRecordModel>(RecordCount);
        }

        public HistoryRecordModel AddHistoryRecord(Key key, ClipboardDataModel clipboardData)
        {
            bool wasItemRemoved;
            var removedRecord = default(HistoryRecordModel);
            if (wasItemRemoved = _history.Count == RecordCount)
            {
                removedRecord = _history.First();
                _history.Remove(removedRecord);
            }

            var addedRecord = new HistoryRecordModel(key, clipboardData);
            _history.Add(addedRecord);

            // Notify observers and log notification
            var notification = new ClipboardHistoryNotification(addedRecord, wasItemRemoved, removedRecord);
            _subscription.Broadcast(notification);

            if (_notifications.Count == RecordCount)
            {
                _notifications.Remove(_notifications.First());
            }
            _notifications.Add(notification);

            return addedRecord;
        }

        public IDisposable Subscribe(IObserver<ClipboardHistoryNotification> observer)
        {
            var unsubscriber = _subscription.Subscribe(observer);
            // Replay previous notifications to subscriber
            foreach (var notification in _notifications)
            {
                observer.OnNext(notification);
            }
            return unsubscriber;
        }
    }
}
