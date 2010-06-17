using System;
using System.Collections.Generic;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<Tuple<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2)
        {
            return Zip(seq1, seq2, (t1, t2) => Tuple.New(t1, t2));
        }

        public static Tuple<IEnumerable<T1>, IEnumerable<T2>> Unzip<T1, T2>(this IEnumerable<Tuple<T1, T2>> seq)
        {
            return Tuple.New<IEnumerable<T1>, IEnumerable<T2>>(seq.Select(t => t.Item1).ToArray(), seq.Select(t => t.Item2).ToArray());
        }

        public static Tuple<IEnumerable<T1>, IEnumerable<T2>> Unzip<T1, T2>(this IEnumerable<MutableTuple<T1, T2>> seq)
        {
            return Tuple.New<IEnumerable<T1>, IEnumerable<T2>>(seq.Select(t => t.Item1).ToArray(), seq.Select(t => t.Item2).ToArray());
        }

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, R> zip)
        {
            return Zip(seq1, seq2, (t1, t2, i) => zip(t1, t2));
        }

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<Tuple<T1, T2>> seq, Func<T1, T2, R> zip)
        {
            return Zip(seq, (t1, t2, i) => zip(t1, t2));
        }

        public static void Zip<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Action<T1, T2> zip)
        {
            Zip(seq1, seq2, (t1, t2) => { zip(t1, t2); return 0; }).Ping();
        }

        public static void Zip<T1, T2>(this IEnumerable<Tuple<T1, T2>> seq, Action<T1, T2> zip)
        {
            Zip(seq, (t1, t2) => { zip(t1, t2); return 0; }).Ping();
        }

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, int, R> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (!next1 || !next2)
                {
                    yield break;
                }
                else
                {
                    yield return zip(seq1e.Current, seq2e.Current, i++);
                }
            }
        }

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<Tuple<T1, T2>> seq, Func<T1, T2, int, R> zip)
        {
            return seq.Select((t, i) => zip(t.Item1, t.Item2, i));
        }

        public static void Zip<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Action<T1, T2, int> zip)
        {
            Zip(seq1, seq2, (t1, t2, i) => { zip(t1, t2, i); return 0; }).Ping();
        }

        public static void Zip<T1, T2>(this IEnumerable<Tuple<T1, T2>> seq, Action<T1, T2, int> zip)
        {
            Zip(seq, (t1, t2, i) => { zip(t1, t2, i); return 0; }).Ping();
        }

        public static IEnumerable<Tuple<T1, T2, T3>> Zip<T1, T2, T3>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3)
        {
            return Zip(seq1, seq2, seq3, (t1, t2, t3) => Tuple.New(t1, t2, t3));
        }

        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> Unzip<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> seq)
        {
            return Tuple.New<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(seq.Select(t => t.Item1).ToArray(), seq.Select(t => t.Item2).ToArray(), seq.Select(t => t.Item3).ToArray());
        }

        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> Unzip<T1, T2, T3>(this IEnumerable<MutableTuple<T1, T2, T3>> seq)
        {
            return Tuple.New<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(seq.Select(t => t.Item1).ToArray(), seq.Select(t => t.Item2).ToArray(), seq.Select(t => t.Item3).ToArray());
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Func<T1, T2, T3, R> zip)
        {
            return Zip(seq1, seq2, seq3, (t1, t2, t3, i) => zip(t1, t2, t3));
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(this IEnumerable<Tuple<T1, T2, T3>> seq, Func<T1, T2, T3, R> zip)
        {
            return Zip(seq, (t1, t2, t3, i) => zip(t1, t2, t3));
        }

        public static void Zip<T1, T2, T3>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Action<T1, T2, T3> zip)
        {
            Zip(seq1, seq2, seq3, (t1, t2, t3) => { zip(t1, t2, t3); return 0; }).Ping();
        }

        public static void Zip<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> seq, Action<T1, T2, T3> zip)
        {
            Zip(seq, (t1, t2, t3) => { zip(t1, t2, t3); return 0; }).Ping();
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Func<T1, T2, T3, int, R> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();
            var seq3e = (seq3 ?? Enumerable.Empty<T3>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext(), next3 = seq3e.MoveNext();
                if (!next1 || !next2 || !next3)
                {
                    yield break;
                }
                else
                {
                    yield return zip(seq1e.Current, seq2e.Current, seq3e.Current, i++);
                }
            }
        }

        public static void Zip<T1, T2, T3>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Action<T1, T2, T3, int> zip)
        {
            Zip(seq1, seq2, seq3, (t1, t2, t3, i) => { zip(t1, t2, t3, i); return 0; }).Ping();
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(this IEnumerable<Tuple<T1, T2, T3>> seq, Func<T1, T2, T3, int, R> zip)
        {
            return seq.Select((t, i) => zip(t.Item1, t.Item2, t.Item3, i));
        }

        public static void Zip<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> seq, Action<T1, T2, T3, int> zip)
        {
            Zip(seq, (t1, t2, t3, i) => { zip(t1, t2, t3, i); return 0; }).Ping();
        }
    }
}