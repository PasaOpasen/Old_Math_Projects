using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Qocr.Core.Data.Map2D;
using Qocr.Core.Data.Serialization;

namespace Qocr.UnitTests
{
    [TestClass]
    public class SerializationTests
    {
        private const string EulerValue = "1,2,3,4,5,6,7,8,9";
        private readonly string _defaultCodeValue = $"{EulerValue}{Symbol.SetSplitter}10";

        [TestMethod]
        public void SerializationValues1()
        {
            var symbol = CreateSymbol(_defaultCodeValue);
            Assert.AreEqual(symbol.Codes.Count, 1);
        }

        [TestMethod]
        public void SerializationValues2()
        {
            var symbol = CreateSymbol($"{_defaultCodeValue}{Symbol.Seporator}");
            Assert.AreEqual(symbol.Codes.Count, 1);
        }

        [TestMethod]
        public void SerializationValues3()
        {
            var symbol = CreateSymbol($"{Symbol.Seporator}");
            Assert.AreEqual(symbol.Codes.Count, 0);
        }

        [TestMethod]
        public void SerializationValues4()
        {
            var symbol = CreateSymbol(string.Empty);
            Assert.AreEqual(symbol.Codes.Count, 0);
        }

        [TestMethod]
        public void SerializationValues5()
        {
            var symbol = CreateSymbol($"{_defaultCodeValue}{Symbol.Seporator}{_defaultCodeValue}");

            // Так как HashSet
            Assert.AreEqual(symbol.Codes.Count, 1);
        }

        [TestMethod]
        public void SerializationValues6()
        {
            var symbol = CreateSymbol($"{_defaultCodeValue}{Symbol.Seporator}1,2,3,4,5,6,7,8,9{Symbol.SetSplitter}12");
            Assert.AreEqual(symbol.Codes.Count, 1);
        }

        [TestMethod]
        public void ActualDataSerializationTest()
        {
            var language = new Language
            {
                LocalizationName = "RU-ru",
                FontFamilyNames = new List<string>() { "SomeFont" },
                Chars = new List<Symbol>
                {
                    new Symbol
                    {
                        Chr = 'Б',
                        Codes = new HashSet<SymbolCode>(
                            new[]
                            {
                                new SymbolCode(10, new EulerMonomap2D(EulerValue))
                            })
                    }
                }
            };

            var eulerContainer = new EulerContainer()
            {
                Languages = new List<Language>() { language },
                SpecialChars = new List<Symbol>() { new Symbol() { Chr = 'Ж' } }
            };

            using (var compressed = CompressionUtils.Compress(eulerContainer))
            {
                CompressionUtils.Decompress<EulerContainer>(compressed);
            }
        }

        private static Symbol CreateSymbol(string rawString)
        {
            Symbol symbol = new Symbol { StringsCodes = rawString };

            symbol.OnDeserializedMethod(new StreamingContext());

            return symbol;
        }
    }
}