using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Configuration.Default;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Strings;
using XenoGears.Formats.Engines.Default;
using XenoGears.Reflection.Generics;

namespace XenoGears.Formats.Engines
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class DefaultEngine : TypeEngine
    {
        public override Object Deserialize(Type t, Json json)
        {
            if (json.HasValue && json.Value == null) return null;

            var cfg = t.Config().DefaultEngine().Config;
            if (cfg.IsPrimitive)
            {
                if (t.IsJsonPrimitive())
                {
                    return json.Value;
                }
                else
                {
                    t.SupportsSerializationToString().AssertTrue();
                    var s_value = json.Value.AssertCast<String>();
                    return s_value.FromInvariantString(t);
                }
            }
            else
            {
                var obj = t.Reify();

                if (cfg.IsHash)
                {
                    var add = t.GetMethods(BF.AllInstance).AssertSingle(m => m.Name == "Add" && m.Paramc() == 2);
                    (add.Param(0) == typeof(String)).AssertTrue();
                    var t_v = add.Param(1);

                    json.IsObject.AssertTrue();
                    json.Cast<String, Json>().ForEach(kvp =>
                    {
                        var key = kvp.Key;
                        var value = kvp.Value.Deserialize(t_v);
                        add.Invoke(obj, new []{key, value});
                    });
                }
                else if (cfg.IsList)
                {
                    var add = t.GetMethods(BF.AllInstance).AssertSingle(m => m.Name == "Add" && m.Paramc() == 1);
                    var t_v = add.Param(1);

                    json.IsArray.AssertTrue();
                    json.Values.Cast<Json>().ForEach(j_value =>
                    {
                        var value = j_value.Deserialize(t_v);
                        add.Invoke(obj, new []{value});
                    });
                }
                else if (cfg.IsObject)
                {
                    json.IsObject.AssertTrue();
                    json.Cast<String, Json>().ForEach(kvp =>
                    {
                        var key = kvp.Key;
                        // todo. match key with properties from cfg.Slots (respect styling and JsonPropertyAttribute::Name)
                        var mi = ((Func<MemberInfo>)(() => { throw new NotImplementedException(); }))();
                        var value = kvp.Value.Deserialize(mi);
                        mi.SetValue(obj, value);
                    });
                }
                else
                {
                    throw AssertionHelper.Fail();
                }

                return obj;
            }
        }

        public override Json Serialize(Type t, Object obj)
        {
            if (obj == null) return new Json(null);

            var cfg = t.Config().DefaultEngine().Config;
            if (cfg.IsPrimitive)
            {
                if (t.IsJsonPrimitive())
                {
                    return new JsonPrimitive(obj);
                }
                else
                {
                    t.SupportsSerializationToString().AssertTrue();
                    return new JsonPrimitive(t.ToInvariantString());
                }
            }
            else
            {
                if (cfg.IsHash)
                {
                    var cast = typeof(EnumerableExtensions).GetMethod("Cast", BF.PublicStatic).AssertNotNull();
                    cast = cast.XMakeGenericMethod(typeof(String), typeof(Object));
                    var hash = (IDictionary<String, Object>)cast.Invoke(obj, null);

                    var json = new JsonObject();
                    hash.ForEach(kvp =>
                    {
                        var key = kvp.Key;
                        var value = Json.Serialize(kvp.Value, cfg.HashElement);
                        json.Add(key, value);
                    });

                    return json;
                }
                else if (cfg.IsList)
                {
                    var list = ((IEnumerable)obj).Cast<Object>();

                    var json = new JsonArray();
                    list.ForEach(value =>
                    {
                        var j_value = Json.Serialize(value, cfg.ListElement);
                        json.Add(j_value);
                    });

                    return json;
                }
                else if (cfg.IsObject)
                {
                    var json = new JsonObject();

                    cfg.Slots.ForEach(mi =>
                    {
                        // todo. respect styling and JsonPropertyAttribute::Name
                        var key = ((Func<String>)(() => { throw new NotImplementedException(); }));
                        var value = mi.GetValue(obj);
                        var j_value = Json.Serialize(value, mi);
                        json.Add(key, j_value);
                    });

                    return json;
                }
                else
                {
                    throw AssertionHelper.Fail();
                }
            }
        }
    }
}
