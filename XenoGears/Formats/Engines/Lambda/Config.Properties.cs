using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Engines.Core;

namespace XenoGears.Formats.Engines.Lambda
{
    [DebuggerNonUserCode]
    public static partial class LambdaEngines
    {
        [DebuggerNonUserCode]
        internal class LambdaPropertyEngine : PropertyEngine
        {
            public LambdaPropertyEngine(Func<PropertyInfo, Json, Object> deserialize, Func<PropertyInfo, Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (pi, j) => deserialize(pi, j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(o, j);
            }

            public LambdaPropertyEngine(Func<Json, Object> deserialize, Func<PropertyInfo, Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (pi, j) => deserialize(j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(o, j);
            }

            public LambdaPropertyEngine(Func<PropertyInfo, Json, Object> deserialize, Func<Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (pi, j) => deserialize(pi, j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(j);
            }

            public LambdaPropertyEngine(Func<Json, Object> deserialize, Func<Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (pi, j) => deserialize(j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(j);
            }

            private readonly Func<PropertyInfo, Json, Object> _deserialize;
            public override Object Deserialize(PropertyInfo pi, Json json) { return _deserialize(pi, json); }
            private readonly Func<PropertyInfo, Object, Json> _serialize;
            public override Json Serialize(PropertyInfo pi, Object value) { return _serialize(pi, value); }
        }

        public static PropertyConfig Engine<T>(this PropertyConfig config, Func<Json, T> deserialize, Func<T, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            return config.Engine((pi, j) => deserialize(j).AssertCast<T>(), (pi, o) => serialize(o.AssertCast<T>()));
        }

        public static PropertyConfig Engine(this PropertyConfig config, Func<PropertyInfo, Json, Object> deserialize, Func<PropertyInfo, Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaPropertyEngine)] = new LambdaPropertyEngine(deserialize, serialize);
            return config;
        }

        public static PropertyConfig Engine(this PropertyConfig config, Func<Json, Object> deserialize, Func<PropertyInfo, Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaPropertyEngine)] = new LambdaPropertyEngine(deserialize, serialize);
            return config;
        }

        public static PropertyConfig Engine(this PropertyConfig config, Func<PropertyInfo, Json, Object> deserialize, Func<Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaPropertyEngine)] = new LambdaPropertyEngine(deserialize, serialize);
            return config;
        }

        public static PropertyConfig Engine(this PropertyConfig config, Func<Json, Object> deserialize, Func<Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaPropertyEngine)] = new LambdaPropertyEngine(deserialize, serialize);
            return config;
        }
    }
}
