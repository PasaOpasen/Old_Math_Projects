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
using static МатКлассы2018.FuncMethods.DefInteg;

namespace Интегралы.Дм.ПА
{
    public partial class IntegForm : Form
    {
        public IntegForm()
        {
            InitializeComponent();
            radioButton3.Checked = true;
            radioButton8.Checked = true;
            radioButton18.Checked = true;
            radioButton22.Checked = true;
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private const string message = "Все доступные поля должны быть заполнены действительными числами. Число шагов и число отрезков - натуральными числами. При записи действительных чисел используются запятые, а не точки.";
        private const string caption = "Неверные входные данные!";
        private RealFunc f;
        private Method C;
        private double a, b, eps;
        private int count, seq,nn;

        private void Read()
        {
            try
            {
                double aa = Convert.ToDouble(textBox1.Text),
                bb = Convert.ToDouble(textBox2.Text),
                epss = Convert.ToDouble(textBox4.Text);

                int countt = Convert.ToInt32(textBox3.Text),
                 seqq = Convert.ToInt32(textBox5.Text),
                 nnn= Convert.ToInt32(textBox9.Text);

                if (countt%2 ==1) errorProvider1.SetError(textBox3, "Нечётное число шагов может привести к неточности некоторых методов");
                //else errorProvider1.
                if (nnn > 25) errorProvider2.SetError(textBox9, "Большое число узлов может привести к заметным временным затратам");

            }
            catch
            {
                var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            f = Math.Sin;
            if (radioButton4.Checked) f = (double x) => { return Math.Cos(x) + 1; };
            if (radioButton5.Checked) f = (double x) => { return Math.Sin(x) * x; };
            if (radioButton7.Checked) f = (double x) => { return x * x + 2 * x - 1; };
            if (radioButton9.Checked) f = (double x) => { return Math.Exp(x) * Math.Sin(x); };
            if (radioButton10.Checked) f = (double x) => { return 4 * x * x * x - 3 * x * x + 1; };
            if (radioButton11.Checked) f = (double x) => { return 5; };
            if (radioButton12.Checked) f = (double x) => { return x / (1 + x * x); };
            if (radioButton13.Checked) f = (double x) => { return Math.Sin(2 * x) / (Math.Abs(Math.Cos(x)) + Math.Abs(x) + x * x); };
            if (radioButton14.Checked) f = (double x) => { return Math.Sin(x) + x * x; };
            if (radioButton15.Checked) f = (double x) => { return (x * x * x - x * x + 2 * x - 1) / (x * x - 2 * x + 6); };
            if (radioButton16.Checked) f = (double x) => { return Math.Sqrt(Math.Abs(x)); };
            if (radioButton17.Checked) f = (double x) => { return Math.Abs(Math.Sin(x)) + Math.Cos(x) - 1; };
            if (radioButton24.Checked)
            {
                string s = textBox8.Text;
                try
                {
                    f = Parser.GetDelegate(s);
                    textBox8.Text = Parser.FORMULA;
                }
                catch
                {
                    f = (double x) => 0;
                }
            }

            C = Method.Simpson;
            if (radioButton1.Checked) C = Method.MiddleRect;
            if (radioButton6.Checked) C = Method.Trapez;
            if (radioButton2.Checked) C = Method.Gauss;
            if (radioButton25.Checked) C = Method.Meler;
            if (radioButton26.Checked) C = Method.GaussKronrod15;
            if (radioButton27.Checked) C = Method.GaussKronrod61;
            if (radioButton28.Checked) C = Method.GaussKronrod61Empire;

            a = Convert.ToDouble(textBox1.Text);
            b = Convert.ToDouble(textBox2.Text);
            eps = Convert.ToDouble(textBox4.Text);
            double a0 = a, b0 = b;
            a = Math.Min(a0, b0);
            b = Math.Max(a0, b0);

            count = Convert.ToInt32(textBox3.Text);
             seq = Convert.ToInt32(textBox5.Text);
            nn = Convert.ToInt32(textBox9.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Read();
            double res = 0;

            if (radioButton21.Checked)
            {
                MonteKarloEnum KK = MonteKarloEnum.Usual;
                if (radioButton23.Checked) KK = MonteKarloEnum.Geo;
                МатКлассы2018.Point p = new МатКлассы2018.Point(a, b);
                MultiFunc F = (double[] t)=> f(t[0]);
                h_Count = count;
                n = count;
                EPS = Double.NaN;
                res = MonteKarlo(F, KK,p);
                textBox7.Text = Math.Abs(res-DefIntegral(f, a, b, C, Criterion.StepCount, count, eps, seq)).ToString();
                h_Count = count;
                n = count;
                EPS = Double.NaN;
                //return;
            }
            else
            { 
            Criterion K = Criterion.StepCount;
            if (radioButton19.Checked) K = Criterion.Accuracy;
            if (radioButton20.Checked) K = Criterion.SegmentCount;

           if((int)C<3) res = DefIntegral(f, a, b, C, K, count, eps, seq);
           else res = DefIntegral(f, a, b, C, K, nn, eps, seq);
            }
            textBox6.Text = res.ToString();
            textBox3.Text = h_Count.ToString();
            textBox4.Text = EPS.ToString();

        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            radioButton18.Checked = true;
            groupBox4.Show();
            label5.Show();
            textBox7.Show();
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton21.Checked) radioButton8.Checked = true;
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton21.Checked) radioButton8.Checked = true;
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox4.Hide();
            label5.Hide();
            textBox7.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Parser.INFORMATION, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void button4_Click(object sender, EventArgs e)
        {
            Program.PER = new Perc();
            Test();
            Program.PER.Show();
        }
        public void Test()
        {
            Read();
            double res = 0;

                Criterion K = Criterion.StepCount;
                if (radioButton19.Checked) K = Criterion.Accuracy;
                if (radioButton20.Checked) K = Criterion.SegmentCount;

            for (C = 0; (int)C <= 2; C++)
            {
                res = DefIntegral(f, a, b, C, K, count, eps, seq);
                Program.PER.listView1.Items[(int)C].SubItems[1].Text = res.ToString();
            }
            C +=2;
            res = DefIntegral(f, a, b, (Method)3, K, nn, eps, seq);
            Program.PER.listView1.Items[(int)C].SubItems[1].Text = res.ToString();
            C++;
            res = DefIntegral(f, a, b, (Method)4, K, nn, eps, seq);
            Program.PER.listView1.Items[(int)C].SubItems[1].Text = res.ToString();
            C++;
            res = DefIntegral(f, a, b, (Method)5, K, nn, eps, seq);
            Program.PER.listView1.Items[(int)C].SubItems[1].Text = res.ToString();

            for (MonteKarloEnum KK =0;(int)KK<2;KK++)
            {
                МатКлассы2018.Point p = new МатКлассы2018.Point(a, b);
                MultiFunc F = (double[] t) => f(t[0]);
                h_Count = count;
                n = count;
                EPS = Double.NaN;
                res = MonteKarlo(F, KK, p);
                Program.PER.listView1.Items[(int)KK+3].SubItems[1].Text = res.ToString();
            }
        }
    }
}
