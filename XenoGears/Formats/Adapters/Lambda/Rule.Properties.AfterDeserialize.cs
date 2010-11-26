using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Formats.Configuration;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        public static PropertyRule AfterDeserialize<T>(this PropertyRule rule, Action<T> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, Action<Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, Action<PropertyInfo, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static PropertyRule AfterDeserialize<T>(this PropertyRule rule, Func<T, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, Func<Object, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, Func<PropertyInfo, Object, Object> afterDeserialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserialize, weight));
        }

        public static PropertyRule AfterDeserialize<T>(this PropertyRule rule, params Action<T>[] afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, params Action<Object>[] afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, params Action<PropertyInfo, Object>[] afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static PropertyRule AfterDeserialize<T>(this PropertyRule rule, IEnumerable<Action<T>> afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, IEnumerable<Action<Object>> afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }

        public static PropertyRule AfterDeserialize(this PropertyRule rule, IEnumerable<Action<PropertyInfo, Object>> afterDeserializes)
        {
            return rule.Record(cfg => cfg.AfterDeserialize(afterDeserializes));
        }
    }
}
