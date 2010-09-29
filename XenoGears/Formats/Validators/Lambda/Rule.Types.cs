using System;
using System.Collections.Generic;
using XenoGears.Formats.Configuration;

namespace XenoGears.Formats.Validators.Lambda
{
    public static partial class LambdaValidators
    {
        public static TypeRule AddValidator<T>(this TypeRule rule, Action<T> validator)
        {
            return rule.Record(cfg => cfg.AddValidator(validator));
        }

        public static TypeRule AddValidator(this TypeRule rule, Action<Object> validator)
        {
            return rule.Record(cfg => cfg.AddValidator(validator));
        }

        public static TypeRule AddValidator(this TypeRule rule, Action<Type, Object> validator)
        {
            return rule.Record(cfg => cfg.AddValidator(validator));
        }

        public static TypeRule AddValidators<T>(this TypeRule rule, params Action<T>[] validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static TypeRule AddValidators(this TypeRule rule, params Action<Object>[] validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static TypeRule AddValidators(this TypeRule rule, params Action<Type, Object>[] validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static TypeRule AddValidators<T>(this TypeRule rule, IEnumerable<Action<T>> validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static TypeRule AddValidators(this TypeRule rule, IEnumerable<Action<Object>> validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }

        public static TypeRule AddValidators(this TypeRule rule, IEnumerable<Action<Type, Object>> validators)
        {
            return rule.Record(cfg => cfg.AddValidators(validators));
        }
    }
}
