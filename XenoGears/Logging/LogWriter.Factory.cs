using System;
using System.Collections.Generic;
using XenoGears.Functional;
using XenoGears.Logging.Media;

namespace XenoGears.Logging
{
    public partial class LogWriter
    {
        // todo. implement WeakValueDictionary and think about eviction policy
        private readonly static Dictionary<String, LogWriter> _cache = new Dictionary<String, LogWriter>();

        static LogWriter()
        {
            _cache.Add("Adhoc", new LogWriter(new AdhocMedium()));
            _cache.Add("Console", new LogWriter(new ConsoleMedium()));
            _cache.Add("Trace", new LogWriter(new TraceMedium()));
        }

        public static LogWriter Get(String name)
        {
            return _cache.GetOrCreate(name, () => new LogWriter());
        }

        public static LogWriter Get(Type type)
        {
            return Get(type.AssemblyQualifiedName);
        }
    }
}
