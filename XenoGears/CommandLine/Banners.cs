using System;
using System.IO;
using System.Linq;
using System.Reflection;
using XenoGears.CommandLine.Annotations;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Functional;
using XenoGears.Assertions;
using XenoGears.Strings;

namespace XenoGears.CommandLine
{
    public static class Banners
    {
        public static void About()
        {
            var asm = Assembly.GetEntryAssembly();
            if (asm != null)
            {
                String s_about; using (var stream_about = asm.GetManifestResourceStream(asm.GetName().Name + ".About.txt")) { s_about = stream_about.AsString(); }
                if (!String.IsNullOrEmpty(s_about)) s_about = s_about.Uncapitalize();
                s_about = String.Format("[{0} {1}]{2}", asm.GetName().Name, asm.GetName().Version, String.IsNullOrEmpty(s_about) ? null : (": " + s_about));
                Console.Write(s_about);
            }
        }

        public static void Help()
        {
            var asm = Assembly.GetEntryAssembly();
            var t_cfg = asm == null ? null : asm.GetTypes().SingleOrDefault(t => t.HasAttr<ConfigAttribute>() && !t.IsAbstract);
            if (t_cfg != null)
            {
                var @params = t_cfg.GetProperties(BF.AllInstance)
                    .Where(p => p.HasAttr<ParamAttribute>()).OrderBy(p => p.Attr<ParamAttribute>().Priority).ToReadOnly();

                var s_syntax = String.Format("Syntax: {0}", asm.GetName().Name);
                foreach (var p in @params)
                {
                    var is_optional = t_cfg.GetProperty("Default" + p.Name, BF.AllStatic) != null;
                    var alias = p.Attr<ParamAttribute>().Aliases.First();
                    s_syntax += String.Format(" {0}-{1}:{2}{3}", is_optional ? "[" : "", alias, p.Name, is_optional ? "]" : "");
                }
                s_syntax += " [/verbose]";
                Console.WriteLine(s_syntax);

                var shortcuts = t_cfg.Attrs<ShortcutAttribute>().OrderBy(shortcut => shortcut.Priority);
                if (shortcuts.IsNotEmpty())
                {
                    Console.WriteLine("");
                    if (shortcuts.Count() == 1) Console.Write("Shortcut: ");
                    else { Console.WriteLine("Shortcuts:"); Console.Write("    "); }

                    shortcuts.ForEach((shortcut, i) =>
                    {
                        shortcut.Description.AssertNull();
                        Console.WriteLine(asm.GetName().Name + " " + shortcut.Shortcut);
                        if (i != shortcuts.Count() - 1) Console.Write("    ");
                    });
                }

                Console.WriteLine();
                Console.WriteLine("Parameters:");
                var max_name = @params.MaxOrDefault(p => p.Name.Length);
                max_name = Math.Max("/verbose".Length, max_name);
                var feed = new String(' ', 4 + 1 + max_name + 2);
                var max_desc = 80 - feed.Length;
                (max_desc > 0).AssertTrue();
                Action<String, String> write_name_desc = (name, desc) =>
                {
                    Console.Write("    {0}{1}{2}  ",
                        name.StartsWith("/") ? "" : "%",
                        name.PadRight(max_name),
                        name.StartsWith("/") ? " " : "");

                    var num_lines = (int)Math.Ceiling(1.0 * desc.Length / max_desc);
                    for (var i = 0; i < num_lines; ++i)
                    {
                        if (i != 0) Console.Write(feed);
                        var line = desc.Substring(i * max_desc, Math.Min(max_desc, desc.Length - i * max_desc));
                        Console.WriteLine(line);
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

                            var s_default = String.Format("Default value is {0}.", s_default_value);
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