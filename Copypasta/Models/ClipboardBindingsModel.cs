using System.Collections.Generic;
using System.Windows.Input;
using Copypasta.Models.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardBindingsModel: IClipboardBindingsModel
    {
        private readonly IDictionary<Key, IClipboardItemModel> _clipboardBindings = new Dictionary<Key, IClipboardItemModel>();

        public void AddBinding(Key key, IClipboardItemModel clipboardItem)
        {
            clipboardItem.Key = key;
            _clipboardBindings[key] = clipboardItem;
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
