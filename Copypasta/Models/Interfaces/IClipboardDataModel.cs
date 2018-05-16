using System.IO;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardDataModel
    {
        IOrderedDictionary<string, MemoryStream> ClipboardData { get; }
    }
}