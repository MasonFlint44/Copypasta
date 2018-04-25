using System;
using System.IO;
using System.Windows.Input;
using Copypasta.Models.Interfaces;
using PaperClip.Collections.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardItemModel: IClipboardItemModel
    {
        private Key _key;
        public event EventHandler<PropertyUpdatedEventArgs<Key>> KeyUpdated;
        public Key Key
        {
            get => _key;
            set
            {
                var oldValue = Key;
                _key = value;
                KeyUpdated?.Invoke(this, new PropertyUpdatedEventArgs<Key>(oldValue, value));
            }
        }

        private IOrderedDictionary<string, MemoryStream> _clipboardData;
        public event EventHandler<PropertyUpdatedEventArgs<IOrderedDictionary<string, MemoryStream>>> ClipboardDataUpdated;
        public IOrderedDictionary<string, MemoryStream> ClipboardData
        {
            get => _clipboardData;
            set
            {
                var oldValue = ClipboardData;
                _clipboardData = value;
                ClipboardDataUpdated?.Invoke(this, 
                    new PropertyUpdatedEventArgs<IOrderedDictionary<string, MemoryStream>>(oldValue, value));
            }
        }

        public ClipboardItemModel(Key key, IOrderedDictionary<string, MemoryStream> clipboardData)
        {
            Key = key;
            ClipboardData = clipboardData;
        }
    }
}
