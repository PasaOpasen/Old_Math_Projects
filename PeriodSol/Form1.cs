using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы;

namespace PeriodSol
{
    public partial class Form1 : Form
    {
       static double x0min,x0max,x1min,x1max,pr;
       static int k, count, astep, amin, amax;

        private async void button1_Click(object sender, EventArgs e)
        {
                        ReadGlobal();
            var st = new ConcurrentBag<string>();

            double h1 = (x0max - x0min) / (count - 1), h2 = (x1max - x1min) / (count - 1);
            toolStripStatusLabel1.Text = "Вычисления запущены";

            IProgress<int> progress = new Progress<int>((p) => { save = p; });
            all = ((amax - amin) / astep + 1) * count * count;
            bool[,,] mas = new bool[(amax - amin) / astep + 1, count, count];
            timer1.Start();

            await Task.Run(() => {
                int k = 0;
                for(int aa = amin; aa <= amax; aa += astep)
                {
                        Parallel.For(0, count, (int i) => { 
                   // for(int i=0;i<count;i++)
                    //{
                        for(int j = 0; j < count; j++)
                        {
                            int r = GetPeriod(x0min + i * h1, x1min + j * h2, aa);
                            if (r != 0)
                                st.Add($"{aa} \t{x0min + i * h1} \t{x1min + j * h2} \t{r}");
                            mas[k, i, j] = true;
                            progress.Report(Sum(mas));
                        }
                  //  }

                    });
                    k++;
                }          
            });
            timer1.Stop();

           using(StreamWriter fs=new StreamWriter("Результаты.txt"))
            {
                st.OrderBy((s)=> {
                    double[] d = s.ToDoubleMas();
                    Vectors v = new Vectors(d);
                    return v.GetHashCode();
                });
                fs.WriteLine("a \tx0 \tx1 \tperiod");
                for (int i = 0; i < st.Count; i++)
                    fs.WriteLine(st.ElementAt(i));
             }

            toolStripStatusLabel1.Text = "Вычисления окончены";
            toolStripProgressBar1.Value = 0;
            Process.Start("Результаты.txt");
        }

        private int Sum(bool[,,] b)
        {
            int sum = 0;
            for (int i = 0; i < b.GetLength(0); i++)
                for (int j = 0; j < b.GetLength(1); j++)
                    for (int k = 0;k < b.GetLength(2); k++)
                        if (b[i, j, k])
                            sum++;
            //sum.Show();
            return sum;
        }

        private void ReadGlobal()
        {
            amin = numericUpDown1.Value.ToInt32();
            amax = numericUpDown2.Value.ToInt32();
            astep = numericUpDown3.Value.ToInt32();
            count = numericUpDown4.Value.ToInt32();
            k = numericUpDown5.Value.ToInt32()*2+1;
            x0min = textBox1.Text.ToDouble();
            x0max = textBox2.Text.ToDouble();
            x1min = textBox3.Text.ToDouble();
            x1max = textBox4.Text.ToDouble();
            pr = textBox5.Text.ToDouble()*0.01;
        }

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Задай данные и нажми кнопку";

            timer1.Interval = 1500;
            timer1.Tick += new EventHandler(Timer1_Tick);

           // ReadGlobal();
            //Test();
        }
        private int all, save = 0;
        private void Timer1_Tick(object Sender, EventArgs e)
        {
            //$"{save.ToDouble() / all * toolStripProgressBar1.Maximum}".Show();
            toolStripProgressBar1.Value = (save.ToDouble() / all * toolStripProgressBar1.Maximum).ToInt();
        }

        Func<double, double, int, int> GetPeriod = (double x0, double x1, int a) =>
             {
                double[] mas = new double[k];
                 mas[0] =x0;
                 mas[1] = x1;

                 for (int i = 2; i < k; i++)
                 {
 mas[i] = (a + mas[i - 1]) / mas[i - 2];
                 }
                    

               //  Debug.WriteLine(mas);

                 return GetPeriod2(mas);
             };

        /// <summary>
        /// Найти период в массиве
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mas"></param>
        /// <returns></returns>
        public static int GetPeriod2(double[] mas)
        {
            bool f = true;
            for (int k = 1; k <= mas.Length / 2; k++)
            {
                f = true;
                for (int i = 0; i < k; i++)
                {
                    int s = 0;
                    while (i + (k) * (s + 1) < mas.Length)
                    {
                        if (!FEqv(mas[i + k * s],mas[i + (k) * (s + 1)]))
                        {
                            f = false;
                            goto z1;
                        }
                        s++;
                    }

                }
            z1:
                if (f) return k;
            }
            return 0;
        }

        private static bool FEqv(double d1,double d2)
        {
            return Math.Abs(d1 - d2) / Math.Max(d1, d2) <= pr;
        }


        private void Test()
        {
            //var p = GetPeriod(34, 34, 1);
            var p = GetPeriod(1, 2, 2);
        }
    }
}
