using System;
using XenoGears.Formats.Engines.Configuration.Fluent;

namespace XenoGears.Formats.Engines.Configuration
{
    public static class JsonRegistry
    {
        public static JsonMetadata Metadata(this Type t)
        {
            throw new NotImplementedException();
        }

        public static SingleFluent AdHoc(this Type t)
        {
            throw new NotImplementedException();
        }

        public static MultiFluent AdHoc(Func<Type, bool> t)
        {
            throw new NotImplementedException();
        }

        public static MultiFluent Rule(this Type t)
        {
            return Rule(_t => _t == t);
        }

        public static MultiFluent Rule(Type t, out IDisposable registration)
        {
            return Rule(_t => _t == t, out registration);
        }

        public static MultiFluent Rule(Func<Type, bool> t)
        {
            throw new NotImplementedException();
        }

        public static MultiFluent Rule(Func<Type, bool> t, out IDisposable registration)
        {
            throw new NotImplementedException();
        }
    }
}
