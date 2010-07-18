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
    public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : ITuple, ITupleImpl
        where TRest : ITuple
    {
        private T1 m_Item1;
        private T2 m_Item2;
        private T3 m_Item3;
        private T4 m_Item4;
        private T5 m_Item5;
        private T6 m_Item6;
        private T7 m_Item7;
        private TRest m_Rest;
        private ReadOnlyCollection<Object> m_Items;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
        {
            (rest is ITupleImpl).AssertTrue();

            this.m_Item1 = item1;
            this.m_Item2 = item2;
            this.m_Item3 = item3;
            this.m_Item4 = item4;
            this.m_Item5 = item5;
            this.m_Item6 = item6;
            this.m_Item7 = item7;
            this.m_Rest = rest;

            // performance-wise it better be lazy
            var sevenItems = new Object[] { item1, item2, item3, item4, item5, item6, item7 };
            this.m_Items = sevenItems.Concat(((ITuple)rest).Items).ToReadOnly();
        }

        IMutableTuple ITuple.ToMutable() { return ToMutable(); }

        public MutableTuple<T1, T2, T3, T4, T5, T6, T7, IMutableTuple> ToMutable()
        {
            return MutableTuple.New(m_Item1, m_Item2, m_Item3, m_Item4, m_Item5, m_Item6, m_Item7, m_Rest.ToMutable());
        }

        public MutableTuple<T1, T2, T3, T4, T5, T6, T7, TMutableRest> ToMutable<TMutableRest>()
            where TMutableRest : IMutableTuple
        {
            return MutableTuple.New(m_Item1, m_Item2, m_Item3, m_Item4, m_Item5, m_Item6, m_Item7, (TMutableRest)m_Rest.ToMutable());
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
            Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = (other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).AssertNotNull();
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
            num = comparer.Compare(this.m_Item3, tuple.m_Item3);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item4, tuple.m_Item4);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item5, tuple.m_Item5);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item6, tuple.m_Item6);
            if (num != 0)
            {
                return num;
            }
            num = comparer.Compare(this.m_Item7, tuple.m_Item7);
            if (num != 0)
            {
                return num;
            }
            return comparer.Compare(this.m_Rest, tuple.m_Rest);
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
            Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = (other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>).AssertNotNull();
            return ((((comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2)) && (comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4))) && ((comparer.Equals(this.m_Item5, tuple.m_Item5) && comparer.Equals(this.m_Item6, tuple.m_Item6)) && comparer.Equals(this.m_Item7, tuple.m_Item7))) && comparer.Equals(this.m_Rest, tuple.m_Rest));
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<object>.Default);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            ITupleImpl rest = (ITupleImpl) this.m_Rest;
            if (rest.Size >= 8)
            {
                return rest.GetHashCode(comparer);
            }
            switch ((8 - rest.Size))
            {
                case 1:
                    return TupleImplHelper.CombineHashCodes(comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer));

                case 2:
                    return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer) });

                case 3:
                    return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer) });

                case 4:
                    return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer) });

                case 5:
                    return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer) });

                case 6:
                    return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer) });

                case 7:
                    return TupleImplHelper.CombineHashCodes(new int[] { comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), rest.GetHashCode(comparer) });
            }
            return -1;
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
            sb.Append(", ");
            sb.Append(this.m_Item7);
            sb.Append(", ");
            return ((ITupleImpl) this.m_Rest).ToString(sb);
        }

        int ITupleImpl.Size
        {
            get
            {
                return (7 + ((ITupleImpl)this.m_Rest).Size);
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

        public T4 Item4
        {
            get
            {
                return this.m_Item4;
            }
        }

        public T5 Item5
        {
            get
            {
                return this.m_Item5;
            }
        }

        public T6 Item6
        {
            get
            {
                return this.m_Item6;
            }
        }

        public T7 Item7
        {
            get
            {
                return this.m_Item7;
            }
        }

        public TRest Rest
        {
            get
            {
                return this.m_Rest;
            }
        }
    }
}

