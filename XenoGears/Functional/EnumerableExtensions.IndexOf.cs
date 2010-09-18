using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    // todo. optimizations when seq is ICollection or IList
    // btw this also regards all other places where we can take benefit of seq being a collection

    public static partial class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> seq, T toFind)
        {
            return seq.IndexOf(el => EqualityComparer<T>.Default.Equals(el, toFind));
        }

        public static int IndexOf<T>(this IEnumerable<T> seq, Func<T, bool> predicate)
        {
            var indices = seq.IndicesOf(predicate);
            return indices.IsEmpty() ? -1 : indices.First();
        }

        public static int LastIndexOf<T>(this IEnumerable<T> seq, T toFind)
        {
            return seq.LastIndexOf(el => EqualityComparer<T>.Default.Equals(el, toFind));
        }

        public static int LastIndexOf<T>(this IEnumerable<T> seq, Func<T, bool> predicate)
        {
            var indices = seq.IndicesOf(predicate);
            return indices.IsEmpty() ? -1 : indices.Last();
        }

        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> seq, T toFind)
        {
            return seq.IndicesOf(el => EqualityComparer<T>.Default.Equals(el, toFind));
        }

        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> seq, Func<T, bool> predicate)
        {
            return seq.Select((el, i) => Tuple.Create(predicate(el), i)).Where(t => t.Item1).Select(t => t.Item2);
        }
    }
}
