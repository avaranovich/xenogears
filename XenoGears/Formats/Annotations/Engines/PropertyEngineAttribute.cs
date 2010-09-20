using System;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Annotations.Engines
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class PropertyEngineAttribute : EngineAttribute
    {
        public sealed override Object Deserialize(MemberInfo mi, Json json) { return Deserialize(mi.AssertCast<PropertyInfo>(), json); }
        public sealed override Json Serialize(MemberInfo mi, Object value) { return Serialize(mi.AssertCast<PropertyInfo>(), value); }

        public abstract Object Deserialize(PropertyInfo pi, Json json);
        public abstract Json Serialize(PropertyInfo pi, Object value);
    }
}