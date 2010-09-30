using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Formats.Grammar;
using XenoGears.Functional;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public static dynamic ParseOrDefault(String json)
        {
            return ParseOrDefault(json, null as Json);
        }

        public static dynamic ParseOrDefault(String json, Object @default)
        {
            return ParseOrDefault(json, () => @default);
        }

        public static dynamic ParseOrDefault(String json, Func<Object> @default)
        {
            try { return Parse(json); }
            catch { return new Json((@default ?? (() => null))()); }
        }

        public static dynamic Parse(String s)
        {
            // todo. strictly speaking, this should be inserted directly into lexer
            // and exposed via JsonReader configuration (similarly to AllowComments and AllowSingleQuotedStrings)
            // however, I'm too feeble to manually tinker lexer's FSM: http://litjson.sourceforge.net/doc/manual.html#appendix.parser.lexer.table
            s = QuotePropertyNames(s ?? String.Empty);

            var reader = new JsonReader(s);
            if (!reader.Read()) throw new JsonException("Input string is empty");
            return ReadJson(reader);
        }

        #region JSON preprocessors

        public static String QuotePropertyNames(String s)
        {
            var targets = DetectInsertionTargets(s);
            var buf = new StringBuilder();
            for (var i = 0; i < s.Length; ++i)
            {
                if (targets.Contains(i)) buf.Append("\"");
                buf.Append(s[i]);
            }

            return buf.ToString();
        }

        private static HashSet<int> DetectInsertionTargets(String s)
        {
            HashSet<int> strings, comments;
            DetectEnclaves(s, out strings, out comments);

//            Func<HashSet<int>, List<String>> visualize = enclave =>
//            {
//                var viz = new List<String>();
//                var l_enclave = enclave.ToList();
//                var word = new StringBuilder();
//                for (var i = 0; i < l_enclave.Count(); ++i)
//                {
//                    var p = i == 0 ? int.MinValue : l_enclave[i - 1];
//                    var c = l_enclave[i];
//
//                    if (p + 1 == c)
//                    {
//                        word.Append(s[c]);
//                    }
//                    else
//                    {
//                        if (word.Length > 0) viz.Add(word.ToString());
//                        word = new StringBuilder();
//                        word.Append(s[c]);
//                    }
//                }
//
//                if (word.Length > 0) viz.Add(word.ToString());
//                return viz;
//            };
//            var s_strings = visualize(strings);
//            var s_comments = visualize(comments);

            var iofs = s.IndicesOf(':').Where(i => !strings.Contains(i) && !comments.Contains(i)).ToReadOnly();
            var targets = new List<int>();
//            var unquoted_keys = new List<String>();
            iofs.ForEach(iof =>
            {
                int start = -1, end = -1;
                for (var i = iof - 1; i > 0; --i)
                {
                    var p = i == 0 ? '\0' : s[i - 1];
                    var c = s[i];
                    var n = i == s.Length - 1 ? '\0' : s[i + 1];

                    if (Char.IsWhiteSpace(c) || comments.Contains(i))
                    {
                        if (end == -1) continue;
                        else
                        {
                            start = i + 1;
                            break;
                        }
                    }
                    else if (strings.Contains(i))
                    {
                        start = i + 1;
                        break;
                    }
                    else
                    {
                        if (end == -1) end = i;

                        var is_letter = Char.IsLetter(c);
                        var is_digit = Char.IsDigit(c);
                        var is_underscore = c == '_';
                        var is_dollar = c == '$';
                        var is_escape = c == '\\' && n == 'u';
                        if (is_letter || is_digit || is_underscore || is_dollar || is_escape)
                        {
                            continue;
                        }
                        else
                        {
                            start = i + 1;
                            break;
                        }
                    }
                }

                if (start != -1 && end != -1)
                {
//                    unquoted_keys.Add(s.Slice(start, end + 1));
                    targets.Add(start);
                    targets.Add(end + 1);
                }
            });

            return targets.ToHashSet();
        }

        private static void DetectEnclaves(String s, out HashSet<int> strings, out HashSet<int> comments)
        {
            strings = new HashSet<int>();
            comments = new HashSet<int>();

            const int normal = 0, squo = 1, dquo = 2, sline = 3, mline = 4;
            var escape = false;
            var state = normal;
            for (var i = 0; i < s.Length; ++i)
            {
                var p = i == 0 ? '\0' : s[i - 1];
                var c = s[i];
                var n = i == s.Length - 1 ? '\0' : s[i + 1];

                if (state == normal)
                {
                    if (c == '\'')
                    {
                        state = squo;
                        escape = false;
                        strings.Add(i);
                    }
                    else if (c == '\"')
                    {
                        state = dquo;
                        escape = false;
                        strings.Add(i);
                    }
                    else if (c == '/' && n == '/')
                    {
                        state = sline;
                        escape = false;
                        comments.Add(i);
                        i++;
                        comments.Add(i);
                    }
                    else if (c == '/' && n == '*')
                    {
                        state = mline;
                        escape = false;
                        comments.Add(i);
                        i++;
                        comments.Add(i);
                    }
                    else
                    {
                        state = normal;
                    }
                }
                else if (state == squo)
                {
                    if (escape)
                    {
                        state = squo;
                        escape = false;
                        strings.Add(i);
                    }
                    else
                    {
                        if (c == '\'')
                        {
                            state = normal;
                            escape = false;
                            strings.Add(i);
                        }
                        else if (c == '\\')
                        {
                            state = squo;
                            escape = true;
                            strings.Add(i);
                        }
                        else
                        {
                            state = squo;
                            strings.Add(i);
                        }
                    }
                }
                else if (state == dquo)
                {
                    if (escape)
                    {
                        state = dquo;
                        escape = false;
                        strings.Add(i);
                    }
                    else
                    {
                        if (c == '\"')
                        {
                            state = normal;
                            escape = false;
                            strings.Add(i);
                        }
                        else if (c == '\\')
                        {
                            state = dquo;
                            escape = true;
                            strings.Add(i);
                        }
                        else
                        {
                            state = dquo;
                            strings.Add(i);
                        }
                    }
                }
                else if (state == sline)
                {
                    if (c == '\n')
                    {
                        state = normal;
                    }
                    else
                    {
                        state = sline;
                        comments.Add(i);
                    }
                }
                else if (state == mline)
                {
                    if (c == '*' && n == '/')
                    {
                        state = normal;
                        escape = false;
                        comments.Add(i);
                        i++;
                        comments.Add(i);
                    }
                    else
                    {
                        state = mline;
                        comments.Add(i);
                    }
                }
                else
                {
                    throw AssertionHelper.Fail();
                }
            }
        }

        #endregion

        #region JSON readers

        private static Json ReadJson(JsonReader reader)
        {
            switch (reader.Token)
            {
                case JsonToken.ObjectStart:
                    return ReadObject(reader);
                case JsonToken.ArrayStart:
                    return ReadArray(reader);
                case JsonToken.Int:
                case JsonToken.Long:
                case JsonToken.Double:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                    return ReadPrimitive(reader);
                case JsonToken.None:
                case JsonToken.PropertyName:
                case JsonToken.ObjectEnd:
                case JsonToken.ArrayEnd:
                default:
                    throw new JsonException(String.Format("Unexpected token {0} when reading json", reader.Token));
            }
        }

        private static JsonPrimitive ReadPrimitive(JsonReader reader)
        {
            return new JsonPrimitive(reader.Value);
        }

        private static JsonObject ReadObject(JsonReader reader)
        {
            (reader.Token == JsonToken.ObjectStart).AssertTrue();
            var json = new JsonObject();

            const int gief_key = 0, gief_value = 1;
            var state = gief_key;
            String key = null;
            while (reader.Read())
            {
                var end_of_object = false;
                switch (reader.Token)
                {
                    case JsonToken.PropertyName:
                        if (state != gief_key) throw new JsonException(String.Format("Unexpected token {0} when reading object in state {1}", reader.Token, state));
                        key = reader.Value.AssertCast<String>();
                        state = gief_value;
                        break;
                    case JsonToken.Int:
                    case JsonToken.Long:
                    case JsonToken.Double:
                    case JsonToken.String:
                    case JsonToken.Boolean:
                    case JsonToken.Null:
                    case JsonToken.ObjectStart:
                    case JsonToken.ArrayStart:
                        if (state != gief_value) throw new JsonException(String.Format("Unexpected token {0} when reading object in state {1}", reader.Token, state));
                        json.Add(key, ReadJson(reader));
                        state = gief_key;
                        break;
                    case JsonToken.ObjectEnd:
                        if (state != gief_key) throw new JsonException(String.Format("Unexpected end of object in state {0}", state));
                        end_of_object = true;
                        break;
                    case JsonToken.None:
                    case JsonToken.ArrayEnd:
                    default:
                        throw new JsonException(String.Format("Unexpected token {0} when reading object in state {1}", reader.Token, state));
                }

                if (end_of_object) break;
            }

            if (reader.Token != JsonToken.ObjectEnd) throw new JsonException(String.Format("Unexpected end of object at token {0} in state {1}", reader.Token, state));
            return json;
        }

        private static JsonArray ReadArray(JsonReader reader)
        {
            (reader.Token == JsonToken.ArrayStart).AssertTrue();
            var json = new JsonArray();

            while (reader.Read())
            {
                var end_of_array = false;
                switch (reader.Token)
                {
                    case JsonToken.Int:
                    case JsonToken.Long:
                    case JsonToken.Double:
                    case JsonToken.String:
                    case JsonToken.Boolean:
                    case JsonToken.Null:
                    case JsonToken.ObjectStart:
                    case JsonToken.ArrayStart:
                        json.Add(ReadJson(reader));
                        break;
                    case JsonToken.ArrayEnd:
                        end_of_array = true;
                        break;
                    case JsonToken.None:
                    case JsonToken.PropertyName:
                    case JsonToken.ObjectEnd:
                    default:
                        throw new JsonException(String.Format("Unexpected token {0} when reading array", reader.Token));
                }

                if (end_of_array) break;
            }

            if (reader.Token != JsonToken.ArrayEnd) throw new JsonException(String.Format("Unexpected end of array at token {0}", reader.Token));
            return json;
        }

        #endregion
    }
}
