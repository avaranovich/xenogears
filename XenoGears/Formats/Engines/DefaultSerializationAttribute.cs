using System;
using XenoGears.Formats.Annotations.Engines;

namespace XenoGears.Formats.Engines
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 3) support interfaces => just create stubs for those
    // 4) deserialization is case-insensitive

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class DefaultSerializationAttribute : TypeEngineAttribute
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
