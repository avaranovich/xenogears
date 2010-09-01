using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Functional;
using System.Linq;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Assertions;

namespace System
{
    [DebuggerNonUserCode]
    public static class Tuple
    {
        public static ITuple New(params Object[] items)
        {
            return New(items, typeof(Object).Repeat(items.Count()).ToArray());
        }

        public static ITuple New(Object[] items, Type[] types)
        {
            Func<IEnumerable<Tuple<Object, Type>>, ITuple> mkTuple = itemSpecs =>
            {
                var factories = typeof(Tuple).GetMethods(BF.PublicStatic).Where(m => m.IsGenericMethod);
                var factory = factories.Single(m => m.GetParameters().Count() == itemSpecs.Count());
                factory = factory.XMakeGenericMethod(itemSpecs.Unzip().Item2);
                return factory.Invoke(null, itemSpecs.Unzip().Item1.ToArray()).AssertCast<ITuple>();
            };

            // damn... that's why we need type inference to use functional style
            // upd. with introduction of "types" parameter it only became worse...
            return items.Zip(types).ChopEvery(7).Foldr(default(ITuple), (tuple, slice) =>
                mkTuple(tuple == null ? slice : slice.Concat(Tuple.New<Object, Type>(tuple, tuple.GetType()))));
        }

        public static Tuple<T1> New<T1>(T1 item1)
        {
            return new Tuple<T1>(item1);
        }

        public static Tuple<T1, T2> New<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }

        public static Tuple<T1, T2, T3> New<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }

        public static Tuple<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        public static Tuple<T1, T2, T3, T4, T5> New<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6> New<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7> New<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> New<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
            where TRest : ITuple
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, rest);
        }
    }
}