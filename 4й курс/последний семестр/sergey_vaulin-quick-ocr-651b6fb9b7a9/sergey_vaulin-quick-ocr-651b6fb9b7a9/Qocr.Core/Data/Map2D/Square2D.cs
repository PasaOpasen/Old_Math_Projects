using System;
using System.CodeDom;
using System.Linq;

using Qocr.Core.Interfaces;
using Qocr.Core.Utils;

namespace Qocr.Core.Data.Map2D
{
    /// <summary>
    /// Реализация <see cref="IEulerSquare"/>.
    /// </summary>
    internal class Square2D : IEulerSquare
    {
        private const string PropertyPrefix = "X";

        private const int SquareSideSize = 2;

        /// <summary>
        /// Создание экземпляра класса <see cref="Square2D"/>.
        /// </summary>
        public Square2D(string dots)
        {
            if (dots.Length != SquareSideSize * SquareSideSize)
            {
                throw new ArgumentException(nameof(dots));
            }

            SquareIdent = dots;
            ClassUtils.SetIndexValues(this, PropertyPrefix, dots.Select(chr => chr > '0').Cast<object>().ToArray());
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="Square2D"/>.
        /// </summary>
        public Square2D(params bool[] dots)
        {
            if (dots.Length != SquareSideSize * SquareSideSize)
            {
                throw new ArgumentException(nameof(dots));
            }

            SquareIdent = string.Concat(dots.Select(item => item ? 1 : 0));
            ClassUtils.SetIndexValues(this, PropertyPrefix, dots);
        }

        /// <summary>
        /// <para>[+] [ ]</para> 
        /// <para>[ ] [ ]</para> 
        /// </summary>
        public bool X0 { get; private set; }

        /// <summary>
        /// <para>[ ] [+]</para> 
        /// <para>[ ] [ ]</para> 
        /// </summary>
        public bool X1 { get; private set; }

        /// <summary>
        /// <para>[ ] [ ]</para> 
        /// <para>[ ] [+]</para> 
        /// </summary>
        public bool X2 { get; private set; }

        /// <summary>
        /// <para>[ ] [ ]</para> 
        /// <para>[+] [ ]</para> 
        /// </summary>
        public bool X3 { get; private set; }

        /// <inheritdoc/>
        public int SquareSize => SquareSideSize;

        /// <inheritdoc/>
        public bool IsSquareDetected(int topX, int topY, IMonomap monomap)
        {
            // [1] [2]
            // [3] [4]
            // Обход 1 -> 2 -> 3 -> 4
            return 
                monomap[topX    , topY    ] == X0 && 
                monomap[topX + 1, topY    ] == X1 &&
                monomap[topX,     topY + 1] == X2 &&
                monomap[topX + 1, topY + 1] == X3;
        }

        /// <inheritdoc/>
        public string SquareIdent { get; }
    }
}