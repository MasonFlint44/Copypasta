using System;
using System.Windows.Input;
using Copypasta.Domain.Notifications;
using Copypasta.Models;

namespace Copypasta.Domain.Interfaces
{
    public interface IClipboardHistoryManager: IObservable<ClipboardHistoryNotification>
    {
        int RecordCount { get; }
        HistoryRecordModel AddHistoryRecord(Key key, ClipboardDataModel clipboardData);
    }
}