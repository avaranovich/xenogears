using System;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Annotations.Engines
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class EngineAttribute : Attribute
    {
        public abstract Object Deserialize(MemberInfo mi, Json json);
        public abstract Json Serialize(MemberInfo mi, Object value);
    }
}