using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Практика_с_фортрана
{
    public partial class ParametrsQu : Form
    {
        public ParametrsQu()
        {
            InitializeComponent();
            textBox6.Text = РабКонсоль.c1.ToString();
            textBox8.Text = РабКонсоль.c2.ToString();
            textBox5.Text = РабКонсоль.h1.ToString();
            textBox9.Text = (РабКонсоль.h-РабКонсоль.h1).ToString();
            textBox4.Text = РабКонсоль.m1.ToString();
            textBox10.Text = РабКонсоль.m2.ToString();
            textBox7.Text = РабКонсоль.w.ToString();
            textBox11.Text = РабКонсоль.a.ToString();
            textBox1.Text = РабКонсоль.steproot.ToString();
            textBox2.Text = РабКонсоль.epsroot.ToString();
            textBox3.Text = РабКонсоль.polesBeg.ToString();
            textBox12.Text = РабКонсоль.polesEnd.ToString();
            numericUpDown1.Value = РабКонсоль.countroot;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            РабКонсоль.c1 = Convert.ToDouble(textBox6.Text);
            РабКонсоль.c2 = Convert.ToDouble(textBox8.Text);
            РабКонсоль.h1 = Convert.ToDouble(textBox5.Text);
            РабКонсоль.h = РабКонсоль.h1+ Convert.ToDouble(textBox9.Text);
            РабКонсоль.m1 = Convert.ToDouble(textBox4.Text);
            РабКонсоль.m2 = Convert.ToDouble(textBox10.Text);
            РабКонсоль.w = Convert.ToDouble(textBox7.Text);
            РабКонсоль.a = Convert.ToDouble(textBox11.Text);
            РабКонсоль.steproot = Convert.ToDouble(textBox1.Text);
            РабКонсоль.epsroot = Convert.ToDouble(textBox2.Text);
            РабКонсоль.countroot = Convert.ToInt32(numericUpDown1.Value);
            РабКонсоль.polesBeg = Convert.ToDouble(textBox3.Text);
            РабКонсоль.polesEnd = Convert.ToDouble(textBox12.Text);
            this.Close();
        }
    }
}
