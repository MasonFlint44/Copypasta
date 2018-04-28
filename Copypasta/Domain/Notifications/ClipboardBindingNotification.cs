using System.Windows.Input;
using Copypasta.Models;

namespace Copypasta.Domain.Notifications
{
    public class ClipboardBindingNotification
    {
        public Key Key { get; }
        public ClipboardDataModel ClipboardData { get; }

        public ClipboardBindingNotification(Key key, ClipboardDataModel clipboardData)
        {
            Key = key;
            ClipboardData = clipboardData;
        }
    }
}
