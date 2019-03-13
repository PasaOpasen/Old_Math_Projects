using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Библиотека_графики
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public static class ForChart
    {
        static double t = 0.05;

        /// <summary>
        /// Добавить ко всем графикам tooltips, сообщающие координаты точек
        /// </summary>
        /// <param name="chart"></param>
        public static void SetToolTips(ref Chart chart)
        {
            for (int i = 0; i < chart.Series.Count; i++)
                chart.Series[i].ToolTip = "X = #VALX, Y = #VALY";
        }
        /// <summary>
        /// Задать границы по оси Y так, чтобы осталось немного лишнего, но равномерно
        /// </summary>
        /// <param name="chart"></param>
        public static void SetAxisesY(ref Chart chart, double coef = 0.05)
        {
            t = coef;
            List<double> list = new List<double>();
            for (int i = 0; i < chart.Series.Count; i++)
                for (int j = 0; j < chart.Series[i].Points.Count; j++)
                    list.AddRange(chart.Series[i].Points[j].YValues);

            double min = list.Min(), max = list.Max();

            if (min == max)
            {
                min -= t;
                max += t;
            }

            chart.ChartAreas[0].AxisY.Minimum = (min > 0) ? min * (1 - t) : min * (1 + t);
            chart.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + t) : max * (1 - t);
        }
        /// <summary>
        /// Очистить массивы точек и скрыть легенды
        /// </summary>
        /// <param name="chart"></param>
        public static void ClearPointsAndHideLegends(ref Chart chart)
        {
            for (int i = 0; i < chart.Series.Count; i++)
            {
                chart.Series[i].Points.Clear();
                chart.Series[i].IsVisibleInLegend = false;
            }
        }
        /// <summary>
        /// Сохранить рисунок с чарта
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="name">Имя файла</param>
        /// <param name="format">Формат изображения</param>
        public static void SaveImageFromChart(Chart chart, string name = "Без имени", System.Windows.Forms.DataVisualization.Charting.ChartImageFormat format = System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить рисунок как...";
            savedialog.FileName = name;
            savedialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";

            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chart.SaveImage(savedialog.FileName, format);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Рисунок не сохранён", ee.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static Color[] DefaltColors
        {
            get
            {
                Color[] mas = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Black, Color.Bisque, Color.Brown, Color.DarkBlue, Color.Gold, Color.HotPink };
                return mas;
            }
        }
        /// <summary>
        /// Создание нескольких Series, подходящих под условия
        /// </summary>
        /// <param name="chart">Чарт, в котором ведётся создание</param>
        /// <param name="names">Массив имён</param>
        /// <param name="width">Ширина кривых</param>
        /// <param name="type">Тип графиков</param>
        /// <param name="style">Стиль рисования</param>
        /// <param name="mas">Массив цветов</param>
        public static void CreatSeries(ref Chart chart, string[] names, int width=3,SeriesChartType type=SeriesChartType.Line, MarkerStyle style= MarkerStyle.Circle,Color[] mas=null)
        {
            if (mas == null)
                mas = DefaltColors;

            for(int i=0;i<names.Length;i++)
            {
                chart.Series.Add(names[i]);
                chart.Series.Last().BorderWidth = width;
                chart.Series.Last().ChartType = type;
                chart.Series.Last().MarkerStyle = style;
                chart.Series.Last().BorderColor = mas[i];
            }
        }
        /// <summary>
        /// Рисование простого графика по массиву аргументов и массиву массивов значений на этих аргументах
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="args"></param>
        /// <param name="values"></param>
        public static void AddMasOfPoints(ref Chart chart, double[] args,params double[][] values)
        {
            for(int i=0;i<values.Length;i++)
            {
                chart.Series[i].Points.Clear();
                chart.Series[i].Points.DataBindXY(args, values[i]);
            }
        }
    }

    public static class ImageActions
    {
        /// <summary>
        /// Слияние двух изображений в одно (рисует одно над другим снизу вверх)
        /// </summary>
        /// <param name="img1"></param>
        /// <param name="img2"></param>
        /// <returns></returns>
        public static Image MergerOfImages(Image img1,Image img2)
        {
            Bitmap res = new Bitmap(Math.Max(img1.Width, img2.Width),img1.Height+img2.Height);

            Graphics g = Graphics.FromImage(res);
            g.DrawImage(img1, 0, 0);
            g.DrawImage(img2, 0, img1.Height);

            return res;
        }
    }
}
