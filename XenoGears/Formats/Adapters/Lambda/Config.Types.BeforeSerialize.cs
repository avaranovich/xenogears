using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        [DebuggerNonUserCode]
        internal class LambdaBeforeSerializeTypeAdapter : TypeAdapter
        {
            public LambdaBeforeSerializeTypeAdapter(Func<Type, Object, Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => beforeSerialize(t, o));
            }

            public LambdaBeforeSerializeTypeAdapter(Func<Object, Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => beforeSerialize(o));
            }

            public LambdaBeforeSerializeTypeAdapter(Action<Type, Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => { beforeSerialize(t, o); return o; });
            }

            public LambdaBeforeSerializeTypeAdapter(Action<Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => { beforeSerialize(o); return o; });
            }

            private readonly Func<Type, Object, Object> _beforeSerialize;
            public override Object AfterDeserialize(Type t, Object value) { return value; }
            public override Object BeforeSerialize(Type t, Object value) { return _beforeSerialize(t, value); }
        }

        public static TypeConfig BeforeSerialize<T>(this TypeConfig config, Action<T> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            return config.BeforeSerialize((t, o) => { typeof(T).IsAssignableFrom(t).AssertTrue(); beforeSerialize(o.AssertCast<T>()); }, weight);
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, Action<Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializeTypeAdapter), () => new List<LambdaBeforeSerializeTypeAdapter>()).AssertCast<List<LambdaBeforeSerializeTypeAdapter>>();
            validators.Add(new LambdaBeforeSerializeTypeAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, Action<Type, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializeTypeAdapter), () => new List<LambdaBeforeSerializeTypeAdapter>()).AssertCast<List<LambdaBeforeSerializeTypeAdapter>>();
            validators.Add(new LambdaBeforeSerializeTypeAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static TypeConfig BeforeSerialize<T>(this TypeConfig config, Func<T, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            return config.BeforeSerialize((t, o) => { typeof(T).IsAssignableFrom(t).AssertTrue(); beforeSerialize(o.AssertCast<T>()); }, weight);
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, Func<Object, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializeTypeAdapter), () => new List<LambdaBeforeSerializeTypeAdapter>()).AssertCast<List<LambdaBeforeSerializeTypeAdapter>>();
            validators.Add(new LambdaBeforeSerializeTypeAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, Func<Type, Object, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializeTypeAdapter), () => new List<LambdaBeforeSerializeTypeAdapter>()).AssertCast<List<LambdaBeforeSerializeTypeAdapter>>();
            validators.Add(new LambdaBeforeSerializeTypeAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static TypeConfig BeforeSerialize<T>(this TypeConfig config, params Action<T>[] beforeSerializes)
        {
            return config.BeforeSerialize((IEnumerable<Action<T>>)beforeSerializes);
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, params Action<Object>[] beforeSerializes)
        {
            return config.BeforeSerialize((IEnumerable<Action<Object>>)beforeSerializes);
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, params Action<Type, Object>[] beforeSerializes)
        {
            return config.BeforeSerialize((IEnumerable<Action<Type, Object>>)beforeSerializes);
        }

        public static TypeConfig BeforeSerialize<T>(this TypeConfig config, IEnumerable<Action<T>> beforeSerializes)
        {
            (beforeSerializes ?? Seq.Empty<Action<T>>()).ForEach(beforeSerialize => config.BeforeSerialize(beforeSerialize));
            return config;
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, IEnumerable<Action<Object>> beforeSerializes)
        {
            (beforeSerializes ?? Seq.Empty<Action<Object>>()).ForEach(beforeSerialize => config.BeforeSerialize(beforeSerialize));
            return config;
        }

        public static TypeConfig BeforeSerialize(this TypeConfig config, IEnumerable<Action<Type, Object>> beforeSerializes)
        {
            (beforeSerializes ?? Seq.Empty<Action<Type, Object>>()).ForEach(beforeSerialize => config.BeforeSerialize(beforeSerialize));
            return config;
        }
    }
}
