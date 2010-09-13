using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;

namespace XenoGears.Strings
{
    [DebuggerNonUserCode]
    public static class RegexHelper
    {
        public static bool IsMatch(this String input, String pattern)
        {
            if (input == null) return false;
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsMatch(this String input, String pattern, RegexOptions options)
        {
            if (input == null) return false;
            return Regex.IsMatch(input, pattern, options);
        }

        public static Match Match(this String input, String pattern)
        {
            if (input == null) return null;
            return Regex.Match(input, pattern);
        }

        public static Match Match(this String input, String pattern, RegexOptions options)
        {
            if (input == null) return null;
            return Regex.Match(input, pattern, options);
        }

        public static ReadOnlyDictionary<String, String> Parse(this String input, String pattern)
        {
            return input.Parse(pattern, RegexOptions.None);
        }

        public static ReadOnlyDictionary<String, String> Parse(this String input, String pattern, RegexOptions options)
        {
            if (input == null) return null;

            var names = new List<String>();
            var m_meta = Regex.Match(pattern, @"\(\?\<(?<name>.*?)\>");
            for (; m_meta.Success; m_meta = m_meta.NextMatch())
            {
                var name = m_meta.Result("${name}");
                names.Add(name);
            }

            var m = input.Match(pattern, options);
            if (m == null || !m.Success) return null;
            return names.ToDictionary(name => name, name => m.Result("${" + name + "}")).ToReadOnly();
        }

        public static String Extract(this String input, String pattern)
        {
            return input.Extract(pattern, RegexOptions.None);
        }

        public static String Extract(this String input, String pattern, RegexOptions options)
        {
            var parsed = input.Parse(pattern);
            return parsed == null ? null : parsed.AssertSingle().Value;
        }

        public static String Replace(this String input, String pattern, Func<Match, String> replacer)
        {
            return input.Replace(pattern, replacer, RegexOptions.None);
        }

        public static String Replace(this String input, String pattern, RegexOptions options, Func<Match, String> replacer)
        {
            return input.Replace(pattern, replacer, options);
        }

        public static String Replace(this String input, String pattern, RegexOptions options, Func<ReadOnlyDictionary<String, String>, String> replacer)
        {
            return input.Replace(pattern, replacer, options);
        }

        public static String Replace(this String input, String pattern, Func<Match, String> replacer, RegexOptions options)
        {
            if (input == null) return null;
            return Regex.Replace(input, pattern, m => replacer(m), options);
        }

        public static String Replace(this String input, String pattern, Func<ReadOnlyDictionary<String, String>, String> replacer, RegexOptions options)
        {
            if (input == null) return null;

            var names = new List<String>();
            var m_meta = Regex.Match(pattern, @"\(\?\<(?<name>.*?)\>");
            for (; m_meta.Success; m_meta = m_meta.NextMatch())
            {
                var name = m_meta.Result("${name}");
                names.Add(name);
            }

            return Regex.Replace(input, pattern, m => replacer(names.ToDictionary(name => name, name => m.Result("${" + name + "}")).ToReadOnly()), options);
        }
    }
}
