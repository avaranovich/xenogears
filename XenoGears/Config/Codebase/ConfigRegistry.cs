using System;
using System.Collections.Generic;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes;
using XenoGears.Assertions;

namespace XenoGears.Config.Codebase
{
    public static class ConfigRegistry
    {
        private static Dictionary<String, Type> _cache = new Dictionary<String, Type>();
        static ConfigRegistry()
        {
            // todo. reload when appdomain assemblies change!
            AppDomain.CurrentDomain.GetAssemblies().ForEach(asm =>
            {
                var a_asms = asm.Attrs<ConfigCodebaseAttribute>();
                if (a_asms.IsNotEmpty())
                {
                    asm.GetTypes().ForEach(t =>
                    {
                        var a_types = t.Attrs<ConfigAttribute>();
                        a_types.ForEach(a => _cache.Add(a.Name, t));
                    });
                }
            });
        }

        public static ReadOnlyDictionary<String, Type> All
        {
            get { return _cache.ToReadOnly(); }
        }

        public static String ConfigName(this Type t)
        {
            if (t == null) return null;
            t.HasAttr<ConfigAttribute>().AssertTrue();
            return t.Attr<ConfigAttribute>().Name;
        }
    }
}