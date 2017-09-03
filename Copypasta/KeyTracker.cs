using Hooks;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Copypasta
{
    public class KeyTracker
    {
        private Dictionary<Key, DateTime> _keyPressedValues = new Dictionary<Key, DateTime>();
        private KeyboardHook _hook = new KeyboardHook();

        public List<Key> Pressed = new List<Key>();
        public event KeyboardHook.KeyHookEventHandler KeyPressed;
        public event KeyboardHook.KeyHookEventHandler KeyUnpressed;

        public KeyTracker()
        {
            _hook.KeyDown += OnKeyDown;
            _hook.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyHookEventArgs e)
        {
            Release(e.KeyCode);
            KeyUnpressed?.Invoke(this, e);
        }

        private void OnKeyDown(object sender, KeyHookEventArgs e)
        {
            if(IsPressed(e.KeyCode)) { return; }

            Press(e.KeyCode);
            KeyPressed?.Invoke(this, e);
        }

        public DateTime? GetTime(Key key)
        {
            if (!_keyPressedValues.TryGetValue(key, out var value)) { return null; }
            return value;
        }

        public bool IsPressed(Key key)
        {
            return _keyPressedValues.TryGetValue(key, out var value);
        }

        public void Press(Key key)
        {
            if (!_keyPressedValues.ContainsKey(key))
            {
                _keyPressedValues.Add(key, DateTime.UtcNow);
                Pressed.Add(key);
            }
        }

        public void Release(Key key)
        {
            _keyPressedValues.Remove(key);
            Pressed.Remove(key);
        }
    }
}
