using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using XenoGears.Strings;
using XenoGears.Assertions;

namespace XenoGears.Functional.Tuples
{
    [DebuggerNonUserCode]
    public static class TupleHelper
    {
        public static ReadOnlyCollection<Object> TupleItems(this Object tuple)
        {
            if (tuple == null) return null;

            var name = tuple.GetType().FullName.Extract("^(?<name>.*?)(`.*)?$");
            var tuple_name = typeof(Tuple).FullName;
            (name == tuple_name).AssertTrue();

            var p_items = tuple.GetType().GetProperties().Where(p => p.Name.StartsWith("Item")).OrderBy(p => int.Parse(p.Name.Substring("Item".Length))).ToReadOnly();
            return p_items.Select(p => p.GetValue(tuple, null)).ToReadOnly();
        }
    }
}
