using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace XenoGears.Functional
{
    // todo. also overload SQO for tuples, i.e.:
    // R Select<T1, T2, R>(this IEnumerable<Tuple<T1, T2>> seqt, Func<T1, T2, R> selector);
    // loads of signatures so I didn't do this right nao

    // todo. more realistic idea: introduce overloads for KeyValuePairs
    // much more easy, won't fill intellisense with loadz of crap and much more useful for my codebase

    // todo. it'd be also a nice idea to add overloads that take indices to more SQO
    // (e.g. see Aggregate methods -> they all could use such overloads)
    // to be honest, not only SQO could benefit from indices (hint: folds, maybe smth else)

    public static partial class EnumerableExtensions
    {
        /* distincts with non-standard comparers */

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> seq, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            return seq.Distinct(new EqualityComparer<T>(comparer, hasher));
        }

        public static IEnumerable<T> Distinct<T, R>(this IEnumerable<T> seq, Func<T, R> selector)
        {
            return seq.Distinct(selector, EqualityComparer<R>.Default);
        }

        public static IEnumerable<T> Distinct<T, R>(this IEnumerable<T> seq, Func<T, R> selector, IEqualityComparer<R> comparer)
        {
            return seq.Distinct((el1, el2) => comparer.Equals(selector(el1), selector(el2)), el => comparer.GetHashCode(selector(el)));
        }

        public static IEnumerable<T> Distinct<T, R>(this IEnumerable<T> seq, Func<T, R> selector, Func<R, R, bool> comparer, Func<R, int> hasher)
        {
            return seq.Distinct(selector, new EqualityComparer<R>(comparer, hasher));
        }

        [DebuggerNonUserCode]
        private class EqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equalityComparer;
            private readonly Func<T, int> _hasher;

            public EqualityComparer(Func<T, T, bool> equalityComparer, Func<T, int> hasher)
            {
                _equalityComparer = equalityComparer;
                _hasher = hasher;
            }

            public static IEqualityComparer<T> Default
            {
                get { return System.Collections.Generic.EqualityComparer<T>.Default; }
            }

            public bool Equals(T x, T y)
            {
                return _equalityComparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _hasher(obj);
            }
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> seq, int num)
        {
            if (num == 0) return seq;
            return seq.Slice(0, -num);
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> seq, int num)
        {
            if (num == 0) return Seq.Empty<T>();
            return seq.Slice(-num);
        }

        public static int Width<T>(this T[] arr) { return arr.Length; }
        public static int Width<T>(this T[,] arr) { return arr.GetLength(1); }
        public static int Width<T>(this T[,,] arr) { return arr.GetLength(2); }
        public static int Height<T>(this T[,] arr) { return arr.GetLength(0); }
        public static int Height<T>(this T[,,] arr) { return arr.GetLength(1); }
        public static int Depth<T>(this T[,,] arr) { return arr.GetLength(0); }

        public static ReadOnlyCollection<int> Dims(this Array arr)
        {
            return 0.UpTo(arr.Rank - 1).Select(i => arr.GetLength(i)).ToReadOnly();
        }
    }
}