using System;
using System.Diagnostics;
using XenoGears.Formats.Engines.Core;

namespace XenoGears.Formats.Engines
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 3) support interfaces => just create stubs for those
    // 4) deserialization is case-insensitive. understand this!
    // 5) don't forget to quote json strings for string literals and string-serializable

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class DefaultEngine : TypeEngine
    {
        public override Object Deserialize(Type t, Json json)
        {
            throw new NotImplementedException();
        }

        public override Json Serialize(Type t, Object value)
        {
            throw new NotImplementedException();
        }
    }
}
