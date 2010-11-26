using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Validators.Core;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public class NotEmpty : PropertyValidator
    {
        public override void Validate(PropertyInfo pi, Object value)
        {
            (Equals(value, null) || Equals(value, String.Empty)).AssertTrue();
        }
    }
}