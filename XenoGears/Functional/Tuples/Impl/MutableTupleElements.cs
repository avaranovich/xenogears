using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Simple;

namespace System
{
    [DebuggerNonUserCode]
    internal class MutableTupleElements : IList<Object>
    {
        private readonly IMutableTuple _tuple;

        public MutableTupleElements(IMutableTuple tuple)
        {
            _tuple = tuple;
        }

        private ReadOnlyCollection<Object> Snapshot
        {
            get
            {
                var size = _tuple.AssertCast<ITupleImpl>().Size;
                return size.Times(i => GetItem(i)).ToReadOnly();
            }
        }

        private Object GetItem(int index)
        {
            var n = index + 1;
            if (n <= 7)
            {
                return _tuple.Get("Item" + n);
            }
            else
            {
                var rest = _tuple.Get("Rest").AssertCast<IMutableTuple>();
                return rest.Items[n - 7];
            }
        }

        private void SetItem(int index, Object item)
        {
            var n = index + 1;
            if (n <= 7)
            {
                _tuple.Set("Item" + n, item);
            }
            else
            {
                var rest = _tuple.Get("Rest").AssertCast<IMutableTuple>();
                rest.Items[n - 7] = item;
            }
        }

        #region Proxy Implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Snapshot.GetEnumerator();
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return Snapshot.GetEnumerator();
        }

        void ICollection<object>.Add(object item)
        {
            ((ICollection<object>)Snapshot).Add(item);
        }

        void ICollection<object>.Clear()
        {
            ((ICollection<object>)Snapshot).Clear();
        }

        bool ICollection<object>.Contains(object item)
        {
            return Snapshot.Contains(item);
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex)
        {
            Snapshot.CopyTo(array, arrayIndex);
        }

        bool ICollection<object>.Remove(object item)
        {
            return ((ICollection<object>)Snapshot).Remove(item);
        }

        int ICollection<object>.Count
        {
            get { return Snapshot.Count; }
        }

        bool ICollection<object>.IsReadOnly
        {
            get { return ((ICollection<object>)Snapshot).IsReadOnly; }
        }

        int IList<object>.IndexOf(object item)
        {
            return Snapshot.IndexOf(item);
        }

        void IList<object>.Insert(int index, object item)
        {
            ((IList<object>)Snapshot).Insert(index, item);
        }

        void IList<object>.RemoveAt(int index)
        {
            ((IList<object>)Snapshot).RemoveAt(index);
        }

        object IList<object>.this[int index]
        {
            get { return GetItem(index); }
            set { SetItem(index, value); }
        }

        #endregion
    }
}
