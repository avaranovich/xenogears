using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Reflection.Simple;

namespace XenoGears.Functional
{
    // todo. think about multithreading and atomicity

    public static partial class EnumerableExtensions
    {
        public static V GetAndRemove<K, V>(this IDictionary<K, V> map, K key)
        {
            var value = map[key];
            map.Remove(key);
            return value;
        }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> map, K key)
        {
            return map.GetOrDefault(key, default(V));
        }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> map, K key, V value)
        {
            return map.GetOrDefault(key, () => value);
        }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> map, K key, Func<V> factory)
        {
            return map.GetOrDefault(key, _ => factory());
        }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> map, K key, Func<K, V> factory)
        {
            return map.ContainsKey(key) ? map[key] : factory(key);
        }

        public static V GetOrCreate<K, V>(this IDictionary<K, V> map, K key)
        {
            return map.GetOrCreate(key, default(V));
        }

        public static V GetOrCreate<K, V>(this IDictionary<K, V> map, K key, V value)
        {
            return map.GetOrCreate(key, () => value);
        }

        public static V GetOrCreate<K, V>(this IDictionary<K, V> map, K key, Func<V> factory)
        {
            return map.GetOrCreate(key, _ => factory());
        }

        public static V GetOrCreate<K, V>(this IDictionary<K, V> map, K key, Func<K, V> factory)
        {
            if (map.ContainsKey(key))
            {
                return map[key];
            }
            else
            {
                var created = factory(key);
                map[key] = created; // poor man's synchronization
                return created;
            }
        }

        public static void AddElements<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> addendum)
        {
            if (addendum == null) return;
            addendum.ForEach(dict.Add);
        }

        public static void RemoveElements<TKey, TValue>(this IDictionary<TKey, TValue> dict, params TKey[] keys)
        {
            dict.RemoveElements((IEnumerable<TKey>)keys);
        }

        public static void RemoveElements<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
        {
            if (keys == null) return;
            keys.ForEach(key => dict.Remove(key));
        }

        public static void RemoveElements<TKey, TValue>(this IDictionary<TKey, TValue> dict, Func<TKey, bool> filter)
        {
            if (filter == null) return;
            dict.Keys.ForEach(key => { if (filter(key)) dict.Remove(key); });
        }

        public static void AddElements<T>(this HashSet<T> hashSet, IEnumerable<T> addendum)
        {
            if (addendum == null) return;
            addendum.ForEach(item => hashSet.Add(item));
        }

        public static Dictionary<K, V> Cast<K, V>(this IEnumerable dictionary)
        {
            return dictionary.Cast<Object>().ToDictionary(el => (K)el.Get("Key"), el => (V)el.Get("Value"));
        }

        public static Dictionary<K, T> ToDictionary<T, K>(this IEnumerable<T> seq, Func<T, int, K> keySelector)
        {
            return seq.ToDictionary(keySelector, EqualityComparer<K>.Default);
        }

        public static Dictionary<K, T> ToDictionary<T, K>(this IEnumerable<T> seq, Func<T, int, K> keySelector, IEqualityComparer<K> comparer)
        {
            return seq.ToDictionary(keySelector, el => el, comparer);
        }

        public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, K> keySelector, Func<T, int, V> valueSelector)
        {
            return seq.ToDictionary(keySelector, valueSelector, EqualityComparer<K>.Default);
        }

        public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, K> keySelector, Func<T, int, V> valueSelector, IEqualityComparer<K> comparer)
        {
            return seq.ToDictionary((el, _) => keySelector(el), valueSelector, comparer);
        }

        public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, int, K> keySelector, Func<T, V> valueSelector)
        {
            return seq.ToDictionary(keySelector, valueSelector, EqualityComparer<K>.Default);
        }

        public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, int, K> keySelector, Func<T, V> valueSelector, IEqualityComparer<K> comparer)
        {
            return seq.ToDictionary(keySelector, (el, _) => valueSelector(el), EqualityComparer<K>.Default);
        }

        public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, int, K> keySelector, Func<T, int, V> valueSelector)
        {
            return seq.ToDictionary(keySelector, valueSelector, EqualityComparer<K>.Default);
        }

        public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> seq, Func<T, int, K> keySelector, Func<T, int, V> valueSelector, IEqualityComparer<K> comparer)
        {
            var map = new Dictionary<K, V>();
            seq.ForEach((el, i) => map.Add(keySelector(el, i), valueSelector(el, i)));
            return map;
        }
    }
}