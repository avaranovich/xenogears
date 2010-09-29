using System;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public class PropertyRule : Rule
    {
        public PropertyRule(Func<PropertyInfo, bool> property, bool isAdhoc) 
            : base(property, isAdhoc)
        {
        }
    }
}