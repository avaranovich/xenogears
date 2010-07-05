using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Logging;
using XenoGears.CommandLine.Annotations;
using XenoGears.CommandLine.Exceptions;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Strings;
using XenoGears.Reflection;
using XenoGears.CommandLine.Helpers;

namespace XenoGears.CommandLine
{
    [Config]
    [DebuggerNonUserCode]
    public abstract class CommandLineConfig
    {
        public static TextWriter Out { get { return Log.Out; } }

        protected static CommandLineConfig Current { get { return Parse(Environment.GetCommandLineArgs()); } }
        protected static CommandLineConfig Parse(IEnumerable<String> args) { return Parse((args ?? Seq.Empty<String>()).ToArray()); }
        protected static CommandLineConfig Parse(params String[] args)
        {
            try
            {
                var t = new StackTrace().GetFrames().Select(f =>
                {
                    var m = f.GetMethod();
                    var decl = m.DeclaringType;
                    while (decl != null && decl.IsCompilerGenerated()) decl = decl.DeclaringType;
                    return decl;
                }).AssertFirst(decl => decl != null && decl != typeof(CommandLineConfig));
                var ctor = t.GetConstructors(BF.All).Single(ci => ci.Params().SingleOrDefault2() == typeof(String[]));
                return (CommandLineConfig)ctor.Invoke(args.MkArray());
            }
            catch (TargetInvocationException tie)
            {
                var cex = tie.InnerException as ConfigException;
                if (cex != null)
                {
                    if (cex.Message != null) Out.WriteLine(cex.Message);
                    Out.WriteLine();
                    Banners.Help();
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool IsVerbose { get; private set; }
        protected CommandLineConfig(String[] s_args)
        {
            if (s_args.Count() == 1 && s_args[0] == "/?")
            {
                Banners.About();
                throw new ConfigException(String.Empty);
            }
            else
            {
                if (s_args.LastOrDefault() == "/verbose")
                {
                    IsVerbose = true;
                    s_args = s_args.SkipLast().ToArray();

                    Out.WriteLine("Detected the \"/verbose\" switch, entering verbose mode.");
                    Out.WriteLine("");
                    Out.Write("Command line args are: ");
                    if (s_args.IsEmpty()) Out.WriteLine("empty");
                    else Out.WriteLine("({0} arg{1})", s_args.Count(), s_args.Count() == 1 ? "" : "s");
                    s_args.ForEach((arg, i) => Out.WriteLine("{0}: {1}", i + 1, arg));
                }

                if (IsVerbose) Out.WriteLine();
                if (IsVerbose) Out.WriteLine("Pre-parsing arguments...");
                var named_args = new Dictionary<String, String>();
                var shortcut_args = new List<String>();
                foreach (var s_arg in s_args)
                {
                    var m = s_arg.Parse("^-(?<name>.*?):(?<value>.*)$");
                    var name = m != null ? m["name"] : null;
                    var value = m != null ? m["value"] : s_arg;
                    if (IsVerbose)
                    {
                        if (m != null) Out.WriteLine("Parsed \"{0}\" as name/value pair: {1} => \"{2}\".", s_arg, name, value);
                        else Out.WriteLine("Parsed \"{0}\" as raw value.", s_arg);
                    }

                    if (name == null)
                    {
                        if (named_args.IsNotEmpty()) throw new ConfigException("Fatal error: shortcut arguments must be specified before named arguments.");
                        else shortcut_args.Add(value);
                    }
                    else
                    {
                        if (named_args.ContainsKey(name)) throw new ConfigException("Fatal error: duplicate argument \"{0}\".", name);
                        else named_args.Add(name, value);
                    }
                }
                if (IsVerbose) Out.WriteLine("Pre-parse completed: found {0} named argument{1} and {2} shortcut argument{3}.",
                    named_args.Count(), named_args.Count() == 1 ? "" : "s",
                    shortcut_args.Count(), shortcut_args.Count() == 1 ? "" : "s");

                if (IsVerbose) Out.WriteLine();
                if (IsVerbose) Out.WriteLine("Parsing arguments...");

                var parsed_args = new Dictionary<PropertyInfo, Object>();
                if (named_args.IsNotEmpty())
                {
                    if (IsVerbose) Out.WriteLine("Parsing named arguments...");
                    parsed_args = ParseArgs(named_args);
                }

                if (shortcut_args.IsNotEmpty())
                {
                    if (IsVerbose) Out.WriteLine("Parsing shortcut arguments...");

                    Dictionary<PropertyInfo, Object> parsed_shortcut_args = null;
                    var shortcuts = GetType().Attrs<ShortcutAttribute>().OrderBy(shortcut => shortcut.Priority);
                    foreach (var shortcut in shortcuts)
                    {
                        if (IsVerbose) Out.WriteLine("Considering shortcut schema \"{0}\"...", shortcut.Schema);
                        var words = shortcut.Schema.SplitWords();
                        if (words.Count() != shortcut_args.Count())
                        {
                            if (IsVerbose) Out.WriteLine("Schema \"{0}\" won't work: argument count mismatch.", shortcut.Schema);
                            continue;
                        }

                        try { parsed_shortcut_args = ParseArgs(words.Zip(shortcut_args).ToDictionary(t => t.Item1, t => t.Item2)); }
                        catch (ConfigException cex)
                        {
                            if (IsVerbose)
                            {
                                Out.WriteLine(cex.Message);
                                Out.WriteLine("Schema \"{0}\" won't work: failed to parse arguments.", shortcut.Schema);
                            }

                            continue;
                        }

                        var dupes = Set.Intersect(parsed_args.Keys, parsed_shortcut_args.Keys);
                        if (dupes.IsNotEmpty())
                        {
                            var a = dupes.AssertFirst().Attr<ParamAttribute>();
                            var name = named_args.Keys.Single(k => a.Aliases.Contains(k));
                            if (IsVerbose) Out.WriteLine("Schema \"{0}\" won't work: shortcut argument duplicates parsed argument \"{1}\".", shortcut.Schema, name);
                            parsed_shortcut_args = null;
                            continue;
                        }
                        else
                        {
                            if (IsVerbose) Out.WriteLine("Schema \"{0}\" works fine, skipping other schemas (if any).", shortcut.Schema);
                            break;
                        }
                    }

                    if (parsed_shortcut_args == null) throw new ConfigException("Fatal error: cannot bind to any of shortcuts.");
                    else parsed_args.AddElements(parsed_shortcut_args);
                }

                if (IsVerbose) Out.WriteLine("Parse completed.");
                if (IsVerbose) Out.WriteLine("");
                if (IsVerbose) Out.WriteLine("Setting configuration parameters...");
                var props = this.GetType().GetProperties(BF.AllInstance).Where(p => p.HasAttr<ParamAttribute>()).OrderBy(p => p.Attr<ParamAttribute>().Priority);
                props.ForEach(p =>
                {
                    if (parsed_args.ContainsKey(p))
                    {
                        var value = parsed_args[p];
                        if (IsVerbose) Out.WriteLine("Resolved %{0} as {1}.", p.Name, value.ToTrace());
                        p.SetValue(this, value, null);
                    }
                    else
                    {
                        var p_default = this.GetType().GetProperty("Default" + p.Name, BF.AllStatic);
                        if (p_default == null) throw new ConfigException("Fatal error: parameter \"{0}\" must be specified.");

                        var value = p_default.GetValue(null, null);
                        if (IsVerbose) Out.WriteLine("Defaulted %{0} to {1}.", p.Name, value.ToTrace());
                        p.SetValue(this, value, null);
                    }
                });
                if (IsVerbose) Out.WriteLine("Configuration parameters successfully set.");
            }
        }

        private Dictionary<PropertyInfo, Object> ParseArgs(Dictionary<String, String> kvps)
        {
            var parsed_args = new Dictionary<PropertyInfo, Object>();
            kvps.ForEach(kvp =>
            {
                if (IsVerbose) Out.WriteLine("Resolving argument \"{0}\"...", kvp.Key);
                var props = this.GetType().GetProperties(BF.AllInstance).Where(p1 => p1.HasAttr<ParamAttribute>()).OrderBy(p1 => p1.Attr<ParamAttribute>().Priority);
                var p = props.SingleOrDefault(p1 => p1.Attr<ParamAttribute>().Aliases.Contains(kvp.Key));
                if (p == null) throw new ConfigException("Fatal error: unknown argument name \"{0}\".", kvp.Key);
                if (IsVerbose) Out.WriteLine("Resolved argument \"{0}\" as: {1}...", kvp.Key, p.GetCSharpRef(ToCSharpOptions.Informative));

                Func<String, Type, Object> parse = (s, t) =>
                {
                    if (t == typeof(FileInfo)) return new FileInfo(s);
                    if (t == typeof(DirectoryInfo)) return new DirectoryInfo(s);
                    return t.FromInvariantString(s);
                };

                if (IsVerbose) Out.WriteLine("Parsing {0} => \"{1}\" as {2}...", kvp.Key, kvp.Value.ToTrace(), p.PropertyType);
                Object value;
                try { value = parse(kvp.Value, p.PropertyType); }
                catch (Exception ex) { throw new ConfigException(ex, "Fatal error: failed to parse value \"{0}\" for argument \"{1}\".", kvp.Value.ToTrace(), kvp.Key); }
                if (IsVerbose) Out.WriteLine("Parsed {0} => \"{1}\" as: {2}.", kvp.Key, kvp.Value.ToTrace(), value.ToTrace());

                var m_validate = this.GetType().GetMethod("Validate" + p.Name, BF.AllStatic);
                if (m_validate != null)
                {
                    try
                    {
                        if (IsVerbose) Out.WriteLine("Validating {0} with {1}...", value.ToTrace(), m_validate.GetCSharpRef(ToCSharpOptions.Informative));
                        var is_valid = (bool)m_validate.Invoke(null, value.MkArray());
                        if (!is_valid) throw new ConfigException("Fatal error: value \"{0}\" is unsuitable for argument \"{1}\".", value.ToTrace(), kvp.Key);
                        if (IsVerbose) Out.WriteLine("Validated {0} as suitable for {1}.", value.ToTrace(), p.GetCSharpRef(ToCSharpOptions.Informative));
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigException(ex, "Fatal error: value \"{0}\" is unsuitable for argument \"{1}\".", value.ToTrace(), kvp.Key);
                    }
                }

                if (parsed_args.ContainsKey(p)) throw new ConfigException("Fatal error: duplicate argument \"{0}\".", kvp.Key);
                else { parsed_args.Add(p, value); }
            });

            return parsed_args;
        }
    }
}