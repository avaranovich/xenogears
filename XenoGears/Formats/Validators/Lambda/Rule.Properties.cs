using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Functional;

namespace XenoGears.Formats.Validators.Lambda
{
    public static partial class LambdaValidators
    {
        public static PropertyRule AddValidator<T>(this PropertyRule rule, Action<T> validator)
        {
            return rule.Record(cfg => cfg.AddValidator(validator));
        }

        public static PropertyRule AddValidator(this PropertyRule rule, Action<Object> validator)
        {
            return rule.Record(cfg => cfg.AddValidator(validator));
        }

        public static PropertyRule AddValidator(this PropertyRule rule, Action<PropertyInfo, Object> validator)
        {
            return rule.Record(cfg => cfg.AddValidator(validator));
        }

        public static PropertyRule AddValidators<T>(this PropertyRule rule, params Action<T>[] validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static PropertyRule AddValidators(this PropertyRule rule, params Action<Object>[] validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static PropertyRule AddValidators(this PropertyRule rule, params Action<PropertyInfo, Object>[] validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static PropertyRule AddValidators<T>(this PropertyRule rule, IEnumerable<Action<T>> validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static PropertyRule AddValidators(this PropertyRule rule, IEnumerable<Action<Object>> validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static PropertyRule AddValidators(this PropertyRule rule, IEnumerable<Action<PropertyInfo, Object>> validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }
    }
}
