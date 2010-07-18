using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace XenoGears.Functional
{
    [DebuggerNonUserCode]
    public static partial class Seq
    {
        public static IEnumerable<T> Empty<T>()
        {
            return Enumerable.Empty<T>();
        }

        public static IEnumerable<int> Nats
        {
            get { return Infinite(i => i); }
        }

        public static IEnumerable<T> Infinite<T>(T t)
        {
            return Infinite(_ => t);
        }

        public static IEnumerable<T> Infinite<T>(Func<T> map)
        {
            return Infinite(_ => map());
        }

        public static IEnumerable<T> Infinite<T>(Func<int, T> map)
        {
            return 0.Unfold(i => i + 1).Select(map);
        }

        public static bool Equal<T>(IEnumerable<T> seq1, IEnumerable<T> seq2)
        {
            return seq1.SequenceEqual(seq2);
        }

        public static bool Equal<T>(IEnumerable<T> seq, T element)
        {
            return seq.SequenceEqual(element);
        }

        public static IEnumerable<T> Concat<T>(IEnumerable<T> seq1, IEnumerable<T> seq2)
        {
            return seq1.Concat(seq2);
        }

        public static IEnumerable<Object> Concat(IEnumerable<Object> seq1, IEnumerable<Object> seq2)
        {
            return seq1.Concat(seq2);
        }

        public static IEnumerable<T> Concat<T>(IEnumerable<T> seq, T element)
        {
            return seq.Concat(element);
        }

        public static IEnumerable<T> Concat<T>(T element, IEnumerable<T> seq)
        {
            return element.Concat(seq);
        }

        public static IEnumerable<T> Concat<T>(IEnumerable<IEnumerable<T>> sets)
        {
            return sets.Concat();
        }

        public static IEnumerable<T> Concat<T>(IEnumerable<ReadOnlyCollection<T>> sets)
        {
            return sets.Concat();
        }

        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] set)
        {
            return set.Concat();
        }
    }
}