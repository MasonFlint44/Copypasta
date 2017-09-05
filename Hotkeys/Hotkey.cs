using System;
using Trackers;

namespace Hotkeys
{
    public class Hotkey
    {
        private KeyTracker _keyTracker = new KeyTracker();
        private KeyCombo _keyCombo;

        public event EventHandler HotkeyPressed;
        public bool Handled { get; set; }

        public Hotkey(KeyCombo combo)
        {
            _keyCombo = combo;
            _keyTracker.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyTrackerEventArgs e)
        {
            if(_keyTracker.Pressed.Count != _keyCombo.Count) { return; }

            for(int i = 0; i < _keyTracker.Pressed.Count; i++)
            {
                if(!_keyCombo.IsValid(i, _keyTracker.Pressed[i]))
                {
                    return;
                }
            }
            e.Handled = Handled;
            HotkeyPressed?.Invoke(this, e);
        }
    }
}
