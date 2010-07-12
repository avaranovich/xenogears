using System;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Assertions;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Closure<T>(this IEnumerable<T> initial, IEnumerable<T> universum, Func<T, T, bool> relation)
        {
            initial.Except(universum).AssertEmpty();
            var closure = initial.ToList();

            var i = 0;
            while (i < closure.Count())
            {
                var el = closure[i++];

                var complement = universum.Except(closure).ToReadOnly();
                var added = complement.Where(elc => relation(el, elc)).ToReadOnly();
                closure.AddElements(added);
            }

            return closure;
        }

        public static IEnumerable<T> Closure<T>(this IEnumerable<T> initial, IEnumerable<T> universum, Func<T, T, T, bool> relation)
        {
            throw new NotImplementedException();
        }
    }
}
