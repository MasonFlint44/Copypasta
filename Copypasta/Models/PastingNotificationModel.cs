using Copypasta.Models.Interfaces;

namespace Copypasta.Models
{
    public class PastingNotificationModel: ISimpleNotificationModel
    {
        public string Title => "Pasting";
        public string Content => "Press a key to paste content";
    }
}
