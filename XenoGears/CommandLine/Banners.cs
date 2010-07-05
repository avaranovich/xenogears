using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using XenoGears.CommandLine.Annotations;
using XenoGears.Logging;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Functional;
using XenoGears.Assertions;
using XenoGears.Strings;

namespace XenoGears.CommandLine
{
    [DebuggerNonUserCode]
    public static class Banners
    {
        public static TextWriter Out { get { return Log.Out; } }

        public static void About()
        {
            var asm = Assembly.GetEntryAssembly();
            if (asm != null)
            {
                String s_about; using (var stream_about = asm.GetManifestResourceStream(asm.GetName().Name + ".About.txt")) { s_about = stream_about.AsString(); }
                if (!String.IsNullOrEmpty(s_about)) s_about = s_about.Uncapitalize();
                s_about = String.Format("[{0} {1}]{2}", asm.GetName().Name, asm.GetName().Version, String.IsNullOrEmpty(s_about) ? null : (": " + s_about));
                Out.Write(s_about);
            }
        }

        public static void Help()
        {
            var asm = Assembly.GetEntryAssembly();
            Help(asm == null ? null : asm.GetTypes().SingleOrDefault(t => t.HasAttr<ConfigAttribute>() && !t.IsAbstract));
        }

        public static void Help(Type t_cfg)
        {
            var asm = t_cfg == null ? null : t_cfg.Assembly;
            if (t_cfg != null)
            {
                var @params = t_cfg.GetProperties(BF.AllInstance)
                    .Where(p => p.HasAttr<ParamAttribute>()).OrderBy(p => p.Attr<ParamAttribute>().Priority).ToReadOnly();

                var cfg_name = t_cfg.Attr<ConfigAttribute>().Name ?? asm.GetName().Name;
                var s_syntax = String.Format("Syntax: {0}", cfg_name);
                foreach (var p in @params)
                {
                    var is_optional = t_cfg.GetProperty("Default" + p.Name, BF.AllStatic) != null;
                    var alias = p.Attr<ParamAttribute>().Aliases.First();
                    s_syntax += String.Format(" {0}-{1}:{2}{3}", is_optional ? "[" : "", alias, p.Name, is_optional ? "]" : "");
                }
                s_syntax += " [/verbose]";
                Out.WriteLine(s_syntax);

                var shortcuts = t_cfg.Attrs<ShortcutAttribute>().OrderBy(shortcut => shortcut.Priority);
                if (shortcuts.IsNotEmpty())
                {
                    Out.WriteLine("");
                    if (shortcuts.Count() == 1) Out.Write("Shortcut: ");
                    else { Out.WriteLine("Shortcuts:"); Out.Write("    "); }

                    shortcuts.ForEach((shortcut, i) =>
                    {
                        shortcut.Description.AssertNull();
                        Out.WriteLine(cfg_name + (shortcut.Shortcut.IsNullOrEmpty() ? "" : (" " + shortcut.Shortcut)));
                        if (i != shortcuts.Count() - 1) Out.Write("    ");
                    });
                }

                Out.WriteLine();
                Out.WriteLine("Parameters:");
                var max_name = @params.MaxOrDefault(p => p.Name.Length);
                max_name = Math.Max("/verbose".Length, max_name);
                var feed = new String(' ', 4 + 1 + max_name + 2);
                var max_desc = 80 - feed.Length;
                (max_desc > 0).AssertTrue();
                Action<String, String> write_name_desc = (name, desc) =>
                {
                    Out.Write(String.Format("    {0}{1}{2}  ",
                        name.StartsWith("/") ? "" : "%",
                        name.PadRight(max_name),
                        name.StartsWith("/") ? " " : ""));

                    var num_lines = (int)Math.Ceiling(1.0 * desc.Length / max_desc);
                    for (var i = 0; i < num_lines; ++i)
                    {
                        if (i != 0) Out.Write(feed);
                        var line = desc.Substring(i * max_desc, Math.Min(max_desc, desc.Length - i * max_desc));
                        Out.WriteLine(line);
                    }
                };
                foreach (var p in @params)
                {
                    var desc = p.Attr<ParamAttribute>().Description ?? String.Empty;

                    var p_default = t_cfg.GetProperty("Default" + p.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (p_default != null)
                    {
                        var default_value = p_default.GetValue(null, null);
                        if (default_value != null)
                        {
                            var s_default_value = default_value == null ? "null" : default_value.ToString();
                            if (default_value is DirectoryInfo)
                            {
                                var dir = default_value as DirectoryInfo;
                                if (dir.FullName == Environment.CurrentDirectory)
                                {
                                    s_default_value = "current dir";
                                }
                            }

                            var s_default = String.Format("Defaults to {0}.", s_default_value);
                            if (!String.IsNullOrEmpty(desc)) { if (!desc.EndsWith(".")) desc += "."; desc += " "; }
                            desc += s_default;
                        }
                    }

                    if (!String.IsNullOrEmpty(desc))
                    {
                        write_name_desc(p.Name, desc);
                    }
                }
                write_name_desc("/verbose", "Turns verbose tracing on, useful for debugging.");
            }
        }
    }
}