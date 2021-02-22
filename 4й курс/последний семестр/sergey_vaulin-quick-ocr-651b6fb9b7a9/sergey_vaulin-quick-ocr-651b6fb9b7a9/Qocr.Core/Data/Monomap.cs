using System;
using System.Drawing;

namespace Qocr.Core.Data
{
    /// <summary>
    /// Реализация IMonomap для работы с <see cref="Bitmap"/>.
    /// </summary>
    public class Monomap : MonomapBase
    {
        private readonly bool[,] _monoImage;
        
        /// <summary>
        /// Создание экземпляра класса <see cref="Monomap"/>.
        /// </summary>
        public Monomap(Bitmap image, Color blackColor)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            _monoImage = new bool[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    _monoImage[x, y] = image.GetPixel(x, y) == blackColor;
                }
            }
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="Monomap"/>.
        /// </summary>
        public Monomap(Bitmap image) : this(image, Color.FromArgb(0, 0, 0))
        {
        }

        /// <inheritdoc/>
        public override int Width => _monoImage.GetLength(0);

        /// <inheritdoc/>
        public override int Height => _monoImage.GetLength(1);

        /// <inheritdoc/>
        public override bool this[int x, int y] => _monoImage[x, y];
    }
}