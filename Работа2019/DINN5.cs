using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Работа2019
{
    public partial class DINN5 : Form
    {
        System.Media.SoundPlayer s = new System.Media.SoundPlayer("2.wav");
        public DINN5()
        {
            InitializeComponent();
            if(!РабКонсоль.DINNplay) s.PlayLooping();
            var t = new Timer();
            t.Interval = 130;
            t.Tick += new EventHandler(t_Tick);
            t.Start();

            textBox1.Text = РабКонсоль.t1.ToString();
            textBox2.Text = РабКонсоль.t2.ToString();
            textBox3.Text = РабКонсоль.t3.ToString();
            textBox4.Text = РабКонсоль.t4.ToString();
            textBox5.Text = РабКонсоль.tm.ToString();
            textBox6.Text = РабКонсоль.tp.ToString();
            textBox7.Text = РабКонсоль.eps.ToString();
            textBox8.Text = РабКонсоль.pr.ToString();
            textBox9.Text = РабКонсоль.gr.ToString();

            Color c = Color.FromArgb(40, Color.Transparent);
            foreach (Label r in this.Controls.OfType<Label>())
            {
                r.BackColor = c;
            }

            listBox1.SelectedItem = gkn[Convert.ToInt32(РабКонсоль.NodesCount)].ToString();
        }
        int[] gkn = new int[] { 15, 21, 31, 41, 51, 61 };
        int z = 0;
        int[] mas = new int[] { 1, 2, 3,4,5,6,7,8,9,8,7,6,5,4,3, 2};
        void t_Tick(object sender, EventArgs e)
        {
            z++;
            z %= mas.Length;
            button2.BackColor = Color.FromArgb(mas[z]*7,Color.Transparent);//Color.Transparent;
            button1.BackColor = Color.FromArgb((mas.Max()-mas[z]) * 7, Color.Transparent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) { s.Stop(); РабКонсоль.DINNplay = false; }
            else РабКонсоль.DINNplay = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            РабКонсоль.t1 = Convert.ToDouble(textBox1.Text);
            РабКонсоль.t2 = Convert.ToDouble(textBox2.Text);
            РабКонсоль.t3 = Convert.ToDouble(textBox3.Text);
            РабКонсоль.t4 = Convert.ToDouble(textBox4.Text);
            РабКонсоль.tm = Convert.ToDouble(textBox5.Text);
            РабКонсоль.tp = Convert.ToDouble(textBox6.Text);
            РабКонсоль.eps = Convert.ToDouble(textBox7.Text);
            РабКонсоль.pr = Convert.ToDouble(textBox8.Text);
            РабКонсоль.gr = Convert.ToDouble(textBox9.Text);
            МатКлассы2018.FuncMethods.DefInteg.Residue.eps = РабКонсоль.eps;

            string nc = (string)listBox1.SelectedItem;
            switch(nc)
            {
             case   "15":
                    РабКонсоль.NodesCount = МатКлассы2018.FuncMethods.DefInteg.GaussKronrod.NodesCount.GK15;
                    break;
                case "21":
                    РабКонсоль.NodesCount = МатКлассы2018.FuncMethods.DefInteg.GaussKronrod.NodesCount.GK21;
                    break;
                case "31":
                    РабКонсоль.NodesCount = МатКлассы2018.FuncMethods.DefInteg.GaussKronrod.NodesCount.GK31;
                    break;
                case "41":
                    РабКонсоль.NodesCount = МатКлассы2018.FuncMethods.DefInteg.GaussKronrod.NodesCount.GK41;
                    break;
                case "51":
                    РабКонсоль.NodesCount = МатКлассы2018.FuncMethods.DefInteg.GaussKronrod.NodesCount.GK51;
                    break;
                case "61":
                    РабКонсоль.NodesCount = МатКлассы2018.FuncMethods.DefInteg.GaussKronrod.NodesCount.GK61;
                    break;
            }
        }
    }
}
