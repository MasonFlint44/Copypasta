using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace PaperClip.Clipboard.Helpers
{
    internal class ClipboardFormatsEnumerable: IEnumerable<DataFormat>
    {
        private readonly ClipboardHelper _clipboardHelper;

        public ClipboardFormatsEnumerable(ClipboardHelper clipboardHelper)
        {
            _clipboardHelper = clipboardHelper;
        }

        public IEnumerator<DataFormat> GetEnumerator()
        {
            return new ClipboardFormatsEnumerator(_clipboardHelper);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
