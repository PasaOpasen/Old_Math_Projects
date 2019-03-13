using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Решение_ОДУ.Дм.ПА
{
    public partial class Butcher : Form
    {
        public Butcher()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\4.PNG";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\5.PNG";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\6.PNG";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\7.PNG";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\8.PNG";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\9.PNG";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\10.PNG";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox4.ImageLocation = "таблицы Бутчера\\11.PNG";
        }
    }
}
