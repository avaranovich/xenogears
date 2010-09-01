using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace XenoGears.Collections.Dictionaries
{
    /// <summary>
    /// Represents a generic collection of key/value pairs that are ordered independently of the key and value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
    public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary, IDictionary<TKey, TValue>
    {
        new int Count { get; }
        new bool IsReadOnly { get; }
        new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        new ICollection<TKey> Keys { get; }
        new ICollection<TValue> Values { get; }
        new TValue this[int index] { get; set; }

        new int Add(TKey key, TValue value);
        void Insert(int index, TKey key, TValue value);
        new void Clear();
    }
}