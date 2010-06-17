using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace XenoGears.Functional
{
    [DebuggerNonUserCode]
    public static partial class Set
    {
        public static bool Equal<T>(IEnumerable<T> set1, IEnumerable<T> set2)
        {
            return set1.SetEqual(set2);
        }

        public static bool Equal<T>(IEnumerable<T> set, T element)
        {
            return set.SetEqual(element.MkArray());
        }

        public static IEnumerable<T> Except<T>(IEnumerable<T> set1, IEnumerable<T> set2)
        {
            return set1.Except(set2);
        }

        public static IEnumerable<T> Except<T>(IEnumerable<T> set, T element)
        {
            return set.Except(element);
        }

        public static IEnumerable<T> Union<T>(IEnumerable<IEnumerable<T>> sets)
        {
            return sets.Union();
        }

        public static IEnumerable<T> Union<T>(IEnumerable<ReadOnlyCollection<T>> sets)
        {
            return sets.Union();
        }

        public static IEnumerable<T> Union<T>(params IEnumerable<T>[] set)
        {
            return set.Union();
        }

        public static IEnumerable<T> Intersect<T>(IEnumerable<IEnumerable<T>> sets)
        {
            return sets.Intersect();
        }

        public static IEnumerable<T> Intersect<T>(IEnumerable<ReadOnlyCollection<T>> sets)
        {
            return sets.Intersect();
        }

        public static IEnumerable<T> Intersect<T>(params IEnumerable<T>[] set)
        {
            return set.Intersect();
        }
    }
}
