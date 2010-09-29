using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Reflection.Attributes.Weight;

namespace XenoGears.Formats.Adapters.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class Adapter : WeightedAttribute
    {
        public virtual Object AfterDeserialize(MemberInfo mi, Object value) { return value; }
        public virtual Object BeforeSerialize(MemberInfo mi, Object value) { return value; }
    }
}