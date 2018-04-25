using System;

namespace Copypasta
{
    public class PropertyUpdatedEventArgs<T>: EventArgs
    {
        public T OldValue { get; }
        public T NewValue { get; }

        public PropertyUpdatedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
