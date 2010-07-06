using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Reflection.Typed;
using NUnit.Framework;

namespace XenoGears.Playground.Reflection.Typed
{
    [TestFixture]
    public class Tests
    {
        public class A
        {
            public int ifield = 42;
            public static int isfield = 42;
            public double dprop { get; set; }

            public void InstanceMethod(int a, string b) { ifield += (a + b.Length); }
            public int InstanceMethodWithReturn() { ifield += 10; return ifield + 21; }

            public static void StaticMethod(int a, string b) { }
            public static int StaticMethodWithReturn() { return 21; }
        }

        [Test]
        public void MainSuccessScenario()
        {
            var a = new A();
            var t = typeof(A);

            var ifield = a.GetSlot<int>("ifield");
            Assert.That(a.ifield, Is.EqualTo(42));
            Assert.That(ifield.Get(), Is.EqualTo(42));

            ifield.Set(100);
            Assert.That(a.ifield, Is.EqualTo(100));
            Assert.That(ifield.Get(), Is.EqualTo(100));

            var isfield = t.GetSlot<int>("isfield");
            Assert.That(A.isfield, Is.EqualTo(42));
            Assert.That(isfield.Get(), Is.EqualTo(42));

            isfield.Set(100);
            Assert.That(A.isfield, Is.EqualTo(100));
            Assert.That(isfield.Get(), Is.EqualTo(100));

            var dprop = a.GetSlot<double>("dprop");
            Assert.That(a.dprop, Is.EqualTo(0));
            Assert.That(dprop.Get(), Is.EqualTo(0));

            dprop.Set(1);
            Assert.That(a.dprop, Is.EqualTo(1));
            Assert.That(dprop.Get(), Is.EqualTo(1));

            var imethvoid1 = a.GetMethod<Action<int, string>>("InstanceMethod");
            imethvoid1(15, "hello world");
            Assert.That(a.ifield, Is.EqualTo(126));

            var imethvoid2 = a.GetMethod<Action<int, _<string>>>("InstanceMethod");
            imethvoid2(-31, "hello");
            Assert.That(a.ifield, Is.EqualTo(100));

            var imethret1 = a.GetMethod<Func<int>>("InstanceMethodWithReturn");
            var ret1 = imethret1();
            Assert.That(a.ifield, Is.EqualTo(110));
            Assert.That(ret1, Is.EqualTo(131));

            var imethret2 = a.GetMethod<Func<_<int>>>("InstanceMethodWithReturn");
            var ret2 = imethret2();
            Assert.That(a.ifield, Is.EqualTo(120));
            Assert.That(ret2.Value, Is.EqualTo(141));

            var imethret3 = a.GetMethod<Func<_>>("InstanceMethodWithReturn");
            var ret3 = imethret3();
            Assert.That(a.ifield, Is.EqualTo(130));
            Assert.That(ret3.Value, Is.EqualTo(151));

            var smethvoid1 = t.GetMethod<Action<int, string>>("StaticMethod");
            smethvoid1(15, "hello world");

            var smethvoid2 = t.GetMethod<Action<int, _<string>>>("StaticMethod");
            smethvoid2(-31, "hello");

            var smethret1 = t.GetMethod<Func<int>>("StaticMethodWithReturn");
            var sret1 = smethret1();
            Assert.That(sret1, Is.EqualTo(21));

            var smethret2 = t.GetMethod<Func<_>>("StaticMethodWithReturn");
            smethret2();
        }

        [Test]
        public void PerformanceTest()
        {
            var a = new A();
            const int times = 1000000;

            // 1. Accessing a field via STR
            Trace.WriteLine("Accessing a field via STR " + times + " times");
            var ifield_reflection = typeof(A).GetField("ifield");
            var ifield_str = a.GetSlot<int>("ifield");

            Trace.Write("Native: ");
            var native_field_start = DateTime.Now;
            Enumerable.Range(1, times).ForEach(i => { var x = a.ifield; a.ifield = x; });
            var native_field_finish = DateTime.Now;
            var native_field_span = native_field_finish - native_field_start;
            Trace.WriteLine(String.Format("{0} ({1:00}%)", native_field_span, 100.0 * native_field_span.Ticks / native_field_span.Ticks));

            Trace.Write("Reflection: ");
            var reflection_field_start = DateTime.Now;
            Enumerable.Range(1, times).ForEach(i => { var x = (int)ifield_reflection.GetValue(a); ifield_reflection.SetValue(a, x); });
            var reflection_field_finish = DateTime.Now;
            var reflection_field_span = reflection_field_finish - reflection_field_start;
            Trace.WriteLine(String.Format("{0} ({1:00}%)", reflection_field_span, 100.0 * reflection_field_span.Ticks / native_field_span.Ticks));

            Trace.Write("STR: ");
            var str_field_start = DateTime.Now;
            Enumerable.Range(1, times).ForEach(i => { var x = ifield_str.Get(); ifield_str.Set(x); });
            var str_field_finish = DateTime.Now;
            var str_field_span = str_field_finish - str_field_start;
            Trace.WriteLine(String.Format("{0} ({1:00}%)", str_field_span, 100.0 * str_field_span.Ticks / native_field_span.Ticks));
            Trace.WriteLine(String.Empty);

            // 2. Invoking an instance method via STR
            Trace.WriteLine("Invoking an instance method via STR " + times + " times");
            var imeth_reflection = typeof(A).GetMethod("InstanceMethod", new[] { typeof(int), typeof(string) });
            var imeth_str = a.GetMethod<Action<int, string>>("InstanceMethod");

            Trace.Write("Native: ");
            var native_imeth_start = DateTime.Now;
            Enumerable.Range(1, times).ForEach(i => a.InstanceMethod(2, "hello world"));
            var native_imeth_finish = DateTime.Now;
            var native_imeth_span = native_imeth_finish - native_imeth_start;
            Trace.WriteLine(String.Format("{0} ({1:00}%)", native_imeth_span, 100.0 * native_imeth_span.Ticks / native_imeth_span.Ticks));

            Trace.Write("Reflection: ");
            var reflection_imeth_start = DateTime.Now;
            Enumerable.Range(1, times).ForEach(i => imeth_reflection.Invoke(a, new object[] { 2, "hello world" }));
            var reflection_imeth_finish = DateTime.Now;
            var reflection_imeth_span = reflection_imeth_finish - reflection_imeth_start;
            Trace.WriteLine(String.Format("{0} ({1:00}%)", reflection_imeth_span, 100.0 * reflection_imeth_span.Ticks / native_imeth_span.Ticks));

            Trace.Write("STR: ");
            var str_imeth_start = DateTime.Now;
            Enumerable.Range(1, times).ForEach(i => imeth_str(2, "hello world"));
            var str_imeth_finish = DateTime.Now;
            var str_imeth_span = str_imeth_finish - str_imeth_start;
            Trace.WriteLine(String.Format("{0} ({1:00}%)", str_imeth_span, 100.0 * str_imeth_span.Ticks / native_imeth_span.Ticks));
        }
    }
}
