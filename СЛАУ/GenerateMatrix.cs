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
    public partial class GenerateMatrix : Form
    {
        public GenerateMatrix()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            radioButton2.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public SLAU M;

        private void button2_Click(object sender, EventArgs e)
        {
            M = new SLAU(Convert.ToInt32(numericUpDown1.Value));

            if(radioButton1.Checked)
            {
                for(int i=0;i<M.Size;i++)
                    for(int j=0;j<M.Size;j++)
                    {
                        M.A[i, j] = 1.0 / (i + j + 1);
                        M.A[j, i] = M.A[i, j];
                    }
            }


            if(radioButton2.Checked)
            {
                for (int i = 0; i < M.Size; i++)
                    M.b[i] = 1;
            }
            if(radioButton3.Checked)
            {
                for (int i = 0; i < M.Size; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < M.Size; j++)
                        sum += M.A[i, j];
                   M.b[i] = sum;
                }
                    
            }


            StreamWriter sw = new StreamWriter("Последняя особая система.txt");
            Console.SetOut(sw);
            this.M.Show();
            sw.Close();

            Program.F.slau = new SLAU(this.M);
            this.Close();
            //button1_Click(sender, e);
        }
    }
}
