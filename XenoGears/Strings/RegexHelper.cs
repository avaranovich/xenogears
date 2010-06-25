using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdvancedMailMerge.Helpers.Assertions;

namespace AdvancedMailMerge.Helpers.Strings
{
    [DebuggerNonUserCode]
    internal static class RegexHelper
    {
        public static Match Match(this String input, String pattern)
        {
            return Regex.Match(input, pattern);
        }

        public static Dictionary<String, String> Parse(this String input, String pattern)
        {
            var names = new List<String>();
            var m_meta = Regex.Match(pattern, @"\(\?\<(?<name>.*?)\>");
            for (; m_meta.Success; m_meta = m_meta.NextMatch())
            {
                var name = m_meta.Result("${name}");
                names.Add(name);
            }

            var m = input.Match(pattern);
            if (!m.Success) return null;
            return names.ToDictionary(name => name, name => m.Result("${" + name + "}"));
        }

        public static String Extract(this String input, String pattern)
        {
            var parsed = input.Parse(pattern);
            return parsed == null ? null : parsed.AssertSingle().Value;
        }
    }
}
