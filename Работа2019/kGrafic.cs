using System;
//using System.Windows.Media;
using System.Diagnostics;
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
using static МатКлассы2018.Number;
using static Functions;
using MP = System.Tuple<double, double[]>;

namespace Работа2019
{
    public partial class kGrafic : Form
    {
        public kGrafic()
        {
            InitializeComponent();
            chart1.Series[0].IsVisibleInLegend = false;
            button4.Hide();
            radioButton1_CheckedChanged(new object(), new EventArgs());
            this.chart1.MouseWheel += new MouseEventHandler(cMouseWheel);
            this.chart1.MouseClick += new MouseEventHandler(chart1_MouseClick);
            toolTip1.AutoPopDelay = 4700;

            timer1.Interval = 500;
            timer1.Tick += new EventHandler(Timer1_Tick);
            // Enable timer.  
            //timer1.Enabled = true;
            //Process.Start("работа2019.r");
            //new DINN5().Show();
            listBox1.SelectedIndex = 1;
            listBox2.SelectedIndex = 0;
        }
        int[] prbar;
        private void Timer1_Tick(object Sender, EventArgs e)
        {
            progressBar1.Value = (prbar.Sum().ToDouble() / prbar.Length * progressBar1.Maximum).ToInt();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        double[] args;
        Roots.MethodRoot method;
        bool half = false;
        private void button1_Click(object sender, EventArgs e)
        {
            string met = (string)listBox1.SelectedItem;//met.Show();
            half = false;
            switch (met)
            {
                case "Bisec":
                    method = Roots.MethodRoot.Bisec;
                    break;
                case "Brent":
                    method = Roots.MethodRoot.Brent;
                    break;
                case "Broyden":
                    method = Roots.MethodRoot.Broyden;
                    break;
                case "Secant":
                    method = Roots.MethodRoot.Secant;
                    break;
                case "NewtonRaphson":
                    method = Roots.MethodRoot.NewtonRaphson;
                    break;
                case "Combine":
                    method = Roots.MethodRoot.Combine;
                    break;
                case "Halfc (not supplemented)":
                    half = true;
                    break;
                default:
                    method = Roots.MethodRoot.Brent;
                    break;
            }

            if (radioButton1.Checked) { rad1(); may = true; }
            else
            {
                chart1.Series.Clear();
                РабКонсоль.w = Convert.ToDouble(textBox13.Text);
                double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text); int itcount = РабКонсоль.countroot, k = Convert.ToInt32(numericUpDown1.Value);
                double h = (end - beg) / (k - 1);
                double kt1 = k1(РабКонсоль.w), kt2 = k2(РабКонсоль.w);

                ComplexFunc s1 = (Complex alp) => sigma(alp * alp, kt1);
                ComplexFunc s2 = (Complex alp) => sigma(alp * alp, kt2);

                if (radioButton5.Checked)
                {
                    othergraph("Δ", Deltas, beg, k, h);
                }
                if (radioButton6.Checked) { othergraph("σ1", s1, beg, k, h); }
                if (radioButton7.Checked) { othergraph("σ2", s2, beg, k, h); }
                may = false;
            }

            //CopyPoint();
        }

        Vectors[] mas, masN;

        private void SimpleBeginAboutChart()
        {
            int ind = listBox2.SelectedIndex;
            chart1.Series.Clear();
            //string name1;
            switch (ind)
            {
                case 0:
                    chart1.Series.Add("α: Δ(α,ω)=ΔPRMS(α)=0");
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart1.Series[0].Color = Color.Blue;
                    chart1.Series.Add("α: Δ(α,ω)=ΔN(α)=0");
                    chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[1].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart1.Series[1].Color = Color.Red;
                    break;
                case 1:
                    chart1.Series.Add("α: 1/K33 = 0");
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart1.Series[0].Color = Color.Blue;
                    chart1.Series.Add("α: Δ(α,ω)=ΔN(α)=0");
                    chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[1].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart1.Series[1].Color = Color.Red;
                    break;
                case 2:
                    chart1.Series.Add("α: 1/tr(K) = 0");
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart1.Series[0].Color = Color.Blue;
                    break;
                default:
                    chart1.Series.Add("α: 1/det(K) = 0");
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart1.Series[0].Color = Color.Blue;
                    break;
            }

        }

