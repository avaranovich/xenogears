using System;
using System.Reflection;

namespace XenoGears.Formats.Annotations.Adapters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class AdapterAttribute : Attribute
    {
        public abstract Object AfterDeserialize(MemberInfo mi, Object value);
        public abstract Object BeforeSerialize(MemberInfo mi, Object value);
    }
}