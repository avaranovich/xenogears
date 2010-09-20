using System;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Annotations.Validators;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NotNullAttribute : PropertyValidatorAttribute
    {
        public override void Validate(PropertyInfo pi, Object value)
        {
            (value != null).AssertTrue();
        }
    }
}