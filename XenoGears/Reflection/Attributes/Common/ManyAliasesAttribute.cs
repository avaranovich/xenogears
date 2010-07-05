using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XenoGears.Reflection.Attributes.Common
{
    public abstract class ManyAliasesAttribute : Attribute
    {
        public ReadOnlyCollection<String> Aliases { get; private set; }

        protected ManyAliasesAttribute(params String[] aliases)
        {
            var raw = (aliases ?? Enumerable.Empty<String>()).ToList();
            var parsed = new List<String>();
            foreach (var name in raw)
            {
                var sub_names = name.Split(',').Select(s => s.Trim());
                parsed.AddRange(sub_names);
            }

            if (parsed.Count() == 0) throw new Exception("Empty aliases lists are not allowed");
            Aliases = new ReadOnlyCollection<String>(parsed);
        }
    }
}