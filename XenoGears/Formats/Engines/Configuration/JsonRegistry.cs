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
            // todo. configure all these types once and forget about this config
            // do not persist this stuff as a rule for all upcoming types
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
            IDisposable _;
            return Rule(t, out _);
        }

        public static MultiFluent Rule(Func<Type, bool> t, out IDisposable registration)
        {
            // todo. configure all these types now and apply the configuration for upcoming types
            // do persist this stuff as a rule for all upcoming types
            throw new NotImplementedException();
        }
    }
}
