using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        // this is introduced to provide non-ambiguous Concat sig for the operands
        public static IEnumerable<Object> Concat(this IEnumerable<Object> seq1, IEnumerable<Object> seq2)
        {
            return Enumerable.Concat(seq1, seq2);
        }

        public static IEnumerable<T> Concat<T>(this T element, IEnumerable<T> seq)
        {
            return element.MkArray().Concat(seq);
        }

        public static IEnumerable<T> Concat<T>(this T element1, T element2, IEnumerable<T> seq)
        {
            return new[] { element1, element2 }.Concat(seq);
        }

        public static IEnumerable<T> Concat<T>(this T element1, T element2, T element3, IEnumerable<T> seq)
        {
            return new[] { element1, element2, element3 }.Concat(seq);
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> seq, T element)
        {
            return seq.SequenceEqual(element.MkArray());
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> set, params T[] elements)
        {
            return set.Concat((IEnumerable<T>)elements);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> set, params IEnumerable<T>[] others)
        {
            return set.Concat((IEnumerable<IEnumerable<T>>)others);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> set, IEnumerable<ReadOnlyCollection<T>> others)
        {
            return set.Concat(others.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> set, IEnumerable<IEnumerable<T>> others)
        {
            return Enumerable.Concat(set.MkArray(), others).Concat();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<ReadOnlyCollection<T>> seqs)
        {
            return seqs.Cast<IEnumerable<T>>().Concat();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T[]> seqs)
        {
            return ((IEnumerable<IEnumerable<T>>)seqs).Concat();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> seqs)
        {
            if (seqs.IsNullOrEmpty()) return Seq.Empty<T>();
            return seqs.Fold((acc, set) => Enumerable.Concat(acc, set ?? Seq.Empty<T>()));
        }
    }
}