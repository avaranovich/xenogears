using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Validators.Core;
using XenoGears.Functional;

namespace XenoGears.Formats.Validators.Lambda
{
    public static partial class LambdaValidators
    {
        [DebuggerNonUserCode]
        internal class LambdaTypeValidator : TypeValidator
        {
            public LambdaTypeValidator(Action<Object> logic)
            {
                logic.AssertNotNull();
                _logic = (_, o) => logic(o);
            }

            public LambdaTypeValidator(Action<Type, Object> logic)
            {
                logic.AssertNotNull();
                _logic = logic;
            }

            private readonly Action<Type, Object> _logic;
            public override void Validate(Type t, Object value) { _logic(t, value); }
        }

        public static TypeConfig AddValidator<T>(this TypeConfig config, Action<T> validator)
        {
            if (validator == null) return config;
            return config.AddValidator((t, o) => { typeof(T).IsAssignableFrom(t).AssertTrue(); validator(o.AssertCast<T>()); });
        }

        public static TypeConfig AddValidator(this TypeConfig config, Action<Object> validator)
        {
            if (validator == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaTypeValidator), () => new List<LambdaTypeValidator>()).AssertCast<List<LambdaTypeValidator>>();
            validators.Add(new LambdaTypeValidator(validator));
            return config;
        }

        public static TypeConfig AddValidator(this TypeConfig config, Action<Type, Object> validator)
        {
            if (validator == null) return config;
            var validators = config.Hash.GetOrCreate(typeof(LambdaTypeValidator), () => new List<LambdaTypeValidator>()).AssertCast<List<LambdaTypeValidator>>();
            validators.Add(new LambdaTypeValidator(validator));
            return config;
        }

        public static TypeConfig AddValidators<T>(this TypeConfig config, params Action<T>[] validators)
        {
            return config.AddValidators((IEnumerable<Action<T>>)validators);
        }

        public static TypeConfig AddValidators(this TypeConfig config, params Action<Object>[] validators)
        {
            return config.AddValidators((IEnumerable<Action<Object>>)validators);
        }

        public static TypeConfig AddValidators(this TypeConfig config, params Action<Type, Object>[] validators)
        {
            return config.AddValidators((IEnumerable<Action<Type, Object>>)validators);
        }

        public static TypeConfig AddValidators<T>(this TypeConfig config, IEnumerable<Action<T>> validators)
        {
            (validators ?? Seq.Empty<Action<T>>()).ForEach(validator => config.AddValidator(validator));
            return config;
        }

        public static TypeConfig AddValidators(this TypeConfig config, IEnumerable<Action<Object>> validators)
        {
            (validators ?? Seq.Empty<Action<Object>>()).ForEach(validator => config.AddValidator(validator));
            return config;
        }

        public static TypeConfig AddValidators(this TypeConfig config, IEnumerable<Action<Type, Object>> validators)
        {
            (validators ?? Seq.Empty<Action<Type, Object>>()).ForEach(validator => config.AddValidator(validator));
            return config;
        }
    }
}
