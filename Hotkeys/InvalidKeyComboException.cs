using System;

namespace Hotkeys
{
    public class InvalidKeyComboException : Exception
    {
        public InvalidKeyComboException() { }

        public InvalidKeyComboException(string message) : base(message) { }

        public InvalidKeyComboException(string message, Exception innerException) : base(message, innerException) { }
    }
}
