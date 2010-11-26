using System;
using System.Collections.Generic;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Strings
{
    public static partial class ToStringHelper
    {
        public static String NatToAAA(this int index)
        {
            return (index - 1).SZtoAAA();
        }

        public static String SZtoAAA(this int index)
        {
            var powers = 1.Unfold(i => i * 26);
            var psums = powers.Skip(1).Scanbe(0, (psum1, power1, _) => power1 + psum1);

            (index >= 0).AssertTrue();
            var ndigits = psums.IndexOf(psum1 => psum1 > index);
            var psum = psums.Nth(ndigits - 1);
            var remnant = index - psum;

            var indices = powers.Take(ndigits).Scanrbe(remnant, (curr, power, _) => curr % power, (curr, dimSize, _) => curr / dimSize);
            return indices.Select(i => (char)('A' + i)).StringJoin("");
        }

        public static String GetStringLiteral(this String s)
        {
            return "\"" + s.Select(c => (c != '\\' && c != '\"') ? new String(c, 1) : "\\" + c)
                .StringJoin(String.Empty) + "\"";
        }

        public static String CSharpNameToHumanReadableString(this String s)
        {
            return s.CSharpNameToHumanReadableString(true);
        }

        public static String CSharpNameToHumanReadableString(this String s, bool capitalize)
        {
            return s.SplitCSharpNameIntoWords().Select((word, i) =>
            {
                if (i == 0)
                {
                    return (capitalize ? word.Capitalize() : word.Uncapitalize());
                }
                else
                {
                    return word.Any(Char.IsLower) ? word.Uncapitalize() : word;
                }
            }).StringJoin(" ");
        }

        private static IEnumerable<String> SplitCSharpNameIntoWords(this String s)
        {
            var word = String.Empty;
            foreach (var c in s)
            {
                if (c == '_')
                {
                    if (word.IsNotEmpty())
                    {
                        yield return word;
                        word = String.Empty;
                    }
                }
                else
                {
                    try
                    {
                        var shouldYieldWord =
                            (word.Any(Char.IsLower) && Char.IsUpper(c)) ||
                            (word.Count() > 1 && word.All(Char.IsUpper) && Char.IsLower(c));

                        if (shouldYieldWord)
                        {
                            yield return word;
                            word = String.Empty;
                        }
                    }
                    finally
                    {
                        word += c;
                    }
                }
            }

            if (word.IsNotEmpty()) yield return word;
        }
    }
}
