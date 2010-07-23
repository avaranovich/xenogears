using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy("System.Collections.Generic.Mscorlib_CollectionDebugView`1,mscorlib,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089")]
    [DebuggerNonUserCode]
    public abstract class BaseList<T> : IList<T>
    {
        protected abstract IEnumerable<T> Read();

        public virtual bool IsReadOnly { get { return true; } }
        protected virtual void InsertAt(int index, T el) {}
        protected virtual void UpdateAt(int index, T el) {}
        public virtual void RemoveAt(int index) {}

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<T> GetEnumerator()
        {
            return Read().GetEnumerator();
        }

        public void Add(T item)
        {
            IsReadOnly.AssertFalse();
            Insert(Count, item);
        }

        public void Clear()
        {
            IsReadOnly.AssertFalse();
            Count.TimesDo(_ => RemoveAt(0));
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Read().ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            IsReadOnly.AssertFalse();

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
            get { return Read().Count(); }
        }

        public int IndexOf(T item)
        {
            return Read().IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            IsReadOnly.AssertFalse();
            InsertAt(index, item);
        }

        public T this[int index]
        {
            get { return Read().Nth(index); }
            set { UpdateAt(index, value); }
        }
    }
}