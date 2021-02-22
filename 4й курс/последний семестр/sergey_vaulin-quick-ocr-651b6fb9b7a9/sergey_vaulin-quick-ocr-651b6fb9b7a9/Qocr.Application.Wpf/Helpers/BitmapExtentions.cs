using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Qocr.Application.Wpf.Helpers
{
    /// <summary>
    /// Набор расширений для <see cref="Bitmap"/>.
    /// </summary>
    public static class BitmapExtentions
    {
        /// <summary>
        /// <see cref="ImageSource"/> в <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="imageSource">Ссылка на <see cref="ImageSource"/>.</param>
        /// <returns>Ссылка на <see cref="Bitmap"/>.</returns>
        public static Bitmap ToBitmap(this ImageSource imageSource)
        {
            return ToBitmap((BitmapSource)imageSource);
        }

        /// <summary>
        /// <see cref="BitmapSource"/> в <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="bitmapsource">Ссылка на <see cref="BitmapSource"/>.</param>
        /// <returns>Ссылка на <see cref="Bitmap"/>.</returns>
        public static Bitmap ToBitmap(this BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        /// <summary>
        /// <see cref="Bitmap"/> в <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="bitmap">Ссылка на <see cref="Bitmap"/>.</param>
        /// <returns>Ссылка на <see cref="BitmapSource"/>.</returns>
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}