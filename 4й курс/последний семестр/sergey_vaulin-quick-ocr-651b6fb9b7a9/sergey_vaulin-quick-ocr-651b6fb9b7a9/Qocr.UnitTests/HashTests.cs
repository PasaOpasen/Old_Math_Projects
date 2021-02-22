using Microsoft.VisualStudio.TestTools.UnitTesting;

using Qocr.Core.Data.Map2D;

namespace Qocr.UnitTests
{
    [TestClass]
    public class HashTests
    {
        [TestMethod]
        public void TwoHashsTest()
        {
            EulerMonomap2D euler1 = new EulerMonomap2D("0,1");
            EulerMonomap2D euler2 = new EulerMonomap2D("1,0");
            Assert.AreNotEqual(euler1.GetHashCode(), euler2.GetHashCode());
        }

        [TestMethod]
        public void TwoHashsTest2()
        {
            EulerMonomap2D euler1 = new EulerMonomap2D("1");
            EulerMonomap2D euler2 = new EulerMonomap2D("0,0,0,0,0,0,0,0,0,0,0,0,0,0,1");
            Assert.AreNotEqual(euler1.GetHashCode(), euler2.GetHashCode());
        }

        [TestMethod]
        public void EqualHashsTest()
        {
            EulerMonomap2D euler1 = new EulerMonomap2D("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1");
            EulerMonomap2D euler2 = new EulerMonomap2D("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1");
            Assert.AreEqual(euler1.GetHashCode(), euler2.GetHashCode());
        }

        [TestMethod]
        public void NotEqualHashsTest()
        {
            EulerMonomap2D euler1 = new EulerMonomap2D("1,0,1,0,1,0,1,0,1,0,1,0,1,0,1");
            EulerMonomap2D euler2 = new EulerMonomap2D("0,1,0,1,0,1,0,1,0,1,0,1,0,1,0");
            Assert.AreNotEqual(euler1.GetHashCode(), euler2.GetHashCode());
        }

        [TestMethod]
        public void BoundsTest()
        {
            EulerMonomap2D euler = new EulerMonomap2D("30,30,30,30,30,30,30,30,30,30,30,30,30,30,30");
            var hashCode = euler.GetHashCode();
            Assert.IsTrue(true);
        }
    }
}