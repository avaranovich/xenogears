using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        // todo. I'd love the indexer to cache seq if it encounters negative indices

        public static T Nth<T>(this IEnumerable<T> seq, int i)
        {
            if (i < 0) i += seq.Count();
            return seq.ElementAt(i);
        }

        public static T NthOrDefault<T>(this IEnumerable<T> seq, int i)
        {
            return seq.NthOrDefault(i, default(T));
        }

        public static T NthOrDefault<T>(this IEnumerable<T> seq, int i, T @default)
        {
            return seq.NthOrDefault(i, () => @default);
        }

        public static T NthOrDefault<T>(this IEnumerable<T> seq, int i, Func<T> @default)
        {
            if (i < 0) i += seq.Count();
            return seq.ElementAtOrDefault(i, @default);
        }

        public static T Second<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(1);
        }

        public static T SecondOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(1);
        }

        public static T SecondOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(1, @default);
        }

        public static T SecondOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(1, @default);
        }

        public static T Third<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(2);
        }

        public static T ThirdOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(2);
        }

        public static T ThirdOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(2, @default);
        }

        public static T ThirdOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(2, @default);
        }

        public static T Fourth<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(3);
        }

        public static T FourthOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(3);
        }

        public static T FourthOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(3, @default);
        }

        public static T FourthOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(3, @default);
        }

        public static T Fifth<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(4);
        }

        public static T FifthOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(4);
        }

        public static T FifthOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(4, @default);
        }

        public static T FifthOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(4, @default);
        }

        public static T SecondLast<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(-2);
        }

        public static T SecondLastOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(-2);
        }

        public static T SecondLastOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(-2, @default);
        }

        public static T SecondLastOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(-2, @default);
        }

        public static T ThirdLast<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(-3);
        }

        public static T ThirdLastOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(-3);
        }

        public static T ThirdLastOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(-3, @default);
        }

        public static T ThirdLastOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(-3, @default);
        }

        public static T FourthLast<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(-4);
        }

        public static T FourthLastOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(-4);
        }

        public static T FourthLastOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(-4, @default);
        }

        public static T FourthLastOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(-4, @default);
        }

        public static T FifthLast<T>(this IEnumerable<T> seq)
        {
            return seq.Nth(-5);
        }

        public static T FifthLastOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.NthOrDefault(-5);
        }

        public static T FifthLastOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.NthOrDefault(-5, @default);
        }

        public static T FifthLastOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.NthOrDefault(-5, @default);
        }
    }
}
