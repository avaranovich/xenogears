using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        // orders without "by"

        public static IEnumerable<T> Order<T>(this IEnumerable<T> seq)
        {
            return seq.OrderBy(el => el);
        }

        public static IEnumerable<T> Order<T>(this IEnumerable<T> seq, IComparer<T> comparer)
        {
            return seq.OrderBy(el => el, comparer);
        }

        public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> seq)
        {
            return seq.OrderByDescending(el => el);
        }

        public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> seq, IComparer<T> comparer)
        {
            return seq.OrderByDescending(el => el, comparer);
        }

        public static IEnumerable<T> Then<T>(this IOrderedEnumerable<T> seq)
        {
            return seq.ThenBy(el => el);
        }

        public static IEnumerable<T> Then<T>(this IOrderedEnumerable<T> seq, IComparer<T> comparer)
        {
            return seq.ThenBy(el => el, comparer);
        }

        public static IEnumerable<T> ThenDescending<T>(this IOrderedEnumerable<T> seq)
        {
            return seq.ThenByDescending(el => el);
        }

        public static IEnumerable<T> ThenDescending<T>(this IOrderedEnumerable<T> seq, IComparer<T> comparer)
        {
            return seq.ThenByDescending(el => el, comparer);
        }

        // orders without "by" - with lambda comparers

        public static IEnumerable<T> Order<T>(this IEnumerable<T> seq, Func<T, T, int> comparer)
        {
            return seq.OrderBy(el => el, new Comparer<T>(comparer));
        }

        public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> seq, Func<T, T, int> comparer)
        {
            return seq.OrderByDescending(el => el, new Comparer<T>(comparer));
        }

        public static IEnumerable<T> Then<T>(this IOrderedEnumerable<T> seq, Func<T, T, int> comparer)
        {
            return seq.ThenBy(el => el, new Comparer<T>(comparer));
        }

        public static IEnumerable<T> ThenDescending<T>(this IOrderedEnumerable<T> seq, Func<T, T, int> comparer)
        {
            return seq.ThenByDescending(el => el, new Comparer<T>(comparer));
        }

        [DebuggerNonUserCode]
        private class Comparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _comparer;
            public Comparer(Func<T, T, int> comparer)
            {
                _comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return _comparer(x, y);
            }
        }

        // enumerable's orders with lambda comparers

        public static IOrderedEnumerable<T> OrderBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector, Func<K, K, int> comparer)
        {
            return source.OrderBy(keySelector, new Comparer<K>(comparer));
        }

        public static IOrderedEnumerable<T> OrderByDescending<T, K>(this IEnumerable<T> source, Func<T, K> keySelector, Func<K, K, int> comparer)
        {
            return source.OrderByDescending(keySelector, new Comparer<K>(comparer));
        }

        public static IOrderedEnumerable<T> ThenBy<T, K>(this IOrderedEnumerable<T> source, Func<T, K> keySelector, Func<K, K, int> comparer)
        {
            return source.ThenBy(keySelector, new Comparer<K>(comparer));
        }

        public static IOrderedEnumerable<T> ThenByDescending<T, K>(this IOrderedEnumerable<T> source, Func<T, K> keySelector, Func<K, K, int> comparer)
        {
            return source.ThenByDescending(keySelector, new Comparer<K>(comparer));
        }
    }
}
