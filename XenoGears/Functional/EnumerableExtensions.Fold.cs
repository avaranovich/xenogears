using System;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Assertions;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static T Fold<T>(this IEnumerable<T> seq, Func<T, T, T> fold)
        {
            return seq.Fold((a, t, _) => fold(a, t));
        }

        public static T Fold<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Fold(fold, t => t);
        }

        public static R Fold<T, R>(this IEnumerable<T> seq, Func<T, T, T> fold, Func<T, R> map)
        {
            return seq.Fold((a, t, _) => fold(a, t), map);
        }

        public static R Fold<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, R> map)
        {
            seq.AssertNotEmpty();
            return map(seq.Skip(1).Fold(seq.First(), fold));
        }

        public static A Fold<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, A> fold)
        {
            return seq.Fold(seed, (a, t, _) => fold(a, t));
        }

        public static A Fold<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Fold(seed, fold, a => a);
        }

        public static R Fold<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, A> fold, Func<A, R> map)
        {
            return seq.Fold(seed, (a, t, _) => fold(a, t), map);
        }

        public static R Fold<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, R> map)
        {
            var acc = seed;
            seq.ForEach((el, i) => acc = fold(acc, el, i));
            return map(acc);
        }

        public static T Foldr<T>(this IEnumerable<T> seq, Func<T, T, T> fold)
        {
            return seq.Foldr((a, t, _) => fold(a, t));
        }

        public static T Foldr<T>(this IEnumerable<T> seq, Func<T, T, int, T> fold)
        {
            return seq.Foldr(fold, t => t);
        }

        public static R Foldr<T, R>(this IEnumerable<T> seq, Func<T, T, T> fold, Func<T, R> map)
        {
            return seq.Foldr((a, t, _) => fold(a, t), map);
        }

        public static R Foldr<T, R>(this IEnumerable<T> seq, Func<T, T, int, T> fold, Func<T, R> map)
        {
            seq.AssertNotEmpty();
            return map(seq.SkipLast(1).Foldr(seq.Last(), fold));
        }

        public static A Foldr<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, A> fold)
        {
            return seq.Foldr(seed, (a, t, _) => fold(a, t));
        }

        public static A Foldr<T, A>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold)
        {
            return seq.Foldr(seed, fold, a => a);
        }

        public static R Foldr<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, A> fold, Func<A, R> map)
        {
            return seq.Foldr(seed, (a, t, _) => fold(a, t), map);
        }

        public static R Foldr<T, A, R>(this IEnumerable<T> seq, A seed, Func<A, T, int, A> fold, Func<A, R> map)
        {
            var acc = seed;
            seq.Reverse().ForEach((el, i) => acc = fold(acc, el, i));
            return map(acc);
        }
    }
}