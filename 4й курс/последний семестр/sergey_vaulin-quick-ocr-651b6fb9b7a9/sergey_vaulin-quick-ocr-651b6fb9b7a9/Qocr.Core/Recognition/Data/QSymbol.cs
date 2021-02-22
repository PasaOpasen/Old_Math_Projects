using System.Drawing;

using Qocr.Core.Data.Map2D;
using Qocr.Core.Interfaces;

namespace Qocr.Core.Recognition.Data
{
    /// <summary>
    /// Класс образа символа.
    /// </summary>
    public class QSymbol
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="QSymbol"/>.
        /// </summary>
        public QSymbol(IMonomap monomap, Point startPoint, EulerMonomap2D euler)
        {
            Euler = euler;
            Monomap = monomap;
            StartPoint = startPoint;
        }


        /// <summary>
        /// Значение эйлеровой характеристики.
        /// </summary>
        public EulerMonomap2D Euler { get; }


        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        public IMonomap Monomap { get; }

        /// <summary>
        /// Левая верхняя точка изображения.
        /// </summary>
        /// <remarks>Ширину и высоту можно узнать из <see cref="IMonomap"/>.</remarks>
        public Point StartPoint { get; private set; }

        /// <summary>
        /// Ширина изображения.
        /// </summary>
        public int Width => Monomap.Width;

        /// <summary>
        /// Высота изображения.
        /// </summary>
        public int Height => Monomap.Height;
    }
}