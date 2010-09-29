using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        internal class LambdaAfterDeserializePropertyAdapter : PropertyAdapter
        {
            public LambdaAfterDeserializePropertyAdapter(Func<PropertyInfo, Object, Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => afterDeserialize(pi, o));
            }

            public LambdaAfterDeserializePropertyAdapter(Func<Object, Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => afterDeserialize(o));
            }

            public LambdaAfterDeserializePropertyAdapter(Action<PropertyInfo, Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => { afterDeserialize(pi, o); return o; });
            }

            public LambdaAfterDeserializePropertyAdapter(Action<Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<PropertyInfo, Object, Object>)((pi, o) => o) : ((pi, o) => { afterDeserialize(o); return o; });
            }

            private readonly Func<PropertyInfo, Object, Object> _afterDeserialize;
            public override Object BeforeSerialize(PropertyInfo pi, Object value) { return value; }
            public override Object AfterDeserialize(PropertyInfo pi, Object value) { return _afterDeserialize(pi, value); }
        }

        public static PropertyConfig AfterDeserialize<T>(this PropertyConfig config, Action<T> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            return config.AfterDeserialize((pi, o) => { typeof(T).IsAssignableFrom(pi.PropertyType).AssertTrue(); afterDeserialize(o.AssertCast<T>()); }, weight);
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, Action<Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializePropertyAdapter), () => new List<LambdaAfterDeserializePropertyAdapter>()).AssertCast<List<LambdaAfterDeserializePropertyAdapter>>();
            validators.Add(new LambdaAfterDeserializePropertyAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, Action<PropertyInfo, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializePropertyAdapter), () => new List<LambdaAfterDeserializePropertyAdapter>()).AssertCast<List<LambdaAfterDeserializePropertyAdapter>>();
            validators.Add(new LambdaAfterDeserializePropertyAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static PropertyConfig AfterDeserialize<T>(this PropertyConfig config, Func<T, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            return config.AfterDeserialize((pi, o) => { typeof(T).IsAssignableFrom(pi.PropertyType).AssertTrue(); afterDeserialize(o.AssertCast<T>()); }, weight);
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, Func<Object, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializePropertyAdapter), () => new List<LambdaAfterDeserializePropertyAdapter>()).AssertCast<List<LambdaAfterDeserializePropertyAdapter>>();
            validators.Add(new LambdaAfterDeserializePropertyAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, Func<PropertyInfo, Object, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializePropertyAdapter), () => new List<LambdaAfterDeserializePropertyAdapter>()).AssertCast<List<LambdaAfterDeserializePropertyAdapter>>();
            validators.Add(new LambdaAfterDeserializePropertyAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static PropertyConfig AfterDeserialize<T>(this PropertyConfig config, params Action<T>[] afterDeserializes)
        {
            return config.AfterDeserialize((IEnumerable<Action<T>>)afterDeserializes);
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, params Action<Object>[] afterDeserializes)
        {
            return config.AfterDeserialize((IEnumerable<Action<Object>>)afterDeserializes);
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, params Action<PropertyInfo, Object>[] afterDeserializes)
        {
            return config.AfterDeserialize((IEnumerable<Action<PropertyInfo, Object>>)afterDeserializes);
        }

        public static PropertyConfig AfterDeserialize<T>(this PropertyConfig config, IEnumerable<Action<T>> afterDeserializes)
        {
            (afterDeserializes ?? Seq.Empty<Action<T>>()).ForEach(afterDeserialize => config.AfterDeserialize(afterDeserialize));
            return config;
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, IEnumerable<Action<Object>> afterDeserializes)
        {
            (afterDeserializes ?? Seq.Empty<Action<Object>>()).ForEach(afterDeserialize => config.AfterDeserialize(afterDeserialize));
            return config;
        }

        public static PropertyConfig AfterDeserialize(this PropertyConfig config, IEnumerable<Action<PropertyInfo, Object>> afterDeserializes)
        {
            (afterDeserializes ?? Seq.Empty<Action<PropertyInfo, Object>>()).ForEach(afterDeserialize => config.AfterDeserialize(afterDeserialize));
            return config;
        }
    }
}
