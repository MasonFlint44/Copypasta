using Copypasta.Models.Interfaces;
using System;
using System.Windows.Input;

namespace Copypasta.Domain.Interfaces
{
    public interface IClipboardBindingManager
    {
        event EventHandler BindingAdded;
        void AddBinding(IClipboardItemModel clipboardItem);
        IClipboardItemModel GetData(Key key);
    }
}