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

namespace СЛАУ
{
    public partial class SLAUform : Form
    {
        private int t1, t2;
        public SLAU slau;
        public SLAUform()
        {
            InitializeComponent();
            t1 = t2 = 0;
            Random rand = new Random();

            dataGridView1.RowCount = 5;
            dataGridView1.ColumnCount = 5;
            dataGridView2.RowCount = 5;
            dataGridView2.ColumnCount = 1;
            dataGridView3.ColumnCount = 1;
            firstRowNum.Value = 5;
            textBox2.Text = "0,001";

            dataGridView2.Columns[0].Width = 40;
            for (int i = 0; i < 5; i++)
            {
                dataGridView1.Columns[i].Width = 40;
                dataGridView2.Rows[i].Cells[0].Value = 1;
                for (int j = 0; j < 5; j++)
                    dataGridView1.Rows[i].Cells[j].Value = (i + j) % 7 + rand.Next(-i - 9, j);
            }

            textBox1.Hide();
            textBox2.Hide();
            numericUpDown1.Hide();
            label5.Hide();
            label6.Hide();
            dataGridView3.Hide();

            radioButton2.Checked = true;
            checkBox2.Checked = true;
            checkBox6.Checked = true;

            S = new SLAU();
            slau = new SLAU();
            SS = S;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void firstRowNum_ValueChanged(object sender, EventArgs e)
        {
            Random rand = new Random();
            int size = (int)firstRowNum.Value;
            dataGridView1.RowCount = size;
            dataGridView1.ColumnCount = size;
            dataGridView2.RowCount = size;
            for (int i = 0; i < size; i++)
            {
                dataGridView1.Columns[i].Width = 40;
                dataGridView2.Rows[i].Cells[0].Value = 1;
                for (int j = 0; j < size; j++)
                    dataGridView1.Rows[i].Cells[j].Value = (i + j) % 7 + rand.Next(-i - 9, j);
            }
            dataGridView3.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
dataGridView2.Rows[i].Cells[0].Value = 0;
                dataGridView3.Rows[i].Cells[0].Value = 0;
            }
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                for (int j = 0; j < dataGridView2.RowCount; j++)
                    dataGridView1.Rows[i].Cells[j].Value = 0;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            t1++;
            if (t1 % 2 == 1 || t2 % 2 == 1)
            {
                textBox2.Show();
                numericUpDown1.Show();
                label5.Show();
                label6.Show();
            }
            else
            {
                textBox2.Hide();
                numericUpDown1.Hide();
                label5.Hide();
                label6.Hide();
            }
            t1 = t1 % 2;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            t2++;
            if (t1 % 2 == 1 || t2 % 2 == 1)
            {
                textBox2.Show();
                numericUpDown1.Show();
                label5.Show();
                label6.Show();
            }
            else
            {
                textBox2.Hide();
                numericUpDown1.Hide();
                label5.Hide();
                label6.Hide();
            }
            t2 = t2 % 2;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox5.Checked = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int size = (int)dataGridView2.RowCount;
            for (int i = 0; i < size; i++)
                for (int j = i + 1; j < size; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = 0;
                    dataGridView1.Rows[j].Cells[i].Value = 0;
                }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int size = (int)dataGridView2.RowCount;
            for (int i = 0; i < size; i++)
                for (int j = i + 2; j < size; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = 0;
                    dataGridView1.Rows[j].Cells[i].Value = 0;
                }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int size = (int)dataGridView2.RowCount;
            for (int i = 0; i < size; i++)
            {
                dataGridView1.Rows[i].Cells[i].Value = rand.Next(-i - 9, 10);
                for (int j = i + 1; j < size; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = (i + j) % 7 + rand.Next(-i - 9, j);
                    dataGridView1.Rows[j].Cells[i].Value = dataGridView1.Rows[i].Cells[j].Value;
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
        }

        private SLAU S,SS;
        public void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                GiveMatrixFromFixFile(sr);
                
                //MessageBox.Show(sr.ReadToEnd());
                sr.Close();
            }
        }

