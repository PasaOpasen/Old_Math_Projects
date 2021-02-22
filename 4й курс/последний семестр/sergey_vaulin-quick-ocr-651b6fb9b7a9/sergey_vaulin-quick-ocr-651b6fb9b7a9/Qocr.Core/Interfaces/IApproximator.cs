using System.Drawing;

namespace Qocr.Core.Interfaces
{
    /// <summary>
    /// Аппроксимизатор изображения.
    /// </summary>
    public interface IApproximator
    {
        /// <summary>
        /// Аппроксимизировать изображение.
        /// </summary>
        /// <param name="image">Ссылка на исходное изображение.</param>
        /// <returns>Ссылка на <see cref="IMonomap"/>.</returns>
        IMonomap Approximate(Bitmap image);
    }
}
