﻿using System;
using System.Linq;
using XenoGears.Formats.Configuration.Default.Fluent;
using XenoGears.Functional;
using XenoGears.Assertions;

namespace XenoGears.Formats.Configuration.Default
{
    public static class Gateway
    {
        public static FluentConfig DefaultEngine(this Configuration.Config generic_config)
        {
            return generic_config.Hash.GetOrCreate(typeof(Gateway), () =>
            {
                var type = generic_config.Type;
                var new_config = new Config(type);
                var new_fluent_config = new FluentConfig(new_config);

                var fluent_rules = Repository.Rules.Where(rule => rule.AppliesTo(type)).Select(rule => rule.DefaultEngine()).ToReadOnly();
                var rules = fluent_rules.Select(rule => rule.Rule).ToReadOnly();
                rules.ForEach(rule => rule.Apply(new_fluent_config));

                return new_fluent_config;
            }).AssertCast<FluentConfig>();
        }

        public static FluentRule DefaultEngine(this Configuration.Rule generic_rule)
        {
            return generic_rule.Hash.GetOrCreate(typeof(Gateway), () =>
            {
                var new_rule = new Rule(generic_rule.AppliesTo);
                return new FluentRule(new_rule);
            }).AssertCast<FluentRule>();
        }
    }
}
