using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection.Attributes.Snippets
{
    [DebuggerNonUserCode]
    public static class SnippetRegistry
    {
        private static Object _cacheLock = new Object();
        private static Dictionary<Type, ReadOnlyCollection<Snippet>> _cache = 
            new Dictionary<Type, ReadOnlyCollection<Snippet>>();

        static SnippetRegistry()
        {
            AppDomain.CurrentDomain.AssemblyLoad += (o, e) =>
            {
                lock (_cacheLock) { _cache = new Dictionary<Type, ReadOnlyCollection<Snippet>>(); }
            };
        }

        public static ReadOnlyCollection<Snippet> For<A>()
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
                        var annotation = typeof(A).Attr<SnippetAnnotationAnnotationAttribute>();
                        var includeAllMethods = annotation.AutoIncludeAllMethodsInMarkedClasses;
                        var a_method = typeof(A);
                        var a_type = annotation.TypeMarker ?? a_method;
                        var a_asm = annotation.AssemblyMarker ?? a_type;

                        // todo. cache this and invalidate cache on appdomain changes
                        _cache.Add(typeof(A), AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm =>
                        {
                            var a_asms = asm.Attrs(a_asm);
                            if (a_asms.IsEmpty())
                            {
                                return Enumerable.Empty<Snippet>();
                            }
                            else
                            {
                                return asm.GetTypes().SelectMany(t =>
                                {
                                    var a_types = t.Attrs(a_type);
                                    if (a_types.IsEmpty())
                                    {
                                        return Enumerable.Empty<Snippet>();
                                    }
                                    else
                                    {
                                        return t.GetMethods(BF.All).SelectMany(m =>
                                        {
                                            var a_methods = includeAllMethods ? 
                                                ((A)null).MkArray() :  m.Attrs(a_method);
                                            if (a_methods.IsEmpty())
                                            {
                                                return Enumerable.Empty<Snippet>();
                                            }
                                            else
                                            {
                                                var cp = Combinatorics.CartesianProduct(a_asms, a_types, a_methods);
                                                return cp.Select(triple => new Snippet(annotation, triple.Item1, triple.Item2, triple.Item3, m));
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
    }
}
