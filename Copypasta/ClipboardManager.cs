using System;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;

namespace Copypasta
{
    public class ClipboardManager
    {
        private Dictionary<Key, DataObject> _bindings = new Dictionary<Key, DataObject>();

        // Save data in clipboard to provided key
        //[STAThread]
        public void SaveClipboardTo(Key key)
        {
            var data = Clipboard.GetDataObject();
            if (_bindings.ContainsKey(key))
            {
                _bindings.Remove(key);
            }
            _bindings.Add(key, Copy(data));
        }

        // Looks up data for provided key and copies it to the clipboard
        //[STAThread]
        public bool CopyToClipboard(Key key)
        {
            bool success;
            if (success = _bindings.TryGetValue(key, out var data))
            {
                Clipboard.SetDataObject(data, true);
            }
            return success;
        }

        // Copy data to a new instance of DataObject
        private DataObject Copy(IDataObject source)
        {
            var dataObject = new DataObject();
            foreach (var format in source.GetFormats())
            {
                dataObject.SetData(format, source.GetData(format));
            }
            return dataObject;
        }
    }
}
