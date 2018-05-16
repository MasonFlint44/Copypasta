using Copypasta.Models.Interfaces;

namespace Copypasta.Models
{
    public class CopyingNotificationModel: ISimpleNotificationModel
    {
        public string Title => "Copying";
        public string Content => "Press a key to bind clipboard";
    }
}
