using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Configuration
{
    // todo. add rules support for lambda adapters, engines, validators
    // this implies moving BeforeSerialize, AfterDeserialize, Engine, Validators into separate class ala Default::FluentConfig
    // and introducing a separate Lambda namespace within Configuration namespace
    // that namespace should contain its own Gateway that unites all methods from all lambda stuff!

    // that Gateway should also apply all rules for newly created configs
    // todo. this leads to the conclusion that we must formalize all rules
    // so that application of rules can be done by repository classes: Types and Properties
    // e.g.: <repository>
    // config #1: hash = foo => FooConfig, bar => BarConfig
    // config #2: hash = foo => FooConfig
    // rule #1: hash = bar => BarRule
    // then:
    // rule #1 can be applied only to config #1 in fully automatic fashion ala "rule.Hash[bar].ApplyTo(config.Hash[bar])"
    // concrete implementation of ApplyTo will, of course, vary from configuration hive to configuration hive

    [DebuggerNonUserCode]
    internal static class Repository
    {
        public static Dictionary<MemberInfo, Config> Configs { get; private set; }
        public static List<Rule> Rules { get; private set; }

        static Repository()
        {
            Configs = new Dictionary<MemberInfo, Config>();
            Rules = new List<Rule>();
        }
    }
}
