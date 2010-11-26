using System;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Assertions;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static T SingleOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.SingleOrDefault(() => @default);
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            var count = 0;
            var ret_ok = false;
            var ret = default(T);
            foreach (var el in seq)
            {
                var curr = count++;
                if (curr == 0)
                {
                    ret_ok = true;
                    ret = el;
                }
                else
                {
                    throw AssertionHelper.Fail();
                }
            }

            return ret_ok ? ret : @default();
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter, T @default)
        {
            return seq.Where(filter).SingleOrDefault(@default);
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter, Func<T> @default)
        {
            return seq.Where(filter).SingleOrDefault(@default);
        }

        public static T SingleOrDefault2<T>(this IEnumerable<T> seq, Func<T, bool> filter, T @default)
        {
            return seq.Where(filter).SingleOrDefault2(@default);
        }

        public static T SingleOrDefault2<T>(this IEnumerable<T> seq, Func<T, bool> filter, Func<T> @default)
        {
            return seq.Where(filter).SingleOrDefault2(@default);
        }

        public static T SingleOrDefault2<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.SingleOrDefault2(() => @default);
        }

        public static T SingleOrDefault2<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            var count = 0;
            var ret_ok = false;
            var ret = default(T);
            foreach (var el in seq)
            {
                var curr = count++;
                if (curr == 0)
                {
                    ret_ok = true;
                    ret = el;
                }
                else
                {
                    ret_ok = false;
                }
            }

            return ret_ok ? ret : @default();
        }

        public static T ElementAtOrDefault<T>(this IEnumerable<T> seq, int i, T @default)
        {
            return seq.ElementAtOrDefault(i, () => @default);
        }

        public static T ElementAtOrDefault<T>(this IEnumerable<T> seq, int i, Func<T> @default)
        {
            var curr = 0;
            foreach (var el in seq)
            {
                if (curr++ == i) return el;
            }

            return @default();
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.ElementAtOrDefault(0, @default);
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.ElementAtOrDefault(0, @default);
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter, T @default)
        {
            return seq.Where(filter).ElementAtOrDefault(0, @default);
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter, Func<T> @default)
        {
            return seq.Where(filter).ElementAtOrDefault(0, @default);
        }

        public static T LastOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.Reverse().FirstOrDefault(@default);
        }

        public static T LastOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.Reverse().FirstOrDefault(@default);
        }

        public static T LastOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter, T @default)
        {
            return seq.Reverse().FirstOrDefault(filter, @default);
        }

        public static T LastOrDefault<T>(this IEnumerable<T> seq, Func<T, bool> filter, Func<T> @default)
        {
            return seq.Reverse().FirstOrDefault(filter, @default);
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.IsEmpty() ? @default : seq.Max();
        }

        public static R MaxOrDefault<T, R>(this IEnumerable<T> seq, Func<T, R> selector, T @default)
        {
            return seq.Select(selector).MaxOrDefault(selector(@default));
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.IsEmpty() ? @default() : seq.Max();
        }

        public static R MaxOrDefault<T, R>(this IEnumerable<T> seq, Func<T, R> selector, Func<T> @default)
        {
            return seq.Select(selector).MaxOrDefault(() => selector(@default()));
        }

        public static T MinOrDefault<T>(this IEnumerable<T> seq, T @default)
        {
            return seq.IsEmpty() ? @default : seq.Min();
        }

        public static R MinOrDefault<T, R>(this IEnumerable<T> seq, Func<T, R> selector, T @default)
        {
            return seq.Select(selector).MinOrDefault(selector(@default));
        }

        public static T MinOrDefault<T>(this IEnumerable<T> seq, Func<T> @default)
        {
            return seq.IsEmpty() ? @default() : seq.Min();
        }

        public static R MinOrDefault<T, R>(this IEnumerable<T> seq, Func<T, R> selector, Func<T> @default)
        {
            return seq.Select(selector).MinOrDefault(() => selector(@default()));
        }
    }
}
