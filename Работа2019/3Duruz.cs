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
using МатКлассы2018;
using МатКлассы2019;
using static Functions;

namespace Работа2019
{
    public partial class _3Duruz : Form
    {
        public _3Duruz()
        {
            InitializeComponent();
            timer1.Interval = 1500;
            timer1.Tick += new EventHandler(Timer1_Tick);

            label10.Hide();
            numericUpDown3.Hide();
        }
        private void Timer1_Tick(object Sender, EventArgs e)
        {
            if (save > 0)
            {
                Forms.UG.toolStripStatusLabel1.Text = $"Считаются значения функции u. Сохранено значений PMRSN: {prmsnmem.Lenght}. Осталось найти примерно {all - save} точек";
            }
            else
            {
                Forms.UG.toolStripStatusLabel1.Text = $"Сохранено значений PMRSN: {prmsnmem.Lenght}";
            }
            Forms.UG.IsManyPRMSN();

            Forms.UG.progressBar1.Value = (save.ToDouble() / all * Forms.UG.progressBar1.Maximum).ToInt();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int all = 0, save = 0;
        private DateTime time;

        private async void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            Waves.Circle circle = new Waves.Circle(new МатКлассы2018.Point(textBox1.Text.ToDouble(), textBox2.Text.ToDouble()), textBox3.Text.ToDouble());
            double w = textBox4.Text.ToDouble();
            double x0 = textBox5.Text.ToDouble(), X = textBox6.Text.ToDouble(), y0 = textBox7.Text.ToDouble(), Y = textBox8.Text.ToDouble();
            int xc = Convert.ToInt32(numericUpDown1.Value);
            int yc = Convert.ToInt32(numericUpDown2.Value);
            string tit = $"circle = (({circle.center.x}; {circle.center.y}), r = {circle.radius}), w = {w}, (xmin, xmax, xcount, ymin, ymax, ycount) = ({x0}, {X}, {xc}, {y0}, {Y}, {yc})";

            all = yc * xc;

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить рисунок как...";
            savedialog.FileName = Environment.CurrentDirectory + "\\3D ur, uz.txt";
            savedialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.ShowHelp = true;
            //if (savedialog.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {
            timer1.Start();
            IProgress<int> progress = new Progress<int>((p) => { save = p; });

            Forms.UG.toolStripStatusLabel1.Text = "Вычисления запущены";
            time = DateTime.Now;
            Forms.UG.stopshow();
            //Forms.UG.timer1.Start(); Forms.UG.timer2.Start();
            Forms.UG.source = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = Forms.UG.source.Token;
            await Task.Run(() =>
            {
                МатКлассы2019.Waves.Circle.FieldToFile(savedialog.FileName,
                    (double x, double y, МатКлассы2018.Point circlecenter, /*МатКлассы2018.Point[] normals*/МатКлассы2018.Point normal) =>
                    {

                        var t = Forms.UG.uj(x, y, 0, w, circlecenter, normal/*s*/);
                        return new Tuple<Number.Complex, Number.Complex>(Number.Complex.Sqrt(t[0].Sqr() + t[1].Sqr()), t[2]);
                    },
                    circle,
                    x0, X, xc, y0, Y, yc,
                    progress,//ref Forms.UG.prbar,
                    token,
                    tit,
                    Convert.ToInt32(numericUpDown3.Value)
                            );
            });
            //Forms.UG.timer1.Stop(); Forms.UG.timer2.Stop();
            TimeSpan ts = DateTime.Now - time;
            timer1.Stop();

            Forms.UG.stophide();

            if (token.IsCancellationRequested)
            {
                Forms.UG.toolStripStatusLabel1.Text = "Произошла отмена операции из 3D"; Forms.UG.progressBar1.Value = 0;
            }
            else
            {
                Forms.UG.toolStripStatusLabel1.Text = $"Вычисления выполнены и записаны в файл. Время вычислений: {ts}, среднее время вычилений 10-ти точек (sec.): {ts.Milliseconds.ToDouble() * 10 / xc / yc}. Вызван скрипт R";
                StartProcess("3Duruz.r");
            }

            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show("Произошла ошибка", ee.Message,
            //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //}          
        }

        public void StartProcess(string fileName)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.EnableRaisingEvents = true;

            process.Exited += (sender, e) =>
            {
                Console.WriteLine($"Процесс завершен с кодом {process.ExitCode}");
                Process.Start("3D ur, uz.pdf");
                Process.Start("urAbs.html");
                Forms.UG.toolStripStatusLabel1.Text = "Диалог с 3D закончен полностью";
                this.Close();
            };

            process.Start();
        }
    }
}
