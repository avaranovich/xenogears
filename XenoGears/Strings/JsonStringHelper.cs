using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XenoGears.Strings
{
    [DebuggerNonUserCode]
    public static class JsonStringHelper
    {
        private static Dictionary<char, String> _specialChars =
            new Dictionary<char, String>();

        static JsonStringHelper()
        {
            _specialChars['\0'] = @"\0";
            _specialChars['\b'] = @"\b";
            _specialChars['\t'] = @"\t";
            _specialChars['\n'] = @"\n";
            _specialChars['\v'] = @"\v";
            _specialChars['\f'] = @"\f";
            _specialChars['\r'] = @"\r";
            _specialChars['\"'] = "\\\"";
            _specialChars['\''] = @"\'";
            _specialChars['\\'] = @"\\";
        }

        public static String ToJsonString(this String s)
        {
            if (s == null) return "null";
            var sb = new StringBuilder();
            sb.Append("\"").Append(EscapeJsonString(s, '\'')).Append("\"");
            return sb.ToString();
        }

        private static String EscapeJsonString(String s, char allowedQuote)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c == allowedQuote)
                {
                    sb.Append(c);
                }
                else
                {
                    if (_specialChars.ContainsKey(c))
                    {
                        sb.Append(_specialChars[c]);
                    }
                    else
                    {
                        if (c < 0x0020 || c > 0x007f)
                        {
                            sb.Append(@"\u" + ((int)c).ToString("x4"));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static String FromJsonString(this String s)
        {
            if (s == null) return null;
            if (s == "null") return null;

            try
            {
                var matchSingleQuote = Regex.Match(s, "^'(?<string>.*)'$");
                if (matchSingleQuote.Success)
                {
                    return UnescapeJsonString(matchSingleQuote.Result("${string}"), '\'');
                }

                var matchDoubleQuote = Regex.Match(s, "^\"(?<string>.*)\"$");
                if (matchDoubleQuote.Success)
                {
                    return UnescapeJsonString(matchDoubleQuote.Result("${string}"), '\"');
                }
            }
            catch (Exception e)
            {
                throw new NotSupportedException(Environment.NewLine + String.Format(
                    "Deserialization error: Unexpected string literal format: {0}", s), e);
            }

            throw new NotSupportedException(Environment.NewLine + String.Format(
                "Deserialization error: Unexpected string literal format: {0}", s));
        }

        private static String UnescapeJsonString(String s, char disallowedQuote)
        {
            //ECMA-262 [7.8.4 String Literals] (p18)
            // 1) backslash + zero, bfnrtv, single and double quote, backslash
            // 2) \xff (two digits)
            // 3) \uffff (four digits)

            var sb = new StringBuilder();

            var i = 0;
            while (i < s.Length)
            {
                if (s[i] == '\\')
                {
                    var next = s.Length == i + 1 ? null : (char?)s[i + 1];
                    if (next == '0' || next == disallowedQuote || next == '\\' ||
                        next == 'b' || next == 'f' || next == 'n' ||
                        next == 'r' || next == 't' || next == 'v')
                    {
                        sb.Append(_specialChars
                            .Where(kvp => kvp.Value == "" + s[i] + s[i + 1])
                            .Single().Key);
                        i += 2;
                    }
                    else if (next == 'x')
                    {
                        if (s.Length < i + 4)
                        {
                            throw new NotSupportedException(Environment.NewLine + String.Format(
                                "'{0}' is an invalid escape sequence", s.Substring(i)));
                        }
                        else
                        {
                            try
                            {
                                var hex = s.Substring(i + 2, 2);
                                var escapedChar = (char)int.Parse(hex, NumberStyles.HexNumber);
                                sb.Append(escapedChar);
                                i += 4;
                            }
                            catch (Exception e)
                            {
                                throw new NotSupportedException(Environment.NewLine + String.Format(
                                    "'{0}' is an invalid escape sequence", s.Substring(i, 4)), e);
                            }
                        }
                    }
                    else if (next == 'u')
                    {
                        if (s.Length < i + 6)
                        {
                            throw new NotSupportedException(Environment.NewLine + String.Format(
                                "'{0}' is an invalid escape sequence", s.Substring(i)));
                        }
                        else
                        {
                            try
                            {
                                var hex = s.Substring(i + 2, 4);
                                var escapedChar = (char)int.Parse(hex, NumberStyles.HexNumber);
                                sb.Append(escapedChar);
                                i += 6;
                            }
                            catch (Exception e)
                            {
                                throw new NotSupportedException(Environment.NewLine + String.Format(
                                    "'{0}' is an invalid escape sequence", s.Substring(i, 6)), e);
                            }
                        }
                    }
                    else
                    {
                        throw new NotSupportedException(Environment.NewLine + String.Format(
                            "'{0}' is an invalid escape sequence", "" + s[i] + next));
                    }
                }
                else if (s[i] == disallowedQuote || s[i] == '\n' || s[i] == '\r' || s[i] == '\x2028' || s[i] == '\x2029')
                {
                    throw new NotSupportedException(Environment.NewLine + String.Format(
                        "'{0}' cannot be a part of string literal", s[i]));
                }
                else
                {
                    sb.Append(s[i++]);
                }
            }

            return sb.ToString();
        }
    }
}
