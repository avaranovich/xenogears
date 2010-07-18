using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Collections.Virtual
{
    [DebuggerNonUserCode]
    public class VirtualDictionary<K, V> : BaseDictionary<K, V>
    {
        private readonly Func<IEnumerable<KeyValuePair<K, V>>> _read;
        private readonly Action<K, V> _create = (k, v) => { throw new NotSupportedException(); };
        private readonly Action<K, V> _update = (k, v) => { throw new NotSupportedException(); };
        private readonly Action<K> _delete = k => { throw new NotSupportedException(); };

        public VirtualDictionary(Func<IEnumerable<KeyValuePair<K, V>>> read)
        {
            _read = read.AssertNotNull();
        }

        public VirtualDictionary(Func<IEnumerable<KeyValuePair<K, V>>> read, Action<K, V> create, Action<K, V> update, Action<K> delete)
            : this(read)
        {
            _read = read;
            _create = create.AssertNotNull();
            _update = update.AssertNotNull();
            _delete = delete.AssertNotNull();
        }

        public override int Count
        {
            get
            {
                return _read().Count();
            }
        }

        public override IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _read().GetEnumerator();
        }

        public override bool ContainsKey(K key)
        {
            return _read().Any(kvp => EqualityComparer<K>.Default.Equals(kvp.Key, key));
        }

        public override bool TryGetValue(K key, out V value)
        {
            if (ContainsKey(key))
            {
                value = _read().Single(kvp => EqualityComparer<K>.Default.Equals(kvp.Key, key)).Value;
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        public override bool IsReadOnly
        {
            get { return _create == null && _update == null && _delete == null; }
        }

        public override void Add(K key, V value)
        {
            _create(key, value);
        }

        public override bool Remove(K key)
        {
            var exists = ContainsKey(key);
            if (exists) _delete(key);
            return exists;
        }

        public override void Clear()
        {
            Keys.ForEach(key => Remove(key));
        }

        protected override void SetValue(K key, V value)
        {
            _update(key, value);
        }
    }
}