namespace Qocr.Core.Interfaces
{
    /// <summary>
    /// <see cref="IMonomap"/> с возможностью редактирования цвета ячеек.
    /// </summary>
    internal interface IEditMonomap : IMonomap
    {
        /// <summary>
        /// Получение бита изображения.
        /// </summary>
        /// <param name="x">Значение координаты Х.</param>
        /// <param name="y">Значение координаты Y.</param>
        /// <returns>Значение закрашена или нет точка.</returns>
        new bool this[int x, int y] { get; set; }
    }
}