using System.Windows.Input;

namespace PaperClip.Hotkeys.Interfaces
{
    public interface IKeyCombo
    {
        Key Key { get; }
        ModifierKeys? Modifiers { get; }
        bool IsPressed(ModifierKeys? modifiers, Key key);
    }
}