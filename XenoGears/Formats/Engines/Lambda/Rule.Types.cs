using System;
using XenoGears.Formats.Configuration;

namespace XenoGears.Formats.Engines.Lambda
{
    public static partial class LambdaEngines
    {
        public static TypeRule Engine<T>(this TypeRule rule, Func<Json, T> deserialize, Func<T, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static TypeRule Engine(this TypeRule rule, Func<Type, Json, Object> deserialize, Func<Type, Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static TypeRule Engine(this TypeRule rule, Func<Json, Object> deserialize, Func<Type, Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static TypeRule Engine(this TypeRule rule, Func<Type, Json, Object> deserialize, Func<Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }

        public static TypeRule Engine(this TypeRule rule, Func<Json, Object> deserialize, Func<Object, Json> serialize)
        {
            return rule.Record(cfg => cfg.Engine(deserialize, serialize));
        }
    }
}