        private async void rad1()
        {
            SimpleBeginAboutChart();
            progressBar1.Value = 0;

            double tmin = РабКонсоль.polesBeg, tmax = РабКонсоль.polesEnd, eps = РабКонсоль.epsroot, step = РабКонсоль.steproot;
            double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text);
            FuncMethods.Optimization.EPS = eps;
            FuncMethods.Optimization.STEP = step;
            int itcount = РабКонсоль.countroot, k = Convert.ToInt32(numericUpDown1.Value);
            double h = (end - beg) / (k - 1);
            mas = new Vectors[k];
            masN = new Vectors[k];
            args = new double[k];
            Vectors[] mass = new Vectors[k];
            prbar = new int[k]; timer1.Enabled = true;

            int ind = listBox2.SelectedIndex;

            await Task.Run(() =>
            {
                Parallel.For(0, k, (int i) =>
                {
                    //for (int i = 0; i < k; i++)
                    //{
                        args[i] = beg + i * h;

                    //double coef = Expendator2.MaxApproxAbs((Complex a) => Deltass(a, args[i]),tmin,tmax,step);// *Math.Exp(-args[i].Sqr()*h);
                    ComplexFunc del;

                    switch (ind)
                    {
                        case 0:
                            del = (Complex a) => Deltass(a, args[i]);
                            break;
                        case 1:
                            del = (Complex a) => K(a, 1, 0, 0, args[i])[2, 2].Reverse();
                            break;
                        case 2:
                            del = (Complex a) => K(a, 1, 0, 0, args[i]).Track.Reverse();
                            break;
                        default:
                            del = (Complex a) => K(a, 1, 0, 0, args[i]).Det.Reverse();
                            break;
                    }

                    if (!half) mas[i] = Roots.OtherMethod(del, tmin, tmax, step, eps, method, checkBox4.Checked);//FuncMethods.Optimization.Halfc(del, tmin, tmax, step, eps, itcount);
                    else mas[i] = FuncMethods.Optimization.Halfc(/*(Complex c)=>del(c).ReIm*/del, tmin, tmax, step, eps, itcount).DoubleMas.Where(n => del(n).Abs < eps).Distinct().ToArray();
                    //mas[i] = new Vectors(0);
                    //использовать ли корни N

                    masN[i] = DeltassNPosRoots(args[i], tmin, tmax);
                    //ComplexFunc delN = (Complex a) => DeltassN(a, args[i]);
                    //mas[i].UnionWith(masN[i]/*FuncMethods.Optimization.Halfc(delN, tmin, tmax, step, eps, itcount)*/);
                    //mas[i].Show(); 

                    bool ch2 = checkBox2.Checked, ch3 = checkBox3.Checked;
                    if (ind <= 1)
                    {
                        if (ch2 && ch3)
                            mass[i] = new Vectors(Vectors.Union(mas[i], masN[i]));
                        else if (ch2)
                            mass[i] = new Vectors(mas[i]);
                        else
                            mass[i] = new Vectors(masN[i]);
                    }
                    else mass = mas;


                    //вывод корней
                    if (checkBox1.Checked)
                    {
                        List<double> value = new List<double>();
                        for (int j = 0; j < mass[i].n; j++)
                            value.Add((Deltass(mass[i][j], args[i]) * DeltassN(mass[i][j], args[i])).Abs);
                        Console.WriteLine($"ω ={args[i]}; {mass[i].ToString()} \t--> {(new Vectors(value.ToArray())).ToString()}"); "".Show();
                    }

                    prbar[i] = 1;
                     //}
               });
            });

            if (checkBox2.Checked)
            {
                for (int i = 0; i < k; i++)
                {
                    if (radioButton3.Checked) for (int j = 0; j < mas[i].n; j++) chart1.Series[0].Points.AddXY(args[i], mas[i][j]);
                    if (radioButton4.Checked) for (int j = 0; j < mas[i].n; j++) chart1.Series[0].Points.AddXY(args[i], mas[i][j] / args[i]);
                }
            }
            if (checkBox3.Checked&& ind <= 1)
            {
                for (int i = 0; i < k; i++)
                {
                    if (radioButton3.Checked) for (int j = 0; j < masN[i].n; j++) chart1.Series[1].Points.AddXY(args[i], masN[i][j]);
                    if (radioButton4.Checked) for (int j = 0; j < masN[i].n; j++) chart1.Series[1].Points.AddXY(args[i], masN[i][j] / args[i]);
                }
            }


            if (radioButton3.Checked) chart1.Titles[0].Text = "График ζn(ω)";
            if (radioButton4.Checked) chart1.Titles[0].Text = "График ζn(ω)/ω";
            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].ToolTip = $"X = #VALX, Y = #VALY";
            //textBox13.Text = РабКонсоль.w.ToString();

