using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.CommandLine.Annotations;
using XenoGears.CommandLine.Exceptions;
using XenoGears.CommandLine.Helpers;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Strings;

namespace XenoGears.CommandLine
{
    [Config, DebuggerNonUserCode]
    public abstract class CommandLineConfig
    {
        protected static CommandLineConfig Current
        {
            get
            {
                try
                {
                    var t = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
                    var ctor = t.GetConstructors(BF.All).Single(ci => ci.Params().SingleOrDefault2() == typeof(String));
                    return (CommandLineConfig)ctor.Invoke(new Object[]{Environment.CommandLine});
                }
                catch (ConfigException cex)
                {
                    if (cex.Message != null) Console.WriteLine(cex.Message);
                    Console.WriteLine();
                    Banners.Help();
                    return null;
                }
            }
        }

        public bool IsVerbose { get; private set; }
        protected CommandLineConfig(String commandLine)
        {
            // todo. implement this correctly
            var s_args = commandLine.Split(' ');

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

                    Console.WriteLine("Detected the \"/verbose\" switch, entering verbose mode.");
                    Console.WriteLine("Command line is {0}", commandLine.ToTrace());
                    Console.Write("Command line args are: ");
                    if (s_args.IsEmpty()) Console.WriteLine("empty");
                    else Console.WriteLine();
                    foreach (var arg in s_args) Console.WriteLine(arg);
                    if (s_args.IsNotEmpty()) Console.WriteLine();
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
                        if (m != null) Console.WriteLine("Parsed \"{0}\" as name/value pair: {1} => \"{2}\".", s_arg, name, value);
                        else Console.WriteLine("Parsed \"{0}\" as raw value.", s_arg);
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

                if (IsVerbose) Console.WriteLine("Parsing named arguments...");
                var parsed_named_args = ParseArgs(named_args);
                if (IsVerbose) foreach (var kvp in parsed_named_args)
                {
                    var a = kvp.Key.Attr<ParamAttribute>();
                    var name = named_args.Keys.Single(k => a.Aliases.Contains(k));
                    var value = named_args[name];
                    Console.WriteLine("Parsed {0} => \"{1}\" as: {2}.",
                        name, value, kvp.Value == null ? "<null>" : kvp.Value.ToString());
                }

                if (shortcut_args.IsNotEmpty())
                {
                    if (IsVerbose) Console.WriteLine("Parsing shortcut args...");

                    Dictionary<PropertyInfo, String> parsed_shortcut_args = null;
                    var shortcuts = GetType().Attrs<ShortcutAttribute>().OrderBy(shortcut => shortcut.Priority);
                    foreach (var shortcut in shortcuts)
                    {
                        if (IsVerbose) Console.WriteLine("Considering shortcut schema \"{0}\"...", shortcut.Shortcut);
                    }
                }

                // if isverbose, then emit parsing config...
                // after parsing emit newline

//                if (IsVerbose) Console.WriteLine("Resolved %ProjectName as {0}.", ProjectName.ToTrace());

//                TargetDir = Path.GetFullPath(args[2]);
//                var slash = Path.DirectorySeparatorChar.ToString();
//                if (!TargetDir.EndsWith(slash)) TargetDir += slash;

                if (IsVerbose) Console.WriteLine("Parse completed.");
                throw new NotImplementedException();
            }
        }

        private Dictionary<PropertyInfo, Object> ParseArgs(Dictionary<String, String> kvps)
        {
            throw new NotImplementedException();
        }
    }
}