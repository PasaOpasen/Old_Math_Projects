using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Qocr.Core.Data;
using Qocr.Core.Interfaces;
using Qocr.Core.Recognition.Data;

namespace Qocr.Core.Recognition.Logic
{
    /// <summary>
    /// Сканер, для поиска изображений.
    /// </summary>
    public class DefaultScanner : IScanner
    {
        private readonly Point[] _nearbyCells = {
            // [X] [ ] [ ]
            // [ ] [O] [ ]
            // [ ] [ ] [ ]
            new Point(-1, -1),

            // [ ] [X] [ ]
            // [ ] [O] [ ]
            // [ ] [ ] [ ]
            new Point(0, -1),

            // [ ] [ ] [X]
            // [ ] [O] [ ]
            // [ ] [ ] [ ]
            new Point(1, -1),

            // [ ] [ ] [ ]
            // [X] [O] [ ]
            // [ ] [ ] [ ]
            new Point(-1, 0),

            // [ ] [ ] [ ]
            // [ ] [O] [X]
            // [ ] [ ] [ ]
            new Point(1, 0),

            // [ ] [ ] [ ]
            // [ ] [O] [ ]
            // [X] [ ] [ ]
            new Point(-1, 1),

            // [ ] [ ] [ ]
            // [ ] [O] [ ]
            // [ ] [X] [ ]
            new Point(0, 1),

            // [ ] [ ] [ ]
            // [ ] [O] [ ]
            // [ ] [ ] [X]
            new Point(1, 1),
        };

        /// <summary>
        /// Распарсить изображение на более меньшие.
        /// </summary>
        /// <param name="sourceImage">Исходное распознаваемое изображение.</param>
        /// <returns>Список найденных фрагментов.</returns>
        public IList<QSymbol> GetFragments(IMonomap sourceImage)
        {
            var result = new List<QSymbol>();
            var editMonomap = new EditMonomap(sourceImage);

            for (int y = 0; y < editMonomap.Height; y++)
            {
                for (int x = 0; x < editMonomap.Width; x++) 
                {
                    if (editMonomap[x, y])
                    {
                        // Заполняем первую точку, что бы внутри метода проверку не делать для заполнения.
                        BitmapPad pad = new BitmapPad();
                        pad.SetPoint(x, y);

                        // Начинаем рекурсивное создание фигуры
                        FillBitmapPad(x, y, editMonomap, pad);

                        var symbol = new QSymbol(pad, pad.TopLeftPoint, EulerCharacteristicComputer.Compute2D(pad));
                        result.Add(symbol);
                    }
                }
            }

            return result;
        }

        private void FillBitmapPad(int x, int y, EditMonomap monomap, BitmapPad pad)
        {
            var recursionStack = new Stack<LightPoint>();
            recursionStack.Push(new LightPoint(x, y));

            do
            {
                var currentPoint = recursionStack.Pop();
                x = currentPoint.X;
                y = currentPoint.Y;

                for (int i = 0; i < _nearbyCells.Length; i++)
                {
                    var nearbyCell = _nearbyCells[i];
                    int nextX = x + nearbyCell.X;
                    int nextY = y + nearbyCell.Y;

                    if (IsBlackCheckNearbyCell(nextX, nextY, monomap, pad))
                    {
                        recursionStack.Push(new LightPoint(nextX, nextY));
                    }
                }
            } while (recursionStack.Any());
        }

        private bool IsBlackCheckNearbyCell(int x, int y, IEditMonomap monomap, BitmapPad pad)
        {
            bool isBlack = monomap[x, y];
            if (isBlack)
            {
                pad.SetPoint(x, y);
                monomap[x, y] = false;
            }

            return isBlack;
        }

        private struct LightPoint
        {
            /// <summary>
            /// Создание экземпляра класса <see cref="LightPoint"/>.
            /// </summary>
            public LightPoint(int x, int y)
            {
                X = x;
                Y = y;
            }

            /// <summary>
            /// Значение Х.
            /// </summary>
            public int X { get; }

            /// <summary>
            /// Значение Y.
            /// </summary>
            public int Y { get; }
        }
    }
}