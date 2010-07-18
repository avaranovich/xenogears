using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XenoGears.Collections
{
    [DebuggerNonUserCode]
    public class ReadOnlyDictionary<K, V> : BaseDictionary<K, V>
    {
        private IDictionary<K, V> _inner;

        public ReadOnlyDictionary(IDictionary<K, V> innerDictionary)
        {
            _inner = innerDictionary;
        }

        public override int Count
        {
            get { return _inner.Count; }
        }

        public override IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public override bool ContainsKey(K key)
        {
            return _inner.ContainsKey(key);
        }

        public override bool TryGetValue(K key, out V value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override void Add(K key, V value)
        {
            throw new NotSupportedException(String.Format(
                "Cannot add item '{0}' to the dictionary: dictionary is read-only", new KeyValuePair<K, V>(key, value)));
        }

        public override bool Remove(K key)
        {
            throw new NotSupportedException(String.Format(
                "Cannot remove item with key '{0}' from the dictionary: dictionary is read-only", key));
        }

        public override void Clear()
        {
            throw new NotSupportedException(String.Format(
                "Cannot clear the dictionary: dictionary is read-only"));
        }

        protected override void SetValue(K key, V value)
        {
            throw new NotSupportedException(String.Format(
                "Cannot add item '{0}' to the dictionary: dictionary is read-only",
                new KeyValuePair<K, V>(key, value)));
        }
    }
}