using System;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Annotations.Validators;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NotEmptyAttribute : PropertyValidatorAttribute
    {
        public override void Validate(PropertyInfo pi, Object value)
        {
            (Equals(value, null) || Equals(value, String.Empty)).AssertTrue();
        }
    }
}