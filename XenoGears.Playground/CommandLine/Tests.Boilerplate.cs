﻿using System;
using System.IO;
using NUnit.Framework;
using XenoGears.Logging;
using XenoGears.Playground.Framework;
using XenoGears.Strings;

namespace XenoGears.Playground.CommandLine
{
    public partial class Tests : BaseTests
    {
        private String dirpath { get { return new DirectoryInfo(Environment.CurrentDirectory).FullName; } }
        private String dirname { get { return new DirectoryInfo(Environment.CurrentDirectory).Name; } }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            MultiplexLogs(Logger.Get(typeof(Config)));
        }

        protected override String PreprocessResult(String s_actual)
        {
            s_actual = s_actual.Replace(dirpath, "<CurrentDir>");
            s_actual = s_actual.Replace(dirpath.Uncapitalize(), "<CurrentDir>");
            return s_actual.Replace(dirpath.Capitalize(), "<CurrentDir>");
        }

        private void RunTest(Action test)
        {
            (test ?? (() => {}))();
            VerifyResult(Out.ToString());
        }
    }
}
