using System.Windows.Input;

namespace Copypasta.Models.Notifications
{
    public class HistoryRecordNotification
    {
        public Key Key { get; }
        public ClipboardDataModel ClipboardData { get; }

        public HistoryRecordNotification(Key key, ClipboardDataModel clipboardData)
        {
            Key = key;
            ClipboardData = clipboardData;
        }
    }
}
