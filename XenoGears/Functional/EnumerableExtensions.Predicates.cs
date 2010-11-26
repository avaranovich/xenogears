using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static bool IsEmpty(this IEnumerable seq)
        {
            return seq.Cast<Object>().IsEmpty();
        }

        public static bool IsNullOrEmpty(this IEnumerable seq)
        {
            return seq == null || seq.Cast<Object>().IsEmpty();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> seq)
        {
            foreach (var element in seq)
            {
                return false;
            }

            return true;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> seq)
        {
            foreach (var element in seq ?? Enumerable.Empty<T>())
            {
                return false;
            }

            return true;
        }

        public static bool IsNotEmpty(this IEnumerable seq)
        {
            return !seq.IsEmpty();
        }

        public static bool IsNeitherNullNorEmpty(this IEnumerable seq)
        {
            return !seq.IsNullOrEmpty();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> seq)
        {
            return !seq.IsEmpty();
        }

        public static bool IsNeitherNullNorEmpty<T>(this IEnumerable<T> seq)
        {
            return !seq.IsNullOrEmpty();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            foreach (var element in seq.Where(filter))
            {
                return false;
            }

            return true;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            foreach (var element in (seq ?? Enumerable.Empty<T>()).Where(filter))
            {
                return false;
            }

            return true;
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return !seq.IsEmpty(filter);
        }

        public static bool IsNeitherNullNorEmpty<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return !seq.IsNullOrEmpty(filter);
        }

        public static bool IsEmpty(this StringBuilder buf)
        {
            return buf.Length == 0;
        }

        public static bool IsNullOrEmpty(this StringBuilder buf)
        {
            return buf == null || buf.Length == 0;
        }

        public static bool IsNotEmpty(this StringBuilder buf)
        {
            return !buf.IsEmpty();
        }

        public static bool IsNeitherNullNorEmpty(this StringBuilder buf)
        {
            return !buf.IsNullOrEmpty();
        }

        public static bool All(this IEnumerable<bool> seq)
        {
            return seq.All(b => b);
        }

        public static bool Any(this IEnumerable<bool> seq)
        {
            return seq.Any(b => b);
        }

        public static bool ExactlyOne(this IEnumerable<bool> seq)
        {
            return seq.Count(b => b) == 1;
        }

        public static bool ExactlyOne<T>(this IEnumerable<T> seq)
        {
            return seq.Count() == 1;
        }

        public static bool ExactlyOne<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Count(filter) == 1;
        }

        public static bool None(this IEnumerable<bool> seq)
        {
            return !seq.Any();
        }

        public static bool None<T>(this IEnumerable<T> seq)
        {
            return !seq.Any();
        }

        public static bool None<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return !seq.Any(filter);
        }

        public static bool AllDistinct<T>(this IEnumerable<T> seq)
        {
            return seq.Distinct().Count() == seq.Count();
        }

        public static bool AllDistinct<T>(this IEnumerable<T> seq, IEqualityComparer<T> comparer)
        {
            return seq.Distinct(comparer).Count() == seq.Count();
        }

        public static bool AllDistinct<T>(this IEnumerable<T> seq, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            return seq.Distinct(comparer, hasher).Count() == seq.Count();
        }

        public static bool AllDistinct<T, R>(this IEnumerable<T> seq, Func<T, R> selector)
        {
            return seq.Distinct(selector).Count() == seq.Count();
        }

        public static bool AllDistinct<T, R>(this IEnumerable<T> seq, Func<T, R> selector, IEqualityComparer<R> comparer)
        {
            return seq.Distinct(selector, comparer).Count() == seq.Count();
        }

        public static bool AllDistinct<T, R>(this IEnumerable<T> seq, Func<T, R> selector, Func<R, R, bool> comparer, Func<R, int> hasher)
        {
            return seq.Distinct(selector, comparer, hasher).Count() == seq.Count();
        }

        public static bool AnyDuplicate<T>(this IEnumerable<T> seq)
        {
            return !seq.AllDistinct();
        }

        public static bool AnyDuplicate<T>(this IEnumerable<T> seq, IEqualityComparer<T> comparer)
        {
            return !seq.AllDistinct(comparer);
        }

        public static bool AnyDuplicate<T>(this IEnumerable<T> seq, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            return seq.AnyDuplicate(comparer, hasher);
        }

        public static bool AnyDuplicate<T, R>(this IEnumerable<T> seq, Func<T, R> selector)
        {
            return !seq.AllDistinct(selector);
        }

        public static bool AnyDuplicate<T, R>(this IEnumerable<T> seq, Func<T, R> selector, IEqualityComparer<R> comparer)
        {
            return !seq.AllDistinct(selector, comparer);
        }

        public static bool AnyDuplicate<T, R>(this IEnumerable<T> seq, Func<T, R> selector, Func<R, R, bool> comparer, Func<R, int> hasher)
        {
            return !seq.AllDistinct(selector, comparer, hasher);
        }

        public static bool AllMatch<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, bool> predicate)
        {
            return AllMatch(seq1, seq2, (t1, t2, i) => predicate(t1, t2));
        }

        public static bool AllMatch<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, int, bool> predicate)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (next1 ^ next2)
                {
                    return false;
                }
                else if (!next1 && !next2)
                {
                    return true;
                }
                else
                {
                    if (!predicate(seq1e.Current, seq2e.Current, i++))
                    {
                        return false;
                    }
                }
            }
        }

        public static bool AnyMatch<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, bool> predicate)
        {
            return AnyMatch(seq1, seq2, (t1, t2, i) => predicate(t1, t2));
        }

        public static bool AnyMatch<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, int, bool> predicate)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (next1 ^ next2)
                {
                    return false;
                }
                else if (!next1 && !next2)
                {
                    return false;
                }
                else
                {
                    if (predicate(seq1e.Current, seq2e.Current, i++))
                    {
                        return true;
                    }
                }
            }
        }
    }
}