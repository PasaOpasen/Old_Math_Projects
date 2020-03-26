using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Явная_и_неявная_схема.Дм.ПА
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            radioButton1_CheckedChanged(new object(), new EventArgs());
            numericUpDown1.Minimum = (decimal)Program.F1.x0;
            numericUpDown1.Maximum = (decimal)Program.F1.X;
            numericUpDown2.Minimum = (decimal)Program.F1.t0;
            numericUpDown2.Maximum = (decimal)Program.F1.T;
            numericUpDown1.Increment = (decimal)Program.F1.h;
            numericUpDown2.Increment = (decimal)Program.F1.tau;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Hide();numericUpDown1.Hide();
            label3.Show(); numericUpDown2.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Show(); numericUpDown1.Show();
            label3.Hide(); numericUpDown2.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0;i<3;i++)
                 Program.F1.chart1.Series[i].Points.Clear();
            double x=Convert.ToDouble(numericUpDown1.Value), t=Convert.ToDouble(numericUpDown2.Value);
            double ustep;

            Program.F1.chart1.Series[0].IsVisibleInLegend = true;
            if(radioButton2.Checked)
            {
                ustep = (Program.F1.T - Program.F1.t0) / 121;
                for(int i=0;i<122;i++)
                {
                    double arg = Program.F1.t0 + i * ustep;
                    Program.F1.chart1.Series[0].Points.AddXY(arg, Program.F1.u(arg, x));
                }
                Program.F1.chart1.Series[0].Name = $"u(t,{x})";

                int fix = (int)Math.Round((x - Program.F1.x0) / Program.F1.h);
                if(Program.F1.checkBox1.Checked)
                {
                    for(int i=0;i<Program.F1.tcount;i++)
                        Program.F1.chart1.Series[1].Points.AddXY(Program.F1.t0+i*Program.F1.tau, Program.F1.r1[i].Values[fix]);
                    Program.F1.chart1.Series[1].Name = $"Явная схема";
                    Program.F1.chart1.Series[1].IsVisibleInLegend = true;
                }
                if (Program.F1.checkBox2.Checked)
                {
                    for (int i = 0; i < Program.F1.tcount; i++)
                        Program.F1.chart1.Series[2].Points.AddXY(Program.F1.t0 + i * Program.F1.tau, Program.F1.r2[i].Values[fix]);
                    Program.F1.chart1.Series[2].Name = $"Неявная схема";
                    Program.F1.chart1.Series[2].IsVisibleInLegend = true;
                }
            }
            else
            {
                ustep = (Program.F1.X - Program.F1.x0) / 121;
                for (int i = 0; i < 122; i++)
                {
                    double arg = Program.F1.x0 + i * ustep;
                    Program.F1.chart1.Series[0].Points.AddXY(arg, Program.F1.u(t,arg ));
                }
                Program.F1.chart1.Series[0].Name = $"u({t},x)";

                int fix = (int)Math.Round((t - Program.F1.t0) / Program.F1.tau);
                if (Program.F1.checkBox1.Checked)
                {
                    for (int i = 0; i < Program.F1.xcount; i++)
                        Program.F1.chart1.Series[1].Points.AddXY(Program.F1.x0 + i * Program.F1.h, Program.F1.r1[fix].Values[i]);
                    Program.F1.chart1.Series[1].Name = $"Явная схема";
                    Program.F1.chart1.Series[1].IsVisibleInLegend = true;
                }
                if (Program.F1.checkBox2.Checked)
                {
                    for (int i = 0; i < Program.F1.xcount; i++)
                        Program.F1.chart1.Series[2].Points.AddXY(Program.F1.x0 + i * Program.F1.h, Program.F1.r2[fix].Values[i]);
                    Program.F1.chart1.Series[2].Name = $"Неявная схема";
                    Program.F1.chart1.Series[2].IsVisibleInLegend = true;
                }
            }

        }
    }
}
