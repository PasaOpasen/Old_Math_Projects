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
using VectorNetFunc = System.Collections.Generic.List<System.Tuple<double, МатКлассы.Vectors>>;
using JR.Utils.GUI.Forms;
using System.Threading;

namespace Решение_уравнения_теплопроводности.Дм.ПА
{
    public partial class Shoot : Form
    {
        int maxdim = 5;
        Color[] color = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Violet, Color.SandyBrown, Color.Pink, Color.Indigo,Color.Black };
        public Shoot()
        {
            InitializeComponent();            

            ClearChart();
            for (int i = 0; i < 4; i++)
            {
                var c = (DataGridView)this.Controls["dataGridView" + (i + 1).ToString()];
                c.RowCount = maxdim;
                c.ColumnCount = 1;
                c.Columns[0].Width = 200;
                c.BackgroundColor = Color.Blue;
            }
            numericUpDown4.Value = maxdim;
            numericUpDown3.Maximum = maxdim;

            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                int i = j + 1;
                dataGridView4.Rows[j].Cells[0].Value = $"sin({i}*x) + cos(x^{i})";
                dataGridView3.Rows[j].Cells[0].Value = $"{i}*cos({i}*x) - sin(x^{i})*{i}*(x^({i - 1}))";
                dataGridView2.Rows[j].Cells[0].Value = Math.Sin(i * 1) + Math.Cos(Math.Pow(1, i));
                dataGridView1.Rows[j].Cells[0].Value = $"cos(exp(x^{i}))/(1+x)";
            }
            numericUpDown5.Value = numericUpDown1.Value;

