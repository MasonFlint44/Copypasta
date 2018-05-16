using System;
using System.Windows.Input;
using Copypasta.Models.Notifications;

namespace Copypasta.Models.Interfaces
{
    public interface IHistoryRecordModel: IObservable<HistoryRecordNotification>
    {
        ClipboardDataModel ClipboardData { get; set; }
        Key Key { get; set; }
    }
}