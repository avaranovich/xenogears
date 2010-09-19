using System;
using System.Reflection;

namespace XenoGears.Formats.Adapters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class PropertyAdapterAttribute : Attribute
    {
        public abstract Object AfterDeserialize(PropertyInfo pi, Object value);
        public abstract Object BeforeSerialize(PropertyInfo pi, Object value);
    }
}