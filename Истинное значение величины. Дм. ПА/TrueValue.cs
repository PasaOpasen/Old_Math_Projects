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
using МатКлассы2018;

namespace Истинное_значение_величины.Дм.ПА
{
    public partial class TrueValue : Form
    {
        private int oldsize=0;
        public TrueValue()
        {
            InitializeComponent();
            dataGridView1.Hide();
            dataGridView2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
            textBox1.Hide();
            textBox2.Hide();

            dataGridView1.RowCount = 5;
            dataGridView2.RowCount = 5;
            dataGridView3.RowCount = 5;
            dataGridView1.ColumnCount = 1;
            dataGridView2.ColumnCount = 1;
            dataGridView3.ColumnCount = 1;

            dataGridView1.Columns[0].Width = 100;
            dataGridView2.Columns[0].Width = 100;
            dataGridView3.Columns[0].Width = 120;

            for (int i = 0; i < 5; i++)
            {
                dataGridView3.Rows[i].Cells[0].Value = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void firstRowNum_ValueChanged(object sender, EventArgs e)
        {
            int size = (int)firstRowNum.Value;
            dataGridView3.RowCount = size;
            //if(size>oldsize)
            for (int i = 0; i < size; i++)
                dataGridView3.Rows[i].Cells[0].Value = 0;
            
            //oldsize = size;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = dataGridView3.RowCount;
            if (firstRowNum.Value != size) size--;

            dataGridView1.Show(); dataGridView2.RowCount = size;
            dataGridView2.Show(); dataGridView1.RowCount = size;
            label3.Show();
            label4.Show();
            label5.Show();
            textBox1.Show();
            textBox2.Show();
            textBox2.Text = "";

            Vectors v = new Vectors(size);
            for (int i = 0; i < size; i++) v[i] = Convert.ToDouble(dataGridView3.Rows[i].Cells[0].Value);

            Vectors v1 = v.RelAcVec, v2 = v.RelAcSqr;
            for (int i = 0; i < size; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = v1[i];
                dataGridView2.Rows[i].Cells[0].Value = v2[i];
            }
            textBox2.Text = v.ArithmeticAv + " +/- " + v.Average;

                FileStream fs = new FileStream("Данные.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            v.TrueValShowFull();

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
        }
    }
}
