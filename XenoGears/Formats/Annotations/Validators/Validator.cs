using System;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Annotations.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class Validator : Attribute
    {
        public abstract void Validate(MemberInfo mi, Object value);
    }
}