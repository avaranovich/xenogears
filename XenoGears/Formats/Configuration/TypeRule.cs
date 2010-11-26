using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public class TypeRule : Rule
    {
        public new Func<Type, bool> Filter { get; set; }
        public bool AppliesTo(Type t) { return t != null && Filter(t); }
        public TypeRule(Func<Type, bool> filter) : base(filter) { Filter = filter; }

        private readonly List<Action<TypeConfig>> _clauses = new List<Action<TypeConfig>>();
        public List<Action<TypeConfig>> Clauses { get { return _clauses; } }
        internal TypeRule Record(Action<TypeConfig> change) { Clauses.Add(change); Apply(); return this; }
        internal TypeRule Record<T>(Func<TypeConfig, T> change) { Clauses.Add(cfg => change(cfg)); Apply(); return this; }

        public void Apply()
        {
            var configs = Repository.Configs.Where(kvp => kvp.Key is Type && Filter((Type)kvp.Key)).Select(kvp => kvp.Value).ToReadOnly();
            configs.ForEach(config => Apply(config.AssertCast<TypeConfig>()));
        }

        private readonly Dictionary<TypeConfig, int> _log = new Dictionary<TypeConfig, int>();
        public void Apply(TypeConfig config)
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