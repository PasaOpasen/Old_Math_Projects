using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Qocr.Core.Data;
using Qocr.Core.Recognition.Data;

namespace Qocr.UnitTests
{
    [TestClass]
    public class BitmapPadTests
    {
        [TestMethod]
        public void OnePointTest()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(1000, 1000);

            Assert.AreEqual(pad.Height, 1);
            Assert.AreEqual(pad.Width, 1);
            Assert.AreEqual(pad[0, 0], true);
        }

        [TestMethod]
        public void Square4x4TestNegativeCoorinates()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(1000, 1000);
            pad.SetPoint(999, 999);
            pad.SetPoint(1000, 999);
            pad.SetPoint(999, 1000);

            Assert.AreEqual(pad.Height, 2);
            Assert.AreEqual(pad.Width, 2);

            Assert.AreEqual(pad[0, 0], true);
            Assert.AreEqual(pad[1, 1], true);
            Assert.AreEqual(pad[0, 1], true);
            Assert.AreEqual(pad[1, 0], true);
        }

        [TestMethod]
        public void Square4x4TestPositiveCoorinates()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(1000, 1000);
            pad.SetPoint(1001, 1001);
            pad.SetPoint(1000, 1001);
            pad.SetPoint(1001, 1000);

            Assert.AreEqual(pad.Height, 2);
            Assert.AreEqual(pad.Width, 2);

            Assert.AreEqual(pad[0, 0], true);
            Assert.AreEqual(pad[1, 1], true);
            Assert.AreEqual(pad[0, 1], true);
            Assert.AreEqual(pad[1, 0], true);
        }

        [TestMethod]
        public void LineTest()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(1000, 1000);
            pad.SetPoint(999, 999);
            pad.SetPoint(1001, 1001);

            Assert.AreEqual(pad.Height, 3);
            Assert.AreEqual(pad.Width, 3);

            Assert.AreEqual(pad[0, 0], true);
            Assert.AreEqual(pad[1, 1], true);
            Assert.AreEqual(pad[2, 2], true);

            Assert.AreEqual(pad[1, 0], false);
            Assert.AreEqual(pad[2, 0], false);

            Assert.AreEqual(pad[0, 1], false);
            Assert.AreEqual(pad[2, 1], false);

            Assert.AreEqual(pad[0, 2], false);
            Assert.AreEqual(pad[1, 2], false);
        }

        [TestMethod]
        public void CleanTest()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(20, 20);

            Assert.AreEqual(pad.Height, 1);
            Assert.AreEqual(pad.Width, 1);

            pad.ClearPad();

            Assert.AreEqual(pad.Height, 0);
            Assert.AreEqual(pad.Width, 0);

            pad.SetPoint(1000, 1000);

            Assert.AreEqual(pad.Height, 1);
            Assert.AreEqual(pad.Width, 1);
        }

        [TestMethod]
        public void ExternalPointsTest()
        {
            //                1000
                   // [ ] [X] [ ] [ ]
            //1000 // [ ] [ ] [X] [X]
                   // [ ] [ ] [X] [X]
                   // [ ] [ ] [ ] [ ]
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(1000, 1000);
            pad.SetPoint(999, 999);
            pad.SetPoint(1001, 1000);
            pad.SetPoint(1000, 1001);
            pad.SetPoint(1001, 1001);

            Assert.AreEqual(pad.Width, 3);
            Assert.AreEqual(pad.Height, 3);

            Assert.AreEqual(pad[0, 0], true);
            Assert.AreEqual(pad[1, 1], true);
            Assert.AreEqual(pad[2, 1], true);
            Assert.AreEqual(pad[1, 2], true);
            Assert.AreEqual(pad[2, 2], true);

            var tl = pad.TopLeftPoint;
            Assert.AreEqual(tl.X, 999);
            Assert.AreEqual(tl.Y, 999);

            var rb = pad.RightBottomPoint;
            Assert.AreEqual(rb.X, 1001);
            Assert.AreEqual(rb.Y, 1001);
        }

        [TestMethod]
        public void StretchPadSizeTest()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(0, 0);

            StretchPad stretchPad = new StretchPad(pad);

            Assert.AreEqual(stretchPad.Height, 3);
            Assert.AreEqual(stretchPad.Width, 3);
        }

        [TestMethod]
        public void StretchPadContantTest()
        {
            BitmapPad pad = new BitmapPad();
            pad.SetPoint(0, 0);

            StretchPad stretchPad = new StretchPad(pad);

            Assert.IsFalse(stretchPad[0, 0]);
            Assert.IsFalse(stretchPad[1, 0]);
            Assert.IsFalse(stretchPad[2, 0]);

            Assert.IsFalse(stretchPad[0, 1]);
            Assert.IsTrue(stretchPad[1, 1]);
            Assert.IsFalse(stretchPad[2, 1]);

            Assert.IsFalse(stretchPad[0, 2]);
            Assert.IsFalse(stretchPad[1, 2]);
            Assert.IsFalse(stretchPad[2, 2]);
        }
    }
}
