using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using XenoGears.Reflection.Generics.StructuralTrees;

namespace XenoGears.Reflection.Generics
{
    [DebuggerNonUserCode]
    public static class GenericArgsInferenceHelper
    {
        public static bool CanInferGargs(this Type pattern, Type actual)
        {
            return pattern.InferGargs(actual) != null;
        }

        public static Dictionary<String, Type> InferGargs(this Type pattern, Type actual)
        {
            var map1 = pattern.GetStructuralTree();
            var map2 = actual.GetStructuralTree();

            var inferences = new Dictionary<String, Type>();
            var result = map1.Keys.All(key =>
            {
                var map1i = map1.First(kvp => kvp.Key == key);
                var map2i = map2.First(kvp => kvp.Key == key);

                if (!map1i.Value.SameBasisType(map2i.Value))
                {
                    if (!map1i.Value.IsGenericParameter)
                    {
                        return false;
                    }
                    else
                    {
                        var source = actual.SelectStructuralUnit(map2i.Key);
                        var t1inferred = pattern.InferStructuralUnit(map1i.Key, source);

                        if (t1inferred != null)
                        {
                            if (!inferences.ContainsKey(map2i.Key))
                            {
                                inferences.Add(map2i.Key, source);
                                return true;
                            }
                            else
                            {
                                return inferences[map2i.Key] == source;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return true;
                }
            });

            return result ? inferences : null;
        }
    }
}