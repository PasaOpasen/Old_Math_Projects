using Qocr.Core.Data.Map2D;

namespace Qocr.Core.Interfaces
{
    /// <summary>
    /// Сравнение двух эйлеровых наборов.
    /// </summary>
    public interface IEulerComparer
    {
        /// <summary>
        /// Минимальный размер популяции символа.
        /// </summary>
        int MinPopularity { get; }

        /// <summary>
        /// Минимальное значение для диапазонных значений.
        /// </summary>
        int RoundingLimit { get; }
        
        void Compare(EulerMonomap2D euler1, EulerMonomap2D euler2, out int rounding, out int equals);
    }
}