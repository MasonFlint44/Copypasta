using System.IO;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardDataModel
    {
        public IOrderedDictionary<string, MemoryStream> ClipboardData { get; }

        public ClipboardDataModel(IOrderedDictionary<string, MemoryStream> clipboardData)
        {
            ClipboardData = clipboardData;
        }
    }
}
