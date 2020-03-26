using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсач
{
    public partial class FormResult : Form
    {
        public FormResult()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.FORM.Close();
            Program.Form1.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Процесс запущен" + Environment.NewLine;
            int k = Convert.ToInt32(Program.Form1.textBox7.Text);

            if (Program.Form1.radioButton1.Checked)
            {
                int n = Int32.Parse(Program.Form1.textBox3.Text);
                int cu = Int32.Parse(Program.Form1.textBox1.Text);
                int gf = Int32.Parse(Program.Form1.textBox2.Text);
                KursMethods.Desigion(n, gf, cu);

                textBox1.Text += "Данные считаны" + Environment.NewLine;

                if (Program.Form1.checkBox1.Checked)
                {
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    KursMethods.Illustrating();
                    Program.FORM.chart1.Refresh();
                }
                if (Program.Form1.checkBox2.Checked)
                {
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    KursMethods.Fixity();
                    Program.FORM.chart1.Refresh();
                }
            
            if (Program.Form1.checkBox3.Checked)
                {
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    
                    KursMethods.Quality(n, k, gf, cu); Program.FORM.chart1.Refresh();
                }
             }
            if (Program.Form1.radioButton2.Checked)
            {
                int a = Int32.Parse(Program.Form1.textBox4.Text);
                int b = Int32.Parse(Program.Form1.textBox5.Text);
                int h = Int32.Parse(Program.Form1.textBox6.Text);

                textBox1.Text += "Данные считаны" + Environment.NewLine;

                if (Program.Form1.checkBox6.Checked) KursMethods.Pictures_ill(a, b, h);//графики приближения для 4-10 функций с шагом 3
                if (Program.Form1.checkBox5.Checked) KursMethods.Pictures_fix(a, b, h);//картинки зависимости погрешности аппроксимации для от 30 до 40 функций, шаг 30
                if (Program.Form1.checkBox4.Checked) KursMethods.Pictures_qua(k, a, b, h); //картинки зависимости погрешности от кривой для 20 функций с кривыми от 40 до 100 и шагом 20
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            //KursMethods.Desigion(200,1,1);
            //KursMethods.Fixity(SLAUpok.Method.Gauss, SLAUpok.Method.GaussSpeedy);
            //chart1.SaveImage("iuf.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
