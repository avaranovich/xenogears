using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Collections.Virtual
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy("System.Collections.Generic.Mscorlib_CollectionDebugView`1,mscorlib,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089")]
    [DebuggerNonUserCode]
    public class VirtualList<T> : IList<T>
    {
        private readonly Func<IEnumerable<T>> _read;
        private readonly Action<int, T> _insertAt = (index, el) => { throw new NotSupportedException(); };
        private readonly Action<int, T> _updateAt = (index, el) => { throw new NotSupportedException(); };
        private readonly Action<int> _removeAt = index => { throw new NotSupportedException(); };

        public VirtualList(Func<IEnumerable<T>> read)
        {
            _read = read.AssertNotNull();
        }

        public VirtualList(Func<IEnumerable<T>> read, Action<int, T> insertAt, Action<int, T> updateAt, Action<int> removeAt)
            : this(read)
        {
            _insertAt = insertAt.AssertNotNull();
            _updateAt = updateAt.AssertNotNull();
            _removeAt = removeAt.AssertNotNull();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<T> GetEnumerator()
        {
            return _read().GetEnumerator();
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            Count.TimesDo(_ => RemoveAt(0));
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            _read().ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            else
            {
                RemoveAt(index);
                return true;
            }
        }

        public int Count
        {
            get { return _read().Count(); }
        }

        public bool IsReadOnly
        {
            get { return _insertAt == null && _updateAt != null && _removeAt == null; }
        }

        public int IndexOf(T item)
        {
            return _read().IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _insertAt(index, item);
        }

        public void RemoveAt(int index)
        {
            _removeAt(index);
        }

        public T this[int index]
        {
            get { return _read().Nth(index); }
            set { _updateAt(index, value); }
        }
    }
}