            dataGridView1.Hide(); dataGridView3.Hide(); dataGridView4.Hide();
            label4.Hide();label5.Hide();label6.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ClearChart()
        {
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].IsVisibleInLegend = false;
                chart1.Series[i].BorderWidth = 3;
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chart1.Series[i].Points.Clear();
                chart1.Series[i].Color = color[i];
                chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            maxdim = Convert.ToInt32(numericUpDown4.Value);
            numericUpDown3.Maximum = maxdim;
            for (int i = 0; i < 4; i++)
            {
                var c = (DataGridView)this.Controls["dataGridView" + (i + 1).ToString()];
                c.RowCount = maxdim;
            }
        }

        VRealFunc f;
        TwoVectorToVector F;
        Vectors alpha;
        VectorFunc u;
        VectorFunc res;
        List<VectorFunc> list;
        List<Vectors> vlist;

        VectorNetFunc[] tmp; VectorNetFunc result;List<VectorNetFunc> netlist;

        double beg, end, eps, step;
        int stepcount, count, ris;

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown5.Value = numericUpDown1.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = numericUpDown5.Value;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            string s = "";
            for (int i = 0; i < norms.Count; i++)
                s += norms[i] + Environment.NewLine;
            FlexibleMessageBox.Show(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            norms = new List<string>();
            chart1.Series.Clear();
            beg = Convert.ToDouble(textBox3.Text); end = Convert.ToDouble(textBox4.Text); eps = Convert.ToDouble(textBox2.Text); step = Convert.ToDouble(textBox1.Text);
            stepcount = Convert.ToInt32(numericUpDown1.Value); count = Convert.ToInt32(numericUpDown2.Value); ris=Convert.ToInt32(numericUpDown5.Value);
            if (radioButton1.Checked) stepcount = (int)((end - beg) / (step));

            list = new List<VectorFunc>(); vlist = new List<Vectors>();netlist = new List<VectorNetFunc>();
            Func<double,double>[] masf = new Func<double,double>[maxdim], masu = new Func<double,double>[maxdim], masF = new Func<double,double>[maxdim];
            alpha = new Vectors(maxdim);


            double pi3 = 3 * Math.PI / 2;
            for (int i = 0; i < maxdim; i++)
            {
                //masf[i] = Parser.GetDelegate(dataGridView3.Rows[i].Cells[0].Value.ToString());
                //masu[i] = Parser.GetDelegate(dataGridView4.Rows[i].Cells[0].Value.ToString());
                //masF[i] = Parser.GetDelegate(dataGridView1.Rows[i].Cells[0].Value.ToString());
                alpha[i] = Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value);
            }
            f = (double t, Vectors v) =>
              {
                  Vectors g = new Vectors(maxdim);
                  //for (int i = 0; i < maxdim; i++)
                  //    g[i] = masf[i](v[i]);
                  //g[0] = 0; g[1] = v[2]; g[2] = -v[1];
                  g[0] = 0;
                  g[1] = v[0]*v[2];
                  g[2] = -v[1];
                  g[3] = -Math.Sinh(pi3 - t) / Math.Cosh(pi3);
                  //g[4] = v[4]/15 + Math.Exp(t/15) * v[2];
                  g[4] = v[2] * (t + Math.Exp(v[1]))+v[1]; g[4] /= 100;
                  //g[5] = Math.Exp(t)/ 300 / Math.PI * (t*t+3*t+1) ;
                  //if (t == 0) g[5] = 1.0 / 3 / Math.PI;
                  //else g[5] = v[5]*(t * t + 3 * t + 1) / (t+1)/t;
                  return g;
              };
            F = (Vectors a,Vectors b) =>
            {
                double t = Math.Min(Math.Max(a.MaxAbs, b.MaxAbs).Reverse().Sqr(),1);t *= t;
                Vectors g = new Vectors(maxdim);
                //for (int i = 0; i < maxdim; i++)
                //    g[i] = masF[i](t);
                g[0] = t*(a[0]+b[0]-2);
                g[1] = ((a*b).Abs()*t/a.EuqlidNorm/b.EuqlidNorm).Sqr()/1000;
                g[2] = (a[2]+b[2]).Abs()*(a[2]-Math.Exp(-(b[2]+1)))+(a[3]-b[3]).Sqr().Sqr()*t/(1+ (a[3] - b[3]).Sqr());
                g[3] = Math.Abs(a[3] - b[3]) * t/100;
                double pow = (a[4] - b[4])*t;
                g[4] = ((1+pow.Sqr()).Reverse()-1).Abs()/1000;
                //g[5] = t*t*(a[5] * (a[1] + b[1] + a[3])*Math.Abs(b[5]-(1+3*Math.PI)*Math.Exp(3*Math.PI))/100);
                return g;
            };
            u = (double t) =>
            {
                Vectors g = new Vectors(maxdim);
                //for (int i = 0; i < maxdim; i++)
                //    g[i] = masu[i](t);
                g[1] = Math.Sin(t);
                g[2] = Math.Cos(t);
                g[0] = 1;               
                g[3] = Math.Cosh( pi3- t) / Math.Cosh(pi3)-1;
                //g[4] = Math.Exp(t/15) * Math.Sin(t) ;
                g[4] = Math.Exp(Math.Sin(t)) + t * Math.Sin(t)-1;g[4] /= 100;
                //g[5] = Math.Exp(t)*(1 + t) * t / 300 / Math.PI;
                return g;
            };


            bool ready = false;
            Thread task = new Thread(() =>
            {
                res = FuncMethods.ODU.ShootQu(f, F, alpha, out list, out vlist, out netlist, beg, end, stepcount, FuncMethods.ODU.Method.F, eps, Convert.ToDouble(textBox5.Text), checkBox1.Checked);
                ready = true;

                MessageBox.Show("Решение найдено! Нажми ОК для продолжения", "Успешно", MessageBoxButtons.OK);
            }
             );
            task.IsBackground = true;
            task.Start();          

            DialogResult dr = MessageBox.Show("Решение ищется, нужно подождать", "Выполняется поиск",MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if(dr==DialogResult.Cancel)
            {
                task.Abort();
                if(!ready) return;
            }
            else
            {
                //task.IsBackground = false;
            }

            result = new VectorNetFunc();
            tmp = new VectorNetFunc[count+1];
            if (!radioButton1.Checked) step = (end - beg) / (stepcount -1);

            int dim = /*Convert.ToInt32(numericUpDown3.Value)*/1;

            for (int i = 0; i < vlist.Count; i++)
                vlist[i].Show();

            //if(vlist.Count>1)


            chart1.Series.Add( $"Искомая функция ({dim})");
            chart1.Series.Add($"Функция u(t,α) α[{vlist.Count}] = {vlist.Last()} ({dim})" + Environment.NewLine);
            if(count>0)
            for (int i = 0; i<Math.Min(list.Count, 10); i+=Math.Max(Math.Min(list.Count,10)/count,1)/*i < list.Count - 1; i += Math.Max(list.Count / count, 1)*/)
                chart1.Series.Add($"Функция u(t,α) α = {vlist[i]} ({dim})");

            ClearChart();
            chart1.Series[0].IsVisibleInLegend = true; chart1.Series[0].BorderWidth = 4;
            chart1.Series[1].IsVisibleInLegend = true;chart1.Series[1].BorderWidth = 4;
            //if (vlist.Count > 1)
            int y = 0;
            if (count > 0)
                for (int i = 0; i<Math.Min(list.Count, 10); i+=Math.Max(Math.Min(list.Count,10)/count,1)/*i < list.Count - 1; i += Math.Max(list.Count / count, 1)*/)
            {
                chart1.Series[2 + y].Name = $"Функция u(t,α) α[{i+1}] = {vlist[i]} ({dim})";
                chart1.Series[2 + y++].IsVisibleInLegend = true;
            }

            //step = (end - beg) / (ris-1);
            y = 0;
            if (count > 0)
                for (int j = 0; j<Math.Min(list.Count, 10); j+=Math.Max(Math.Min(list.Count,10)/count,1)/*j < list.Count - 1; j += Math.Max(list.Count / count, 1)*/)
                tmp[y++] = new VectorNetFunc();

            for (int i = 0; i < stepcount; i++)
            {
                double arg = beg + i * step;
                chart1.Series[0].Points.AddXY(arg, u(arg)[0]);
                result.Add(new Tuple<double, Vectors>(arg, res(arg)));
                chart1.Series[1].Points.AddXY(arg, result.Last().Item2[0]);

                y = 0;
                if (count > 0)
                {
                    for (int j = 0; j<Math.Min(list.Count, 10); j+=Math.Max(Math.Min(list.Count,10)/count,1)/*j < list.Count - 1; j += Math.Max(list.Count / count, 1)*/)
                    //tmp[y++].Add(new Tuple<double, Vectors>(arg, list[j](arg)));
                    tmp[y++] = netlist[j];
                y = 0;
                for (int j = 0; j<Math.Min(list.Count, 10); j+=Math.Max(Math.Min(list.Count,10)/count,1)/*j < list.Count - 1; j += Math.Max(list.Count / count, 1)*/)
                    chart1.Series[2 + y].Points.AddXY(arg, tmp[y++][i].Item2[0]);
                }

            }

            norms.Add($"||u(t,α[{vlist.Count}])-u(t)|| = {Distance(/*result*/netlist[netlist.Count-1],u)}");y = 0;
            if (count > 0)
                for (int j = 0; j<Math.Min(list.Count, 10); j+=Math.Max(Math.Min(list.Count,10)/count,1)/*j < list.Count - 1; j += Math.Max(list.Count / count, 1)*/)
                norms.Add($"||u(t,α[{j+1}])-u(t)|| = {Distance(tmp[y++], u)}");
           

        }
        List<string> norms = new List<string>();
        private double Distance(VectorNetFunc f, VectorFunc S)
        {
            double s = 0;
            for (int i = 0; i < f.Count; i++)
                s += (f[i].Item2 - S(f[i].Item1)).EuqlidNorm;
            return s / f.Count;
        }


    private void button8_Click(object sender, EventArgs e)
    {
        int y = 0;
        int dim = Convert.ToInt32(numericUpDown3.Value) - 1;
        for (int i = 0; i < chart1.Series.Count; i++)
            chart1.Series[i].Points.Clear();

        chart1.Series[0].Name = ($"Искомая функция ({dim + 1})");
        chart1.Series[1].Name = ($"Функция u(t,α) α[{vlist.Count}] = {vlist.Last()} ({dim + 1})" + Environment.NewLine);
        if (count > 0)
            for (int i = 0; i < Math.Min(list.Count, 10); i += Math.Max(Math.Min(list.Count, 10) / count, 1)/*i < list.Count - 1; i += Math.Max(list.Count / count, 1)*/)
            {
                chart1.Series[2 + y].Name = $"Функция u(t,α) α[{i + 1}] = {vlist[i]} ({dim + 1})";
                chart1.Series[2 + y++].IsVisibleInLegend = true;
            }

        for (int i = 0; i < stepcount; i++)
        {
            double arg = beg + i * step;
            chart1.Series[0].Points.AddXY(arg, u(arg)[dim]);
            y = 0;
            if (count > 0)
                for (int j = 0; j < Math.Min(list.Count, 10); j += Math.Max(Math.Min(list.Count, 10) / count, 1)/*j < list.Count - 1; j += Math.Max(list.Count / count, 1)*/)
                    chart1.Series[2 + y].Points.AddXY(arg, tmp[y++][i].Item2[dim]);

            chart1.Series[1].Points.AddXY(arg, result[i].Item2[dim]);
        }
    }
    }
}
