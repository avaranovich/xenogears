using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Strings;
using XenoGears.Traits.Dumpable;

namespace XenoGears.Exceptions
{
    [DebuggerNonUserCode]
    public abstract class BaseException : ApplicationException
    {
        [IncludeInMessage]
        public Guid Id { get; private set; }

        [IncludeInMessage]
        public abstract bool IsUnexpected { get; }

        protected BaseException()
            : this(null)
        {
        }

        protected BaseException(Exception innerException)
            : base(null, innerException)
        {
            Id = Guid.NewGuid();
        }

        public Dictionary<String, Object> Properties
        {
            get
            {
                var props = GetType().GetProperties(BF.All).Where(p => p.HasAttr<IncludeInMessageAttribute>());
                return props.ToDictionary(p => p.Name, p => p.GetValue(this, null));
            }
        }

        private int PPDepth
        {
            get
            {
                var frames = new StackTrace().GetFrames();
                return frames.Count(f => f.GetMethod() == typeof(BaseException).GetProperty("PrettyProperties", BF.All).GetGetMethod(true));
            }
        }

        // todo. respect DebuggerTypeProxy and DebuggerDisplay attributes
        public Dictionary<String, String> PrettyProperties
        {
            get
            {
                var props = GetType().GetProperties(BF.All).Where(p => p.HasAttr<IncludeInMessageAttribute>());
                var vals = props.ToDictionary(p => p.Name, p => p.GetValue(this, null));
                vals = vals.Where(kvp => kvp.Value != null).ToDictionary();

                var valStrings = new Dictionary<String, String>();
                foreach (var kvp in vals)
                {
                    var nativeList = kvp.Value is IEnumerable &&
                        !(kvp.Value is String) && !(kvp.Value is IDumpableAsText);
                    var list = nativeList ? ((IEnumerable)kvp.Value).Cast<Object>() : Enumerable.Empty<Object>();
                    list = kvp.Value.MkArray().Concat(list);

                    Func<Object, bool> isKvp = o => o != null &&
                        o.GetType().SameMetadataToken(typeof(KeyValuePair<,>));
                    Func<Object, Object> getKey = o =>
                        isKvp(o) ? o.GetType().GetProperty("Key").GetValue(o, null) : null;
                    Func<Object, Object> getValue = o =>
                        isKvp(o) ? o.GetType().GetProperty("Value").GetValue(o, null) : null;

                    Func<Object, String> valtos = o => null;
                    valtos = o => // hello recursion in lambdas
                    {
                        if (o is BaseException)
                        {
                            var rex = (BaseException)o;
                            var indent = new String(' ', PPDepth * 2);
                            return String.Format("{0}{1}ExceptionType: {2}{3}",
                                Environment.NewLine, indent, rex.GetType(), rex.Message);
                        }
                        else if (o is MemberInfo)
                        {
                            return ((MemberInfo)o).GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces);
                        }
                        else if (o is ICustomAttributeProvider)
                        {
                            return ((ICustomAttributeProvider)o).GetCSharpAttributesClause(ToCSharpOptions.InformativeWithNamespaces);
                        }
                        else if (isKvp(o))
                        {
                            return valtos(getValue(o));
                        }
                        else if (o is IDumpableAsText)
                        {
                            // todo. hack! think of something generic
                            if (o.GetType().Hierarchy().Any(t => t.GetMethods(BF.AllInstance).Any(m => m.Name == "ToDebugString")))
                            {
                                try
                                {
                                    return o.ToString();
                                }
                                // todo. fix the following scenario:
                                // 1) ex is BaseException
                                // 2) some [IncludeInMessage] prop value throws BaseException in ToString
                                // 3) that BaseException has reference to that prop value
                                // ...
                                // stack overflow
                                catch (Exception ex)
                                {
                                    return "<Exception: " + ex.GetType().Name + ">";
                                }
                            }

                            try
                            {
                                var dump = ((IDumpableAsText)o).DumpAsText();
                                if (dump.Contains(Environment.NewLine)) dump = Environment.NewLine + dump;
                                return dump;
                            }
                            // todo. fix the following scenario:
                            // 1) ex is BaseException
                            // 2) some [IncludeInMessage] prop value throws BaseException in ToString
                            // 3) that BaseException has reference to that prop value
                            // ...
                            // stack overflow
                            catch (Exception ex)
                            {
                                return "<Exception: " + ex.GetType().Name + ">";
                            }
                        }
                        else
                        {
                            return o == null ? "null" : ((Func<String>)(() =>
                            {
                                try
                                {
                                    return o.ToString();
                                }
                                // todo. fix the following scenario:
                                // 1) ex is BaseException
                                // 2) some [IncludeInMessage] prop value throws BaseException in ToString
                                // 3) that BaseException has reference to that prop value
                                // ...
                                // stack overflow
                                catch (Exception ex)
                                {
                                    return "<Exception: " + ex.GetType().Name + ">";
                                }
                            }))();
                        }
                    };

                    var index = 0;
                    foreach (var listEl in list)
                    {
                        if (index == 0)
                        {
                            valStrings.Add(kvp.Key, valtos(listEl));
                        }
                        else
                        {
                            var key = String.Format("{0}[{1}]", kvp.Key, index - 1);
                            if (isKvp(listEl))
                            {
                                var indent = new String(' ', PPDepth * 2);
                                var kvp_valtos = valtos(getValue(listEl));
                                kvp_valtos = kvp_valtos.Replace(Environment.NewLine + indent, Environment.NewLine + indent + "  ");

                                var valString = String.Format("{0}{1}Key: {2}{0}{1}Value: {3}",
                                    Environment.NewLine, indent, valtos(getKey(listEl)), kvp_valtos);
                                valStrings.Add(key, valString);
                            }
                            else
                            {
                                valStrings.Add(key, valtos(listEl));
                            }
                        }

                        ++index;
                    }
                }

                if (PPDepth > 1 && InnerException != null)
                {
                    if (!(InnerException is BaseException))
                    {
                        var indent = new String(' ', PPDepth * 2);
                        var valString = String.Format("{0}{1}Type: {2}{0}{1}Message: {3}",
                            Environment.NewLine, indent, InnerException.GetType(),
                            InnerException.Message);
                        valStrings.Add("InnerException", valString);
                    }
                    else
                    {
                        var indent = new String(' ', PPDepth * 2);
                        var valString = String.Format("{0}{1}ExceptionType: {2}{3}",
                            Environment.NewLine, indent, InnerException.GetType(),
                            InnerException.Message);
                        valStrings.Add("InnerException", valString);
                    }
                }

                return valStrings;
            }
        }

        public sealed override string Message
        {
            get
            {
                var indent = new String(' ', PPDepth * 2);
                var valStrings = PrettyProperties;
                var mix = valStrings.Select(kvp => String.Format("{0}{2}: {3}{1}", 
                    indent, Environment.NewLine, kvp.Key, kvp.Value));
                var @out = Environment.NewLine + mix.StringJoin(String.Empty);

                var nl = Environment.NewLine;
                var doubleNl = nl + nl;
                while (@out.Contains(doubleNl))
                {
                    @out = @out.Replace(doubleNl, nl);
                }

                return @out;
            }
        }
    }
}