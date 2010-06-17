using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace XenoGears.Reflection.Anonymous
{
    [DebuggerNonUserCode]
    public static class AnonymousTypesHelper
    {
        public static Type ForgeAnonymousType(this IDictionary<String, Type> schema)
        {
            return new AnonymousTypeBuilder(schema.Keys.ToArray(), schema.Values.ToArray()).ToAnonymousType();
        }
    }
}