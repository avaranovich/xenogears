using NUnit.Framework;
using XenoGears.CommandLine;

namespace XenoGears.Playground.CommandLine
{
    [TestFixture]
    public partial class Tests
    {
        [Test, Category("Hot")]
        public void Help()
        {
            RunTest(() => Banners.Help(typeof(Config)));
        }

        [Test]
        public void Parse_Empty()
        {
            RunTest(() => Config.Parse("/verbose"));
        }

        [Test]
        public void Parse_Name()
        {
            RunTest(() => Config.Parse("Libptx", "/verbose"));
        }

        [Test]
        public void Parse_Target()
        {
            RunTest(() => Config.Parse(@"d:\Projects\Active\Libptx\", "/verbose"));
        }

        [Test]
        public void Parse_Name_Target()
        {
            RunTest(() => Config.Parse("Libptx", @"d:\Projects\Active\Libptx\", "/verbose"));
        }

        [Test]
        public void Parse_Name_Target_Vcs()
        {
            RunTest(() => Config.Parse("Libptx", @"d:\Projects\Active\Libptx\", "-vcs:hg", "/verbose"));
        }

        [Test]
        public void Parse_Target_Template()
        {
            RunTest(() => Config.Parse(@"d:\Projects\Active\Libptx\", "lite", "/verbose"));
        }

        [Test]
        public void Parse_Name_Target_Template()
        {
            RunTest(() => Config.Parse("Libptx", @"d:\Projects\Active\Libptx\", "lite", "/verbose"));
        }

        [Test]
        public void Parse_Regular()
        {
            RunTest(() => Config.Parse("-name:Libptx", @"-target:d:\Projects\Active\Libptx\", "-vcs:hg", "/verbose"));
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