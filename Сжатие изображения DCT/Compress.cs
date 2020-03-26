using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Globalization;

namespace Сжатие_изображения_DCT
{
    public partial class Compress : Form
    {
        public Compress()
        {
            InitializeComponent();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(op.FileName);
                label1.Text = $"Сжимаемое изображение ({Сжатие.WeightBitmap(new Bitmap(pictureBox1.Image)).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
            }
                

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int k = (int)numericUpDown1.Value;
            Сжатие.Compression(new Bitmap(pictureBox1.Image), k);
            pictureBox2.Image = GlobalMembers.CompressBitMap;
            label2.Text = $"Результат (k = {k}, {Сжатие.WeightBitmap(GlobalMembers.CompressBitMap).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int k1 = (int)numericUpDown2.Value;
            int k2= (int)numericUpDown3.Value;
            for(int k=k1;k<=k2;k++)
            {
            Сжатие.Compression(new Bitmap(pictureBox1.Image), k);
            pictureBox2.Image = GlobalMembers.CompressBitMap;
            label2.Text = $"Результат (k = {k}, {Сжатие.WeightBitmap(GlobalMembers.CompressBitMap).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
                this.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(radioButton1.Checked)
                {
                    ImageToFileWithHoffmanAndBack.CompressionEncode(new Bitmap(pictureBox1.Image), op.FileName);
                    label2.Text = $"Переведено в файл {op.FileName} (в общем {ImageToFileWithHoffmanAndBack.LastFilesWeight1.ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} ({ImageToFileWithHoffmanAndBack.LastFilesWeight2.ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))}) байт)";
                }

                if(radioButton2.Checked)
                {
                int k = (int)numericUpDown1.Value;
                Сжатие.BitmapToFile(new Bitmap(pictureBox1.Image), op.FileName, k,radioButton5.Checked);
                label2.Text = $"Переведено в файл {op.FileName} ({Сжатие.FILEweight(op.FileName).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
                }

                if (radioButton3.Checked)
                {
                    int k = (int)numericUpDown1.Value;
                    Сжатие.SVDBitmapToFile(new Bitmap(pictureBox1.Image), op.FileName, radioButton5.Checked);
                    label2.Text = $"Переведено в файл {op.FileName} (в общем {(Сжатие.FILEweight(op.FileName)+ Сжатие.FILEweight("U.txt")+ Сжатие.FILEweight("VT.txt")).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(radioButton1.Checked)
                {
                    pictureBox2.Image = ImageToFileWithHoffmanAndBack.CompressionDecode(op.FileName);
                    label2.Text = $"Результат ({Сжатие.WeightBitmap(new Bitmap(pictureBox2.Image)).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
                }

                if(radioButton2.Checked)
                {
                pictureBox2.Image = Сжатие.FileToBitmap(op.FileName,radioButton5.Checked);
                label2.Text = $"Результат ({Сжатие.WeightBitmap(new Bitmap(pictureBox2.Image)).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
                }

                if (radioButton3.Checked)
                {
                    pictureBox2.Image = Сжатие.SVDFileToBitmap(op.FileName, radioButton5.Checked);
                    label2.Text = $"Результат ({Сжатие.WeightBitmap(new Bitmap(pictureBox2.Image)).ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} байт)";
                }
            }
        }

        void DCTHide()
        {
            button5.Hide();button6.Hide();
            label3.Hide();label4.Hide();label5.Hide();
            numericUpDown1.Hide();
            numericUpDown2.Hide();
            numericUpDown3.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DCTHide();
            groupBox2.Hide();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            DCTHide();
            groupBox2.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button5.Show(); button6.Show();
            label3.Show(); label4.Show(); label5.Show();
            numericUpDown1.Show();
            numericUpDown2.Show();
            numericUpDown3.Show();
            groupBox2.Show();
        }
    }
}
