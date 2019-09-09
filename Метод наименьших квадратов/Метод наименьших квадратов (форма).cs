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
using МатКлассы;
using Point = МатКлассы.Point;

namespace Метод_наименьших_квадратов
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            checkBox1.Checked = true;
            checkBox6.Checked = true;
            textBox1.Text = "0"; textBox2.Text = "5";
            textBox3.Text = "0 1 2 3";
            textBox4.Text = "4";
            textBox5.Text = "0 1" + Environment.NewLine + "1 3" + Environment.NewLine + "2 4" + Environment.NewLine + "5 -5";
            textBox6.Text = "2";
            radioButton4.Checked = true;

            label2.Hide(); textBox3.Hide();
            label3.Hide(); textBox4.Hide();
            label4.Hide(); textBox5.Hide();

            checkBox3.Hide();
            checkBox4.Hide();
            checkBox5.Hide();

            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].IsVisibleInLegend = false;

            label7.Hide(); label8.Hide();
            numericUpDown1.Hide();
            numericUpDown2.Hide();
        }

        //private static void ShowPoints(FuncMethods.NetFunc net)
        //{
        //    Point[] p = net.Points;
        //    Draw_mas(p, Color.Blue);
        //}
        private static void Draw_mas(Point[] r, Color color) //нарисовать массив точек
        {

            //SetColor(R, G, B); // задаем цвет линии
            //if(R==0) 
            //Program.FORM.chart1.Series[0].Color = Color.Green;
            FuncMethods.NetFunc u = new FuncMethods.NetFunc(r);

            int N = r.Length;
            double e = (u.MaxArg - u.MinArg) / 200;
            for (int i = 0; i < N; i++)
            {
                Program.FORM.chart1.Series.Add("");
                int count = Program.FORM.chart1.Series.Count - 1;
                Program.FORM.chart1.Series[count].Color = color;

                Program.FORM.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart1.Series[count].IsVisibleInLegend = false;
                Program.FORM.chart1.Series[count].BorderWidth = 3;
                Program.FORM.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y - e); // устанавливаем курсор на точку
                Program.FORM.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y + e); //рисуем line
                //-----------------------------------------------------------------
                Program.FORM.chart1.Series.Add("");
                count = Program.FORM.chart1.Series.Count - 1;
                Program.FORM.chart1.Series[count].Color = color;

                Program.FORM.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart1.Series[count].IsVisibleInLegend = false;
                Program.FORM.chart1.Series[count].BorderWidth = 3;
                Program.FORM.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y + e); // устанавливаем курсор на точку
                Program.FORM.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y - e); //рисуем line
            }
            //Program.FORM.chart1.Series[0].Color = Color.Red;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Program.FORM.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show(); textBox1.Show(); textBox2.Show();
            label2.Hide(); textBox3.Hide();
            label3.Hide(); textBox4.Hide();
            label4.Hide(); textBox5.Hide();
            groupBox3.Show();

            //checkBox3.Show();
            //checkBox4.Show();
            //checkBox5.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Hide(); textBox1.Hide(); textBox2.Hide();
            label2.Show(); textBox3.Show();
            label3.Hide(); textBox4.Hide();
            label4.Hide(); textBox5.Hide();
            groupBox3.Show();
            checkBox3.Hide();
            checkBox4.Hide();
            checkBox5.Hide();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label1.Hide(); textBox1.Hide(); textBox2.Hide();
            label2.Hide(); textBox3.Hide();
            label3.Show(); textBox4.Show();
            label4.Show(); textBox5.Show();
            groupBox3.Hide();

            checkBox3.Hide();
            checkBox4.Hide();
            checkBox5.Hide();
        }

        private void Analys(double[,] m, int beg, int end)
        {
            Vectors[] mas = new Vectors[m.GetLength(0)];
            Matrix M = new Matrix(m);
            for (int i = 0; i < mas.Length; i++)
                mas[i] = M.GetLine(i, beg, end);
            if (checkBox1.Checked)
            {
                Console.WriteLine("--------Для системы мономов---------");
                $"Наилучшая аппроксимация равна {mas[1].Min} при числе функций {beg + Array.IndexOf(mas[1].DoubleMas, mas[1].Min)}".Show();
            }
            if (checkBox2.Checked)
            {
                Console.WriteLine("--------Для системы полиномов Лежандра---------");
                $"Наилучшая аппроксимация равна {mas[2].Min} при числе функций {beg + Array.IndexOf(mas[2].DoubleMas, mas[2].Min)}".Show();
            }
            if (checkBox6.Checked)
            {
                Console.WriteLine("--------Для ортонормированной системы тригонометрических полиномов---------");
                $"Наилучшая аппроксимация равна {mas[6].Min} при числе функций {beg + Array.IndexOf(mas[6].DoubleMas, mas[6].Min)}".Show();
            }
            if (checkBox7.Checked)
            {
                Console.WriteLine("--------Для системы Хаара---------");
                $"Наилучшая аппроксимация равна {mas[7].Min} при числе функций {beg + Array.IndexOf(mas[7].DoubleMas, mas[7].Min)}".Show();
            }
            if (checkBox8.Checked)
            {
                Console.WriteLine("--------Для системы экспонент---------");
                $"Наилучшая аппроксимация равна {mas[8].Min} при числе функций {beg + Array.IndexOf(mas[8].DoubleMas, mas[8].Min)}".Show();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++) Program.FORM.chart1.Series[i].Points.Clear();
            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].IsVisibleInLegend = false;
            for (int i = 9; i < Program.FORM.chart1.Series.Count; i++) { Program.FORM.chart1.Series.RemoveAt(i); i--; }

            RealFunc f = Math.Sin;
            FuncMethods.NetFunc net = new FuncMethods.NetFunc();
            RealFunc g1, g2, g3, g4, g5, g6, g7;
            //SequenceFunc Monom, Leg, Cheb, Lager, Her, Trig, Har;
            g1 = g2 = g3 = g4 = g5 = g6 = g7 = null;

            //определить действительную функцию
            if (radioButton5.Checked) f = (double x) => (Math.Cos(x) - 1);
            if (radioButton6.Checked) f = (double x) => (Math.Abs(x - 1));
            if (radioButton7.Checked) f = (double x) => (Math.Exp(x - x * x));
            if (radioButton8.Checked) f = (double x) => (Math.Exp(x) / (Math.Abs(x) + 1));
            if (radioButton9.Checked) f = (double x) => (x * Math.Sin(x) / (Math.Abs(x) + 2));
            if (radioButton10.Checked) f = (double x) => (x / (1 + x * x));
            if (radioButton11.Checked) f = (double x) => (x * Math.Cos(x));
            if (radioButton12.Checked) f = (double x) => (Math.Log(1 + Math.Abs(x)));
            if (radioButton13.Checked) f = (double x) => (x * x * x + x - 4);
            if (radioButton14.Checked) f = (double x) => (3);
            if (radioButton15.Checked) f = (double x) => (2 * x - Math.Exp(-x));
            if (radioButton16.Checked)
            {
                string s = textBox7.Text;
                try
                {
                    f = Parser.GetDelegate(s);
                    textBox7.Text = Parser.FORMULA;
                }
                catch
                {
                    f = (double x) => 0;
                }
            }
            chart1.Series[0].IsVisibleInLegend = true;

            FileStream fs = new FileStream("Данные об аппроксимации.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            const string message = "Все доступные поля должны быть заполнены действительными числами. Число шагов и число отрезков - натуральными числами. При записи действительных чисел используются запятые, а не точки. Количество заверенных точек должно быть не больше количества действительно заданных точек.";
            const string caption = "Неверные входные данные!";
            try
            {
                int n1 = Convert.ToInt32(numericUpDown1.Value), n2 = Convert.ToInt32(numericUpDown2.Value);
                double[,] MAS = new double[chart1.Series.Count, n2 + 1];

                //определить сеточную функцию
                if (!radioButton1.Checked)
                {
                    double a = Convert.ToDouble(textBox1.Text), b = Convert.ToDouble(textBox2.Text);
                    int n = Convert.ToInt16(textBox6.Text),ccount=Convert.ToInt32(numericUpDown3.Value);
                    double h = (b - a)/(ccount-1);
                    Point[] mas1, mas2,mas3=new Point[ccount];

                    for (int i = 0; i < ccount; i++)
                        mas3[i] = new Point(a+i*h,f(a+i*h));

                    //чтение набора узлов
                    int m = Convert.ToInt32(textBox4.Text);
                    mas2 = new МатКлассы.Point[m];
                    string s;
                    string[] st;
                    for (int k = 0; k < m; k++)
                    {
                        s = textBox5.Lines[k];
                        st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа
                        mas2[k] = new МатКлассы.Point(Convert.ToDouble(st[0]), Convert.ToDouble(st[1]));
                    }
                    //чтение массива абцисс
                    s = textBox3.Lines[0];
                    st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа;
                    m = st.Length;
                    mas1 = new МатКлассы.Point[m];
                    for (int k = 0; k < m; k++)
                    {
                        double v = Convert.ToDouble(st[k]);
                        mas1[k] = new МатКлассы.Point(v, f(v));
                    }

                    if (radioButton2.Checked) net = new FuncMethods.NetFunc(mas1);
                    else net = new FuncMethods.NetFunc(mas2);
                    if (checkBox10.Checked) net = new FuncMethods.NetFunc(mas3);

                    a = net.MinArg;
                    b = net.MaxArg;

                    if (checkBox9.Checked)
                        for (n = n1; n <= n2; n++)
                        {
                            Matrix O = new Matrix(MAS);
                            O.PrintMatrix(); "".Show();

                            if (checkBox1.Checked)
                            {
                                SequenceFunc t = FuncMethods.Monoms;
                                RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Other, n);
                                if (radioButton2.Checked)
                                    MAS[1, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                else
                                    MAS[1, n] = FuncMethods.NetFunc.Distance(net, g);
                                chart1.Series[1].Points.AddXY(n, Math.Log10(MAS[1, n])); chart1.Series[1].IsVisibleInLegend = true;
                            }

                            if (checkBox2.Checked)
                            {
                                SequenceFunc t = FuncMethods.Lezhandrs(a, b);
                                RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Orthogonal, n);
                                if (radioButton2.Checked)
                                    MAS[2, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                else
                                    MAS[2, n] = FuncMethods.NetFunc.Distance(net, g);
                                chart1.Series[2].Points.AddXY(n, Math.Log10(MAS[2, n])); chart1.Series[2].IsVisibleInLegend = true;
                            }

                            if (checkBox6.Checked)
                            {
                                SequenceFunc t = FuncMethods.TrigSystem(a, b);
                                RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Orthonormal, n);
                                if (radioButton2.Checked)
                                    MAS[6, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                else
                                    MAS[6, n] = FuncMethods.NetFunc.Distance(net, g);
                                chart1.Series[6].Points.AddXY(n, Math.Log10(MAS[6, n])); chart1.Series[6].IsVisibleInLegend = true;
                            }
                            if (checkBox7.Checked)
                            {
                                SequenceFunc t = FuncMethods.HaarSystem(a, b);
                                RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Orthogonal, n);
                                if (radioButton2.Checked)
                                    MAS[7, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                else
                                    MAS[7, n] = FuncMethods.NetFunc.Distance(net, g);
                                chart1.Series[7].Points.AddXY(n, Math.Log10(MAS[7, n])); chart1.Series[7].IsVisibleInLegend = true;
                            }
                            if (checkBox8.Checked)
                            {
                                SequenceFunc t = (double x, int k) => Math.Exp(k * x);
                                RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Other, n);
                                if (radioButton2.Checked)
                                    MAS[8, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                else
                                    MAS[8, n] = FuncMethods.NetFunc.Distance(net, g);
                                chart1.Series[8].Points.AddXY(n, Math.Log10(MAS[8, n])); chart1.Series[8].IsVisibleInLegend = true;
                            }
                        }
                    else//если рисуется не график зависимости от числа функций
                    {
                        if (radioButton2.Checked) { net = new FuncMethods.NetFunc(mas1); Draw_mas(mas1, Color.Blue); }
                        else { net = new FuncMethods.NetFunc(mas2); Draw_mas(mas2, Color.Blue); }
                        if (checkBox10.Checked) { net = new FuncMethods.NetFunc(mas3); Draw_mas(mas3, Color.Blue); }
                        a = net.MinArg;
                        b = net.MaxArg;

                        //ShowPoints(net);//показать точки на графике
                        if (checkBox1.Checked)
                        {
                            SequenceFunc t = FuncMethods.Monoms;
                            //SequencePol t = FuncMethods.Monom;
                            RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Other, n);
                            chart1.Series[1].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[1].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы мономов---------");
                            if (radioButton2.Checked) FuncMethods.ShowApprox(f, net.Arguments, t, SequenceFuncKind.Other, n);
                            else FuncMethods.ShowApprox(net, t, SequenceFuncKind.Other, n);
                        }
                        //Наименьшие перпендикуляры
                        if (n == 2)
                        {
                            double a0 = 0, a1 = 0;
                            double dist = 0, distold = 0; ;
                            SLAU S = new SLAU(2);
                            for (int i = 0; i < net.CountKnots; i++)
                            {
                                S.A[0, 0] += net.Arg(i);
                                S.A[1, 0] += net.Arg(i) * net[i];
                                S.A[1, 1] += net[i];
                                S.b[1] -= net[i] * net[i];
                            }
                            S.A[0, 1] = net.CountKnots;
                            S.b[0] = -S.A[1, 1];
                            //S.Show();
                            S.GaussSelection();
                            a1 = S.x[0]; a0 = S.x[1];
                            RealFunc pol = new Polynom(new double[] { -a0, -a1 }).Value;
                            SLAU T = new SLAU(FuncMethods.Monoms, net, n);
                            T.Gauss();//T.Show();

                            for (int i = 0; i < net.CountKnots; i++)
                            {
                                dist += Math.Pow(a1 * net.Arg(i) + net[i] + a0, 2);
                                distold += Math.Pow(T.x[1] * net.Arg(i) + net[i] + T.x[0], 2);
                            }
                            dist /= net.CountKnots;
                            distold /= net.CountKnots;
                            dist = Math.Sqrt(dist);
                            distold = Math.Sqrt(distold);

                            Console.WriteLine("\t(в среднеквадратичной норме перпендикуляров) равна {0}", distold);

                            Program.FORM.chart1.Series.Add("Прямая с наилучшими перпендикулярами");
                            int count = Program.FORM.chart1.Series.Count - 1;
                            Program.FORM.chart1.Series[count].Color = Color.Black;
                            Program.FORM.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                            //Program.FORM.chart1.Series[count].IsVisibleInLegend = false;
                            Program.FORM.chart1.Series[count].BorderWidth = 2;
                            chart1.Series[count].Points.DataBindXY(МатКлассы.Point.PointsX(pol, 100, a, b), МатКлассы.Point.PointsY(pol, 100, a, b));
                            Console.WriteLine("--------Для наименьших перпендикуляров---------");
                            Console.WriteLine("Аппроксимация сеточной функции полученной функцией");
                            Console.WriteLine("\t(в дискретной среднеквадратичной норме) равна {0}", FuncMethods.NetFunc.Distance(net, pol));
                            Console.WriteLine("\t(в среднеквадратичной норме перпендикуляров) равна {0}", dist);
                        }

                        if (checkBox2.Checked)
                        {
                            SequenceFunc t = FuncMethods.Lezhandrs(a, b);
                            //SequencePol t = FuncMethods.Lezhandr(a, b);
                            RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Orthogonal, n);
                            chart1.Series[2].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[2].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы полиномов Лежандра---------");
                            if (radioButton2.Checked) FuncMethods.ShowApprox(f, net.Arguments, t, SequenceFuncKind.Orthogonal, n);
                            else FuncMethods.ShowApprox(net, t, SequenceFuncKind.Orthogonal, n);
                        }

                        if (checkBox6.Checked)
                        {
                            SequenceFunc t = FuncMethods.TrigSystem(a, b);
                            RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Orthonormal, n);
                            chart1.Series[6].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[6].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для ортонормированной системы тригонометрических полиномов---------");
                            if (radioButton2.Checked) FuncMethods.ShowApprox(f, net.Arguments, t, SequenceFuncKind.Orthonormal, n);
                            else FuncMethods.ShowApprox(net, t, SequenceFuncKind.Orthonormal, n);
                        }
                        if (checkBox7.Checked)
                        {
                            SequenceFunc t = FuncMethods.HaarSystem(a, b);
                            RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Orthogonal, n);
                            chart1.Series[7].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[7].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы Хаара---------");
                            if (radioButton2.Checked) FuncMethods.ShowApprox(f, net.Arguments, t, SequenceFuncKind.Orthogonal, n);
                            else FuncMethods.ShowApprox(net, t, SequenceFuncKind.Orthogonal, n);
                        }
                        if (checkBox8.Checked)
                        {
                            SequenceFunc t = (double x, int k) => Math.Exp(k * x);
                            RealFunc g = FuncMethods.Approx(net, t, SequenceFuncKind.Other, n);
                            chart1.Series[8].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[8].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы экспонент---------");
                            if (radioButton2.Checked) FuncMethods.ShowApprox(f, net.Arguments, t, SequenceFuncKind.Other, n);
                            else FuncMethods.ShowApprox(net, t, SequenceFuncKind.Other, n);
                        }
                    }


                }
                else
                {
                    double a = Convert.ToDouble(textBox1.Text), b = Convert.ToDouble(textBox2.Text);
                    int n = Convert.ToInt16(textBox6.Text);

                    if (checkBox9.Checked)
                    {
                        chart1.Series[0].Name = "Функция по системе Хаара, выраженная без учёта ортогональности этой системы";
                        chart1.Series[0].IsVisibleInLegend = true;
                        for (n = n1; n <= n2; n++)
                        {
                            if (checkBox1.Checked)
                            {
                                //SequenceFunc t = FuncMethods.Monoms;
                                SequencePol t = FuncMethods.Monom;
                                RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                                MAS[1, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                chart1.Series[1].Points.AddXY(n, Math.Log10(MAS[1, n])); chart1.Series[1].IsVisibleInLegend = true;
                                chart1.Series[1].Name = "Мономы с применением ультра-гибрида";

                                //g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b, false);
                                //MAS[0, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                //chart1.Series[0].Points.AddXY(n, Math.Log10(MAS[0, n]));
                                //chart1.Series[0].IsVisibleInLegend = true;
                            }
                            if (checkBox2.Checked)
                            {
                                //SequenceFunc t = FuncMethods.Lezhandrs(a,b);
                                SequencePol t = FuncMethods.Lezhandr(a, b);

                                Polynom s = new Polynom(new double[] { -2 * a / (b - a) - 1, 2.0 / (b - a) });
                                RealFunc[] MyMas = new RealFunc[n];
                                for (int i = 0; i < n; i++)
                                    MyMas[i] = /*Polynom.Lezh(i).Value;*/Polynom.Lezh(i).Value(s).Value;
                                //RealFunc g = FuncMethods.ApproxForLezhandr(f, MyMas, a, b);
                                RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Orthogonal, n, a, b);
                                MAS[2, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                chart1.Series[2].Points.AddXY(n, Math.Log10(MAS[2, n])); chart1.Series[2].IsVisibleInLegend = true;
                            }
                            if (checkBox6.Checked)
                            {
                                SequenceFunc t = FuncMethods.TrigSystem(a, b);
                                RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Orthonormal, n, a, b);
                                MAS[6, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                chart1.Series[6].Points.AddXY(n, Math.Log10(MAS[6, n])); chart1.Series[6].IsVisibleInLegend = true;
                                chart1.Series[6].Name = "Тригонометрическая система с применением ультра-гибрида";

                                //g = FuncMethods.Approx(f, t, SequenceFuncKind.Orthonormal, n, a, b, false);
                                //MAS[0, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                //chart1.Series[0].Points.AddXY(n, Math.Log10(MAS[0, n]));
                                //chart1.Series[0].IsVisibleInLegend = true;
                            }
                            if (checkBox7.Checked)
                            {
                                SequenceFunc t = FuncMethods.HaarSystem(a, b);
                                RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Orthogonal, n, a, b);
                                MAS[7, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                chart1.Series[7].Points.AddXY(n, Math.Log10(MAS[7, n])); chart1.Series[7].IsVisibleInLegend = true;
                                chart1.Series[7].Name = "Система Хаара как ортогональная система";

                                //g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b, false);
                                //MAS[0, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                //chart1.Series[0].Points.AddXY(n, Math.Log10(MAS[0, n]));
                                //chart1.Series[0].IsVisibleInLegend = true;
                            }
                            if (checkBox8.Checked)
                            {
                                SequenceFunc t = (double x, int k) => Math.Exp(k * x);
                                RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                                MAS[8, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                chart1.Series[8].Points.AddXY(n, Math.Log10(MAS[8, n])); chart1.Series[8].IsVisibleInLegend = true;
                                chart1.Series[8].Name = "Экспоненты с применением ультра-гибрида";

                                //g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b, false);
                                //MAS[0, n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
                                //chart1.Series[0].Points.AddXY(n, Math.Log10(MAS[0, n]));
                                //chart1.Series[0].IsVisibleInLegend = true;
                            }
                        }
                        Analys(MAS, n1, n2);
                    }
                    else
                    {
                        chart1.Series[0].Points.DataBindXY(МатКлассы.Point.PointsX(f, 100, a, b), МатКлассы.Point.PointsY(f, 100, a, b));
                        if (checkBox1.Checked)
                        {
                            //SequenceFunc t = FuncMethods.Monoms;
                            SequencePol t = FuncMethods.Monom;
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                            chart1.Series[1].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[1].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы мономов---------");
                            FuncMethods.ShowApprox(f, t, SequenceFuncKind.Other, n, a, b);
                        }
                        if (checkBox2.Checked)
                        {
                            //SequenceFunc t = FuncMethods.Lezhandrs(a,b);
                            SequencePol t = FuncMethods.Lezhandr(a, b);
                            Polynom s = new Polynom(new double[] { -2 * a / (b - a) - 1, 2.0 / (b - a) });
                            RealFunc[] MyMas = new RealFunc[n];
                            for (int i = 0; i < n; i++)
                                MyMas[i] = /*Polynom.Lezh(i).Value;*/Polynom.Lezh(i).Value(s).Value;
                            RealFunc g = FuncMethods.ApproxForLezhandr(f, MyMas, a, b);
                            chart1.Series[2].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[2].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы полиномов Лежандра---------");
                            FuncMethods.ShowApprox(f, t, SequenceFuncKind.Orthogonal, n, a, b);
                        }
                        if (checkBox3.Checked)
                        {
                            //SequencePol t = FuncMethods.Cheb(a,b);
                            SequenceFunc t = FuncMethods.Chebs(a, b);
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                            chart1.Series[3].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[3].IsVisibleInLegend = true;
                        }
                        if (checkBox4.Checked)
                        {
                            SequenceFunc t = FuncMethods.Lagerrs(a, b);
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                            chart1.Series[4].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[4].IsVisibleInLegend = true;
                        }
                        if (checkBox5.Checked)
                        {
                            SequenceFunc t = FuncMethods.Hermits(a, b);
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                            chart1.Series[5].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[5].IsVisibleInLegend = true;
                        }
                        if (checkBox6.Checked)
                        {
                            SequenceFunc t = FuncMethods.TrigSystem(a, b);
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Orthonormal, n, a, b);
                            chart1.Series[6].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[6].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для ортонормированной системы тригонометрических полиномов---------");
                            FuncMethods.ShowApprox(f, t, SequenceFuncKind.Orthonormal, n, a, b);
                        }
                        if (checkBox7.Checked)
                        {
                            SequenceFunc t = FuncMethods.HaarSystem(a, b);
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Orthogonal, n, a, b);
                            chart1.Series[7].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[7].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы Хаара---------");
                            FuncMethods.ShowApprox(f, t, SequenceFuncKind.Orthogonal, n, a, b);
                        }
                        if (checkBox8.Checked)
                        {
                            SequenceFunc t = (double x, int k) => Math.Exp(k * x);
                            RealFunc g = FuncMethods.Approx(f, t, SequenceFuncKind.Other, n, a, b);
                            chart1.Series[8].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, a, b), МатКлассы.Point.PointsY(g, 100, a, b)); chart1.Series[8].IsVisibleInLegend = true;
                            Console.WriteLine("--------Для системы экспонент---------");
                            FuncMethods.ShowApprox(f, t, SequenceFuncKind.Other, n, a, b);
                        }
                    }


                }
            }
            catch
            {
                var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                sw.Close();



                Console.SetOut(tmp);
                Console.WriteLine("Запись завершена!");

                Program.DATA.textBox1.Text = "";
                StreamReader sr = new StreamReader("Данные об аппроксимации.txt");
                string ss = "";
                while (ss != null)
                {

                    Program.DATA.textBox1.Text += ss + Environment.NewLine;
                    ss = sr.ReadLine();
                }
                sr.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.DATA.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить картинку как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    StreamWriter sr = new StreamWriter(/*savedialog.FileName+*/"Данные об аппроксимации.txt");
                    string s = "";
                    for (int i = 0; i < Program.DATA.textBox1.Lines.Length; i++)
                    {
                        s = (string)Program.DATA.textBox1.Lines[i];
                        sr.WriteLine(s);
                    }
                    sr.Close();
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Parser.INFORMATION, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        bool ch9 = false;
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            ch9 = !ch9;
            if (ch9)
            {
                label7.Show(); label8.Show();
                numericUpDown1.Show();
                numericUpDown2.Show();
            }
            else
            {
                label7.Hide(); label8.Hide();
                numericUpDown1.Hide();
                numericUpDown2.Hide();
            }
        }
    }
}
