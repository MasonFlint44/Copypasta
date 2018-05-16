using System.IO;
using Copypasta.Models.Interfaces;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardDataModel : IClipboardDataModel
    {
        public IOrderedDictionary<string, MemoryStream> ClipboardData { get; }

        public ClipboardDataModel(IOrderedDictionary<string, MemoryStream> clipboardData)
        {
            ClipboardData = clipboardData;
        }
    }
}
