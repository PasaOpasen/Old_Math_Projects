//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU General Public License version 3 (GPLv3)
//
//  Email: pavel_torgashov@mail.ru.
//
//  Copyright (C) Pavel Torgashov, 2011. 

using System;
using System.Collections.Generic;
using System.Drawing;

namespace ContourAnalysisNS
{
    [Serializable]
    public class Template
    {
        public string name;
        public Contour contour;
        public Contour autoCorr;
        public Point startPoint;
        public bool preferredAngleNoMore90 = false;
        
        public int autoCorrDescriptor1;
        public int autoCorrDescriptor2;
        public int autoCorrDescriptor3;
        public int autoCorrDescriptor4;
        public double contourNorma;
        public double sourceArea;
        [NonSerialized]
        public object tag;

        public Template(Point[] points, double sourceArea, int templateSize)
        {
            this.sourceArea = sourceArea;
            startPoint = points[0];
            contour = new Contour(points);
            contour.Equalization(templateSize);
            contourNorma = contour.Norma;
            autoCorr = contour.AutoCorrelation(true);

            CalcAutoCorrDescriptions();
        }


        static int[] filter1 = { 1, 1, 1, 1 };
        static int[] filter2 = { -1, -1, 1, 1 };
        static int[] filter3 = { -1, 1, 1, -1 };
        static int[] filter4 = { -1, 1, -1, 1 };

        /// <summary>
        /// Calc wavelets convolution for ACF
        /// </summary>
        public void CalcAutoCorrDescriptions()
        {
            int count = autoCorr.Count;
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;
            double sum4 = 0;
            for (int i = 0; i < count; i++)
            {
                double v = autoCorr[i].Norma;
                int j = 4*i/count;

                sum1 += filter1[j] * v;
                sum2 += filter2[j] * v;
                sum3 += filter3[j] * v;
                sum4 += filter4[j] * v;
            }

            autoCorrDescriptor1 = (int)(100 * sum1 / count);
            autoCorrDescriptor2 = (int)(100 * sum2 / count);
            autoCorrDescriptor3 = (int)(100 * sum3 / count);
            autoCorrDescriptor4 = (int)(100 * sum4 / count);
        }

        public void Draw(Graphics gr, Rectangle rect)
        {
            gr.DrawRectangle(Pens.SteelBlue, rect);
            rect = new Rectangle(rect.Left, rect.Top, rect.Width - 24, rect.Height);

            var contour = this.contour.Clone();
            var autoCorr = this.autoCorr.Clone();
            //contour.Normalize();
            autoCorr.Normalize();

            //draw contour
            Rectangle r = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
            r.Inflate(-20, -20);
            var points = contour.GetPoints(startPoint);
            Rectangle boundRect = Rectangle.Round(contour.GetBoundsRect());

            double w = boundRect.Width;
            double h = boundRect.Height;
            float k = (float)Math.Min(r.Width/w, r.Height/h);
            int dx = startPoint.X - contour.SourceBoundingRect.Left;
            int dy = startPoint.Y - contour.SourceBoundingRect.Top;
            int ddx = -(int)(boundRect.Left * k);
            int ddy = (int)(boundRect.Bottom * k);
            for (int i = 0; i < points.Length; i++)
                points[i] = new Point(r.Left + ddx + (int)((points[i].X - contour.SourceBoundingRect.Left - dx) * k), r.Top + ddy + (int)((points[i].Y - contour.SourceBoundingRect.Top - dy) * k));
            //
            gr.DrawPolygon(Pens.Red, points);
            //draw ACF
            r = new Rectangle(rect.Width / 2 + rect.X, rect.Y, rect.Width / 2, rect.Height);
            r.Inflate(-20, -20);

            List<Point> angles = new List<Point>();
            for (int i = 0; i < autoCorr.Count; i++)
            {
                int x = r.X + 5 + i * 3;
                int v = (int)(autoCorr[i % autoCorr.Count].Norma * r.Height);
                gr.FillRectangle(Brushes.Blue, x, r.Bottom - v, 3, v);
                angles.Add(new Point(x, r.Bottom - (int)(r.Height * (0.5d + autoCorr[i%autoCorr.Count].Angle / 2 / Math.PI))));
            }

            try
            {
                gr.DrawLines(Pens.Red, angles.ToArray());
            }
            catch(OverflowException)
            { ;}

            Pen redPen = new Pen(Color.FromArgb(100, Color.Black));
            for (int i = 0; i <= 10; i++)
            {
                gr.DrawLine(redPen, r.X, r.Bottom - i * r.Height / 10, r.X + r.Width, r.Bottom - i * r.Height / 10);
            }

            //descriptors
            {
                int x = rect.Right;
                int y = r.Bottom - r.Height/2;
                gr.DrawLine(Pens.Gray, x, y, x+23, y);
                if (autoCorrDescriptor1 < int.MaxValue && autoCorrDescriptor1 > int.MinValue)
                {
                    gr.FillRectangle(Brushes.Red, x, y - (autoCorrDescriptor1 < 0 ? 0 : autoCorrDescriptor1 * r.Height / 100), 5, Math.Abs(autoCorrDescriptor1) * r.Height / 100);
                    gr.FillRectangle(Brushes.Red, x + 6, y - (autoCorrDescriptor2 < 0 ? 0 : autoCorrDescriptor2 * r.Height / 100), 5, Math.Abs(autoCorrDescriptor2) * r.Height / 100);
                    gr.FillRectangle(Brushes.Red, x + 12, y - (autoCorrDescriptor3 < 0 ? 0 : autoCorrDescriptor3 * r.Height / 100), 5, Math.Abs(autoCorrDescriptor3) * r.Height / 100);
                    gr.FillRectangle(Brushes.Red, x + 18, y - (autoCorrDescriptor4 < 0 ? 0 : autoCorrDescriptor4 * r.Height / 100), 5, Math.Abs(autoCorrDescriptor4) * r.Height / 100);
                }
            }
        }
    }

    /// <summary>
    /// List of templates
    /// </summary>
    [Serializable]
    public class Templates : List<Template>
    {
        public int templateSize = 30;
    }
}
