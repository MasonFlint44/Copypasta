using System.Windows;

namespace Copypasta.DataAccess.Interfaces
{
    public interface IClipboardDataAccess
    {
        bool PersistClipboardData { get; set; }
        IDataObject ClipboardData { get; set; }
    }
}
