using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Annotations.Adapters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class PropertyAdapterAttribute : AdapterAttribute
    {
        public sealed override Object AfterDeserialize(MemberInfo mi, Object value) { return AfterDeserialize(mi.AssertCast<PropertyInfo>(), value); }
        public sealed override Object BeforeSerialize(MemberInfo mi, Object value) { return BeforeSerialize(mi.AssertCast<PropertyInfo>(), value); }

        public abstract Object AfterDeserialize(PropertyInfo pi, Object value);
        public abstract Object BeforeSerialize(PropertyInfo pi, Object value);
    }
}