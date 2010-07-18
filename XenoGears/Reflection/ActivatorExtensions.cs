using System;
using System.Diagnostics;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class ActivatorExtensions
    {
        public static Object CreateInstance(this Type t, params Object[] args)
        {
            return Activator.CreateInstance(t, args);
        }
    }
}