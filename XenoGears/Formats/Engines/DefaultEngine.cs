using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Formats.Configuration.Default.Annotations;
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
using XenoGears.Reflection.Attributes;

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
                if (t.IsArray) obj = Enumerable.ToList((dynamic)obj);

                if (cfg.IsHash)
                {
                    var add = t.GetDictionarySetter().AssertNotNull();
                    (add.Param(0) == typeof(String)).AssertTrue();
                    var t_v = add.Param(1);

                    json.IsObject.AssertTrue();
                    json.Cast<String, Json>().ForEach(kvp =>
                    {
                        var key = kvp.Key;
                        // todo. what if the value passed has a subtype of t_v
                        // we won't be able to find out this and will deserialize incorrectly!
                        var value = kvp.Value.Deserialize(t_v);
                        add.Invoke(obj, new []{key, value});
                    });
                }
                else if (cfg.IsList)
                {
                    var add = t.GetListAdder().AssertNotNull();
                    var t_v = add.Param(0);

                    json.IsArray.AssertTrue();
                    json.Values.Cast<Json>().ForEach(j_value =>
                    {
                        // todo. what if the value passed has a subtype of t_v
                        // we won't be able to find out this and will deserialize incorrectly!
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
                        var mi = cfg.Slots.AssertSingle(s =>
                        {
                            var a_include = s.AttrOrNull<JsonIncludeAttribute>();
                            var a_key = a_include == null ? null : a_include.Name;
                            var name = a_key ?? s.Name;
                            return String.Compare(name, key, true) == 0;
                        });

                        var value = kvp.Value.Deserialize(mi);
                        mi.SetValue(obj, value);
                    });
                }
                else
                {
                    throw AssertionHelper.Fail();
                }

                if (t.IsArray) obj = Enumerable.ToArray((dynamic)obj);
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
                    var hash = (IDictionary<String, Object>)cast.Invoke(null, new[]{obj});

                    var json = new JsonObject();
                    hash.ForEach(kvp =>
                    {
                        var key = kvp.Key;
                        var value = Json.Serialize(kvp.Value);
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
                        var j_value = Json.Serialize(value);
                        json.Add(j_value);
                    });

                    return json;
                }
                else if (cfg.IsObject)
                {
                    var json = new JsonObject();

                    cfg.Slots.Where(mi => mi.CanRead()).ForEach(mi =>
                    {
                        var a_include = mi.AttrOrNull<JsonIncludeAttribute>();
                        var a_key = a_include == null ? null : a_include.Name;
                        var key = a_key ?? mi.Name.ToLower();
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