            //           if(radioButton2.Checked) CurvesShow(mas,args);
        }
        private void othergraph(string func, ComplexFunc f, double beg, int k, double hh)
        {
            chart1.Series.Add($"Re {func}"); chart1.Series[0].Color = Color.Red;
            chart1.Series.Add($"Im {func}"); chart1.Series[1].Color = Color.Green;
            chart1.Series.Add($"Abs {func}"); chart1.Series[2].Color = Color.Blue;
            for (int i = 0; i < 3; i++)
            {
                chart1.Series[i].BorderWidth = 3;
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";
            }
            for (int i = 0; i < k; i++)
            {
                double arg = beg + i * hh;
                Number.Complex val = f(arg); val.Show();
                chart1.Series[0].Points.AddXY(arg, val.Re);
                chart1.Series[1].Points.AddXY(arg, val.Im);
                chart1.Series[2].Points.AddXY(arg, val.Abs);
                prbar[i] = 1;
            }

        }


        private void CurvesShow(Vectors[] m, double[] ar)
        {
            var list = new List<Vectors>(m);
            var arg = new List<double>(ar);
            int k = 0;
            while (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].n < 1)
                    {
                        list.RemoveAt(i);
                        arg.RemoveAt(i);
                        i--;
                    }

                for (int i = 1; i < list.Count; i++)
                    if (list[i].n == 1 && list[i - 1].n == 1)
                    {
                        list.RemoveAt(i - 1);
                        arg.RemoveAt(i - 1);
                        i--;
                    }
                    else if (list[i].n < list[i - 1].n)
                    {
                        list.RemoveAt(i);
                        arg.RemoveAt(i);
                        i--;
                    }
                //for (int i = 1; i < list.Count; i++) list[i].Show();

                chart1.Series.Add($"Дисперсионка {k + 1}");
                chart1.Series[k].BorderWidth = 2;
                chart1.Series[k].Color = Color.Blue;
                chart1.Series[k].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                for (int i = 0; i < list.Count; i++)
                {
                    //list[i].Show();list[i].n.Show();
                    chart1.Series[k].Points.Add(arg[i], list[i][list[i].n - 1]);
                    list[i] = new Vectors(list[i], 0, list[i].n - 2);
                }
                k++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //РабКонсоль.c = Convert.ToDouble(textBox1.Text);
            //РабКонсоль.h = Convert.ToDouble(textBox2.Text);
            //РабКонсоль.a = Convert.ToDouble(textBox3.Text);

            double tmin = РабКонсоль.polesBeg, tmax = РабКонсоль.polesEnd, eps = РабКонсоль.epsroot, step = РабКонсоль.steproot;
            double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text);

            int itcount = РабКонсоль.countroot, k = Convert.ToInt32(numericUpDown1.Value);

            string name = $"tmin={tmin} tmax={tmax} eps={eps} step={step} kcount={k}  ([{beg},{end}])";
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
                    chart1.SaveImage(savedialog.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Рисунок не сохранён", ee.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            РабКонсоль.w = Convert.ToDouble(textBox13.Text);
            double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text); int itcount = РабКонсоль.countroot, k = Convert.ToInt32(numericUpDown1.Value);
            double h = (end - beg) / (k - 1);
            chart1.Series.Add("Re Δ"); chart1.Series[0].Color = Color.Red;
            chart1.Series.Add("Im Δ"); chart1.Series[1].Color = Color.Green;
            chart1.Series.Add("Abs Δ"); chart1.Series[2].Color = Color.Blue;
            for (int i = 0; i < 3; i++)
            {
                chart1.Series[i].BorderWidth = 3;
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            }
            for (int i = 0; i < k; i++)
            {
                double arg = beg + i * h;
                Number.Complex val = Deltas(arg); val.Show();
                chart1.Series[0].Points.AddXY(arg, val.Re);
                chart1.Series[1].Points.AddXY(arg, val.Im);
                chart1.Series[2].Points.AddXY(arg, val.Abs);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Show();
            groupBox3.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Hide();
            groupBox3.Show();
        }

        private MP[][] points;
        private double XLen, YLen, x0, X, y0, Y;

        bool may = false;
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (!(checkBox2.Checked || checkBox3.Checked)) checkBox3.Checked = true;
            if (may) ReDraw();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!(checkBox2.Checked || checkBox3.Checked)) checkBox2.Checked = true;
            if (may) ReDraw();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int v = listBox2.SelectedIndex;
            if (v <= 1)
            {
                checkBox2.Show();
                checkBox3.Show();
                checkBox2.Checked = true;
                checkBox3.Checked = true;
            }
            else
            {
                checkBox2.Hide();
                checkBox3.Hide();
            }
        }

        private void CopyPoint()
        {
            points = new MP[chart1.Series.Count][];
            List<double> x = new List<double>(), y = new List<double>();
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                points[i] = new MP[chart1.Series[i].Points.Count];
                for (int j = 0; j < points[i].Length; j++)
                {
                    points[i][j] = new Tuple<double, double[]>(chart1.Series[i].Points[j].XValue, chart1.Series[i].Points[j].YValues);
                    x.Add(chart1.Series[i].Points[j].XValue);
                    y.AddRange(chart1.Series[i].Points[j].YValues);
                }
            }
            x0 = x.Min();
            X = x.Max();
            XLen = X - x0;
            y0 = y.Min();
            Y = y.Max();
            YLen = Y - y0;
        }

        private void cMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            e.Delta.Show();
            double coef = 1 - e.Delta * 0.01;
            double tx = X * coef, ty = Y * coef;
            МатКлассы2018.Point loc;

            //if (e.Delta > 0)
            //{
            loc = new МатКлассы2018.Point(e.X, e.Y);
            //}
            //else
            //{

            //}
            double tx0 = loc.x - tx / 2, txX = loc.x + tx / 2;
            double ty0 = loc.y - ty / 2, tyY = loc.y + ty / 2;

            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].Points.Clear();
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                for (int j = 0; j < points.GetLength(2); j++)
                    if (points[i][j].Item1 >= tx0 && points[i][j].Item1 <= txX)
                    {
                        List<double> list = new List<double>();
                        for (int k = 0; k < points[i][j].Item2.Length; k++)
                            if (points[i][j].Item2[k] >= ty0 && points[i][j].Item2[k] <= tyY)
                                list.Add(points[i][j].Item2[k]);
                        if (list.Count > 0) chart1.Series[i].Points.AddXY(points[i][j].Item1, list);
                    }

            }

            chart1.Invalidate();
        }

        double xOne = 0;
        double xTwo = 0;
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                var result = chart1.HitTest(e.X, e.Y);
                if (result.PointIndex >= 0)
                {
                    xOne = result.Series.Points[result.PointIndex].XValue;
                    xTwo = result.Series.Points[result.PointIndex].YValues[0];
                    Complex PRMS = Deltass(xTwo, xOne), N = DeltassN(xTwo, xOne);
                    toolTip1.Show($"ω = {xOne} α = {xTwo} deltaPRMS = {PRMS} deltaN = {N} |delta| = {(N * PRMS).Abs}", chart1);
                }
            }
            finally { }
        }


        private void ReDraw()
        {
            SimpleBeginAboutChart();

            int k = args.Length;
            Vectors[] mass = new Vectors[k];

            for (int i = 0; i < k; i++)
            {
                bool ch2 = checkBox2.Checked, ch3 = checkBox3.Checked;
                if (ch2 && ch3)
                    mass[i] = new Vectors(Vectors.Union(mas[i], masN[i]));
                else if (ch2)
                    mass[i] = new Vectors(mas[i]);
                else
                    mass[i] = new Vectors(masN[i]);
            }

            //for (int i = 0; i < k; i++)
            //{
            //    if (radioButton3.Checked) for (int j = 0; j < mass[i].n; j++) chart1.Series[0].Points.AddXY(args[i], mass[i][j]);
            //    if (radioButton4.Checked) for (int j = 0; j < mass[i].n; j++) chart1.Series[0].Points.AddXY(args[i], mass[i][j] / args[i]);
            //}
            if (checkBox2.Checked)
            {
                for (int i = 0; i < k; i++)
                {
                    if (radioButton3.Checked) for (int j = 0; j < mas[i].n; j++) chart1.Series[0].Points.AddXY(args[i], mas[i][j]);
                    if (radioButton4.Checked) for (int j = 0; j < mas[i].n; j++) chart1.Series[0].Points.AddXY(args[i], mas[i][j] / args[i]);
                }
            }
            if (checkBox3.Checked)
            {
                for (int i = 0; i < k; i++)
                {
                    if (radioButton3.Checked) for (int j = 0; j < masN[i].n; j++) chart1.Series[1].Points.AddXY(args[i], masN[i][j]);
                    if (radioButton4.Checked) for (int j = 0; j < masN[i].n; j++) chart1.Series[1].Points.AddXY(args[i], masN[i][j] / args[i]);
                }
            }

            if (radioButton3.Checked) chart1.Titles[0].Text = "График ζn(ω)";
            if (radioButton4.Checked) chart1.Titles[0].Text = "График ζn(ω)/ω";
            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].ToolTip = $"X = #VALX, Y = #VALY";
        }
    }
}
