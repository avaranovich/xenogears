using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        [DebuggerNonUserCode]
        internal class LambdaBeforeSerializePropertyAdapter : PropertyAdapter
        {
            public LambdaBeforeSerializePropertyAdapter(Func<PropertyInfo, Object, Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => beforeSerialize(pi, o));
            }

            public LambdaBeforeSerializePropertyAdapter(Func<Object, Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => beforeSerialize(o));
            }

            public LambdaBeforeSerializePropertyAdapter(Action<PropertyInfo, Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => { beforeSerialize(pi, o); return o; });
            }

            public LambdaBeforeSerializePropertyAdapter(Action<Object> beforeSerialize)
            {
                _beforeSerialize = beforeSerialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => { beforeSerialize(o); return o; });
            }

            private readonly Func<PropertyInfo, Object, Object> _beforeSerialize;
            public override Object AfterDeserialize(PropertyInfo pi, Object value) { return value; }
            public override Object BeforeSerialize(PropertyInfo pi, Object value) { return _beforeSerialize(pi, value); }
        }

        public static PropertyConfig BeforeSerialize<T>(this PropertyConfig config, Action<T> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            return config.BeforeSerialize((pi, o) => beforeSerialize(o.AssertCast<T>()), weight);
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, Action<Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializePropertyAdapter), () => new List<LambdaBeforeSerializePropertyAdapter>()).AssertCast<List<LambdaBeforeSerializePropertyAdapter>>();
            validators.Add(new LambdaBeforeSerializePropertyAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, Action<PropertyInfo, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializePropertyAdapter), () => new List<LambdaBeforeSerializePropertyAdapter>()).AssertCast<List<LambdaBeforeSerializePropertyAdapter>>();
            validators.Add(new LambdaBeforeSerializePropertyAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static PropertyConfig BeforeSerialize<T>(this PropertyConfig config, Func<T, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            return config.BeforeSerialize((pi, o) => beforeSerialize(o.AssertCast<T>()), weight);
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, Func<Object, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializePropertyAdapter), () => new List<LambdaBeforeSerializePropertyAdapter>()).AssertCast<List<LambdaBeforeSerializePropertyAdapter>>();
            validators.Add(new LambdaBeforeSerializePropertyAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, Func<PropertyInfo, Object, Object> beforeSerialize, double weight = 1.0)
        {
            if (beforeSerialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaBeforeSerializePropertyAdapter), () => new List<LambdaBeforeSerializePropertyAdapter>()).AssertCast<List<LambdaBeforeSerializePropertyAdapter>>();
            validators.Add(new LambdaBeforeSerializePropertyAdapter(beforeSerialize){Weight = weight});
            return config;
        }

        public static PropertyConfig BeforeSerialize<T>(this PropertyConfig config, params Action<T>[] beforeSerializes)
        {
            return config.BeforeSerialize((IEnumerable<Action<T>>)beforeSerializes);
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, params Action<Object>[] beforeSerializes)
        {
            return config.BeforeSerialize((IEnumerable<Action<Object>>)beforeSerializes);
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, params Action<PropertyInfo, Object>[] beforeSerializes)
        {
            return config.BeforeSerialize((IEnumerable<Action<PropertyInfo, Object>>)beforeSerializes);
        }

        public static PropertyConfig BeforeSerialize<T>(this PropertyConfig config, IEnumerable<Action<T>> beforeSerializes)
        {
            (beforeSerializes ?? Seq.Empty<Action<T>>()).ForEach(beforeSerialize => config.BeforeSerialize(beforeSerialize));
            return config;
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, IEnumerable<Action<Object>> beforeSerializes)
        {
            (beforeSerializes ?? Seq.Empty<Action<Object>>()).ForEach(beforeSerialize => config.BeforeSerialize(beforeSerialize));
            return config;
        }

        public static PropertyConfig BeforeSerialize(this PropertyConfig config, IEnumerable<Action<PropertyInfo, Object>> beforeSerializes)
        {
            (beforeSerializes ?? Seq.Empty<Action<PropertyInfo, Object>>()).ForEach(beforeSerialize => config.BeforeSerialize(beforeSerialize));
            return config;
        }
    }
}
