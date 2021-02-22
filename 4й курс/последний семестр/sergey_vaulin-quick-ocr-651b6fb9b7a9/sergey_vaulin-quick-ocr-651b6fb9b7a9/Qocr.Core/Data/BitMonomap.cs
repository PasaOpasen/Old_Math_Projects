using System;
using System.Drawing;

using Qocr.Core.Interfaces;

namespace Qocr.Core.Data
{
    /// <summary>
    /// Реализация <see cref="IMonomap"/> для работы с бинарным массивом.
    /// </summary>
    public class BitMonomap : MonomapBase
    {
        private readonly bool[,] _imageCode;

        /// <summary>
        /// Создание экземпляра класса <see cref="BitMonomap"/>.
        /// </summary>
        public BitMonomap(bool[,] imageCode)
        {
            if (imageCode == null)
            {
                throw new ArgumentNullException(nameof(imageCode));
            }

            if (imageCode.Length == 0)
            {
                throw new ArgumentException("Array empty", nameof(imageCode));
            }

            _imageCode = imageCode;
        }
        
        /// <inheritdoc/>
        public override int Width => _imageCode.GetLength(0);

        /// <inheritdoc/>
        public override int Height => _imageCode.GetLength(1);

        /// <inheritdoc/>
        public override bool this[int x, int y]
        {
            get
            {
                return _imageCode[x, y];
            }
        } 
    }
}