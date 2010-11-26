using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Engines.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class PropertyEngine : Engine
    {
        public sealed override Object Deserialize(MemberInfo mi, Json json) { return Deserialize(mi.AssertCast<PropertyInfo>(), json); }
        public sealed override Json Serialize(MemberInfo mi, Object value) { return Serialize(mi.AssertCast<PropertyInfo>(), value); }

        public abstract Object Deserialize(PropertyInfo pi, Json json);
        public abstract Json Serialize(PropertyInfo pi, Object value);
    }
}