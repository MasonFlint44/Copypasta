using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels.Designer
{
    internal class DesignerNotificationBalloonViewModel: INotificationBalloonViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ContentText => "This is a sample notification.";
        public Image Image => Properties.Resources.NotifyIcon.ToBitmap();
        public string Title => "Sample notification";
        public int Timeout => 5000;

        public event EventHandler Closing;
        public event EventHandler Closed;
        public event EventHandler Showing;
        public event EventHandler Shown;

        public ICommand ShowCommand { get; }
        public ICommand ShownCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ClosedCommand { get; }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            throw new NotImplementedException();
        }
    }
}
