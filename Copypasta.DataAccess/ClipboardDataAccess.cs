using System.Windows;
using Copypasta.DataAccess.Extensions;
using Copypasta.DataAccess.Interfaces;

namespace Copypasta.DataAccess
{
    public class ClipboardDataAccess : IClipboardDataAccess
    {
        // Indicates whether the clipboard data should be persisted after the application terminates
        public bool PersistClipboardData { get; set; } = true;

        public IDataObject ClipboardData
        {
            get => Clipboard.GetDataObject().Clone();
            set => Clipboard.SetDataObject(value, PersistClipboardData);
        }
    }
}
