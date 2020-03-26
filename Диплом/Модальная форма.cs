using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using МатКлассы;
using static Курсач.KursMethods;
using static МатКлассы.FuncMethods.DefInteg;

namespace Курсач
{
    public partial class FormResult : Form
    {
        public FormResult()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.FORM.Close();
            Program.Form1.Close();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Процесс запущен" + Environment.NewLine;
            int k = Convert.ToInt32(Program.Form1.textBox7.Text);

            //BiharmonicEquation.Test();
            //Tables();

            if (Program.Form1.radioButton1.Checked)
            {
                int n = Int32.Parse(Program.Form1.textBox3.Text);
                int cu = Int32.Parse(Program.Form1.textBox1.Text);
                int gf = Int32.Parse(Program.Form1.textBox2.Text);

                //FonPotok(new ThreadStart(() => KursMethods.Desigion(n, gf, cu)));

                textBox1.Text += "Данные считаны" + Environment.NewLine;

                if (Program.Form1.checkBox1.Checked)
                {
                    KursMethods.Desigion(n, gf, cu);
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    /*await Task.Run(()=>*/
                    KursMethods.Illustrating()/*)*/;
                    Program.FORM.chart1.Refresh();
                }
                if (Program.Form1.checkBox2.Checked)
                {
                    KursMethods.Desigion(n, gf, cu);
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    /*await Task.Run(()=>*/
                    KursMethods.Fixity()/*)*/;
                    Program.FORM.chart1.Refresh();
                }

                if (Program.Form1.checkBox3.Checked)
                {
                    KursMethods.Desigion(n, gf, cu);
                    KursMethods.bstr = "";
                    KursMethods.sl = "";

                    /*await Task.Run(()=> */
                    KursMethods.Quality(n, k, gf, cu)/*)*/;
                    Program.FORM.chart1.Refresh();
                }
                if (Program.Form1.checkBox7.Checked)
                {
                    KursMethods.Desigion(n, gf, cu);
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    KursMethods.LnGraf(n, gf, cu);
                }

                if (Program.Form1.checkBox9.Checked)
                {
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    double[] Ls = Program.Form1.textBox12.Text.ToDoubleMas();

                    BigarmRes(n, gf, cu, Ls);
                }
            }
            if (Program.Form1.radioButton2.Checked)
            {
                int a = Int32.Parse(Program.Form1.textBox4.Text);
                int b = Int32.Parse(Program.Form1.textBox5.Text);
                int h = Int32.Parse(Program.Form1.textBox6.Text);

                textBox1.Text += "Данные считаны" + Environment.NewLine;

                if (Program.Form1.checkBox6.Checked) /*await Task.Run(() => */KursMethods.Pictures_ill(a, b, h)/*)*/;//графики приближения для 4-10 функций с шагом 3
                if (Program.Form1.checkBox5.Checked) /*await Task.Run(()=> */KursMethods.Pictures_fix(a, b, h)/*)*/;//картинки зависимости погрешности аппроксимации для от 30 до 40 функций, шаг 30
                if (Program.Form1.checkBox4.Checked) /*await Task.Run(()=> */KursMethods.Pictures_qua(k, a, b, h)/*)*/; //картинки зависимости погрешности от кривой для 20 функций с кривыми от 40 до 100 и шагом 20
                if (Program.Form1.checkBox8.Checked)
                {
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    for (int n = a; n <= b; n += h)
                    {
                        for (int cu = 1; cu < KursMethods.CountCircle; cu++)
                            for (int gf = 1; gf < KursMethods.KGF; gf++)
                            {
                                KursMethods.LnGraf(n, gf, cu);
                            }
                    }
                }
                if (Program.Form1.checkBox10.Checked)
                {
                    KursMethods.bstr = "";
                    KursMethods.sl = "";
                    double[] Ls = Program.Form1.textBox13.Text.ToDoubleMas();


                    for (int n = a; n <= b; n += h)
                    {
                        for (int cu = 1; cu <= KursMethods.CountCircle; cu++)
                            for (int gf = 1; gf < KursMethods.KGF-1; gf++)
                            {
                                BigarmRes(n, gf, cu, Ls);
                            }
                    }


                }
            }
        }

        private void FonPotok(ThreadStart e)
        {
            Thread a = new Thread(e);
            a.IsBackground = true;
            a.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            //KursMethods.Desigion(200,1,1);
            //KursMethods.Fixity(SLAUpok.Method.Gauss, SLAUpok.Method.GaussSpeedy);
            //chart1.SaveImage("iuf.png", System.Drawing.Imaging.ImageFormat.Png);
        }


