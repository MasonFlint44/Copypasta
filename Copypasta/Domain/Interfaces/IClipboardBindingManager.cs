using System;
using System.Windows.Input;
using Copypasta.Domain.Notifications;
using Copypasta.Models;

namespace Copypasta.Domain.Interfaces
{
    public interface IClipboardBindingManager: IObservable<ClipboardBindingNotification>
    {
        void AddBinding(Key key, ClipboardDataModel clipboardItem);
        ClipboardDataModel GetBindingData(Key key);
    }
}