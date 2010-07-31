using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;

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

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> flattenedDict)
        {
            var dict = new Dictionary<TKey, TValue>();
            dict.AddElements(flattenedDict);
            return dict;
        }

        public static ReadOnlyDictionary<K, V> ToReadOnly<K, V>(this IDictionary<K, V> map)
        {
            return new ReadOnlyDictionary<K, V>(map);
        }

        public static OrderedDictionary<K, V> ToOrderedDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, K> key_selector, Func<T, V> value_selector)
        {
            var ordered = new OrderedDictionary<K, V>();
            seq.ForEach(el => ordered.Add(key_selector(el), value_selector(el)));
            return ordered;
        }

        public static OrderedDictionary<TKey, TValue> ToOrderedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> flattenedDict)
        {
            return flattenedDict.ToOrderedDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}