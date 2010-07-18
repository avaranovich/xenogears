using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        // todo. I'd love slice to cache seq if it encounters negative indices

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> seq, int fromInclusive)
        {
            if (fromInclusive < 0) fromInclusive += seq.Count();
            return seq.Skip(fromInclusive);
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> seq, int fromInclusive, int toExclusive)
        {
            if (fromInclusive < 0) fromInclusive += seq.Count();
            if (toExclusive < 0) toExclusive += seq.Count();
            if (fromInclusive >= toExclusive) return Enumerable.Empty<T>();
            return seq.Skip(fromInclusive).Take(toExclusive - fromInclusive);
        }
    }
}