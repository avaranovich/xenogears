using System;
using System.Reflection;
using XenoGears.Formats.Configuration;

namespace XenoGears.Formats.Engines.Lambda
{
    public static partial class LambdaEngines
    {
        public static PropertyRule Engine<T>(this PropertyRule rule, Func<Json, T> deserialize, Func<T, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static PropertyRule Engine(this PropertyRule rule, Func<PropertyInfo, Json, Object> deserialize, Func<PropertyInfo, Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static PropertyRule Engine(this PropertyRule rule, Func<Json, Object> deserialize, Func<PropertyInfo, Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static PropertyRule Engine(this PropertyRule rule, Func<PropertyInfo, Json, Object> deserialize, Func<Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static PropertyRule Engine(this PropertyRule rule, Func<Json, Object> deserialize, Func<Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }
    }
}
