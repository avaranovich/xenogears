using System;
using System.Diagnostics;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class FluencyHelper
    {
        public static T Fluent<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }

        public static R Fluent<T, R>(this T obj, Func<T, R> func)
        {
            return func(obj);
        }
    }
}
