using NUnit.Framework;
using XenoGears.CommandLine;

namespace XenoGears.Playground.CommandLine
{
    [TestFixture]
    public partial class Tests
    {
        [Test, Category("Hot")]
        public void TestHelp()
        {
            Banners.Help(typeof(Config));
        }

        [Test]
        public void TestParse_Name()
        {
            Config.Parse("Libptx");
        }

        [Test]
        public void TestParse_Target()
        {
            Config.Parse(@"d:\Projects\Active\Libptx\");
        }

        [Test]
        public void TestParse_Name_Target()
        {
            Config.Parse("Libptx", @"d:\Projects\Active\Libptx\");
        }

        [Test]
        public void TestParse_Name_Target_Vcs()
        {
            Config.Parse("Libptx", @"d:\Projects\Active\Libptx\", "-vcs:hg");
        }

        [Test]
        public void TestParse_Target_Template()
        {
            Config.Parse(@"d:\Projects\Active\Libptx\", "lite");
        }

        [Test]
        public void TestParse_Name_Target_Template()
        {
            Config.Parse("Libptx", @"d:\Projects\Active\Libptx\", "lite");
        }

        [Test]
        public void TestParse_Regular()
        {
            Config.Parse("-name:Libptx", @"-target:d:\Projects\Active\Libptx\", "-vcs:hg");
        }

        [Test]
        public void TestParse_Invalid_Duped1()
        {
            Config.Parse("-name:Libptx", "-name:Libptx");
        }

        [Test]
        public void TestParse_Invalid_Duped2()
        {
            Config.Parse("-name:Libptx", "-project-name:Libptx");
        }

        [Test]
        public void TestParse_Invalid_Mix1()
        {
            Config.Parse("Libptx", "-name:Libptx");
        }

        [Test]
        public void TestParse_Invalid_Mix2()
        {
            Config.Parse("Libptx", "-vcs:hg", @"d:\Projects\Active\Libptx\",);
        }
    }
}