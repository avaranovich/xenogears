using System.Linq;
using NUnit.Framework;
using XenoGears.Functional;

namespace XenoGears.Playground
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void TestChopEvery()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopEvery(7);
            var expected = new[] {1.UpTo(7), 8.UpTo(14), 15.UpTo(15)};

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopBefore1()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopBefore(v => v % 2 == 1);
            var expected = new int[0].Concat(1.UpTo(13).Step(2).Select(i => new[] { i, i + 1 })).Concat(15.MkArray());

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopBefore2()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopBefore(v => v % 2 == 0);
            var expected = 1.MkArray().Concat(2.UpTo(14).Step(2).Select(i => new[] { i, i + 1 }));

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopAfter1()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopAfter(v => v % 2 == 1);
            var expected = 1.MkArray().Concat(2.UpTo(14).Step(2).Select(i => new[] { i, i + 1 })).Concat(new int[0]);

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopAfter2()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopAfter(v => v % 2 == 0);
            var expected = 1.UpTo(13).Step(2).Select(i => new[] { i, i + 1 }).Concat(15.MkArray());

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopBetween1()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopBetween((v1, v2) => v1 % 2 == 1);
            var expected = 1.MkArray().Concat(2.UpTo(14).Step(2).Select(i => new[] { i, i + 1 }));

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopBetween2()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopBetween((v1, v2) => v1 % 2 == 0);
            var expected = 1.UpTo(13).Step(2).Select(i => new[] { i, i + 1 }).Concat(15.MkArray());

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }

        [Test]
        public void TestChopBetween3()
        {
            var seq = 1.UpTo(15);
            var actual = seq.ChopBetween((v1, v2) => true);
            var expected = Seq.Infinite(i => i.MkArray()).Skip(1).Take(15);

            var theSame = actual.AllMatch(expected, (subseq1, subseq2) => subseq1.SequenceEqual(subseq2));
            Assert.IsTrue(theSame);
        }
    }
}
