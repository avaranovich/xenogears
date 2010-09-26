using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection.Attributes.Snippets
{
    [DebuggerNonUserCode]
    public static class SnippetRegistry
    {
        private static readonly Object _cacheLock = new Object();
        private static Dictionary<Type, ReadOnlyCollection<Snippet<MethodInfo>>> _cacheForMethods =
            new Dictionary<Type, ReadOnlyCollection<Snippet<MethodInfo>>>();
        private static Dictionary<Type, ReadOnlyCollection<Snippet<PropertyInfo>>> _cacheForProperties =
            new Dictionary<Type, ReadOnlyCollection<Snippet<PropertyInfo>>>();

        static SnippetRegistry()
        {
            AppDomain.CurrentDomain.AssemblyLoad += (o, e) =>
            {
                lock (_cacheLock) { _cacheForMethods = new Dictionary<Type, ReadOnlyCollection<Snippet<MethodInfo>>>(); }
                lock (_cacheLock) { _cacheForProperties = new Dictionary<Type, ReadOnlyCollection<Snippet<PropertyInfo>>>(); }
            };
        }

        public static ReadOnlyCollection<Snippet<MethodInfo>> Methods<A>()
            where A : Attribute
        {
            // temporary variable is introduced in order
            // not to get a crash when the cache is invalidated upon new assembly load
            var cache = _cacheForMethods;

            if (!cache.ContainsKey(typeof(A)))
            {
                lock (_cacheLock)
                {
                    if (!cache.ContainsKey(typeof(A)))
                    {
                        var annotation = typeof(A).Attr<SnippetAnnotationAnnotationAttribute>();
                        var includeAllMembers = annotation.AutoIncludeAllMembersInMarkedClasses;
                        var a_method = typeof(A);
                        var a_type = annotation.TypeMarker ?? a_method;
                        var a_asm = annotation.AssemblyMarker ?? a_type;

                        // todo. cache this and invalidate cache on appdomain changes
                        cache.Add(typeof(A), AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm =>
                        {
                            var a_asms = asm.Attrs(a_asm);
                            if (a_asms.IsEmpty())
                            {
                                return Enumerable.Empty<Snippet<MethodInfo>>();
                            }
                            else
                            {
                                return asm.GetTypes().SelectMany(t =>
                                {
                                    var a_types = t.Attrs(a_type);
                                    if (a_types.IsEmpty())
                                    {
                                        return Enumerable.Empty<Snippet<MethodInfo>>();
                                    }
                                    else
                                    {
                                        return t.GetMethods(BF.All).SelectMany(m =>
                                        {
                                            var a_methods = includeAllMembers ? 
                                                ((A)null).MkArray() :  m.Attrs(a_method);
                                            if (a_methods.IsEmpty())
                                            {
                                                return Enumerable.Empty<Snippet<MethodInfo>>();
                                            }
                                            else
                                            {
                                                var cp = Combinatorics.CartesianProduct(a_asms, a_types, a_methods);
                                                return cp.Select(triple => new Snippet<MethodInfo>(annotation, triple.Item1, triple.Item2, triple.Item3, m));
                                            }
                                        });
                                    }
                                });
                            }
                        }).ToReadOnly());
                    }
                }
            }

            // temporary variable is used in order
            // not to get a crash when the cache is invalidated upon new assembly load
            return cache[typeof(A)];
        }

        public static ReadOnlyDictionary<Type, ReadOnlyCollection<Snippet<MethodInfo>>> MethodsByHost<A>()
            where A : Attribute
        {
            return Methods<A>().GroupBy(s => s.Member.DeclaringType).ToDictionary(g => g.Key, g => g.ToReadOnly()).ToReadOnly();
        }

        public static ReadOnlyCollection<Snippet<PropertyInfo>> Properties<A>()
            where A : Attribute
        {
            // temporary variable is introduced in order
            // not to get a crash when the cache is invalidated upon new assembly load
            var cache = _cacheForProperties;

            if (!cache.ContainsKey(typeof(A)))
            {
                lock (_cacheLock)
                {
                    if (!cache.ContainsKey(typeof(A)))
                    {
                        var annotation = typeof(A).Attr<SnippetAnnotationAnnotationAttribute>();
                        var includeAllMembers = annotation.AutoIncludeAllMembersInMarkedClasses;
                        var a_prop = typeof(A);
                        var a_type = annotation.TypeMarker ?? a_prop;
                        var a_asm = annotation.AssemblyMarker ?? a_type;

                        // todo. cache this and invalidate cache on appdomain changes
                        cache.Add(typeof(A), AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm =>
                        {
                            var a_asms = asm.Attrs(a_asm);
                            if (a_asms.IsEmpty())
                            {
                                return Enumerable.Empty<Snippet<PropertyInfo>>();
                            }
                            else
                            {
                                return asm.GetTypes().SelectMany(t =>
                                {
                                    var a_types = t.Attrs(a_type);
                                    if (a_types.IsEmpty())
                                    {
                                        return Enumerable.Empty<Snippet<PropertyInfo>>();
                                    }
                                    else
                                    {
                                        return t.GetProperties(BF.All).SelectMany(p =>
                                        {
                                            var a_props = includeAllMembers ? 
                                                ((A)null).MkArray() :  p.Attrs(a_prop);
                                            if (a_props.IsEmpty())
                                            {
                                                return Enumerable.Empty<Snippet<PropertyInfo>>();
                                            }
                                            else
                                            {
                                                var cp = Combinatorics.CartesianProduct(a_asms, a_types, a_props);
                                                return cp.Select(triple => new Snippet<PropertyInfo>(annotation, triple.Item1, triple.Item2, triple.Item3, p));
                                            }
                                        });
                                    }
                                });
                            }
                        }).ToReadOnly());
                    }
                }
            }

            // temporary variable is used in order
            // not to get a crash when the cache is invalidated upon new assembly load
            return cache[typeof(A)];
        }

        public static ReadOnlyDictionary<Type, ReadOnlyCollection<Snippet<PropertyInfo>>> PropertiesByHost<A>()
            where A : Attribute
        {
            return Properties<A>().GroupBy(s => s.Member.DeclaringType).ToDictionary(g => g.Key, g => g.ToReadOnly()).ToReadOnly();
        }
    }
}
