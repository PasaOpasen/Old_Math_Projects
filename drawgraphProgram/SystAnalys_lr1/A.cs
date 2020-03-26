using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.OpenFileDialog;
using МатКлассы;

namespace SystAnalys_lr1
{
    public partial class A : Form
    {
        public A()
        {
            InitializeComponent();
            dataGridView1.RowCount = 9;
            dataGridView1.ColumnCount = 8;
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Columns[i].Width = 40;
                dataGridView1.Rows[i].Cells[i].Value = 0;
                for (int j = i+1; j < 8; j++)
                {
                    int tmp = rand.Next() % 2;
                   dataGridView1.Rows[i].Cells[j].Value = tmp;
                    dataGridView1.Rows[j].Cells[i].Value = tmp;
                }     
            }
            dataGridView1.AllowUserToAddRows = false;
        }

        private void firstRowNum_ValueChanged(object sender, EventArgs e)
        {
            Matrix M = new Matrix(dataGridView1.RowCount, dataGridView1.ColumnCount);
            for (int i = 0; i < M.RowCount; i++)
                for (int j = 0; j < M.ColCount; j++)
                    M[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            int tmp = Convert.ToInt32(firstRowNum.Value);
            dataGridView1.ColumnCount = tmp;
            dataGridView1.RowCount = tmp;

            if (tmp > M.ColCount)
                for (int i = M.ColCount; i < tmp; i++)
                {
                    dataGridView1.Columns[i].Width = 40;
                    for (int j = 0; j < tmp; j++)
                    {
                       dataGridView1.Rows[j].Cells[i].Value = 0;
                       dataGridView1.Rows[i].Cells[j].Value = 0;
                    }
                        
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    dataGridView1.Rows[i].Cells[j].Value = 0;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[i].Value = 0;
                for (int j = i + 1; j < dataGridView1.ColumnCount; j++)
                {
                    int tmp = rand.Next() % 2;
                    dataGridView1.Rows[i].Cells[j].Value = tmp;
                    dataGridView1.Rows[j].Cells[i].Value = tmp;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Program.FORM.g == null) Program.FORM.g = new Graphs();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqMatrix M = new SqMatrix(dataGridView1.RowCount);
            //if (M == null) M = new SqMatrix();
            for (int i = 0; i < M.RowCount; i++)
                for (int j = 0; j < M.ColCount; j++)
                    M[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            
            Program.FORM.g = new Graphs(M);
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);


                SqMatrix M;
                try
                {
                      M = new SqMatrix(sr);
                }
                catch { M = new SqMatrix((int)firstRowNum.Value); }

                dataGridView1.ColumnCount = M.ColCount;
                dataGridView1.RowCount = M.ColCount;
                firstRowNum.Value = M.ColCount;
                for (int i = 0; i < M.ColCount; i++)
                {
                    dataGridView1.Columns[i].Width = 40;
                    for (int j = 0; j < M.ColCount; j++)
                        dataGridView1.Rows[i].Cells[j].Value = M[i,j];
                }

                //MessageBox.Show(sr.ReadToEnd());
                sr.Close();
            }
        }
    }
}
