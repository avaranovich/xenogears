using System;
using System.Collections.Generic;
using XenoGears.Functional;

namespace XenoGears.Logging
{
    public partial class Logger
    {
        // todo. implement WeakValueDictionary and think about eviction policy
        private readonly static Dictionary<String, Logger> _cache = new Dictionary<String, Logger>();

        public static Logger Get(String name)
        {
            return _cache.GetOrCreate(name, () => new Logger(name));
        }

        public static Logger Get(Type type)
        {
            return Get(type.AssemblyQualifiedName);
        }

        public static Logger Adhoc
        {
            get { return Get("Adhoc"); }
        }
    }
}
