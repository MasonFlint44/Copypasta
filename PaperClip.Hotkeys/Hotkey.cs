using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PaperClip.Hotkeys.Interfaces;
using PaperClip.Trackers;
using PaperClip.Trackers.Interfaces;

namespace PaperClip.Hotkeys
{
    public class Hotkey : IHotkey
    {
        private readonly IKeyTracker _keyTracker;
        private int _combosIndex;
        private readonly List<IKeyCombo> _combos;
        private HashSet<Key> _pressedKeys;
        private IKeyCombo CurrentCombo => _combos[_combosIndex];

        public event EventHandler<IHotkeyEventArgs> HotkeyPressed;
        public bool Handled { get; set; }
        public bool IsPressed { get; private set; }

        public Hotkey(IKeyTracker keyTracker, params IKeyCombo[] combos)
        {
            _keyTracker = keyTracker;
            _combos = combos.ToList();
            _keyTracker.KeyPressed += OnKeyPressed;
            _keyTracker.KeyUnpressed += OnKeyUnpressed;
        }

        public Hotkey(params IKeyCombo[] combos) : this(new KeyTracker(), combos)
        {
        }

        private void OnKeyPressed(object sender, IKeyTrackerEventArgs e)
        {
            if (CurrentCombo.IsPressed(_keyTracker.Modifiers, e.Key))
            {
                if (_combos.Count != ++_combosIndex) return;

                _combosIndex = 0;

                var args = new HotkeyEventArgs
                {
                    Handled = Handled
                };

                _pressedKeys = _keyTracker.GetPressedKeysSet();
                IsPressed = true;
                HotkeyPressed?.Invoke(this, args);

                e.Handled = args.Handled;
            }
            else if (!e.IsModifier)
            {
                IsPressed = false;
                _combosIndex = 0;
            }
        }

        private void OnKeyUnpressed(object sender, IKeyTrackerEventArgs e)
        {
            if (!IsPressed) { return; }
            if (!_pressedKeys.Contains(e.Key)) { return; }

            IsPressed = false;
            _combosIndex = 0;
        }
    }
}