        public void GiveMatrixFromFixFile(System.IO.StreamReader filename)
        {
                SLAU M;

                try
                {
                    M = new SLAU(filename);
                }
                catch (Exception es)
                {
                    M = new SLAU((int)firstRowNum.Value);
                    MessageBox.Show(es.Message + " В программу входит нулевая система указанной размерности.");
                }
                GiveMatrixFromForm(M);
        }
        public void GiveMatrixFromForm(SLAU M)
        {
                dataGridView1.ColumnCount = M.Size;
                dataGridView1.RowCount = M.Size;
                dataGridView2.RowCount = M.Size;
                dataGridView3.RowCount = M.Size;
                firstRowNum.Value = M.Size;
                for (int i = 0; i < M.Size; i++)
                {
                    dataGridView1.Columns[i].Width = 40;
                    dataGridView2.Rows[i].Cells[0].Value = M.b[i];
                    dataGridView3.Rows[i].Cells[0].Value = M.x[i];
                    for (int j = 0; j < M.Size; j++)
                        dataGridView1.Rows[i].Cells[j].Value = M.A[i, j];
                }
                S = new SLAU(M);
                dataGridView3.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            (new GenerateMatrix()).ShowDialog();
            GiveMatrixFromForm(slau);

            //string s = "Особая система.txt";
            //using (var sr=new System.IO.StreamReader(s))
            //    GiveMatrixFromFixFile(sr);

        }

        private void SLAUform_Load(object sender, EventArgs e)
        {

        }


