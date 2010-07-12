using System;
using System.IO;
using XenoGears.Playground.Framework;

namespace XenoGears.Playground.CommandLine
{
    public partial class Tests : BaseTests
    {
        private String dirpath { get { return new DirectoryInfo(Environment.CurrentDirectory).FullName; } }
        private String dirname { get { return new DirectoryInfo(Environment.CurrentDirectory).Name; } }

        private void RunTest(Action test)
        {
            (test ?? (() => {}))();
            VerifyResult(Out.ToString());
        }
    }
}
