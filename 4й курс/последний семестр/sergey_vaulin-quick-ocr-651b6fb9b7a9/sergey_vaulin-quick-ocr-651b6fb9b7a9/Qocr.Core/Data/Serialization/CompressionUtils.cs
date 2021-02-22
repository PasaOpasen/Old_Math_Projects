using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Qocr.Core.Data.Serialization
{
    /// <summary>
    /// Методы для работы со сжатыми данными нужными для распознавания.
    /// </summary>
    public static class CompressionUtils
    {
        /// <summary>
        /// Распаковка объекта тип {TResult} из потока.
        /// </summary>
        /// <typeparam name="TResult">De-serialization тип.</typeparam>
        /// <param name="sourceStream">Исходный поток с данными объекта.</param>
        /// <returns></returns>
        public static TResult Decompress<TResult>(Stream sourceStream)
        {
            sourceStream.Position = 0;
            using (var stream = new DeflateStream(sourceStream, CompressionMode.Decompress))
            {
                var serializer = new DataContractSerializer(typeof(TResult));
                return (TResult)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Упаковать объект.
        /// </summary>
        /// <param name="data">Ссылка на объект.</param>
        /// <returns>Поток со сжатыми данными.</returns>
        public static Stream Compress(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var compressedStream = new MemoryStream();
            using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress, true))
            {
                // Форматирование в XML не нужен
                using (var xmlWriter = XmlWriter.Create(deflateStream, new XmlWriterSettings { Indent = false }))
                {
                    var serializer = new DataContractSerializer(data.GetType());
                    serializer.WriteObject(xmlWriter, data);
                    return compressedStream;
                }
            }
            
        }
    }
}