using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы;
using System.IO;

namespace Симплекс_метод
{
    public partial class SimplexTab : Form
    {
        public SimplexTab()
        {
            InitializeComponent();
            textBox1.Text = "19 2 3 1 0 0 0" + Environment.NewLine +
"13 2 1 0 1 0 0" + Environment.NewLine +
"15 0 3 0 0 1 0" + Environment.NewLine +
"18 3 0 0 0 0 1";
            textBox3.Text = "0 -7 -5 0 0 0 0";
            textBox4.Text = "0 0 1 1 1 1";
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPr Fo = new AboutPr();
            Fo.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.FORM.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string s = textBox4.Text;
            string[] st = s.Split(' ');
            int n = st.Length;
            Vectors p = new Vectors(n);
            for (int i = 0; i < n; i++) p[i] = Convert.ToDouble(st[i]);

            StreamWriter sf = new StreamWriter(@"Сгенерированная таблица.txt");
            for (int i = 0; i < textBox1.Lines.Count(); i++) sf.WriteLine(textBox1.Lines[i]);
            sf.WriteLine(textBox3.Lines[0]);//sf.WriteLine(p.ToRationalString());
            sf.Close();

            StreamReader fs = new StreamReader(@"Сгенерированная таблица.txt");
            StreamWriter nsf = new StreamWriter(@"Полное решение.txt");
            //Console.WriteLine(Vectors.SimplexInteger(ref p, fs, nsf));
            Vectors result = new Vectors(p);
            //Считать таблицу
            //чтение данных
            int k = 0;//число единиц
            for (int i = 0; i < n; i++) if (result[i] != 0) k++;
            Vectors[] lines = new Vectors[k + 1];
            for (int i = 0; i < k + 1; i++)
            {
                lines[i] = new Vectors(n + 1);
                s = fs.ReadLine();
                st = s.Split(' ');
                for (int j = 0; j <= n; j++) lines[i][j] = Convert.ToDouble(st[j]);
            }
            fs.Close();
            //Найти решение обычным симплекс-методом
            double end = Vectors.SimpleSimplex(ref result, ref lines, nsf);
            nsf.Close();

            //FileStream fss = new FileStream("Полное решение.txt", FileMode.Create);
            ////TextWriter tmp = Console.Out;
            //StreamWriter sw = new StreamWriter(fss);
            //Console.SetOut(sw);
            //Console.WriteLine(Vectors.SimplexInteger(ref p, fs, nsf));
            //sw.Close();
            //Console.SetOut(tmp);
            //Console.WriteLine("Запись завершена!");

            StreamReader sr = new StreamReader("Полное решение.txt");
            s = "";
            while (s != null)
            {

                textBox2.Text+=s+Environment.NewLine;
                s = sr.ReadLine();
            }
            sr.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string s = textBox4.Text;
            string[] st = s.Split(' ');
            int n = st.Length;
            Vectors p = new Vectors(n);
            for (int i = 0; i < n; i++) p[i] = Convert.ToDouble(st[i]);

            StreamWriter sf = new StreamWriter(@"Сгенерированная таблица.txt");
            for (int i = 0; i < textBox1.Lines.Count(); i++) sf.WriteLine(textBox1.Lines[i]);
            sf.WriteLine(textBox3.Lines[0]);//sf.WriteLine(p.ToRationalString());
            sf.Close();

            StreamReader fs = new StreamReader(@"Сгенерированная таблица.txt");
            StreamWriter nsf = new StreamWriter(@"Полное решение.txt");

            Console.WriteLine(Vectors.SimplexInteger(ref p, fs, nsf));
            
            StreamReader sr = new StreamReader("Полное решение.txt");
            s = "";
            while (s != null)
            {

                textBox2.Text += s + Environment.NewLine;
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void button4_Click(object sender, EventArgs e)
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
                    System.IO.File.WriteAllLines(s, textBox2.Lines);
                    
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранит файлйцтцйивйцивйц", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
