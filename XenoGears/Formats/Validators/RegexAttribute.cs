using System;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class RegexAttribute : PropertyValidatorAttribute
    {
        public String Regex { get; set; }

        public RegexAttribute(String regex)
        {
            Regex = regex;
        }

        public override void Validate(PropertyInfo pi, Object value)
        {
            var s_value = value.AssertCast<String>();
            s_value.AssertMatch(Regex);
        }
    }
}