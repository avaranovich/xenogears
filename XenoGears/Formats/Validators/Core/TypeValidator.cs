using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Validators.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public abstract class TypeValidator : Validator
    {
        public sealed override void Validate(MemberInfo mi, Object value) { Validate(mi.AssertCast<Type>(), value); }
        public abstract void Validate(Type t, Object value);
    }
}