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

namespace Критерии_принятия_решений_в_условиях_неопределённости
{
    public partial class Критерии : Form
    {
        public Критерии()
        {
            InitializeComponent();
            dataGridView1.RowCount = 6;
            dataGridView1.ColumnCount = 5;
            dataGridView3.RowCount = 5;
            dataGridView3.ColumnCount = 1;
            dataGridView2.RowCount = 2;
            dataGridView2.ColumnCount = 5;
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                dataGridView1.Columns[i].Width = 40;
                dataGridView2.Columns[i].Width = 80;
                dataGridView2.Rows[0].Cells[i].Value=0.2;
                for (int j = 0; j < 5; j++)
                    dataGridView1.Rows[i].Cells[j].Value = (i + j) % 7 + rand.Next(-i - 9, j);
            }
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            //dataGridView3.AllowUserToAddRows = false;

            radioButton1.Checked = true;
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;

            label3.Hide();
            dataGridView3.Hide();
            textBox1.Hide();

            //label4.Hide();
            //dataGridView2.Hide();

            //label5.Hide();
            //textBox2.Hide();

            //label6.Hide();
            //textBox3.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView3.Rows[i].Cells[0].Value = 0;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    dataGridView1.Rows[i].Cells[j].Value = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Matrix M = new Matrix(dataGridView1.RowCount, dataGridView1.ColumnCount);
            for (int i = 0; i < M.RowCount; i++)
                for (int j = 0; j < M.ColCount; j++)
                    M[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            int tmp = Convert.ToInt32(numericUpDown1.Value);
            dataGridView1.ColumnCount = tmp;

            dataGridView2.ColumnCount = tmp;
            for(int i=0;i<tmp;i++)
            dataGridView2.Rows[0].Cells[i].Value = 1.0/tmp;

            if(tmp>M.ColCount)
                for(int i=M.ColCount;i<tmp;i++)
                {
                    dataGridView1.Columns[i].Width = 40;
                    dataGridView2.Columns[i].Width = 80;
                    for (int j = 0; j < M.RowCount; j++)
                        dataGridView1.Rows[j].Cells[i].Value = 0;
                }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void firstRowNum_ValueChanged(object sender, EventArgs e)
        {
            dataGridView3.Hide();
            label3.Hide();

            Matrix M = new Matrix(dataGridView1.RowCount, dataGridView1.ColumnCount);
            for (int i = 0; i < M.RowCount; i++)
                for (int j = 0; j < M.ColCount; j++)
                    M[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            int tmp = Convert.ToInt32(firstRowNum.Value);
            dataGridView1.RowCount = tmp;
            dataGridView3.RowCount = tmp;
            if (tmp > M.RowCount)
                for (int i = 0; i < M.ColCount; i++)
                    for (int j = M.RowCount; j < tmp; j++)
                        dataGridView1.Rows[j].Cells[i].Value = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            label4.Show();
            dataGridView2.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = true;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox5.Checked = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox6.Checked = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            checkBox7.Checked = true;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            checkBox8.Checked = true;
            label4.Show();
            dataGridView2.Show();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Checked = true;
            label4.Show();
            dataGridView2.Show();
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            checkBox10.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label4.Show();
            dataGridView2.Show();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            label4.Show();
            dataGridView2.Show();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            label4.Show();
            dataGridView2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Matrix M = new Matrix(dataGridView1.RowCount, dataGridView1.ColumnCount);
            for (int i = 0; i < M.RowCount; i++)
                for (int j = 0; j < M.ColCount; j++)
                    M[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            FileStream fs = new FileStream("Данные.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            textBox1.Text = "";
            Vectors q = new Vectors(dataGridView2.ColumnCount);
            for (int i = 0; i < q.n; i++) q[i] = Convert.ToDouble(dataGridView2.Rows[0].Cells[i].Value);
            double a = Convert.ToDouble(textBox2.Text), b = Convert.ToDouble(textBox3.Text);

            Vectors v;
            if (checkBox1.Checked)
            {
                UnderUncertainty.AverageGain(M, out v,q);
                if (radioButton1.Checked) Fill(v);
            }
                if (checkBox2.Checked)
                {
                UnderUncertainty.MiniMax(M, out v);
                if (radioButton2.Checked) Fill(v);
            }
            if (checkBox3.Checked)
            {
                UnderUncertainty.MaxiMax(M, out v);
                if (radioButton3.Checked) Fill(v);
            }
            if (checkBox4.Checked)
            { UnderUncertainty.Laplas(M, out v);
                if (radioButton4.Checked) Fill(v);
            }
            if (checkBox5.Checked)
            { UnderUncertainty.Vald(M, out v);
                if (radioButton5.Checked) Fill(v);
            }
            if (checkBox6.Checked)
            { UnderUncertainty.Savage(M, out v);
                if (radioButton6.Checked) Fill(v);
            }
            if (checkBox7.Checked)
            { UnderUncertainty.Hurwitz(M, out v,a);
                if (radioButton7.Checked) Fill(v);
            }
            if (checkBox8.Checked)
            { UnderUncertainty.HodgeLeman(M, out v,b,q);
                if (radioButton8.Checked) Fill(v);
            }
            if (checkBox9.Checked)
            { UnderUncertainty.Germeier(M, out v,q);
                if (radioButton9.Checked) Fill(v);
            }
            if (checkBox10.Checked)
            { UnderUncertainty.Powers(M, out v);
                if (radioButton10.Checked) Fill(v);
            }

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            textBox1.Text = "";
            StreamReader sr = new StreamReader("Данные.txt");
            string ss = "";
            while (ss != null)
            {

                textBox1.Text += ss + Environment.NewLine;
                ss = sr.ReadLine();
            }
            sr.Close();

            label3.Show();
            dataGridView3.Show();
            textBox1.Show();
        }
        private void Fill(Vectors v)
        {
            for (int i = 0; i < v.n; i++) dataGridView3.Rows[i].Cells[0].Value = v[i];
        }
    }
}
