using System;
using Copypasta.StateMachine;

namespace Copypasta.ViewModels.Interfaces
{
    public interface INotificationViewModel
    {
        event EventHandler NotificationTimeoutEvent;
        event EventHandler ShowCopyingNotificationEvent;
        event EventHandler ShowPastingNotificationEvent;
        event EventHandler HideNotificationEvent;
        void ShowNotification(CopypastaState state);
        void HideNotification();
        void NotificationTimeout();
    }
}
