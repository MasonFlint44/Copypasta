using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Copypasta.Domain.Interfaces;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.Views
{
    /// <summary>
    /// Interaction logic for NotifyIconWindow.xaml
    /// </summary>
    public partial class NotifyIconWindow : Window
    {
        private readonly INotificationDispatcher _notificationDispatcher;
        private readonly IHistoryMenuViewModel _historyMenuViewModel;

        public NotifyIconWindow(INotificationDispatcher notificationDispatcher, IHistoryMenuViewModel historyMenuViewModel)
        {
            _notificationDispatcher = notificationDispatcher;
            _historyMenuViewModel = historyMenuViewModel;

            InitializeComponent();
            HistoryMenu.DataContext = _historyMenuViewModel;

            _notificationDispatcher.Subscribe(notification =>
            {
                ShowNotification(notification.Notification);
            });
        }

        private void ShowNotification(INotificationBalloonViewModel notification)
        {
            if (notification == null) { return; }

            var notificationBalloon = new NotificationBalloon(notification);
            notificationBalloon.ClosingAnimationCompleted += (o, eventArgs) =>
            {
                NotifyIcon.CloseBalloon();

                if(!notification.ClosedCommand.CanExecute(null)) { return; }
                notification.ClosedCommand.Execute(null);
            };
            // NotificationBalloon handles animation and timeout itself
            NotifyIcon.ShowCustomBalloon(notificationBalloon, PopupAnimation.None, null);
            // Place popup against right edge of screen
            NotifyIcon.CustomBalloon.HorizontalOffset += 1;
        }
    }
}
