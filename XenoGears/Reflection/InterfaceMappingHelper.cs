using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class InterfaceMappingHelper
    {
        private static readonly Dictionary<Tuple<Type, Type>, ReadOnlyDictionary<MethodBase, MethodBase>> _im2ifCache = new Dictionary<Tuple<Type, Type>, ReadOnlyDictionary<MethodBase, MethodBase>>();
        public static ReadOnlyDictionary<MethodBase, MethodBase> MapImplsToInterfaces(this Type t, Type iface)
        {
            if (t == null || iface == null) return null;
            return _im2ifCache.GetOrCreate(Tuple.Create(t, iface), () =>
            {
                var map = t.GetInterfaceMap(iface);

                var res = new Dictionary<MethodBase, MethodBase>();
                for (var i = 0; i < map.TargetMethods.Length; i++)
                {
                    res.Add(map.TargetMethods[i], map.InterfaceMethods[i]);
                }

                return res.ToReadOnly();
            });
        }

        public static ReadOnlyDictionary<MethodBase, MethodBase> MapImplsToInterfaces(this Type t, IEnumerable<Type> ifaces)
        {
            var map = new Dictionary<MethodBase, MethodBase>();
            ifaces.ForEach(iface => map.AddElements(t.MapImplsToInterfaces(iface)));
            return map.ToReadOnly();
        }

        public static ReadOnlyDictionary<MethodBase, MethodBase> MapImplsToInterfaces(this Type t, params Type[] ifaces)
        {
            return t.MapImplsToInterfaces((IEnumerable<Type>)ifaces);
        }

        private static readonly Dictionary<Tuple<Type, Type>, ReadOnlyDictionary<MethodBase, MethodBase>> _if2imCache = new Dictionary<Tuple<Type, Type>, ReadOnlyDictionary<MethodBase, MethodBase>>();
        public static ReadOnlyDictionary<MethodBase, MethodBase> MapInterfacesToImpls(this Type t, Type iface)
        {
            if (t == null || iface == null) return null;
            return _if2imCache.GetOrCreate(Tuple.Create(t, iface), () =>
            {
                var map = t.GetInterfaceMap(iface);

                var res = new Dictionary<MethodBase, MethodBase>();
                for (var i = 0; i < map.TargetMethods.Length; i++)
                {
                    res.Add(map.InterfaceMethods[i], map.TargetMethods[i]);
                }

                return res.ToReadOnly();
            });
        }

        public static ReadOnlyDictionary<MethodBase, MethodBase> MapInterfacesToImpls(this Type t, IEnumerable<Type> ifaces)
        {
            var map = new Dictionary<MethodBase, MethodBase>();
            ifaces.ForEach(iface => map.AddElements(t.MapInterfacesToImpls(iface)));
            return map.ToReadOnly();
        }

        public static ReadOnlyDictionary<MethodBase, MethodBase> MapInterfacesToImpls(this Type t, params Type[] ifaces)
        {
            return t.MapInterfacesToImpls((IEnumerable<Type>)ifaces);
        }
    }
}