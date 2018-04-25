using Copypasta.Models.Interfaces;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Domain.Interfaces
{
    public interface IClipboardHistoryManager
    {
        ICircularList<IClipboardItemModel> History { get; }
    }
}