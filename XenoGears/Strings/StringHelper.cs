using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Functional;

namespace XenoGears.Strings
{
    [DebuggerNonUserCode]
    public static class StringHelper
    {
        public static String Slice(this String source, int fromInclusive)
        {
            if (source == null) return String.Empty;

            if (fromInclusive < 0) fromInclusive += source.Length;
            if (fromInclusive >= source.Length) return String.Empty;

            return source.Substring(fromInclusive);
        }

        public static String Slice(this String source, int fromInclusive, int toExclusive)
        {
            if (source == null) return String.Empty;

            if (fromInclusive < 0) fromInclusive += source.Length;
            if (toExclusive < 0) toExclusive += source.Length;

            if (fromInclusive >= source.Length) return String.Empty;
            if (fromInclusive >= toExclusive) return String.Empty;
            if (toExclusive >= source.Length) return source.Substring(fromInclusive);

            return source.Substring(fromInclusive, toExclusive - fromInclusive);
        }

        public static String[] SplitLines(this String s)
        {
            return s.Split(new []{Environment.NewLine, "\n", "\r", "\r\n"}, StringSplitOptions.None);
        }

        public static int NthIndexOf(this String s, String substring, int n)
        {
            if (n <= 0)
            {
                return -1;
            }
            else
            {
                var shift = 0;
                Enumerable.Range(1, n).ForEach(i => 
                {
                    var index = s.IndexOf(substring);
                    if (i != n)
                    {
                        shift += index + 3;
                        s = s.Substring(index + 3);
                    }
                });

                return shift + s.IndexOf(substring);
            }
        }

        public static String Capitalize(this String s)
        {
            if (s.IsNullOrEmpty())
            {
                return s;
            }
            else
            {
                return Char.ToUpper(s[0]) + new String(s.Skip(1).ToArray());
            }
        }

        public static String Uncapitalize(this String s)
        {
            if (s.IsNullOrEmpty())
            {
                return s;
            }
            else
            {
                return Char.ToLower(s[0]) + new String(s.Skip(1).ToArray());
            }
        }

        public static String Indent(this String s)
        {
            return s.Indent("    ");
        }

        public static String Indent(this String s, int indent)
        {
            return s.Indent(new String(' ', indent));
        }

        public static String Indent(this String s, String indent)
        {
            if (s == null) return indent;
            return s.SplitLines().Select(line => 
                line.IsNullOrEmpty() ? line : indent + line).StringJoin(Environment.NewLine);
        }

        public static int DetectExcessiveIndentation(this String s)
        {
            var statistic = s.SplitLines().Where(line => !line.IsNullOrEmpty())
                .Select(line => line.DetectIndentation());
            return statistic.Any() ? statistic.Min() : 0;
        }

        private static int DetectIndentation(this String line)
        {
            int numOfSpaces;
            for (numOfSpaces = 0; numOfSpaces < line.Length && line[numOfSpaces] == ' '; numOfSpaces++) ;
            return numOfSpaces;
        }

        // todo. handle escape sequences here as well
        public static String Quote(this String s)
        {
            return "\"" + s + "\"";
        }

        // todo. handle escape sequences here as well
        public static String Unquote(this String s)
        {
            if (s.Length >= 2)
            {
                if (s[0] == '\'' && s[s.Length - 1] == '\'')
                {
                    return s.Substring(1, s.Length - 2);
                }

                if (s[0] == '\"' && s[s.Length - 1] == '\"')
                {
                    return s.Substring(1, s.Length - 2);
                }
            }

            return s;
        }

        public static String Parenthesize(this String s)
        {
            return String.Format("({0})", s);
        }

        public static String ParenthesizeIf(this String s, bool p)
        {
            return p ? s.Parenthesize() : s;
        }

        public static String Unparenthesize(this String s)
        {
            if (s.StartsWith("(") && s.EndsWith(")"))
            {
//                return s.Skip(1).Reverse().Skip(1).Reverse().StringJoin();
                return s.Substring(1, s.Length - 2);
            }
            else
            {
                return s;
            }
        }

        public static String UnparenthesizeIf(this String s, bool p)
        {
            return p ? s.Unparenthesize() : s;
        }

        public static String QuoteBraces(this String s)
        {
            return s.Replace("{", "{{").Replace("}", "}}");
        }
    }
}