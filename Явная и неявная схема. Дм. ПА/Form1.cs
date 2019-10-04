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

namespace Явная_и_неявная_схема.Дм.ПА
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton2_CheckedChanged(new object(), new EventArgs());
            label6.Hide();
            label17.Text = "";
            label18.Text = "";
            for (int i = 0; i < 3; i++)
            {
chart1.Series[i].IsVisibleInLegend = false;
chart1.Series[i].ToolTip = "X = #VALX, Y = #VAL Y";
            }
                

            
        }

        public Func<double,double,double>u, f;
        public Func<double,double> u0, f1, f2;
        public double a, A1, B1, A2, B2, x0, X, t0, T,tau,h;

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            label10.Show(); label12.Show();
            textBox6.Show();
            textBox8.Show();

            if(radioButton1.Checked)
            {
                textBox6.Text = "-3";
                textBox7.Text = "1";
                textBox10.Text = "3*sin(t)";
                textBox8.Text = "4";
                textBox9.Text = "2";
                textBox11.Text = "18*sin(t)";
            }
            if(radioButton2.Checked)
            {
                textBox6.Text = "-2";
                textBox7.Text = "-2";
                textBox10.Text = "0";
                textBox8.Text = "1";
                textBox9.Text = "-1";
                textBox11.Text = "-15";
            }
            if(radioButton3.Checked)
            {
                textBox6.Text = "-2";
                textBox7.Text = "1";
                textBox10.Text = "t^2+2*t+2";
                textBox8.Text = "1";
                textBox9.Text = $"{-2.0 / Math.PI}";
                textBox11.Text = $"-t^2-{4.0 / Math.PI}";
            }

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label10.Hide();label12.Hide();
            textBox6.Hide();
            textBox8.Hide();

            if (radioButton1.Checked)
            {
                textBox7.Text = "8";
                textBox10.Text = "0";
                textBox9.Text = "2";
                textBox11.Text = "14*sin(t)";
            }
            if (radioButton2.Checked)
            {
                textBox7.Text = "2";
                textBox10.Text = "2*t";
                textBox9.Text = "0,2";
                textBox11.Text = $"5+{Math.Pow(Math.E, 5) / 5}*t";
            }
            if (radioButton3.Checked)
            {
                textBox7.Text = "2";
                textBox10.Text = "2*t^2+4";
                textBox9.Text = $"2";
                textBox11.Text = $"{Math.PI}*t+4";
            }
        }

        public int xcount, tcount;
        public List<FuncMethods.NetFunc> r1 = new List<FuncMethods.NetFunc>(), r2 = new List<FuncMethods.NetFunc>();

        void Clear()
        {
            for(int i=0;i<3;i++)
            {
                chart1.Series[i].Points.Clear();
                chart1.Series[i].IsVisibleInLegend = false;
            }
            label5.Hide();
            label6.Hide();
            label7.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();

            a = Convert.ToDouble(textBox5.Text);
            A1 = Convert.ToDouble(textBox6.Text);
            B1 = Convert.ToDouble(textBox7.Text);
            A2 = Convert.ToDouble(textBox8.Text);
            B2 = Convert.ToDouble(textBox9.Text);
            x0 = Convert.ToDouble(textBox3.Text);
            X = Convert.ToDouble(textBox4.Text);
            t0 = Convert.ToDouble(textBox1.Text);
            T = Convert.ToDouble(textBox2.Text);
            xcount = Convert.ToInt32(numericUpDown2.Value);
            tcount = Convert.ToInt32(numericUpDown1.Value);

            f1 = Parser.GetDelegate(textBox10.Text);
            f2 = Parser.GetDelegate(textBox11.Text);

            if(radioButton1.Checked)
            {
                u = (double t, double x) => x * Math.Sin(t);
                f = (double t, double x) => x * Math.Cos(t);
                u0 = (double x) => 0;
            }
            if (radioButton2.Checked)
            {
                u = (double t, double x) => x*x + t*Math.Exp(x);
                f = (double t, double x) => -2-Math.Exp(x)*(t-1);
                u0 = (double x) => x*x;
            }
            if (radioButton3.Checked)
            {
                u = (double t, double x) => x*t+2+Math.Cos(x)*t*t;
                f = (double t, double x) => Math.Cos(x)*t*(3*t+2)+x;
                u0 = (double x) => 2;
            }


            double eps1, eps2;
            h = (X - x0) / (xcount - 1);
            tau = (T - t0) / (tcount - 1);
            label17.Text = $"tau = {tau}";
            label18.Text = $"h = {h}";


            if(checkBox1.Checked)
            {
            r1 = FuncMethods.ODU.TU(new DRealFunc( f), f1, f2, u0,new DRealFunc( u), a, A1, B1, A2, B2, x0, X, t0, T, xcount, tcount, out eps1, true,radioButton5.Checked);

            label7.Text = $"aτ/h^2 = {a*tau/h/h}";
                label5.Show();
                label7.Show();
                label5.Text = $"Точность явной схемы = {eps1}";
            }
            if(checkBox2.Checked)
            {
                r2= FuncMethods.ODU.TU(new DRealFunc( f), f1, f2, u0, new DRealFunc( u), a, A1, B1, A2, B2, x0, X, t0, T, xcount, tcount, out eps2, false,radioButton5.Checked);
                label6.Show();
                label6.Text = $"Точность неявной схемы = {eps2}";
            }

            Program.F2 = new Form2();
            Program.F2.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label9.Text = "f(t,x) = xcos(t)";
            label16.Text = "u(0,x) = 0";
            textBox3.Text = "0";
            textBox4.Text = "7";
            textBox5.Text = "12";

            if(radioButton5.Checked)
            {
            textBox6.Text = "-3";
            textBox7.Text = "1";
            textBox10.Text = "3*sin(t)";
            textBox8.Text = "4";
            textBox9.Text = "2";
            textBox11.Text = "18*sin(t)";
            }
            else
            {
                textBox7.Text = "8";
                textBox10.Text = "0";
                textBox9.Text = "2";
                textBox11.Text = "14*sin(t)";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label9.Text = "f(t,x) = exp(x)(1-t)-2";
            label16.Text = "u(0,x) = x^2";
            textBox3.Text = "0";
            textBox4.Text = "5";
            textBox5.Text = "1";

            if(radioButton5.Checked)
            {
            textBox6.Text = "-2";
            textBox7.Text = "-2";
            textBox10.Text = "0";
            textBox8.Text = "1";
            textBox9.Text = "-1";
            textBox11.Text = "-15";
            }
            else
            {
                textBox7.Text = "2";
                textBox10.Text = "2*t";
                textBox9.Text = "0,2";
                textBox11.Text = $"5+{Math.Pow(Math.E,5)/5}*t";
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label9.Text = "f(t,x) = cos(x)t(3t+2)+x";
            label16.Text = "u(0,x) = 2";
            textBox3.Text = "0";
            textBox4.Text = $"{Math.PI/2}";
            textBox5.Text = "3";

            if(radioButton5.Checked)
            {
                textBox6.Text = "-2";
                textBox7.Text = "1";
                textBox10.Text = "t^2+2*t+2";
                textBox8.Text = "1";
                textBox9.Text = $"{-2.0 / Math.PI}";
                textBox11.Text = $"-t^2-{4.0 / Math.PI}";
            }
            else
            {
                textBox7.Text = "2";
                textBox10.Text = "2*t^2+4";
                textBox9.Text = $"2";
                textBox11.Text = $"{Math.PI}*t+4";
            }

        }
    }
}
