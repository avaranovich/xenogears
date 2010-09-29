using System;
using System.Reflection;
using XenoGears.Functional;
using XenoGears.Traits.Disposable;

namespace XenoGears.Formats.Configuration
{
    public static class PropertyConfig
    {
        public static Config Adhoc(this PropertyInfo pi)
        {
            return Repository.Configs.GetOrCreate(pi, () => new Config(pi));
        }

        public static Rule Adhoc(this Func<PropertyInfo, bool> pi)
        {
            return new Rule(pi, true);
        }

        public static Rule Rule(this Func<PropertyInfo, bool> pi)
        {
            IDisposable _;
            return Rule(pi, out _);
        }

        public static Rule Rule(this Func<PropertyInfo, bool> pi, out IDisposable unreg)
        {
            var rule = new Rule(pi, false);
            Repository.Rules.Add(rule);
            unreg = new DisposableAction(() => Repository.Rules.Remove(rule));
            return rule;
        }
    }
}
