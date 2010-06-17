using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static T SingleOrDefault2<T>(this IEnumerable<T> seq, Func<T, bool> filter)
        {
            return seq.Where(filter).SingleOrDefault2();
        }

        public static T SingleOrDefault2<T>(this IEnumerable<T> seq)
        {
            var nextIndex = 0;
            var lastElement = default(T);
            foreach (var element in seq)
            {
                if (nextIndex == 1)
                {
                    return default(T);
                }

                lastElement = element;
                ++nextIndex;
            }

            return nextIndex == 1 ? lastElement : default(T);
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.MaxOrDefault(default(T));
        }

        public static R MaxOrDefault<T, R>(this IEnumerable<T> seq, Func<T, R> selector)
        {
            return seq.Select(selector).MaxOrDefault();
        }

        public static T MinOrDefault<T>(this IEnumerable<T> seq)
        {
            return seq.MinOrDefault(default(T));
        }

        public static R MinOrDefault<T, R>(this IEnumerable<T> seq, Func<T, R> selector)
        {
            return seq.Select(selector).MinOrDefault();
        }
    }
}
