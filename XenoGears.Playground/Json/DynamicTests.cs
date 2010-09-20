using System;
using System.Dynamic;
using NUnit.Framework;

namespace XenoGears.Playground.Json
{
    [TestFixture]
    public class DynamicTests
    {
        private class Foo : DynamicObject
        {
            private readonly Object _foo;
            public Foo(Object foo) { _foo = foo; }

            public override bool TryConvert(ConvertBinder binder, out Object result)
            {
                if (binder.ReturnType == _foo.GetType()) { result = _foo; return true; }
                if (binder.ReturnType == typeof(String)) { result = _foo.ToString(); return true; }
                result = null; return false;
            }
        }

        [Test]
        public void TestCasts()
        {
            dynamic foo = new Foo(10);
            var foo_int = (int)foo;
            var foo_string = (String)foo;
            var foo_bar = (DateTime)foo;
        }
    }
}
