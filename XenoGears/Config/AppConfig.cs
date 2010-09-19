using System;
using XenoGears.Config.Codebase;
using XenoGears.Formats;
using XenoGears.Assertions;
using XenoGears.Reflection.Attributes;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Config
{
    public static class AppConfig
    {
        public static T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public static Object Get(Type t)
        {
            if (t == null) return null;
            var a_cfg = t.Attr<ConfigAttribute>();
            var json = Json.Load(a_cfg.Location);
            return json.Deserialize(t);
        }

        public static Object Get(String name)
        {
            if (name == null) return null;
            return Get(ConfigRegistry.All[name]);
        }

        public static void Put(Object obj)
        {
            Put(obj.AssertNotNull().GetType(), obj);
        }

        public static void Put(Type t, Object obj)
        {
            var a_cfg = t.Attr<ConfigAttribute>();
            Json.Save(a_cfg.Location, new Json(obj));
        }

        public static void Put(String name, Object obj)
        {
            Put(ConfigRegistry.All[name], obj);
        }

        public static void Merge(Object obj)
        {
            Merge(obj.AssertNotNull().GetType(), obj);
        }

        public static void Merge(Type t, Object obj)
        {
            var cfg = new Json(Get(t));

            var json = new Json(obj);
            json.Keys.ForEach(key =>
            {
                var p = cfg.GetType().GetProperty(key, BF.AllInstance | BF.IgnoreCase);
                var v = json[key].Deserialize(p);
                p.SetValue(cfg, v, null);
            });

            var a_cfg = t.Attr<ConfigAttribute>();
            Json.Save(a_cfg.Location, cfg);
        }

        public static void Merge(String name, Object obj)
        {
            Merge(ConfigRegistry.All[name], obj);
        }
    }
}