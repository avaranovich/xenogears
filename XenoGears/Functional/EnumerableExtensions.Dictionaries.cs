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
    }
}