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
using Библиотека_графики;

namespace Практика_с_фортрана
{
    public partial class TransformForm : Form
    {
        public TransformForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int count;
        double x, z, h1;
        Number.Complex a, b, h;
        ComplexFunc f;

        private void button1_Click(object sender, EventArgs e)
        {
            РабКонсоль.GRFORM.Init();
            //РабКонсоль.GRFORM.chart1.Series[0].Points.Clear(); РабКонсоль.GRFORM.chart1.Series[1].Points.Clear();

            count = Convert.ToInt32(numericUpDown1.Value);
            x = Convert.ToDouble(textBox1.Text);
            z = Convert.ToDouble(textBox2.Text);
            string s1 = "", s2 = "";

            if (radioButton1.Checked) { a = new Number.Complex(0); b = new Number.Complex(РабКонсоль.t1); s1 = $" на отрезке [0,t1={b}]"; }
            if (radioButton2.Checked) { a = new Number.Complex(РабКонсоль.t1, 0); b = new Number.Complex(РабКонсоль.t1, -РабКонсоль.tm); s1 = $" на отрезке [t1={a},t1-itm={b}]"; }
            if (radioButton3.Checked) { a = new Number.Complex(РабКонсоль.t1, -РабКонсоль.tm); b = new Number.Complex(РабКонсоль.t4, -РабКонсоль.tm); s1 = $" на отрезке [t1-itm={a},t4-itm={b}]"; }
            if (radioButton4.Checked) { a = new Number.Complex(РабКонсоль.t4, -РабКонсоль.tm); b = new Number.Complex(РабКонсоль.t4); s1 = $" на отрезке [t4-itm={a},t4={b}]"; }
            if (radioButton5.Checked) { a = new Number.Complex(РабКонсоль.t4); b = new Number.Complex(РабКонсоль.gr); s1 = $" на отрезке [t4={a},gr={b}]"; }

            h = (b - a) / (count - 1);
            h1 = (b - a).Abs / (count - 1);

            if (radioButton6.Checked) { f = (Number.Complex ee) => РабКонсоль.K1(ee, z) * РабКонсоль.Q(ee) * Number.Complex.Ch(Number.Complex.I * x * ee); s2 = $"K1(α,{z})Q(α)Ch(iα*{x})"; }
            if (radioButton7.Checked) { f = (Number.Complex ee) => РабКонсоль.K2(ee, z) * РабКонсоль.Q(ee) * Number.Complex.Ch(Number.Complex.I * x * ee); s2 = $"K2(α,{z})Q(α)Ch(iα*{x})"; }
            if (radioButton8.Checked) { f = (Number.Complex ee) => РабКонсоль.K1_(ee, z) * РабКонсоль.Q(ee) * Number.Complex.Ch(Number.Complex.I * x * ee); s2 = $"K1'(α,{z})Q(α)Ch(iα*{x})"; }
            if (radioButton9.Checked) { f = (Number.Complex ee) => РабКонсоль.K2_(ee, z) * РабКонсоль.Q(ee) * Number.Complex.Ch(Number.Complex.I * x * ee); s2 = $"K2'(α,{z})Q(α)Ch(iα*{x})"; }
            if (radioButton10.Checked) { f = (Number.Complex ee) => РабКонсоль.sigma1(ee); s2 = $"σ1(α)"; }
            if (radioButton11.Checked) { f = (Number.Complex ee) => РабКонсоль.sigma1(ee).Sqr(); s2 = $"σ1(α)^2"; }

            РабКонсоль.GRFORM.chart1.Series[0].Name = "Re " + s2 + s1;
            РабКонсоль.GRFORM.chart1.Series[1].Name = "Im " + s2 + s1;
            
            for (int i = 0; i < count; i++)
            {
                double arg = i * h1;
                Number.Complex arg1 = a + i * h;
                Number.Complex fx = f(arg1);$"f(при {arg}) = {fx}".Show();
                РабКонсоль.GRFORM.chart1.Series[0].Points.AddXY(arg, fx.Re);
                РабКонсоль.GRFORM.chart1.Series[1].Points.AddXY(arg, fx.Im);
            }


            ForChart.SetAxisesY(ref РабКонсоль.GRFORM.chart1);
            РабКонсоль.GRFORM.Refresh();
        }
    }
}
