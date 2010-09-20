using System;
using System.Reflection;

namespace XenoGears.Formats.Annotations.Engines
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class EngineAttribute : Attribute
    {
        public abstract Object Deserialize(MemberInfo mi, Json json);
        public abstract Json Serialize(MemberInfo mi, Object value);
    }
}