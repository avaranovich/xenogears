using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Functional
{
    public static partial class EnumerableExtensions
    {
        public static T[] MkArray<T>(this T entity)
        {
            return new []{ entity };
        }

        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> seq)
        {
            var seqAsRoc = seq as ReadOnlyCollection<T>;
            return seqAsRoc ?? new ReadOnlyCollection<T>(seq.ToList());
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> seq)
        {
            var seqAsHashSet = seq as HashSet<T>;
            return seqAsHashSet ?? new HashSet<T>(seq);
        }

        public static Dictionary<K, V> Map<K, V>(this K key, V value)
        {
            var map = new Dictionary<K, V>();
            map.Add(key, value);
            return map;
        }

        public static Dictionary<K, V> Map<K, V>(this IEnumerable<K> keys, IEnumerable<V> values)
        {
            var map = new Dictionary<K, V>();
            var count = keys.Count();
            (count == values.Count()).AssertTrue();

            // todo. find out why this doesn't work
//            keys.Zip(values, map.Add);
            keys.ForEach((k, i) => map.Add(k, values.ElementAt(i)));

            return map;
        }

        public static Dictionary<K, V> Map<K, V>(this IEnumerable<K> keys, params V[] values)
        {
            return Map(keys, (IEnumerable<V>)values);
        }

        public static ReadOnlyDictionary<K, V> ToReadOnly<K, V>(this IDictionary<K, V> map)
        {
            return new ReadOnlyDictionary<K, V>(map);
        }
    }
}