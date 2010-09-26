using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Annotations.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class TypeValidatorAttribute : ValidatorAttribute
    {
        public sealed override void Validate(MemberInfo mi, Object value) { Validate(mi.AssertCast<Type>(), value); }
        public abstract void Validate(Type t, Object value);
    }
}