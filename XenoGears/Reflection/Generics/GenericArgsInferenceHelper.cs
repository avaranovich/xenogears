using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Generics.StructuralTrees;

namespace XenoGears.Reflection.Generics
{
    [DebuggerNonUserCode]
    public static class GenericArgsInferenceHelper
    {
        public static Type[] InferGargs(this Type pattern, Type actual)
        {
            var t_pattern = pattern.GetStructuralTree();
            var t_actual = actual.GetStructuralTree();

            var map = new Dictionary<int, Type>();
            foreach (var kvp_pattern in t_pattern)
            {
                var path = kvp_pattern.Key;
                var subt_pattern = kvp_pattern.Value;
                var subt_actual = t_actual.GetOrDefault(path);

                if (subt_actual != null)
                {
                    if (subt_pattern.IsGenericParameter)
                    {
                        map[subt_pattern.GenericParameterPosition] = subt_actual;
                    }
                    else
                    {
                        if (subt_pattern == subt_actual)
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            if (map.IsEmpty())
            {
                return new Type[0];
            }
            else
            {
                (map.MaxOrDefault(kvp => kvp.Key) == map.Count()).AssertTrue();
                return map.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
            }
        }
    }
}