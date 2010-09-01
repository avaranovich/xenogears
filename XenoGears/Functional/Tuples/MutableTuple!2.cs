using System.Diagnostics;
using XenoGears.Assertions;

namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    [DebuggerNonUserCode]
    public class MutableTuple<T1, T2> : IMutableTuple, ITupleImpl
    {
        private T1 m_Item1;
        private T2 m_Item2;
        private MutableTupleElements m_Items;

        public MutableTuple(T1 item1, T2 item2)
        {
            this.m_Item1 = item1;
            this.m_Item2 = item2;
            this.m_Items = new MutableTupleElements(this);
        }

        ITuple IMutableTuple.ToImmutable() { return ToImmutable(); }
        public Tuple<T1, T2> ToImmutable()
        {
            return Tuple.New(m_Item1, m_Item2);
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
            MutableTuple<T1, T2> MutableTuple = (other as MutableTuple<T1, T2>).AssertNotNull();
            int num = 0;
            num = comparer.Compare(this.m_Item1, MutableTuple.m_Item1);
            if (num != 0)
            {
                return num;
            }
            return comparer.Compare(this.m_Item2, MutableTuple.m_Item2);
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
            MutableTuple<T1, T2> MutableTuple = (other as MutableTuple<T1, T2>).AssertNotNull();
            return (comparer.Equals(this.m_Item1, MutableTuple.m_Item1) && comparer.Equals(this.m_Item2, MutableTuple.m_Item2));
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return TupleImplHelper.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2));
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
            sb.Append(")");
            return sb.ToString();
        }

        int ITupleImpl.Size
        {
            get
            {
                return 2;
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

        public T2 Item2
        {
            get
            {
                return this.m_Item2;
            }
            set
            {
                this.m_Item2 = value;
            }
        }
    }
}

