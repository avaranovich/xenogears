using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Adapters.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class PropertyAdapter : Adapter
    {
        public sealed override Object AfterDeserialize(MemberInfo mi, Object value) { return AfterDeserialize(mi.AssertCast<PropertyInfo>(), value); }
        public sealed override Object BeforeSerialize(MemberInfo mi, Object value) { return BeforeSerialize(mi.AssertCast<PropertyInfo>(), value); }

        public virtual Object AfterDeserialize(PropertyInfo pi, Object value) { return value; }
        public virtual Object BeforeSerialize(PropertyInfo pi, Object value) { return value; }
    }
}