using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using МатКлассы;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Point = МатКлассы.Point;



namespace Курсач
{
    public static class Program
    {
        public static MyForm Form1;
        public static FormResult FORM;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 = new MyForm();
            FORM = new FormResult();

            //Application.Run(new MyForm());
            Application.Run(Form1);
        }
    }

    /// <summary>
    /// Методы курсача
    /// </summary>
    public class KursMethods
    {
        //public static MyForm FORM = new MyForm();

        /// <summary>
        /// Массив базисных точек плоскости
        /// </summary>
        public static Point[] masPoints;
        /// <summary>
        /// Граничная функция от точки
        /// </summary>
        public static Functional fi = (Point a) => 0;
        /// <summary>
        /// Квадрат граничной функции от точки
        /// </summary>
        public static Functional fis = (Point a) => fi(a) * fi(a);
        /// <summary>
        /// Функция наилучшей аппроксимации граничной функции
        /// </summary>
        public static Functional Approx = (Point a) =>
        {
            double sum = 0;

            for (int i = 1; i <= N; i++)
            {
                sum += MySLAU.x[i - 1] * masPoints[i - 1].PotentialF((Point)a);
            }
            return sum;
        };

        //public static Functional fi0;
        /// <summary>
        /// Номер области
        /// </summary>
        public static int CIRCLE = 1;
        /// <summary>
        /// Номер граничной функции
        /// </summary>
        public static int GF = 1;
        /// <summary>
        /// Количество базисных точек
        /// </summary>
        public static int N = 10;
        public static bool zero = false; // наличие нулевых элементов в массиве
        /// <summary>
        /// Всего граничных функций
        /// </summary>
        public static readonly int KGF = 8;
        /// <summary>
        /// Всего кривых
        /// </summary>
        public static readonly int MAXCIRCLE = 4;
        public static readonly double EPS = 0.00001;
        public static readonly double CONSTANT = 1;
        public static readonly double pi = Math.PI;
        /// <summary>
        /// Точность аппроксимации
        /// </summary>
        public static double RESULT;


        //-------------------------------------
        public static int cr = 250; //цветовые параметры для границ
        public static int cg = 0;
        public static int cb = 0;
        public static int mr = 0;
        public static int mg = 250;
        public static int mb = 0;

        //------------------------------------
        public static string dir_Curve_name = new string(new char[50]); //имя внутренней папки и подпапки
        public static string dir_func_name = new string(new char[50]);
        public static string chstr = new string(new char[100]);
        public static string sl = "\\";
        public static string bstr = "";

        /// <summary>
        /// Радиус области
        /// </summary>
        public static double MIN_RADIUS = 0.5;
        /// <summary>
        /// Радиус вне области
        /// </summary>
        public static double MAX_RADIUS = 3.5;

        /// <summary>
        /// Функция произведений базисных функций
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double BasisFuncPow(int i, int j, Point z)
        {
            if ((i == N) && (j == N))
            {
                return fi(z) * fi(z);
            }
            if (i == N)
            {
                return masPoints[j].PotentialF(z) * fi(z);
            }
            if (j == N)
            {
                return masPoints[i].PotentialF(z) * fi(z);
            }
            return masPoints[i].PotentialF(z) * masPoints[j].PotentialF(z);
        }
        /// <summary>
        /// Граница области, в которой решается диф. уравнение
        /// </summary>
        public static CurveK myCurve = new CurveK();
        /// <summary>
        /// Система, которую придётся решить
        /// </summary>
        public static SLAUpok MySLAU = new SLAUpok();
        /// <summary>
        /// Метод решения системы по умолчанию
        /// </summary>
        public static SLAUpok.Method baseMethod = SLAUpok.Method.UltraHybrid;//SLAUpok.Method.Holets;// //SLAUpok.Method.GaussSpeedy;// SLAUpok.Method.UltraHybrid;
        /// <summary>
        /// Частичное решение системы выбранным методом
        /// </summary>
        /// <param name="i"></param>
        /// <param name="mett"></param>
        public static void Method_des(int i, SLAUpok.Method mett)
        {
            switch (mett)
            {
                case SLAUpok.Method.Gauss:
                    MySLAU.Gauss(i);
                    break;
                case SLAUpok.Method.Holets:
                    MySLAU.Holets(i);
                    break;
                case SLAUpok.Method.Jak:
                    MySLAU.Jak(i);
                    break;
                case SLAUpok.Method.Speedy:
                    MySLAU.Speedy(i);
                    break;
                case SLAUpok.Method.GaussSpeedy:
                    MySLAU.GaussSpeedy(i);
                    break;
                case SLAUpok.Method.GaussSpeedyMinimize:
                    MySLAU.GaussSpeedyMinimize(i);
                    break;
                case SLAUpok.Method.UltraHybrid:
                    MySLAU.UltraHybrid(i);
                    break;
            }
        }

        public static void Desigion(int s)
        {
            ForDesigion.Building(s); //чтение и работа с данными
            ForDesigion.Search(); //поиск решения и вывод погрешности
        }
        public static void Desigion(int s, int g, int cu)
        {
            ForDesigion.Building(s, g, cu); //чтение и работа с данными
            ForDesigion.Search(); //поиск решения и вывод погрешности
        }
        public class ForDesigion
        {
            /// <summary>
            /// Построение массива точек при чтении из файла или при автоматическом генерировании
            /// </summary>
            /// <param name="t"></param>
            public static void Building(int t)
            {
                if (t > 0) //заполнение массива базисных точек не из файла
                {
                    GetDefaultData(); //чтение данных не из файла
                    Display(); //сопоставление первым двум числам из файла - области и граничной функции

                    CurveK c1 = new CurveK(0, 2 * pi, TestFuncAndCurve.u1h, TestFuncAndCurve.v1h); //кривая, в окрестности которой эти точки берутся
                    CurveK c2 = new CurveK(0, 3 * MAX_RADIUS, TestFuncAndCurve.u2h, TestFuncAndCurve.v2h);
                    CurveK c3 = new CurveK(0, 4 * MAX_RADIUS, TestFuncAndCurve.u3h, TestFuncAndCurve.v3h);
                    CurveK c4 = new CurveK(0, 1.5 * MAX_RADIUS, TestFuncAndCurve.u4h, TestFuncAndCurve.v4h);

                    switch (CIRCLE)
                    {
                        case 1:
                            FillMassiv(c1, t); //заполнить массив
                            break;
                        case 2:
                            FillMassiv(c2, t); //заполнить массив
                            break;
                        case 3:
                            FillMassiv(c3, t); //заполнить массив
                            break;
                        case 4:
                            FillMassiv(c4, t); //заполнить массив
                                               //fi= TestFuncAndCurve::GFunctions[GF - 1];
                            break;
                    }
                    RandomSwapping(N * 2);
                }
                else
                {
                    ReadDataFromFile(); //чтение данных из файла
                    Display(); //сопоставление первым двум числам из файла - области и граничной функции
                    Screening(); //отсеивание из массива точек одинаковых точек
                    RandomSwapping(N * 2); //перемешать массив
                }
            }

            /// <summary>
            /// Построение массива точек при автоматическом генерировании, зная номер кривой и граничной функции
            /// </summary>
            /// <param name="t"></param>
            /// <param name="g"></param>
            /// <param name="cu"></param>
            public static void Building(int t, int g, int cu) //построение массива точек при чтении из файла или при генерировании
            {
                GF = g;
                CIRCLE = cu;
                Display(); //сопоставление первым двум числам из файла - области и граничной функции
                CurveK c1 = new CurveK(0, 2 * pi, TestFuncAndCurve.u1h, TestFuncAndCurve.v1h); //кривая, в окрестности которой эти точки берутся
                CurveK c2 = new CurveK(0, 3 * MAX_RADIUS, TestFuncAndCurve.u2h, TestFuncAndCurve.v2h);
                CurveK c3 = new CurveK(0, 4 * MAX_RADIUS, TestFuncAndCurve.u3h, TestFuncAndCurve.v3h);
                CurveK c4 = new CurveK(0, 1.5 * MAX_RADIUS, TestFuncAndCurve.u4h, TestFuncAndCurve.v4h);

                switch (CIRCLE)
                {
                    case 1:
                        FillMassiv(c1, t); //заполнить массив
                        break;
                    case 2:
                        FillMassiv(c2, t); //заполнить массив
                        break;
                    case 3:
                        FillMassiv(c3, t); //заполнить массив
                        break;
                    case 4:
                        FillMassiv(c4, t); //заполнить массив
                        break;
                }

                RandomSwapping(N * 2);
            }

            public static void ReadDataFromFile()
            {
                string buff = new string(new char[150]); //создание вспомогательного символьного массива
                int k = 0; //счётчик
                double r; //вспомогательное действительное число
                StreamReader fin = new StreamReader("input.txt"); //объявление объекта для чтения из файла
                buff = Convert.ToString(fin.ReadLine()); //чтение ненужной текстовой информации
                CIRCLE = fin.Read(); //чтение номера области
                buff = Convert.ToString(fin.ReadLine());
                GF = fin.Read(); //чтение номера граничной функции
                buff = Convert.ToString(fin.ReadLine());

                while ((r = fin.Read()) != 0)
                {
                    k++; //пока координаты точек считываются, прибавлять к счётчику
                }
                N = k / 2; //вычисление мощности множества базисных точек
                masPoints = new Point[N];

                fin.Close();
                fin = new StreamReader("input.txt"); //объявление объекта для чтения из файла
                buff = Convert.ToString(fin.ReadLine());
                CIRCLE = Int32.Parse(fin.ReadLine());
                buff = Convert.ToString(fin.ReadLine());
                GF = Int32.Parse(fin.ReadLine());
                buff = Convert.ToString(fin.ReadLine());
                for (int i = 0; i < N; i++)
                {
                    masPoints[i].x = fin.Read();
                    masPoints[i].y = fin.Read(); //заполнение массива точек
                }
                fin.Close();
            }
            public static void GetDefaultData()
            {
                string buff = new string(new char[150]); //создание вспомогательного символьного массива
                StreamReader fin = new StreamReader("input.txt"); //объявление объекта для чтения из файла
                buff = Convert.ToString(fin.ReadLine()); //чтение ненужной текстовой информации
                CIRCLE = Int32.Parse(fin.ReadLine()); //чтение номера области
                buff = Convert.ToString(fin.ReadLine());
                GF = Int32.Parse(fin.ReadLine()); //чтение номера граничной функции
                fin.Close();
            }
            public static void Display() //сопоставление первым двум числам из файла области и граничной функци
            {
                if ((GF <= 0) || (GF > KGF) || (CIRCLE > MAXCIRCLE) || (CIRCLE <= 0))
                {
                    WriteAboutError(); //если выбранный номер больше числа граничных функций на кривую
                }
                else
                {
                    fi = TestFuncAndCurve.GFunctions[GF - 1]; //граничная функция - функция с номером GF для кривой с номером CIRCLE
                    CurveK c1 = new CurveK(0, 2 * pi, TestFuncAndCurve.u1, TestFuncAndCurve.v1);
                    CurveK c2 = new CurveK(0, 3 * MIN_RADIUS, TestFuncAndCurve.u2, TestFuncAndCurve.v2);
                    CurveK c3 = new CurveK(0, 4 * MIN_RADIUS, TestFuncAndCurve.u3, TestFuncAndCurve.v3);
                    CurveK c4 = new CurveK(0, 1.5 * MIN_RADIUS, TestFuncAndCurve.u4, TestFuncAndCurve.v4);
                    switch (CIRCLE)
                    {
                        case 1:
                            myCurve = c1;
                            break;
                        case 2:
                            myCurve = c2;
                            break;
                        case 3:
                            myCurve = c3;
                            break;
                        case 4:
                            myCurve = c4;
                            break;
                    }
                }

            }

            /// <summary>
            /// Дать случайное отклонение точки от кривой
            /// </summary>
            /// <returns></returns>
            public static double GetRandomEps()
            {
                double e = RandomNumbers.NextNumber() % 25 * EPS / 25;
                double p = RandomNumbers.NextNumber();
                double q = RandomNumbers.NextNumber();
                if (Math.Sign((sbyte)p - q) < 0)
                {
                    return Math.Sign((sbyte)p - q) * Math.Min(MAX_RADIUS - MIN_RADIUS, e) / 100;
                }
                return Math.Sign((sbyte)p - q) * e;
            }

            /// <summary>
            /// Заполнить массив базисных точек не через файл - около такой кривой и от стольки точек
            /// </summary>
            /// <param name="c"></param>
            /// <param name="z"></param>
            public static void FillMassiv(CurveK c, int z)
            {
                N = z;
                masPoints = new Point[N];
                for (int i = 0; i < z; i++)
                {
                    masPoints[i] = c.Transfer(c.a + (c.b - c.a) * i / z);
                    double l = GetRandomEps() / Math.Sqrt(2);
                    masPoints[i].x += l;
                    masPoints[i].y += l;

                }
            }

            //private static void Swap<T>(ref T lhs, ref T rhs)
            //{
            //    T temp;
            //    temp = lhs;
            //    lhs = rhs;
            //    rhs = temp;
            //}

            /// <summary>
            /// Случайно перемешать массив masPoints в р действий
            /// </summary>
            /// <param name="p"></param>
            public static void RandomSwapping(int p)
            {
                for (int i = 1; i <= p; i++)
                {
                    int a = RandomNumbers.NextNumber() % N;
                    int b = RandomNumbers.NextNumber() % N;
                    Expendator.Swap<Point>(ref masPoints[a], ref masPoints[b]);
                }
            }

            /// <summary>
            /// Удаление из массива элемента i
            /// </summary>
            /// <param name="i"></param>
            /// <param name="a"></param>
            public static void DeleteElement(int i, Point[] a)
            {
                a[N - 1] = a[i];
                for (int j = i; j < N - 1; j++) a[j] = a[j + 1];
                N--;
            }

            /// <summary>
            /// Отсеивание из массива повторяющихся элементов
            /// </summary>
            /// <param name="masPoints"></param>
            public static void ExceptionMas(Point[] masPoints) //
            {
                for (int i = 1; i < N; i++)
                {
                    if (masPoints[i - 1].x == masPoints[i].x && masPoints[i - 1].y == masPoints[i].y)
                    {
                        DeleteElement(i, masPoints);
                        i--;
                    } //удаление из массива точек повторяющихся элементов
                }
            }

            /// <summary>
            /// Очистка массива от повторяющихся элементов
            /// </summary>
            public static void Screening()
            {
                Array.Sort(masPoints); //сортировка точек по компаратору
                ExceptionMas(masPoints); //отсеивание повторяющихся точек
            }

            /// <summary>
            /// Сообщение в файл об ошибке
            /// </summary>
            public static void WriteAboutError()
            {
                StreamWriter fout = new StreamWriter("output.txt");
                fout.WriteLine("НЕСУЩЕСТВУЮЩИЙ НОМЕР ГРАНИЧНОЙ ФУНКЦИИ ИЛИ ОБЛАСТИ! Всего имеется " + CIRCLE + " границ области и " + KGF + " граничных функций!");
                fout.Close();
                Environment.Exit(0);
            }

            /// <summary>
            /// Вывод вектора решения и точности (после поиска решения)
            /// </summary>
            /// <param name="x"></param>
            /// <param name="EPS"></param>
            public static void WriteErrorDataInFile(double[] x, double EPS)
            {
                //запись в файл
                string buf = new string(new char[250]);
                string str;
                string d1 = Convert.ToString(baseMethod);
                string d2 = Convert.ToString(N);
                string d3 = Convert.ToString(MIN_RADIUS);
                string d4 = Convert.ToString(MAX_RADIUS);
                string d5 = Convert.ToString(GF);
                string d6 = Convert.ToString(CIRCLE);
                str = "Вектор решения и точность аппроксимации для функции " + d5 + " на кривой " + d6 + " методом " + d1 + " при числе функций " + d2 + ".txt";
                buf = Convert.ToString(str);

                StreamWriter fout = new StreamWriter(buf);
                fout.WriteLine("Вектор решения (" + N + " точек):");
                for (int i = 0; i < N; i++)
                {
                    fout.WriteLine(x[i]);
                }
                fout.WriteLine("Получившаяся невязка при количестве функций " + N + " равна " + SLAUpok.NEVA); //вывести невязку
                fout.WriteLine("Разница приближённого решения с граничной функцией при числе базисных функций " + N + " равна " + EPS);
                Program.FORM.chart1.Titles[0].Text = "Качество аппроксимации: " + EPS.ToString();
                RESULT = EPS;
                fout.Close();
            }

            public static void Error() //нахождение и вывод точности по известному решению
            {
                double p = myCurve.Firstkind(N, N);
                double sum = 0;
                double[] Ax = new double[N];
                SLAU.Func_in_matrix.Matrix_power(ref Ax, KursMethods.MySLAU.A, KursMethods.MySLAU.x, N);
                for (int i = 0; i < N; i++)
                {
                    sum += KursMethods.MySLAU.x[i] * Ax[i]; //myCurve.Firstkind(i,N);
                }
                WriteErrorDataInFile(KursMethods.MySLAU.x, Math.Sqrt(Math.Abs(p - sum)));
            }
            public static void Search()
            {
                KursMethods.MySLAU.Make(N); //создать систему порядка, равного числу базисных точек

                //Matrix A=new Matrix(N);

                for (int i = 0; i < N; i++) //заполнить систему
                {
                    //KursMethods.MySLAU.x[i] = 1;

                    KursMethods.MySLAU.b[i] = myCurve.Firstkind(i, N);
                    KursMethods.MySLAU.A[i, i] = myCurve.Firstkind(i, i);
                    //A[i, i] = KursMethods.MySLAU.A[i, i];
                    for (int j = i + 1; j < N; j++) //так как матрица симметрическая
                    {
                        double tmp = myCurve.Firstkind(i, j);
                        KursMethods.MySLAU.A[i, j] = tmp;
                        KursMethods.MySLAU.A[j, i] = tmp;
                        //A[i, j] = tmp;
                        //A[j, i] = tmp;
                    }
                }
                //-----------------------------------------решить систему-----------------------------------
                Method_des(N, baseMethod);

                //StreamWriter e = new StreamWriter("kf.txt");
                //e.WriteLine(A.PrintMatrix());
                //A.PrintMatrix();
                //e.Close();

                Error();
            }
        }

        /// <summary>
        /// Иллюстрирование
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="masPoints"></param>
        /// <param name="x"></param>
        /// <param name="myCurve"></param>
        /// <param name="m"></param>
        public static void Illustrating()
        {

            //RealFunc s = (double i) => fi(myCurve.Transfer(i));
            //RealFunc r = (double i) => Approx(myCurve.Transfer(i));
            double ep = 0.001;

            //double[] a1 = Point.PointsX(s, ep, myCurve.a, myCurve.b);
            //double[] b1= Point.PointsY(s, ep, myCurve.a, myCurve.b);

            Program.FORM.chart1.Series[0].Points.Clear();
            Program.FORM.chart1.Series[1].Points.Clear();

            //Program.FORM.chart1.Series[0].Points.DataBindXY(a1, b1);

            Program.FORM.chart1.Series[0].Name = $"Граничная функция {GF} на кривой {CIRCLE}";
            Program.FORM.chart1.Series[0].Points.AddXY(myCurve.a, fi(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку
            for (double i = myCurve.a + ep; i <= myCurve.b - ep; i += ep)
                Program.FORM.chart1.Series[0].Points.AddXY(i, fi(myCurve.Transfer(i)));

            // графики сумм m потенциальных функций
            Program.FORM.chart1.Series[1].Name = $"Сумма {N} потенциальных функций";
            Program.FORM.textBox1.Text += $"Начинается рисование графика для области {CIRCLE} и граничной функции {GF}";
            Program.FORM.textBox1.Text += Environment.NewLine;

            Program.FORM.chart1.Series[1].Points.AddXY(myCurve.a, Approx(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку
            for (double j = myCurve.a + ep; j <= myCurve.b; j += ep)
                Program.FORM.chart1.Series[1].Points.AddXY(j, Approx(myCurve.Transfer(j)));

            //запись в файл bmp
            string buf = new string(new char[150]);
            string str;
            //memset(buf, 0, sizeof(sbyte));
            string d1 = Convert.ToString(GF);
            string d2 = Convert.ToString(CIRCLE);
            string d3 = Convert.ToString(N);
            str = bstr + sl + "График 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";

            buf = Convert.ToString(str);
            Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);

        }

        /// <summary>
        /// Картинки приближения для minN-maxN функций c шагом cu
        /// </summary>
        /// <param name="minN"></param>
        /// <param name="maxN"></param>
        /// <param name="cu"></param>
        public static void Pictures_ill(int minN, int maxN, int cu)
        {
            string str1;
            string str2;
            int t = MAXCIRCLE * KGF * ((maxN - minN) / cu + 1);
            int ind = 0;

            for (KursMethods.CIRCLE = 1; KursMethods.CIRCLE <= MAXCIRCLE; KursMethods.CIRCLE++)
            {
                string d5 = Convert.ToString(GF);
                string d6 = Convert.ToString(CIRCLE);
                string d7 = Convert.ToString(baseMethod);
                str1 = "Данные для кривой " + d6;
                dir_Curve_name = str1;
                Directory.CreateDirectory(dir_Curve_name); //создать папку для кривой

                for (KursMethods.GF = 1; KursMethods.GF <= KGF; KursMethods.GF++)
                {
                    d5 = Convert.ToString(GF);
                    d6 = Convert.ToString(CIRCLE);
                    d7 = Convert.ToString(baseMethod);
                    str2 = "При граничной функции " + d5;
                    dir_func_name = str2;
                    bstr = str1 + sl + str2;
                    chstr = bstr;
                    Directory.CreateDirectory(chstr); //создать в ней папку для функции

                    //SLAUpok.VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);
                    //for(int i=2;i<minN;i++) Desigion(i, KursMethods.GF, KursMethods.CIRCLE);

                    for (int m = minN; m <= maxN; m += cu)
                    {
                        SLAUpok.VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);
                        ind++;
                        Desigion(m, KursMethods.GF, KursMethods.CIRCLE);//заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
                        Illustrating();// график граничной функции и приближения

                        Program.FORM.textBox1.Text += "-------Осталось ";
                        Program.FORM.textBox1.Text += t - ind;
                        Program.FORM.textBox1.Text += Environment.NewLine;
                        Program.FORM.textBox1.Refresh();
                        Program.FORM.chart1.Refresh();
                        Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();

                    }
                }
            }
        }

        public static void Fixity()
        { //график функции (число потенциалов)->(расстояние до граничной функции)
            Program.FORM.chart1.Series[0].Points.Clear();
            Program.FORM.chart1.Series[1].Points.Clear();

            SLAUpok.VALUE_FOR_ULTRA = myCurve.Firstkind(fis); //?
            double[] Errors = new double[N]; //массив погрешностей
            ForFixity.Create(Errors); //заполнить массив ошибок
            ForFixity.WriteMassiv(Errors); //вывести погрешности
            if (zero)
                ForFixity.Show(Errors); //нарисовать ломанную ошибок
        }

        public static void Fixity(SLAUpok.Method A, SLAUpok.Method B)
        { //график функции (число потенциалов)->(расстояние до граничной функции) для методов A и B

            Program.FORM.chart1.Series[0].Points.Clear();
            Program.FORM.chart1.Series[1].Points.Clear();

            SLAUpok.VALUE_FOR_ULTRA = 10; //?
            double[] ErrorsA = new double[N]; //массив погрешностей A
            double[] ErrorsB = new double[N]; //массив погрешностей A
            ForFixity.Create(ErrorsA, ErrorsB, A, B); //заполнить массив ошибок
                                                      /*if (zero)*/
            ForFixity.Show(ErrorsA, ErrorsB, A, B); //нарисовать ломанную ошибок
        }

        public class ForFixity
        {
            /// <summary>
            /// Возврат погрешности в евклидовой норме
            /// </summary>
            /// <returns></returns>
            public static double ReturnError()
            {
                double p = myCurve.Firstkind(N, N);
                double sum = 0;

                double[] Ax = new double[N];
                SLAU.Func_in_matrix.Matrix_power(ref Ax, KursMethods.MySLAU.A, KursMethods.MySLAU.x, N);
                for (int i = 0; i < N; i++)
                {
                    sum += KursMethods.MySLAU.x[i] * Ax[i];
                }
                return Math.Sqrt(Math.Abs(p - sum));
            }

            /// <summary>
            /// Рисование самого графика на основе массива точек
            /// </summary>
            /// <param name="Errors"></param>
            public static void Show(double[] Errors)
            {
                //Program.FORM.chart1.Series[0].Points.Clear();
                //Program.FORM.chart1.Series[1].Points.Clear();
                Program.FORM.chart1.Series[1].Name = "";

                //double max = Math.Log10(Errors[0]);
                //double min = Math.Log10(Errors[N - 1]);
                //for (int i = 0; i < N; i++) //определение минимума и максимума
                //{
                //    double tmp = Math.Log10(Errors[i]);
                //    if (tmp < min)
                //    {
                //        min = tmp;
                //    }
                //    if (tmp > max)
                //    {
                //        max = tmp;
                //    }
                //}

                Program.FORM.chart1.Series[0].Color = Color.Red; // задаем цвет линии (красный)
                Program.FORM.chart1.Series[0].Points.AddXY(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку
                Program.FORM.chart1.Series[0].Name = $"Качество аппроксимации функции {GF} на области {CIRCLE} в зависимости от числа базисных функций {N}";

                for (int i = 0; i < N; i++)
                    Program.FORM.chart1.Series[0].Points.AddXY(i + 1, Math.Log10(Errors[i]));


                string buf = new string(new char[250]);
                string str;
                //memset(buf, 0, sizeof(sbyte));
                string d2 = Convert.ToString(N);
                string d3 = Convert.ToString(GF);
                string d4 = Convert.ToString(CIRCLE);
                string d5 = Convert.ToString(GF);
                string d6 = Convert.ToString(baseMethod);
                //str = "График 2 качества аппроксимации в зависимости от числа базисных функций (" + d2 + ").bmp";
                str = "График 2 качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").bmp";
                buf = Convert.ToString(str);

                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                //CloseWindow("График 2 качества аппроксимации в зависимости от числа базисных функций.bmp");// закрываем окно (создаем bmp-файл)
            }

            /// <summary>
            /// Рисование самого графика на основе массива точек
            /// </summary>
            /// <param name="ErrorsA"></param>
            /// <param name="ErrorsB"></param>
            /// <param name="A"></param>
            /// <param name="B"></param>
            public static void Show(double[] ErrorsA, double[] ErrorsB, SLAUpok.Method A, SLAUpok.Method B)
            {
                //Program.FORM.chart1.Series[0].Points.Clear();
                //Program.FORM.chart1.Series[1].Points.Clear();

                //double max = Math.Log10(ErrorsA[0]);
                //double min = Math.Log10(ErrorsA[N - 1]);
                //for (int i = 0; i < N; i++) //определение минимума и максимума
                //{
                //    double tmp = Math.Log10(ErrorsA[i]);
                //    double tmpp = Math.Log10(ErrorsB[i]);
                //    if (tmp < min)
                //    {
                //        min = tmp;
                //    }
                //    if (tmp > max)
                //    {
                //        max = tmp;
                //    }
                //    if (tmpp < min)
                //    {
                //        min = tmpp;
                //    }
                //    if (tmpp > max)
                //    {
                //        max = tmpp;
                //    }
                //}

                Program.FORM.chart1.Series[0].Name = $"Аппроксимация функции {GF} на области {CIRCLE} методом {A} при числе точек {N}";
                Program.FORM.chart1.Series[0].Color = Color.Red; // задаем цвет линии (красный)
                Program.FORM.chart1.Series[0].Points.AddXY(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку

                for (int i = 0; i < N; i++)
                {
                    Program.FORM.chart1.Series[0].Points.AddXY(i + 1, Math.Log10(ErrorsA[i])); //cout+i+" "+Errors[i]+endl;
                }

                //SetColor(0, 250, 0); // задаем цвет линии (зеленый)
                Program.FORM.chart1.Series[1].Name = $"Аппроксимация функции {GF} на области {CIRCLE} методом {B} при числе точек {N}";
                Program.FORM.chart1.Series[1].Color = Color.Green;
                Program.FORM.chart1.Series[1].Points.AddXY(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку
                for (int i = 0; i < N; i++)
                {
                    Program.FORM.chart1.Series[1].Points.AddXY(i + 1, Math.Log10(ErrorsB[i])); //cout+i+" "+Errors[i]+endl;
                }

                string buf = new string(new char[250]);
                string str;
                //memset(buf, 0, sizeof(sbyte));
                string d2 = Convert.ToString(N);
                string d3 = Convert.ToString(GF);
                string d4 = Convert.ToString(CIRCLE);
                string d5 = Convert.ToString(A);
                string d6 = Convert.ToString(B);
                //str = "График 2 качества аппроксимации в зависимости от числа базисных функций (" + d2 + ").bmp";
                str = bstr + sl + "График 2 качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") в зависимости от числа базисных функций (" + d2 + ") при методах " + d5 + " (красный) и " + d6 + " (зелёный).bmp";
                buf = Convert.ToString(str);
                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                //CloseWindow("График 2 качества аппроксимации в зависимости от числа базисных функций.bmp");// закрываем окно (создаем bmp-файл)
            }

            public static double Error(int k) //частичная погрешность
            {
                double p = myCurve.Firstkind(N, N);
                double sum = 0;

                double[] Ax = new double[N];
                SLAUpok.Func_in_matrix.Matrix_power(ref Ax, KursMethods.MySLAU.A, KursMethods.MySLAU.x, k);
                for (int i = 0; i < k; i++)
                {
                    sum += KursMethods.MySLAU.x[i] * Ax[i];
                }
                double EPS = Math.Abs(p - sum);
                return Math.Sqrt(EPS);

            }

            public static void Create(double[] Errors)
            {
                Program.FORM.textBox1.Text += "Для графика в зависимости от числа точек:";
                Program.FORM.textBox1.Text += Environment.NewLine;
                for (int i = 0; i < N; i++)
                {
                    Method_des(i, baseMethod); //решить частичную систему нужным методом
                    Errors[i] = Error(i); //заполнить массив погрешностей

                    if (i % 10 == 0)
                    {
                        Program.FORM.textBox1.Text += i + 1;
                        Program.FORM.textBox1.Text += " -> ";
                        Program.FORM.textBox1.Text += Errors[i];
                        Program.FORM.textBox1.Text += Environment.NewLine;
                        Program.FORM.textBox1.Refresh();
                        Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();
                    }
                }
                //Errors[N - 1] = RESULT;
            }

            public static void Create(double[] ErrorsA, double[] ErrorsB, SLAUpok.Method A, SLAUpok.Method B)
            {
                Program.FORM.textBox1.Text += "Для графика в зависимости от числа точек:";
                Program.FORM.textBox1.Text += Environment.NewLine;
                for (int i = 0; i < N; i++)
                {
                    Method_des(i, A); //решить частичную систему нужным методом
                    ErrorsA[i] = Error(i); //заполнить массив погрешностей
                    if (i % 10 == 0)
                    {
                        Program.FORM.textBox1.Text += i + 1;
                        Program.FORM.textBox1.Text += " (A) -> ";
                        Program.FORM.textBox1.Text += ErrorsA[i];
                        Program.FORM.textBox1.Text += Environment.NewLine;
                        Program.FORM.textBox1.Refresh();
                        Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();
                    }

                } //при использовании ультра-гибрида приходится разбивать на два цикла
                for (int i = 0; i < N; i++)
                {
                    Method_des(i, B); //решить частичную систему нужным методом
                    ErrorsB[i] = Error(i); //заполнить массив погрешностей
                    if (i % 10 == 0)
                    {
                        Program.FORM.textBox1.Text += i + 1;
                        Program.FORM.textBox1.Text += " (B) -> ";
                        Program.FORM.textBox1.Text += ErrorsB[i];
                        Program.FORM.textBox1.Text += Environment.NewLine;
                        Program.FORM.textBox1.Refresh();
                        Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();
                    }
                }
            }

            public static void WriteAboutZero(int NN, int GFf, int cCIRCLE, SLAUpok.Method meth, int num) //сообщение об ошибке для графика 2
            {
                //запись в файл
                string buf = new string(new char[250]);
                string str;
                //memset(buf, 0, sizeof(sbyte));
                string d2 = Convert.ToString(NN);
                string d3 = Convert.ToString(GFf);
                string d4 = Convert.ToString(cCIRCLE);
                string d6 = Convert.ToString(meth);
                str = "Сообщение об ошибке для графика 2 погрешностей (без log) качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").txt";
                buf = Convert.ToString(str);
                StreamWriter fout = new StreamWriter(buf);
                fout.WriteLine("Невозможно построить логарифмический график, поскольку на элементе " + num + "(=0) функция принимает значение -infinity");
                fout.Close();
            }

            public static void WriteMassiv(double[] x) //вывод массива погрешностей
            {
                //запись в файл
                string buf = new string(new char[250]);
                string str;
                //memset(buf, 0, sizeof(sbyte));
                string d2 = Convert.ToString(N);
                string d3 = Convert.ToString(GF);
                string d4 = Convert.ToString(CIRCLE);
                string d6 = Convert.ToString(baseMethod);
                str = "Файл 2 погрешностей (без log) качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").txt";
                buf = Convert.ToString(str);
                StreamWriter fout = new StreamWriter(buf);

                zero = true; //нулей нет
                int j = -1; //номер с нулём
                double min = x[0]; //минимальный элемент, максимальный, первый скачок, максимальный скачок
                double max = x[0];
                double mins = -1;
                double maxs = -1;
                int mini = 0; //соответствующие номера
                int maxi = 0;
                int mins1 = -1;
                int maxs1 = -1;
                int mins2 = -1;
                int maxs2 = -1;
                for (int i = 0; i < N; i++)
                {
                    fout.WriteLine(x[i]);
                    if (x[i] == 0)
                    {
                        zero = false; //есть нули
                        j = i; //зафиксировать номер
                    }
                    //поиск минимального и максимального элемента
                    if (x[i] < min)
                    {
                        min = x[i];
                        mini = i;
                    }
                    else if (x[i] > max)
                    {
                        max = x[i];
                        maxi = i;
                    }
                }

                for (int i = 1; i < N; i++)
                {
                    if (x[i] - x[i - 1] > 0)
                    {
                        mins = x[i] - x[i - 1];
                        mins1 = i - 1;
                        mins2 = i;
                        maxs = mins;
                        maxs1 = mins1;
                        maxs2 = mins2;
                        for (int u = i + 1; u < N; u++)
                        {
                            double p = x[u] - x[u - 1];
                            if (p > 0)
                            {
                                if (p > maxs)
                                {
                                    maxs = p;
                                    maxs1 = u - 1;
                                    maxs2 = u;
                                }
                            }
                        }
                        goto end1;
                    }
                }
                end1:
                fout.WriteLine();
                fout.WriteLine("Анализ массива:");
                fout.WriteLine("1)Минимальное значение " + min + " на элементе " + mini);
                fout.WriteLine("2)Максимальное значение " + max + " на элементе " + maxi);
                if (mins > 0)
                {
                    fout.WriteLine("3)Первый скачок " + mins + " с элементa " + mins1 + " на элемент " + mins2);
                    fout.WriteLine("4)Максимальный скачок " + maxs + " с элементa " + maxs1 + " на элемент " + maxs2);
                }

                fout.Close();

                Program.FORM.chart1.Titles[0].Text = "Наилучшая аппроксимация равна " + min + " на элементе " + (mini + 1);

                if (!zero)
                {
                    WriteAboutZero(N, GF, CIRCLE, baseMethod, j); //написать сообщение об ошибке
                }
            }
        }

        /// <summary>
        /// Нарисовать систему картинок для от minN до maxN функций, cu кривых
        /// </summary>
        /// <param name="minN"></param>
        /// <param name="maxN"></param>
        /// <param name="cu"></param>
        public static void Pictures_fix(int minN, int maxN, int cu)
        {
            string str1;
            string str2;
            int t = MAXCIRCLE * KGF * ((maxN - minN) / cu + 1);
            int ind = 0;

            for (KursMethods.CIRCLE = 1; KursMethods.CIRCLE <= MAXCIRCLE; KursMethods.CIRCLE++)
            {
                string d5 = Convert.ToString(GF);
                string d6 = Convert.ToString(CIRCLE);
                string d7 = Convert.ToString(baseMethod);
                str1 = "Данные для кривой " + d6;
                dir_Curve_name = str1;
                Directory.CreateDirectory(dir_Curve_name); //создать папку для кривой

                for (KursMethods.GF = 1; KursMethods.GF <= KGF; KursMethods.GF++)
                {
                    d5 = Convert.ToString(GF);
                    d6 = Convert.ToString(CIRCLE);
                    d7 = Convert.ToString(baseMethod);
                    str2 = "При граничной функции " + d5;
                    dir_func_name = str2;
                    bstr = str1 + sl + str2;
                    chstr = bstr;
                    Directory.CreateDirectory(chstr); //создать в ней папку для функции
                    for (int m = minN; m <= maxN; m += cu)
                    {
                        ind++;
                        Desigion(m, KursMethods.GF, KursMethods.CIRCLE); //заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
                        Fixity(); //график зависимости погрешности аппроксимации от числа базисных точек
                        Program.FORM.textBox1.Text += "-------Осталось ";
                        Program.FORM.textBox1.Text += t - ind;
                        Program.FORM.textBox1.Text += Environment.NewLine;
                        //Program.FORM.Invalidate();
                        Program.FORM.textBox1.Refresh();
                        Program.FORM.chart1.Refresh();
                        Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();
                    }
                }
            }
        }

        //-----------------------------------------------------------

        /// <summary>
        /// График зависимости качества аппроксимации от радиуса при s функциях и d кривых
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <param name="g"></param>
        /// <param name="cu"></param>
        public static void Quality(int s, int d, int g, int cu)
        {
            //------------------------------обнуление данных
            //Program.FORM.chart1.Series[0].Points.Clear();
            //Program.FORM.chart1.Series[1].Points.Clear();
            Program.FORM.chart1.Series.Clear();
            //Program.FORM.chart1.Series[0].Name = "   ";
            //Program.FORM.chart1.Series[1].Name = "  ";
            Program.FORM.chart1.Series.Add("   ");
            Program.FORM.chart1.Series.Add("  ");
            Program.FORM.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart1.Titles[0].Text = "Кривые, на которых происходит проверка";

            double[] Errors = new double[d]; //массив погрешностей
            //Errors[d - 1] = 100;
            double EPSs = (MAX_RADIUS - MIN_RADIUS) / d;
            ForQuality.Draw_CIRCLE(MIN_RADIUS, Color.Red); //нарисовать краcным изначальную окружность 
            Program.FORM.textBox1.Text += $"Для графика в зависимости от радиуса на кривой {cu} при граничной функции {g}:";
            Program.FORM.textBox1.Text += Environment.NewLine;
            int i = 0;
            double tmp = MAX_RADIUS;
            bool isred = false; //наличие набора с чистым нулём

            //заполнение массива
            for (MAX_RADIUS = MIN_RADIUS + EPSs; MAX_RADIUS <= tmp + EPSs; MAX_RADIUS += EPSs)
            {
                SLAUpok.VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);
                if ((g != 0) && (cu != 0))
                {
                    Desigion(s, g, cu);
                }
                else
                {
                    Desigion(s);
                }
                Errors[i] = ForFixity.ReturnError();

                Color color = Color.Green;
                ForQuality.Draw_CIRCLE(MAX_RADIUS, Color.Green); //нарисовать окружность
                if (Errors[i] == 0)
                {
                    Program.FORM.textBox1.Text += "Аппроксимация в машинный ноль на элементе " + (i + 1) + " (радиус " + MAX_RADIUS + " )" + Environment.NewLine;
                    color = Color.Red;
                    //ForQuality.Draw_mas(masPoints, color); //нарисовать точки массива masPoints
                    isred = true;
                } //если на этих точках точность максимальна, нарисовать их красными
                ForQuality.Draw_mas(masPoints, color); //нарисовать точки массива masPoints

                if (i % 10 == 0)
                {
                    Program.FORM.textBox1.Text += "Расстояние при радиусе области ";
                    Program.FORM.textBox1.Text += MIN_RADIUS;
                    Program.FORM.textBox1.Text += " и радиусе кривой ";
                    Program.FORM.textBox1.Text += MAX_RADIUS;
                    Program.FORM.textBox1.Text += " равно ";
                    Program.FORM.textBox1.Text += Errors[i];
                    Program.FORM.textBox1.Text += Environment.NewLine;
                    Program.FORM.textBox1.Refresh();
                    Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();
                }
                i++;
                if (i == d) break;
            }

            Program.FORM.chart1.Refresh();
            Program.FORM.textBox1.Text += "------Массив данных записан";
            Program.FORM.textBox1.Text += Environment.NewLine;
            Program.FORM.textBox1.Refresh();
            Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();

            //записать в файл массив и провести анализ
            string buf = new string(new char[250]);
            string str;
            string newstr;
            string d1 = Convert.ToString(s);
            string d2 = Convert.ToString(d);
            string d3 = Convert.ToString(MIN_RADIUS);
            string d4 = Convert.ToString(MAX_RADIUS);
            string d5 = Convert.ToString(GF);
            string d6 = Convert.ToString(CIRCLE);
            string d7 = Convert.ToString(baseMethod);
            str = "Файл 3 погрешностей качества аппроксимации методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.txt";
            newstr = bstr + sl + str; //полный адрес
            buf = Convert.ToString(newstr);
            StreamWriter fout = new StreamWriter(buf);

            zero = true; //нулей нет
            int j = -1; //номер нуля
            double min = Errors[0]; //минимальный элемент, максимальный, первый скачок, максимальный скачок
            double max = Errors[0];
            double mins = -1;
            double maxs = -1;
            int mini = 0; //соответствующие номера
            int maxi = 0;
            int mins1 = -1;
            int maxs1 = -1;
            int mins2 = -1;
            int maxs2 = -1;
            for (int dis = 0; dis < d; dis++)
            {
                fout.WriteLine(Errors[dis] + " \t --> log10 = " + Math.Log10(Errors[dis]));
                if ((Errors[dis] <= 0) || (Errors[dis] != Errors[dis])) //проверка на 0 и на NaN
                {
                    zero = false;
                    j = dis;
                }
                //поиск минимального и максимального элемента
                if (Errors[dis] < min)
                {
                    min = Errors[dis];
                    mini = dis;
                }
                else if (Errors[dis] > max)
                {
                    max = Errors[dis];
                    maxi = dis;
                }
            }
            for (int dis = 1; dis < d; dis++)
            {
                if (Errors[dis] - Errors[dis - 1] > 0)
                {
                    mins = Errors[dis] - Errors[dis - 1];
                    mins1 = dis - 1;
                    mins2 = dis;
                    maxs = mins;
                    maxs1 = mins1;
                    maxs2 = mins2;
                    for (int jj = dis + 1; jj < d; jj++)
                    {
                        double p = Errors[jj] - Errors[jj - 1];
                        if (p > 0)
                        {
                            if (p > maxs)
                            {
                                maxs = p;
                                maxs1 = jj - 1;
                                maxs2 = jj;
                            }
                        }
                    }
                    goto end1;
                }
            }
            end1:
            fout.WriteLine();
            fout.WriteLine("Анализ массива:");
            fout.WriteLine("1)Минимальное значение " + min + " на элементе " + mini);
            fout.WriteLine("2)Максимальное значение " + max + " на элементе " + maxi);
            Program.FORM.chart1.Titles[0].Text = "Наилучшая аппроксимация равна " + (min) + " и достигается при радиусе " + (MIN_RADIUS + (mini + 1) * EPSs);
            if (mins > 0)
            {
                fout.WriteLine("3)Первый скачок " + mins + " с элементa " + mins1 + " на элемент " + mins2);
                fout.WriteLine("4)Максимальный скачок " + maxs + " с элементa " + maxs1 + " на элемент " + maxs2);
            }
            if (!zero)
            {
                fout.Write("Невозможно построить логарифмический график, поскольку на элементе " + j + 1 + " (радиус " + MIN_RADIUS + j * EPSs + ") функция принимает значение " + Math.Log10(Errors[j]));
            }
            fout.Close();

            if (isred == zero) throw new Exception("Есть нули, и нет нулей");

            //если есть набор с чистым нулём, показать его на графике
            if (isred)
            {
                Program.FORM.chart1.Series.Add($"{s} потенциальных функций...");
                Program.FORM.chart1.Series.Add($"...при граничной функции {g}");
                str = "График 3.2 кривых и базисных потенциалов методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            else //нарисовать график, если нет нулей
            {

                Program.FORM.chart1.Series.Add($"{s} потенциальных функций...");
                Program.FORM.chart1.Series.Add($"...при граничной функции {g}");
                str = "График 3.2 кривых и базисных потенциалов методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                //------------------------------обнуление данных
                Program.FORM.chart1.Series.Clear();
                Program.FORM.chart1.Series.Add($" ");
                Program.FORM.chart1.Series.Add($"  ");
                Program.FORM.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                //Program.FORM.chart1.Titles[0].Text = "Кривые, на которых происходит проверка";
                Program.FORM.chart1.Titles[0].Text = "Наилучшая аппроксимация равна " + (min) + " и достигается при радиусе " + (MIN_RADIUS + (mini + 1) * EPSs);
                //Рисование графика зависимости
                Program.FORM.chart1.Series[0].Name = "Качество аппроксимации функции " + g + " в зависимости от радиуса, (" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ")";

                //double maxx = Math.Log10(Errors[0]);
                //double minn = max;
                //for (int dis = 1; dis < d; dis++)
                //{
                //    double tmpp = Math.Log10(Errors[dis]);
                //    if (tmpp < minn)
                //    {
                //        minn = tmpp;
                //    }

                //    if (tmpp > maxx)
                //    {
                //        maxx = tmpp;
                //    }
                //}

                Program.FORM.chart1.Series[0].Points.Clear();
                Program.FORM.chart1.Series[0].Color = Color.Red; // задаем цвет линии (красный)
                Program.FORM.chart1.Series[0].Points.AddXY(MIN_RADIUS + EPSs, Math.Log10(Errors[0])); // устанавливаем курсор на точку
                for (int dis = 1; dis < d; dis++)
                    Program.FORM.chart1.Series[0].Points.AddXY(MIN_RADIUS + (dis + 1) * EPSs, Math.Log10(Errors[dis])); //рисуем ломанную

                Program.FORM.chart1.Refresh();
                str = "График 3.1 качества аппроксимации методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        public class ForQuality
        {
            public static void Draw_CIRCLE(double radius, Color color) //нарисовать окружность
            {
                //SetColor(R, G, B); // задаем цвет линии
                double d = radius - MIN_RADIUS;
                int count = 0;


                if (d > 0)
                {
                    Program.FORM.chart1.Series.Add("");
                    count = Program.FORM.chart1.Series.Count - 1;
                    Program.FORM.chart1.Series[count].Color = color;
                    Program.FORM.chart1.Series[count].IsVisibleInLegend = false;
                    Program.FORM.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }
                else Program.FORM.chart1.Series[count].Color = color;

                double ep = 0.01;
                CurveK c3 = new CurveK(0, 3 * radius, TestFuncAndCurve.u2h, TestFuncAndCurve.v2h);
                switch (CIRCLE)
                {
                    case 1:
                        Program.FORM.chart1.Series[count].Points.AddXY(radius, 0); // устанавливаем курсор на точку
                        for (double i = ep; i <= 2 * pi; i += ep)
                        {
                            Program.FORM.chart1.Series[count].Points.AddXY(radius * Math.Cos(i), radius * Math.Sin(i)); //рисуем CIRCLE
                        }
                        break;
                    case 2: //треугольник
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d, -0.5 * d / Math.Sqrt(3));
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + radius, -0.5 * d / Math.Sqrt(3));
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + 0.5 * radius, -0.5 * d / Math.Sqrt(3) + 0.5 * radius * Math.Sqrt(3));
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d, -0.5 * d / Math.Sqrt(3));
                        //Program.FORM.chart1.Series[0].Points.AddXY(c3.Transfer(radius).x, c3.Transfer(radius).y);
                        //Program.FORM.chart1.Series[0].Points.AddXY(c3.Transfer(2*radius).x, c3.Transfer(2*radius).y);
                        //Program.FORM.chart1.Series[0].Points.AddXY(c3.Transfer(3*radius).x, c3.Transfer(3*radius).y);
                        break;
                    case 3: //квадрат
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + 0, -0.5 * d + 0);
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d, -0.5 * d + radius);
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + radius, -0.5 * d + radius);
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + radius, -0.5 * d + 0);
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + 0, -0.5 * d + 0);
                        break;
                    case 4: //острие
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + 0, -0.25 * d * Math.Sqrt(3) + 0);
                        Program.FORM.chart1.Series[count].Points.AddXY(-0.5 * d + radius, -0.25 * d * Math.Sqrt(3) + 0);

                        for (double i = ep; i <= pi / 3; i += ep)
                        {
                            Program.FORM.chart1.Series[count].Points.AddXY(radius * Math.Cos(i) - 0.5 * d, radius * Math.Sin(i) - 0.25 * d * Math.Sqrt(3)); //рисуем CIRCLE
                        }
                        for (double i = 2 * pi / 3; i <= pi; i += ep)
                        {
                            Program.FORM.chart1.Series[count].Points.AddXY(radius * Math.Cos(i) - 0.5 * d + radius, radius * Math.Sin(i) - 0.25 * d * Math.Sqrt(3)); //рисуем CIRCLE
                        }
                        break;
                }

            }
            public static void Draw_mas(Point[] r, Color color) //нарисовать массив точек
            {

                //SetColor(R, G, B); // задаем цвет линии
                //if(R==0) 
                //Program.FORM.chart1.Series[0].Color = Color.Green;

                double e = 0.03;
                for (int i = 0; i < N; i++)
                {
                    Program.FORM.chart1.Series.Add("");
                    int count = Program.FORM.chart1.Series.Count - 1;
                    Program.FORM.chart1.Series[count].Color = color;

                    Program.FORM.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    Program.FORM.chart1.Series[count].IsVisibleInLegend = false;

                    Program.FORM.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y - e); // устанавливаем курсор на точку
                    Program.FORM.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y + e); //рисуем line
                    Program.FORM.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y + e); // устанавливаем курсор на точку
                    Program.FORM.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y - e); //рисуем line
                }
                //Program.FORM.chart1.Series[0].Color = Color.Red;
            }
        }

        /// <summary>
        /// Картинки для функций от minN до maxN и шагом cu на m кривых
        /// </summary>
        /// <param name="m"></param>
        /// <param name="minN"></param>
        /// <param name="maxN"></param>
        /// <param name="cu"></param>
        public static void Pictures_qua(int m, int minN, int maxN, int cu)
        {
            string str1;
            string str2;
            int tt = MAXCIRCLE * KGF * ((maxN - minN) / cu + 1);
            int ind = 0;
            for (CIRCLE = 1; CIRCLE <= MAXCIRCLE; CIRCLE++)
            {

                string d5 = Convert.ToString(GF);
                string d6 = Convert.ToString(CIRCLE);
                string d7 = Convert.ToString(baseMethod);
                str1 = "Данные для кривой " + d6;
                dir_Curve_name = str1;
                Directory.CreateDirectory(dir_Curve_name); //создать папку для кривой

                for (GF = 1; GF <= KGF; GF++)
                {
                    d5 = Convert.ToString(GF);
                    d6 = Convert.ToString(CIRCLE);
                    d7 = Convert.ToString(baseMethod);
                    str2 = "При граничной функции " + d5;
                    dir_func_name = str2;
                    bstr = str1 + sl + str2;
                    chstr = bstr;
                    Directory.CreateDirectory(chstr); //создать в ней папку для функции

                    for (int t = minN; t <= maxN; t += cu)
                    {
                        ind++;
                        //я переставил первые два аргумента местами
                        Quality(t, m, GF, CIRCLE); //график зависимости погрешности от кривой, возле которой берутся базисные точки
                        Program.FORM.textBox1.Text += "-------Осталось ";
                        Program.FORM.textBox1.Text += tt - ind;
                        Program.FORM.textBox1.Text += Environment.NewLine;
                        Program.FORM.textBox1.Refresh();
                        Program.FORM.chart1.Refresh();

                        //Program.FORM.textBox1.ScrollToCaret();
                        Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();

                    }
                }
            }
        }

        /// <summary>
        /// Пространство тестовых функций и данных
        /// </summary>
        public class TestFuncAndCurve
        {
            //возможные параметризации для области
            //окружность радиуса MIN_RADIUS
            public static double u1(double t)
            {
                return MIN_RADIUS * Math.Cos(t);
            }
            public static double v1(double t)
            {
                return MIN_RADIUS * Math.Sin(t);
            }

            //соответствующая окружность радиуса MAX_RADIUS (около которой берутся базисные точки)
            public static double u1h(double t)
            {
                return MAX_RADIUS * Math.Cos(t);
            }
            public static double v1h(double t)
            {
                return MAX_RADIUS * Math.Sin(t);
            }

            //равносторонний треугольник со стороной MIN_RADIUS
            public static double u2(double t)
            {
                if ((t >= 0) && (t <= 2 * MIN_RADIUS))
                {
                    return t / 2;
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return 3 * MIN_RADIUS - 1 * t;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v2(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return t / 2 * Math.Sqrt(3);
                }
                if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))
                {
                    return -t / 2 * Math.Sqrt(3) + MIN_RADIUS * Math.Sqrt(3);
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return 0;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double dis = MAX_RADIUS - MIN_RADIUS; //кажется, из-за глобальности этих переменных всегда происходит стягивание одной вершины в одну и ту же точку
            public static double mdx = dis / 2;
            public static double mdy = mdx / Math.Sqrt(3);
            //соответствующий равносторонний треугольник со стороной MAX_RADIUS
            public static double u2h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2; //но если внутри каждой вставить это,оставив те глобальные, всё получится
                if ((t >= 0) && (t <= 2 * MAX_RADIUS))
                {
                    return t / 2 - mdx;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return 3 * MAX_RADIUS - 1 * t - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v2h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                mdy = mdx / Math.Sqrt(3);
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return t / 2 * Math.Sqrt(3) - mdy;
                }
                if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))
                {
                    return -t / 2 * Math.Sqrt(3) + MAX_RADIUS * Math.Sqrt(3) - mdy;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return 0 - mdy;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }

            //квадрат со стороной MIN_RADIUS
            public static double u3(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return t;
                }
                if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))
                {
                    return MIN_RADIUS;
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return 3 * MIN_RADIUS - t;
                }
                if ((t >= 3 * MIN_RADIUS) && (t <= 4 * MIN_RADIUS))
                {
                    return 0;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v3(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return 0;
                }
                if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))
                {
                    return t - MIN_RADIUS;
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return MIN_RADIUS;
                }
                if ((t >= 3 * MIN_RADIUS) && (t <= 4 * MIN_RADIUS))
                {
                    return 4 * MIN_RADIUS - t;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            //соответствующий квадрат со стороной MAX_RADIUS
            public static double u3h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return t - mdx;
                }
                if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))
                {
                    return MAX_RADIUS - mdx;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return 3 * MAX_RADIUS - t - mdx;
                }
                if ((t >= 3 * MAX_RADIUS) && (t <= 4 * MAX_RADIUS))
                {
                    return 0 - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v3h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return 0 - mdx;
                }
                if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))
                {
                    return t - MAX_RADIUS - mdx;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return MAX_RADIUS - mdx;
                }
                if ((t >= 3 * MAX_RADIUS) && (t <= 4 * MAX_RADIUS))
                {
                    return 4 * MAX_RADIUS - t - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            //острие
            public static double u4(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return t;
                }
                if ((t >= MIN_RADIUS) && (t <= 1.5 * MIN_RADIUS))
                {
                    return 3 * MIN_RADIUS - 2 * t;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v4(double t)
            {
                if ((t >= 0) && (t <= 0.5 * MIN_RADIUS))
                {
                    return Math.Sqrt(MIN_RADIUS * MIN_RADIUS - (t - MIN_RADIUS) * (t - MIN_RADIUS));
                }
                if ((t >= 0.5 * MIN_RADIUS) && (t <= MIN_RADIUS))
                {
                    return Math.Sqrt(MIN_RADIUS * MIN_RADIUS - t * t);
                }
                if ((t >= MIN_RADIUS) && (t <= 1.5 * MIN_RADIUS))
                {
                    return 0;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }

            public static double u4h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                mdy = mdx / Math.Sqrt(3);
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return t - mdx;
                }
                if ((t >= MAX_RADIUS) && (t <= 1.5 * MAX_RADIUS))
                {
                    return 3 * MAX_RADIUS - 2 * t - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v4h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                mdy = mdx * Math.Sqrt(3) / 2;
                if ((t >= 0) && (t <= 0.5 * MAX_RADIUS))
                {
                    return Math.Sqrt(MAX_RADIUS * MAX_RADIUS - (t - MAX_RADIUS) * (t - MAX_RADIUS)) - mdy;
                }
                if ((t >= 0.5 * MAX_RADIUS) && (t <= MAX_RADIUS))
                {
                    return Math.Sqrt(MAX_RADIUS * MAX_RADIUS - t * t) - mdy;
                }
                if ((t >= MAX_RADIUS) && (t <= 1.5 * MAX_RADIUS))
                {
                    return 0 - mdy;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }

            //граничные функции и массив граничных функций
            public static double g1(Point point)
            {
                return Math.Cos(point.x) * Math.Cos(point.y);
            }
            public static double g2(Point point)
            {
                return Math.Sin(point.y);
            }
            public static double g3(Point point)
            {
                return point.x * point.x + 4;
            }

            public static double g4(Point point)
            {
                return CONSTANT;
            }

            public static double g5(Point point)
            {
                return masPoints[0].PotentialF((Point)point);
            }

            public static double g6(Point point)
            {
                return Math.Log(Math.Abs(point.x * point.y) + 1) + 2 * point.x;
            }
            public static double g7(Point point)
            {
                if (Math.Cos(point.x * point.y) != 0)
                {
                    return Math.Cos(2 * point.x) / Math.Cos(point.x * point.y);
                }
                return 1;
            }

            public static double g8(Point point)
            {
                double dx = MIN_RADIUS / 2;
                double dy = MIN_RADIUS / 2 * Math.Sqrt(3);
                double argument;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: Point d = point;
                Point d = new Point(point); //d.x=-0.25;d.y=-0.25;
                d.x -= dx;
                d.y -= dy; //сдвиг к началу координат

                if (d.x == 0)
                {
                    argument = pi / 2 * Math.Sign((sbyte)d.y);
                }
                else
                {
                    if (d.y == 0)
                    {
                        argument = pi * Math.Sign((sbyte)-1 + Math.Sign((sbyte)d.x));
                    }
                    else
                    {
                        argument = Math.Atan(d.y / d.x) + Math.Sign((sbyte)Math.Abs(d.x) - d.x) * Math.Sign((sbyte)d.y) * pi;
                    }
                }
                //return argument;
                //cout+d.x+" "+d.y+" аргумент в доли: "+argument/pi+endl;

                if ((-pi <= argument) && (argument < -2 * pi / 3))
                {
                    return -1.0 / 2;
                }
                if ((-2 * pi / 3 <= argument) && (argument <= -pi / 3))
                {
                    return 0; //return 1; //единицы будут
                }
                if ((-pi / 3 < argument) && (argument <= pi / 2))
                {
                    return 1.0 / 2; //return 1; //уже не будут
                }
                /*if ((pi / 2 < argument) && (argument <= pi))*/
                return -1.0 / 2;
            }
            public static Functional[] GFunctions = { g1, g2, g3, g4, g5, g6, g7, g8 };
        }

        internal static class RandomNumbers
        {
            private static System.Random r;

            internal static int NextNumber()
            {
                if (r == null)
                    Seed();

                return r.Next();
            }

            internal static int NextNumber(int ceiling)
            {
                if (r == null)
                    Seed();

                return r.Next(ceiling);
            }

            internal static void Seed()
            {
                r = new System.Random();
            }

            internal static void Seed(int seed)
            {
                r = new System.Random(seed);
            }
        }

    }

    /// <summary>
    /// Класс кривых для курсача - пришлось так сделать, чтобы добавить новый метод
    /// </summary>
    public class CurveK : Curve
    {
        public CurveK(double a0, double b0, RealFunc uu, RealFunc vv) : base(a0, b0, uu, vv) { }
        public CurveK() : base() { }

        /// <summary>
        /// Вычисление криволинейного интеграла первого рода по этой кривой от функции BasisFuncPow(int i,int j,Point z)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double Firstkind(int i, int j)
        {
            Get_h(ITER_INTEG);
            double value = 0;
            for (int k = 1; k <= M; k++)
            {
                value += (KursMethods.BasisFuncPow(i, j, Transfer(a + (k) * _h)) + KursMethods.BasisFuncPow(i, j, Transfer(a + (k - 1) * _h))) * Point.Eudistance(Transfer(a + (k - 1) * _h), Transfer(a + (k) * _h)) / 2; //метод трапеций
            }
            return value;
        }
    }

    /// <summary>
    /// Класс СЛАУ с методами их решения (продолжение)
    /// </summary>
    public class SLAUpok : SLAU
    {
        public SLAUpok() : base() { }
        public SLAUpok(StreamReader fs) : base(fs) { }
        public SLAUpok(SLAUpok M, int t) : base(M, t) { }

        public double Error(int k) //частичная погрешность
        {
            double p = KursMethods.myCurve.Firstkind(KursMethods.N, KursMethods.N);
            double sum = 0;

            double[] Ax = new double[KursMethods.N];
            Func_in_matrix.Matrix_power(ref Ax, A, x, k);
            for (int i = 0; i < k; i++)
            {
                sum += x[i] * Ax[i];
            }
            double EPS = Math.Abs(p - sum);
            return Math.Sqrt(EPS);
        }

        public static double VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);

        public void UltraHybrid(int t) //гибридный с координатной минимизацией по последней координате
        {
            double[] c = new double[t];
            for (int i = 0; i < t - 1; i++)
                c[i] = x[i];
            //Пересоздать систему
            SLAUpok M = new SLAUpok(this, t);
            //VALUE_FOR_ULTRA = M.Error(t);

            double sum = 0;
            GaussSpeedy(t);

            if ((VALUE_FOR_ULTRA < Error(t)) && (t >= 1)) //если погрешность выросла - исправить это
            {
                for (int i = 0; i < t - 1; i++)
                {
                    x[i] = c[i];
                }
                x[t - 1] = 0;
                if (t != 0)
                {
                    for (int j = 0; j < t - 1; j++)
                    {
                        sum += x[j] * A[t - 1, j];
                    }
                    x[t - 1] = (b[t - 1] - sum) / A[t - 1, t - 1];
                    sum = 0;

                    double tmp1 = Error(t);

                    if (VALUE_FOR_ULTRA < tmp1) //погрешность опять выросла
                    {
                        for (int i = 0; i < t - 1; i++)
                        {
                            x[i] = c[i];
                        }
                        x[t - 1] = 0;
                    }
                }
            }

            VALUE_FOR_ULTRA = Error(t);
            NEVA = Nev(A, x, b, t);
        }
    }
}
