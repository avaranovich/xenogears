using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Validators.Core;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public class Regex : PropertyValidator
    {
        public String Pattern { get; set; }

        public Regex(String pattern)
        {
            Pattern = pattern;
        }

        public override void Validate(PropertyInfo pi, Object value)
        {
            var s_value = value.AssertCast<String>();
            s_value.AssertMatch(Pattern);
        }
    }
}