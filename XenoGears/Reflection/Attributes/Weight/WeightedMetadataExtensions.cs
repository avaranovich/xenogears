using System;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection.Attributes.Weight
{
    [DebuggerNonUserCode]
    public static class WeightedMetadataExtensions
    {
        public static double Weight<T>(this Type t)
            where T : WeightedAttribute
        {
            return t.Attr<T>().Weight;
        }

        public static double Metric<T>(this Type t)
            where T : WeightedAttribute
        {
            return 1 / t.Weight<T>();
        }

        public static double Weight<T>(this MethodInfo mi)
            where T : WeightedAttribute
        {
            var t = mi.DeclaringType;
            return t.Attr<T>().Weight * mi.Attr<T>().Weight;
        }

        public static double Metric<T>(this MethodInfo mi)
            where T : WeightedAttribute
        {
            return 1 / mi.Weight<T>();
        }
    }
}