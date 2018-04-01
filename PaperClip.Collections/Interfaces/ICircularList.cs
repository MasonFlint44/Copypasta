using System;
using System.Collections.Generic;

namespace PaperClip.Collections.Interfaces
{
    public interface ICircularList<T>: IEnumerable<T>
    {
        T this[int index] { get; set; }

        event EventHandler<ICircularListUpdatedEventArgs<T>> ListUpdated;
        int Count { get; }
        T First { get; }
        T Last { get; }
        int Size { get; }

        void Add(T item);
        void Clear();
        bool Contains(T item);
        int IndexOf(T item);
    }
}