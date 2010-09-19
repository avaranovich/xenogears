using System;
using System.Reflection;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class PropertyValidatorAttribute : Attribute
    {
        public abstract void Validate(PropertyInfo pi, Object value);
    }
}