using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Formats.Annotations.Adapters;

namespace XenoGears.Formats.Adapters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class Password : PropertyAdapter
    {
        public override Object AfterDeserialize(PropertyInfo pi, Object value)
        {
            var s_value = value.AssertCast<String>();
            return s_value == null ? null : Encoding.UTF8.GetString(Convert.FromBase64String(s_value));
        }

        public override Object BeforeSerialize(PropertyInfo pi, Object value)
        {
            var s_value = value.AssertCast<String>();
            return s_value == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(s_value));
        }
    }
}