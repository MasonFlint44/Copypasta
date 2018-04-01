using System;
using Copypasta.Models.Interfaces;

namespace Copypasta.Models
{
    public class PropertyUpdatedEventArgs<T>: EventArgs, IPropertyUpdatedEventArgs<T>
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
