using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Traits.Disposable;
using XenoGears.Assertions;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public static class Types
    {
        public static TypeConfig Config(this Type t)
        {
            return t.Adhoc();
        }

        public static TypeConfig Adhoc(this Type t)
        {
            if (t == null) return new TypeConfig(t);
            return Repository.Configs.GetOrCreate(t, () =>
            {
                var config = new TypeConfig(t);
                var rules = Repository.Rules.OfType<TypeRule>().Where(rule => rule.AppliesTo(t)).ToReadOnly();
                rules.ForEach(rule => rule.Apply(config));
                return config;
            }).AssertCast<TypeConfig>();
        }

        public static TypeRule Adhoc(this Func<Type, bool> t)
        {
            t.AssertNotNull();
            return new TypeRule(t);
        }

        public static TypeRule Rule(this Func<Type, bool> t)
        {
            t.AssertNotNull();
            IDisposable _;
            return Rule(t, out _);
        }

        public static TypeRule Rule(this Func<Type, bool> t, out IDisposable unreg)
        {
            t.AssertNotNull();
            var rule = new TypeRule(t);
            Repository.Rules.Add(rule);
            unreg = new DisposableAction(() => Repository.Rules.Remove(rule));
            return rule;
        }
    }
}
