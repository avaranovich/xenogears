using System;
using System.Collections.Generic;
using System.IO;
using XenoGears.CommandLine;
using XenoGears.CommandLine.Annotations;

namespace XenoGears.Playground.CommandLine
{
    [Config("prjinit")]
    [Shortcut("", Priority = 1)]
    [Shortcut("target", Priority = 2)]
    [Shortcut("name", Priority = 3)]
    [Shortcut("target template", Priority = 4)]
    [Shortcut("name target", Priority = 5)]
    [Shortcut("name target template", Priority = 6)]
    internal class Config : CommandLineConfig
    {
        [Param("name", "prj-name", "project-name", Description = "Name of the project. Defaults to the name of target dir.")]
        public String ProjectName { get; private set; }
        private static String DefaultProjectName { get { return null; } }

        [Param("template", "template-name", Description = "Codebase template.")]
        public String TemplateName { get; set; }
        private static String DefaultTemplateName { get { return "lite"; } }

        [Param("vcs", "vcs-name", "vcs-provider", Description = "VCS provider.")]
        public String VcsName { get; set; }
        private static String DefaultVcsName { get { return "hg"; } }

        [Param("repo", "vcs-repo", Description = "VCS repository, e.g. https://prjinit.googlecode.com/hg/.")]
        public String VcsRepo { get; private set; }
        private static String DefaultVcsRepo { get { return null; } }

        [Param("target", "dir", "target-dir", Description = "Path to target directory.")]
        public DirectoryInfo TargetDir { get; private set; }
        private static DirectoryInfo DefaultTargetDir { get { return new DirectoryInfo(Environment.CurrentDirectory); } }

        internal new static Config Parse(params String[] args) { return (Config)CommandLineConfig.Parse(args); }
        internal new static Config Parse(IEnumerable<String> args) { return (Config)CommandLineConfig.Parse(args); }
        private Config(String[] args) : base(args) { }
    }
}
