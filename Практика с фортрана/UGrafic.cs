using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using МатКлассы2018;
using static МатКлассы2018.Number;
using Библиотека_графики;

namespace Практика_с_фортрана
{
    public partial class UGrafic : Form
    {
        public UGrafic()
        {
            InitializeComponent();
            colorDialog1.FullOpen = true;
            colorDialog1.Color = Color.Green;
            button3.Hide();
            ForChart.SetToolTips(ref chart1);
            label16.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "z:";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "x:";
        }

        double[] xval = null, uRval = null, uIval = null,umodval = null;
        double[] xvalres, uResRe, uResIm, uResAbs;
        int width = 3;Color color = Color.Blue;
        double fix, beg, end;

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void UGrafic_Load(object sender, EventArgs e)
        {

        }

        private void ResShow()
        {
            //label15.Show();/*label16.Show();*/label17.Show();label18.Show();
            //textBox12.Show();textBox13.Show();
            numericUpDown3.Show();//numericUpDown4.Show();
            label7.Show();
        }
        private void ResHide()
        {
            //label15.Hide(); label16.Hide(); label17.Hide(); label18.Hide();
            //textBox12.Hide(); textBox13.Hide();
            numericUpDown3.Hide(); //numericUpDown4.Hide();
            label7.Hide();
        }

