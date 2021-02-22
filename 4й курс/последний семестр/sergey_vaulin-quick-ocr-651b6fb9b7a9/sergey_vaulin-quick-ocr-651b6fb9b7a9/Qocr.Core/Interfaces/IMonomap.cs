using System;

namespace Qocr.Core.Interfaces
{
    /// <summary>
    /// Монохромное изображение.
    /// </summary>
    public interface IMonomap
    {
        /// <summary>
        /// Получение бита изображения.
        /// </summary>
        /// <param name="x">Значение координаты Х.</param>
        /// <param name="y">Значение координаты Y.</param>
        /// <returns>Значение закрашена или нет точка.</returns>
        bool this[int x, int y] { get; }

        /// <summary>
        /// Ширина изображения.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Высота изображения.
        /// </summary>
        int Height { get; }
    }
}
