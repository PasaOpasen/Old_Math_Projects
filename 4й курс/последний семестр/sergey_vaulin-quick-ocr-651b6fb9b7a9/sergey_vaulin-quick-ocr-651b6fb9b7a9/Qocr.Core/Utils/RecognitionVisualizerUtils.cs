using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Qocr.Core.Recognition.Data;

namespace Qocr.Core.Utils
{
    /// <summary>
    /// Визуализация отчёта после распознания.
    /// </summary>
    public static class RecognitionVisualizerUtils
    {
        private const float Thickness = 1;

        private static readonly Pen KnownPen = new Pen(Color.LimeGreen, Thickness);

        private static readonly Pen AssumptionPen = new Pen(Color.Orange, Thickness);

        private static readonly Pen UnknownPen = new Pen(Color.Red, Thickness);

        private static readonly IDictionary<QState, Pen> DefaultMapping = new Dictionary<QState, Pen>
        {
            { QState.Ok, KnownPen },
            { QState.Assumptions, AssumptionPen },
            { QState.Unknown, UnknownPen }
        };

        /// <summary>
        /// Визуализировать результат распознания на <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="bitmap">Ссылка на <see cref="Bitmap"/>.</param>
        /// <param name="report">Ссылка на результат распознавания.</param>
        public static void Visualize(Bitmap bitmap, QReport report)
        {
            var thickness = Thickness;
            foreach (var symbol in report.Symbols)
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawRectangle(DefaultMapping[symbol.State], symbol.StartPoint.X - thickness, symbol.StartPoint.Y - thickness, symbol.Width + thickness, symbol.Height + thickness);   
                }
            }
        }
    }
}
