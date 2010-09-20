using System;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Annotations.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class PropertyValidatorAttribute : ValidatorAttribute
    {
        public sealed override void Validate(MemberInfo mi, Object value) { Validate(mi.AssertCast<PropertyInfo>(), value); }
        public abstract void Validate(PropertyInfo pi, Object value);
    }
}