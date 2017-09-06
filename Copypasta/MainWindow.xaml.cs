using System;
using System.Windows;
using System.Windows.Media.Animation;
using Copypasta.Notifications;
using Hotkeys;

namespace Copypasta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Hotkey _hotkey = new Hotkey(new KeyCombo("ctrl").And("v"));
        private ClipboardNotification clipboardNotification;

        private Stateless.StateMachine<string, string> fsm = new Stateless.StateMachine<string, string>("initial");

        public MainWindow()
        {
            _hotkey.HotkeyPressed += HotkeyPressed;
            InitializeComponent();

            clipboardNotification = new ClipboardNotification();
            clipboardNotification.ClipboardUpdated += OnClipboardUpdate;
        }

        private void OnClipboardUpdate(object sender, ClipboardUpdatedEventArgs e)
        {
            Console.WriteLine("Clipboard updated 1");
        }

        private void HotkeyPressed(object sender, EventArgs e)
        {
            Close();
        }

        private void KeyPressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            KeyPressTextBox.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));
            BeginAnimation(OpacityProperty, fadeOut);
        }

        private void OnClipboardUpdated(object sender, ClipboardUpdatedEventArgs e)
        {
            Console.WriteLine("Clipboard updated");
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            clipboardNotification = new ClipboardNotification(this);
            clipboardNotification.ClipboardUpdated += OnClipboardUpdated;
        }
    }
}
