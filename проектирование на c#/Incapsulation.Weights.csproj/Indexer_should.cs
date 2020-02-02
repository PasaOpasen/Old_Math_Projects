using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    [TestFixture]
    public class Indexer_should
    {
        private readonly double[] range1to4 = { 1, 2, 3, 4 };

        [Test]
        public void HaveCorrectLength()
        {
            // 1, [ 2, 3, ] 4
            var indexer = new Indexer(range1to4, start:1, length:2);
            Assert.AreEqual(2, indexer.Length);
        }

        [TestCase(0, 2)]
        [TestCase(1, 3)]
        public void GetCorrectly(int index, int value)
        {
            var indexer = new Indexer(range1to4, 1, 2);
            Assert.AreEqual(value, indexer[index]);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void SetCorrectly(int index)
        {
            var indexer = new Indexer(range1to4, 1, 2);
            indexer[index] = 10;
            Assert.AreEqual(10, range1to4[1 + index]);
        }

        [Test]
        public void IndexerDoesNotCopyArray()
        {
            // 1, [ 2, 3,] 4
            var indexer1 = new Indexer(range1to4, 1, 2);
            // [1,  2,] 3,  4
            var indexer2 = new Indexer(range1to4, 0, 2);
            indexer1[0] = 100500;
            Assert.AreEqual(100500, indexer2[1]);
        }

        [TestCase(-1, 3)]
        [TestCase(1, -1)]
        [TestCase(1, 10)]
        [TestCase(0, 5)]
        [TestCase(1, 4)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(4, 1)]
        public void ConstructorFails_WhenRangeIsInvalid(int start, int length)
        {
            Assert.Throws(typeof(ArgumentException), () => new Indexer(range1to4, start, length));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 4)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 3)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(4, 0)]
        public void ConstructoNotFail_WhenRangeIsValid(int start, int length)
        {
            new Indexer(range1to4, start, length);
        }

        [TestCase(-1)]
        [TestCase(2)]
        public void IndexerGetter_Fails_WhenIndexIsWrong(int index)
        {
            var indexer = new Indexer(range1to4, 1, 2);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { var a = indexer[index]; });
        }

        [TestCase(-1)]
        [TestCase(2)]
        public void IndexerSetter_Fails_WhenIndexIsWrong(int index)
        {
            var indexer = new Indexer(range1to4, 1, 2);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { indexer[index] = 1; });
        }
    }
}
