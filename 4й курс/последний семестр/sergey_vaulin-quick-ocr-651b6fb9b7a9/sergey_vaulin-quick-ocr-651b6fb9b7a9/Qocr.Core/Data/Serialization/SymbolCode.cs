using System;
using System.Diagnostics;
using System.Xml.Serialization;

using Qocr.Core.Data.Map2D;

namespace Qocr.Core.Data.Serialization
{
    /// <summary>
    /// Информация по каждому символу в <see cref="Symbol"/>.
    /// </summary>
    [DebuggerDisplay("{Height}-{EulerCode}")]
    public class SymbolCode
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="SymbolCode"/>.
        /// </summary>
        public SymbolCode(int height, EulerMonomap2D eulerCode)
        {
            if (height <= 0)
            {
                throw new ArgumentException(nameof(height));
            }

            EulerCode = eulerCode;
            Height = height;
        }
        
        /// <summary>
        /// Размер шрифта.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Значение эйлеровой характеристики.
        /// </summary>
        public EulerMonomap2D EulerCode { get; }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return EulerCode.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj?.GetHashCode() == GetHashCode();
        }
    }
}