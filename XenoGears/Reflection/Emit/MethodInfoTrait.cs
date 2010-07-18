using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Reflection.Generics;
using XenoGears.Functional;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static class MethodInfoTrait
    {
        public static bool IsInvariantTo(this MethodBase source, MethodBase method)
        {
            if (source.IsGenericMethod || method.IsGenericMethod) throw new NotImplementedException(); // TODO: implement it somewhen

            var me = source.GetParameters();
            var he = method.GetParameters();

            if (source.Ret() != method.Ret() && !source.Ret().IsAssignableFrom(method.Ret())) return false;
            if (me.Length != he.Length) return false;
            for (var i = 0; i < me.Length; i++)
                if (he[i].ParameterType != me[i].ParameterType && !he[i].ParameterType.IsAssignableFrom(me[i].ParameterType)) return false;

            return true;
        }

        private static readonly Dictionary<MethodInfo, ReadOnlyCollection<MethodBase>> _declarationsCache = new Dictionary<MethodInfo, ReadOnlyCollection<MethodBase>>();
        public static ReadOnlyCollection<MethodBase> Declarations(this MethodInfo m)
        {
            if (m == null) return null;
            if (m.DeclaringType == null) return Seq.Empty<MethodBase>().ToReadOnly();
            if (m.DeclaringType.IsInterface) return Seq.Empty<MethodBase>().ToReadOnly();
            return _declarationsCache.GetOrCreate(m, () => m.DeclarationsImpl().ToReadOnly());
        }

        private static IEnumerable<MethodBase> DeclarationsImpl(this MethodInfo m)
        {
            foreach (var t_iface in m.DeclaringType.GetInterfaces())
            {
                var map = m.DeclaringType.MapImplsToInterfaces(t_iface);

                // note. here I've faced a strange bug
                // when m is an explicit interface implementation, map ain't contain it :O
                // so the line below refuses to work!
//                var m_iface = map.GetOrDefault(m);
                var m_iface = map.SingleOrDefault(kvp => kvp.Key.MethodHandle == m.MethodHandle).Value;

                if (m_iface != null)
                {
                    yield return m_iface;
                }
            }
        }
    }
}