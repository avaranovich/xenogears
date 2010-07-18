using System;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Assertions;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChopEvery<T>(this IEnumerable<T> seq, int sliceSize)
        {
            sliceSize.AssertThat(i => i > 0);
            return seq.ChopAfter((_, index) => (index + 1) % sliceSize == 0);
        }

        public static IEnumerable<IEnumerable<T>> ChopBefore<T>(this IEnumerable<T> seq, Func<T, bool> chopBeforeElementIf)
        {
            return seq.ChopBefore((el, _) => chopBeforeElementIf(el));
        }

        public static IEnumerable<IEnumerable<T>> ChopBefore<T>(this IEnumerable<T> seq, Func<T, int, bool> chopBeforeElementIf)
        {
            return seq.ChopBefore(seq, chopBeforeElementIf);
        }

        public static IEnumerable<IEnumerable<T>> ChopAfter<T>(this IEnumerable<T> seq, Func<T, bool> chopAfterElementIf)
        {
            return seq.ChopAfter((el, _) => chopAfterElementIf(el));
        }

        public static IEnumerable<IEnumerable<T>> ChopAfter<T>(this IEnumerable<T> seq, Func<T, int, bool> chopAfterElementIf)
        {
            return seq.ChopAfter(seq, chopAfterElementIf);
        }

        public static IEnumerable<IEnumerable<T>> ChopBetween<T>(this IEnumerable<T> seq, Func<T, T, bool> chopBetweenElementsIf)
        {
            return seq.ChopBetween((prev, curr, _) => chopBetweenElementsIf(prev, curr));
        }

        public static IEnumerable<IEnumerable<T>> ChopBetween<T>(this IEnumerable<T> seq, Func<T, T, int, bool> chopBetweenElementsIf)
        {
            return seq.ChopBefore(seq.SlideOuter2().SkipLast(1), (pair, i) =>
                i == 0 ? false : chopBetweenElementsIf(pair.Item1, pair.Item2, i));
        }

        public static IEnumerable<IEnumerable<T>> ChopBefore<T, S>(this IEnumerable<T> seq, IEnumerable<S> scan, Func<S, bool> chopBeforeElementIf)
        {
            return seq.ChopBefore(scan, (s, _) => chopBeforeElementIf(s));
        }

        public static IEnumerable<IEnumerable<T>> ChopAfter<T, S>(this IEnumerable<T> seq, IEnumerable<S> scan, Func<S, bool> chopAfterElementIf)
        {
            return seq.ChopAfter(scan, (s, _) => chopAfterElementIf(s));
        }

        public static IEnumerable<IEnumerable<T>> ChopBefore<T, S>(this IEnumerable<T> seq, IEnumerable<S> scan, Func<S, int, bool> chopBeforeElementIf)
        {
            return seq.Zip(scan).Scanbi(
                Enumerable.Empty<T>(),
                (a, ts, i) => chopBeforeElementIf(ts.Item2, i) ? ts.Item1.MkArray(): a.Concat(ts.Item1),
                (a, ts, i) => chopBeforeElementIf(ts.Item2, i) ? a : null,
                (a, _) => a.IsNotEmpty() ? a : null
            ).Where(slice => slice != null);
        }

        public static IEnumerable<IEnumerable<T>> ChopAfter<T, S>(this IEnumerable<T> seq, IEnumerable<S> scan, Func<S, int, bool> chopAfterElementIf)
        {
            return seq.Zip(scan).Scanbi(
                Enumerable.Empty<T>(),
                (a, ts, i) => chopAfterElementIf(ts.Item2, i) ? Enumerable.Empty<T>() : a.Concat(ts.Item1),
                (a, ts, i) => chopAfterElementIf(ts.Item2, i) ? a.Concat(ts.Item1) : null,
                (a, _) => a
            ).Where(slice => slice != null);
        }
    }
}
