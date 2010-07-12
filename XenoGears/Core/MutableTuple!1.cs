using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Core.Impl;

namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    [DebuggerNonUserCode]
    public class MutableTuple<T1> : IMutableTuple, ITupleImpl
    {
        private T1 m_Item1;
        private MutableTupleElements m_Items;

        public MutableTuple(T1 item1)
        {
            this.m_Item1 = item1;
            this.m_Items = new MutableTupleElements(this);
        }

        ITuple IMutableTuple.ToImmutable() { return ToImmutable(); }
        public Tuple<T1> ToImmutable()
        {
            return Tuple.New(m_Item1);
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
            MutableTuple<T1> MutableTuple = (other as MutableTuple<T1>).AssertNotNull();
            return comparer.Compare(this.m_Item1, MutableTuple.m_Item1);
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
            MutableTuple<T1> MutableTuple = (other as MutableTuple<T1>).AssertNotNull();
            return comparer.Equals(this.m_Item1, MutableTuple.m_Item1);
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

        IList<Object> IMutableTuple.Items
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
            set
            {
                this.m_Item1 = value;
            }
        }
    }
}

