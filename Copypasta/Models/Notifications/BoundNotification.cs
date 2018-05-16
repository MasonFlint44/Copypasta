using System.Windows.Input;

namespace Copypasta.Models.Notifications
{
    public class BoundNotification
    {
        public Key Key { get; }
        public ClipboardDataModel ClipboardData { get; }

        public BoundNotification(Key key, ClipboardDataModel clipboardData)
        {
            Key = key;
            ClipboardData = clipboardData;
        }
    }
}