using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection.Generics.StructuralTrees
{
    [DebuggerNonUserCode]
    public static class TypeStructuralTreeHelper
    {
        public static TypeStructuralTree GetStructuralTree(this Type type)
        {
            return new TypeStructuralTree(type);
        }

        public static MethodStructuralTree GetStructuralTree(this MethodInfo mi)
        {
            return new MethodStructuralTree(mi);
        }

        public static Type SelectStructuralUnit(this Type type, String path)
        {
            return type.GetStructuralTree().Select(path);
        }

        public static Type SelectStructuralUnit(this MethodInfo mi, String path)
        {
            return mi.GetStructuralTree().Select(path);
        }
        
        public static Type InferStructuralUnit(this Type type, String path, Type inferred)
        {
            return type.GetStructuralTree().Infer(path, inferred).BuildType();
        }

        public static Type InferStructuralUnits(this Type type, Dictionary<String, Type> batch)
        {
            return type.GetStructuralTree().Infer(batch).BuildType();
        }

        public static MethodInfo InferStructuralUnit(this MethodInfo mi, String path, Type inferred)
        {
            return mi.GetStructuralTree().Infer(path, inferred).BuildMethod();
        }

        public static MethodInfo InferStructuralUnits(this MethodInfo mi, Dictionary<String, Type> batch)
        {
            return mi.GetStructuralTree().Infer(batch).BuildMethod();
        }
    }
}