using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Qocr.Core.Recognition.Data
{
    /// <summary>
    /// Результат распознавания.
    /// </summary>
    public class QReport
    {
        private const char Space = ' ';

        /// <summary>
        /// Создание экземпляра класса <see cref="QReport"/>.
        /// </summary>
        public QReport(IList<QAnalyzedSymbol> symbols)
        {
            Symbols = new ReadOnlyCollection<QAnalyzedSymbol>(symbols);
        }

        /// <summary>
        /// Ссылка на все найденные символы.
        /// </summary>
        public ReadOnlyCollection<QAnalyzedSymbol> Symbols { get; }

        /// <summary>
        /// Получить "сырой" тест 
        /// </summary>
        /// <returns></returns>
        public string RawText()
        {
            const int IgnoreSizeSquare = 3;

            var symbols = Symbols.Where(symbol => symbol.Height > IgnoreSizeSquare && symbol.Height > IgnoreSizeSquare) .ToList();
            StringBuilder result = new StringBuilder();

            do
            {
                var topmostSymbol = GetTopmostSymbol(symbols);
                var lineSymbols = GetLineSymbols(symbols, topmostSymbol);
                if (!lineSymbols.Any())
                {
                    // Значит даже сам символ не попал в диапазон неравенства.
                    symbols.Remove(topmostSymbol);
                    continue;
                }
                symbols.RemoveAll(lineSymbols.Contains);
                var lineString = GetLineText(lineSymbols);
                if (!string.IsNullOrEmpty(lineString))
                {
                    result.AppendLine(lineString);
                }
            } while (symbols.Any());

            return result.ToString();
        }

        private string GetLineText(ICollection<QAnalyzedSymbol> lineSymbols)
        {
            if (!lineSymbols.Any())
            {
                return string.Empty;
            }

            if (lineSymbols.Count == 1 && lineSymbols.ElementAt(0).State == QState.Assumptions)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            var orderedSymbols = lineSymbols.OrderBy(symbol => symbol.StartPoint.X).ToArray();
            int spaceSize = lineSymbols.Select(symbol => symbol.Width).Max();

            for (int i = 1; i < orderedSymbols.Length; i++)
            {
                var before = orderedSymbols[i - 1];
                var current = orderedSymbols[i];

                if (before.StartPoint == current.StartPoint)
                {
                    continue;
                }

                var beforeRight = before.StartPoint.X + before.Width;
                var currentLeft = current.StartPoint.X;
                var diff = currentLeft - beforeRight;
                if (diff > 0 && spaceSize > diff)
                {
                    spaceSize = diff;
                }
            }

            if (spaceSize < 3)
            {
                spaceSize = 3;
            }

            Append(result, orderedSymbols[0]);
            
            for (int i = 1; i < orderedSymbols.Length; i++)
            {
                var before = orderedSymbols[i - 1];
                var current = orderedSymbols[i];

                var beforeRight = before.StartPoint.X + before.Width;
                var currentLeft = current.StartPoint.X;
                if (currentLeft - beforeRight > spaceSize + 2)
                {
                    result.Append(Space);
                }

                Append(result, current);
            }

            return result.ToString();
        }

        private void Append(StringBuilder builder, QAnalyzedSymbol symbol)
        {
            builder.Append(symbol.Char);

            /*
            var chr = symbol.Char;
            if (symbol.State == QState.Ok)
            {
                builder.Append(chr);
            }
            else
            {
                builder.Append($"({chr})");
            }
            */
        }

        private static ICollection<QAnalyzedSymbol> GetLineSymbols(ICollection<QAnalyzedSymbol> symbols, QAnalyzedSymbol oneSymbol)
        {
            int lineVector = oneSymbol.StartPoint.Y + oneSymbol.Height / 2;
            List<QAnalyzedSymbol> result = new List<QAnalyzedSymbol>();

            // Ищем все символы на линии с oneSymbol
            foreach (var symbol in symbols)
            {
                var symbolY1 = symbol.StartPoint.Y;
                var symbolY2 = symbol.StartPoint.Y + symbol.Height;
                if (symbolY1 < lineVector && lineVector < symbolY2)
                {
                    result.Add(symbol);
                }
            }
            
            return result;
        }

        private static QAnalyzedSymbol GetTopmostSymbol(ICollection<QAnalyzedSymbol> symbols)
        {
            if (!symbols.Any())
            {
                return null;
            }

            // Ищем самый верхний символ (с наименьшим значением Y)
            QAnalyzedSymbol result = symbols.First();
            foreach (var symbol in symbols)
            {
                var minY = result.StartPoint.Y;
                var curY = symbol.StartPoint.Y;

                if (curY < minY)
                {
                    result = symbol;
                }
            }

            return result;
        }
    }
}