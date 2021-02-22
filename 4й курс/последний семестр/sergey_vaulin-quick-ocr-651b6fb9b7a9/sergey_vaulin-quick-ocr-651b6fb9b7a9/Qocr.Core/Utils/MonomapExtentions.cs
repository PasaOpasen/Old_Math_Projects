using System;
using System.Drawing;

using Qocr.Core.Data;
using Qocr.Core.Interfaces;

namespace Qocr.Core.Utils
{
    /// <summary>
    /// Расширения для работы с <see cref="IMonomap"/>.
    /// </summary>
    public static class MonomapExtentions
    {
        /// <summary>
        /// Копировать <see cref="IMonomap"/>.
        /// </summary>
        /// <param name="monomap">Ссылка на <see cref="IMonomap"/>.</param>
        /// <returns>Ссылка на скопированный <see cref="IMonomap"/>.</returns>
        // TODO Пока не хочется <see cref="ICloneable"/> обязывать реализовывать.
        public static IMonomap Clone(this IMonomap monomap)
        {
            bool[,] newMonomap = new bool[monomap.Width, monomap.Height];
            for (int x = 0; x < monomap.Width; x ++)
            {
                for (int y = 0; y < monomap.Height; y++)
                {
                    newMonomap[x, y] = monomap[x, y];
                }
            }

            return new BitMonomap(newMonomap);
        }

        /// <summary>
        /// Преобразовать <see cref="IMonomap"/> в <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="monomap">Ссылка на <see cref="IMonomap"/>.</param>
        /// <returns>Ссылка на <see cref="Bitmap"/>.</returns>
        public static Bitmap ToBitmap(this IMonomap monomap)
        {
            var bitmap = new Bitmap(monomap.Width, monomap.Height);
            for (var y = 0; y < monomap.Height; y++)
            {
                for (var x = 0; x < monomap.Width; x++)
                {
                    bitmap.SetPixel(x, y, monomap[x, y] ? Color.Black : Color.White);
                }
            }

            return bitmap;
        }
    }
}