using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Библиотека_графики;
using МатКлассы;
using static МатКлассы.Number;

namespace Практика_с_фортрана
{
    public partial class Energy : Form
    {
        Func<double,double> Ez, Ex,Ez0;
        public Energy()
        {
            InitializeComponent();
            Библиотека_графики.ForChart.SetToolTips(ref chart1);
            Библиотека_графики.ForChart.ClearPointsAndHideLegends(ref chart1);

            Ez0 = (double z) =>
            {
                ComplexFunc us = (Number.Complex x) => (РабКонсоль.uRes(x.Re, 0).Conjugate * РабКонсоль.q(x.Re) + РабКонсоль.uRes(-x.Re, 0).Conjugate * РабКонсоль.q(-x.Re)) / РабКонсоль.m1;
                return (FuncMethods.DefInteg.GaussKronrod.DINN_GK(us, 0, 0, 0, 0, 0, 0)).Re;
            };
            Ez = (double z) =>
              {
                  //ComplexFunc us = (Number.Complex x) => РабКонсоль.uRes(x.Re, z).Conjugate * (РабКонсоль.uRes(x.Re, z+ РабКонсоль.eps) - РабКонсоль.uRes(x.Re , z- РабКонсоль.eps)) / (2 * РабКонсоль.eps);
                  //ComplexFunc us_ = (Number.Complex x) => РабКонсоль.uRes(-x.Re, z).Conjugate * (РабКонсоль.uRes(-x.Re, z + РабКонсоль.eps) - РабКонсоль.uRes(-x.Re, z - РабКонсоль.eps)) / (2 * РабКонсоль.eps);
                  //return (FuncMethods.DefInteg.GaussKronrod.DINN_GK(us, 0, 0, 0, 0, 0, 0) + FuncMethods.DefInteg.GaussKronrod.DINN_GK(us_, 0, 0, 0, 0, 0, 0)).Abs;

                  //ComplexFunc us = (Number.Complex x) => (РабКонсоль.uRes(x.Re, z).Conjugate * (РабКонсоль.uRes(x.Re, z + РабКонсоль.eps) - РабКонсоль.uRes(x.Re, z - РабКонсоль.eps)) + РабКонсоль.uRes(-x.Re, z).Conjugate * (РабКонсоль.uRes(-x.Re, z + РабКонсоль.eps) - РабКонсоль.uRes(-x.Re, z - РабКонсоль.eps))) / (2 * РабКонсоль.eps);
                  //return (FuncMethods.DefInteg.GaussKronrod.DINN_GK(us, 0, 0, 0, 0, 0, 0)).Abs;

                  //ComplexFunc us = (Number.Complex x) => РабКонсоль.uRes(x.Re, z).Conjugate * РабКонсоль.uResdz(x.Re, z) + РабКонсоль.uRes(-x.Re, z).Conjugate * РабКонсоль.uResdz(-x.Re, z);
                  //return (FuncMethods.DefInteg.GaussKronrod.DINN_GK(us, 0, 0, 0, 0, 0, 0)).Re;

                  //ComplexFunc us = (Number.Complex x) => РабКонсоль.u(x.Re, z).Conjugate * РабКонсоль.u_(x.Re, z).Re + РабКонсоль.u(-x.Re, z).Conjugate * РабКонсоль.u_(-x.Re, z).Re;
                  //return (FuncMethods.DefInteg.GaussKronrod.DINN_GK(us, 0, 0, 0, 0, 0, 0)).Re;

                  ComplexFunc us = (Number.Complex x) => РабКонсоль.U_(x, z) * РабКонсоль.U(x.Conjugate, z).Conjugate + РабКонсоль.U_(-x, z) * РабКонсоль.U(-x.Conjugate, z).Conjugate;
                  return (FuncMethods.DefInteg.GaussKronrod.DINN_GK(us, РабКонсоль.t1, РабКонсоль.t2, РабКонсоль.t3, РабКонсоль.t4, РабКонсоль.tm)).Im / 2 / Math.PI;
              };
            Ex = (double x) =>
              {
                  //ComplexFunc u = (Number.Complex z) => (РабКонсоль.uRes(x + РабКонсоль.eps, z.Re) - РабКонсоль.uRes(x - РабКонсоль.eps, z.Re)) / (2 * РабКонсоль.eps) * РабКонсоль.uRes(x, z.Re).Conjugate;
                  //return FuncMethods.DefInteg.GaussKronrod.MySimpleGaussKronrod(u, -РабКонсоль.h, 0).Re;

                  ComplexFunc u = (Number.Complex z) => РабКонсоль.uResdx(x,z.Re) * РабКонсоль.uRes(x, z.Re).Conjugate;
                  return FuncMethods.DefInteg.GaussKronrod.MySimpleGaussKronrod(u, -РабКонсоль.h, 0).Im;

                  //Func<double,double> ureal = (double z) => u(z).Re;
                  //return FuncMethods.DefInteg.GaussKronrod.Integral(ureal, -РабКонсоль.h, 0);
              };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Библиотека_графики.ForChart.SaveImageFromChart(chart1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new DINN5().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }

        //private void SetPoles(double beg = 0.01,double end=15,double step=1e-3,double eps=1e-4,int count =50)
        //{
        //    РабКонсоль.Poles = ((Complex[])FuncMethods.Optimization.Halfc(РабКонсоль.delta, beg, end, step, eps, count))/*.Where(c=>c!=0).ToArray()*/;
        //    List<Complex> value = new List<Complex>(), newmas = new List<Complex>();

        //    double wtf = РабКонсоль.delta(РабКонсоль.Poles[0]).Abs;
        //    if (wtf < 1e-3)
        //        newmas.Add(РабКонсоль.Poles[0]);
        //    for (int j = 1; j < РабКонсоль.Poles.Length; j++)
        //        newmas.Add(РабКонсоль.Poles[j]);
        //    РабКонсоль.Poles = newmas.ToArray();
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            Библиотека_графики.ForChart.ClearPointsAndHideLegends(ref chart1);

            //РабКонсоль.c1 = Convert.ToDouble(textBox6.Text);
            //РабКонсоль.c2 = Convert.ToDouble(textBox8.Text);
            //РабКонсоль.h1 = Convert.ToDouble(textBox5.Text);
            //РабКонсоль.h = РабКонсоль.h1+ Convert.ToDouble(textBox9.Text);
            //РабКонсоль.m1 = Convert.ToDouble(textBox4.Text);
            //РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);
            //РабКонсоль.w = Convert.ToDouble(textBox7.Text);
            //РабКонсоль.a = Convert.ToDouble(textBox11.Text);

            РабКонсоль.SetPoles(РабКонсоль.polesBeg,РабКонсоль.polesEnd, РабКонсоль.steproot,РабКонсоль.epsroot,РабКонсоль.countroot);    

            double a = Convert.ToDouble(textBox1.Text), b = Convert.ToDouble(textBox2.Text);
            int count = Convert.ToInt32(numericUpDown1.Value);
            double h = (b - a) / (count - 1);

            double[] arg = new double[count], ez = new double[count], ex = new double[count], ez0 = new double[count],ex2=new double[count],ez0q=new double[count];

            for (int i = 0; i < count; i++)
                arg[i] = a + i * h;
            //arg.Show();
            if (checkBox1.Checked)
                Parallel.For(0, count, (int i) => ez[i] = Ez(arg[i]));
            if (checkBox3.Checked ||checkBox5.Checked)
                {
                    double tmp = Ez(0),tmpq=Ez0(0);
                for (int i = 0; i < count; i++)
                {
                    ez0[i] = tmp;
                    ez0q[i] = tmpq;
                }
                }
            if(checkBox2.Checked || checkBox4.Checked)
                Parallel.For(0, count, (int i) =>
                { ex[i] = Ex(arg[i]); ex2[i] = 2 * ex[i]; });

            if(checkBox1.Checked)
            {
                chart1.Series[0].IsVisibleInLegend = true;
                for (int i = 0; i < count; i++)
                    chart1.Series[0].Points.AddXY(arg[i], ez[i]);
            }
            if (checkBox2.Checked)
            {
                chart1.Series[1].IsVisibleInLegend = true;
                for (int i = 0; i < count; i++)
                    chart1.Series[1].Points.AddXY(arg[i], ex[i]);
            }
            if (checkBox3.Checked)
            {
                chart1.Series[2].IsVisibleInLegend = true;
                for (int i = 0; i < count; i++)
                    chart1.Series[2].Points.AddXY(arg[i], ez0[i]);
            }
            if (checkBox4.Checked)
            {
                chart1.Series[3].IsVisibleInLegend = true;
                for (int i = 0; i < count; i++)
                    chart1.Series[3].Points.AddXY(arg[i], ex2[i]);
            }
            if (checkBox5.Checked)
            {
                chart1.Series[4].IsVisibleInLegend = true;
                for (int i = 0; i < count; i++)
                    chart1.Series[4].Points.AddXY(arg[i], ez0q[i]);
            }

            Библиотека_графики.ForChart.SetAxisesY(ref chart1);

            Array.Sort(ex);
            label3.Text = $"max|E(x)| = {Math.Max(Math.Abs(ex[0]),Math.Abs(ex.Last()))}";
        }
    }
}
