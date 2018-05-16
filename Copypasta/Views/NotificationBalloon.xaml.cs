using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.Views
{
    /// <summary>
    /// Interaction logic for NotificationBalloon.xaml
    /// </summary>
    public partial class NotificationBalloon : UserControl
    {
        private INotificationBalloonViewModel _viewModel;
        private readonly DispatcherTimer _timer;

        private double AnimationDuration => .35;

        public event EventHandler ClosingAnimationCompleted; 

        public NotificationBalloon(INotificationBalloonViewModel viewModel)
        {
            DataContextChanged += OnDataContextChanged;
            DataContext = viewModel;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(_viewModel.Timeout)
            };
            _timer.Tick += (sender, args) => _viewModel.Close();
            _viewModel.Shown += (sender, args) => _timer.Start();

            Loaded += OnLoaded_ExecuteShowCommand;
            Loaded += OnLoaded_SlideLeftAnimation;
            _viewModel.Closing += OnClosingBalloon_SlideRightAnimation;

            InitializeComponent();
        }

        private void OnLoaded_ExecuteShowCommand(object obj, RoutedEventArgs routedEventArgs)
        {
            _viewModel.Show();
        }

        private void OnClosingBalloon_SlideRightAnimation(object sender, EventArgs eventArgs)
        {
            // Prevent animation from getting triggered multiple times if CloseCommand is executed more than once
            _viewModel.Closing -= OnClosingBalloon_SlideRightAnimation;
            _timer.Stop();

            var animation = new DoubleAnimation(Canvas.GetLeft(Border), Canvas.Width, new Duration(TimeSpan.FromSeconds(AnimationDuration)));
            animation.Completed += (o, args) =>
            {
                ClosingAnimationCompleted?.Invoke(this, EventArgs.Empty);
            };
            Border.BeginAnimation(Canvas.LeftProperty, animation);
        }

        private void OnLoaded_SlideLeftAnimation(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation(Canvas.Width, 0, new Duration(TimeSpan.FromSeconds(AnimationDuration)));
            animation.Completed += (o, args) =>
            {
                if (!_viewModel.ShownCommand.CanExecute(null)) { return; }
                _viewModel.ShownCommand.Execute(null);
            };
            Border.BeginAnimation(Canvas.LeftProperty, animation);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = e.NewValue as INotificationBalloonViewModel ?? throw new InvalidOperationException("DataContext must be of type: INotificationBalloonViewModel");
        }
    }
}
