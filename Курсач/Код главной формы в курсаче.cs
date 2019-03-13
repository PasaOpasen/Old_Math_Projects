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
using System.IO;
using Point = МатКлассы2018.Point;

namespace Курсач
{
    public partial class MyForm : Form
    {
        static int p = 0;
        public MyForm()
        {
            InitializeComponent();

            radioButton1.Checked = true;
            checkBox1.Checked = true;
            checkBox6.Checked = true;

            textBox1.Text = "1";
            textBox2.Text = "1";
            textBox3.Text = "8";
            textBox4.Text = "10";
            textBox5.Text = "40";
            textBox6.Text = "5";
            textBox7.Text = "15";

            //groupBox2.Hide();
            groupBox3.Hide();
            label7.Hide();
            textBox7.Hide();
        }

        private void Курсач_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FormResult FORM = new FormResult();
            Program.FORM.textBox1.Text += "----> Окно запущено" + Environment.NewLine;
            Program.FORM.ShowDialog();
            //Program.FORM.Show();
            
            //if(radioButton1.Checked)
            //{
            //    int n = Int32.Parse(textBox3.Text);
            //    int cu = Int32.Parse(textBox1.Text);
            //    int gf = Int32.Parse(textBox2.Text);
            //    KursMethods.Desigion(n,gf,cu);

            //    if(checkBox1.Checked) KursMethods.Illustrating();
            //    if (checkBox2.Checked) KursMethods.Fixity();
            //    if (checkBox3.Checked) KursMethods.Quality(n, 20, gf, cu);
            //}
            //if(radioButton2.Checked)
            //{
            //    int a = Int32.Parse(textBox4.Text);
            //    int b = Int32.Parse(textBox5.Text);
            //    int h = Int32.Parse(textBox6.Text);

            //    if (checkBox6.Checked) KursMethods.Pictures_ill(a, b, h);//графики приближения для 4-10 функций с шагом 3
            //    if (checkBox5.Checked) KursMethods.Pictures_fix(a, b, h);//картинки зависимости погрешности аппроксимации для от 30 до 40 функций, шаг 30
            //    if (checkBox4.Checked) KursMethods.Pictures_qua(20, a, b, h); //картинки зависимости погрешности от кривой для 20 функций с кривыми от 40 до 100 и шагом 20
            //}

            ///////FileGrafic();
            //Make_TestFuncAndCurve();

            //KursMethods.Desigion(50);//заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения

            //Program.FORM.textBox1.Text += "Illustrating" + Environment.NewLine;

            //KursMethods.Illustrating(KursMethods.fi, KursMethods.masPoints, KursMethods.MySLAU.x, KursMethods.myCurve, 1);// график граничной функции и приближения

            //KursMethods.Desigion(70,2,2);
            //KursMethods.Illustrating(KursMethods.fi, KursMethods.masPoints, KursMethods.MySLAU.x, KursMethods.myCurve, 1);

            //Program.FORM.textBox1.Text += "Fixity" + Environment.NewLine;
            //KursMethods.Fixity();//график зависимости погрешности аппроксимации от числа базисных точек

            //Program.FORM.textBox1.Text += "Quality" + Environment.NewLine;
            //KursMethods.Quality(10, 8, 0, 0);//график зависимости погрешности от кривой, возле которой берутся базисные точки

            //KursMethods.Pictures_fix(30, 180, 10);//картинки зависимости погрешности аппроксимации для от 30 до 40 функций, шаг 30
            //KursMethods.Pictures_qua(40, 25, 105, 50); //картинки зависимости погрешности от кривой для 20 функций с кривыми от 40 до 100 и шагом 20
            //KursMethods.Pictures_ill(5,30,5);//графики приближения для 4-10 функций с шагом 3
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.Form1.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Hide();
            groupBox3.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Hide();
            groupBox2.Show();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            label7.Show();
            textBox7.Show();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            p++;
            if(p%2!=0)
            {
            label7.Show();
            textBox7.Show();
            }
            else
            {
                label7.Hide();
                textBox7.Hide();
            }
            p %= 2;
        }
    }

}

