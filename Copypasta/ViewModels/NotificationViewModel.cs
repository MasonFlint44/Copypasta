using System;
using Copypasta.StateMachine;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels
{
    public class NotificationViewModel : INotificationViewModel
    {
        public event EventHandler NotificationTimeoutEvent;
        public event EventHandler ShowCopyingNotificationEvent;
        public event EventHandler ShowPastingNotificationEvent;
        public event EventHandler HideNotificationEvent;

        public void ShowNotification(CopypastaState state)
        {
            switch (state)
            {
                case CopypastaState.Copying:
                    ShowCopyingNotificationEvent?.Invoke(this, EventArgs.Empty);
                    return;
                case CopypastaState.Pasting:
                    ShowPastingNotificationEvent?.Invoke(this, EventArgs.Empty);
                    return;
                default:
                    throw new NotImplementedException($"Cannot show notification in state: {state}");
            }
        }

        public void HideNotification()
        {
            HideNotificationEvent?.Invoke(this, EventArgs.Empty);
        }

        public void NotificationTimeout()
        {
            NotificationTimeoutEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
