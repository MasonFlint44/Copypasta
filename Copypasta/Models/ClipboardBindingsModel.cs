using System;
using System.Collections.Generic;
using System.Windows.Input;
using Copypasta.Models.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardBindingsModel: IClipboardBindingsModel
    {
        private readonly IDictionary<Key, IClipboardItemModel> _clipboardBindings = new Dictionary<Key, IClipboardItemModel>();

        public event EventHandler BindingAdded;

        public void AddBinding(IClipboardItemModel clipboardItem)
        {
            _clipboardBindings[clipboardItem.Key] = clipboardItem;
            BindingAdded?.Invoke(this, EventArgs.Empty);
        }

        public IClipboardItemModel GetData(Key key)
        {
            if(_clipboardBindings.TryGetValue(key, out var clipboardItem))
            {
                return clipboardItem;
            }
            return null;
        }
    }
}
