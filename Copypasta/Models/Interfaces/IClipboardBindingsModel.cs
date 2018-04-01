using System;
using System.Windows.Input;

namespace Copypasta.Models.Interfaces
{
    public interface IClipboardBindingsModel
    {
        event EventHandler BindingAdded;
        void AddBinding(IClipboardItemModel clipboardItem);
        IClipboardItemModel GetData(Key key);
    }
}