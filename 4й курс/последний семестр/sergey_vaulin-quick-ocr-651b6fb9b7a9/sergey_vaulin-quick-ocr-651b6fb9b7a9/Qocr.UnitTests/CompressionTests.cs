using Microsoft.VisualStudio.TestTools.UnitTesting;

using Qocr.Core.Data.Serialization;

namespace Qocr.UnitTests
{
    [TestClass]
    public class CompressionTests
    {
        [TestMethod]
        public void CompressTest()
        {
            var testClass = new TestClass { IntProperty = 100, StringProperty = "Test" };

            var compressed = CompressionUtils.Compress(testClass);
            var obj = CompressionUtils.Decompress<TestClass>(compressed);

            Assert.AreEqual(testClass.IntProperty, obj.IntProperty);
            Assert.AreEqual(testClass.StringProperty, obj.StringProperty);
        }

        /// <summary>
        /// Тестовый класс.
        /// </summary>
        public class TestClass
        {
            /// <summary>
            /// Строковое поле.
            /// </summary>
            public string StringProperty { get; set; }

            /// <summary>
            /// Цифровое поле.
            /// </summary>
            public int IntProperty { get; set; }
        }
    }
}