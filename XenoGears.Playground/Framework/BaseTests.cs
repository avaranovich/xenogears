using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Logging.Media;
using XenoGears.Reflection.Generics;
using XenoGears.Streams;
using XenoGears.Strings;
using XenoGears.Logging;
using NUnit.Framework;
using XenoGears.Traits.Disposable;

namespace XenoGears.Playground.Framework
{
    public abstract class BaseTests
    {
        private Dictionary<String, Object> _flash = null;
        protected Dictionary<String, Object> Flash { get { return _flash; } }

        protected Guid Id = Guid.NewGuid();
        protected LevelLogger Log { get; private set; }
        protected StringBuilder Out { get; private set; }
        private LogWriter _writer;
        private DisposableAction _teardown = new DisposableAction(() => {});

        [SetUp]
        public virtual void SetUp()
        {
            _writer = LogWriter.Get(this.GetType().AssemblyQualifiedName + "::" + Id + "::" + Guid.NewGuid());
            _writer.Medium = new AdhocMedium();
            _writer.Multiplex(Out = new StringBuilder());
            Log = Logger.Get(this.GetType().AssemblyQualifiedName + "::" + Id + "::" + Guid.NewGuid()).Debug;
            Log.OverrideWriter(_writer);
            MultiplexLogs(Logger.Adhoc);

            UnitTest.Context["Current Fixture"] = this.GetType();
            _flash = new Dictionary<String, Object>();
        }

        protected void MultiplexLogs(params String[] names) { MultiplexLogs((IEnumerable<String>)names); }
        protected void MultiplexLogs(IEnumerable<String> names) { MultiplexLogs(names.Select(name => Logger.Get(name))); }
        protected void MultiplexLogs(params Logger[] loggers) { MultiplexLogs((IEnumerable<Logger>)loggers); }
        protected void MultiplexLogs(IEnumerable<Logger> loggers) { loggers.ForEach(logger => _teardown += logger.OverrideWriter(_writer)); }

        [TearDown]
        public virtual void TearDown()
        {
            _teardown.Dispose();
            Out = null;
        }

        protected virtual String PreprocessReference(String s_reference)
        {
            return s_reference;
        }

        protected virtual byte[] PreprocessReference(byte[] bb_reference)
        {
            return bb_reference;
        }

        protected virtual String PreprocessResult(String s_actual)
        {
            return s_actual;
        }

        protected virtual byte[] PreprocessResult(byte[] bb_actual)
        {
            return bb_actual;
        }

        protected virtual Assembly ReferenceAssembly()
        {
            var test_fixture = UnitTest.CurrentFixture.AssertNotNull();
            return test_fixture.Assembly;
        }

        protected virtual String ReferenceNamespace()
        {
            var test_fixture = UnitTest.CurrentFixture.AssertNotNull();
            return test_fixture.Namespace + ".Reference";
        }

        protected virtual ReadOnlyCollection<String> ReferenceWannabes()
        {
            var unit_test = UnitTest.CurrentTest.AssertNotNull();
            var test_fixture = UnitTest.CurrentFixture.AssertNotNull();

            var fnameWannabes = new List<String>();
            var cs_opt = ToCSharpOptions.Informative;
            cs_opt.EmitCtorNameAsClassName = true;
            var s_name = unit_test.IsConstructor ? unit_test.DeclaringType.GetCSharpRef(cs_opt).Replace("<", "[").Replace(">", "]").Replace("&", "!").Replace("*", "!") : unit_test.Name;
            var s_sig = unit_test.Params().Select(p => p.GetCSharpRef(cs_opt).Replace("<", "[").Replace(">", "]").Replace("&", "!").Replace("*", "!")).StringJoin("_");
            var s_declt = test_fixture.GetCSharpRef(cs_opt).Replace("<", "[").Replace(">", "]").Replace("&", "!").Replace("*", "!");
            fnameWannabes.Add(s_name);
            fnameWannabes.Add(s_name + "_" + s_sig);
            fnameWannabes.Add(s_declt + "_" + s_name);
            fnameWannabes.Add(s_declt + "_" + s_name + "_" + s_sig);

            return fnameWannabes.ToReadOnly();
        }

        protected virtual String ReferenceText()
        {
            var res_asm = ReferenceAssembly();
            var resources = res_asm.Resources();
            var wannabes = ReferenceWannabes().Select(name => ReferenceNamespace() + "." + name).ToReadOnly();
            var f_reference = wannabes.SingleOrDefault2(wannabe => resources.ExactlyOne(name => String.Compare(name, wannabe, true) == 0));
            return f_reference == null ? null : res_asm.ReadText(f_reference);
        }

        protected virtual byte[] ReferenceBinary()
        {
            var res_asm = ReferenceAssembly();
            var resources = res_asm.Resources();
            var wannabes = ReferenceWannabes().Select(name => ReferenceNamespace() + "." + name).ToReadOnly();
            var f_reference = wannabes.SingleOrDefault2(wannabe => resources.ExactlyOne(name => String.Compare(name, wannabe, true) == 0));
            return f_reference == null ? null : res_asm.ReadBinary(f_reference);
        }

