using Copypasta.Models;

namespace Copypasta.Domain.Notifications
{
    public class ClipboardHistoryNotification
    {
        public HistoryRecordModel AddedRecord { get; }
        public bool WasRecordRemoved { get; }
        public HistoryRecordModel RemovedRecord { get; }

        public ClipboardHistoryNotification(HistoryRecordModel addedRecord, bool wasRecordRemoved, HistoryRecordModel removedRecord)
        {
            AddedRecord = addedRecord;
            WasRecordRemoved = wasRecordRemoved;
            RemovedRecord = removedRecord;
        }
    }
}
