using System;
using Copypasta.Domain.Notifications;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.Domain.Interfaces
{
    public interface INotificationDispatcher: IObservable<NotificationDispatcherNotification>
    {
        INotificationBalloonViewModel CurrentNotification { get; }

        void CloseNotification();
        void ShowNotification(IBoundNotificationModel notification);
        void ShowNotification(ISimpleNotificationModel notification);
    }
}