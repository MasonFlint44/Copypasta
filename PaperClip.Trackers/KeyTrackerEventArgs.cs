using System.Windows.Input;
using PaperClip.Trackers.Interfaces;

namespace PaperClip.Trackers
{
    public class KeyTrackerEventArgs : IKeyTrackerEventArgs
    {
        public Key Key { get; protected internal set; }
        public bool IsModifier { get; protected internal set; }
        public bool Handled { get; set; }
    }
}
