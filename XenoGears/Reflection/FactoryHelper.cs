using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class FactoryHelper
    {
        public static Object CreateInstance(this Type t, params Object[] args)
        {
            return Activator.CreateInstance(t, args);
        }

        public static Object CreateUninitialized(this Type t)
        {
            return FormatterServices.GetUninitializedObject(t);
        }
    }
}