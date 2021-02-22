using System;
using System.Drawing;

namespace Qocr.Generator.Data
{
    /// <summary>
    /// <see cref="EventArgs"/> для события со ссылкой на изображение.
    /// </summary>
    public class BitmapEventArgs : EventArgs
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="BitmapEventArgs"/>.
        /// </summary>
        public BitmapEventArgs(Bitmap generatedBitmap, Font currentFont, char chr)
        {
            GeneratedBitmap = generatedBitmap;
            CurrentFont = currentFont;
            Chr = chr;
        }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        public Bitmap GeneratedBitmap { get; private set; }

        /// <summary>
        /// Ссылка на шрифт изображения.
        /// </summary>
        public Font CurrentFont { get; private set; }

        public char Chr { get; private set; }
    }
}