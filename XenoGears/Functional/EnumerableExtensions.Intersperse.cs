using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2)
        {
            return Zip(seq1, seq2).SelectMany(pair => new []{pair.Item1, pair.Item2});
        }
    }
}
