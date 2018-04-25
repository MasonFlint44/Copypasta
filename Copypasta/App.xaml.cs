using System.Windows;
using System.Windows.Input;
using WindowsInput;
using Copypasta.Controllers;
using Copypasta.Domain;
using Copypasta.ViewModels;
using Copypasta.Views;
using PaperClip.Hotkeys;
using PaperClip.Trackers;
using Clipboard = PaperClip.Clipboard.Clipboard;

namespace Copypasta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var clipboard = new Domain.Clipboard(new Clipboard());
            var clipboardHistoryManager = new ClipboardHistoryManager(10);
            var clipboardBindingManager = new ClipboardBindingManager();
            var keyTracker = new KeyTracker();
            var ctrlCHotkey = new Hotkey(keyTracker, new KeyCombo(ModifierKeys.Control, Key.C));
            var ctrlVHotkey = new Hotkey(keyTracker, new KeyCombo(ModifierKeys.Control, Key.V));
            var escHotkey = new Hotkey(keyTracker, new KeyCombo(Key.Escape));
            var inputSimulator = new InputSimulator();
            var notificationViewModel = new NotificationViewModel();
            var historyMenuViewModel = new HistoryMenuViewModel(clipboardHistoryManager);
            var copypasta = new CopypastaController(clipboard, clipboardHistoryManager, clipboardBindingManager,
                ctrlVHotkey, ctrlCHotkey, escHotkey, keyTracker, inputSimulator, notificationViewModel);
            var notifyIconWindow = new NotifyIconWindow(notificationViewModel, historyMenuViewModel);
        }
    }
}