        bool resis = true;
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            resis = !resis;
            if (resis) ResShow();
            else ResHide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Show();//numericUpDown3.Show(); ResShow();
            if (!(checkBox1.Checked || checkBox2.Checked || checkBox3.Checked)) { checkBox4.Checked = false; checkBox4.Hide(); numericUpDown3.Hide(); ResHide(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0;i<chart1.Series.Count;i++)
            {
chart1.Series[i].Points.Clear();
                chart1.Series[i].IsVisibleInLegend = false;
            }

            //simpleSound.Stop();
            button3.Show();

            //РабКонсоль.c1 =Convert.ToDouble(textBox6.Text);
            //РабКонсоль.h1 = Convert.ToDouble(textBox5.Text);
            //РабКонсоль.w = Convert.ToDouble(textBox7.Text);
            //РабКонсоль.m1 = Convert.ToDouble(textBox4.Text);
            //РабКонсоль.c2 = Convert.ToDouble(textBox8.Text);
            //РабКонсоль.h =РабКонсоль.h1+ Convert.ToDouble(textBox9.Text);
            //РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);
            //РабКонсоль.a = Convert.ToDouble(textBox11.Text);
            //РабКонсоль.epsroot = Convert.ToDouble(textBox12.Text);

            fix = Convert.ToDouble(textBox1.Text);
            beg = Convert.ToDouble(textBox2.Text); end = Convert.ToDouble(textBox3.Text);
            int k = Convert.ToInt32(numericUpDown1.Value),kres=Convert.ToInt32(numericUpDown3.Value);

            РабКонсоль.SetPolesDef();


            double h = (end - beg) / (k - 1),hres= (end - beg) / (kres - 1);
            xval = new double[k]; uRval = new double[k]; uIval = new double[k]; umodval = new double[k];
            xvalres = new double[kres];uResRe = new double[kres];uResIm = new double[kres];uResAbs = new double[kres];
           // chart1.Series[0].Color = this.color;
            chart1.Series[0].BorderWidth = this.width;

            if(radioButton2.Checked)
            {
                    chart1.Series[0].Name = $"Re(u({fix},z))";
                    chart1.Series[1].Name = $"Im(u({fix},z))";
                chart1.Series[2].Name=$"|u({fix},z)|";
            }
            else if(radioButton1.Checked)
            {
                chart1.Series[0].Name = $"Re(u(x,{fix}))";
                chart1.Series[1].Name = $"Im(u(x,{fix}))";
                chart1.Series[2].Name = $"|u(x,{fix})|";
            }

            if (radioButton2.Checked)
            {
                Parallel.For(0, k, (int i) =>
                {
                    xval[i] = beg + i * h;
                    Complex tmp = РабКонсоль.u(fix, xval[i]); $"u({fix} , {xval[i]}) = \t{tmp}".Show();
                    uRval[i] = tmp.Re;uIval[i] = tmp.Im;
                    umodval[i] = tmp.Abs;
                });
                if(checkBox4.Checked)
                    Parallel.For(0,kres,(int i)=> 
                    {
                        xvalres[i] = beg + i * hres;
                        Complex tmp = РабКонсоль.uRes(fix, xvalres[i]); $"uRes({fix} , {xvalres[i]}) = \t{tmp}".Show();
                        uResRe[i] = tmp.Re; uResIm[i] = tmp.Im;
                        uResAbs[i] = tmp.Abs;
                    });
            }
            else if (radioButton1.Checked)
            {
                Parallel.For(0, k, (int i) =>
                {
                    xval[i] = beg + i * h;
                    Complex tmp = РабКонсоль.u(xval[i],fix); $"u({xval[i]} , {fix}) = \t{tmp}".Show();
                    uRval[i] = tmp.Re; uIval[i] = tmp.Im;
                    umodval[i] = tmp.Abs;
                });
                if (checkBox4.Checked)
                    Parallel.For(0, kres, (int i) =>
                    {
                        xvalres[i] = beg + i * hres;
                        Complex tmp = РабКонсоль.uRes( xvalres[i],fix); $"uRes({xvalres[i]} , {fix}) = \t{tmp}".Show();
                        uResRe[i] = tmp.Re; uResIm[i] = tmp.Im;
                        uResAbs[i] = tmp.Abs;
                    });
            }


            //if (radioButton2.Checked)
            //    for(int i=0;i<k;i++)
            //    {
            //        xval[i] = beg + i * h;
            //        uRval[i] = РабКонсоль.u(new МатКлассы2018.Point(fix, xval[i]));
            //        umodval[i] = Math.Abs(uRval[i]);
            //    }
            //else if(radioButton1.Checked)
            //    for (int i = 0; i < k; i++)
            //    {
            //        xval[i] = beg + i * h;
            //        uRval[i] = РабКонсоль.u(new МатКлассы2018.Point(xval[i],fix));//uRval[i].Show();
            //        umodval[i] = Math.Abs(uRval[i]);
            //    }

            

            var list = new List<double>();
            if (checkBox1.Checked) { chart1.Series[0].Points.DataBindXY(xval, uRval); chart1.Series[0].IsVisibleInLegend = true; list.AddRange(uRval); if (checkBox4.Checked) { chart1.Series[3].Points.DataBindXY(xvalres, uResRe); chart1.Series[3].IsVisibleInLegend = true; list.AddRange(uResRe); } }
            if (checkBox2.Checked) { chart1.Series[2].Points.DataBindXY(xval, umodval); chart1.Series[2].IsVisibleInLegend = true; list.AddRange(umodval); if (checkBox4.Checked) { chart1.Series[5].Points.DataBindXY(xvalres, uResAbs); chart1.Series[5].IsVisibleInLegend = true; list.AddRange(uResAbs); } }
            if (checkBox3.Checked) { chart1.Series[1].Points.DataBindXY(xval, uIval); chart1.Series[1].IsVisibleInLegend = true; list.AddRange(uIval); if (checkBox4.Checked) { chart1.Series[4].Points.DataBindXY(xvalres, uResIm); chart1.Series[4].IsVisibleInLegend = true; list.AddRange(uResIm); } }

            //simpleSound.PlayLooping();

            double max = list.Max(), min = list.Min(),t=0.05;
            chart1.ChartAreas[0].AxisY.Minimum = (min > 0) ?min*(1-t):min*(1+t);
            chart1.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + t) : max * (1 - t);

            if(checkBox4.Checked)
            {
            РабКонсоль.Poles.Show();
            label16.Text=$"Полюсов найдено: {РабКонсоль.Poles.Length}";
                label16.Show();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            new DINN5().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = "x";
            if (radioButton2.Checked) s = "z";
            string name = $"{s} = {fix} ([{beg},{end}])";
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
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            this.color= colorDialog1.Color;
            chart1.BackColor = this.color;
            //chart1.Series[0].Color = colorDialog1.Color;
            //if (uRval != null) { chart1.Series[0].Points.Clear(); chart1.Series[0].Points.DataBindXY(xval, uRval); }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            width = Convert.ToInt32(numericUpDown2.Value);
            chart1.Series[0].BorderWidth = width;
            if (uRval != null) { chart1.Series[0].Points.Clear(); chart1.Series[0].Points.DataBindXY(xval, uRval); }
        }

        SoundPlayer simpleSound = new SoundPlayer(@"1.wav");

        private void button2_Click(object sender, EventArgs e)
        {
            simpleSound.Stop();
            this.Close();
        }
    }
}
