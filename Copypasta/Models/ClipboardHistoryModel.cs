using Copypasta.Models.Interfaces;
using PaperClip.Collections;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardHistoryModel: IClipboardHistoryModel
    {
        public ICircularList<IClipboardItemModel> History { get; }

        public ClipboardHistoryModel(int historyCount)
        {
            History = new CircularList<IClipboardItemModel>(historyCount);
        }
    }
}
