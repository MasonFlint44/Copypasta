using System.Windows.Input;

namespace Trackers
{
    public class KeyTrackerEventArgs : TrackerEventArgs
    {
        public Key KeyCode { get; set; }
    }
}
