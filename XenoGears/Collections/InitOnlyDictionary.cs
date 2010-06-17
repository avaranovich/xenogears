using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XenoGears.Collections
{
    // todo. once and forever implement a dictionary that can dynamically switch state:
    // todo. regular, add-only, edit-only, readonly
    // todo. after that redo certain classes like StructuralMaps and InferenceCaches
    // todo. i mean: more precisely define their behavior using new capabilities
    [DebuggerNonUserCode]
    public class InitOnlyDictionary<K, V> : BaseDictionary<K, V>
    {
        private IDictionary<K, V> _inner;

        public InitOnlyDictionary()
            : this(new Dictionary<K, V>())
        {
        }

        public InitOnlyDictionary(IDictionary<K, V> innerDictionary)
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
            get { return false; }
        }

        public override void Add(K key, V value)
        {
            _inner.Add(key, value);
        }

        public override bool Remove(K key)
        {
            throw new NotSupportedException(String.Format(
                "Cannot remove item with key '{0}' from the dictionary: dictionary is init-only", key));
        }

        public override void Clear()
        {
            throw new NotSupportedException(String.Format(
                "Cannot clear the dictionary: dictionary is init-only"));
        }

        protected override void SetValue(K key, V value)
        {
            if (!_inner.ContainsKey(key))
            {
                Add(key, value);
            }
            else
            {
                if (!EqualityComparer<V>.Default.Equals(_inner[key], value))
                {
                    throw new NotSupportedException(String.Format(
                        "Cannot assign a value '{0}' for the key '{1}': " +
                        "dictionary is init-only and it already contains the value '{2}'",
                        value, key, _inner[key]));
                }
            }
        }
    }
}