using System;
using System.Collections;
using System.Collections.Generic;
using PaperClip.Collections.Interfaces;

namespace PaperClip.Collections
{
    public class CircularList<T> : ICircularList<T>
    {
        private T[] _elements;
        private int _head;

        public int Size { get; }
        public int Count { get; private set; }
        public event EventHandler<ICircularListUpdatedEventArgs<T>> ListUpdated;

        public T this[int index]
        {
            get
            {
                int difference;
                if ((difference = (_head - 1) - index) < 0 && Count == Size)
                {
                    return _elements[Size + difference];
                }
                return _elements[difference];
            }
            set
            {
                int difference;
                if ((difference = (_head - 1) - index) < 0 && Count == Size)
                {
                    _elements[Size + difference] = value;
                    return;
                }
                _elements[difference] = value;
            }
        }

        public T First => this[0];

        public T Last => this[Count - 1];

        public CircularList(int size)
        {
            Size = size;
            _elements = new T[size];
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Add(T item)
        {
            // Get element to be removed
            bool wasElementRemoved;
            var removedElement = default(T);
            if (wasElementRemoved = Count == Size)
            {
                removedElement = this[Count - 1];
            }

            _elements[_head++] = item;

            if (_head == Size)
            {
                _head = 0;
            }

            if (Count < Size)
            {
                Count++;
            }

            ListUpdated?.Invoke(this, new CircularListUpdatedEventArgs<T>(item, wasElementRemoved, removedElement));
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void Clear()
        {
            Count = 0;
            _head = 0;
            _elements = new T[Size];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
