using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Qocr.Core.Data;
using Qocr.Core.Recognition.Data;

namespace Qocr.UnitTests
{
    [TestClass]
    public class EditMonomapTests
    {
        private readonly bool[,] _sourceImage = {
                {
                    true,
                    true
                },
                {
                    true,
                    true
                },
            };

        [TestMethod]
        public void SimpleCreationTest()
        {
            EditMonomap monomap = new EditMonomap(_sourceImage);

            Assert.AreEqual(monomap.Width, _sourceImage.GetLength(0));
            Assert.AreEqual(monomap.Height, _sourceImage.GetLength(1));
        }

        [TestMethod]
        public void AreEqualTest()
        {
            EditMonomap monomap = new EditMonomap(_sourceImage);

            Assert.AreEqual(monomap[0, 0], true);
            Assert.AreEqual(monomap[0, 1], true);

            Assert.AreEqual(monomap[1, 0], true);
            Assert.AreEqual(monomap[1, 1], true); 
        }

        [TestMethod]
        public void OutOfBoundsTest()
        {
            EditMonomap monomap = new EditMonomap(_sourceImage);

            Assert.AreEqual(monomap[-1, -1], false);
            Assert.AreEqual(monomap[-1, 0], false);
            Assert.AreEqual(monomap[0, -1], false);

            Assert.AreEqual(monomap[2, 2], false);
            Assert.AreEqual(monomap[1, 2], false);
            Assert.AreEqual(monomap[2, 1], false);
        }
    }
}
