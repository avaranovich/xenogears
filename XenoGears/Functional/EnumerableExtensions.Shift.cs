using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> seq, int i)
        {
            return seq.Skip(i).Concat(seq.Take(i));
        }

        public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> seq, int i)
        {
            return seq.Skip(seq.Count() - i).Concat(seq.Take(seq.Count() - i));
        }

        public static IEnumerable<T> ShiftLeft<T>(this IEnumerable<T> seq, int i)
        {
            return seq.ShiftLeft(i, default(T));
        }

        public static IEnumerable<T> ShiftLeft<T>(this IEnumerable<T> seq, int i, T filler)
        {
            return seq.ShiftLeft(i, () => filler);
        }

        public static IEnumerable<T> ShiftLeft<T>(this IEnumerable<T> seq, int i, Func<T> filler)
        {
            return seq.ShiftLeft(i, _ => filler());
        }

        public static IEnumerable<T> ShiftLeft<T>(this IEnumerable<T> seq, int i, Func<int, T> filler)
        {
            return seq.Skip(i).Concat(i.Times(filler));
        }

        public static IEnumerable<T> ShiftRight<T>(this IEnumerable<T> seq, int i)
        {
            return seq.ShiftRight(i, default(T));
        }

        public static IEnumerable<T> ShiftRight<T>(this IEnumerable<T> seq, int i, T filler)
        {
            return seq.ShiftRight(i, () => filler);
        }

        public static IEnumerable<T> ShiftRight<T>(this IEnumerable<T> seq, int i, Func<T> filler)
        {
            return seq.ShiftRight(i, _ => filler());
        }

        public static IEnumerable<T> ShiftRight<T>(this IEnumerable<T> seq, int i, Func<int, T> filler)
        {
            return ((seq.Count() - i).Times(filler)).Concat(seq.Take(seq.Count() - i));
        }
    }
}
