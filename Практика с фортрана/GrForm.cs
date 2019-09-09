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
using Библиотека_графики;


namespace Практика_с_фортрана
{
    public partial class GrForm : Form
    {
        public GrForm()
        {
            InitializeComponent();
            chart1.Series[0].IsVisibleInLegend = false; chart1.Series[1].IsVisibleInLegend = false;
            chart1.Series[0].ToolTip = "X = #VALX, Y = #VALY";
            chart1.Series[1].ToolTip = "X = #VALX, Y = #VALY";
        }
        double beg, end, h;
        int k;

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "μ1 |U'(α,0)|";
            chart1.Series[1].Name = "|Q(α)|";
            for(int i=0;i<k;i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.K1_(arg[i], 0) * РабКонсоль.m1* РабКонсоль.Q(arg[i])).Abs;
                f2[i] = РабКонсоль.Q(arg[i]).Abs;
            }
            chart1.Series[0].Points.DataBindXY(arg,f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "μ2 |U'(α,-h)|";
            chart1.Series[1].Name = "0";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.K2_(arg[i], -РабКонсоль.h) * РабКонсоль.m2 * РабКонсоль.Q(arg[i])).Abs;
                f2[i] = 0;
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "|K1(α,-h1)Q(α)|";
            chart1.Series[1].Name = "|K2(α,-h1)Q(α)|";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.K1(arg[i], -РабКонсоль.h1) * РабКонсоль.Q(arg[i])).Abs;
                f2[i] = (РабКонсоль.K2(arg[i], -РабКонсоль.h1) * РабКонсоль.Q(arg[i])).Abs; ;
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "μ1 |K1'(α,-h1)Q(α)|";
            chart1.Series[1].Name = "μ2 |K2'(α,-h1)Q(α)|";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = РабКонсоль.m1 * (РабКонсоль.K1_(arg[i], -РабКонсоль.h1) * РабКонсоль.Q(arg[i])).Abs;
                f2[i] = РабКонсоль.m2 * (РабКонсоль.K2_(arg[i], -РабКонсоль.h1) * РабКонсоль.Q(arg[i])).Abs; ;
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new DINN5().Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "μ1 u'(x,0)";
            chart1.Series[1].Name = "q(x)";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.uk1_(arg[i], 0) * РабКонсоль.m1).Re;f1[i].Show();
                f2[i] = РабКонсоль.q(arg[i]);
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "μ2 u'(x,-h)";
            chart1.Series[1].Name = "0";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.uk2_(arg[i], -РабКонсоль.h) * РабКонсоль.m2).Re; f1[i].Show();
                f2[i] = 0;
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "∫K1(α,-h1)Q(α)";
            chart1.Series[1].Name = "∫K2(α,-h1)Q(α)";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.uk1(arg[i], -РабКонсоль.h1)).Re;
                f2[i] = (РабКонсоль.uk2(arg[i], -РабКонсоль.h1)).Re;
                if(i!=0)
                    if ((f2[i] - f2[i - 1]).Abs() > РабКонсоль.epsjump)
                    {
                        $"Для пары аргументов {arg[i-1]} и {arg[i]}".Show();
                        for (int s = 0; s < 2; s++)
                            for (int y = 0; y < FuncMethods.DefInteg.GaussKronrod.MasListDinnInfo[2*s+1].Count; y++)
                                FuncMethods.DefInteg.GaussKronrod.MasListDinnInfo[2*s+1][y].Show();
                    }

            }
            //for (int i = 1; i < k; i++)
            //    if ((f2[i] - f2[i - 1]).Abs() > РабКонсоль.epsjump)
            //        for (int s = 0; s < 2; s++)
            //        for(int y=0;y< FuncMethods.DefInteg.GaussKronrod.MasListDinnInfo[s].Count;y++)
            //            FuncMethods.DefInteg.GaussKronrod.MasListDinnInfo[s][y].Show();
                
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Init();
            chart1.Series[0].Name = "μ1 ∫K1'(α,-h1)Q(α)";
            chart1.Series[1].Name = "μ2 ∫K2'(α,-h1)Q(α)";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = РабКонсоль.m1 * (РабКонсоль.uk1_(arg[i], -РабКонсоль.h1)).Re; f1[i].Show();
                f2[i] = РабКонсоль.m2 * (РабКонсоль.uk2_(arg[i], -РабКонсоль.h1)).Re;
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Init();

            string name = $"с1 = {РабКонсоль.c1} с2 = {РабКонсоль.c2} h1 = {РабКонсоль.h1} h2 = {РабКонсоль.h-РабКонсоль.h1} a = {РабКонсоль.a} m1 = {РабКонсоль.m1} m2 = {РабКонсоль.m2} w = {РабКонсоль.w} ([{beg},{end}])";
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

        private void button12_Click(object sender, EventArgs e)
        {
            new TransformForm().Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Init();
            РабКонсоль.SetPolesDef();
            chart1.Series[0].Name = "μ1 u(res)'(x,0)";
            chart1.Series[1].Name = "q(x)";
            for (int i = 0; i < k; i++)
            {
                arg[i] = beg + i * h;
                f1[i] = (РабКонсоль.uResdz(arg[i], 0) * РабКонсоль.m1).Re; f1[i].Show();
                f2[i] = РабКонсоль.q(arg[i]);
            }
            chart1.Series[0].Points.DataBindXY(arg, f1);
            chart1.Series[1].Points.DataBindXY(arg, f2);

            ForChart.SetAxisesY(ref chart1);
        }

        double[] arg, f1, f2;
        public void Init()
        {
            //РабКонсоль.c1 = Convert.ToDouble(textBox6.Text);
            //РабКонсоль.c2 = Convert.ToDouble(textBox8.Text);
            //РабКонсоль.h1 = Convert.ToDouble(textBox5.Text);
            //РабКонсоль.h = РабКонсоль.h1+ Convert.ToDouble(textBox9.Text);
            //РабКонсоль.m1 = Convert.ToDouble(textBox4.Text);
            //РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);
            //РабКонсоль.w = Convert.ToDouble(textBox7.Text);
            //РабКонсоль.a = Convert.ToDouble(textBox3.Text);

            beg = Convert.ToDouble(textBox2.Text);
            end = Convert.ToDouble(textBox1.Text);
            int i = Convert.ToInt32(numericUpDown1.Value);
            arg = new double[i];
            f1 = new double[i];
            f2 = new double[i];
            h = (end - beg) / (i - 1);
            k = i;

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[0].IsVisibleInLegend = true; chart1.Series[1].IsVisibleInLegend = true;
        }

    }   
}
