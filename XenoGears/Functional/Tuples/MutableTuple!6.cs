using System.Diagnostics;
using XenoGears.Assertions;

namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    [DebuggerNonUserCode]
    public class MutableTuple<T1, T2, T3, T4, T5, T6> : IMutableTuple, ITupleImpl
    {
        private T1 m_Item1;
        private T2 m_Item2;
        private T3 m_Item3;
        private T4 m_Item4;
        private T5 m_Item5;
        private T6 m_Item6;
        private MutableTupleElements m_Items;

        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            this.m_Item1 = item1;
            this.m_Item2 = item2;
            this.m_Item3 = item3;
            this.m_Item4 = item4;
            this.m_Item5 = item5;
            this.m_Item6 = item6;
            this.m_Items = new MutableTupleElements(this);
        }

        ITuple IMutableTuple.ToImmutable() { return ToImmutable(); }
        public Tuple<T1, T2, T3, T4, T5, T6> ToImmutable()
        {
            return Tuple.New(m_Item1, m_Item2, m_Item3, m_Item4, m_Item5, m_Item6);
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
            MutableTuple<T1, T2, T3, T4, T5, T6> MutableTuple = (other as MutableTuple<T1, T2, T3, T4, T5, T6>).AssertNotNull();
            int num = 0;
            num = comparer.Compare(this.m_Item1, MutableTuple.m_Item1);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item2, MutableTuple.m_Item2);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item3, MutableTuple.m_Item3);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item4, MutableTuple.m_Item4);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item5, MutableTuple.m_Item5);
            if (num != 0)
            {
                return num;
            }
            return comparer.Compare(this.m_Item6, MutableTuple.m_Item6);
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
            MutableTuple<T1, T2, T3, T4, T5, T6> MutableTuple = (other as MutableTuple<T1, T2, T3, T4, T5, T6>).AssertNotNull();
            return ((((comparer.Equals(this.m_Item1, MutableTuple.m_Item1) && comparer.Equals(this.m_Item2, MutableTuple.m_Item2)) && (comparer.Equals(this.m_Item3, MutableTuple.m_Item3) && comparer.Equals(this.m_Item4, MutableTuple.m_Item4))) && comparer.Equals(this.m_Item5, MutableTuple.m_Item5)) && comparer.Equals(this.m_Item6, MutableTuple.m_Item6));
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6) });
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
            sb.Append(", ");
            sb.Append(this.m_Item4);
            sb.Append(", ");
            sb.Append(this.m_Item5);
            sb.Append(", ");
            sb.Append(this.m_Item6);
            sb.Append(")");
            return sb.ToString();
        }

        int ITupleImpl.Size
        {
            get
            {
                return 6;
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

        public T3 Item3
        {
            get
            {
                return this.m_Item3;
            }
            set
            {
                this.m_Item3 = value;
            }
        }

        public T4 Item4
        {
            get
            {
                return this.m_Item4;
            }
            set
            {
                this.m_Item4 = value;
            }
        }

        public T5 Item5
        {
            get
            {
                return this.m_Item5;
            }
            set
            {
                this.m_Item5 = value;
            }
        }

        public T6 Item6
        {
            get
            {
                return this.m_Item6;
            }
            set
            {
                this.m_Item6 = value;
            }
        }
    }
}

