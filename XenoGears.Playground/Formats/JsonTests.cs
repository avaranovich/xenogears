using System;
using System.Collections.Generic;
using NUnit.Framework;
using XenoGears.Collections.Dictionaries;
using XenoGears.Formats;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Formats.Configuration.Default.Annotations;
using XenoGears.Formats.Engines;
using XenoGears.Functional;
using XenoGears.Playground.Framework;
using System.Linq;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Configuration.Default;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Lambda;
using XenoGears.Formats.Engines.Lambda;
using XenoGears.Formats.Validators.Lambda;

namespace XenoGears.Playground.Formats
{
    [TestFixture]
    public class JsonTests : BaseTests
    {
        public class BaseFoo
        {
            public bool Ok { get; private set; }
            internal List<Foo> Foos { get; set; }
        }

        public class Foo : BaseFoo
        {
            public OrderedDictionary<String, IList<IBar>> Bars { get; private set; }
            private int Calc { get { return Ok.SafeHashCode() ^ Bars.SafeHashCode() ^ Foos.SafeHashCode(); } }
        }

        [AttributeUsage(AttributeTargets.Class)]
        private class IBarAdapter : TypeAdapter
        {
            public override Object AfterDeserialize(Type t, Object value)
            {
                var bar = value.AssertCast<IBar>();
                if (bar == null) return null;
                bar.Baz = bar.Baz ?? "null";
                return bar;
            }
        }

        public interface IBaz
        {
            String Baz { get; set; }
        }

        public interface IBar : IBaz
        {
            Qux Qux { get; set; }
        }

        [AdhocEngine, Json(DefaultCtor = false)]
        public class Qux
        {
            public int Value { get; set; }
            public Qux(int value) { Value = value; }

            public bool Equals(Qux other) { if (ReferenceEquals(null, other)) return false; if (ReferenceEquals(this, other)) return true; return other.Value == Value; }
            public override bool Equals(object obj) { if (ReferenceEquals(null, obj)) return false; if (ReferenceEquals(this, obj)) return true; if (obj.GetType() != typeof(Qux)) return false; return Equals((Qux)obj); }
            public override int GetHashCode() { return Value; }

            private void Deserialize(dynamic json) { Value = (int)json; }
            private Json Serialize() { return new JsonPrimitive(Value); }
        }

        [Test, Category("Hot")]
        public void Test1()
        {
            typeof(Foo).Config().DefaultEngine().OptOutNonPublic.NotSlots(mi => mi.Name == "Calc");
            typeof(IBar).GetProperty("Qux").Config().AfterDeserialize((Qux qux) => { qux.Value *= 10; return qux; });
            Properties.Rule(pi => pi.DeclaringType == typeof(Foo)).AfterDeserialize((pi, o) => Log.WriteLine("AfterDeserialize Foo::", pi.Name).Ignore());
            typeof(Foo).GetProperty("Ok").Config().Engine((pi, j) => { Log.WriteLine("Deserializing Foo::Ok"); return new DefaultEngine().Deserialize(pi.PropertyType, j); },
                (pi, o) => { Log.WriteLine("Serializing Foo::Ok"); return new DefaultEngine().Serialize(pi, o); });
            typeof(Foo).GetProperty("Ok").Config().AddValidator(_ => Log.WriteLine("Validating Foo::Ok"));
            typeof(Foo).Config().AddValidator(_ => Log.WriteLine("Validating Foo"));
            typeof(Foo).Config().AfterDeserialize(o => Log.WriteLine("AfterDeserialize Foo").Ignore());

            var s_json = InputText();
            var json = Json.Parse(s_json);
            Assert.AreEqual(new Json(false), json[0].ok);
            Assert.AreEqual(false, (bool)json[0].ok);
            Assert.AreEqual(new Json("e/*i,[{]}',[{]}\\,[{]}\",[{]}n//*/"), json[0].bars.ein[0].baz);
            Assert.AreEqual(2, (int)json[0].bars.ein[1].qux);
            Assert.AreEqual(new Json(new {}), json[0].foos[1].bars);
            Assert.AreEqual(new Json(null), json[0].bars.drei);
            Assert.AreEqual(null, (IList<IBar>)json[0].bars.drei);
            // todo. find out why this doesn't work
//            Assert.IsTrue(Seq.Equal(new []{0, 1}, json.Keys));
            Assert.IsTrue(Seq.Equal(new []{0, 1}, ((IEnumerable<Object>)json.Keys).Cast<int>()));
            Assert.IsTrue(Seq.Equal(new []{"ok", "bars", "foos"}, json[0].Keys));

            var anon = new Object[]
            {
                new
                {
                    ok = false, 
                    bars = new
                    {
                        ein = new []
                        {
                            new { qux = 1, baz = "e/*i,[{]}',[{]}\\,[{]}\",[{]}n//*/" },
                            new { qux = 2, baz = "" }
                        },
                        zwei = new []
                        {
                            new { qux = 1 },
                        },
                        drei = (Object)null
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
            Assert.AreEqual(anon.ToJson(), json);
            Assert.AreEqual(anon.ToJson(), ((Json)json).ToJson());
            Assert.AreEqual(anon.ToJson().ToCompactString(), json.ToCompactString());
            Assert.AreEqual(anon.ToJson().ToPrettyString(), json.ToPrettyString());

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
                            Assert.AreEqual(new Qux(10), bar0_value0.Qux); // property adapter!
                            Assert.AreEqual("e/*i,[{]}',[{]}\\,[{]}\",[{]}n//*/", bar0_value0.Baz);
                        var bar0_value1 = bar0_value[1];
                            Assert.AreEqual(new Qux(20), bar0_value1.Qux); // property adapter!
                            Assert.AreEqual("", bar0_value1.Baz);
                    var bar1 = foo0_bars.AsEnumerable().Nth(1);
                        Assert.AreEqual("zwei", bar1.Key);
                        var bar1_value = bar1.Value;
                        Assert.AreEqual(1, bar1_value.Count);
                        var bar1_value0 = bar1_value[0];
                            Assert.AreEqual(new Qux(10), bar1_value0.Qux); // property adapter!
                            Assert.AreEqual("null", bar1_value0.Baz); // type adapter!
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

            Log.WriteLine(Environment.NewLine + "FINAL RESULT:");
            Log.WriteLine(foos.ToJson().ToPrettyString());
            VerifyResult(Out.ToString());
        }
    }
}