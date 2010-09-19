using System;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Web.Urls
{
    public static class UrlTrait
    {
        public static ReadOnlyDictionary<String, String> Parse(this Url url, String template)
        {
            return new ParsedUrl(url, template).Match;
        }
    }
}