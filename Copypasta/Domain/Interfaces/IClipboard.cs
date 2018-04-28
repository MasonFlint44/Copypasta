using System;
using Copypasta.Domain.Notifications;
using Copypasta.Models;

namespace Copypasta.Domain.Interfaces
{
    public interface IClipboard: IObservable<ClipboardNotification>
    {
        ClipboardDataModel ClipboardData { get; set; }
    }
}
