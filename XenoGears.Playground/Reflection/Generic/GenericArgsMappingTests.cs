using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using XenoGears.Reflection;
using XenoGears.Reflection.Generics.StructuralTrees;

namespace XenoGears.Playground.Reflection.Generic
{
    [TestFixture]
    public class GenericArgsMappingTests
    {
        [Test]
        public void TestGenericMethodMapping()
        {
            var t = typeof(Generic2<int, double>);
            var f1 = t.GetMethod("Meth");

            var mapping = f1.GetGenericArgsMapping();
            Assert.AreEqual(4, mapping.Count);
            Assert.AreEqual("ret[0][0]", mapping.ElementAt(0).Key);
            Assert.AreEqual("m[0]", mapping.ElementAt(0).Value);
            Assert.AreEqual("a1[1]", mapping.ElementAt(1).Key);
            Assert.AreEqual("m[0]", mapping.ElementAt(1).Value);
            Assert.AreEqual("a0", mapping.ElementAt(2).Key);
            Assert.AreEqual("t[0]", mapping.ElementAt(2).Value);
            Assert.AreEqual("a1[0]", mapping.ElementAt(3).Key);
            Assert.AreEqual("t[0]", mapping.ElementAt(3).Value);

            Assert.AreEqual("T3", f1.SelectStructuralUnit(mapping.ElementAt(0).Key).Name);
            Assert.AreEqual("T3", f1.SelectStructuralUnit(mapping.ElementAt(1).Key).Name);
            Assert.AreEqual("Int32", f1.SelectStructuralUnit(mapping.ElementAt(2).Key).Name);
            Assert.AreEqual("Int32", f1.SelectStructuralUnit(mapping.ElementAt(3).Key).Name);
        }

        private class Generic2<T1, T2>
        {
            public T3[,][] Meth<T3>(T1 o1, Func<T1, T3> o2)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void TestGenericFieldMapping()
        {
            var t = typeof(Generic1<int, double>);
            var f1 = t.GetFieldOrProperty("Field1");
            var f2 = t.GetFieldOrProperty("Field2");

            var mapping1 = f1.GetGenericArgsMapping();
            Assert.AreEqual(1, mapping1.Count);
            Assert.AreEqual("[0][1][0]", mapping1.ElementAt(0).Key);
            Assert.AreEqual("[1]", mapping1.ElementAt(0).Value);

            var mapping2 = f2.GetGenericArgsMapping();
            Assert.AreEqual(1, mapping2.Count);
            Assert.AreEqual("", mapping2.ElementAt(0).Key);
            Assert.AreEqual("[0]", mapping2.ElementAt(0).Value);

            var f1t = t.GetFieldOrPropertyType("Field1");
            var f2t = t.GetFieldOrPropertyType("Field2");
            Assert.AreEqual("Double", f1t.SelectStructuralUnit(mapping1.ElementAt(0).Key).Name);
            Assert.AreEqual("Double", t.SelectStructuralUnit(mapping1.ElementAt(0).Value).Name);
            Assert.AreEqual("Int32", f2t.SelectStructuralUnit(mapping2.ElementAt(0).Key).Name);
            Assert.AreEqual("Int32", t.SelectStructuralUnit(mapping2.ElementAt(0).Value).Name);
        }

        private class Generic1<K, V>
        {
            public List<Dictionary<int, V[]>> Field1 = null;
            public K Field2 = default(K);
        }
    }
}