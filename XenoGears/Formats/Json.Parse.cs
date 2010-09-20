using System;

namespace XenoGears.Formats
{
    // todo.
    // also understand inline comments!!

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

        public static Json Parse(String json)
        {
            throw new NotImplementedException();
        }
    }
}
