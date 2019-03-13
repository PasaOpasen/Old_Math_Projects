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
using static МатКлассы2018.FuncMethods;
using static МатКлассы2018.FuncMethods.ODU;
using JR.Utils.GUI.Forms;
using VectorNetFunc = System.Collections.Generic.List<System.Tuple<double, МатКлассы2018.Vectors>>;

namespace Решение_ОДУ.Дм.ПА
{
    public partial class Система_ОДУ : Form
    {
        public delegate Vectors VectorFunc(double x);
        VectorNetFunc[] mas=new VectorNetFunc[8];
        public Система_ОДУ()
        {
            InitializeComponent();
            ClearChart();

            for (int i = 0; i < 8; i++)
                chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";

            label7.Hide();
            dataGridView1.Hide();
            button6.Hide();
            button7.Hide();
            Searchfunc = null;

            var t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Start();

            dataGridView1.RowCount = 3;
            dataGridView2.RowCount = 3;
            dataGridView3.RowCount = 3;
            dataGridView1.ColumnCount = 1;
            dataGridView2.ColumnCount = 1;
            dataGridView3.ColumnCount = 1;
            dataGridView1.Columns[0].Width = 200;
            dataGridView2.Columns[0].Width = 220;
            dataGridView3.Columns[0].Width = 70;
            dataGridView1.BackgroundColor = Color.SlateBlue;
            dataGridView2.BackgroundColor = Color.SlateBlue;
            dataGridView3.BackgroundColor = Color.SlateBlue;
            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                int i = j + 1;
               dataGridView1.Rows[j].Cells[0].Value = $"sin({i}*x) + cos(x^{i})";
                dataGridView2.Rows[j].Cells[0].Value = $"{i}*cos({i}*x) - sin(x^{i})*{i}*(x^({i-1}))";
               dataGridView3.Rows[j].Cells[0].Value = Math.Sin(i*1)+Math.Cos(Math.Pow(1,i));              
            }

            button8.Hide();
            numericUpDown3.Hide();
            label9.Hide();
            numericUpDown2.Hide();
        }
        public VRealFunc f = null;
        public VectorFunc Searchfunc;
        //public RealFunc[] mas;

        int z = 0;
        void t_Tick(object sender, EventArgs e)
        {
            z++;
            z %= 2;
            if (z == 0)
                button2.BackColor = Color.White;
            else
                button2.BackColor = Color.Transparent;
        }

