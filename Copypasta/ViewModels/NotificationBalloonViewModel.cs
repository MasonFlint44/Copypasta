using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Copypasta.Annotations;
using Copypasta.ViewModels.Interfaces;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels.Helpers;

namespace Copypasta.ViewModels
{
    public class NotificationBalloonViewModel : INotificationBalloonViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Showing; 
        public event EventHandler Shown;
        public event EventHandler Closing;
        public event EventHandler Closed;

        public ICommand ShowCommand => new RelayCommand(x => Showing?.Invoke(this, EventArgs.Empty));
        public ICommand ShownCommand => new RelayCommand(x => Shown?.Invoke(this, EventArgs.Empty));
        public ICommand CloseCommand => new RelayCommand(x => Closing?.Invoke(this, EventArgs.Empty));
        public ICommand ClosedCommand => new RelayCommand(x => Closed?.Invoke(this, EventArgs.Empty));

        public int Timeout => 3500;

        private Image _image;
        public Image Image
        {
            get => _image;
            private set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            private set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _contentText;
        public string ContentText
        {
            get => _contentText;
            private set
            {
                _contentText = value;
                OnPropertyChanged(nameof(ContentText));
            }
        }

        private NotificationBalloonViewModel()
        {
            _image = Properties.Resources.NotifyIcon.ToBitmap();
        }

        public NotificationBalloonViewModel(IBoundNotificationModel model): this()
        {
            model.Subscribe(notification =>
            {
                Title = GetText(notification.Key);
                ContentText = GetText(notification.ClipboardData);
            });
        }

        public NotificationBalloonViewModel(ISimpleNotificationModel model) : this()
        {
            Title = model.Title;
            ContentText = model.Content;
        }

        // Execute ShowCommand
        public void Show()
        {
            if (!ShowCommand.CanExecute(null)) { return; }
            ShowCommand.Execute(null);
        }

        // Execute CloseCommand
        public void Close()
        {
            if (!CloseCommand.CanExecute(null)) { return; }
            CloseCommand.Execute(null);
        }

        private static string GetText(Key key)
        {
            if (key == Key.None) { return "Press a key..."; }
            return $"Key: {key}";
        }

        private static string GetText(IClipboardDataModel clipboardData)
        {
            if(clipboardData?.ClipboardData == null) { return string.Empty; }
            if (!clipboardData.ClipboardData.TryGetValue(DataFormats.UnicodeText.ToLower(), out var stream))
            {
                return string.Empty;
            }
            return Encoding.Unicode.GetString(stream.ToArray()).TrimEnd('\0');
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
