using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Functions;

namespace Работа2019
{
    public partial class ParametrsQu : Form
    {
        public ParametrsQu()
        {
            InitializeComponent();
            textBox6.Text = ro.ToString();
            textBox8.Text =h.ToString();
            textBox5.Text = lamda.ToString();
            textBox9.Text = mu.ToString();
            textBox7.Text = РабКонсоль.w.ToString();
            textBox1.Text = РабКонсоль.steproot.ToString();
            textBox2.Text = РабКонсоль.epsroot.ToString();
            textBox3.Text = РабКонсоль.polesBeg.ToString();
            textBox12.Text = РабКонсоль.polesEnd.ToString();
            numericUpDown1.Value = РабКонсоль.countroot;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ro = Convert.ToDouble(textBox6.Text);
            h = Convert.ToDouble(textBox8.Text);
            lamda = Convert.ToDouble(textBox5.Text);
            mu = Convert.ToDouble(textBox9.Text);
            РабКонсоль.w = Convert.ToDouble(textBox7.Text);
            РабКонсоль.steproot = Convert.ToDouble(textBox1.Text);
            РабКонсоль.epsroot = Convert.ToDouble(textBox2.Text);
            РабКонсоль.countroot = Convert.ToInt32(numericUpDown1.Value);
            РабКонсоль.polesBeg = Convert.ToDouble(textBox3.Text);
            РабКонсоль.polesEnd = Convert.ToDouble(textBox12.Text);
            AfterChaigeData();
            this.Close();
        }
    }
}
