using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Adapters.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class TypeAdapter : Adapter
    {
        public sealed override Object AfterDeserialize(MemberInfo mi, Object value) { return AfterDeserialize(mi.AssertCast<Type>(), value); }
        public sealed override Object BeforeSerialize(MemberInfo mi, Object value) { return BeforeSerialize(mi.AssertCast<Type>(), value); }

        public virtual Object AfterDeserialize(Type t, Object value) { return value; }
        public virtual Object BeforeSerialize(Type t, Object value) { return value; }
    }
}