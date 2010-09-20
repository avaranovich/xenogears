using System;
using System.Reflection;

namespace XenoGears.Formats.Annotations.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public abstract class ValidatorAttribute : Attribute
    {
        public abstract void Validate(MemberInfo mi, Object value);
    }
}