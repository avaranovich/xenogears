using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Functional;

namespace XenoGears.Reflection.Generics.StructuralTrees
{
    [DebuggerNonUserCode]
    public static class GenericArgsMappingHelper
    {
        // say, we have a type Type<(1) T1, (2) T2> with the method of the following signature:
        // T3[,][] Meth<(3) T3>(T1 o1, Func<T1, T3> o2) that has the following public type points:
        //
        // ReturnValue = T3 (a)[,][]
        // Args = T1 (b), Func<T1 (c), T3 (d)>
        // 
        // to be perfectly fine we need the following typelinks to be built:
        // (3) <-> (a), (1) <-> (b), (1) <-> (c), (3) <-> (d)
        // 
        // after some analysis we build the following mapping:
        // (key <- value)
        // a0 <- t[0]
        // a1[0] <- t[0]
        // a1[1] <- m[0]
        // ret[0][0] <- m[0]

        public static Dictionary<String, String> GetGenericArgsMapping(this MethodInfo method)
        {
            if (method == null)
            {
                return null;
            }
            else
            {
                var pattern = (MethodInfo)method.Module.ResolveMethod(method.MetadataToken);
                var methodGenericArgs = pattern.XGetGenericArguments();
                var typeGenericArgs = pattern.DeclaringType.XGetGenericArguments();

                var mapping = new Dictionary<String, String>();
                mapping.AddElements(AppendMethodMapping(pattern, methodGenericArgs, "m"));
                mapping.AddElements(AppendMethodMapping(pattern, typeGenericArgs, "t"));
                return mapping;
            }
        }

        public static Dictionary<String, String> GetGenericArgsMapping(this Type type) 
        {
            return type == null ? null : GetGenericArgsMapping(
                type.GetBasisType(), 
                type.GetBasisType().XGetGenericArguments());
        }

        public static Dictionary<String, String> GetGenericArgsMapping(this MemberInfo member)
        {
            if (member == null)
            {
                return null;
            }
            else if (!(member is FieldInfo) && !(member is PropertyInfo))
            {
                throw new NotSupportedException(String.Format(
                    "Member '{0}' of type '{1}' is not supported", member, member.MemberType));
            }
            else
            {
                var t = member.DeclaringType;
                if (!t.IsGenericType)
                {
                    return new Dictionary<String, String>();
                }
                else
                {
                    var t_genericDef = t.XGetGenericDefinition();
                    var typedefArgs = t_genericDef.XGetGenericArguments();
                    var genericMember = t_genericDef.GetFieldOrProperty(member.Name);

                    var memberType = genericMember.GetFieldOrPropertyType();
                    return memberType.GetGenericArgsMapping(typedefArgs);
                }
            }
        }

        private static Dictionary<String, String> AppendMethodMapping(MethodInfo method, IEnumerable<Type> types, String prefix)
        {
            var mapping = new Dictionary<String, String>();
            mapping.AddElements(GetGenericArgsMapping(method.ReturnType, types, "ret", prefix));
            for (var i = 0; i < method.GetParameters().Length; ++i)
            {
                var parameterType = method.GetParameters()[i].ParameterType;
                mapping.AddElements(GetGenericArgsMapping(parameterType, types, "a" + i, prefix));
            }

            return mapping;
        }

        public static Dictionary<String, String> GetGenericArgsMapping(this Type anyType, IEnumerable<Type> typedefArgs, String prefixMember, String prefixTypedef)
        {
            var mapping = new Dictionary<String, String>();
            foreach (var kvp in GetGenericArgsMapping(anyType, typedefArgs))
            {
                mapping.Add(prefixMember + kvp.Key, prefixTypedef + kvp.Value);
            }

            return mapping;
        }

        public static Dictionary<String, String> GetGenericArgsMapping(this Type anyType, IEnumerable<Type> typedefArgs)
        {
            var fieldGenericArgs = anyType.GetStructuralTree();

            var mapping = new Dictionary<String, String>();
            for (var i = 0; i < typedefArgs.Count(); i++)
            {
                var typeDefGenericArg = typedefArgs.ElementAt(i);
                foreach (var fieldGenericArg in fieldGenericArgs)
                {
                    if (typeDefGenericArg.SameMetadataToken(fieldGenericArg.Value))
                    {
                        mapping[fieldGenericArg.Key] = "[" + i + "]";
                    }
                }
            }

            return mapping;
        }
    }
}