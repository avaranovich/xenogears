using System;
using System.Diagnostics;
using XenoGears.Reflection.Attributes.Common;

namespace XenoGears.CommandLine.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class ParamAttribute : ManyAliasesAttribute
    {
        public int Priority { get; set; }
        public String Description { get; set; }

        public ParamAttribute(params String[] aliases)
            : base(aliases)
        {
        }
    }
}