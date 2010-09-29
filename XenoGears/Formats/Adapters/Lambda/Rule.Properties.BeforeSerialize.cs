using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        public static PropertyRule BeforeSerialize<T>(this PropertyRule rule, Action<T> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, Action<Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, Action<PropertyInfo, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static PropertyRule BeforeSerialize<T>(this PropertyRule rule, Func<T, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, Func<Object, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, Func<PropertyInfo, Object, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static PropertyRule BeforeSerialize<T>(this PropertyRule rule, params Action<T>[] beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, params Action<Object>[] beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, params Action<PropertyInfo, Object>[] beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static PropertyRule BeforeSerialize<T>(this PropertyRule rule, IEnumerable<Action<T>> beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, IEnumerable<Action<Object>> beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static PropertyRule BeforeSerialize(this PropertyRule rule, IEnumerable<Action<PropertyInfo, Object>> beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }
    }
}
