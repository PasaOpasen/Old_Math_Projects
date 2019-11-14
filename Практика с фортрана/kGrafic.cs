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

namespace Практика_с_фортрана
{
    public partial class kGrafic : Form
    {
        public kGrafic()
        {
            InitializeComponent();
            radioButton2.Hide();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        Vectors[] mas;
        double[] args;
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < chart1.Series.Count; i++)
            { chart1.Series.RemoveAt(i); i--; }
            

            //chart1.Series[0].Points.Clear();
            chart1.Series.Clear();
            if(radioButton1.Checked)
            {
                chart1.Series.Add(" ");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
            if (radioButton3.Checked) chart1.Titles[0].Text = "График ζn(ω)";
            if (radioButton4.Checked) chart1.Titles[0].Text = "График ζn(ω)/ω";
            for(int i=0;i<chart1.Series.Count;i++)
            chart1.Series[i].ToolTip = "X = #VALX, Y = #VALY";

            //РабКонсоль.c1 = Convert.ToDouble(textBox12.Text);
            //РабКонсоль.h1 = Convert.ToDouble(textBox11.Text);
            //РабКонсоль.m1 = Convert.ToDouble(textBox3.Text);
            //РабКонсоль.c2 = Convert.ToDouble(textBox2.Text);
            //РабКонсоль.h = РабКонсоль.h1 + Convert.ToDouble(textBox1.Text);
            //РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);

            double tmin = Convert.ToDouble(textBox7.Text), tmax = Convert.ToDouble(textBox6.Text), eps = РабКонсоль.epsroot, step = РабКонсоль.steproot;
            double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text);
            FuncMethods.Optimization.EPS = eps;
            FuncMethods.Optimization.STEP = step;

            int itcount = РабКонсоль.countroot,k =Convert.ToInt32(numericUpDown1.Value);

            double h = (end - beg) / (k - 1);
            mas = new Vectors[k];
            args = new double[k];

            for(int i=0;i<k;i++)
            {
                args[i] = beg + i * h;
                РабКонсоль.w = args[i];
               if(radioButton5.Checked) mas[i] = FuncMethods.Optimization.Halfc(РабКонсоль.delta, tmin, tmax, step, eps, itcount);
                if (radioButton6.Checked) mas[i] = FuncMethods.Optimization.Bisec(РабКонсоль.delta, tmin, tmax).ToDoubleMas();

                //корректировка корней
                if(checkBox1.Checked)
                {
                List<double> value = new List<double>(), newmas = new List<double>();
                for (int j = 0; j < mas[i].Deg; j++)
                {
                    double wtf = РабКонсоль.delta(mas[i][j]).Abs;
                    if (wtf < 10 * eps)
                    {
                        value.Add(wtf);
                        newmas.Add(mas[i][j]);
                    }
                }
                mas[i] = newmas.ToArray();
                Console.WriteLine($"{mas[i].ToString()} \t--> {(new Vectors(value.ToArray())).ToString()}");"".Show();
                }


                if (radioButton1.Checked &&radioButton3.Checked) for(int j=0;j<mas[i].Deg;j++) chart1.Series[0].Points.AddXY(args[i], mas[i].DoubleMas[j]);
                if (radioButton1.Checked && radioButton4.Checked) for (int j = 0; j < mas[i].Deg; j++) chart1.Series[0].Points.AddXY(args[i], mas[i].DoubleMas[j]/args[i]);
                textBox13.Text = РабКонсоль.w.ToString();
            }
            if(radioButton2.Checked) CurvesShow(mas,args);
        }
        private void CurvesShow(Vectors[] m,double[] ar)
        {
            var list = new List<Vectors>(m);
            var arg = new List<double>(ar);
            int k = 0;
            while(list.Count>0)
            {
                for(int i=0;i<list.Count;i++)
                    if(list[i].Deg<1)
                    {
                        list.RemoveAt(i);
                        arg.RemoveAt(i);
                        i--;
                    }

                for (int i = 1; i < list.Count; i++)
                   if (list[i].Deg == 1 && list[i -1].Deg == 1)
                    {
                        list.RemoveAt(i-1);
                        arg.RemoveAt(i-1);
                        i--;
                    }
                else if(list[i].Deg<list[i-1].Deg)
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

                for(int i=0;i<list.Count;i++)
                {
                    //list[i].Show();list[i].Deg.Show();
                    chart1.Series[k].Points.Add(arg[i],list[i][list[i].Deg-1]);
                    list[i] = new Vectors(list[i], 0, list[i].Deg - 2);
                }
                k++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //РабКонсоль.c = Convert.ToDouble(textBox1.Text);
            //РабКонсоль.h = Convert.ToDouble(textBox2.Text);
            //РабКонсоль.a = Convert.ToDouble(textBox3.Text);

            double tmin = Convert.ToDouble(textBox7.Text), tmax = Convert.ToDouble(textBox6.Text), eps = РабКонсоль.epsroot, step = РабКонсоль.steproot;
            double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text);

            int itcount = РабКонсоль.countroot, k = Convert.ToInt32(numericUpDown1.Value);

            string name = $"с = {РабКонсоль.c} h = {РабКонсоль.h} a = {РабКонсоль.a} tmin={tmin} tmax={tmax} eps={eps} step={step} kcount={k}  ([{beg},{end}])";
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
            //РабКонсоль.c1 = Convert.ToDouble(textBox12.Text);
            //РабКонсоль.h1 = Convert.ToDouble(textBox11.Text);
            //РабКонсоль.m1 = Convert.ToDouble(textBox3.Text);
            //РабКонсоль.c2 = Convert.ToDouble(textBox2.Text);
            //РабКонсоль.h = РабКонсоль.h1 + Convert.ToDouble(textBox1.Text);
            //РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);
            double beg = Convert.ToDouble(textBox4.Text), end = Convert.ToDouble(textBox5.Text); int itcount = РабКонсоль.countroot, k = Convert.ToInt32(numericUpDown1.Value);
            double h = (end - beg) / (k - 1);
            chart1.Series.Add("Re Δ"); chart1.Series[0].Color = Color.Red;
            chart1.Series.Add("Im Δ"); chart1.Series[1].Color = Color.Green;
            chart1.Series.Add("Abs Δ"); chart1.Series[2].Color = Color.Blue;
            for (int i=0;i<3;i++)
            {
                chart1.Series[ i].BorderWidth = 3;
                chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[ i].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            }
            for(int i=0;i<k;i++)
            {
                double arg = beg + i * h;
                Number.Complex val = РабКонсоль.delta(arg);val.Show();
                chart1.Series[0].Points.AddXY(arg, val.Re);
                chart1.Series[1].Points.AddXY(arg, val.Im);
                chart1.Series[2].Points.AddXY(arg, val.Abs);
            }
              
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }
    }
}
