using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace PaperClip.Clipboard.Helpers
{
    internal class ClipboardFormatsEnumerator : IEnumerator<DataFormat>
    {
        private readonly ClipboardHelper _clipboardHelper;
        private DataFormat _format;

        public ClipboardFormatsEnumerator(ClipboardHelper clipboardHelper)
        {
            _clipboardHelper = clipboardHelper;
            _clipboardHelper.OpenClipboard();
        }

        public void Dispose()
        {
            _clipboardHelper.CloseClipboard();
        }

        public bool MoveNext()
        {
            // Get next format
            var formatId = _format?.Id ?? 0;
            if ((formatId = _clipboardHelper.EnumClipboardFormats(formatId)) == 0)
            {
                return false;
            }
            _format = DataFormats.GetDataFormat(formatId);

            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public DataFormat Current => _format;

        object IEnumerator.Current => Current;
    }
}
