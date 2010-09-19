using System;

namespace XenoGears.Formats.Adapters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public abstract class TypeAdapterAttribute : Attribute
    {
        public abstract Object AfterDeserialize(Type t, Object value);
        public abstract Object BeforeSerialize(Type t, Object value);
    }
}