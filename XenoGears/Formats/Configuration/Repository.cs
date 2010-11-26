using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Configuration
{
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
