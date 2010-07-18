using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Functional;

namespace XenoGears.Reflection.Anonymous
{
    [DebuggerNonUserCode]
    internal static class AnonymousTypesCache
    {
        [DebuggerNonUserCode]
        private class CacheKey
        {
            private String[] Args { get; set; }
            private Type[] Types { get; set; }

            public CacheKey(String[] args, Type[] types)
            {
                Args = args ?? new String[0];
                Types = types ?? new Type[0];
            }

            public override bool Equals(object obj)
            {
                var other = obj as CacheKey;
                return other != null && Args.SequenceEqual(other.Args) && Types.SequenceEqual(other.Types);
            }

            public override int GetHashCode()
            {
                var num = 0x10cee8ad;
                Args.ForEach(arg => num = (-1521134295 * num) + EqualityComparer<String>.Default.GetHashCode(arg));
                Types.ForEach(type => num = (-1521134295 * num) + EqualityComparer<Type>.Default.GetHashCode(type));
                return num;
            }
        }

        private static readonly Dictionary<CacheKey, Type> _cache = 
            new Dictionary<CacheKey, Type>();

        public static Type Get(String[] args, Type[] types, Func<Type> factory)
        {
            lock (_cache)
            {
                var key = new CacheKey(args, types);
                if (!_cache.ContainsKey(key))
                {
                    _cache.Add(key, factory());
                }

                return _cache[key];
            }
        }
    }
}
