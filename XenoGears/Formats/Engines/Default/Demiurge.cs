using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration.Default;
using XenoGears.Reflection;
using XenoGears.Reflection.Emit2;
using XenoGears.Reflection.Generics;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Formats.Configuration;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Streams;
using XenoGears.Reflection.Emit;
using XenoGears.Strings;

namespace XenoGears.Formats.Engines.Default
{
    internal static class Demiurge
    {
        public static Object Reify(this Type t)
        {
            if (t.SameMetadataToken(typeof(IEnumerable<>)) ||
                t.SameMetadataToken(typeof(ICollection<>)) ||
                t.SameMetadataToken(typeof(IList<>)))
            {
                var t_el = t.XGetGenericArguments().AssertSingle();
                t = typeof(List<>).XMakeGenericType(t_el);
            }

            if (t.SameMetadataToken(typeof(IDictionary<,>)))
            {
                var t_key = t.XGetGenericArguments().First();
                if (t_key == typeof(String))
                {
                    var t_value = t.XGetGenericArguments().Second();
                    t = typeof(Dictionary<,>).XMakeGenericType(t_key, t_value);
                }
            }

            if (t.IsInterface || t.IsAbstract)
            {
                var key = typeof(Demiurge).Assembly.ReadKey("XenoGears.XenoGears.snk");
                var unit = Codegen.Units["XenoGears.Demiurge", key];
                return unit.Context.GetOrCreate(t, () =>
                {
                    var name = String.Format("Reified_{0}", t.Name);
                    var rt = unit.Module.DefineType(name, TA.Public);
                    if (t.IsInterface) t.Hierarchy().ForEach(rt.AddInterfaceImplementation);
                    else rt.SetParent(t);

                    var abstracts = t.Hierarchy().SelectMany(ht => ht.GetProperties(BF.AllInstance)).Where(p => p.IsAbstract()).ToReadOnly();
                    abstracts = abstracts.Where(p1 => abstracts.None(p2 => p2.Overrides(p1))).ToReadOnly();
                    abstracts.ForEach(p =>
                    {
                        MethodBuilder get, set;
                        rt.DefineOverride(p, out get, out set);

                        var f = rt.DefineField("_" + p.Name.Uncapitalize(), p.PropertyType, FA.Private);
                        if (get != null) get.il().ldarg(0).ldfld(f).ret();
                        if (set != null) set.il().ldarg(0).ldarg(1).stfld(f).ret();
                    });

                    return rt.CreateType();
                });
            }

            var cfg = t.Config().DefaultEngine().Config;
            var default_ctor = cfg.DefaultCtor && t.HasDefaultCtor();
            return default_ctor ? t.CreateInstance() : t.CreateUninitialized();
        }
    }
}
