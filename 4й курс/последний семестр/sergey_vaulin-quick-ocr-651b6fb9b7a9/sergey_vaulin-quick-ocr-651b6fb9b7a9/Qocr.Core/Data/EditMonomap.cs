using Qocr.Core.Interfaces;

namespace Qocr.Core.Data
{
    /// <summary>
    /// Monomap с возможностью редактирования битов.
    /// </summary>
    internal class EditMonomap : IEditMonomap
    {
        private readonly bool[,] _bitmap;

        private const int Thickness = 1;

        private const int BorderThickness = 2 * Thickness;

        /// <summary>
        /// Создание экземпляра класса <see cref="EditMonomap"/>.
        /// </summary>
        public EditMonomap(bool[,] bitmap)
            : this(new BitMonomap(bitmap))
        {
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="EditMonomap"/>.
        /// </summary>
        public EditMonomap(IMonomap monomap)
        {
            // Вокруг изображения добавил рамку в 1 пиксель
            _bitmap = new bool[monomap.Width + BorderThickness, monomap.Height + BorderThickness];

            for (int x = 0; x < monomap.Width; x++)
            {
                for (int y = 0; y < monomap.Height; y++)
                {
                    _bitmap[x + Thickness, y + Thickness] = monomap[x, y];
                }
            }
        }

        /// <inheritdoc/>
        public int Width => _bitmap.GetLength(0) - BorderThickness;

        /// <inheritdoc/>
        public int Height => _bitmap.GetLength(1) - BorderThickness;

        /// <inheritdoc/>
        public bool this[int x, int y]
        {
            get
            {
                return _bitmap[x + Thickness, y + Thickness];
            }

            set
            {
                _bitmap[x + Thickness, y + Thickness] = value;
            }
        }
    }
}