using System.Windows.Input;

namespace PaperClip.Trackers.Interfaces
{
    public interface IKeyTrackerEventArgs: ITrackerEventArgs
    {
        Key Key { get; }
        bool IsModifier { get; }
    }
}
