using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes.Weight;

namespace XenoGears.Reflection.Attributes.Codebase
{
    [DebuggerNonUserCode]
    public static class CodebaseRegistry
    {
        private static Object _cacheLock = new Object();
        private static Dictionary<Type, ReadOnlyCollection<Assembly>> _cache = 
            new Dictionary<Type, ReadOnlyCollection<Assembly>>();

        static CodebaseRegistry()
        {
            AppDomain.CurrentDomain.AssemblyLoad += (o, e) =>
            {
                lock (_cacheLock) { _cache = new Dictionary<Type, ReadOnlyCollection<Assembly>>(); }
            };
        }

        public static ReadOnlyCollection<Assembly> For<A>()
            where A : Attribute
        {
            // temporary variable is introduced in order
            // not to get a crash when the cache is invalidated upon new assembly load
            var cache = _cache;

            if (!cache.ContainsKey(typeof(A)))
            {
                lock (_cacheLock)
                {
                    if (!cache.ContainsKey(typeof(A)))
                    {
                        _cache.Add(typeof(A), AppDomain.CurrentDomain.GetAssemblies()
                            .Where(asm => asm.HasAttr(typeof(A)))
                            .OrderByDescending(asm => asm.Attrs(typeof(A)).MaxOrDefault(
                                a => a is WeightedAttribute ? ((WeightedAttribute)a).Weight : 1.0))
                            .ToReadOnly());
                    }
                }
            }

            // temporary variable is used in order
            // not to get a crash when the cache is invalidated upon new assembly load
            return cache[typeof(A)];
        }
    }
}
