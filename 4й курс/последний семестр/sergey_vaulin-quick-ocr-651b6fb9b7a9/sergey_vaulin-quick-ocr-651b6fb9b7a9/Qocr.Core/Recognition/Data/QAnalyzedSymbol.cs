using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Qocr.Core.Recognition.Data
{
    /// <summary>
    /// Вся информация о символе на изображении.
    /// </summary>
    [DebuggerDisplay("{Char} - {StartPoint} - {State}")]
    public class QAnalyzedSymbol : QSymbol
    {
        /// <summary>
        /// Символ, который будет выведен по-умолчанию.
        /// </summary>
        public const char Default = '?';

        /// <summary>
        /// Создание экземпляра класса <see cref="QAnalyzedSymbol"/>.
        /// </summary>
        public QAnalyzedSymbol(QSymbol sourceSymbol, IEnumerable<QChar> recognitionChars)
            : base(sourceSymbol.Monomap, sourceSymbol.StartPoint, sourceSymbol.Euler)
        {
            recognitionChars = recognitionChars ?? Enumerable.Empty<QChar>();
            Chars = new ReadOnlyCollection<QChar>(recognitionChars.ToList());
        }

        /// <summary>
        /// Результат распознания символа.
        /// </summary>
        public ReadOnlyCollection<QChar> Chars { get; }

        /// <summary>
        /// Символ.
        /// </summary>
        public char Char
        {
            get
            {
                var firstCharData = Chars.FirstOrDefault(chr => chr.State == QState.Ok) ??
                                    Chars.Where(chr => chr.Popularity > 3)
                                        .OrderByDescending(chr => chr.EulerRoundings)
                                        .ThenByDescending(chr => chr.Popularity)
                                        .ThenByDescending(chr => chr.EulerEquals)
                                        .FirstOrDefault();

                return firstCharData?.Char ?? Default;
            }
        }
        
        /// <summary>
        /// Наилучший результат распознания.
        /// </summary>
        public QState State
        {
            get
            {
                return Chars.Any() ? Chars.Min(chr => chr.State) : QState.Unknown;
            }
        }
    }
}