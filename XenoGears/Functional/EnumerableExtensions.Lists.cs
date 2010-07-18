using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    // todo. think about multithreading and atomicity

    public static partial class EnumerableExtensions
    {
        public static ICollection<T> AddElements<T>(this ICollection<T> mutable, params T[] addendum)
        {
            return mutable.AddElements((IEnumerable<T>)addendum);
        }

        public static ICollection<T> AddElements<T>(this ICollection<T> mutable, IEnumerable<T> addendum)
        {
            if (addendum != null) addendum.ForEach(mutable.Add);
            return mutable;
        }

        public static IList<T> RemoveLast<T>(this IList<T> mutable)
        {
            if (mutable.IsNotEmpty()) mutable.RemoveAt(mutable.Count() - 1);
            return mutable;
        }

        public static ICollection<T> RemoveElements<T>(this ICollection<T> mutable)
        {
            mutable.Clear();
            return mutable;
        }

        public static ICollection<T> RemoveElements<T>(this ICollection<T> mutable, params T[] seq)
        {
            return mutable.RemoveElements((IEnumerable<T>)seq);
        }

        public static ICollection<T> RemoveElements<T>(this ICollection<T> mutable, IEnumerable<T> seq)
        {
            if (seq != null) seq.ForEach(el => mutable.Remove(el));
            return mutable;
        }

        public static ICollection<T> RemoveElements<T>(this ICollection<T> mutable, Func<T, bool> filter)
        {
            if (filter != null) mutable.Where(filter).ForEach(el => mutable.Remove(el));
            return mutable;
        }

        public static ICollection<T> SetElements<T>(this ICollection<T> mutable, params T[] seq)
        {
            return mutable.SetElements((IEnumerable<T>)seq);
        }

        public static ICollection<T> SetElements<T>(this ICollection<T> mutable, IEnumerable<T> seq)
        {
            seq = seq.ToArray();
            mutable.Clear();
            mutable.AddElements(seq);
            return mutable;
        }

        public static ICollection<T> ReplaceElements<T>(this IList<T> mutable, T find, T replace)
        {
            return mutable.ReplaceElements(el => EqualityComparer<T>.Default.Equals(el, find), el => replace);
        }

        public static ICollection<T> ReplaceElements<T>(this IList<T> mutable, Func<T, bool> filter, Func<T, T> map)
        {
            0.UpTo(mutable.Count() - 1).ForEach(i =>
            {
                if (filter(mutable[i]))
                {
                    mutable[i] = map(mutable[i]);
                }
            });

            return mutable;
        }
    }
}