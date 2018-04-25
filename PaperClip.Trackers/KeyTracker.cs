using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PaperClip.Hooks.KeyboardHook;
using PaperClip.Hooks.KeyboardHook.Interfaces;
using PaperClip.Trackers.Interfaces;

namespace PaperClip.Trackers
{
    public class KeyTracker: IKeyTracker
    {
        private readonly IKeyboardHook _keyboardHook;
        private readonly HashSet<Key> _pressed = new HashSet<Key>();
        private readonly Dictionary<Key, ModifierKeys> _leftModifierMappings = new Dictionary<Key, ModifierKeys>
        {
            { Key.LeftCtrl, ModifierKeys.Control },
            { Key.LeftAlt, ModifierKeys.Alt },
            { Key.LeftShift, ModifierKeys.Shift },
            { Key.LWin, ModifierKeys.Windows }

        };
        private readonly Dictionary<Key, ModifierKeys> _rightModifierMappings = new Dictionary<Key, ModifierKeys>
        {
            { Key.RightCtrl, ModifierKeys.Control },
            { Key.RightAlt, ModifierKeys.Alt },
            { Key.RightShift, ModifierKeys.Shift },
            { Key.RWin, ModifierKeys.Windows }
        };
        private ModifierKeys _leftModifiers;
        private ModifierKeys _rightModifiers;

        public ModifierKeys Modifiers { get; private set; }
        public event EventHandler<IKeyTrackerEventArgs> KeyPressed;
        public event EventHandler<IKeyTrackerEventArgs> KeyUnpressed;

        public KeyTracker(IKeyboardHook keyboardHook)
        {
            _keyboardHook = keyboardHook;
        }

        public KeyTracker()
        {
            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyDown += OnKeyDown;
            _keyboardHook.KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object sender, IKeyboardHookEventArgs e)
        {
            if (IsKeyPressed(e.Key)) { return; }

            var isModifier = false;
            if (_leftModifierMappings.TryGetValue(e.Key, out var modifier))
            {
                _leftModifiers = _leftModifiers | modifier;
                Modifiers = Modifiers | _leftModifiers;
                isModifier = true;
            }
            else if (_rightModifierMappings.TryGetValue(e.Key, out modifier))
            {
                _rightModifiers = _rightModifiers | modifier;
                Modifiers = Modifiers | _rightModifiers;
                isModifier = true;
            }
            _pressed.Add(e.Key);

            var args = new KeyTrackerEventArgs
            {
                Handled = e.Handled,
                Key = e.Key,
                IsModifier = isModifier
            };
             KeyPressed?.Invoke(this, args);

            e.Handled = args.Handled;
        }

        private void OnKeyUp(object sender, IKeyboardHookEventArgs e)
        {
            var isModifier = false;
            if (_leftModifierMappings.TryGetValue(e.Key, out var modifier))
            {
                _leftModifiers = _leftModifiers ^ modifier;
                Modifiers = _leftModifiers | _rightModifiers;
                isModifier = true;
            }
            else if (_rightModifierMappings.TryGetValue(e.Key, out modifier))
            {
                _rightModifiers = _rightModifiers ^ modifier;
                Modifiers = _leftModifiers | _rightModifiers;
                isModifier = true;
            }
            _pressed.Remove(e.Key);

            var args = new KeyTrackerEventArgs
            {
                Handled = e.Handled,
                Key = e.Key,
                IsModifier = isModifier
            };
            KeyUnpressed?.Invoke(this, args);

            e.Handled = args.Handled;
        }

        public bool IsKeyPressed(Key key)
        {
            return _pressed.Contains(key);
        }

        public List<Key> GetPressedKeys()
        {
            return _pressed.ToList();
        }

        public HashSet<Key> GetPressedKeysSet()
        {
            return new HashSet<Key>(_pressed);
        }
    }
}
