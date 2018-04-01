using System;
using System.Windows;
using System.Windows.Input;
using Copypasta.Models.Interfaces;

namespace Copypasta.Models
{
    public class ClipboardItemModel: IClipboardItemModel
    {
        private Key _key;
        public event EventHandler<IPropertyUpdatedEventArgs<Key>> KeyUpdated;
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

        private IDataObject _clipboardData;
        public event EventHandler<IPropertyUpdatedEventArgs<IDataObject>> ClipboardDataUpdated;
        public IDataObject ClipboardData
        {
            get => _clipboardData;
            set
            {
                var oldValue = ClipboardData;
                _clipboardData = value;
                ClipboardDataUpdated?.Invoke(this, new PropertyUpdatedEventArgs<IDataObject>(oldValue, value));
            }
        }

        public ClipboardItemModel(Key key, IDataObject clipboardData)
        {
            Key = key;
            ClipboardData = clipboardData;
        }
    }
}
