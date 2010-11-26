using System;
using System.Collections.Generic;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Adapters.Lambda
{
    public static partial class LambdaAdapters
    {
        public static TypeRule BeforeSerialize<T>(this TypeRule rule, Action<T> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, Action<Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, Action<Type, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static TypeRule BeforeSerialize<T>(this TypeRule rule, Func<T, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, Func<Object, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, Func<Type, Object, Object> beforeSerialize, double weight = 1.0)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerialize, weight));
        }

        public static TypeRule BeforeSerialize<T>(this TypeRule rule, params Action<T>[] beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, params Action<Object>[] beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, params Action<Type, Object>[] beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static TypeRule BeforeSerialize<T>(this TypeRule rule, IEnumerable<Action<T>> beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, IEnumerable<Action<Object>> beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }

        public static TypeRule BeforeSerialize(this TypeRule rule, IEnumerable<Action<Type, Object>> beforeSerializes)
        {
            return rule.Record(cfg => cfg.BeforeSerialize(beforeSerializes));
        }
    }
}
