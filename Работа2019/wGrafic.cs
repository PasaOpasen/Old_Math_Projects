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
using static МатКлассы2018.Number;
using static МатКлассы2018.FuncMethods.Optimization;
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
            for(int i=0;i<3;i++)
            chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        double[] xval = null, uRval = null, uIval = null, uAbsval = null;

        private void button4_Click(object sender, EventArgs e)
        {
            double min = Convert.ToDouble(textBox4.Text), max = Convert.ToDouble(textBox5.Text);
            double z = Convert.ToDouble(textBox6.Text), x = Convert.ToDouble(textBox7.Text);
            int n = Convert.ToInt32(numericUpDown1.Value);

            string name = $"с = {РабКонсоль.c} h = {РабКонсоль.h} a = {РабКонсоль.a} z={z} x={x} kcount={n}  ([{min},{max}])";
            //SaveFileDialog savedialog = new SaveFileDialog();
            //savedialog.Title = "Сохранить рисунок как...";
            //savedialog.FileName = name;
            //savedialog.Filter = "Image files (*.bmp)|*.bmp|All files (*.*)|*.*";

            //savedialog.OverwritePrompt = true;
            //savedialog.CheckPathExists = true;
            //savedialog.ShowHelp = true;
            //if (savedialog.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {
            //        chart1.SaveImage(savedialog.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Bmp);
            //    }
            //    catch (Exception ee)
            //    {
            //        MessageBox.Show("Рисунок не сохранён", ee.Message,
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            Библиотека_графики.ForChart.SaveImageFromChart(chart1, name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new DINN5().Show();
        }

        double[] cc, hh, aa, xx, zz;
        private void InitializeMas()
        {
            string[] st = textBox1.Text.Split(' '); st = st.Where(n => n.Length > 0).ToArray();
            cc = new double[st.Length];
            for (int i = 0; i < st.Length; i++) cc[i] = Convert.ToDouble(st[i]);

            st = textBox2.Text.Split(' '); st = st.Where(n => n.Length > 0).ToArray();
            hh = new double[st.Length];
            for (int i = 0; i < st.Length; i++) hh[i] = Convert.ToDouble(st[i]);

            st = textBox3.Text.Split(' '); st = st.Where(n => n.Length > 0).ToArray();
            aa = new double[st.Length];
            for (int i = 0; i < st.Length; i++) aa[i] = Convert.ToDouble(st[i]);

            st = textBox7.Text.Split(' '); st = st.Where(n => n.Length > 0).ToArray();
            xx = new double[st.Length];
            for (int i = 0; i < st.Length; i++) xx[i] = Convert.ToDouble(st[i]);

            st = textBox6.Text.Split(' '); st = st.Where(n => n.Length > 0).ToArray();
            zz = new double[st.Length];
            for (int i = 0; i < st.Length; i++) zz[i] = Convert.ToDouble(st[i]);
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    InitializeMas();

        //    this.chart1.Series.Clear();

            //    double min = Convert.ToDouble(textBox4.Text), max = Convert.ToDouble(textBox5.Text);
            //    double z , x;
            //    int n = Convert.ToInt32(numericUpDown1.Value);

            //    double h = (max - min) / (n - 1);
            //    xval = new double[n]; uIval = new double[n]; uRval = new double[n]; uAbsval = new double[n];

            //    for (int q = 0; q < cc.Length; q++)
            //    {
            //        РабКонсоль.c = cc[q];
            //        for (int w = 0; w < hh.Length; w++)
            //        {
            //            РабКонсоль.h = hh[w];
            //            for (int ee = 0; ee < aa.Length; ee++)
            //            {
            //                РабКонсоль.a = aa[ee];
            //                for (int r = 0; r < xx.Length; r++)
            //                {
            //                    x = xx[r];
            //                    for (int t = 0; t < zz.Length; t++)
            //                    {
            //                        z = zz[t];

            //                            this.chart1.Series.Add($"Re u({x},{z}) (c={РабКонсоль.c} h={РабКонсоль.h} a={РабКонсоль.a})");
            //                            this.chart1.Series.Add($"Im u({x},{z}) (c={РабКонсоль.c} h={РабКонсоль.h} a={РабКонсоль.a})");
            //                            this.chart1.Series.Add($"|u({x},{z})| (c={РабКонсоль.c} h={РабКонсоль.h} a={РабКонсоль.a})");
            //                            int T = 210/(cc.Length+hh.Length+aa.Length+xx.Length+zz.Length)*(q+w+ee+r+t)+45;

            //                        for (int j = 0; j < 3; j++)
            //                        {
            //                            chart1.Series[chart1.Series.Count - 3 + j].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            //                            chart1.Series[chart1.Series.Count - 3 + j].IsVisibleInLegend = false;
            //                            chart1.Series[chart1.Series.Count - 3 + j].BorderWidth = 2;                          
            //                            switch(j)
            //                            {
            //                                case 0:
            //                                chart1.Series[chart1.Series.Count - 3 + j].Color = Color.FromArgb(T,0,0 );
            //                                    break;
            //                                case 1:
            //                                    chart1.Series[chart1.Series.Count - 3 + j].Color = Color.FromArgb(0,T , 0);
            //                                    break;
            //                                default:
            //                                    chart1.Series[chart1.Series.Count - 3 + j].Color = Color.FromArgb(0, 0,T );
            //                                    break;
            //                            }

            //                        }

            //                        for (int i = 0; i < n; i++)
            //                        {
            //                            xval[i] = min + i * h;
            //                            РабКонсоль.w = xval[i];
            //                            Complex tmp = РабКонсоль.u(x, z);
            //                            uRval[i] = tmp.Re;
            //                            uIval[i] = tmp.Im;
            //                            uAbsval[i] = tmp.Abs;
            //                        }

            //                        if (checkBox1.Checked) { chart1.Series[chart1.Series.Count - 3].Points.DataBindXY(xval, uRval); chart1.Series[chart1.Series.Count-3].IsVisibleInLegend = true; }
            //                        if (checkBox2.Checked) { chart1.Series[chart1.Series.Count - 2].Points.DataBindXY(xval, uIval); chart1.Series[chart1.Series.Count - 2].IsVisibleInLegend = true; }
            //                        if (checkBox3.Checked) { chart1.Series[chart1.Series.Count - 1].Points.DataBindXY(xval, uAbsval); chart1.Series[chart1.Series.Count - 1].IsVisibleInLegend = true; }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}

            private void button1_Click(object sender, EventArgs e)
            {
                this.chart1.Series[0].IsVisibleInLegend = false;
                this.chart1.Series[1].IsVisibleInLegend = false;
                this.chart1.Series[2].IsVisibleInLegend = false;
                this.chart1.Series[0].Points.Clear();
                this.chart1.Series[1].Points.Clear();
                this.chart1.Series[2].Points.Clear();

                РабКонсоль.c1 = Convert.ToDouble(textBox1.Text); РабКонсоль.h1 = Convert.ToDouble(textBox2.Text); РабКонсоль.m1 = Convert.ToDouble(textBox3.Text);
            РабКонсоль.c2 = Convert.ToDouble(textBox8.Text); РабКонсоль.h = РабКонсоль.h1+ Convert.ToDouble(textBox9.Text); РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);
            РабКонсоль.a = Convert.ToDouble(textBox11.Text);
            double min = Convert.ToDouble(textBox4.Text), max = Convert.ToDouble(textBox5.Text);
                double z = Convert.ToDouble(textBox6.Text), x = Convert.ToDouble(textBox7.Text);
                int n = Convert.ToInt32(numericUpDown1.Value);

                double h = (max - min) / (n - 1);
                xval = new double[n]; uIval = new double[n]; uRval = new double[n]; uAbsval = new double[n];
                this.chart1.Series[0].Name = $"Re u({x},{z})";
                this.chart1.Series[1].Name = $"Im u({x},{z})";
                this.chart1.Series[2].Name = $"|u({x},{z})|";

            bool par = false;

            if(!par)
            for (int i = 0; i < n; i++)
            {
                xval[i] = min + i * h;
                РабКонсоль.w = xval[i];
                Complex tmp = РабКонсоль.u(x, z); $"u({x} , {z}) = {tmp} | w = {РабКонсоль.w}".Show();
                    uRval[i] = tmp.Re;
                uIval[i] = tmp.Im;
                uAbsval[i] = tmp.Abs;
            }
            else
            Parallel.For(0, n, (int i) =>
              {
                  xval[i] = min + i * h;
                  РабКонсоль.w = xval[i];
                  Complex tmp = РабКонсоль.u(x, z); $"u({x} , {z}) = {tmp}".Show();
                  uRval[i] = tmp.Re;
                  uIval[i] = tmp.Im;
                  uAbsval[i] = tmp.Abs;
              });

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
