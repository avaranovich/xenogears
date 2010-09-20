using System;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Annotations.Adapters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class TypeAdapterAttribute : AdapterAttribute
    {
        public sealed override Object AfterDeserialize(MemberInfo mi, Object value) { return AfterDeserialize(mi.AssertCast<Type>(), value); }
        public sealed override Object BeforeSerialize(MemberInfo mi, Object value) { return BeforeSerialize(mi.AssertCast<Type>(), value); }

        public abstract Object AfterDeserialize(Type t, Object value);
        public abstract Object BeforeSerialize(Type t, Object value);
    }
}