using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PaperClip.Trackers.Interfaces
{
    public interface IKeyTracker
    {
        ModifierKeys Modifiers { get; }
        event EventHandler<IKeyTrackerEventArgs> KeyPressed;
        event EventHandler<IKeyTrackerEventArgs> KeyUnpressed;
        bool IsKeyPressed(Key key);
        List<Key> GetPressedKeys();
        HashSet<Key> GetPressedKeysSet();
    }
}
