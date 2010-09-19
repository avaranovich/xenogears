using System;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class TypeValidatorAttribute : Attribute
    {
        public abstract void Validate(Type t, Object value);
    }
}