using PaperClip.Collections.Interfaces;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardHistoryModel
    {
        ICircularList<IClipboardItemModel> History { get; }
    }
}