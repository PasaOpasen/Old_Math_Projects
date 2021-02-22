using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

using Qocr.Core.Data.Map2D;
using Qocr.Core.Data.Serialization;
using Qocr.Core.Interfaces;
using Qocr.Core.Recognition.Data;

namespace Qocr.Core.Recognition.Logic
{
    /// <summary>
    /// Анализатор изображения на наличие в нём символа, и распознание его.
    /// </summary>
    public class DefaultFragmentAnalyzer : IFragmentAnalyzer
    {
        private readonly IEulerComparer _comparer;
        
        private readonly int _minimalHeight;

        private readonly ReadOnlyCollection<Symbol> _knownSymbols;

        /// <summary>
        /// Создание экземпляра класса <see cref="DefaultFragmentAnalyzer"/>.
        /// </summary>
        public DefaultFragmentAnalyzer(EulerContainer container) 
            : this(container, new DefaultEulerComparer())
        {
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="DefaultFragmentAnalyzer"/>.
        /// </summary>
        public DefaultFragmentAnalyzer(EulerContainer container, IEulerComparer comparer)
        {
            _comparer = comparer;
            _minimalHeight =
                container.Languages.SelectMany(
                    language => language.Chars.SelectMany(chr => chr.Codes.Select(code => code.Height))).Min();

            var allSymbols = container.Languages.SelectMany(language => language.Chars).ToList();
            _knownSymbols = new ReadOnlyCollection<Symbol>(allSymbols);
        }

        /// <inheritdoc/>
        public bool TryFindSymbol(QSymbol currentFragment, out QAnalyzedSymbol analyzedSymbol)
        {
            /*
             * Поиск полного соответствия фрагмента символу базы знаний.
             */
            foreach (var possibleSymbol in _knownSymbols)
            {
                // Если данный символ присутствует в базе знаний
                if (
                    possibleSymbol.Codes.Where(code => code.Height == currentFragment.Monomap.Height)
                        .Any(item => item.EulerCode.GetHashCode() == currentFragment.Euler.GetHashCode()))
                {
                    analyzedSymbol = new QAnalyzedSymbol(
                        currentFragment,
                        new[]
                        {
                            new QChar(possibleSymbol.Chr, QState.Ok)
                        });
                    return true;
                }
            }

            analyzedSymbol = null;
            return false;
        }

        /// <inheritdoc/>
        public QAnalyzedSymbol AnalyzeFragment(
            QSymbol currentFragment,
            IEnumerable<QSymbol> unknownFragments,
            IProducerConsumerCollection<QAnalyzedSymbol> recognizedSymbols)
        {
            /*
             * Алгоритм работы.
             * P.S. Мы работаем со 100% не известным символом, либо фрагментом символа.
             * 
             
            Input1: ip
            Input2: Red Alert  
                сначала будет R потом A затем l.
                e, d будут подмяты затем на e, r будет сплющивание вектора

            Input3: iiа  результат при анализе ii они сольются
             */

            // TODO Позже к высоте можно привязываться
            const int DefaultSplitStep = 3;
            var currentFragmentMinVector = currentFragment.StartPoint.Y + DefaultSplitStep;

            // Распознанные символы, лежащие в диапазоне поиска
            var currentLineRecognizedChars =
                recognizedSymbols.Where(
                    symbol =>
                        symbol.StartPoint.Y <= currentFragmentMinVector &&
                        currentFragmentMinVector <= symbol.StartPoint.Y + symbol.Height).ToArray();
            
            Point bottomRightPoint;
            if (currentLineRecognizedChars.Any())
            {
                var lineChars = currentLineRecognizedChars.ToDictionary(
                    symbol => symbol,
                    symbol => symbol.StartPoint.Y + symbol.Height);

                var lowestBound = lineChars.Values.Max();
                var lowestSymbol = lineChars.First(symbol => symbol.Value == lowestBound);

                // Минимальная нижняя точка для текущей линии для распознанных символов
                var minLineY = currentFragment.StartPoint.Y + lowestSymbol.Key.Height;

                bottomRightPoint = new Point(
                    currentFragment.StartPoint.X + currentFragment.Width,
                    minLineY);
            }
            else
            {
                // Нижняя правая точка будет ширина фрагмента.
                bottomRightPoint = new Point(
                    currentFragment.StartPoint.X + currentFragment.Width,
                    currentFragment.StartPoint.Y + currentFragment.Height + _minimalHeight);
            }

            // Мерджим фрагменты которые лежат в прямоугольнике fragment.StartPoint > ! > bottomRightPoint
            QSymbol[] mergedFragments = unknownFragments.Where(
                fragment => IsFragmentInsideSquare(fragment, currentFragment.StartPoint, bottomRightPoint))
                .ToArray();

            EulerMonomap2D currentEuler = EulerMonomap2D.Empty;
            currentEuler = mergedFragments.Select(fragment => fragment.Euler)
                .Aggregate(currentEuler, (current, mergedEuler) => current + mergedEuler);

            QAnalyzedSymbol analyzedSymbol;
            if (TryFindSymbol(currentFragment, out analyzedSymbol))
            {
                recognizedSymbols.TryAdd(analyzedSymbol);

                // TODO Symbol стоит смержить, но после всех манипуляций!! Саму картинку
                currentFragment = currentFragment;

                return analyzedSymbol;
            }

            // Результат анализа всех букв !!! 
            var resultData = new List<QChar>();
            
            // Идём по всем буквам языка
            foreach (var possibleSymbol in _knownSymbols)
            {
                QChar @char;
                if (TryIntelliRecognition(
                    possibleSymbol,
                    currentEuler,
                    bottomRightPoint.Y - currentFragment.StartPoint.Y,
                    out @char) && @char.Popularity > _comparer.MinPopularity)
                {
                    // Не стоит пропускать проверку всех символов, так как стоит найти 3 и з цифро-буквы, либо аналоги А ру. и а англ.
                    resultData.Add(@char);
                }
            }
            
            return new QAnalyzedSymbol(currentFragment, resultData);
        }

        private static bool IsFragmentInsideSquare(QSymbol fragment, Point leftTop, Point rightBottom)
        {
            // Признак того, что фрагмент лежит в прямоугольнике.
            var rectangle1 = new Rectangle(
                fragment.StartPoint.X,
                fragment.StartPoint.Y,
                fragment.Width,
                fragment.Height);
            var rectangle2 = new Rectangle(leftTop, new Size(rightBottom.X - leftTop.X, rightBottom.Y - leftTop.Y));
            if (Rectangle.Intersect(rectangle1, rectangle2) == Rectangle.Empty)
            {
                return false;
            }

            return true;
        }

        private bool TryIntelliRecognition(
            Symbol possibleSymbol,
            EulerMonomap2D currentCharEuler,
            int height,
            out QChar @char)
        {
            @char = null;

            // TODO дорогое вычисление, пока тестовый вариант
            /*
             * Анализировать:
             * 1. Equals
             * 2. -1 < X < 1
             * 3. Количество совпадений в базе знаний что бы не опираться на косячный шрифт.
             */
            var possibleSybols = possibleSymbol.Codes.Select(
                code =>
                {
                    int rounding, equals;
                    _comparer.Compare(code.EulerCode, currentCharEuler, out rounding, out equals);
                    return new { EulerEquals = equals, EulerRounding = rounding };
                })

                // TODO Как минимум половина набора совпадает (Стоит играться)
                .Where(result => result.EulerEquals >= _comparer.RoundingLimit).ToArray();

            if (!possibleSybols.Any())
            {
                return false;
            }

            var bestValue = possibleSybols[0];
            foreach (var possibleSybol in possibleSybols)
            {
                if (possibleSybol.EulerRounding > bestValue.EulerRounding)
                {
                    bestValue = possibleSybol;
                }
            }

            @char = new QChar(possibleSymbol.Chr, QState.Assumptions, bestValue.EulerEquals, bestValue.EulerRounding, possibleSybols.Length);
            return true;
        }
    }
}