using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Traits.Disposable;
using XenoGears.Assertions;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public static class Properties
    {
        public static PropertyConfig Config(this PropertyInfo pi)
        {
            return pi.Adhoc();
        }

        public static PropertyConfig Adhoc(this PropertyInfo pi)
        {
            if (pi == null) return new PropertyConfig(pi);
            pi = pi.DeclaringType.GetProperty(pi.Name, BF.All | BF.DeclOnly);
            return Repository.Configs.GetOrCreate(pi, () =>
            {
                var config = new PropertyConfig(pi);
                var rules = Repository.Rules.OfType<PropertyRule>().Where(rule => rule.AppliesTo(pi)).ToReadOnly();
                rules.ForEach(rule => rule.Apply(config));
                return config;
            }).AssertCast<PropertyConfig>();
        }

        public static PropertyRule Adhoc(this Func<PropertyInfo, bool> pi)
        {
            pi.AssertNotNull();
            return new PropertyRule(pi);
        }

        public static PropertyRule Rule(this Func<PropertyInfo, bool> pi)
        {
            pi.AssertNotNull();
            IDisposable _;
            return Rule(pi, out _);
        }

        public static PropertyRule Rule(this Func<PropertyInfo, bool> pi, out IDisposable unreg)
        {
            pi.AssertNotNull();
            var rule = new PropertyRule(pi);
            Repository.Rules.Add(rule);
            unreg = new DisposableAction(() => Repository.Rules.Remove(rule));
            return rule;
        }
    }
}