        protected void VerifyResult(String s_actual)
        {
            var s_reference = ReferenceText();
            if (s_reference != null)
            {
                VerifyResult(s_reference, s_actual);
            }
            else
            {
                Log.EnsureBlankLine();
                Log.WriteLine(s_actual);

                Assert.Fail(String.Format(Environment.NewLine +
                    "Couldn't find a file in resources that contains reference result for unit test '{1}'.{0}" +
                    "Please, verify the trace dumped above and put it into one of the following files under the Reference folder next to the test suite: {0}" +
                    ReferenceWannabes().Select(s => String.Format("\"{0}\"", s)).StringJoin(", ") + ".{0}" +
                    "Also be sure not to forget to select build action 'Embedded Resource' in file properties widget!",
                    Environment.NewLine, UnitTest.CurrentTest.GetCSharpRef(ToCSharpOptions.Informative)));
            }
        }

        protected void VerifyResult(String s_expected, String s_actual)
        {
            (s_expected != null && s_actual != null).AssertTrue();
            s_expected = PreprocessReference(s_expected);
            s_actual = PreprocessResult(s_actual);

            var success = false;
            String failMsg = null;
            if (s_expected.IsEmpty())
            {
                Log.EnsureBlankLine();
                Log.WriteLine(s_actual);

                Assert.Fail(String.Format(
                    "Reference result for unit test '{1}' is empty.{0}" +
                    "Please, verify the trace dumped above and put in into the resource file.",
                    Environment.NewLine, UnitTest.CurrentTest.GetCSharpRef(ToCSharpOptions.Informative)));
            }
            else
            {
                var expected = s_expected.SplitLines();
                var actual = s_actual.SplitLines();
                if (expected.Count() != actual.Count())
                {
                    success = false;
                    failMsg = String.Format(
                        "Number of lines doesn't match. Expected {0}, actually found {1}" + Environment.NewLine,
                        expected.Count(), actual.Count());
                }
                else
                {
                    success = expected.Zip(actual).SkipWhile((t, i) =>
                    {
                        if (t.Item1 != t.Item2)
                        {
                            var maxLines = Math.Max(actual.Count(), expected.Count());
                            var maxDigits = (int)Math.Floor(Math.Log10(maxLines)) + 1;
                            failMsg = String.Format(
                                "Line {1} (starting from 1) doesn't match.{0}{4}{2}{0}{5}{3}",
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
                }

                if (!success)
                {
                    var maxLines = Math.Max(actual.Count(), expected.Count());
                    var maxDigits = (int)Math.Floor(Math.Log10(maxLines + 1)) + 1;
                    var maxActual = Math.Max(actual.MaxOrDefault(line => line.Length), "Actual".Length);
                    var maxExpected = Math.Max(expected.MaxOrDefault(line => line.Length), "Expected".Length);
                    var total = maxDigits + 3 + maxActual + 3 + maxExpected;

                    Log.EnsureBlankLine();
                    Log.WriteLine(String.Format("{0} | {1} | {2}",
                        "N".PadRight(maxDigits),
                        "Actual".PadRight(maxActual),
                        "Expected".PadRight(maxExpected)));
                    Log.WriteLine(total.Times("-"));

                    0.UpTo(maxLines - 1).ForEach(i =>
                    {
                        var l_actual = actual.ElementAtOrDefault(i, String.Empty);
                        var l_expected = expected.ElementAtOrDefault(i, String.Empty);
                        Log.WriteLine(String.Format("{0} | {1} | {2}",
                            (i + 1).ToString().PadLeft(maxDigits),
                            l_actual.PadRight(maxActual),
                            l_expected.PadRight(maxExpected)));
                    });

                    Log.EnsureBlankLine();
                    Log.WriteLine(failMsg);
                    Assert.Fail("Actual result doesn't match expected result.");
                }
            }
        }

        protected void VerifyResult(byte[] bb_actual)
        {
            var bb_reference = ReferenceBinary();
            if (bb_reference != null)
            {
                VerifyResult(bb_reference, bb_actual);
            }
            else
            {
                Log.EnsureBlankLine();
                Log.WriteLine(bb_actual);

                Assert.Fail(String.Format(Environment.NewLine +
                    "Couldn't find a file in resources that contains reference result for unit test '{1}'.{0}" +
                    "Please, verify the trace dumped above and put it into one of the following files under the Reference folder next to the test suite: {0}" +
                    ReferenceWannabes().Select(s => String.Format("\"{0}\"", s)).StringJoin(", ") + ".{0}" +
                    "Also be sure not to forget to select build action 'Embedded Resource' in file properties widget!",
                    Environment.NewLine, UnitTest.CurrentTest.GetCSharpRef(ToCSharpOptions.Informative)));
            }
        }

        protected void VerifyResult(byte[] bb_expected, byte[] bb_actual)
        {
            throw new NotImplementedException();
        }
    }
}
