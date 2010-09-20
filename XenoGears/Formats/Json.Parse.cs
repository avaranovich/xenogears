using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Strings;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public static Json ParseOrDefault(String json)
        {
            return ParseOrDefault(json, null as Json);
        }

        public static Json ParseOrDefault(String json, Json @default)
        {
            return ParseOrDefault(json, () => @default);
        }

        public static Json ParseOrDefault(String json, Func<Json> @default)
        {
            try { return Parse(json); }
            catch { return (@default ?? (() => null))(); }
        }

        public static Json Parse(String s)
        {
            s = s ?? String.Empty;
            s = s.Replace(@"//.*$", "", RegexOptions.Multiline);
            s = s.Replace(@"/\*.*?\*/", "", RegexOptions.Singleline);
            s = s.Trim();
            s.AssertNotEmpty();

            var json = new Json();
            if (s[0] == '{')
            {
                var contents = s.AssertExtract(@"^\s*\{\s*(?<contents>.*?)\*\}\s*$");
                var parts = DisassembleJsonObject(contents).ToReadOnly();

                parts.ForEach(part =>
                {
                    var map1 = part.Parse(@"^""(?<key>.*?)"":(?<value>.*)$");
                    var map2 = part.Parse(@"^'(?<key>.*?)':(?<value>.*)$");
                    var map3 = part.Parse(@"^(?<key>.*?):(?<value>.*)$");
                    var map = (map1 ?? map2 ?? map3).AssertNotNull();

                    var key = map["key"].Trim();
                    var value = map["value"].Trim();
                    json.Add(key, Parse(value));
                });
            }
            else if (s[0] == '[')
            {
                var contents = s.AssertExtract(@"^\s*\{\s*(?<contents>.*?)\*\}\s*$");
                var parts = DisassembleJsonObject(contents).ToReadOnly();
                parts.ForEach((part, i) => json[i] = Parse(part));
            }
            else
            {
                var primitive = ((Func<Object>)(() =>
                {
                    if (s == "false") return false;
                    if (s == "true") return true;

                    if (s.Matches(@"^'.*'$"))
                    {
                        return s.FromJsonString();
                    }
                    else if (s.Matches("^\".*\"$"))
                    {
                        return s.FromJsonString();
                    }
                    else
                    {
                        int integer;
                        if (int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out integer))
                        {
                            return integer;
                        }

                        double floatingPoint;
                        if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out floatingPoint))
                        {
                            return floatingPoint;
                        }

                        // todo. maybe add custom converters, e.g. for datetime
                        // todo. maybe even integrate this with V8, but what for?
                        throw AssertionHelper.Fail();
                    }
                }))();

                json._my_primitive = primitive;
            }

            return json;
        }

        // Splits JSON lists and object definitions into structural parts.
        // String.Split method won't work for lists of objects.
        private static IEnumerable<String> DisassembleJsonObject(String s)
        {
            var brackBalance = 0;
            var braceBalance = 0;
            var squote = false;
            var dquote = false;
            var buffer = new StringBuilder();

            foreach (var c in s)
            {
                switch (c)
                {
                    case '[':
                        ++brackBalance;
                        break;
                    case ']':
                        --brackBalance;
                        break;
                    case '{':
                        ++braceBalance;
                        break;
                    case '}':
                        --braceBalance;
                        break;
                    case '\'':
                        squote = !squote;
                        break;
                    case '\"':
                        dquote = !dquote;
                        break;
                    default:
                        if (c == ',' && brackBalance == 0 && braceBalance == 0 && !squote && !dquote)
                        {
                            yield return buffer.ToString().Trim();
                            buffer = new StringBuilder();
                            continue;
                        }
                        break;
                }

                buffer.Append(c);
            }

            var lastElement = buffer.ToString().Trim();
            if (!String.IsNullOrEmpty(lastElement))
            {
                yield return lastElement;
            }
        }
    }
}
