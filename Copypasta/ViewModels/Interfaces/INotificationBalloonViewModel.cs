using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;

namespace Copypasta.ViewModels.Interfaces
{
    public interface INotificationBalloonViewModel: INotifyPropertyChanged
    {
        Image Image { get; }
        string Title { get; }
        string ContentText { get; }
        int Timeout { get; }

        event EventHandler Showing;
        event EventHandler Shown;
        event EventHandler Closing;
        event EventHandler Closed;

        ICommand ShowCommand { get; }
        ICommand ShownCommand { get; }
        ICommand CloseCommand { get; }
        ICommand ClosedCommand { get; }

        void Show();
        void Close();
    }
}