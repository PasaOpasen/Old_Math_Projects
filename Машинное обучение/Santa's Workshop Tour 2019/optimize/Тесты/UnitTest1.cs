using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using МатКлассы;
using System.Diagnostics;

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
    }
}
