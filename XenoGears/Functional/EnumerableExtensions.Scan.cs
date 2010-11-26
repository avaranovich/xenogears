using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Scanai<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanai(fold, (a, _1, _2) => a, a => a);
        }

        public static IEnumerable<A> Scanai<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanai(seed, fold, (a, _1, _2) => a, a => a);
        }

        public static IEnumerable<R> Scanai<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map, Func<T, R> mapFirst)
        {
            return seq.Skip(1).Scanai(seq.First(), fold, map, mapFirst);
        }

        public static IEnumerable<R> Scanai<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map, Func<A, R> mapFirst)
        {
            yield return mapFirst(seed);

            var acc = seed;
            var index = 0;

            foreach (var el in seq)
            {
                acc = fold(acc, el, index);
                yield return map(acc, el, index);
                index++;
            }
        }

        public static IEnumerable<T> Scanae<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanae(fold, (T a, T _1, int _2) => a);
        }

        public static IEnumerable<A> Scanae<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanae(seed, fold, (a, _1, _2) => a);
        }

        public static IEnumerable<R> Scanae<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map)
        {
            return seq.Skip(1).Scanae(seq.First(), fold, map);
        }

        public static IEnumerable<R> Scanae<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map)
        {
            var acc = seed;
            var index = 0;

            foreach (var el in seq)
            {
                acc = fold(acc, el, index);
                yield return map(acc, el, index);
                index++;
            }
        }

        public static IEnumerable<T> Scanbi<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanbi(fold, (a, _1, _2) => a, (a, _) => a);
        }

        public static IEnumerable<A> Scanbi<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanbi(seed, fold, (a, _1, _2) => a, (a, _) => a);
        }

        public static IEnumerable<R> Scanbi<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map, Func<T, int, R> mapLast)
        {
            return seq.Skip(1).Scanbi(seq.First(), fold, map, mapLast);
        }

        public static IEnumerable<R> Scanbi<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map, Func<A, int, R> mapLast)
        {
            var acc = seed;
            var index = 0;

            foreach (var el in seq)
            {
                yield return map(acc, el, index);
                acc = fold(acc, el, index);
                ++index;
            }

            yield return mapLast(acc, index);
        }

        public static IEnumerable<T> Scanbe<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanbe(fold, (T a, T _1, int _2) => a);
        }

        public static IEnumerable<A> Scanbe<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanbe(seed, fold, (a, _1, _2) => a);
        }

        public static IEnumerable<R> Scanbe<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map)
        {
            return seq.Skip(1).Scanbe(seq.First(), fold, map);
        }

        public static IEnumerable<R> Scanbe<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map)
        {
            var acc = seed;
            var index = 0;

            foreach (var el in seq)
            {
                yield return map(acc, el, index);
                acc = fold(acc, el, index);
                ++index;
            }
        }

        public static IEnumerable<T> Scanrai<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanrai(fold, (a, _1, _2) => a, a => a);
        }

        public static IEnumerable<A> Scanrai<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanrai(seed, fold, (a, _1, _2) => a, a => a);
        }

        public static IEnumerable<R> Scanrai<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map, Func<T, R> mapFirst)
        {
            return seq.SkipLast(1).Scanrai(seq.Last(), fold, map, mapFirst);
        }

        public static IEnumerable<R> Scanrai<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map, Func<A, R> mapFirst)
        {
            yield return mapFirst(seed);

            var acc = seed;
            var index = 0;

            foreach (var el in seq.Reverse())
            {
                acc = fold(acc, el, index);
                yield return map(acc, el, index);
                ++index;
            }
        }


        public static IEnumerable<T> Scanrae<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanrae(fold, (T a, T _1, int _2) => a);
        }

        public static IEnumerable<A> Scanrae<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanrae(seed, fold, (a, _1, _2) => a);
        }

        public static IEnumerable<R> Scanrae<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map)
        {
            return seq.SkipLast(1).Scanrae(seq.Last(), fold, map);
        }

        public static IEnumerable<R> Scanrae<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map)
        {
            var acc = seed;
            var index = 0;

            foreach (var el in seq.Reverse())
            {
                acc = fold(acc, el, index);
                yield return map(acc, el, index);
                ++index;
            }
        }

        public static IEnumerable<T> Scanrbi<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanrbi(fold, (a, _1, _2) => a, (a, _) => a);
        }

        public static IEnumerable<A> Scanrbi<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanrbi(seed, fold, (a, _1, _2) => a, (a, _) => a);
        }

        public static IEnumerable<R> Scanrbi<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map, Func<T, int, R> mapLast)
        {
            return seq.SkipLast(1).Scanrbi(seq.Last(), fold, map, mapLast);
        }

        public static IEnumerable<R> Scanrbi<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map, Func<A, int, R> mapLast)
        {
            var acc = seed;
            var index = 0;

            foreach (var el in seq.Reverse())
            {
                yield return map(acc, el, index);
                acc = fold(acc, el, index);
                ++index;
            }

            yield return mapLast(acc, index);
        }

        public static IEnumerable<T> Scanrbe<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Scanrbe(fold, (T a, T _1, int _2) => a);
        }

        public static IEnumerable<A> Scanrbe<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Scanrbe(seed, fold, (a, _1, _2) => a);
        }

        public static IEnumerable<R> Scanrbe<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, T, int, R> map)
        {
            return seq.SkipLast(1).Scanrbe(seq.Last(), fold, map);
        }

        public static IEnumerable<R> Scanrbe<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, T, int, R> map)
        {
            var acc = seed;
            var index = 0;

            foreach (var el in seq.Reverse())
            {
                yield return map(acc, el, index);
                acc = fold(acc, el, index);
                ++index;
            }
        }    
    }
}