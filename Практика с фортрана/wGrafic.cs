using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы;
using static МатКлассы.Number;
using static МатКлассы.FuncMethods.Optimization;
using System.Media;
using Библиотека_графики;

namespace Практика_с_фортрана
{
    public partial class wGrafic : Form
    {
        public wGrafic()
        {
            InitializeComponent();
            this.chart1.Series[0].IsVisibleInLegend = false;
            this.chart1.Series[1].IsVisibleInLegend = false;
            this.chart1.Series[2].IsVisibleInLegend = false;
            for (int i = 0; i < 3; i++)
                chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";

        }

        double[] xval = null, uRval = null, uIval = null, uAbsval = null;

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            new ParametrsQu().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double min = Convert.ToDouble(textBox4.Text), max = Convert.ToDouble(textBox5.Text);
            double z = Convert.ToDouble(textBox6.Text), x = Convert.ToDouble(textBox7.Text);
            int n = Convert.ToInt32(numericUpDown1.Value);

            string name = $"с = {РабКонсоль.c} h = {РабКонсоль.h} a = {РабКонсоль.a} z={z} x={x} kcount={n}  ([{min},{max}])";

            Библиотека_графики.ForChart.SaveImageFromChart(chart1, name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new DINN5().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.chart1.Series[0].IsVisibleInLegend = false;
            this.chart1.Series[1].IsVisibleInLegend = false;
            this.chart1.Series[2].IsVisibleInLegend = false;
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();

            double min = Convert.ToDouble(textBox4.Text), max = Convert.ToDouble(textBox5.Text);
            double z = Convert.ToDouble(textBox6.Text), x = Convert.ToDouble(textBox7.Text);
            int n = Convert.ToInt32(numericUpDown1.Value);

            double h = (max - min) / (n - 1);
            xval = new double[n]; uIval = new double[n]; uRval = new double[n]; uAbsval = new double[n];
            this.chart1.Series[0].Name = $"Re u({x},{z})";
            this.chart1.Series[1].Name = $"Im u({x},{z})";
            this.chart1.Series[2].Name = $"|u({x},{z})|";

                for (int i = 0; i < n; i++)
                {
                    xval[i] = min + i * h;
                    РабКонсоль.w = xval[i];
                    Complex tmp = РабКонсоль.u(x, z); $"u({x} , {z}) = {tmp} | w = {РабКонсоль.w}".Show();
                    uRval[i] = tmp.Re;
                    uIval[i] = tmp.Im;
                    uAbsval[i] = tmp.Abs;
                }

            var list = new List<double>();
            if (checkBox1.Checked) { chart1.Series[0].Points.DataBindXY(xval, uRval); chart1.Series[0].IsVisibleInLegend = true; list.AddRange(uRval); }
            if (checkBox2.Checked) { chart1.Series[1].Points.DataBindXY(xval, uIval); chart1.Series[1].IsVisibleInLegend = true; list.AddRange(uIval); }
            if (checkBox3.Checked) { chart1.Series[2].Points.DataBindXY(xval, uAbsval); chart1.Series[2].IsVisibleInLegend = true; list.AddRange(uAbsval); }
            double maxx = list.Max(), minn = list.Min(), t = 0.05;
            chart1.ChartAreas[0].AxisY.Minimum = (minn > 0) ? minn * (1 - t) : minn * (1 + t);
            chart1.ChartAreas[0].AxisY.Maximum = (maxx > 0) ? maxx * (1 + t) : maxx * (1 - t);
        }
    }

}
