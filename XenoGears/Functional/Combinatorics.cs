using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using System;

namespace XenoGears.Functional
{
    [DebuggerNonUserCode]
    public static class Combinatorics
    {
        public static IEnumerable<Tuple<T>> CartesianProduct<T>(IEnumerable<T> seq1)
        {
            return CartesianProductImpl(seq1.ToArray());
        }

        private static IEnumerable<Tuple<T>> CartesianProductImpl<T>(IEnumerable<T> seq1)
        {
            var dims = new []{seq1.Count()};
            foreach (var perm in dims.CartesianProduct())
            {
                yield return Tuple.New(
                    seq1.ElementAt(perm.ElementAt(0)));
            }
        }

        public static IEnumerable<Tuple<T1, T2>> CartesianProduct<T1, T2>(IEnumerable<T1> seq1, IEnumerable<T2> seq2)
        {
            return CartesianProductImpl(seq1.ToArray(), seq2.ToArray());
        }

        private static IEnumerable<Tuple<T1, T2>> CartesianProductImpl<T1, T2>(IEnumerable<T1> seq1, IEnumerable<T2> seq2)
        {
            var dims = new []{seq1.Count(), seq2.Count()};
            foreach (var perm in dims.CartesianProduct())
            {
                yield return Tuple.New(
                    seq1.ElementAt(perm.ElementAt(0)),
                    seq2.ElementAt(perm.ElementAt(1)));
            }
        }

        public static IEnumerable<Tuple<T1, T2, T3>> CartesianProduct<T1, T2, T3>(IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3)
        {
            return CartesianProductImpl(seq1.ToArray(), seq2.ToArray(), seq3.ToArray());
        }

        private static IEnumerable<Tuple<T1, T2, T3>> CartesianProductImpl<T1, T2, T3>(IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3)
        {
            var dims = new []{seq1.Count(), seq2.Count(), seq3.Count()};
            foreach (var perm in dims.CartesianProduct())
            {
                yield return Tuple.New(
                    seq1.ElementAt(perm.ElementAt(0)),
                    seq2.ElementAt(perm.ElementAt(1)),
                    seq3.ElementAt(perm.ElementAt(2)));
            }
        }

        public static IEnumerable<T[]> CartesianProduct<T>(IEnumerable<T>[] seqs)
        {
            return CartesianProductImpl((IEnumerable<T>[])seqs.Select(seq => seq.ToArray()).ToArray());
        }

        private static IEnumerable<T[]> CartesianProductImpl<T>(IEnumerable<T>[] seqs)
        {
            var dims = seqs.Select(seq => seq.Count()).ToArray();
            foreach (var perm in dims.CartesianProduct())
            {
                yield return seqs.Select((seq, i) => seq.ElementAt(perm.ElementAt(i))).ToArray();
            }
        }

        public static IEnumerable<int[]> CartesianProduct(this int[] dims)
        {
            (dims.Length > 0).AssertTrue();
            for (var i = 0; i < dims.Product(); ++i)
            {
                var digits = new List<int>();
                dims.Reverse().Aggregate(i, (curr, dim) =>
                {
                    digits.Add(curr % dim);
                    return curr / dim;
                });

                digits.Reverse();
                yield return digits.ToArray();
            }
        }
    }
}