﻿using System;

namespace PaperClip.Clipboard.Interfaces
{
    public interface IClipboardMonitor
    {
        event EventHandler<ClipboardUpdatedEventArgs> ClipboardUpdated;
    }
}
