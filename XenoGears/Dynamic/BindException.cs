using System;
using System.Diagnostics;
using System.Dynamic;
using XenoGears.Collections.Dictionaries;
using XenoGears.Exceptions;

namespace XenoGears.Dynamic
{
    [DebuggerNonUserCode]
    public class BindException : BaseException
    {
        [IncludeInMessage]
        public DynamicMetaObjectBinder Binder { get; private set; }

        [IncludeInMessage]
        public OrderedDictionary<String, Object> Args { get; private set; }

        public BindException(DynamicMetaObjectBinder binder, OrderedDictionary<String, Object> args, Exception innerException)
            : base(innerException)
        {
            Binder = binder;
            Args = args;
        }
    }
}