        Color[] color = new Color[] {Color.Blue,Color.Red,Color.Green,Color.Black,Color.Gold,Color.Orange,Color.Aqua };

        private void BigarmRes(int n, int gr, int cu, double[] L)
        {
            z1:
            L = new double[10];var r = new  MathNet.Numerics.Random.CryptoRandomSource();
            for (int u = 0; u < 10; u++)
                L[u] =RandomNumbers.NextDouble2(MIN_RADIUS,MAX_RADIUS).ToString().Substring(0,4).ToDouble();
            L = L.Distinct().ToArray();
            //Array.Sort(L);

            Functional[] D3 = new Functional[4 + 1+1-1];

            chart1.Series.Clear();
            chart2.Series.Clear();
            IntegralClass.n = 128;
 //           FuncMethods.DefInteg.countY = 200;

            ForDesigion.Building(n, gr + KGF-1, cu, null, null); //чтение и работа с данными

            //KursMethods.CIRCLE = cu;
            //KursMethods.GF = gr;
            var t = BiharmonicEquation.LastMethod(n, GF, CIRCLE,maxmax:Int32.MaxValue);
            if (t.Item2.Contains(0)) goto z1;

            int countpoint = 120;
            double ep = (myCurveQ.b - myCurveQ.a) / countpoint;

            Program.FORM.chart2.Series.Add($"u = {FuncName[GF - 1]} на кривой {CircleName[CIRCLE - 1]}");
            Program.FORM.chart2.Series.Last().BorderWidth = 4;
            Program.FORM.chart2.Series.Last().Color = color[0];
            Program.FORM.chart2.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart2.Series.Last().BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            Program.FORM.chart2.Series.Last().Points.AddXY(myCurveQ.a, U(myCurveQ.Transfer(myCurve.a)));
            for (double i = myCurveQ.a + ep; i <= myCurveQ.b - ep; i += ep)
            {
                Program.FORM.chart2.Series.Last().Points.AddXY(i, U(myCurveQ.Transfer(i)));
            }
            D3[0] = U;

            Program.FORM.chart2.Series.Add($"Method 2");
            Program.FORM.chart2.Series.Last().BorderWidth = 3;
            Program.FORM.chart2.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart2.Series.Last().Points.AddXY(myCurveQ.a, t.Item4(myCurveQ.Transfer(myCurveQ.a)));
            for (double i = myCurveQ.a + ep; i <= myCurveQ.b - ep; i += ep)
            {
                Program.FORM.chart2.Series.Last().Points.AddXY(i, t.Item4(myCurveQ.Transfer(i)));
            }
            Program.FORM.chart2.Series.Last().Color = color[1];
            D3[1] = (МатКлассы.Point point) => t.Item4(point);

            Program.FORM.chart1.Series.Add($" "); Program.FORM.chart1.Series.Last().IsVisibleInLegend = false;
            Program.FORM.chart1.Series.Add($"Method 2");
            Program.FORM.chart1.Series.Last().BorderWidth = 3;
            Program.FORM.chart1.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart1.Series.Last().Points.AddXY(1, Math.Log10(t.Item2[0]));
            for (int i = 1; i < t.Item3.Length; i++)
            {
                Program.FORM.chart1.Series.Last().Points.AddXY(i + 1, Math.Log10(t.Item2[i]));
            }
            Program.FORM.chart1.Series.Last().Color = color[1];
            Program.FORM.chart1.Series.Last().MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
            Program.FORM.chart1.Series.Last().MarkerSize = 16;


            //метод 1

            StreamWriter mes = new StreamWriter(adress + "methods.txt");
            mes.WriteLine("Method 2");

            for (int k = 0; k < 3; k++)
            {
                LMIN_RADIUS = L[k];
                ForDesigion.CreateByCIRCLE();
                ForDesigion.Search(false, null, null); //поиск решения и вывод погрешности

                mes.WriteLine($"Method 1 (L = {LMIN_RADIUS})");
                Program.FORM.chart2.Series.Add($"Method 1 (L = {LMIN_RADIUS})");
                Program.FORM.chart2.Series.Last().BorderWidth = 3;
                Program.FORM.chart2.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart2.Series.Last().Points.AddXY(myCurveQ.a, M1Approx(myCurveQ.Transfer(myCurveQ.a)));
                for (double i = myCurveQ.a + ep; i <= myCurveQ.b - ep; i += ep)
                {
                    Program.FORM.chart2.Series.Last().Points.AddXY(i, M1Approx(myCurveQ.Transfer(i)));
                }
                Program.FORM.chart2.Series.Last().Color = color[2+k];

                if(2+k!=5)
                D3[2+k] = GetFunctional();

                Program.FORM.chart1.Series.Add($"Method 1 (L = {LMIN_RADIUS})");
                Program.FORM.chart1.Series.Last().BorderWidth = 3;
                Program.FORM.chart1.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart1.Series.Last().Points.AddXY(1, Math.Log10(MySLAU.ErrorsM1[0]));
                for (int i = 1; i < t.Item3.Length; i++)
                {
                    Program.FORM.chart1.Series.Last().Points.AddXY(i + 1, Math.Log10(MySLAU.ErrorsM1[i]));
                }
                Program.FORM.chart1.Series.Last().Color = color[2 + k];
                Program.FORM.chart1.Series.Last().MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                Program.FORM.chart1.Series.Last().MarkerSize = 16-2*k;
            }
            mes.Close();

            //// графики сумм m потенциальных функций
            //Program.FORM.chart1.Series[1].Name = $"Σcω n = {N}";
            //Program.FORM.chart2.Series[1].Name = $"Σcα n = {N}";
            //Program.FORM.textBox1.Text += $"Начинается рисование графика для области {CircleName[CIRCLE - 1]} и граничной функции {FuncName[GF - 1]}";
            //Program.FORM.textBox1.Text += Environment.NewLine;


            //    double[] l = new double[(int)Math.Ceiling((myCurve.b - myCurve.a) / ep + 1)], l2 = new double[l.Length];

            //    Parallel.For(0, l.Length, (int j) =>
            //    {
            //        l[j] = Approx(myCurve.Transfer(myCurve.a + ep * (j)));
            //        l2[j] = OldApprox(myCurve.Transfer(myCurve.a + ep * (j)));
            //    });
            //    int q = 0;
            //    for (double j = myCurve.a; j <= myCurve.b; j += ep)
            //    {
            //        Program.FORM.chart2.Series[1].Points.AddXY(j, l2[q]);
            //        Program.FORM.chart1.Series[1].Points.AddXY(j, l[q++]);
            //    }
            //    mas2.AddRange(l2);
            //    mas.AddRange(l);


            //double min = mas.Min(), max = mas.Max();
            //if (min == 0) min = -5 * ep;
            //if (max == 0) max = 5 * ep;
            //Program.FORM.chart1.ChartAreas[0].AxisY.Minimum = (min > 0) ? min * (1 - ep) : min * (1 + ep);
            //Program.FORM.chart1.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + ep) : max * (1 - ep);

            //min = mas2.Min(); max = mas2.Max();
            //if (min == 0) min = -5 * ep;
            //if (max == 0) max = 5 * ep;
            //Program.FORM.chart2.ChartAreas[0].AxisY.Minimum = (min > 0) ? min * (1 - ep) : min * (1 + ep);
            //Program.FORM.chart2.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + ep) : max * (1 - ep);


            //"График аппроксимации вычислен".Show();

            ////запись в файл bmp
            //string buf = new string(new char[150]);
            //string str, str2, buf2;
            ////memset(buf, 0, sizeof(sbyte));
            string d1 = Convert.ToString(GF);
            string d2 = Convert.ToString(CIRCLE);
            string d3 = Convert.ToString(N);
            //str = bstr + sl + "График 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";
            //str2 = bstr + sl + "График (плотности) 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";


            //buf = Convert.ToString(str);
            //buf2 = Convert.ToString(str2);
            //Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
            //Program.FORM.chart2.SaveImage(buf2, System.Drawing.Imaging.ImageFormat.Bmp);

            chart1.Titles.Last().Text = "Зависимость логарифма от аппроксимации от числа функций";
            chart2.Titles.Last().Text = "График функции u и её приближений на границе области";
            chart1.Titles.Last().Font = new Font(FontFamily.GenericSansSerif, 14);
            chart2.Titles.Last().Font = new Font(FontFamily.GenericSansSerif, 14);

            Библиотека_графики.ForChart.SetAxisesY(ref chart2);
            Program.FORM.chart1.SaveImage(adress + "Bigarm погреш. аппрокс. граничной функции (" + d1 + ") и её приближений при числе базисных точек до " + d3 + " на кривой (" + d2 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);
            Program.FORM.chart2.SaveImage(adress + "Bigarm граничной функции (" + d1 + ") и её приближений при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);


            "Заполнение файлов!!!!".Show();
            if (CIRCLE == 1)  МатКлассы.ForScripts.MakeFilesForSurfaces(-otrval, otrval, -otrval, otrval, 120, adress + "", D3, filt,true);
            else МатКлассы.ForScripts.MakeFilesForSurfaces(-MIN_RADIUS * 0.1, MIN_RADIUS * 1.1, -MIN_RADIUS * 0.1, MIN_RADIUS * 1.1, 100, adress + "", D3, filt,true);

            StreamWriter st = new StreamWriter(adress + "info.txt");
            st.WriteLine($"Circle = {CircleName[CIRCLE - 1]}, func = {FuncName[GF - 1]}, Qr = {MIN_RADIUS}, count = {d3}.pdf");
            st.WriteLine($"Circle = {CircleName[CIRCLE - 1]}, func = {FuncName[GF - 1]}, Qr = {MIN_RADIUS}, count = {d3} (Diff).html");
            st.Close();

            Process.Start(adress + "Approx3D2.R");

        }

        private Functional GetFunctional()
        {

            Vectors ds = new Vectors(MySLAU.x);

           return (МатКлассы.Point u) =>
            {
                Functional ro = (МатКлассы.Point y) =>
                {
                    double sss = 0;
                    for (int i = 0; i < masPoints.Length; i++) sss += ds[i] * KursMethods.masPoints[i].PotentialF(u);
                    return sss * KursMethods.Exy(u, y);
                };
                //return IntegralClass.Integral(ro, KursMethods.CIRCLE - 1);
                return  DoubleIntegral(ro, KursMethods.myCurveQ, KursMethods.myCurveQ.S, FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY)/*IntegralClass.Integral(ro, KursMethods.CIRCLE - 1)*/ /*- right(x)*/;
            };
        }


        private void Tables()
        {
            int ch = 7;
            string nr = "$n$=";

            for(GF=3;GF<=7;GF++)
            {
                int[] n = new int[] { 5, 10, 20, 30, 40 };
                using(StreamWriter fs=new StreamWriter($"GF = {GF}.txt"))
                {
                    fs.WriteLine(@"\begin{table}[h]\begin{center}  \begin{tabular}[t]{|c|l|c|c|c|c|c|}\hline");
                    fs.WriteLine($"\\multicolumn{"{"+2+"}"}{"{"+"|c|"+"}"}{"{"+"Область и метод"+"}"} & {nr+n[0]} & {nr + n[1]} & {nr + n[2]} & {nr + n[3]} & {nr + n[4]} \\\\ \\hline");

                    for (CIRCLE = 1; CIRCLE <= 3; CIRCLE++)
                    {
                        fs.WriteLine($"\\multirow{3}{"*"}{CIRCLE}");

                        double[] L = new double[10]; var r = new MathNet.Numerics.Random.CryptoRandomSource();
                        for (int u = 0; u < 10; u++)
                            L[u] = RandomNumbers.NextDouble2(MIN_RADIUS, MAX_RADIUS).ToString().Substring(0, 4).ToDouble();
                        L = L.Distinct().ToArray();

                        ForDesigion.Building(n.Max(), GF + KGF - 1, CIRCLE, null, null); //чтение и работа с данными

                        for (int k = 0; k < 2; k++)
                        {
                            
                            LMIN_RADIUS = L[k];
                            ForDesigion.CreateByCIRCLE();
                            ForDesigion.Search(false, null, null); //поиск решения и вывод погрешности
                            //$"!!!!!!!!!!!!!!!!!! Массив погрешностей: {new Vectors(MySLAU.ErrorsM1)}".Show();

                            fs.WriteLine($"& Метод 1 (L = {LMIN_RADIUS}) & {MySLAU.ErrorsM1[n[0]-1].ToString(ch)}  & {MySLAU.ErrorsM1[n[1] - 1].ToString(ch)}  & {MySLAU.ErrorsM1[n[2] - 1].ToString(ch)}  & {MySLAU.ErrorsM1[n[3] - 1].ToString(ch)} & {MySLAU.ErrorsM1[n[4] - 1].ToString(ch)}  \\\\ \\cline{"{"+"2 - 7"+"}"} ");
                        }

                        var t = BiharmonicEquation.LastMethod(n.Max(), GF, CIRCLE, maxmax: Int32.MaxValue);

                        fs.WriteLine($"& Метод 2     & {t.Item2[n[0] - 1].ToString(ch)}  & {t.Item2[n[1] - 1].ToString(ch)}  & {t.Item2[n[2] - 1].ToString(ch)}  & {t.Item2[n[3] - 1].ToString(ch)} & {t.Item2[n[4] - 1].ToString(ch)}\\\\ \\hline");

                    }

                    fs.WriteLine(@"\end{tabular}\end{center}\end{table}");
                }
            }
        }

    }
}
