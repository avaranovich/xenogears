using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;

namespace XenoGears.Web.Urls
{
    public class ParsedUrl
    {
        // todo. ensure that this instance is immutable
        public Url Url { get; private set; }
        public String Template { get; private set; }
        public ReadOnlyDictionary<String, String> Match { get; private set; }

        public ParsedUrl(Url url, String template)
        {
            Url = url.AssertNotNull().Clone();
            Template = template.AssertNotNull();
            Match = ParseUrl(url, template);
        }

        private static ReadOnlyDictionary<String, String> ParseUrl(String url, String template)
        {
            var buffer = new StringBuilder();
            Func<char, bool> isSpecial = c => ".?+*{}^$[](){}\\".IndexOf(c) != -1;

            var state = 0;
            var name = String.Empty;
            var keys = new List<String>();
            for (var i = 0; i < template.Length; ++i)
            {
                var curr = template[i];
                var next = i == template.Length - 1 ? '\0' : template[i + 1];

                if (state == 0)
                {
                    if (curr == '{' && next != '{')
                    {
                        name = String.Empty;
                        state = 1;
                    }
                    else
                    {
                        if (isSpecial(curr)) buffer.Append("\\");
                        buffer.Append(curr);
                    }
                }
                else if (state == 1)
                {
                    if (curr == '}' && next != '}')
                    {
                        buffer.AppendFormat("(?<{0}>.*?)", name);
                        keys.Add(name);
                        state = 0;
                    }
                    else
                    {
                        if (isSpecial(curr)) name += "\\";
                        name += curr;
                    }
                }
                else
                {
                    throw AssertionHelper.Fail();
                }
            }

            (state == 0).AssertTrue();
            var regex = String.Format("^{0}$", buffer);

            if (url == null) return null;
            var match = Regex.Match(url, regex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return keys.ToDictionary(key => key, key => match.Result("${" + key + "}")).ToReadOnly();
            }
            else
            {
                return null;
            }
        }
    }
}