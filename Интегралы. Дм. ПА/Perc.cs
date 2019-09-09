using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using МатКлассы;

namespace Интегралы.Дм.ПА
{
    public partial class Perc : Form
    {
        public Perc()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double v = Convert.ToDouble(textBox1.Text);
            for(int i=0;i< listView1.Items.Count;i++)
            {
                double tmp = Convert.ToDouble(listView1.Items[i].SubItems[1].Text);
                try
                {
                    listView1.Items[i].SubItems[2].Text = ((decimal)(Math.Abs(tmp - v) / v * 100)).ToString() + "%";
                }
                catch
                {
                    listView1.Items[i].SubItems[2].Text = (Math.Abs(tmp - v) / v * 100).ToString() + "%";
                }
            }
                    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.Close();
            Program.G.Test()/*button4_Click(sender, e)*/;
            button2_Click(sender,e);
        }
    }
}
