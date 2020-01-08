using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using МатКлассы;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Тесты
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var arr = new int[] { 2, 3, 4, 4, 5, 6, 4, 3, 8 };
            var s = Expendator.IndexesWhichGetSum(arr, 9);
            int sum = 0;
            for (int i = 0; i < s.Length; i++)
                sum+=arr[s[i]];

            Assert.AreEqual(sum, 9);
        }

        [TestMethod]
        public void TestGetRandom()
        {
            var numbers = new int[] { 1, 2, 3, 4, 5 };
            var probs = new double[] { 1, 2, 0, 2, 1 };

            var k = Enumerable.Range(0, 100).Select(s => Expendator.GetRandomNumberFromArrayWithProbabilities(numbers, probs)).ToArray();

            var k1= k.Count(p => p == numbers[0]);
            var k2 = k.Count(p => p == numbers[1]);
            var k3 = k.Count(p => p == numbers[2]);
            var k4 = k.Count(p => p == numbers[3]);
            var k5 = k.Count(p => p == numbers[4]);
            $"".Show();
        }
    }
}
