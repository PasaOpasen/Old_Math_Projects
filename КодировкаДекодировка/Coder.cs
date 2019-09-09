using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static МатКлассы.Coding;
using МатКлассы;
using JR.Utils.GUI.Forms;

namespace КодировкаДекодировка
{
    public partial class Coder : Form
    {
        //int b2 = 0;
        string info = "";

        public Coder()
        {
            InitializeComponent();
            button2.Hide();
            numericUpDown1.Minimum = -abc.Length+1;
            numericUpDown1.Maximum = abc.Length-1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Show();
            textBox3.Text = "";
            if (radioButton1.Checked) textBox2.Text = Zesar(textBox1.Text, true,(int)numericUpDown1.Value);
            if (radioButton2.Checked) textBox2.Text = Simple(textBox1.Text, true);
            if(radioButton3.Checked) textBox2.Text = Polib(textBox1.Text, true);
            if (radioButton4.Checked) textBox2.Text = Playfer(textBox1.Text, true,textBox4.Text);
            if (radioButton5.Checked) textBox2.Text = Vert(textBox1.Text, true, textBox5.Text);
            if (radioButton6.Checked) textBox2.Text = Hoffman(textBox1.Text, true);
            if(radioButton7.Checked)
                textBox2.Text = VigenereEncode(textBox1.Text, Generate_Pseudorandom_KeyWord(Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value)));
            if (radioButton8.Checked) textBox2.Text = Vernam(textBox1.Text, true);

            info += Environment.NewLine;
            info += $"ИСХОДНЫЙ ТЕКСТ: {textBox1.Text}"+Environment.NewLine;
            info += $"КОДИРОВКА: {textBox2.Text}" + Environment.NewLine;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var fs = new System.IO.StreamWriter("Файл с информацией.txt");
            fs.Write(info);
            fs.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) textBox3.Text = Zesar(textBox2.Text, false, (int)numericUpDown1.Value);
            if (radioButton2.Checked) textBox3.Text = Simple(textBox2.Text, false);
            if (radioButton3.Checked) textBox3.Text = Polib(textBox2.Text, false);
            if (radioButton4.Checked) textBox3.Text = Playfer(textBox2.Text, false, textBox4.Text);
            if (radioButton5.Checked) textBox3.Text = Vert(textBox2.Text, false, textBox5.Text);
            if (radioButton6.Checked) textBox3.Text = Hoffman(textBox2.Text, false);
            if (radioButton7.Checked)
                textBox3.Text = VigenereDecode(textBox2.Text, Generate_Pseudorandom_KeyWord(Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value)));
            if (radioButton8.Checked) textBox3.Text = Vernam(textBox2.Text, false);

            info += Environment.NewLine;
            info += $"КОДИРОВКА: {textBox2.Text}" + Environment.NewLine;
            info += $"ДЕКОДИРОВКА: {textBox3.Text}" + Environment.NewLine;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) textBox3.Text = Zesar(textBox1.Text, false, (int)numericUpDown1.Value);
            if (radioButton2.Checked) textBox3.Text = Simple(textBox1.Text, false);
            if (radioButton3.Checked) textBox3.Text = Polib(textBox1.Text, false);
            if (radioButton4.Checked) textBox3.Text = Playfer(textBox1.Text, false, textBox4.Text);
            if (radioButton5.Checked) textBox3.Text = Vert(textBox1.Text, false, textBox5.Text);
            if (radioButton6.Checked) textBox3.Text = Hoffman(textBox1.Text, false);
            if (radioButton7.Checked) textBox3.Text = VigenereDecode(textBox1.Text, Generate_Pseudorandom_KeyWord(Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value)));
            if (radioButton8.Checked) textBox3.Text = Vernam(textBox1.Text, false);

            info += Environment.NewLine;
            info += $"ИСХОДНИК: {textBox1.Text}" + Environment.NewLine;
            info += $"ДЕКОДИРОВКА: {textBox3.Text}" + Environment.NewLine;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string tmp = "";
            for (int i = 0; i < TempList.Count; i++)
                tmp += TempList[i] + Environment.NewLine;
            //MessageBox.Show(tmp,"Окно промежуточной информации",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
            FlexibleMessageBox.Show(tmp, "Окно промежуточной информации");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            info += Environment.NewLine;
            info += $"ИСХОДНИК: {textBox1.Text}" + Environment.NewLine;
            string s = Hacking(textBox1.Text);
            info += $"ВЗЛОМ: {s}" + Environment.NewLine;
            FlexibleMessageBox.Show(s);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            info += Environment.NewLine;
            info += $"КОДИРОВКА: {textBox2.Text}" + Environment.NewLine;
            string s = Hacking(textBox2.Text);
            info += $"ВЗЛОМ: {s}" + Environment.NewLine;
            FlexibleMessageBox.Show(s);
            // FlexibleMessageBox.Show(Hacking(textBox2.Text));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName,Encoding.Default);
                
                string s = sr.ReadToEnd();
                textBox1.Text += s;
                
                sr.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            TempList = new List<string>();
        }
    }
}