        private void ClearChart()
        {
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].IsVisibleInLegend = false;
                chart1.Series[i].BorderWidth = 3;
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[i].Points.Clear();
            }
        }

        string netf = "";
        private void ShowNetFunc(VectorNetFunc f)
        {
            for (int i = 0; i < f.Count - 1; i++)
                netf += $"{f[i].Item1} (+{f[i + 1].Item1 - f[i].Item1}) ";
            netf += f[f.Count - 1].Item1 + Environment.NewLine + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearChart();
            if (checkBox11.Checked)
                for (int i = 0; i < 8; i++)
                    chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            else
                for (int i = 0; i < 8; i++)
                    chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;

            if (radioButton1.Checked)
            { 
                try
                {
                    RealFunc[] q = new RealFunc[(int)numericUpDown2.Value];
                    for(int i=0;i<(int)numericUpDown2.Value;i++)
                    {
                        string s = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    q[i] = Parser.GetDelegate(s);
                    dataGridView2.Rows[i].Cells[0].Value = Parser.FORMULA;
                    }

                  f = (double x, Vectors u) =>
                  {
                      Vectors v = new Vectors((int)numericUpDown2.Value);
                      for (int i = 0; i < v.Deg; i++)
                          v[i] = q[i](x);
                      return v;
                  };
                }
                catch
                {
                    f = (double x, Vectors u) =>
                    {
                        Vectors v = new Vectors((int)numericUpDown2.Value);
                        for (int i = 0; i < v.Deg; i++)
                            v[i] = Math.Cos(x*(i+1));
                        return v;
                    };
                }
            }
            if (radioButton2.Checked)
                f = (double x, Vectors u) =>
                {
                    Vectors v = new Vectors(2);
                    v[0] = u[0] + 3 * Math.Exp(x) * Math.Cos(x);
                    v[1] = 1 / x * (1 / x + u[1]);
                    return v;
                };
            if (radioButton5.Checked) f = (double x, Vectors u) =>
           {
               Vectors v = new Vectors(2);
               v[0]= 1 / Math.Cos(x) - u[0] * Math.Tan(x);
               v[1] = Math.Exp(x) - 2 * u[1];
               return v;
           };

            double beg = Convert.ToDouble(textBox2.Text), end = Convert.ToDouble(textBox4.Text),  eps = Convert.ToDouble(textBox7.Text);

            Vectors begval;
            if (radioButton1.Checked) begval = new Vectors((int)numericUpDown2.Value);
            else begval = new Vectors(2);
            for (int i = 0; i < begval.Deg; i++) begval[i] = Convert.ToDouble(dataGridView3.Rows[i].Cells[0].Value);

            double step = Convert.ToDouble(textBox5.Text);
            if (radioButton9.Checked)
            {
                int stepcount = Convert.ToInt32(numericUpDown1.Value);
                step = (end - beg) / (stepcount);
            }

            List<double> list = new List<double>();
            info = new List<string>();
            netf = "";
            button7.Show();

            if (checkBox9.Checked)
            {
                try
                {
                    RealFunc[] q = new RealFunc[(int)numericUpDown2.Value];
                    for (int i = 0; i < (int)numericUpDown2.Value; i++)
                    {
                        string s = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        q[i] = Parser.GetDelegate(s);
                        dataGridView1.Rows[i].Cells[0].Value = Parser.FORMULA;
                    }

                    Searchfunc = (double x) =>
                    {
                        Vectors v = new Vectors((int)numericUpDown2.Value);
                        for (int i = 0; i < v.Deg; i++)
                            v[i] = q[i](x);
                        return v;
                    };
                }
                catch
                {
                    Searchfunc = (double x) =>
                    {
                        Vectors v = new Vectors((int)numericUpDown2.Value);
                        for (int i = 0; i < v.Deg; i++)
                            v[i] = Math.Sin(x * (i + 1)) / (i + 1);
                        return v;
                    };
                }
                RealFunc tmp = (double x) => Searchfunc(x)[0];
                double[] values = МатКлассы2018.Point.PointsY(tmp, 200, beg, end);
                chart1.Series[8].Points.DataBindXY(МатКлассы2018.Point.PointsX(tmp, 200, beg, end), values); //list.AddRange(values);
                chart1.Series[8].IsVisibleInLegend = true;

                button6.Show();
            }

            info.Add($"-----Погрешности разных методов на отрезке интегрирования [{beg},{end}] c начальным шагом {step}:");

            if (checkBox1.Checked)
            {
            VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.E1, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[0].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[0].Name = "Метод Эйлера";
                chart1.Series[0].IsVisibleInLegend = true;
                if(checkBox9.Checked)info.Add($"\tУ метода Эйлера ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[0] = func;
            }
            if (checkBox2.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.E2, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[1].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[1].Name = "Метод Эйлера с пересчётом";
                chart1.Series[1].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ метода Эйлера с пересчётом ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[1] = func;
            }
            if (checkBox3.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.H, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[2].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[2].Name = "Метод Хойна";
                chart1.Series[2].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ метода Хойна ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[2] = func;
            }
            if (checkBox4.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.RK3, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[3].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[3].Name = "Метод Рунге-Кутты 3 порядка";
                chart1.Series[3].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ метода Рунге-Кутты 3 порядка ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[3] = func;
            }
            if (checkBox5.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.RK4, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[4].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[4].Name = "Метод Рунге-Кутты 4 порядка";
                chart1.Series[4].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ метода Рунге-Кутты 4 порядка ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[4] = func;
            }
            if (checkBox6.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.P38, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[5].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[5].Name = "Правило трёх восьмых";
                chart1.Series[5].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ правила трёх восьмых ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[5] = func;
            }
            if (checkBox7.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.F, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[6].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[6].Name = "Метод Фельдберга";
                chart1.Series[6].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ метода Фельдберга ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[6] = func;
            }
            if (checkBox8.Checked)
            {
                VectorNetFunc func = ODU.ODUsearch(f, begval, beg, end, step, ODU.Method.C, eps, checkBox10.Checked); //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[7].Points.AddXY(func[i].Item1, func[i].Item2[0]);
                chart1.Series[7].Name = "Метод Ческино";
                chart1.Series[7].IsVisibleInLegend = true;
                if (checkBox9.Checked) info.Add($"\tУ метода Ческино ({func.Count}): \t{Distance(func, Searchfunc)}");
                ShowNetFunc(func);
                mas[7] = func;
            }
            //---------------------------------------------------------------------------------------------------------------------------------------------------
            //double t = 0.1, min = list.Min(), max = list.Max();
            //chart1.ChartAreas[0].AxisY.Minimum = min * (1 - t * Math.Sign(min));
            //chart1.ChartAreas[0].AxisY.Maximum = max * (1 + t * Math.Sign(max));

            label9.Show();
            numericUpDown2.Show();
            numericUpDown3.Value = 1;
        }

        private double Distance(VectorNetFunc f,VectorFunc S)
        {
            double s = 0;
            for(int i=0;i<f.Count;i++)
                s += (f[i].Item2 - S(f[i].Item1)).EuqlidNorm;
            return s/f.Count;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Parser.INFORMATION, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new Butcher()).Show();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new Система_ОДУ()).Show();
            this.Close();
        }

        private double m = 0;
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            m++;
            m %= 2;
            if (m == 1)
            {
                label7.Show();
                dataGridView1.Show();
            }
            else
            {
                label7.Hide();
                dataGridView1.Hide();
            }
        }

        List<string> info;
        private void button6_Click(object sender, EventArgs e)
        {
            string s = "";
            for (int i = 0; i < info.Count; i++)
                s += info[i] + Environment.NewLine;
            // FlexibleMessageBox.FONT = SystemFonts.CaptionFont;
            FlexibleMessageBox.Show(s);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FlexibleMessageBox.Show(netf, "Эскиз сетки в использованных методах");
        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            numericUpDown3.Maximum = numericUpDown2.Value;
            button8.Show();
            numericUpDown3.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            button5_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            button7_Click(sender, e);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            button6_Click(sender, e);
        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox9_CheckedChanged(sender,e);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int k = (int)numericUpDown2.Value;
            dataGridView1.RowCount = k;
            dataGridView2.RowCount = k;
            dataGridView3.RowCount = k;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ClearChart();
            if (checkBox11.Checked)
                for (int i = 0; i < 8; i++)
                    chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            else
                for (int i = 0; i < 8; i++)
                    chart1.Series[i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;
            double beg = Convert.ToDouble(textBox2.Text), end = Convert.ToDouble(textBox4.Text), eps = Convert.ToDouble(textBox7.Text);

            Vectors begval;
            if (radioButton1.Checked) begval = new Vectors((int)numericUpDown2.Value);
            else begval = new Vectors(2);
            for (int i = 0; i < begval.Deg; i++) begval[0] = Convert.ToDouble(dataGridView3.Rows[i].Cells[0].Value);

            double step = Convert.ToDouble(textBox5.Text);
            if (radioButton9.Checked)
            {
                int stepcount = Convert.ToInt32(numericUpDown1.Value);
                step = (end - beg) / (stepcount);
            }


            int k = (int)numericUpDown3.Value-1;
            RealFunc tmp = (double x) => Searchfunc(x)[k];
            double[] values = МатКлассы2018.Point.PointsY(tmp, 200, beg, end);
            chart1.Series[8].Points.DataBindXY(МатКлассы2018.Point.PointsX(tmp, 200, beg, end), values); //list.AddRange(values);
            chart1.Series[8].IsVisibleInLegend = true;

            if (checkBox1.Checked)
            {
                VectorNetFunc func = mas[0]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[0].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[0].Name = "Метод Эйлера";
                chart1.Series[0].IsVisibleInLegend = true;
            }
            if (checkBox2.Checked)
            {
                VectorNetFunc func = mas[1]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[1].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[1].Name = "Метод Эйлера с пересчётом";
                chart1.Series[1].IsVisibleInLegend = true;
            }
            if (checkBox3.Checked)
            {
                VectorNetFunc func = mas[2]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[2].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[2].Name = "Метод Хойна";
                chart1.Series[2].IsVisibleInLegend = true;
            }
            if (checkBox4.Checked)
            {
                VectorNetFunc func = mas[3]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[3].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[3].Name = "Метод Рунге-Кутты 3 порядка";
                chart1.Series[3].IsVisibleInLegend = true;
            }
            if (checkBox5.Checked)
            {
                VectorNetFunc func = mas[4]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[4].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[4].Name = "Метод Рунге-Кутты 4 порядка";
                chart1.Series[4].IsVisibleInLegend = true;
            }
            if (checkBox6.Checked)
            {
                VectorNetFunc func = mas[5]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[5].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[5].Name = "Правило трёх восьмых";
                chart1.Series[5].IsVisibleInLegend = true;
            }
            if (checkBox7.Checked)
            {
                VectorNetFunc func = mas[6]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[6].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[6].Name = "Метод Фельдберга";
                chart1.Series[6].IsVisibleInLegend = true;
            }
            if (checkBox8.Checked)
            {
                VectorNetFunc func = mas[7]; //list.AddRange(func.Values);
                for (int i = 0; i < func.Count; i++)
                    chart1.Series[7].Points.AddXY(func[i].Item1, func[i].Item2[k]);
                chart1.Series[7].Name = "Метод Ческино";
                chart1.Series[7].IsVisibleInLegend = true;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label9.Show();
            numericUpDown2.Show();
        }
    }
}

