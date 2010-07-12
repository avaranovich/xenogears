using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using XenoGears.Assertions;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Functional;

namespace System
{
    [DebuggerNonUserCode]
    public static class MutableTuple
    {
        public static IMutableTuple New(params Object[] items)
        {
            return New(items, typeof(Object).Repeat(items.Count()).ToArray());
        }

        public static IMutableTuple New(Object[] items, Type[] types)
        {
            Func<IEnumerable<Tuple<Object, Type>>, IMutableTuple> mkMutableTuple = itemSpecs =>
            {
                var factories = typeof(MutableTuple).GetMethods(BF.PublicStatic).Where(m => m.IsGenericMethod);
                var factory = factories.Single(m => m.GetParameters().Count() == itemSpecs.Count());
                factory = factory.XMakeGenericMethod(itemSpecs.Unzip().Item2);
                return factory.Invoke(null, itemSpecs.Unzip().Item1.ToArray()).AssertCast<IMutableTuple>();
            };

            // damn... that's why we need type inference to use functional style
            // upd. with introduction of "types" parameter it only became worse...
            return items.Zip(types).ChopEvery(7).Foldr(default(IMutableTuple), (tuple, slice) =>
                mkMutableTuple(tuple == null ? slice : slice.Concat(Tuple.New<Object, Type>(tuple, tuple.GetType()))));
        }

        public static MutableTuple<T1> New<T1>(T1 item1)
        {
            return new MutableTuple<T1>(item1);
        }

        public static MutableTuple<T1, T2> New<T1, T2>(T1 item1, T2 item2)
        {
            return new MutableTuple<T1, T2>(item1, item2);
        }

        public static MutableTuple<T1, T2, T3> New<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new MutableTuple<T1, T2, T3>(item1, item2, item3);
        }

        public static MutableTuple<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new MutableTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        public static MutableTuple<T1, T2, T3, T4, T5> New<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new MutableTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }

        public static MutableTuple<T1, T2, T3, T4, T5, T6> New<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new MutableTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }

        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7> New<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }

        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> New<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
            where TRest : IMutableTuple
        {
            return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, rest);
        }
    }

}
