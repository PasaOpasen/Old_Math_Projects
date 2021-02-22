namespace Qocr.Core.Recognition.Data
{
    /// <summary>
    /// Состояние распознания.
    /// </summary>
    public enum QState
    {
        /// <summary>
        /// Символ найден и полностью распознан.
        /// </summary>
        Ok,

        /// <summary>
        /// Символ в базе данных не найден, но есть предположение что он похож на один из предложенных.
        /// </summary>
        Assumptions,

        /// <summary>
        /// Символ не найден и не распознан.
        /// </summary>
        Unknown
    }
}