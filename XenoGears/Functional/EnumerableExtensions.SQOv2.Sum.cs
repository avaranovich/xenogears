using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).Sum();
        }

        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).Sum();
        }

        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).Sum();
        }

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).Sum();
        }

        public static uint Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, uint> selector)
        {
            throw new NotImplementedException();
        }

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).Sum();
        }

        public static ulong Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, ulong> selector)
        {
            throw new NotImplementedException();
        }
    }
}
