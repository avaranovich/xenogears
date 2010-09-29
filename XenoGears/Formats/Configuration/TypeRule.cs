using System;
using System.Diagnostics;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public class TypeRule : Rule
    {
        public TypeRule(Func<Type, bool> type, bool isAdhoc) 
            : base(type, isAdhoc)
        {
        }
    }
}