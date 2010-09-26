using System;
using System.Diagnostics;

namespace XenoGears.Web.Rest.Security
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class SecurityCodebaseAttribute : Attribute
    {
    }
}
