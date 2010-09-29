﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public class PropertyRule : Rule
    {
        public Func<PropertyInfo, bool> Filter { get; private set; }
        public PropertyRule(Func<PropertyInfo, bool> filter, bool isAdhoc) : base(filter, isAdhoc) { Filter = filter; }

        private readonly List<Action<PropertyConfig>> _clauses = new List<Action<PropertyConfig>>();
        public List<Action<PropertyConfig>> Clauses { get { return _clauses; } }
        internal PropertyRule Record(Action<PropertyConfig> change) { Clauses.Add(change); return this; }
        internal PropertyRule Record<T>(Func<PropertyConfig, T> change) { Clauses.Add(cfg => change(cfg)); return this; }

        public void Apply()
        {
            var configs = Repository.Configs.Where(kvp => kvp.Key is PropertyInfo && Filter((PropertyInfo)kvp.Key)).Select(kvp => kvp.Value).ToReadOnly();
            configs.ForEach(config => Apply(config.AssertCast<PropertyConfig>()));
        }

        private readonly Dictionary<PropertyConfig, int> _log = new Dictionary<PropertyConfig, int>();
        public void Apply(PropertyConfig config)
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