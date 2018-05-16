using System;
using Copypasta.Domain.Interfaces;
using Copypasta.Domain.Notifications;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels;
using Copypasta.ViewModels.Interfaces;
using PaperClip.Reactive;

namespace Copypasta.Domain
{
    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly Subscription<NotificationDispatcherNotification> _subscription = new Subscription<NotificationDispatcherNotification>();

        private INotificationBalloonViewModel _currentNotification;
        public INotificationBalloonViewModel CurrentNotification
        {
            get => _currentNotification;
            private set
            {
                if (_currentNotification != null)
                {
                    _currentNotification.Closed -= OnCurrentNotificationClosed;
                }
                _currentNotification = value;
                if (_currentNotification != null)
                {
                    _currentNotification.Closed += OnCurrentNotificationClosed;
                }
                _subscription.Broadcast(new NotificationDispatcherNotification(_currentNotification));
            }
        }

        private void OnCurrentNotificationClosed(object sender, EventArgs e)
        {
            CurrentNotification = null;
        }

        public void ShowNotification(IBoundNotificationModel notification)
        {
            ShowNotification(new NotificationBalloonViewModel(notification));
        }

        public void ShowNotification(ISimpleNotificationModel notification)
        {
            ShowNotification(new NotificationBalloonViewModel(notification));
        }

        private void ShowNotification(INotificationBalloonViewModel notification)
        {
            if (CurrentNotification == null)
            {
                CurrentNotification = notification;
                return;
            }

            // Prevent CurrentNotification from getting set to null when it is getting replaced
            CurrentNotification.Closed -= OnCurrentNotificationClosed;
            CurrentNotification.Closed += (sender, args) =>
            {
                CurrentNotification = notification;
            };
            CloseNotification();
        }

        public void CloseNotification()
        {
            CurrentNotification?.Close();
        }

        public IDisposable Subscribe(IObserver<NotificationDispatcherNotification> observer)
        {
            var unsubscriber = _subscription.Subscribe(observer);
            // Replay current notification to subscriber
            observer.OnNext(new NotificationDispatcherNotification(CurrentNotification));
            return unsubscriber;
        }
    }
}
