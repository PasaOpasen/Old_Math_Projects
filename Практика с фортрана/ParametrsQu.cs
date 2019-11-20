using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Практика_с_фортрана.РабКонсоль;

namespace Практика_с_фортрана
{
    public partial class ParametrsQu : Form
    {
        public ParametrsQu()
        {
            InitializeComponent();
            textBox6.Text = c1.ToString();
            textBox8.Text = c2.ToString();
            textBox5.Text = h1.ToString();
            textBox9.Text = (h - h1).ToString();
            textBox4.Text = m1.ToString();
            textBox10.Text = m2.ToString();
            textBox7.Text = w.ToString();
            textBox11.Text = a.ToString();
            textBox1.Text = steproot.ToString();
            textBox2.Text = epsroot.ToString();
            textBox3.Text = polesBeg.ToString();
            textBox12.Text = polesEnd.ToString();
            textBox13.Text = ro.ToString();
            textBox14.Text = xzero.ToString();
            numericUpDown1.Value = countroot;

            double sqr(double q) => q * q;

            textBox6.TextChanged += (o, e) =>
            {
                textBox4.Text = (Convert.ToDouble(textBox13.Text) / sqr(Convert.ToDouble(textBox6.Text))).ToString();
            };
            textBox8.TextChanged += (o, e) =>
            {
                textBox10.Text = (Convert.ToDouble(textBox13.Text) / sqr(Convert.ToDouble(textBox8.Text))).ToString();
            };

            textBox13.TextChanged += (o, e) =>
            {
                textBox4.Text = (Convert.ToDouble(textBox13.Text) / sqr(Convert.ToDouble(textBox6.Text))).ToString();
                textBox10.Text = (Convert.ToDouble(textBox13.Text) / sqr(Convert.ToDouble(textBox8.Text))).ToString();
            };

        }

        private void button1_Click(object sender, EventArgs e)
        {
            c1 = Convert.ToDouble(textBox6.Text);
            c2 = Convert.ToDouble(textBox8.Text);
            h1 = Convert.ToDouble(textBox5.Text);
            h = h1 + Convert.ToDouble(textBox9.Text);
            m1 = Convert.ToDouble(textBox4.Text);
            m2 = Convert.ToDouble(textBox10.Text);
            w = Convert.ToDouble(textBox7.Text);
            a = Convert.ToDouble(textBox11.Text);
            steproot = Convert.ToDouble(textBox1.Text);
            epsroot = Convert.ToDouble(textBox2.Text);
            countroot = Convert.ToInt32(numericUpDown1.Value);
            polesBeg = Convert.ToDouble(textBox3.Text);
            polesEnd = Convert.ToDouble(textBox12.Text);
            ro = Convert.ToDouble(textBox13.Text);
            xzero = Convert.ToDouble(textBox14.Text);

            t1 = Math.Min(k1(w), k2(w)) / 2;
            t2 = t1;
            t3 = t1;
            t4 = Math.Max(k1(w), k2(w)) + 1;

            this.Close();
        }
    }
}
