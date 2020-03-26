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
using static МатКлассы.FuncMethods;
using JR.Utils.GUI.Forms;

namespace Решение_ОДУ.Дм.ПА
{
    public partial class SearchSol : Form
    {
        public Func<double,double,double>f = null;
        public Func<double,double> Searchfunc;
        public Func<double,double>[] mas;
        public SearchSol()
        {
            InitializeComponent();
            ClearChart();

            for(int i=0;i<8;i++)
                chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";

            mas = new Func<double,double>[6];
            label7.Hide();
            textBox6.Hide();
            button6.Hide();
            button7.Hide();
            Searchfunc = (double x) => Math.Sin(x);

            var t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }
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
            for(int i=0;i<chart1.Series.Count;i++)
            {
                chart1.Series[i].IsVisibleInLegend = false;
                chart1.Series[i].BorderWidth = 3;
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[i].Points.Clear();
            }
        }

        string netf = "";
        private void ShowNetFunc(NetFunc f)
        {
            for (int i = 0; i < f.CountKnots - 1; i++)
                netf += $"{f.Arg(i)} (+{f.Arg(i + 1)-f.Arg(i)}) ";
            netf += f.Arg(f.CountKnots - 1) + Environment.NewLine + Environment.NewLine;
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
                string s = textBox1.Text;
                try
                {
                    Func<double,double> q = Parser.GetDelegate(s);
                    f = (double x, double u) => q(x);
                    textBox1.Text = Parser.FORMULA;
                }
                catch
                {
                    f = (double u,double x) => 0;
                }
            }
            if (radioButton2.Checked) f = (double x, double u) =>u+3*Math.Exp(x)*Math.Cos(x);
            if (radioButton3.Checked) f = (double x, double u) =>u+2*x-3;
            if (radioButton4.Checked) f = (double x, double u) =>(1/x+u)/x;
            if (radioButton5.Checked) { f = (double x, double u) => 1 / Math.Cos(x) - u * Math.Tan(x); Searchfunc = (double x) => Math.Sin(x) + Math.Cos(x); }
            if (radioButton6.Checked) f = (double x, double u) =>(2*u-x)/(2*x+u);
            if (radioButton7.Checked) f = (double x, double u) => Math.Exp(x)-2*u;

            double beg = Convert.ToDouble(textBox2.Text), end = Convert.ToDouble(textBox4.Text), begval = Convert.ToDouble(textBox3.Text),eps=Convert.ToDouble(textBox7.Text);

           double step = Convert.ToDouble(textBox5.Text);
           if(radioButton9.Checked)
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
                string s = textBox6.Text;
                try
                {
                    Searchfunc = Parser.GetDelegate(s);
                    textBox6.Text = Parser.FORMULA;
                }
                catch
                {
                    Searchfunc = (double x) => 0;
                }
                double[] values = МатКлассы.Point.PointsY(Searchfunc, 100, beg, end);
                chart1.Series[8].Points.DataBindXY(МатКлассы.Point.PointsX(Searchfunc, 100, beg, end), values); list.AddRange(values);
                chart1.Series[8].IsVisibleInLegend = true;

                button6.Show();
            }

            info.Add($"-----Погрешности разных методов на отрезке интегрирования [{beg},{end}] c начальным шагом {step}:");
            if (checkBox1.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc( f), beg, end, step, ODU.Method.E1, begval,eps,checkBox10.Checked);list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[0].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[0].Name = "Метод Эйлера";
                chart1.Series[0].IsVisibleInLegend = true;
                info.Add($"\tУ метода Эйлера ({func.CountKnots}): \t{NetFunc.Distance(func,Searchfunc)}");
                if(checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal()-Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox2.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.E2, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[1].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[1].Name = "Метод Эйлера с пересчётом";
                chart1.Series[1].IsVisibleInLegend = true;
                info.Add($"\tУ метода Эйлера с пересчётом ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox3.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.H, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[2].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[2].Name = "Метод Хойна";
                chart1.Series[2].IsVisibleInLegend = true;
                info.Add($"\tУ метода Хойна ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox4.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.RK3, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[3].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[3].Name = "Метод Рунге-Кутты 3 порядка";
                chart1.Series[3].IsVisibleInLegend = true;
                info.Add($"\tУ метода Рунге-Кутты 3 порядка ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox5.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.RK4, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[4].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[4].Name = "Метод Рунге-Кутты 4 порядка";
                chart1.Series[4].IsVisibleInLegend = true;
                info.Add($"\tУ метода Рунге-Кутты 4 порядка ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox6.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.P38, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[5].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[5].Name = "Правило трёх восьмых";
                chart1.Series[5].IsVisibleInLegend = true;
                info.Add($"\tУ правила трёх восьмых ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox7.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.F, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[6].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[6].Name = "Метод Фельдберга";
                chart1.Series[6].IsVisibleInLegend = true;
                info.Add($"\tУ метода Фельдберга ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }
            if (checkBox8.Checked)
            {
                NetFunc func = ODU.ODUsearch(new DRealFunc(f), beg, end, step, ODU.Method.C, begval, eps, checkBox10.Checked); list.AddRange(func.Values);
                for (int i = 0; i < func.CountKnots; i++)
                    chart1.Series[7].Points.AddXY(func.Arg(i), func[i]);
                chart1.Series[7].Name = "Метод Ческино";
                chart1.Series[7].IsVisibleInLegend = true;
                info.Add($"\tУ метода Ческино ({func.CountKnots}): \t{NetFunc.Distance(func, Searchfunc)}");
                if (checkBox9.Checked) info.Add($"\t Точность на конце отрезка равна {Math.Abs(func.LastVal() - Searchfunc(func.LastArg()))}");
                ShowNetFunc(func);
            }

            double t = 0.1,min=list.Min(),max=list.Max();
            chart1.ChartAreas[0].AxisY.Minimum =  min* (1 - t*Math.Sign(min));
            chart1.ChartAreas[0].AxisY.Maximum =  max* (1 + t * Math.Sign(max));

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
            Application.Restart();
        }

        private double m = 0;
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            m++;
            m %= 2;
            if(m==1)
            {
                label7.Show();
                textBox6.Show();
            }
            else
            {
                label7.Hide();
                textBox6.Hide();
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
            FlexibleMessageBox.Show(netf,"Эскиз сетки в использованных методах");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            (new Система_ОДУ()).Show();
        }
    }
}
