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
    public partial class ReverseMatrix : Form
    {
        public ReverseMatrix()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FileStream fs = new FileStream("Последняя обратная матрица.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            SqMatrix M= new SqMatrix(Program.F.SMatrix);
            "----------Исходная матрица (A):".Show();
            M.PrintMatrix(); Console.WriteLine(); Console.WriteLine();
            //M.PrintMatrix();
            if (radioButton1.Checked)
                M = Program.F.SMatrix.Reverse;
            if (radioButton2.Checked)
                M = Program.F.SMatrix.Invert();

            if (checkBox1.Checked)
                M = SqMatrix.ReverseCorrect(Program.F.SMatrix, M,Convert.ToDouble(textBox2.Text),Convert.ToInt32(numericUpDown1.Value),checkBox2.Checked);

            //textBox1.Text = "";
            "----------Обратная матрица (A.Reverse):".Show();
            M.PrintMatrix();Console.WriteLine();
            "----------Матрица А.Reverse*А:".Show();
            SqMatrix One =  Program.F.SMatrix*M;
            One.PrintMatrix(); Console.WriteLine();
            "----------Матрица E-А.Reverse*А:".Show();
            SqMatrix Zero = SqMatrix.E(One.RowCount)-One;
            Zero.PrintMatrix(); Console.WriteLine();
            "----------Для матрицы E-А.Reverse*А:".Show();
            "Кубическая норма:".Show();
            Zero.CubeNorm.Show();
            "Октаэдрическая норма:".Show();
            Zero.OctNorn.Show();
            "Максимальная абсолютная величина в матрице:".Show();
            Zero.MaxofMod.Show();




            sw.Close();
            Console.SetOut(tmp);
            //Console.WriteLine("Запись завершена!");

            textBox1.Text = "";
            StreamReader sr = new StreamReader("Последняя обратная матрица.txt");
            string ss = "";
            while (ss != null)
            {

                textBox1.Text += ss + Environment.NewLine;
                ss = sr.ReadLine();
            }
            sr.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked && !checkBox1.Checked) checkBox1.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked && !checkBox1.Checked) checkBox2.Checked = false;
        }
    }
}
