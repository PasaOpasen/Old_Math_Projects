using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы;
using JR.Utils.GUI.Forms;

namespace Распознавание_цифр
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Hides();

            sp = new SymbolSamples("t (vec).txt", "t (percent).txt", "t (char).txt");

            net = NeuroNet.FromFile("neuronet.txt");

            toolTip1.SetToolTip(button1, "Выбрать изображение для распознавания");
            toolTip1.SetToolTip(button2, "Распознать цифры с изображения");
            toolTip1.SetToolTip(button3, "Закрыть приложение");
            toolTip1.SetToolTip(listBox1, "Узнать, почему алгоритм выдал конкретно эти цифры");

            var t = new Timer();
            t.Interval = 250;
            t.Tick += new EventHandler(t_Tick);
            t.Start();

            toolStripLabel1.Text = "Программа готова к работе. Нажмите \"Распознать\" для распознавания исходного изображения или \"Выбрать изображение\" для выбора другого изображения";
        }
        int[] mas = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2 };
        int z = 0;
        void t_Tick(object sender, EventArgs e)
        {
            z++;
            z %= mas.Length;
            button3.BackColor = Color.FromArgb(mas[z] * 7, Color.Blue);
        }

        private NeuroNet net;


        private void Hides()
        {
            label1.Hide();
            label2.Hide();
            label3.Hide();
            listBox1.Hide();
        }
        private void Shows()
        {
            label1.Show();
            label2.Show();
            label3.Show();
            listBox1.Show();
        }

        SymbolSamples sp;

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Tuple<char, char,char, string[]>[] res;
        string[] resneu;

        private async void Method1()
        {
            neu = false;

            Tuple<  Vectors[],Vectors[]> mas = await Task.Run(() => Expend.GetVectors(new Bitmap(pictureBox1.BackgroundImage)));

            Program.form.toolStripLabel1.Text = "Вычисления окончены. На новом изображении показано, как программа зафиксировала цифры и в каком порядке";
            pictureBox1.BackgroundImage = Image.FromFile($"res{Expend.count}.png");

            label1.Text = "Результат: эйлеровы хар. ";label2.Text = "вероятности: ";label3.Text = "произведение: ";
            listBox1.Items.Clear();

            res = new Tuple<char,char,char, string[]>[mas.Item1.Length];
            for(int i = 0; i < res.Length; i++)
            {
                res[i] = sp.GetResult(mas.Item1[i],mas.Item2[i]);
                label1.Text += res[i].Item1;
                label2.Text += res[i].Item2;
                label3.Text += res[i].Item3;
                listBox1.Items.Add($"Результат {res[i].Item1}; {res[i].Item2}");
            }
            Shows();
        }
        private async void Method2()
        {
            neu = false;

            sp = new SymbolSamples("hau (vec).txt", "hau (percent).txt", "hau (char).txt");

            Tuple<Vectors[], Vectors[]> mas =await Task.Run(()=>  Expend.GetHauVectors(Expend.GetCurvePointSets(new Bitmap(pictureBox1.BackgroundImage)), 200, 5, 1e-3));

            Program.form.toolStripLabel1.Text = "Вычисления окончены. Жёлтым выделены найденные границы";
            pictureBox1.BackgroundImage = Image.FromFile($"border{Expend.count++}.png");

            label1.Text = "Результат: "; label2.Text = "прямые Хафа: "; label3.Text = "+учёт числа контуров: ";
            listBox1.Items.Clear();

            res = new Tuple<char, char, char, string[]>[mas.Item1.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = sp.GetResult(mas.Item1[i], mas.Item2[i]);
                //label1.Text += res[i].Item1;
                label2.Text += res[i].Item1;
                label3.Text += res[i].Item3;
                listBox1.Items.Add($"Результат {res[i].Item1}; {res[i].Item2}");
            }
Shows();
        }

        bool neu = false;
        private async void Method3()
        {
            int n = 10, m = 10;
            Bitmap b = new System.Drawing.Bitmap(pictureBox1.BackgroundImage);

            Program.form.toolStripLabel1.Text = "С изображения считываются пиксельные множества и производится коррекция качества";
            Vectors[] v =await Task.Run(()=> Expend.GetVectors(ref b, n, m));
            //Vectors.VectorsToFile(v, "vectors.txt");
            b.Save($"neuro{Expend.count}.png", System.Drawing.Imaging.ImageFormat.Png);
            pictureBox1.BackgroundImage = Image.FromFile($"neuro{Expend.count++}.png");

            label1.Text = "Результат: "; label2.Text = " "; label3.Text = "";
            listBox1.Items.Clear();

            Program.form.toolStripLabel1.Text = "Нейросеть выдаёт результаты";
            resneu =new string[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                Tuple<string, string> tmp = net.EndRes(v[i]);
                label1.Text += tmp.Item1;
                listBox1.Items.Add($"Результат = {tmp.Item1}");
                resneu[i]=$"{tmp.Item2}";
            }
            Shows();

            neu = true;
            Program.form.toolStripLabel1.Text = "Вычисления окончены. На новом изображении показано, что именно считывала нейросеть";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) Method1();
            if (radioButton2.Checked) Method2();
            if (radioButton3.Checked) Method3();

            button2.Hide();
        }

        private string ToString(string[] st)
        {
            string s = "";
            for(int i=0;i<st.Length;i++)
            {
                s += st[i];
                s += Environment.NewLine;
                    }
            return s;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if(neu)
                FlexibleMessageBox.Show(resneu[i], $"Вектор вероятностей (для набора 1234567890) у конкретно этой цифры:");
            else
            FlexibleMessageBox.Show(ToString(res[i].Item4),$"Разница полученных данных с образцами для результата [{res[i].Item1};{res[i].Item2}]");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripLabel1.Text = "Открыт диалог выбора тестового изображения";

            openFileDialog1.InitialDirectory = Environment.CurrentDirectory + @"\Samples";
            openFileDialog1.FileName ="sample.png";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;

            pictureBox1.BackgroundImage = Image.FromFile(filename);
            button2.Show();

            toolStripLabel1.Text = "Тестовое изображение задано. Теперь его можно распознавать";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void открытьФайлСправкиЧерезСтандартноеПриложениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("описание.pdf");
        }

        private void сохранитьТекущееИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = $"Рисунок ({DateTime.Now.ToString().Replace(':',' ')}).png";
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить рисунок как...";
            savedialog.FileName = name;
            savedialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";

            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.BackgroundImage.Save(savedialog.FileName);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Рисунок не сохранён", ee.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void показатьСправкуВОкнеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }
    }
}
