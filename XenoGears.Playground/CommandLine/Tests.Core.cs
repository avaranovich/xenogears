using NUnit.Framework;
using XenoGears.CommandLine;

namespace XenoGears.Playground.CommandLine
{
    [TestFixture]
    public partial class Tests
    {
        [Test]
        public void Help()
        {
            RunTest(() => Banners.Help(typeof(Config)));
        }

        [Test]
        public void Parse_Empty()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse("/verbose");
                Assert.AreEqual(null, cfg.ProjectName);
                Assert.AreEqual("lite", cfg.TemplateName);
                Assert.AreEqual("hg", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(dirpath, cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Name()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse("Libptx", "/verbose");
                Assert.AreEqual("Libptx", cfg.ProjectName);
                Assert.AreEqual("lite", cfg.TemplateName);
                Assert.AreEqual("hg", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(dirpath, cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Target()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse(@"d:\Projects\Active\Libptx\", "/verbose");
                Assert.AreEqual(null, cfg.ProjectName);
                Assert.AreEqual("lite", cfg.TemplateName);
                Assert.AreEqual("hg", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(@"d:\Projects\Active\Libptx\", cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Name_Target()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse("libptx", @"d:\Projects\Active\Libptx\", "/verbose");
                Assert.AreEqual("libptx", cfg.ProjectName);
                Assert.AreEqual("lite", cfg.TemplateName);
                Assert.AreEqual("hg", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(@"d:\Projects\Active\Libptx\", cfg.TargetDir.FullName);
            });
        }

        [Test, Category("Hot")]
        public void Parse_Name_Target_Vcs()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse("libptx", @"d:\Projects\Active\Libptx\", "-vcs:mercurial", "/verbose");
                Assert.AreEqual("libptx", cfg.ProjectName);
                Assert.AreEqual("lite", cfg.TemplateName);
                Assert.AreEqual("mercurial", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(@"d:\Projects\Active\Libptx\", cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Target_Template()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse(@"d:\Projects\Active\Libptx\", "default", "/verbose");
                Assert.AreEqual(null, cfg.ProjectName);
                Assert.AreEqual("default", cfg.TemplateName);
                Assert.AreEqual("hg", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(@"d:\Projects\Active\Libptx\", cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Name_Target_Template()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse("libptx", @"d:\Projects\Active\Libptx\", "default", "/verbose");
                Assert.AreEqual("libptx", cfg.ProjectName);
                Assert.AreEqual("default", cfg.TemplateName);
                Assert.AreEqual("hg", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(@"d:\Projects\Active\Libptx\", cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Regular()
        {
            RunTest(() =>
            {
                var cfg = Config.Parse("-name:libptx", @"-target:d:\Projects\Active\Libptx\", "-vcs:mercurial", "/verbose");
                Assert.AreEqual("libptx", cfg.ProjectName);
                Assert.AreEqual("lite", cfg.TemplateName);
                Assert.AreEqual("mercurial", cfg.VcsName);
                Assert.AreEqual(null, cfg.VcsRepo);
                Assert.AreEqual(@"d:\Projects\Active\Libptx\", cfg.TargetDir.FullName);
            });
        }

        [Test]
        public void Parse_Invalid_Duped1()
        {
            RunTest(() => Config.Parse("-name:Libptx", "-name:Libptx", "/verbose"));
        }

        [Test]
        public void Parse_Invalid_Duped2()
        {
            RunTest(() => Config.Parse("-name:Libptx", "-project-name:Libptx", "/verbose"));
        }

        [Test]
        public void Parse_Invalid_Mix1()
        {
            RunTest(() => Config.Parse("Libptx", "-name:Libptx", "/verbose"));
        }

        [Test]
        public void Parse_Invalid_Mix2()
        {
            RunTest(() => Config.Parse("Libptx", "-vcs:hg", @"d:\Projects\Active\Libptx\", "/verbose"));
        }
    }
}