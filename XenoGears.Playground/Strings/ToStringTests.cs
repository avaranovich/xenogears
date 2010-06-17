using NUnit.Framework;
using XenoGears.Strings;

namespace XenoGears.Playground.Strings
{
    [TestFixture]
    public class ToStringTests
    {
        [Test]
        public void TestSZtoAAA()
        {
            Assert.AreEqual("A", 0.SZtoAAA());
            Assert.AreEqual("Z", 25.SZtoAAA());
            Assert.AreEqual("AA", 26.SZtoAAA());
            Assert.AreEqual("AZ", 51.SZtoAAA());
            Assert.AreEqual("BA", 52.SZtoAAA());
            Assert.AreEqual("PR", 433.SZtoAAA());
            Assert.AreEqual("YZ", 675.SZtoAAA());
            Assert.AreEqual("ZZ", 701.SZtoAAA());
            Assert.AreEqual("AAA", 702.SZtoAAA());
            Assert.AreEqual("AAZ", 727.SZtoAAA());
            Assert.AreEqual("ABA", 728.SZtoAAA());
            Assert.AreEqual("AYZ", 1351.SZtoAAA());
            Assert.AreEqual("AZA", 1352.SZtoAAA());
            Assert.AreEqual("AZZ", 1377.SZtoAAA());
            Assert.AreEqual("BAA", 1378.SZtoAAA());
        }
    }
}