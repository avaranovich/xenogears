using System;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NotNullAttribute : PropertyValidatorAttribute
    {
        public override void Validate(PropertyInfo pi, Object value)
        {
            (value != null).AssertTrue();
        }
    }
}