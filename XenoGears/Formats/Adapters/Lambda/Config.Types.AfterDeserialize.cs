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
        internal class LambdaAfterDeserializeTypeAdapter : TypeAdapter
        {
            public LambdaAfterDeserializeTypeAdapter(Func<Type, Object, Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => afterDeserialize(t, o));
            }

            public LambdaAfterDeserializeTypeAdapter(Func<Object, Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => afterDeserialize(o));
            }

            public LambdaAfterDeserializeTypeAdapter(Action<Type, Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => { afterDeserialize(t, o); return o; });
            }

            public LambdaAfterDeserializeTypeAdapter(Action<Object> afterDeserialize)
            {
                _afterDeserialize = afterDeserialize == null ? (Func<Type, Object, Object>)((t, o) => o) : ((t, o) => { afterDeserialize(o); return o; });
            }

            private readonly Func<Type, Object, Object> _afterDeserialize;
            public override Object BeforeSerialize(Type t, Object value) { return value; }
            public override Object AfterDeserialize(Type t, Object value) { return _afterDeserialize(t, value); }
        }

        public static TypeConfig AfterDeserialize<T>(this TypeConfig config, Action<T> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            return config.AfterDeserialize((t, o) => afterDeserialize(o.AssertCast<T>()), weight);
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, Action<Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializeTypeAdapter), () => new List<LambdaAfterDeserializeTypeAdapter>()).AssertCast<List<LambdaAfterDeserializeTypeAdapter>>();
            validators.Add(new LambdaAfterDeserializeTypeAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, Action<Type, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializeTypeAdapter), () => new List<LambdaAfterDeserializeTypeAdapter>()).AssertCast<List<LambdaAfterDeserializeTypeAdapter>>();
            validators.Add(new LambdaAfterDeserializeTypeAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static TypeConfig AfterDeserialize<T>(this TypeConfig config, Func<T, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            return config.AfterDeserialize((t, o) => afterDeserialize(o.AssertCast<T>()), weight);
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, Func<Object, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializeTypeAdapter), () => new List<LambdaAfterDeserializeTypeAdapter>()).AssertCast<List<LambdaAfterDeserializeTypeAdapter>>();
            validators.Add(new LambdaAfterDeserializeTypeAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, Func<Type, Object, Object> afterDeserialize, double weight = 1.0)
        {
            if (afterDeserialize == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaAfterDeserializeTypeAdapter), () => new List<LambdaAfterDeserializeTypeAdapter>()).AssertCast<List<LambdaAfterDeserializeTypeAdapter>>();
            validators.Add(new LambdaAfterDeserializeTypeAdapter(afterDeserialize){Weight = weight});
            return config;
        }

        public static TypeConfig AfterDeserialize<T>(this TypeConfig config, params Action<T>[] afterDeserializes)
        {
            return config.AfterDeserialize((IEnumerable<Action<T>>)afterDeserializes);
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, params Action<Object>[] afterDeserializes)
        {
            return config.AfterDeserialize((IEnumerable<Action<Object>>)afterDeserializes);
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, params Action<Type, Object>[] afterDeserializes)
        {
            return config.AfterDeserialize((IEnumerable<Action<Type, Object>>)afterDeserializes);
        }

        public static TypeConfig AfterDeserialize<T>(this TypeConfig config, IEnumerable<Action<T>> afterDeserializes)
        {
            (afterDeserializes ?? Seq.Empty<Action<T>>()).ForEach(afterDeserialize => config.AfterDeserialize(afterDeserialize));
            return config;
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, IEnumerable<Action<Object>> afterDeserializes)
        {
            (afterDeserializes ?? Seq.Empty<Action<Object>>()).ForEach(afterDeserialize => config.AfterDeserialize(afterDeserialize));
            return config;
        }

        public static TypeConfig AfterDeserialize(this TypeConfig config, IEnumerable<Action<Type, Object>> afterDeserializes)
        {
            (afterDeserializes ?? Seq.Empty<Action<Type, Object>>()).ForEach(afterDeserialize => config.AfterDeserialize(afterDeserialize));
            return config;
        }
    }
}
