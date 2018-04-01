using System;
using System.Windows;
using Copypasta.ViewModels.Interfaces;
using Hardcodet.Wpf.TaskbarNotification;

namespace Copypasta.Views
{
    /// <summary>
    /// Interaction logic for NotifyIconWindow.xaml
    /// </summary>
    public partial class NotifyIconWindow : Window
    {
        private INotificationViewModel _notificationViewModel;
        
        public NotifyIconWindow(INotificationViewModel notificationViewModel, IHistoryMenuViewModel historyMenuViewModel)
        {
            DataContextChanged += OnDataContextChanged;
            DataContext = notificationViewModel;

            InitializeComponent();
            ConfigureEventHandlers();
            HistoryMenu.DataContext = historyMenuViewModel;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _notificationViewModel = e.NewValue as INotificationViewModel ?? throw new InvalidOperationException("DataContext must be of type: INotificationViewModel");
        }

        private void ConfigureEventHandlers()
        {
            _notificationViewModel.ShowCopyingNotificationEvent += (_sender, args) =>
            {
                NotifyIcon.ShowBalloonTip("Copying...", "Press a key to save clipboard", BalloonIcon.None);
            };
            _notificationViewModel.ShowPastingNotificationEvent += (_sender, args) =>
            {
                NotifyIcon.ShowBalloonTip("Pasting...", "Press a key to paste data", BalloonIcon.None);
            };
            _notificationViewModel.HideNotificationEvent += (_sender, args) =>
            {
                NotifyIcon.HideBalloonTip();
            };
            NotifyIcon.TrayBalloonTipClosed += (o, args) =>
            {
                _notificationViewModel.NotificationTimeout();
            };
        }
    }
}
