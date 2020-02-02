using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incapsulation.RationalNumbers
{
    [TestFixture]
    public class Rational_should
    {
        public void AssertEqual(int expectedNumerator, int expectedDenominator, Rational actual)
        {
            Assert.False(actual.IsNan);
            Assert.AreEqual(expectedNumerator, actual.Numerator);
            Assert.AreEqual(expectedDenominator, actual.Denominator);
        }

        [Test]
        public void InitializeSimpleRatioCorrectly()
        {
            AssertEqual(1, 2, new Rational(1, 2));
        }

        [Test]
        public void InitializeWitoutDenomerator()
        {
            AssertEqual(4, 1, new Rational(4));
        }

        [Test]
        public void InitializeWithZeroDenomerator()
        {
            Assert.True(new Rational(2, 0).IsNan);
        }

        public void BeCorrectWithZeroNumerator()
        {
            AssertEqual(0, 1, new Rational(0, 5));
        }

        [TestCase(1, 2, 2, 4)]
        [TestCase(-1, 2, -2, 4)]
        [TestCase(-1, 2, 2, -4)]
        [TestCase(1, 2, -2, -4)]
        [TestCase(1, 2, 1, 2)]
        [TestCase(1, 2, 8, 16)]
        [TestCase(2, 3, 10, 15)]
        [TestCase(4, 7, 16, 28)]
        [TestCase(3, 256, 12, 1024)]
        [TestCase(1, 1, 1, 1)]
        public void InitializeAndReduce1(int expectedNum, int expectedDen, int num, int den)
        {
            AssertEqual(expectedNum, expectedDen, new Rational(num, den));
        }

        [Test]
        public void Sum()
        {
            AssertEqual(1, 2, new Rational(1, 4) + new Rational(1, 4));
        }

        [Test]
        public void SumWithNan()
        {
            Assert.True((new Rational(1, 2) + new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) + new Rational(1, 2)).IsNan);
        }

        [Test]
        public void Subtract()
        {
            AssertEqual(1, 4, new Rational(1, 2) - new Rational(1, 4));
        }

        [Test]
        public void SubtractWithNan()
        {
            Assert.True((new Rational(1, 2) - new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) - new Rational(1, 2)).IsNan);
        }

        [Test]
        public void Multiply()
        {
            AssertEqual(-1, 4, new Rational(-1, 2) * new Rational(1, 2));
        }

        [Test]
        public void MultiplyWithNan()
        {
            Assert.True((new Rational(1, 2) * new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) * new Rational(1, 2)).IsNan);
        }

        [Test]
        public void Divide()
        {
            AssertEqual(-1, 2, new Rational(1, 4) / new Rational(-1, 2));
        }

        [Test]
        public void DivideWithNan()
        {
            Assert.True((new Rational(1, 2) / new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) / new Rational(1, 2)).IsNan);
        }

        [Test]
        public void DivideToZero()
        {
            Assert.True((new Rational(1, 2) / new Rational(0, 5)).IsNan);
        }

        [TestCase(1, 2, 0.5d)]
        [TestCase(10, 5, 2d)]
        [TestCase(-1, 5, -0.2d)]
        [TestCase(10, 0, double.NaN)]
        [TestCase(-10, 0, double.NaN)]
        [TestCase(0, 0, double.NaN)]
        public void ConvertToDouble(int numerator, int denominator, double expectedValue)
        {
            double v = new Rational(numerator, denominator);
            Assert.AreEqual(expectedValue, v, 1e-7);
        }

        [Test]
        public void ConvertFromInt()
        {
            Rational r = 5;
            AssertEqual(5, 1, r);
        }

        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(3, 1, 3)]
        [TestCase(2, 2, 1)]
        [TestCase(6, 3, 2)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 3, 4)]
        [TestCase(12, 4, 3)]
        [TestCase(12, 6, 2)]
        [TestCase(12, 12, 1)]
        [TestCase(1000, 1, 1000)]
        public void ExplicitlyConvertToInt(int numerator, int denominator, int expectedValue)
        {
            int a = (int)new Rational(numerator, denominator);
            Assert.AreEqual(expectedValue, a);
        }

        [TestCase(1, 2)]
        [TestCase(12, 5)]
        [TestCase(12, 10)]
        [TestCase(25, 8)]
        [TestCase(2, 3)]
        [TestCase(2, 4)]
        public void ExplicitlyConvertToIntAndFailsIfNonCorvertable(int numerator, int denominator)
        {
            Assert.Catch<Exception>(() => { int a = (int)new Rational(numerator, denominator); });
        }
    }
}
