using System;
using System.Reflection;
using XenoGears.Formats.Annotations.Engines;

namespace XenoGears.Formats.Engines
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 3) support interfaces => just create stubs for those
    // 4) deserialization is case-insensitive

    public class DefaultEngineAttribute : EngineAttribute
    {
        public override Object Deserialize(MemberInfo mi, Json json)
        {
            throw new NotImplementedException();
        }

        public override Json Serialize(MemberInfo mi, Object value)
        {
            throw new NotImplementedException();
        }
    }
}
