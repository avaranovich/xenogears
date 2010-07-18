using System;
using System.Diagnostics;

namespace XenoGears.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class AssemblyBuiltFromAttribute : Attribute
    {
        public String Repository { get; set; }
        public String Revision { get; set; }
        public String CodebaseHash { get; set; }

        public AssemblyBuiltFromAttribute(){}
        public AssemblyBuiltFromAttribute(String repository, String revision) { Repository = repository; Revision = revision; }
        public AssemblyBuiltFromAttribute(String repository, String revision, String codebaseHash) { Repository = repository; Revision = revision; CodebaseHash = codebaseHash; }
    }
}