using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Qocr.Core.Recognition.Data
{
    /// <summary>
    /// Информация о распознаваемом символе.
    /// </summary>
    [DebuggerDisplay("{Char} @EulerEquals {EulerEquals} @Popularity {Popularity} @EulerRoundings {EulerRoundings}")]
    public class QChar
    {
        /// <summary>
        /// Неизвестный символ.
        /// </summary>
        public static QChar Unknown = new QChar('?', QState.Unknown);

        /// <summary>
        /// Создание экземпляра класса <see cref="QChar"/>.
        /// </summary>
        public QChar(char chr, QState state)
            : this(chr, state, 16, 16, int.MaxValue)
        {
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="QChar"/>.
        /// </summary>
        public QChar(char chr, QState state, int eulerEquals, int eulerRoundings, int popularity)
        {
            State = state;
            EulerEquals = eulerEquals;
            EulerRoundings = eulerRoundings;
            Popularity = popularity;
            Char = chr;
        }
        
        /// <summary>
        /// Количество значений эйлеровых периодов, которые крайне близко лежат к образцу базы знаний.
        /// </summary>
        public int EulerRoundings { get; private set; }

        /// <summary>
        /// Количество периодов, полностью совпадающих.
        /// </summary>
        public int EulerEquals { get; private set; }

        /// <summary>
        /// Насколько часто в базе знаний встречаются для данного символа удовлетворяющих эвристики результатов
        /// </summary>
        public int Popularity { get; private set; }

        /// <summary>
        /// Статус распознания.
        /// </summary>
        public QState State { get; private set; }

        /// <summary>
        /// Символ.
        /// </summary>
        public char Char { get; private set; }
    }
}