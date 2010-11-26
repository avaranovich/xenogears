using System;
using System.Diagnostics;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Web.Urls
{
    [DebuggerNonUserCode]
    public static class UrlTrait
    {
        public static ReadOnlyDictionary<String, String> Parse(this Url url, String template)
        {
            return new ParsedUrl(url, template).Match;
        }
    }
}