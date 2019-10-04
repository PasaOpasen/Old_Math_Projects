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
using static МатКлассы.FuncMethods.ODU;

namespace Задача_Штурма_Лиувилля
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart1.Series[1].ToolTip = "X = #VALX, Y = #VALY";
            chart1.Series[0].ToolTip = "X = #VALX, Y = #VALY";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear(); chart1.Series[1].Points.Clear();

            double a, b, A, B, D, A1, B1, D1;
            a = Convert.ToDouble(textBox12.Text);b = Convert.ToDouble(textBox13.Text);
            A = textBox6.Text.ToDouble();B = textBox7.Text.ToDouble();D = textBox8.Text.ToDouble();
            A1 = textBox11.Text.ToDouble();B1 = textBox10.Text.ToDouble();D1 = textBox9.Text.ToDouble();

            Func<double,double> u, g, h, s, f;
            u = Parser.GetDelegate(textBox1.Text); 
            g = Parser.GetDelegate(textBox2.Text); 
            h = Parser.GetDelegate(textBox3.Text); 
            s = Parser.GetDelegate(textBox4.Text); 
            f = Parser.GetDelegate(textBox5.Text); 

//u(0).Show();
//g(0).Show();
//h(0).Show();
//s(0).Show();
//f(0).Show();

            int N = Convert.ToInt32(numericUpDown1.Value);
            double nev;
            FuncMethods.NetFunc res = SchLiuQu(g,h,s,f,out nev,a,b,N,A,B,D,A1,B1,D1,radioButton4.Checked);
            double t = (b - a) / (150 - 1);
            for(int i=0;i<150;i++)
            {
                double arg = a + i * t;
                chart1.Series[0].Points.AddXY(arg, u(arg));
            }
            chart1.Series[1].Points.DataBindXY(res.Arguments,res.Values);

            label15.Text ="Расстояние = "+FuncMethods.NetFunc.Distance(res,u).ToString();
            label16.Text= "Невязка = " + nev.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "1+t+sin(t)";
            textBox2.Text = "exp(t)";
            textBox3.Text = "-exp(t)";
            textBox4.Text = "exp(t)";
            textBox5.Text = "(1+t)*exp(t)";
            textBox6.Text = "-2";
            textBox7.Text = "1";
            textBox8.Text = "-3";
            textBox9.Text = $"{3*Math.PI+1}";
            textBox10.Text = "1";
            textBox11.Text = "9";
            textBox12.Text = "0";
            textBox13.Text = $"{3 * Math.PI}";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "exp(x)*sin(x)+1";
            textBox2.Text = "exp(-x)";
            textBox3.Text = "1";
            textBox4.Text = "exp(-x)-1";
            textBox5.Text = "cos(x)*(1+exp(x))+exp(-x)-1";
            textBox6.Text = "3";
            textBox7.Text = "-5";
            textBox8.Text = "-2";
            textBox9.Text = "3";
            textBox10.Text = "3";
            textBox11.Text = "0";
            textBox12.Text = "0";
            textBox13.Text = $"{3*Math.PI}";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "exp(-4*(x-1)^2)";
            textBox2.Text = "-0,125";
            textBox3.Text = "-(x-1)";
            textBox4.Text = "8*(x-1)^2-1";
            textBox5.Text = "8*(x-1)^2*exp(-4*(x-1)^2)";
            textBox6.Text = "1";
            textBox7.Text = "-16";
            textBox8.Text = $"0";         
            textBox11.Text = "1";
            textBox10.Text = "15";
            textBox9.Text = $"{-Math.Exp(-16)}";
            textBox12.Text = "-1";
            textBox13.Text = "3";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "x";
            textBox2.Text = "sin(x)*x";
            textBox3.Text = "exp(x)";
            textBox4.Text = "-cos(x)";
            textBox5.Text = "sin(x)+exp(x)";
            textBox6.Text = "8";
            textBox7.Text = "3";
            textBox8.Text = $"8";
            textBox11.Text = "6";
            textBox10.Text = "-3";
            textBox9.Text = $"0";
            textBox12.Text = "0";
            textBox13.Text = "2";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "x^2";
            textBox2.Text = "1";
            textBox3.Text = "x";
            textBox4.Text = "-2";
            textBox5.Text = "2";
            textBox6.Text = "3";
            textBox7.Text = "4";
            textBox8.Text = $"4";
            textBox11.Text = "3";
            textBox10.Text = "3";
            textBox9.Text = $"24";
            textBox12.Text = "-2";
            textBox13.Text = "2";
        }
    }
}
