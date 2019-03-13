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
using МатКлассы2019;
using  Point = МатКлассы2018.Point;
using static МатКлассы2019.Waves;

namespace Работа2019
{
    public partial class DC : Form
    {
        static DC()
        {

        }
        static readonly double corner=Math.PI/6;

        public DC()
        {
            InitializeComponent();
            button1_Click(new object(), new EventArgs());
        }

        public DC(Tuple<Point[],Point[]> tuple, DCircle circle, int randomcount = 40)
        {
            InitializeComponent();
            Draw(tuple,circle, randomcount);
        }
        int k = 0;
        private void Draw(Tuple<Point[], Point[]> tuple, DCircle circle, int randomcount=100)
        {
            k = 0;
            chart1.Series[0].Points.Clear();
            for (int i = 1; i < chart1.Series.Count; i++)
                chart1.Series.RemoveAt(i--);
            Point[] p = tuple.Item1;
            Point[] n = tuple.Item2;
            for (int i = 0; i < p.Length; i++)
            {
                chart1.Series[0].Points.AddXY(p[i].x, p[i].y);
                Str(p[i], n[i],Color.Red,circle);
            }

            Rand(circle, randomcount);
        }

        private void Str(Point beg, Point Normal,Color col,DCircle circle)
        {
            chart1.Series.Add(k++.ToString());
            chart1.Series.Last().IsVisibleInLegend = false;
            chart1.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series.Last().BorderWidth = 2;
            chart1.Series.Last().Color = col;

            double r = new Number.Complex(Normal.x,Normal.y).Abs,s=r/4;
            double cor = Math.Acos( (Normal.x)/r)*Math.Sign(Normal.y);
            Point p1 = new Point(s * Math.Cos(-Math.PI - corner), s * Math.Sin(-Math.PI - corner)).Turn(new Point(0), cor); //(cor).Show();
            Point p2 = new Point(s * Math.Cos(-Math.PI + corner) , s * Math.Sin(-Math.PI + corner) ).Turn(new Point(0), cor);

            chart1.Series.Last().Points.AddXY(beg.x, beg.y);
            chart1.Series.Last().Points.AddXY(beg.x+ Normal.x, beg.y+Normal.y);
            chart1.Series.Last().Points.AddXY(beg.x + Normal.x + p1.x, beg.y + Normal.y + p1.y);
            chart1.Series.Last().Points.AddXY(beg.x + Normal.x, beg.y + Normal.y);
            chart1.Series.Last().Points.AddXY(beg.x + Normal.x + p2.x, beg.y + Normal.y + p2.y);
        }

        private void Rand( DCircle circle,int count = 100)
        {
            chart1.Series.Add(k++.ToString());
            chart1.Series.Last().IsVisibleInLegend = false;
            chart1.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series.Last().MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star5;
            chart1.Series.Last().MarkerSize = 8;
            chart1.Series.Last().BorderWidth = 8;
            chart1.Series.Last().Color = Color.Green;
            int n = chart1.Series.Count - 1;
            double cof = textBox6.Text.ToDouble();

            int c = 0;
            Random r = new Random();
            double x, y, q=circle.Radius*cof;
            while (c < count)
            {
                x =-q+ r.NextDouble()*q*2+circle.Center.x;
                y = -q +r.NextDouble() * q*2+circle.Center.y;
                Point p = new Point(x, y);
                if (!circle.ContainPoint(p))
                {
                    chart1.Series[n].Points.AddXY(x, y);
                    var f = circle.GetNormal(p, 0.04*q);
                    Point beg = new Point(p.x - f.x, p.y - f.y);
                    Str(beg, f, Color.Blue,circle);
                    c++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Waves.DCircle c = new Waves.DCircle(new Point(textBox4.Text.ToDouble(), textBox5.Text.ToDouble()), textBox1.Text.ToDouble(), textBox2.Text.ToDouble(), textBox3.Text.ToDouble());
            Draw(c.DrawMasses(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value)), c, Convert.ToInt32(numericUpDown3.Value));
            //c.Center.Show();
        }
    }
}
