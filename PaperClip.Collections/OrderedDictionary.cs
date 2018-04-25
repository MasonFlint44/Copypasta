using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using PaperClip.Collections.Interfaces;

namespace PaperClip.Collections
{
    public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IOrderedDictionary<TKey, TValue>
    {
        private readonly OrderedDictionary _orderedDictionary = new OrderedDictionary();

        public bool ContainsKey(TKey key)
        {
            return _orderedDictionary.Contains(key);
        }

        public void Add(TKey key, TValue value)
        {
            _orderedDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _orderedDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!TryGetValue(item.Key, out var value)) return false;
            if (!new KeyValuePair<TKey, TValue>(item.Key, value).Equals(item)) return false;
            return true;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var i = 0;
            foreach (var element in this)
            {
                array[i++] = element;

                if(i == array.Length) { break; }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if(!Contains(item)) { return false; }
            Remove(item.Key);
            return true;
        }

        public void Insert(int index, TKey key, TValue value)
        {
            _orderedDictionary.Insert(index, key, value);
        }

        public void RemoveAt(int index)
        {
            _orderedDictionary.RemoveAt(index);
        }

        public TValue this[int index]
        {
            get => (TValue)_orderedDictionary[index];
            set => _orderedDictionary[index] = value;
        }

        public bool Remove(TKey key)
        {
            var keyPresent = _orderedDictionary.Contains(key);
            _orderedDictionary.Remove(key);
            return keyPresent;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            if (!_orderedDictionary.Contains(key)) { return false; }
            value = (TValue)_orderedDictionary[key];
            return true;
        }

        public TValue this[TKey key]
        {
            get => (TValue)_orderedDictionary[key];
            set => _orderedDictionary[key] = value;
        }

        public ICollection<TKey> Keys => _orderedDictionary.Keys.Cast<TKey>().ToList();
        public ICollection<TValue> Values => _orderedDictionary.Values.Cast<TValue>().ToList();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var enumerator = _orderedDictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return new KeyValuePair<TKey, TValue>((TKey)enumerator.Key, (TValue)enumerator.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _orderedDictionary.GetEnumerator();
        }

        public int Count => _orderedDictionary.Count;
        public bool IsReadOnly => _orderedDictionary.IsReadOnly;
    }
}
