using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection.Emit
{
    [DebuggerNonUserCode]
    public static class InterfaceMappingTrait
    {
        public static IDictionary<MethodBase, MethodBase> MapImplToInterface(this InterfaceMapping source)
        {
            var res = new Dictionary<MethodBase, MethodBase>();
            for (var i = 0; i < source.TargetMethods.Length; i++)
            {
                res.Add(source.TargetMethods[i], source.InterfaceMethods[i]);
            }

            return res;
        }

        public static IDictionary<MethodBase, MethodBase> MapInterfaceToImpl(this InterfaceMapping source)
        {
            var res = new Dictionary<MethodBase, MethodBase>();
            for (var i = 0; i < source.TargetMethods.Length; i++)
            {
                res.Add(source.InterfaceMethods[i], source.TargetMethods[i]);
            }

            return res;
        }
    }
}