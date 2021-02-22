using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Qocr.Core.Data;
using Qocr.Core.Data.Map2D;
using Qocr.Core.Interfaces;
using Qocr.Core.Recognition;
using Qocr.Core.Utils;

namespace Qocr.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        private const string ImagesPathTemplate = "Qocr.UnitTests.Images.{0}";

        private readonly string _taxiPng = string.Format(ImagesPathTemplate, "taxi.png");

        private readonly string _malevichPng = string.Format(ImagesPathTemplate, "malevich.png");

        private readonly string _malevichInvertedPng = string.Format(ImagesPathTemplate, "inverted_malevich.png");

        private readonly string _malevichInvertedBorderedPng = string.Format(ImagesPathTemplate, "inverted_malevich_bordered.png"); 

        [TestMethod]
        public void TaxiSize()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_taxiPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                Assert.AreEqual(monomap.Height, 2);
                Assert.AreEqual(monomap.Width, 5);
            }
        }

        [TestMethod]
        public void TaxiCellValues()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_taxiPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                Assert.AreEqual(monomap[0, 0], true);
                Assert.AreEqual(monomap[1, 0], false);
                Assert.AreEqual(monomap[2, 0], true);
                Assert.AreEqual(monomap[3, 0], false);
                Assert.AreEqual(monomap[4, 0], true);

                Assert.AreEqual(monomap[0, 1], false);
                Assert.AreEqual(monomap[1, 1], true);
                Assert.AreEqual(monomap[2, 1], false);
                Assert.AreEqual(monomap[3, 1], true);
                Assert.AreEqual(monomap[4, 1], false);
            }
        }

        [TestMethod]
        public void TaxiEuler()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_taxiPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                var euler = EulerCharacteristicComputer.Compute2D(monomap);
                Assert.AreEqual(euler.S0, 3);
                Assert.AreEqual(euler.S1, 3);
                Assert.AreEqual(euler.S2, 3);
                Assert.AreEqual(euler.S3, 3);

                Assert.AreEqual(euler.S4, 0);
                Assert.AreEqual(euler.S5, 0);
                Assert.AreEqual(euler.S6, 0);
                Assert.AreEqual(euler.S7, 0);

                Assert.AreEqual(euler.S8, 2);
                Assert.AreEqual(euler.S9, 2);

                Assert.AreEqual(euler.S10, 0);
                Assert.AreEqual(euler.S11, 0);
                Assert.AreEqual(euler.S12, 0);
                Assert.AreEqual(euler.S13, 0);

                Assert.AreEqual(euler.S14, 0);
            }
        }

        [TestMethod]
        public void MalevichSize()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                Assert.AreEqual(monomap.Height, 4);
                Assert.AreEqual(monomap.Width, 4);
            }
        }

        [TestMethod]
        public void MalevichCellValues()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                Assert.AreEqual(monomap[0, 0], false);
                Assert.AreEqual(monomap[1, 0], false);
                Assert.AreEqual(monomap[2, 0], false);
                Assert.AreEqual(monomap[3, 0], false);

                Assert.AreEqual(monomap[0, 1], false);
                Assert.AreEqual(monomap[1, 1], true);
                Assert.AreEqual(monomap[2, 1], true);
                Assert.AreEqual(monomap[3, 1], false);

                Assert.AreEqual(monomap[0, 2], false);
                Assert.AreEqual(monomap[1, 2], true);
                Assert.AreEqual(monomap[2, 2], true);
                Assert.AreEqual(monomap[3, 2], false);

                Assert.AreEqual(monomap[0, 3], false);
                Assert.AreEqual(monomap[1, 3], false);
                Assert.AreEqual(monomap[2, 3], false);
                Assert.AreEqual(monomap[3, 3], false);
            }
        }

        [TestMethod]
        public void MalevichEuler()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                var euler = EulerCharacteristicComputer.Compute2D(monomap);
                Assert.AreEqual(euler.S0, 1);
                Assert.AreEqual(euler.S1, 1);
                Assert.AreEqual(euler.S2, 1);
                Assert.AreEqual(euler.S3, 1);

                Assert.AreEqual(euler.S4, 1);
                Assert.AreEqual(euler.S5, 1);
                Assert.AreEqual(euler.S6, 1);
                Assert.AreEqual(euler.S7, 1);

                Assert.AreEqual(euler.S8, 0);
                Assert.AreEqual(euler.S9, 0);

                Assert.AreEqual(euler.S10, 0);
                Assert.AreEqual(euler.S11, 0);
                Assert.AreEqual(euler.S12, 0);
                Assert.AreEqual(euler.S13, 0);

                Assert.AreEqual(euler.S14, 1);
            }
        }

        [TestMethod]
        public void MalevichInvertedSize()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichInvertedPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                Assert.AreEqual(monomap.Height, 4);
                Assert.AreEqual(monomap.Width, 4);
            }
        }

        [TestMethod]
        public void MalevichInvertedCellValues()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichInvertedPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                Assert.AreEqual(monomap[0, 0], true);
                Assert.AreEqual(monomap[1, 0], true);
                Assert.AreEqual(monomap[2, 0], true);
                Assert.AreEqual(monomap[3, 0], true);

                Assert.AreEqual(monomap[0, 1], true);
                Assert.AreEqual(monomap[1, 1], false);
                Assert.AreEqual(monomap[2, 1], false);
                Assert.AreEqual(monomap[3, 1], true);

                Assert.AreEqual(monomap[0, 2], true);
                Assert.AreEqual(monomap[1, 2], false);
                Assert.AreEqual(monomap[2, 2], false);
                Assert.AreEqual(monomap[3, 2], true);

                Assert.AreEqual(monomap[0, 3], true);
                Assert.AreEqual(monomap[1, 3], true);
                Assert.AreEqual(monomap[2, 3], true);
                Assert.AreEqual(monomap[3, 3], true);
            }
        }

        [TestMethod]
        public void MalevichInvertedEuler()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichInvertedPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                var euler = EulerCharacteristicComputer.Compute2D(monomap);
                Assert.AreEqual(euler.S0, 1);
                Assert.AreEqual(euler.S1, 1);
                Assert.AreEqual(euler.S2, 1);
                Assert.AreEqual(euler.S3, 1);

                Assert.AreEqual(euler.S4, 4);
                Assert.AreEqual(euler.S5, 4);
                Assert.AreEqual(euler.S6, 4);
                Assert.AreEqual(euler.S7, 4);

                Assert.AreEqual(euler.S8, 0);
                Assert.AreEqual(euler.S9, 0);

                Assert.AreEqual(euler.S10, 1);
                Assert.AreEqual(euler.S11, 1);
                Assert.AreEqual(euler.S12, 1);
                Assert.AreEqual(euler.S13, 1);

                Assert.AreEqual(euler.S14, 0);
            }
        }

        [TestMethod]
        public void MalevichInvertedImagesHaveSimularEuler()
        {
            using (Stream stream1 = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichInvertedPng))
            using (Bitmap bitmap1 = new Bitmap(stream1))
            {
                using (Stream stream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichInvertedBorderedPng))
                using (Bitmap bitmap2 = new Bitmap(stream2))
                {
                    IMonomap monomap1 = new Monomap(bitmap1);
                    var euler1 = EulerCharacteristicComputer.Compute2D(monomap1);

                    IMonomap monomap2 = new Monomap(bitmap2);
                    var euler2 = EulerCharacteristicComputer.Compute2D(monomap2);

                    Assert.AreEqual(euler1.S0, euler2.S0);
                    Assert.AreEqual(euler1.S1, euler2.S1);
                    Assert.AreEqual(euler1.S2, euler2.S2);
                    Assert.AreEqual(euler1.S3, euler2.S3);

                    Assert.AreEqual(euler1.S4, euler2.S4);
                    Assert.AreEqual(euler1.S5, euler2.S5);
                    Assert.AreEqual(euler1.S6, euler2.S6);
                    Assert.AreEqual(euler1.S7, euler2.S7);

                    Assert.AreEqual(euler1.S8, euler2.S8);
                    Assert.AreEqual(euler1.S9, euler2.S9);

                    Assert.AreEqual(euler1.S10, euler2.S10);
                    Assert.AreEqual(euler1.S11, euler2.S11);
                    Assert.AreEqual(euler1.S12, euler2.S12);
                    Assert.AreEqual(euler1.S13, euler2.S13);

                    Assert.AreEqual(euler1.S14, euler2.S14);
                }
            }
        }
        
        [TestMethod]
        public void DiffEuler()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_malevichInvertedPng))
            using (Bitmap bitmap = new Bitmap(stream))
            {
                IMonomap monomap = new Monomap(bitmap);
                var copyMonomap = monomap.Clone();
                var euler = EulerCharacteristicComputer.Compute2D(monomap);
                var copyEuler = EulerCharacteristicComputer.Compute2D(copyMonomap);

                euler = euler - copyEuler;

                Assert.AreEqual(euler.S0, 0);
                Assert.AreEqual(euler.S1, 0);
                Assert.AreEqual(euler.S2, 0);
                Assert.AreEqual(euler.S3, 0);

                Assert.AreEqual(euler.S4, 0);
                Assert.AreEqual(euler.S5, 0);
                Assert.AreEqual(euler.S6, 0);
                Assert.AreEqual(euler.S7, 0);

                Assert.AreEqual(euler.S8, 0);
                Assert.AreEqual(euler.S9, 0);

                Assert.AreEqual(euler.S10, 0);
                Assert.AreEqual(euler.S11, 0);
                Assert.AreEqual(euler.S12, 0);
                Assert.AreEqual(euler.S13, 0);

                Assert.AreEqual(euler.S14, 0);
            }
        }
    }
}
