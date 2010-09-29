using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Assertions;
using System.Linq;
using XenoGears.Formats.Configuration.Default.Fluent;
using XenoGears.Functional;

namespace XenoGears.Formats.Configuration.Default
{
    [DebuggerNonUserCode]
    public class TypeRule
    {
        public Func<Type, bool> Filter { get; private set; }
        public TypeRule(Func<Type, bool> filter) { Filter = filter.AssertNotNull(); }

        private readonly List<Action<FluentConfig>> _clauses = new List<Action<FluentConfig>>();
        public List<Action<FluentConfig>> Clauses { get { return _clauses; } }

        public void Apply()
        {
            var generic_configs = Repository.Configs.Where(kvp => kvp.Key is Type && Filter((Type)kvp.Key)).Select(kvp => kvp.Value).ToReadOnly();
            var configs = generic_configs.Select(cfg => cfg.Hash.GetOrDefault(typeof(Gateway)).AssertCast<FluentConfig>()).Where(cfg => cfg != null).ToReadOnly();
            configs.ForEach(Apply);
        }

        private readonly Dictionary<FluentConfig, int> _log = new Dictionary<FluentConfig, int>();
        public void Apply(FluentConfig config)
        {
            config.AssertNotNull();
            var from = _log.GetOrDefault(config, -1) + 1;
            from.UpTo(_clauses.Count() - 1).ForEach(i =>
            {
                Clauses[i](config);
                _log[config] = i;
            });
        }
    }
}
