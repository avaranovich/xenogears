using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;

namespace XenoGears.Reflection.Anonymous
{
    [DebuggerNonUserCode]
    public static class AnonymousTypesHelper
    {
        public static Type ForgeAnonymousType(this IEnumerable<Tuple<String, Type>> schema)
        {
            var unzipped = schema.Unzip();
            return new AnonymousTypeBuilder(unzipped.Item1.ToArray(), unzipped.Item2.ToArray()).ToAnonymousType();
        }

        public static Type ForgeAnonymousType(this IOrderedDictionary<String, Type> schema)
        {
            return new AnonymousTypeBuilder(schema.Keys.ToArray(), schema.Values.ToArray()).ToAnonymousType();
        }
    }
}