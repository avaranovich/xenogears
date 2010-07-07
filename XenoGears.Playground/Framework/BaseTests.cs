using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Generics;
using XenoGears.Strings;
using XenoGears.Logging;
using NUnit.Framework;
using XenoGears.Traits.Disposable;

namespace XenoGears.Playground.Framework
{
    public abstract class BaseTests
    {
        protected StringBuilder Out { get; set; }
        private IDisposable _overridenOut = new DisposableAction(() => {});

        [SetUp]
        public void SetUp()
        {
            var eavesdropper = new Eavesdropper(Log.Out, Out = new StringBuilder());
            _overridenOut = Log.OverrideOut(eavesdropper) ?? new DisposableAction(() => {});
            UnitTest.Context["Current Fixture"] = this.GetType();
        }

        [TearDown]
        public void TearDown()
        {
            _overridenOut.Dispose();
            Out = null;
        }

        protected virtual String PreprocessResult(String s_actual)
        {
            return s_actual;
        }

        protected void VerifyResult()
        {
            VerifyResult(Out.ToString(), UnitTest.CurrentTest);
        }

        protected void VerifyResult(MethodBase unit_test)
        {
            VerifyResult(Out.ToString(), unit_test, UnitTest.CurrentFixture);
        }

        protected void VerifyResult(Type test_fixture)
        {
            VerifyResult(Out.ToString(), UnitTest.CurrentTest, test_fixture);
        }

        protected void VerifyResult(MethodBase unit_test, Type test_fixture)
        {
            VerifyResult(Out.ToString(), unit_test, test_fixture);
        }

        protected void VerifyResult(String s_actual)
        {
            VerifyResult(s_actual, UnitTest.CurrentTest);
        }

        protected void VerifyResult(String s_actual, MethodBase unit_test)
        {
            VerifyResult(Out.ToString(), unit_test, UnitTest.CurrentFixture);
        }

        protected void VerifyResult(String s_actual, Type test_fixture)
        {
            VerifyResult(Out.ToString(), UnitTest.CurrentTest, UnitTest.CurrentFixture);
        }

        protected void VerifyResult(String s_actual, MethodBase unit_test, Type test_fixture)
        {
            s_actual = PreprocessResult(s_actual);
            unit_test = (unit_test ?? UnitTest.CurrentTest).AssertNotNull();
            test_fixture = (test_fixture ?? UnitTest.CurrentFixture).AssertNotNull();

            var fnameWannabes = new List<String>();
            var s_name = unit_test.Name;
            var s_sig = unit_test.Params().Select(p => p.GetCSharpRef(ToCSharpOptions.Informative).Replace("<", "[").Replace(">", "]").Replace("&", "!").Replace("*", "!")).StringJoin("_");
            var s_declt = test_fixture.GetCSharpRef(ToCSharpOptions.Informative).Replace("<", "[").Replace(">", "]").Replace("&", "!").Replace("*", "!");
            fnameWannabes.Add(s_name);
            fnameWannabes.Add(s_name + "_" + s_sig);
            fnameWannabes.Add(s_declt + "_" + s_name);
            fnameWannabes.Add(s_declt + "_" + s_name + "_" + s_sig);

            var res_asm = test_fixture.Assembly;
            var ns = test_fixture.Namespace + ".Reference.";
            var resources = res_asm.GetManifestResourceNames();
            var f_reference = fnameWannabes.SingleOrDefault2(wannabe =>
                resources.ExactlyOne(n => String.Compare(n, ns + wannabe, true) == 0));

            var success = false;
            String failMsg = null;
            if (f_reference != null)
            {
                String s_reference;
                using (var stream = res_asm.GetManifestResourceStream(ns + f_reference))
                {
                    s_reference = new StreamReader(stream).ReadToEnd();
                }

                if (s_reference.IsEmpty())
                {
                    Trace.WriteLine(s_actual);

                    Assert.Fail(String.Format(
                        "Reference result for unit test '{1}' is empty.{0}" +
                        "Please, verify the trace dumped above and put in into the resource file.",
                        Environment.NewLine, unit_test.GetCSharpDecl(ToCSharpOptions.Informative)));
                }
                else
                {
                    var expected = s_reference.SplitLines();
                    var actual = s_actual.SplitLines();
                    if (expected.Count() != actual.Count())
                    {
                        failMsg = String.Format(
                            "Number of lines doesn't match. Expected {0}, actually found {1}" + Environment.NewLine,
                            expected.Count(), actual.Count());
                    }

                    success = expected.Zip(actual).SkipWhile((t, i) =>
                    {
                        if (t.Item1 != t.Item2)
                        {
                            var maxLines = Math.Max(actual.Count(), expected.Count());
                            var maxDigits = (int)Math.Floor(Math.Log10(maxLines)) + 1;
                            failMsg = String.Format(
                                "Line {1} (starting from 1) doesn't match.{0}{4}{2}{0}{5}{3}{0}",
                                Environment.NewLine, i + 1,
                                t.Item1.Replace(" ", "·"), t.Item2.Replace(" ", "·"),
                                "E:".PadRight(maxDigits + 3), "A:".PadRight(maxDigits + 3));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }).IsEmpty();

                    if (!success)
                    {
                        var maxLines = Math.Max(actual.Count(), expected.Count());
                        var maxDigits = (int)Math.Floor(Math.Log10(maxLines)) + 1;
                        var maxActual = Math.Max(actual.MaxOrDefault(line => line.Length), "Actual".Length);
                        var maxExpected = Math.Max(expected.MaxOrDefault(line => line.Length), "Expected".Length);
                        var total = maxDigits + 3 + maxActual + 3 + maxExpected;

                        Trace.WriteLine(String.Format("{0} | {1} | {2}",
                            "N".PadRight(maxDigits),
                            "Actual".PadRight(maxActual),
                            "Expected".PadRight(maxExpected)));
                        Trace.WriteLine(total.Times("-"));

                        0.UpTo(maxLines - 1).ForEach(i =>
                        {
                            var l_actual = actual.ElementAtOrDefault(i, String.Empty);
                            var l_expected = expected.ElementAtOrDefault(i, String.Empty);
                            Trace.WriteLine(String.Format("{0} | {1} | {2}",
                                i.ToString().PadLeft(maxDigits),
                                l_actual.PadRight(maxActual),
                                l_expected.PadRight(maxExpected)));
                        });

                        Trace.WriteLine(String.Empty);
                        Trace.WriteLine(failMsg);
                        Trace.WriteLine(String.Empty);
                        Assert.Fail("Actual result doesn't match reference result.");
                    }
                }
            }
            else
            {
                Trace.WriteLine(s_actual);

                Assert.Fail(String.Format(Environment.NewLine +
                    "Couldn't find a file in resources that contains reference result for unit test '{1}'.{0}" +
                    "Please, verify the trace dumped above and put it into one of the following files under the Reference folder next to the test suite: {0}" +
                    fnameWannabes.Select(s => String.Format("\"{0}\"", s)).StringJoin(", ") + ".{0}" +
                    "Also be sure not to forget to select build action 'Embedded Resource' in file properties widget!",
                    Environment.NewLine, unit_test.GetCSharpDecl(ToCSharpOptions.Informative)));
            }
        }
    }
}
