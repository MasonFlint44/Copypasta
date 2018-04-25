using Copypasta.Domain.Interfaces;
using Copypasta.Models.Interfaces;
using PaperClip.Collections;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Domain
{
    public class ClipboardHistoryManager: IClipboardHistoryManager
    {
        public ICircularList<IClipboardItemModel> History { get; }

        public ClipboardHistoryManager(int historyCount)
        {
            History = new CircularList<IClipboardItemModel>(historyCount);
        }
    }
}
