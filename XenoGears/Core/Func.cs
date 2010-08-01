using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Exceptions;
using XenoGears.Functional;

namespace XenoGears.Core
{
    [DebuggerNonUserCode]
    public static class Func
    {
        public static Func<R> Memoize<R>(this Func<R> func)
        {
            var dummy = Tuple.New(0);
            var impl = MemoizeImpl(_ => func());
            return () => impl(dummy);
        }

        public static Func<T1, R> Memoize<T1, R>(this Func<T1, R> func)
        {
            var impl = MemoizeImpl(t => func(t.Items.First().AssertCast<T1>()));
            return arg1 => impl(Tuple.New(arg1));
        }

        public static Func<T1, T2, R> Memoize<T1, T2, R>(this Func<T1, T2, R> func)
        {
            var impl = MemoizeImpl(t => func((T1)t.Items.First(), (T2)t.Items.Second()));
            return (arg1, arg2) => impl(Tuple.New(arg1, arg2));
        }

        private static Func<ITuple, R> MemoizeImpl<R>(this Func<ITuple, R> normalized)
        {
            var cache = new Dictionary<ITuple, Tuple<R, Exception>>();

            return args =>
            {
                if (!cache.ContainsKey(args))
                {
                    var result = default(R);
                    var exception = default(Exception);

                    try
                    {
                        result = normalized(args);
                    }
                    catch (Exception ex)
                    {
                        ex.PreserveStackTrace();
                        exception = ex;
                    }

                    cache.Add(args, Tuple.New(result, exception));
                }

                var history = cache[args];
                if (history.Item2 == null)
                {
                    return history.Item1;
                }
                else
                {
                    throw history.Item2;
                }
            };
        }
    }
}
