using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XenoGears.Functional;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static bool SetEqual<T>(this IEnumerable<T> set1, IEnumerable<T> set2)
        {
            return set1.Contains(set2) && set2.Contains(set1);
        }

        public static bool SetEqual<T>(this IEnumerable<T> set, T element)
        {
            return set.SetEqual(element.MkArray());
        }

        public static bool Contains<T>(this IEnumerable<T> set1, IEnumerable<T> set2)
        {
            return set2.All(el => set1.Contains(el));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> set, params T[] elements)
        {
            return set.Except((IEnumerable<T>)elements);
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> set, params IEnumerable<T>[] others)
        {
            return set.Except((IEnumerable<IEnumerable<T>>)others);
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> set, IEnumerable<ReadOnlyCollection<T>> others)
        {
            return set.Except(others.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> set, IEnumerable<IEnumerable<T>> others)
        {
            return others.Fold(set, (curr, other) => Enumerable.Except(curr, other));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, params T[] elements)
        {
            return set.Intersect((IEnumerable<T>)elements);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, params IEnumerable<T>[] others)
        {
            return set.Intersect((IEnumerable<IEnumerable<T>>)others);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, IEnumerable<ReadOnlyCollection<T>> others)
        {
            return set.Intersect(others.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, IEnumerable<IEnumerable<T>> others)
        {
            return set.MkArray().Concat(others).Intersect();
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<ReadOnlyCollection<T>> seqs)
        {
            return seqs.Intersect(EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T[]> seqs)
        {
            return seqs.Intersect(EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> seqs)
        {
            return seqs.Intersect(EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, params T[] elements)
        {
            return set.Intersect(comparer, (IEnumerable<T>)elements);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, params IEnumerable<T>[] others)
        {
            return set.Intersect(comparer, (IEnumerable<IEnumerable<T>>)others);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, IEnumerable<ReadOnlyCollection<T>> others)
        {
            return set.Intersect(comparer, others.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, IEnumerable<IEnumerable<T>> others)
        {
            return set.Concat(others).Intersect();
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<ReadOnlyCollection<T>> seqs, IEqualityComparer<T> comparer)
        {
            return seqs.Cast<IEnumerable<T>>().Intersect(comparer);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T[]> seqs, IEqualityComparer<T> comparer)
        {
            return ((IEnumerable<IEnumerable<T>>)seqs).Intersect(comparer);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> seqs, IEqualityComparer<T> comparer)
        {
            if (seqs.IsNullOrEmpty()) return Seq.Empty<T>();
            return seqs.Fold((acc, set) => acc.Intersect(set ?? Seq.Empty<T>(), comparer));
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, params T[] elements)
        {
            return set.Union((IEnumerable<T>)elements);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, params IEnumerable<T>[] others)
        {
            return set.Union((IEnumerable<IEnumerable<T>>)others);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, IEnumerable<ReadOnlyCollection<T>> others)
        {
            return set.Union(others.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, IEnumerable<IEnumerable<T>> others)
        {
            return set.MkArray().Concat(others).Union();
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<ReadOnlyCollection<T>> seqs)
        {
            return seqs.Union(EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T[]> seqs)
        {
            return seqs.Union(EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> set)
        {
            return set.Union(EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, params T[] elements)
        {
            return set.Union(comparer, (IEnumerable<T>)elements);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, params IEnumerable<T>[] others)
        {
            return set.Union(comparer, (IEnumerable<IEnumerable<T>>)others);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, IEnumerable<ReadOnlyCollection<T>> others)
        {
            return set.Union(comparer, others.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> set, IEqualityComparer<T> comparer, IEnumerable<IEnumerable<T>> others)
        {
            return set.MkArray().Concat(others).Union();
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<ReadOnlyCollection<T>> seqs, IEqualityComparer<T> comparer)
        {
            return seqs.Cast<IEnumerable<T>>().Union(comparer);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T[]> seqs, IEqualityComparer<T> comparer)
        {
            return ((IEnumerable<IEnumerable<T>>)seqs).Union(comparer);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> seqs, IEqualityComparer<T> comparer)
        {
            if (seqs.IsNullOrEmpty()) return Seq.Empty<T>();
            return seqs.Fold((acc, set) => acc.Union(set ?? Seq.Empty<T>(), comparer));
        }
    }
}
