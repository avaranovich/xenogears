using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Exceptions;

namespace XenoGears.Functional.Memoize
{
    [DebuggerNonUserCode]
    public static class Func
    {
        public static Func<R> Memoize<R>(this Func<R> func)
        {
            var dummy = Tuple.Create(0);
            var impl = MemoizeImpl(_ => func());
            return () => impl(dummy);
        }

        public static Func<T1, R> Memoize<T1, R>(this Func<T1, R> func)
        {
            var impl = MemoizeImpl(t => func(((Tuple<T1>)t).Item1));
            return arg1 => impl(Tuple.Create(arg1));
        }

        public static Func<T1, T2, R> Memoize<T1, T2, R>(this Func<T1, T2, R> func)
        {
            var impl = MemoizeImpl(t => func(((Tuple<T1, T2>)t).Item1, ((Tuple<T1, T2>)t).Item2));
            return (arg1, arg2) => impl(Tuple.Create(arg1, arg2));
        }

        private static Func<Object, R> MemoizeImpl<R>(this Func<Object, R> normalized)
        {
            var cache = new Dictionary<Object, Tuple<R, Exception>>();

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

                    cache.Add(args, Tuple.Create(result, exception));
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
