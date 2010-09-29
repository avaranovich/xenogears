using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Validators.Core;
using XenoGears.Functional;

namespace XenoGears.Formats.Validators.Lambda
{
    public static partial class LambdaValidators
    {
        internal class LambdaPropertyValidator : PropertyValidator
        {
            public LambdaPropertyValidator(Action<Object> logic)
            {
                logic.AssertNotNull();
                _logic = (_, o) => logic(o);
            }

            public LambdaPropertyValidator(Action<PropertyInfo, Object> logic)
            {
                logic.AssertNotNull();
                _logic = logic;
            }

            private readonly Action<PropertyInfo, Object> _logic;
            public override void Validate(PropertyInfo t, Object value) { _logic(t, value); }
        }

        public static PropertyConfig AddValidator<T>(this PropertyConfig config, Action<T> validator)
        {
            if (validator == null) return config;
            return config.AddValidator((pi, o) => { typeof(T).IsAssignableFrom(pi.PropertyType).AssertTrue(); validator(o.AssertCast<T>()); });
        }

        public static PropertyConfig AddValidator(this PropertyConfig config, Action<Object> validator)
        {
            if (validator == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaPropertyValidator), () => new List<LambdaPropertyValidator>()).AssertCast<List<LambdaPropertyValidator>>();
            validators.Add(new LambdaPropertyValidator(validator));
            return config;
        }

        public static PropertyConfig AddValidator(this PropertyConfig config, Action<PropertyInfo, Object> validator)
        {
            if (validator == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaPropertyValidator), () => new List<LambdaPropertyValidator>()).AssertCast<List<LambdaPropertyValidator>>();
            validators.Add(new LambdaPropertyValidator(validator));
            return config;
        }

        public static PropertyConfig AddValidators<T>(this PropertyConfig config, params Action<T>[] validators)
        {
            return config.AddValidators((IEnumerable<Action<T>>)validators);
        }

        public static PropertyConfig AddValidators(this PropertyConfig config, params Action<Object>[] validators)
        {
            return config.AddValidators((IEnumerable<Action<Object>>)validators);
        }

        public static PropertyConfig AddValidators(this PropertyConfig config, params Action<PropertyInfo, Object>[] validators)
        {
            return config.AddValidators((IEnumerable<Action<PropertyInfo, Object>>)validators);
        }

        public static PropertyConfig AddValidators<T>(this PropertyConfig config, IEnumerable<Action<T>> validators)
        {
            (validators ?? Seq.Empty<Action<T>>()).ForEach(validator => config.AddValidator(validator));
            return config;
        }

        public static PropertyConfig AddValidators(this PropertyConfig config, IEnumerable<Action<Object>> validators)
        {
            (validators ?? Seq.Empty<Action<Object>>()).ForEach(validator => config.AddValidator(validator));
            return config;
        }

        public static PropertyConfig AddValidators(this PropertyConfig config, IEnumerable<Action<PropertyInfo, Object>> validators)
        {
            (validators ?? Seq.Empty<Action<PropertyInfo, Object>>()).ForEach(validator => config.AddValidator(validator));
            return config;
        }
    }
}
