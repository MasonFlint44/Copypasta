using System.Windows.Input;
using PaperClip.Hotkeys.Interfaces;

namespace PaperClip.Hotkeys
{
    public class KeyCombo : IKeyCombo
    {
        public ModifierKeys? Modifiers { get; }
        public Key Key { get; }

        public KeyCombo(Key key)
        {
            Key = key;
        }

        public KeyCombo(ModifierKeys modifiers, Key key)
        {
            Modifiers = modifiers;
            Key = key;
        }

        public bool IsPressed(ModifierKeys? modifiers, Key key)
        {
            if (Modifiers != null)
            {
                return modifiers == Modifiers && key == Key;
            }
            return key == Key;
        }
    }
}
