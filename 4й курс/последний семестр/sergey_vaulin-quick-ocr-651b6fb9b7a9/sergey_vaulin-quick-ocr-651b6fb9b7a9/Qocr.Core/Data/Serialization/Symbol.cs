using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Qocr.Core.Data.Map2D;

namespace Qocr.Core.Data.Serialization
{
    /// <summary>
    /// Сериализуемая информация о символе.
    /// </summary>
    [DebuggerDisplay("{Chr} ({Codes.Count})")]
    public class Symbol
    {
        /// <summary>
        /// Разделитель всего сериализуемого значения.
        /// </summary>
        internal const string Seporator = ";";

        /// <summary>
        /// Разделитель для свойств сериализуемого значения.
        /// </summary>
        internal const char SetSplitter = '/';

        /// <summary>
        /// Создание экземпляра класса <see cref="Symbol"/>.
        /// </summary>
        public Symbol()
        {
            Codes = new HashSet<SymbolCode>();
        }

        /// <summary>
        /// Символ.
        /// </summary>
        public char Chr { get; set; }

        /// <summary>
        /// Коды символа <see cref="Chr"/>.
        /// </summary>
        [IgnoreDataMember]
        public HashSet<SymbolCode> Codes { get; set; }

        /// <summary>
        /// Строка из кодов.
        /// </summary>
        public string StringsCodes { get; set; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            StringsCodes = string.Join(Seporator, Codes.OrderBy(code => code.Height).Select(code => $"{code.EulerCode}{SetSplitter}{code.Height}"));
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Codes = GetData(StringsCodes);
        }

        private static HashSet<SymbolCode> GetData(string sourceString)
        {
            var splitter = new[]
            {
                Seporator
            };

            var result = new HashSet<SymbolCode>();
            foreach (
                var symbolCode in
                    (sourceString ?? string.Empty).Split(splitter, StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.Split(SetSplitter))
                        .Where(str => str.Any())
                        .Select(split => new { euler = new EulerMonomap2D(split[0]), FontSize = int.Parse(split[1]), })
                        .Select(item => new SymbolCode(item.FontSize, item.euler)))
            {
                result.Add(symbolCode);
            }

            return result;
        }
    }
}