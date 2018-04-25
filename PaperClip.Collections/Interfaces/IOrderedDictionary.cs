using System;
using System.Collections.Generic;

namespace PaperClip.Collections.Interfaces
{
    public interface IOrderedDictionary<TKey, TValue>
    {
        TValue this[int index] { get; set; }
        TValue this[TKey key] { get; set; }

        int Count { get; }
        bool IsReadOnly { get; }
        ICollection<TKey> Keys { get; }
        ICollection<TValue> Values { get; }

        void Add(KeyValuePair<TKey, TValue> item);
        void Add(TKey key, TValue value);
        void Clear();
        bool Contains(KeyValuePair<TKey, TValue> item);
        bool ContainsKey(TKey key);
        //void CopyTo(Array array, int index);
        void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
        void Insert(int index, TKey key, TValue value);
        bool Remove(KeyValuePair<TKey, TValue> item);
        bool Remove(TKey key);
        void RemoveAt(int index);
        bool TryGetValue(TKey key, out TValue value);
    }
}