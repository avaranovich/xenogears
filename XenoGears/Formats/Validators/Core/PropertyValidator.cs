using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Validators.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class PropertyValidator : Validator
    {
        public sealed override void Validate(MemberInfo mi, Object value) { Validate(mi.AssertCast<PropertyInfo>(), value); }
        public abstract void Validate(PropertyInfo pi, Object value);
    }
}