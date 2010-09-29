using System;
using System.Collections.Generic;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        public static TypeRule AfterDeserialize<T>(this TypeRule rule, Action<T> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, Action<Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, Action<Type, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static TypeRule AfterDeserialize<T>(this TypeRule rule, Func<T, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, Func<Object, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, Func<Type, Object, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static TypeRule AfterDeserialize<T>(this TypeRule rule, params Action<T>[] afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, params Action<Object>[] afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, params Action<Type, Object>[] afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static TypeRule AfterDeserialize<T>(this TypeRule rule, IEnumerable<Action<T>> afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, IEnumerable<Action<Object>> afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static TypeRule AfterDeserialize(this TypeRule rule, IEnumerable<Action<Type, Object>> afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }
    }
}
