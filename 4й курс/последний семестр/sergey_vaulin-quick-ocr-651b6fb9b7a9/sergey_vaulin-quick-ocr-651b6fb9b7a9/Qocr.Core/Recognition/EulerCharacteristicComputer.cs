using System.Collections.Generic;
using System.Linq;

using Qocr.Core.Data.Attributes;
using Qocr.Core.Data.Map2D;
using Qocr.Core.Interfaces;
using Qocr.Core.Recognition.Data;

namespace Qocr.Core.Recognition
{
    /// <summary>
    /// Класс для вычисления Эйлеровой характеристики изображения.
    /// </summary>
    public static class EulerCharacteristicComputer
    {
        private static readonly Square2D[] EulerSquares2D = new[]
        {
            "0100",
            "0001",
            "1000",
            "0010",
            "1100",
            "0101",
            "0011",
            "1010",
            "0110",
            "1001",
            "1101",
            "1011",
            "0111",
            "1110",
            "1111"
        }.Select(item => new Square2D(item)).ToArray();

        /// <summary>
        /// Высчитать эйлеровоую характеристику для 2D imageSource.
        /// </summary>
        /// <param name="imageSource">Ссылка на изображение.</param>
        /// <returns>Эйлеровая характеристика.</returns>
        public static EulerMonomap2D Compute2D(IMonomap imageSource)
        {
            Dictionary<string, int> eulerValue = EulerSquares2D.ToDictionary(item => item.SquareIdent, item => 0);
            var fragment2DSize = 2;

            IMonomap stretchPad = new StretchPad(imageSource);
            for (int y = 0; y < stretchPad.Height - fragment2DSize + 1; y++)
            {
                for (int x = 0; x < stretchPad.Width - fragment2DSize + 1; x++)
                {
                    for (int i = 0; i < EulerSquares2D.Length; i++)
                    {
                        var eulerSquare = EulerSquares2D[i];
                        if (eulerSquare.IsSquareDetected(x, y, stretchPad))
                        {
                            eulerValue[eulerSquare.SquareIdent]++;
                            break;
                        }
                    }
                }
            }

            return new EulerMonomap2D(eulerValue);
        }
    }
}