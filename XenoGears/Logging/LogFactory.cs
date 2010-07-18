using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Functional;

namespace XenoGears.Logging
{
    [DebuggerNonUserCode]
    public static class LogFactory
    {
        // todo. implement WeakValueDictionary and think about eviction policy
        private readonly static Dictionary<String, Logger> _cache = new Dictionary<String, Logger>();

        public static Logger GetLogger(String name)
        {
            return _cache.GetOrCreate(name, () => new Logger(name));
        }

        public static Logger GetLogger(Type type)
        {
            return GetLogger(type.AssemblyQualifiedName);
        }
    }
}
