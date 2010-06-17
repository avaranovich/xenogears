using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XenoGears.Functional;
using XenoGears.Assertions;

namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    [DebuggerNonUserCode]
    public class Tuple<T1, T2, T3> : ITuple, ITupleImpl
    {
        private T1 m_Item1;
        private T2 m_Item2;
        private T3 m_Item3;
        private ReadOnlyCollection<Object> m_Items;

        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            this.m_Item1 = item1;
            this.m_Item2 = item2;
            this.m_Item3 = item3;

            // performance-wise it better be lazy
            this.m_Items = new Object[] { item1, item2, item3 }.ToReadOnly();
        }

        IMutableTuple ITuple.ToMutable() { return ToMutable(); }
        public MutableTuple<T1, T2, T3> ToMutable()
        {
            return MutableTuple.New(m_Item1, m_Item2, m_Item3);
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
            Tuple<T1, T2, T3> tuple = (other as Tuple<T1, T2, T3>).AssertNotNull();
            int num = 0;
            num = comparer.Compare(this.m_Item1, tuple.m_Item1);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item2, tuple.m_Item2);
            if (num != 0)
            {
                return num;
            }
            return comparer.Compare(this.m_Item3, tuple.m_Item3);
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
            Tuple<T1, T2, T3> tuple = (other as Tuple<T1, T2, T3>).AssertNotNull();
            return ((comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2)) && comparer.Equals(this.m_Item3, tuple.m_Item3));
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3) });
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
            sb.Append(", ");
            sb.Append(this.m_Item2);
            sb.Append(", ");
            sb.Append(this.m_Item3);
            sb.Append(")");
            return sb.ToString();
        }

        int ITupleImpl.Size
        {
            get
            {
                return 3;
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

        public T2 Item2
        {
            get
            {
                return this.m_Item2;
            }
        }

        public T3 Item3
        {
            get
            {
                return this.m_Item3;
            }
        }
    }
}

