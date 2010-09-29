using System;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Engines.Core;

namespace XenoGears.Formats.Engines.Lambda
{
    public static partial class LambdaEngines
    {
        [DebuggerNonUserCode]
        internal class LambdaTypeEngine : TypeEngine
        {
            public LambdaTypeEngine(Func<Type, Json, Object> deserialize, Func<Type, Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (t, j) => deserialize(t, j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(o, j);
            }

            public LambdaTypeEngine(Func<Json, Object> deserialize, Func<Type, Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (t, j) => deserialize(j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(o, j);
            }

            public LambdaTypeEngine(Func<Type, Json, Object> deserialize, Func<Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (t, j) => deserialize(t, j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(j);
            }

            public LambdaTypeEngine(Func<Json, Object> deserialize, Func<Object, Json> serialize)
            {
                deserialize.AssertNotNull();
                _deserialize = (t, j) => deserialize(j);
                serialize.AssertNotNull();
                _serialize = (o, j) => serialize(j);
            }

            private readonly Func<Type, Json, Object> _deserialize;
            public override Object Deserialize(Type t, Json json) { return _deserialize(t, json); }
            private readonly Func<Type, Object, Json> _serialize;
            public override Json Serialize(Type t, Object value) { return _serialize(t, value); }
        }

        public static TypeConfig Engine<T>(this TypeConfig config, Func<Json, T> deserialize, Func<T, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            return config.Engine((t, j) => { typeof(T).IsAssignableFrom(t).AssertTrue(); return deserialize(j).AssertCast<T>(); },
                (t, o) => { typeof(T).IsAssignableFrom(t).AssertTrue(); return serialize(o.AssertCast<T>()); });
        }

        public static TypeConfig Engine(this TypeConfig config, Func<Type, Json, Object> deserialize, Func<Type, Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaTypeEngine)] = new LambdaTypeEngine(deserialize, serialize);
            return config;
        }

        public static TypeConfig Engine(this TypeConfig config, Func<Json, Object> deserialize, Func<Type, Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaTypeEngine)] = new LambdaTypeEngine(deserialize, serialize);
            return config;
        }

        public static TypeConfig Engine(this TypeConfig config, Func<Type, Json, Object> deserialize, Func<Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaTypeEngine)] = new LambdaTypeEngine(deserialize, serialize);
            return config;
        }

        public static TypeConfig Engine(this TypeConfig config, Func<Json, Object> deserialize, Func<Object, Json> serialize)
        {
            if (deserialize == null || serialize == null) return config;
            config.Hash[typeof(LambdaTypeEngine)] = new LambdaTypeEngine(deserialize, serialize);
            return config;
        }
    }
}
