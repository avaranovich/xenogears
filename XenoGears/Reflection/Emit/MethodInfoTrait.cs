using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using XenoGears.Reflection.Generics;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static class MethodInfoTrait
    {
        public static bool IsInvariantTo(this MethodInfo source, MethodInfo method)
        {
            if (source.IsGenericMethod || method.IsGenericMethod) throw new NotImplementedException(); // TODO: implement it somewhen

            var me = source.GetParameters();
            var he = method.GetParameters();

            if (source.ReturnType != method.ReturnType && !source.ReturnType.IsAssignableFrom(method.ReturnType)) return false;
            if (me.Length != he.Length) return false;
            for (var i = 0; i < me.Length; i++)
                if (he[i].ParameterType != me[i].ParameterType && !he[i].ParameterType.IsAssignableFrom(me[i].ParameterType)) return false;

            return true;
        }

//        private static Dictionary<Tuple<Type, Type>, InterfaceMapping> _mapCache = 
//            new Dictionary<Tuple<Type, Type>, InterfaceMapping>();

        public static IEnumerable<MethodInfo> Declarations(this MethodInfo source)
        {
            if (source == null) return new MethodInfo[0];
            if (source.DeclaringType == null) return new MethodInfo[0];
            if (source.DeclaringType.IsInterface) return new MethodInfo[0];

            return (from iface in source.DeclaringType.GetInterfaces() 
//            let cacheKey = Tuple.New(source.DeclaringType, iface)
//            let map = _mapCache.GetOrCreate(cacheKey, _ => source.DeclaringType.GetInterfaceMap(iface))
//            from entry in map.MapInterfaceToImpl()
            from entry in source.DeclaringType.GetInterfaceMap(iface).MapInterfaceToImpl()
            where entry.Value == source
            select entry.Key).Cast<MethodInfo>();
        }

        public static String ToShortString(this MethodInfo source)
        {
            var buff = new StringBuilder(256);
            buff.Append(source.DeclaringType != null ? source.DeclaringType.ToShortString() : "none")
                .Append("::");

            if (source.IsGenericMethod)
            {
                buff.Append(source.Name.Substring(0, source.Name.IndexOf('`')))
                    .Append('<')
                    .Append(String.Join(",", source.XGetGenericArguments().Select(x => x.ToShortString()).ToArray()))
                    .Append('>');
            }
            else
            {
                buff.Append(source.Name);
            }

            buff.Append('(')
                .Append(String.Join(", ", source.GetParameters().Select(x => x.ParameterType.ToShortString()).ToArray()))
                .Append("): ")
                .Append(source.ReturnType.ToShortString());

            return buff.ToString();
        }
    }
}