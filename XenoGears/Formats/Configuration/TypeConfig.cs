using System;
using XenoGears.Functional;
using XenoGears.Traits.Disposable;

namespace XenoGears.Formats.Configuration
{
    public static class TypeConfig
    {
        public static Config Adhoc(this Type t)
        {
            return Repository.Configs.GetOrCreate(t, () => new Config(t));
        }

        public static Rule Adhoc(this Func<Type, bool> t)
        {
            return new Rule(t, true);
        }

        public static Rule Rule(this Func<Type, bool> t)
        {
            IDisposable _;
            return Rule(t, out _);
        }

        public static Rule Rule(this Func<Type, bool> t, out IDisposable unreg)
        {
            var rule = new Rule(t, false);
            Repository.Rules.Add(rule);
            unreg = new DisposableAction(() => Repository.Rules.Remove(rule));
            return rule;
        }
    }
}
