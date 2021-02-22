using System.Collections.Concurrent;
using System.Collections.Generic;

using Qocr.Core.Recognition.Data;

namespace Qocr.Core.Interfaces
{
    /// <summary>
    /// Анализ <see cref="IMonomap"/> фрагмента на присутствие на изображении символа.
    /// </summary>
    public interface IFragmentAnalyzer
    {
        /// <summary>
        /// Проанализировать изображение.
        /// </summary>
        /// <param name="fragment">Фрагмент изображение.</param>
        /// <param name="unknownFragments">Ссылка на все не распознанные фрагменты.</param>
        /// <param name="recognizedSymbols">Список всех распознанных символов.</param>
        /// <returns>Распознанный символ.</returns>
        QAnalyzedSymbol AnalyzeFragment(
            QSymbol fragment,
            IEnumerable<QSymbol> unknownFragments,
            IProducerConsumerCollection<QAnalyzedSymbol> recognizedSymbols);

        /// <summary>
        /// Попробовать найти символ из базы знаний.
        /// </summary>
        /// <param name="currentFragment">Текущий фрагмент.</param>
        /// <param name="analyzedSymbol">Найденный символ в базе знаний.</param>
        /// <returns>Успех поиска.</returns>
        bool TryFindSymbol(QSymbol currentFragment, out QAnalyzedSymbol analyzedSymbol);
    }
}