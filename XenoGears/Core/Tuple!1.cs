using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    [DebuggerNonUserCode]
    public class Tuple<T1> : ITuple, ITupleImpl
    {
        private T1 m_Item1;
        private ReadOnlyCollection<Object> m_Items;

        public Tuple(T1 item1)
        {
            this.m_Item1 = item1;

            // performance-wise it better be lazy
            this.m_Items = new Object[]{item1}.ToReadOnly();
        }

        IMutableTuple ITuple.ToMutable() { return ToMutable(); }
        public MutableTuple<T1> ToMutable()
        {
            return MutableTuple.New(m_Item1);
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo(obj, Comparer<object>.Default);
        }

        public int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            Tuple<T1> tuple = (other as Tuple<T1>).AssertNotNull();
            return comparer.Compare(this.m_Item1, tuple.m_Item1);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<object>.Default);
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
            {
                return false;
            }
            Tuple<T1> tuple = (other as Tuple<T1>).AssertNotNull();
            return comparer.Equals(this.m_Item1, tuple.m_Item1);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.m_Item1);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            return ((ITupleImpl)this).ToString(sb);
        }

        int ITupleImpl.GetHashCode(IEqualityComparer comparer)
        {
            return this.GetHashCode(comparer);
        }

        string ITupleImpl.ToString(StringBuilder sb)
        {
            sb.Append(this.m_Item1);
            sb.Append(")");
            return sb.ToString();
        }

        int ITupleImpl.Size
        {
            get
            {
                return 1;
            }
        }

        ReadOnlyCollection<Object> ITuple.Items
        {
            get
            {
                return m_Items;
            }
        }

        public T1 Item1
        {
            get
            {
                return this.m_Item1;
            }
        }
    }
}

