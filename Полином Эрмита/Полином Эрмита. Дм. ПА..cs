using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы2018;
using Point = МатКлассы2018.Point;

namespace Полином_Эрмита
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "Контейнер узлов:" + Environment.NewLine;
            textBox2.Text = "1";
            textBox3.Text = "2 3 4 5 0";
            textBox4.Text = "0";
            Lis = new List<MultipleKnot>();
        }
        public List<MultipleKnot> Lis;

        private void Draw_mas(Point[] r, Color color) //нарисовать массив точек
        {

            //SetColor(R, G, B); // задаем цвет линии
            //if(R==0) 
            //Program.FORM.chart1.Series[0].Color = Color.Green;
            FuncMethods.NetFunc u = new FuncMethods.NetFunc(r);

            int N = r.Length;
            double e = (u.MaxArg - u.MinArg) / 150;
            for (int i = 0; i < N; i++)
            {
                this.chart1.Series.Add("");
                int count = this.chart1.Series.Count - 1;
                this.chart1.Series[count].Color = color;

                this.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chart1.Series[count].IsVisibleInLegend = false;
                this.chart1.Series[count].BorderWidth = 3;
                this.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y - e); // устанавливаем курсор на точку
                this.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y + e); //рисуем line
                //-----------------------------------------------------------------
                this.chart1.Series.Add("");
                count = this.chart1.Series.Count - 1;
                this.chart1.Series[count].Color = color;

                this.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chart1.Series[count].IsVisibleInLegend = false;
                this.chart1.Series[count].BorderWidth = 3;
                this.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y + e);
                this.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y - e);
            }
            //Program.FORM.chart1.Series[0].Color = Color.Red;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.FORM.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(textBox2.Text);
            string s = textBox3.Text;
            string[] st = s.Split(' ');
            int n = st.Length;
            double[] vector = new double[n];
            for (int i = 0; i < n; i++) vector[i] = Convert.ToDouble(st[i]);

            Lis.Add(new MultipleKnot(x,vector));
            textBox1.Text += $"x = {x}    y = {s}"+ Environment.NewLine;
            textBox2.Text = textBox3.Text = /*textBox4.Text =*/ "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lis.Clear();
            textBox1.Text = "Контейнер узлов:" + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            for (int i = 1; i < this.chart1.Series.Count; i++) { this.chart1.Series.RemoveAt(i); i--; }

            MultipleKnot[] mas = new MultipleKnot[Lis.Count];
            for (int i = 0; i < mas.Length; i++) mas[i] = Lis[i];

            Polynom p = Polynom.Hermit(mas);
            Polynom q;
            int k = Convert.ToInt32(textBox4.Text);
            if (k == 0)
            {
                q = p;

                Point[] P = new Point[mas.Length];
                for (int i = 0; i < P.Length; i++) P[i] = new Point(mas[i].x, mas[i].y[0]);
                Draw_mas(P, Color.Black);
            }
            else
            {
                q = p | k;
                List<Point> P = new List<Point>();
                for (int i = 0; i < mas.Length; i++)
                    if (mas[i].y.Length >= k + 1) P.Add(new Point(mas[i].x, mas[i].y[k]));
                if(P.Count!=0) Draw_mas(Point.Points(P), Color.Black);
            }

            chart1.Series[0].Points.DataBindXY(МатКлассы2018.Point.PointsX(q.Value, 100, mas[0].x, mas[mas.Length - 1].x), МатКлассы2018.Point.PointsY(q.Value, 100, mas[0].x, mas[mas.Length - 1].x));
        }
    }
}
