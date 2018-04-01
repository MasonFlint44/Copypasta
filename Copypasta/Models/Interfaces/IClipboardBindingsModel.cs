using System.Windows.Input;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardBindingsModel
    {
        void AddBinding(Key key, IClipboardItemModel clipboardItem);
        IClipboardItemModel GetData(Key key);
    }
}