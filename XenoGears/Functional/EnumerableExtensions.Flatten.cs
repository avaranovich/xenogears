using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<Object> Flatten(this IEnumerable<IEnumerable> twoDimensional)
        {
            return twoDimensional.Flatten<Object>();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable> twoDimensional)
        {
            foreach (var oneDimensional in twoDimensional ?? Enumerable.Empty<IEnumerable>())
            {
                foreach (var element in oneDimensional ?? Enumerable.Empty<T>())
                {
                    yield return (T)element;
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> twoDimensional)
        {
            foreach (var oneDimensional in twoDimensional ?? Enumerable.Empty<IEnumerable<T>>())
            {
                foreach (var element in oneDimensional ?? Enumerable.Empty<T>())
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T[]> twoDimensional)
        {
            return Flatten(twoDimensional.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<ReadOnlyCollection<T>> twoDimensional)
        {
            return Flatten(twoDimensional.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<KeyValuePair<K, V>> Flatten<K, V>(this IEnumerable<Dictionary<K, V>> twoDimensional)
        {
            return Flatten(twoDimensional.Cast<IEnumerable<KeyValuePair<K, V>>>());
        }

        public static IEnumerable<T> Flatten<T>(this T root, Func<T, IEnumerable<T>> children)
        {
            var q = new Queue<T>();
            q.Enqueue(root);

            while (q.IsNotEmpty())
            {
                var item = q.Dequeue();
                yield return item;
                children(item).ForEach(q.Enqueue);
            }
        }

        public static IDictionary<T, R> Flatten<T, R>(this T root, Func<T, IEnumerable<T>> children, Func<T, R> mapper)
        {
            return Flatten(root, children).ToDictionary(t => t, mapper);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> rootColl, Func<T, IEnumerable<T>> children)
        {
            return rootColl.SelectMany(root => root.Flatten(children));
        }

        public static IDictionary<T, R> Flatten<T, R>(this IEnumerable<T> rootColl, Func<T, IEnumerable<T>> children, Func<T, R> mapper)
        {
            return Flatten(rootColl, children).ToDictionary(t => t, mapper);
        }
    }
}