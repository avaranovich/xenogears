using System;
using NUnit.Framework;

namespace XenoGears.Playground.Dynamic
{
    [TestFixture]
    public class DynamicProxyTests
    {
        [Test, Category("Hot")]
        public void Test1()
        {
            var foobar = new Expando();
            foobar.Add("foo", "$bar");
            Assert.AreEqual("foo = $bar", foobar.ToString());
            var quux = new Expando();
            quux.Add("qu", "ux");
            Assert.AreEqual("qu = ux", quux.ToString());

            // init
            dynamic map = new Expando();
            Assert.AreEqual("", map.ToString());

            // binaryoperation
            map += foobar;
            map += quux;
            Assert.AreEqual("foo = $bar, qu = ux", map.ToString());

            // fallback to csharpbinder
            map -= quux;
            Assert.AreEqual("foo = $bar", map.ToString());

            // convert
            var s = (String)map;
            Assert.AreEqual("foo = $bar", s);

            // getindex
            var bar = map["foo"];
            Assert.AreEqual("$bar", bar);

            // invoke, fallback to idynamicobject
            map = map(bar : "bar");
            Assert.AreEqual("foo = bar", map.ToString());

            // setindex
            map["two"] = 2;
            Assert.AreEqual("foo = bar, two = 2", map.ToString());
            map.Remove("two");

            //invokemember
            map["fn"] = ((Func<int, int, int>)((x, y) => x + y));
            var z = map.fn(1, 2);
            Assert.AreEqual(3, z);

            // setmember
            map.fn = "fn";
            map.two = "2";
            Assert.AreEqual("foo = bar, fn = fn, two = 2", map.ToString());

            // unaryoperation
            map = !map;
            Assert.AreEqual("oof = bar, nf = fn, owt = 2", map.ToString());
        }
    }
}
