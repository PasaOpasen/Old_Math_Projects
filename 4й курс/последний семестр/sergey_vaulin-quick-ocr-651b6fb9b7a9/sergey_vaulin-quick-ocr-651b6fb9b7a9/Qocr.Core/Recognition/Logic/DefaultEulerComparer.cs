using System;
using System.Collections.Generic;

using Qocr.Core.Data.Map2D;
using Qocr.Core.Interfaces;

namespace Qocr.Core.Recognition.Logic
{
    /// <summary>
    /// Default equality comparer для <see cref="EulerMonomap2D"/>.
    /// </summary>
    public class DefaultEulerComparer : IEulerComparer
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="DefaultEulerComparer"/>.
        /// </summary>
        public DefaultEulerComparer()
        {
        }

        /// <inheritdoc/>
        public int MinPopularity => 3;

        /// <inheritdoc/>
        public int RoundingLimit => 3;

        private void ComputeDiff(int source, int comp, ref int rounding, ref int equals)
        {
            var diff = source - comp;
            if (diff == 0)
            {
                equals++;
                rounding++;
            }
            else if (-1 <= diff && diff <= 1)
            {
                rounding++;
            }
        }

        /// <inheritdoc/>
        void IEulerComparer.Compare(EulerMonomap2D euler1, EulerMonomap2D euler2, out int rounding, out int equals)
        {
            // Количество очень близких значений в интервале
            rounding = 0;

            // Количество полных совпадений
            equals = 0;

            ComputeDiff(euler1.S0, euler2.S0, ref rounding, ref equals);
            ComputeDiff(euler1.S1, euler2.S1, ref rounding, ref equals);
            ComputeDiff(euler1.S2, euler2.S2, ref rounding, ref equals);
            ComputeDiff(euler1.S3, euler2.S3, ref rounding, ref equals);
            ComputeDiff(euler1.S4, euler2.S4, ref rounding, ref equals);
            ComputeDiff(euler1.S5, euler2.S5, ref rounding, ref equals);
            ComputeDiff(euler1.S6, euler2.S6, ref rounding, ref equals);
            ComputeDiff(euler1.S7, euler2.S7, ref rounding, ref equals);
            ComputeDiff(euler1.S8, euler2.S8, ref rounding, ref equals);
            ComputeDiff(euler1.S9, euler2.S9, ref rounding, ref equals);
            ComputeDiff(euler1.S10, euler2.S10, ref rounding, ref equals);
            ComputeDiff(euler1.S11, euler2.S11, ref rounding, ref equals);
            ComputeDiff(euler1.S12, euler2.S12, ref rounding, ref equals); 
            ComputeDiff(euler1.S13, euler2.S13, ref rounding, ref equals);
            ComputeDiff(euler1.S14, euler2.S14, ref rounding, ref equals);
        }
    }
}