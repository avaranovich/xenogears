using System;
using System.Collections.Generic;
using NUnit.Framework;
using XenoGears.Collections.Dictionaries;
using XenoGears.Formats;
using XenoGears.Functional;
using XenoGears.Playground.Framework;
using System.Linq;

namespace XenoGears.Playground.Formats
{
    [TestFixture]
    public class JsonTests : BaseTests
    {
        private class Foo
        {
            public bool Ok { get; private set; }
            public OrderedDictionary<String, IList<IBar>> Bars { get; private set; }
            public List<Foo> Foos { get; set; }
        }

        private interface IBar
        {
            int Qux { get; set; }
            String Baz { get; set; }
        }

        [Test, Category("Hot")]
        public void Test1()
        {
            var s_json = InputText();
            var json = Json.Parse(s_json);
            Assert.AreEqual(new Json(false), json[0].ok);
            Assert.AreEqual(false, (bool)json[0].ok);
            Assert.AreEqual(new Json("ein"), json[0].bars.ein[0].baz);
            Assert.AreEqual(2, (int)json[0].bars.ein[1].qux);
            Assert.AreEqual(new Json(new {}), json[0].foos[1].bars);
            Assert.AreEqual(new Json(null), json[0].bars.drei);
            Assert.AreEqual(null, (List<IBar>)json[0].bars.drei);

            var anon = new Object[]
            {
                new
                {
                    ok = false, 
                    bars = new
                    {
                        ein = new []
                        {
                            new { qux = 1, baz = "ein" },
                            new { qux = 2, baz = "" }
                        },
                        zwei = new []
                        {
                            new { qux = 1 },
                        }
                    },
                    foos = new Object[]
                    {
                        new { ok = true },
                        new { ok = false, bars = new { } },
                    }
                },
                new
                {
                    ok = true
                }
            };
            Assert.AreEqual(anon, json);
            Assert.AreEqual(anon.ToJson(), json.ToJson());
            Assert.AreEqual(anon.ToJson().ToCompactString(), json.ToJson().ToCompactString());
            Assert.AreEqual(anon.ToJson().ToPrettyString(), json.ToJson().ToPrettyString());

            var foos = (List<Foo>)json;
            Assert.AreEqual(1, foos.Count);
            var foo0 = foos[0];
                Assert.AreEqual(false, foo0.Ok);
                var foo0_bars = foo0.Bars;
                    var bar0 = foo0_bars.AsEnumerable().Nth(0);
                        Assert.AreEqual("ein", bar0.Key);
                        var bar0_value = bar0.Value;
                        Assert.AreEqual(2, bar0_value.Count);
                        var bar0_value0 = bar0_value[0];
                            Assert.AreEqual(1, bar0_value0.Qux);
                            Assert.AreEqual("ein", bar0_value0.Baz);
                        var bar0_value1 = bar0_value[1];
                            Assert.AreEqual(2, bar0_value1.Qux);
                            Assert.AreEqual("", bar0_value1.Baz);
                    var bar1 = foo0_bars.AsEnumerable().Nth(1);
                        Assert.AreEqual("zwei", bar1.Key);
                        var bar1_value = bar1.Value;
                        Assert.AreEqual(1, bar1_value.Count);
                        var bar1_value0 = bar1_value[0];
                            Assert.AreEqual(1, bar1_value0.Qux);
                            Assert.AreEqual(null, bar1_value0.Baz);
                    var bar2 = foo0_bars.AsEnumerable().Nth(2);
                        Assert.AreEqual("drei", bar2.Key);
                        Assert.AreEqual(null, bar2.Value);
                var foo0_foos = foo0.Foos;
                Assert.AreEqual(2, foo0_foos.Count);
                var foo0_foo0 = foo0_foos[0];
                    Assert.AreEqual(true, foo0_foo0.Ok);
                    Assert.AreEqual(null, foo0_foo0.Bars);
                    Assert.AreEqual(null, foo0_foo0.Foos);
                var foo0_foo1 = foo0_foos[1];
                    Assert.AreEqual(false, foo0_foo1.Ok);
                    Assert.AreEqual(1, foo0_foo0.Bars.Count);
                    Assert.AreEqual(null, foo0_foo0.Foos);
            var foo1 = foos[1];
                Assert.AreEqual(true, foo1.Ok);
                Assert.AreEqual(null, foo1.Bars);
                Assert.AreEqual(null, foo1.Foos);

            var result = foos.ToJson().ToPrettyString();
            VerifyResult(result);
        }
    }
}