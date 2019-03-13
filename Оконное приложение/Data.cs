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

namespace Оконное_приложение
{
    public partial class Data : Form
    {
        public Data()
        {
            InitializeComponent();
            checkBox1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double M = Convert.ToDouble(textBox4.Text);

            textBox2.Text = "";
            double x = Convert.ToDouble(textBox3.Text);
            if (Program.form.L != null)
                if (Program.form.f != null)
                {
                   textBox2.Text += String.Format("У полинома Лагранажа {0} (погрешность {1})", Program.form.L(x),Math.Abs(Program.form.L(x)- Program.form.f(x))) + Environment.NewLine;
                }  
                else textBox2.Text += String.Format("У полинома Лагранажа {0}", Program.form.L(x)) + Environment.NewLine;
            if (Program.form.N != null)
                if (Program.form.f != null)
                    textBox2.Text += String.Format("У полинома Ньютона {0} (погрешность {1})", Program.form.N(x), Math.Abs(Program.form.N(x) - Program.form.f(x))) + Environment.NewLine;
                else textBox2.Text += String.Format("У полинома Ньютона {0}", Program.form.N(x)) + Environment.NewLine;
            if (Program.form.R != null)
                if (Program.form.f != null)
                    textBox2.Text += String.Format("У рациональной функции {0} (погрешность {1})", Program.form.R(x), Math.Abs(Program.form.R(x) - Program.form.f(x))) + Environment.NewLine;
                else textBox2.Text += String.Format("У рациональной функции {0}", Program.form.R(x)) + Environment.NewLine;
            if (Program.form.S != null)
                if (Program.form.f != null)
                {
                    textBox2.Text += String.Format("У кубического сплайна {0} (погрешность {1})", Program.form.S(x), Math.Abs(Program.form.S(x) - Program.form.f(x))) + Environment.NewLine;
                    textBox2.Text += String.Format("Оценка погрешности сплайна равна {0}",5.0/384*Polynom.hmax*M);
                }
                else textBox2.Text += String.Format("У кубического сплайна {0}", Program.form.S(x)) + Environment.NewLine;
        }
    }
}