        public SqMatrix SMatrix;
        private void button11_Click(object sender, EventArgs e)
        {
            SqMatrix M = new SqMatrix(Convert.ToInt32(firstRowNum.Value));
            for (int i = 0; i < M.ColCount; i++)
                for (int j = 0; j < M.ColCount; j++)
                    M[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
            this.SMatrix = M;
            //this.SMatrix.PrintMatrix();
            if (M.Det == 0)
                MessageBox.Show("Определитель матрицы системы равен 0!");
            else (new ReverseMatrix()).ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                System.IO.StreamReader sr = new System.IO.StreamReader(filename);

                SqMatrix M;
                try
                {
                    M = new SqMatrix(sr);
                }
                catch (Exception es)
                {
                    M = new SqMatrix((int)firstRowNum.Value);
                    MessageBox.Show(es.Message /*+ " В программу входит нулевая система указанной размерности."*/);
                }
                dataGridView1.ColumnCount = M.RowCount;
                dataGridView1.RowCount = M.RowCount;
                dataGridView2.RowCount = M.RowCount;
                dataGridView3.RowCount = M.RowCount;
                firstRowNum.Value = M.RowCount;
                for (int i = 0; i < M.RowCount; i++)
                {
                    dataGridView1.Columns[i].Width = 40;
                    for (int j = 0; j < M.RowCount; j++)
                        dataGridView1.Rows[i].Cells[j].Value = M[i, j];
                }
                SMatrix = new SqMatrix(M);

                //MessageBox.Show(sr.ReadToEnd());
                sr.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string s = savedialog.FileName;

                    //StreamWriter r = new StreamWriter(s);
                    //StreamReader sr = new StreamReader("Полное решение.txt");
                    //while (s != null)
                    //{
                    //    s = sr.ReadLine();
                    //    r.WriteLine(s);
                    //}
                    //sr.Close();
                    //r.Close();
                    System.IO.File.WriteAllLines(s, textBox1.Lines);
                    s = s.Substring(0, s.Length - 4);
                    s = s + " (СЛАУ).txt";

                    FileStream fs = new FileStream(s, FileMode.Create);
                    TextWriter tmp = Console.Out;
                    StreamWriter sw = new StreamWriter(fs);
                    Console.SetOut(sw);
                    this.SS.Show();
                    sw.Close();
                    //fs.Close();
                    

                }
                catch(Exception ee)
                {
                    MessageBox.Show("Невозможно сохранить файл "+ee.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Show();

            int size = (int)dataGridView2.RowCount;
            if (size != firstRowNum.Value) size--;
           
            S = new SLAU(size);
            for (int i = 0; i < size; i++)
            {
                S.b[i] = Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value);
                for (int j = 0; j < size; j++)
                    S.A[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
            }
            dataGridView3.RowCount = size;

            FileStream fs = new FileStream("Данные о невязках.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            //S.Show();
            SqMatrix r = new SqMatrix(S.A);
            double d = r.Det;
            if (d == 0)
            {
                Console.WriteLine("Определитель матрицы системы равен 0!");
                sw.Close();
                Console.SetOut(tmp);
                //Console.WriteLine("Запись завершена!");

                textBox1.Text = "";
                StreamReader oo = new StreamReader("Данные о невязках.txt");
                string o = "";
                while (o != null)
                {

                    textBox1.Text += o + Environment.NewLine;
                    o = oo.ReadLine();
                }
                oo.Close();
                return;
            }
            SqMatrix M = new SqMatrix(S.A);

            if (checkBox1.Checked)
            {
                S.Gauss();
                Console.WriteLine("Погрешность быстрого метода Гаусса = {0}", S.Discrep);
                if (radioButton1.Checked)
                {
                    for (int i = 0; i < size; i++) dataGridView3.Rows[i].Cells[0].Value = S.x[i];
                    SS = new SLAU(S);
                    dataGridView3.Show();
                }
            }
            if (checkBox2.Checked)
            {
                S.GaussSelection();
                Console.WriteLine("Погрешность надёжного метода Гаусса = {0}", S.Discrep);
                if (radioButton2.Checked)
                {
                    for (int i = 0; i < size; i++) dataGridView3.Rows[i].Cells[0].Value = S.x[i];
                    dataGridView3.Show(); SS = new SLAU(S);
                }
            }

            if (checkBox3.Checked)
            {

                bool y = false;
                if (M.IsPositCertain() && M.IsSymmetric())
                {
                    S.Holets(size);
                    Console.WriteLine("Погрешность метода Холетского = {0}", S.Discrep);
                    y = true;
                }
                else Console.WriteLine("Матрица не является симметрической положительно определённой, поэтому методом Холецкого систему решить нельзя!");
                if (M.IsTreeDiag())
                {
                    S.ProRace();
                    Console.WriteLine("Погрешность метода прогонки = {0}", S.Discrep);
                    y = true;
                }
                else Console.WriteLine("Матрица не является тридиагональной, поэтому методом прогонки систему решить нельзя!");

                if (radioButton3.Checked && y)
                {
                    for (int i = 0; i < size; i++) dataGridView3.Rows[i].Cells[0].Value = S.x[i];
                    dataGridView3.Show(); SS = new SLAU(S);
                }
            }


            double eps = Convert.ToDouble(textBox2.Text);
            int k = (int)numericUpDown1.Value;

            for (int i = 0; i < size; i++) S.x[i] = 0;

            if (checkBox4.Checked)
            {
                SqMatrix D = new SqMatrix(size), T = new SqMatrix(S.A);
                for (int i = 0; i < size; i++) D[i, i] = S.A[i, i];
                if (D.Det != 0)
                {
                    SqMatrix B = SqMatrix.I(size) - D.Reverse * (T);

                    if (B.Frobenius < 1)
                    {
                        S.Jak(size, eps, k);
                        Console.WriteLine("Погрешность метода Якоби = {0}", S.Discrep);
                        if (radioButton4.Checked)
                        {
                            for (int i = 0; i < size; i++) dataGridView3.Rows[i].Cells[0].Value = S.x[i];
                            dataGridView3.Show(); SS = new SLAU(S);
                        }
                    }
                    else Console.WriteLine("Норма (Фробениуса) матрицы I-D.Reverse*A равна {0}>=1, поэтому метод простой итерации расходится!", B.Frobenius);
                }
                else Console.WriteLine("Определитель матрицы D=0, поэтому использовать метод простой итерации невозможно!");
            }

            for (int i = 0; i < size; i++) S.x[i] = 0;
            if (checkBox5.Checked)
            {
                if (M.IsPositCertain() && M.IsSymmetric())
                {
                    S.Speedy(size, eps, k);
                Console.WriteLine("Погрешность метода наискорейшего спуска = {0}", S.Discrep);
                if (radioButton5.Checked)
                {
                    for (int i = 0; i < size; i++) dataGridView3.Rows[i].Cells[0].Value = S.x[i];
                    dataGridView3.Show(); SS = new SLAU(S);
                }
                }
                else Console.WriteLine("Матрица не является симметрической положительно определённой, поэтому методом наискорейшего спуска систему решить нельзя!");
            }
            for (int i = 0; i < size; i++) S.x[i] = 0;
            if (checkBox6.Checked)
            {
                if (M.IsPositCertain() && M.IsSymmetric())
                {
                    S.Minimize_coef(size, k);
                    Console.WriteLine("Погрешность метода покоординатной минимизации ({0}-минимизация) = {1}", k, S.Discrep);
                    if (radioButton6.Checked)
                    {
                        for (int i = 0; i < size; i++) dataGridView3.Rows[i].Cells[0].Value = S.x[i];
                        dataGridView3.Show(); SS = new SLAU(S);
                    }
                }
                else Console.WriteLine("Матрица не является симметрической положительно определённой, поэтому методом покоординатной минимизации систему решить нельзя!");
            }

            sw.Close();
            Console.SetOut(tmp);
            //Console.WriteLine("Запись завершена!");

            textBox1.Text = "";
            StreamReader sr = new StreamReader("Данные о невязках.txt");
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
