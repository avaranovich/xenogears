using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Collections;
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
            catch (ConfigException cex)
            {
                if (cex.Message != null) Out.WriteLine(cex.Message);
                Out.WriteLine();
                Banners.Help();
                return null;
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
                    Out.Write("Command line args are: ");
                    if (s_args.IsEmpty()) Out.WriteLine("empty");
                    else Out.WriteLine();
                    foreach (var arg in s_args) Out.WriteLine(arg);
                    if (s_args.IsNotEmpty()) Out.WriteLine();
                }

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

                if (IsVerbose) Out.WriteLine("Parsing named arguments...");
                var parsed_args = ParseArgs(named_args);
                if (IsVerbose) foreach (var kvp in parsed_args)
                {
                    var a = kvp.Key.Attr<ParamAttribute>();
                    var name = named_args.Keys.Single(k => a.Aliases.Contains(k));
                    var value = named_args[name];
                    Out.WriteLine("Parsed {0} => \"{1}\" as: {2}.",
                        name, value, kvp.Value == null ? "<null>" : kvp.Value.ToString());
                }

                if (shortcut_args.IsNotEmpty())
                {
                    if (IsVerbose) Out.WriteLine("Parsing shortcut args...");

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
                    }

                    if (parsed_shortcut_args == null) throw new ConfigException("Fatal error: cannot bind to any of shortcuts.");
                    else parsed_args.AddElements(parsed_shortcut_args);
                }

//                TargetDir = Path.GetFullPath(args[2]);
//                var slash = Path.DirectorySeparatorChar.ToString();
//                if (!TargetDir.EndsWith(slash)) TargetDir += slash;

                if (IsVerbose) Out.WriteLine("Parse completed.");
                parsed_args.ForEach(kvp =>
                {
                    if (IsVerbose) Out.WriteLine("Resolved %{0} as {1}.", kvp.Key.Name, kvp.Value.ToTrace());
                    kvp.Key.SetValue(this, kvp.Value, null);
                });

                throw new NotImplementedException();
            }
        }

        private ReadOnlyDictionary<PropertyInfo, Object> ParseArgs(Dictionary<String, String> kvps)
        {
            var props = this.GetType().GetProperties(BF.AllInstance);
            return kvps.Select(kvp =>
            {
                var p = props.SingleOrDefault(p1 => p1.Attr<ParamAttribute>().Aliases.Contains(kvp.Key));
                if (p == null) throw new ConfigException("Fatal error: unknown argument name \"{0}\"", kvp.Key);

                Func<String, Type, Object> parse = (s, t) =>
                {
                    throw new NotImplementedException();
                };

                Object value;
                try { value = parse(kvp.Value, p.PropertyType); }
                catch (Exception ex) { throw new ConfigException(ex, "Fatal error: failed to parse argument \"{0}\" with value \"{1}\"", kvp.Key, kvp.Value); }

                return Tuple.New(p, value);
            }).ToDictionary(t => t.Item1, t => t.Item2).ToReadOnly();
        }
    }
}