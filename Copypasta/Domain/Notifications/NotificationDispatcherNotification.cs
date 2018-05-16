using Copypasta.ViewModels.Interfaces;

namespace Copypasta.Domain.Notifications
{
    public class NotificationDispatcherNotification
    {
        public INotificationBalloonViewModel Notification { get; }

        public NotificationDispatcherNotification(INotificationBalloonViewModel notification)
        {
            Notification = notification;
        }
    }
}
