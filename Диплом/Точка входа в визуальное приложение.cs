using System;
using System.Diagnostics;
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
using Complex = МатКлассы.Number.Complex;
using static МатКлассы.FuncMethods.DefInteg;
using Библиотека_графики;
using MathNet.Numerics;


namespace Курсач
{
    static class Program
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
    public partial class KursMethods
    {
        static KursMethods() { }

        /// <summary>
        /// Массив базисных точек плоскости
        /// </summary>
        public static Point[] masPoints;
        /// <summary>
        /// Граничная функция от точки
        /// </summary>
        public static Functional fi = (Point a) => 0;
        /// <summary>
        /// Плотность граничной функции
        /// </summary>
        public static Functional fig = (Point a) => 0;
        /// <summary>
        /// Квадрат граничной функции от точки
        /// </summary>
        public static Functional fis = (Point a) => fi(a) * fi(a);
        /// <summary>
        /// Функция наилучшей аппроксимации граничной функции (старая - комбинация потенциалов)
        /// </summary>
        public static Functional OldApprox = (Point a) =>
        {
            double sum = 0;

            for (int i = 1; i <= N; i++)
            {
                sum += MySLAU.x[i - 1] * masPoints[i - 1].PotentialF(a);
            }
            return sum;
        };
        /// <summary>
        /// Функция наилучшей аппроксимации граничной функции (старая - комбинация потенциалов)
        /// </summary>
        public static Functional OldApproxQ = (Point a) =>
        {
            double sum = 0;

            for (int i = 1; i <= N; i++)
            {
                sum += MySLAUQ.x[i - 1] * masPoints[i - 1].PotentialF(a);
            }
            return sum;
        };
        /// <summary>
        /// Функция наилучшей аппроксимации граничной функции
        /// </summary>
        public static Functional Approx = (Point x) =>
        {
            //Functional f = (Point y) => OldApprox(y) * E(new Point(x.x - y.x, x.y - y.y));
            //return DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);

            //return AreaMas[KursMethods.CIRCLE - 1].DInteg(f, 25, true);

            double sum = 0;
            for (int i = 1; i <= N; i++)
            {
                sum += MySLAUQ.x[i - 1] * mu(x, i - 1);
            }
            return sum;
        };

        public static Functional M1Approx = (Point x) =>
          {
              Functional ro = (Point y) => OldApprox(y) * Exy(x, y);
              return IntegralClass.Integral(ro, CIRCLE - 1) /*- ((!filtersDQ[CIRCLE-1](x))?right(x):0.0)*/;
              //return DoubleIntegral(ro, myCurve, myCurve.S, FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);
          };

        public static double otrval => MIN_RADIUS * 1.1;

        public static Action<string, Func<Point, bool>> For3D = (string filename, Func<Point, bool> filt) => МатКлассы.ForScripts.MakeFilesForSurfaces(-otrval, otrval, -otrval, otrval, 120, filename, new Functional[] { fig, OldApproxQ }, filt);
        public static Action<string, Func<Point, bool>> For3D2 = (string filename, Func<Point, bool> filt) => МатКлассы.ForScripts.MakeFilesForSurfaces(-MIN_RADIUS * 0.1, MIN_RADIUS * 1.1, -MIN_RADIUS * 0.1, MIN_RADIUS * 1.1, 100, filename, new Functional[] { fig, OldApproxQ }, filt);

        public static Func<Point, bool> filt,
            filt1 = (Point t) =>
              {
                  return Point.Eudistance(Point.Zero, t) <= MIN_RADIUS;
              }
        ,
            filt2 = (Point t) =>
            {
                if (t.x >= 0 && t.y >= 0 && t.x <= MIN_RADIUS)
                {
                    if (t.x <= 0.5 * MIN_RADIUS)
                        return t.y <= Math.Sqrt(3) * t.x;
                    return t.y <= -Math.Sqrt(3) * t.x + MIN_RADIUS * Math.Sqrt(3);
                }
                return false;
            }
        ,
            filt3 = (Point t) =>
            {
                return t.x >= 0 && t.y >= 0 && t.x <= MIN_RADIUS && t.y <= MIN_RADIUS;
            }
        ,
            filt4 = (Point t) =>
            {
                if (t.x >= 0 && t.y >= 0 && t.x <= MIN_RADIUS)
                {
                    if (t.x <= 0.5 * MIN_RADIUS)
                        return t.y <= Math.Sqrt(MIN_RADIUS.Sqr() - (t.x - MIN_RADIUS).Sqr());
                    return t.y <= Math.Sqrt(MIN_RADIUS.Sqr() - (t.x).Sqr());
                }
                return false;
            };
        /// <summary>
        /// Фильтры принадлежности точки к области
        /// </summary>
        public static Func<Point, bool>[] filters = new Func<Point, bool>[] { filt1, filt2, filt3, filt4 };

        private static double epsQ=1e-12;
        public static Func<Point, bool>[] filtersDQ = new Func<Point, bool>[] {
                         (Point t) =>
              {
                  return Math.Abs(Point.Eudistance(Point.Zero, t)- MIN_RADIUS)<=epsQ;
              }
        ,
             (Point t) =>
            {
                if (t.x >= 0 && t.y >= 0 && t.x <= MIN_RADIUS)
                {
                    if (t.x <= 0.5 * MIN_RADIUS)
                        return Math.Abs(t.y  -Math.Sqrt(3) * t.x)<=epsQ;
                    return Math.Abs(t.y-(-Math.Sqrt(3) * t.x + MIN_RADIUS * Math.Sqrt(3))) <= epsQ;
                }
                return false;
            }
        ,
             (Point t) =>
            {
                return t.x <=epsQ || t.y <=epsQ ||Math.Abs( t.x-  MIN_RADIUS)<= epsQ|| Math.Abs(t.y-MIN_RADIUS) <= epsQ;
            }
        ,
             (Point t) =>
            {
                if (t.x >= 0 && t.y >= 0 && t.x <= MIN_RADIUS)
                {
                    if (t.x <= 0.5 * MIN_RADIUS)
                        return Math.Abs(t.y -(Math.Sqrt(MIN_RADIUS.Sqr() - (t.x - MIN_RADIUS).Sqr())))<=epsQ ;
                    return Math.Abs(t.y- Math.Sqrt(MIN_RADIUS.Sqr() - (t.x).Sqr()))<=epsQ ;
                }
                return false;
            }
    };


        //списки для сохранения значений интегралов (но при замене потенциалов может наебнуться)
        private static List<Tuple<Point, int>> list1 = new List<Tuple<Point, int>>();
        private static List<double> list2 = new List<double>();

        /// <summary>
        /// Общий базисный потенциал
        /// </summary>
        public static Functional E = (Point x) => -Math.Log((new Complex(x.x, x.y)).Abs) /*/ 2 / Math.PI*/;
        public static SeqPointFunc _mu = (Point x, int k) =>
          {
              Functional f = (Point y) => masPoints[k].PotentialF(y) * E(new Point(x.x - y.x, x.y - y.y));//f(new Point(1, 1)).Show();
                                                                                                          //double integ=DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);
                                                                                                          //double integ = AreaMas[KursMethods.CIRCLE-1].DInteg(f, Method.GaussKronrod61,200);
            double integ = IntegralClass.Integral(f, KursMethods.CIRCLE - 1);//AreaMas[KursMethods.CIRCLE - 1].DInteg(f, 30,true);
            list1.Add(new Tuple<Point, int>(x, k));
              list2.Add(integ);
              return integ;
          };
        public static SeqPointFunc mu;


        public static double Density()
        {
            Functional f = (Point x) =>
            {
                double sum = OldApprox(x) - fig(x);
                return sum * sum;
            };
            return IntegralClass.Integral(f, CIRCLE - 1);//DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod61Empire/*GaussKronrod15*/, 0.001, FuncMethods.DefInteg.countY);
        }
        public static double DensityQ()
        {
            Functional f = (Point x) =>
            {
                double sum = OldApproxQ(x) - fig(x);
                return sum * sum;
            };
            return IntegralClass.Integral(f, CIRCLE - 1); //DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod61Empire/*GaussKronrod15*/, 0.001, FuncMethods.DefInteg.countY);
        }

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
        public static readonly int KGF = 11;
        /// <summary>
        /// Всего кривых
        /// </summary>
        public static readonly int MAXCIRCLE = 4;
        public static readonly double EPS = 0.00001;
        public static readonly double CONSTANT = 1.5;
        public static readonly double pi = Math.PI;
        /// <summary>
        /// Точность аппроксимации
        /// </summary>
        public static double RESULT;
        /// <summary>
        /// Число кривых
        /// </summary>
        public static int CountCircle => MAXCIRCLE;

        public static string[] CircleName = new string[] { "CIRCLE", "TRIANGLE", "SQUARE", "EDGE" };
        public static string[] FuncName = new string[KGF];

        public static string dir_Curve_name = new string(new char[50]); //имя внутренней папки и подпапки
        public static string dir_func_name = new string(new char[50]);
        public static string chstr = new string(new char[100]);
        public static string sl = "\\";
        public static string bstr = Program.Form1.DirectName;

        public static string adress = "";//"Векторные" + sl;

        /// <summary>
        /// Радиус области
        /// </summary>
        public static double MIN_RADIUS = 0.5;
        /// <summary>
        /// Радиус вне области
        /// </summary>
        public static double MAX_RADIUS = 3.5;
        /// <summary>
        /// Радиус области L
        /// </summary>
        public static double LMIN_RADIUS = 0.5;
        /// <summary>
        /// MAX pадиус области L
        /// </summary>
        public static double LMAX_RADIUS = 3;

        private static AreaForDoubleInteg[] AreaMas;
        private static void SetAreaMas() => AreaMas = new AreaForDoubleInteg[]
{
            new AreaForDoubleInteg(-MIN_RADIUS,MIN_RADIUS,(t)=>-Math.Sqrt(MIN_RADIUS*MIN_RADIUS-t*t),(t)=>Math.Sqrt(MIN_RADIUS*MIN_RADIUS-t*t)),
            new AreaForDoubleInteg(0,MIN_RADIUS,(t)=>0,(t)=>{
                if(t<=MIN_RADIUS/2)
                    return t*Math.Sqrt(3);
                return Math.Sqrt(3)*(MIN_RADIUS-t);
            }),
            new AreaForDoubleInteg(0,MIN_RADIUS,t=>0,t=>MIN_RADIUS),
            new AreaForDoubleInteg(0,MIN_RADIUS,(t)=>0,(t)=>{
                if(t<=MIN_RADIUS/2)
                    return Math.Sqrt(MIN_RADIUS.Sqr()-(t-MIN_RADIUS).Sqr());
                return Math.Sqrt(MIN_RADIUS.Sqr()-(t).Sqr());
            })
};

        /// <summary>
        /// Функция произведений базисных функций
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double OldBasisFuncPow(int i, int j, Point z)
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
        /// Функция произведений новых базисных функций
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double BasisFuncPow(int i, int j, Point z)
        {
            //$"вызов значения функции {++q}".Show();
            if ((i == N) && (j == N))
            {
                return fi(z) * fi(z);
            }
            if (i == N)
            {
                return mu(z, j) * fi(z);
            }
            if (j == N)
            {
                return mu(z, i) * fi(z);
            }
            return mu(z, i) * mu(z, j);
        }

        /// <summary>
        /// Граница области L, в которой решается диф. уравнение
        /// </summary>
        public static CurveK myCurve = new CurveK();
        /// <summary>
        /// Граница области Q
        /// </summary>
        public static CurveK myCurveQ = new CurveK();

        /// <summary>
        /// Система, которую придётся решить
        /// </summary>
        public static SLAUpok MySLAU = new SLAUpok();
        public static SLAUpok MySLAUQ = new SLAUpok();
        /// <summary>
        /// Метод решения системы по умолчанию
        /// </summary>
        public static SLAUpok.Method baseMethod = SLAUpok.Method.UltraHybrid;//SLAUpok.Method.UltraHybrid;//SLAUpok.Method.Holets;// //SLAUpok.Method.GaussSpeedy;// SLAUpok.Method.UltraHybrid;
        /// <summary>
        /// Частичное решение системы выбранным методом
        /// </summary>
        /// <param name="i"></param>
        /// <param name="mett"></param>
        public static void Method_des(int i, SLAUpok.Method mett, SLAUpok sys)
        {
            switch (mett)
            {
                case SLAUpok.Method.Gauss:
                    sys.Gauss(i);
                    break;
                case SLAUpok.Method.Holets:
                    sys.Holets(i);
                    break;
                case SLAUpok.Method.Jak:
                    sys.Jak(i);
                    break;
                case SLAUpok.Method.Speedy:
                    sys.Speedy(i);
                    break;
                case SLAUpok.Method.GaussSpeedy:
                    sys.GaussSpeedy(i);
                    break;
                case SLAUpok.Method.GaussSpeedyMinimize:
                    sys.GaussSpeedyMinimize(i);
                    break;
                case SLAUpok.Method.UltraHybrid:
                    sys.UltraHybrid(i);
                    break;
            }
        }

        public static void Desigion(int s)
        {
            ForDesigion.Building(s); //чтение и работа с данными
            ForDesigion.Search(); //поиск решения и вывод погрешности
        }
        public static void Desigion(int s, int g, int cu, bool QsystemISNeeded = true, SLAUpok SYSTEM = null, SLAUpok SYSTEMQ = null)
        {
            ForDesigion.Building(s, g, cu, SYSTEM, SYSTEMQ); //чтение и работа с данными
            ForDesigion.Search(QsystemISNeeded, SYSTEM, SYSTEMQ); //поиск решения и вывод погрешности
        }

        /// <summary>
        /// Кривые, в окрестности которых берутся точки
        /// </summary>
        public static CurveK[] curves = new CurveK[]
        {
        new CurveK(0, 2 * pi, TestFuncAndCurve.u1h, TestFuncAndCurve.v1h),//кривая, в окрестности которой эти точки берутся
        new CurveK(0, 3 * MAX_RADIUS, TestFuncAndCurve.u2h, TestFuncAndCurve.v2h),
        new CurveK(0, 4 * MAX_RADIUS, TestFuncAndCurve.u3h, TestFuncAndCurve.v3h),
         new CurveK(0, 1.5 * MAX_RADIUS, TestFuncAndCurve.u4h, TestFuncAndCurve.v4h)
    };
        /// <summary>
        /// Границы областей
        /// </summary>
        public static CurveK[] curvesQ = new CurveK[]
{
        new CurveK(0, 2 * pi, TestFuncAndCurve.u1, TestFuncAndCurve.v1),//кривая, в окрестности которой эти точки берутся
        new CurveK(0, 3 * MIN_RADIUS, TestFuncAndCurve.u2, TestFuncAndCurve.v2),
        new CurveK(0, 4 * MIN_RADIUS, TestFuncAndCurve.u3, TestFuncAndCurve.v3),
         new CurveK(0, 1.5 * MIN_RADIUS, TestFuncAndCurve.u4, TestFuncAndCurve.v4)
};

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
                    var df = new Memoize<Tuple<Point, int>, double>((Tuple<Point, int> tt) => _mu(tt.Item1, tt.Item2)).Value;
                    mu = (Point point, int kk) => df(new Tuple<Point, int>(point, kk));
                    SetAreaMas();
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
            public static void Building(int t, int g, int cu, SLAUpok SYSTEM = null, SLAUpok SYSTEMQ = null) //построение массива точек при чтении из файла или при генерировании
            {
                list1 = new List<Tuple<Point, int>>();
                list2 = new List<double>();
                GF = g% (KGF - 1);//-1, т.к. последняя функция не учитывается
                
                if (SYSTEM == null)
                {
                    CIRCLE = cu;
                    Display(); //сопоставление первым двум числам из файла - области и граничной функции
                    "Display выполнен".Show();
                    CurveK c1 = new CurveK(0, 2 * pi, TestFuncAndCurve.u1h, TestFuncAndCurve.v1h); //кривая, в окрестности которой эти точки берутся
                    CurveK c2 = new CurveK(0, 3 * MAX_RADIUS, TestFuncAndCurve.u2h, TestFuncAndCurve.v2h);
                    CurveK c3 = new CurveK(0, 4 * MAX_RADIUS, TestFuncAndCurve.u3h, TestFuncAndCurve.v3h);
                    CurveK c4 = new CurveK(0, 1.5 * MAX_RADIUS, TestFuncAndCurve.u4h, TestFuncAndCurve.v4h);

                    switch (CIRCLE)
                    {
                        case 1:
                            FillMassiv(c1, t); //заполнить массив
                            filt = new Func<Point, bool>(filt1);
                            break;
                        case 2:
                            FillMassiv(c2, t); //заполнить массив
                            filt = new Func<Point, bool>(filt2);
                            break;
                        case 3:
                            FillMassiv(c3, t); //заполнить массив
                            filt = new Func<Point, bool>(filt3);
                            break;
                        case 4:
                            FillMassiv(c4, t); //заполнить массив
                            filt = new Func<Point, bool>(filt4);
                            break;
                    }

                    RandomSwapping(N * 2);

                    var df = new Memoize<Tuple<Point, int>, double>((Tuple<Point, int> tt) => _mu(tt.Item1, tt.Item2)).Value;
                    mu = (Point point, int kk) => df(new Tuple<Point, int>(point, kk));
                    SetAreaMas();
                }
                else
                {
                    fi = (Point x) =>
                    {
                        Functional f = (Point p) => U(p) * E(new Point(x.x - p.x, x.y - p.y));
                        return DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);
                    };
                    fig = U; //граничная функция - функция с номером GF для кривой с номером CIRCLE

                    MySLAU.curve = SYSTEM.curve;
                    MySLAUQ.curve = SYSTEMQ.curve;
                }


                if (g > KGF)
                {
                    fi = KursMethods.right;
                    fig = U; //граничная функция - функция с номером GF для кривой с номером CIRCLE
                }

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
                    throw new Exception("тут что-то не так");
                    WriteAboutError(); //если выбранный номер больше числа граничных функций на кривую
                }
                else
                {
                    fi = (Point x) =>
                    {
                        Functional f = (Point p) => U(p) * E(new Point(x.x - p.x, x.y - p.y));
                        return DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);
                    };
                    fig = U; //граничная функция - функция с номером GF для кривой с номером CIRCLE
                    CreateByCIRCLE();
                }

            }
            public static void CreateByCIRCLE()
            {
                CurveK c1 = new CurveK(0, 2 * pi, TestFuncAndCurve.u1, TestFuncAndCurve.v1, MIN_RADIUS, TestFuncAndCurve.U[0], TestFuncAndCurve.V[0], TestFuncAndCurve.T[0], TestFuncAndCurve.Ends[0]);
                CurveK c2 = new CurveK(0, 3 * MIN_RADIUS, TestFuncAndCurve.u2, TestFuncAndCurve.v2, MIN_RADIUS, TestFuncAndCurve.U[1], TestFuncAndCurve.V[1], TestFuncAndCurve.T[1], TestFuncAndCurve.Ends[1]);
                CurveK c3 = new CurveK(0, 4 * MIN_RADIUS, TestFuncAndCurve.u3, TestFuncAndCurve.v3, MIN_RADIUS, TestFuncAndCurve.U[2], TestFuncAndCurve.V[2], TestFuncAndCurve.T[2], TestFuncAndCurve.Ends[2]);
                CurveK c4 = new CurveK(0, 1.5 * MIN_RADIUS, TestFuncAndCurve.u4, TestFuncAndCurve.v4, MIN_RADIUS, TestFuncAndCurve.U[3], TestFuncAndCurve.V[3], TestFuncAndCurve.T[3], TestFuncAndCurve.Ends[3]);
                CurveK cc1 = new CurveK(0, 2 * pi, (double t) => TestFuncAndCurve.U[0](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[0](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[0], TestFuncAndCurve.V[0], TestFuncAndCurve.T[0], TestFuncAndCurve.Ends[0]);
                CurveK cc2 = new CurveK(0, 3 * LMIN_RADIUS, (double t) => TestFuncAndCurve.U[1](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[1](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[1], TestFuncAndCurve.V[1], TestFuncAndCurve.T[1], TestFuncAndCurve.Ends[1]);
                CurveK cc3 = new CurveK(0, 4 * LMIN_RADIUS, (double t) => TestFuncAndCurve.U[2](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[2](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[2], TestFuncAndCurve.V[2], TestFuncAndCurve.T[2], TestFuncAndCurve.Ends[2]);
                CurveK cc4 = new CurveK(0, 1.5 * LMIN_RADIUS, (double t) => TestFuncAndCurve.U[3](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[3](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[3], TestFuncAndCurve.V[3], TestFuncAndCurve.T[3], TestFuncAndCurve.Ends[3]);
                switch (CIRCLE)
                {
                    case 1:
                        myCurve = cc1;
                        myCurveQ = c1;
                        break;
                    case 2:
                        myCurve = cc2;
                        myCurveQ = c2;
                        break;
                    case 3:
                        myCurve = cc3;
                        myCurveQ = c3;
                        break;
                    case 4:
                        myCurve = cc4;
                        myCurveQ = c4;
                        break;
                }
                MySLAU.curve = myCurve;
                MySLAUQ.curve = myCurveQ;
            }

            public static void CreateByCIRCLEllll()
            {
                CurveK cc1 = new CurveK(0, 2 * pi, (double t) => TestFuncAndCurve.U[0](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[0](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[0], TestFuncAndCurve.V[0], TestFuncAndCurve.T[0], TestFuncAndCurve.Ends[0]);
                CurveK cc2 = new CurveK(0, 3 * LMIN_RADIUS, (double t) => TestFuncAndCurve.U[1](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[1](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[1], TestFuncAndCurve.V[1], TestFuncAndCurve.T[1], TestFuncAndCurve.Ends[1]);
                CurveK cc3 = new CurveK(0, 4 * LMIN_RADIUS, (double t) => TestFuncAndCurve.U[2](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[2](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[2], TestFuncAndCurve.V[2], TestFuncAndCurve.T[2], TestFuncAndCurve.Ends[2]);
                CurveK cc4 = new CurveK(0, 1.5 * LMIN_RADIUS, (double t) => TestFuncAndCurve.U[3](t, LMIN_RADIUS), (double t) => TestFuncAndCurve.V[3](t, LMIN_RADIUS), LMIN_RADIUS, TestFuncAndCurve.U[3], TestFuncAndCurve.V[3], TestFuncAndCurve.T[3], TestFuncAndCurve.Ends[3]);
                switch (CIRCLE)
                {
                    case 1:
                        myCurve = cc1;
                        break;
                    case 2:
                        myCurve = cc2;
                        break;
                    case 3:
                        myCurve = cc3;
                        break;
                    case 4:
                        myCurve = cc4;
                        break;
                }
                MySLAU.curve = myCurve;
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
                    //double l = GetRandomEps() / Math.Sqrt(2);
                    //masPoints[i].x += l;
                    //masPoints[i].y += l;
                    masPoints[i] =new Point(masPoints[i]+ RandomNumbers.NextDouble2(-0.01, 0.01) * TestFuncAndCurve.Norm[CIRCLE - 1](masPoints[i]));
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
                for (int i = 1; i <= 5 * p; i++)
                {
                    int a = RandomNumbers.NextNumber() % N;//a.Show();
                    int b = RandomNumbers.NextNumber() % N;//b.Show();"".Show();
                    if (a != b) Expendator.Swap<Point>(ref masPoints[a], ref masPoints[b]);
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
            public static void WriteErrorDataInFile(double[] x, double EPS, double EPSQ)
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
                str = "Вектор решения и точность аппроксимации для функции " + d5 + " на кривой " + d6 + " методом " + d1 + " при числе функций " + d2 + $" Lradius = {LMIN_RADIUS} Qradius = {MIN_RADIUS}.txt";
                buf = Convert.ToString(str);

                StreamWriter fout = new StreamWriter(String.Concat(Program.Form1.DirectName, buf));//Console.SetOut(fout);MySLAU.Show();Console.SetOut(Console.Out);
                fout.WriteLine("Вектор решения (" + N + " точек):");
                for (int i = 0; i < N; i++)
                {
                    fout.WriteLine(x[i]);
                }
                fout.WriteLine("Получившаяся невязка при количестве функций " + N + " равна " + MySLAU.Nevaska); //вывести невязку
                fout.WriteLine("Разница приближённого решения с граничной функцией при числе базисных функций " + N + " равна " + EPS);
                Program.FORM.chart1.Titles[0].Text = $"Качество аппроксимации исходника: {EPS.ToString()}({EPSQ}) || Lradius = {LMIN_RADIUS} Qradius = {MIN_RADIUS}";
                Program.FORM.chart2.Titles[0].Text = $"Качество аппроксимации плотности: {Density().ToString()}({DensityQ().ToString()}) || Lradius = {LMIN_RADIUS} Qradius = {MIN_RADIUS}";
                RESULT = EPS;
                fout.Close();
            }

            public static void Error() //нахождение и вывод точности по известному решению
            {
                "Вход в Error".Show();
                double p = myCurve.Firstkind(N, N);
                double sum = 0;
                double[] Ax = new double[N];
                double pQ = myCurveQ.Firstkind(N, N);
                double sumQ = 0;
                double[] AxQ = new double[N];

                SLAU.Func_in_matrix.Matrix_power(ref Ax, KursMethods.MySLAU.A, KursMethods.MySLAU.x, N);
                SLAU.Func_in_matrix.Matrix_power(ref AxQ, KursMethods.MySLAUQ.A, KursMethods.MySLAUQ.x, N);

                for (int i = 0; i < N; i++)
                {
                    sum += KursMethods.MySLAU.x[i] * Ax[i]; //myCurve.Firstkind(i,N);
                    sumQ += KursMethods.MySLAUQ.x[i] * AxQ[i];
                }
                //p += sum;
                //sum = 0;
                //pQ += sumQ;
                //sumQ = 0;

                //for (int i = 0; i < N; i++)
                //    sum += KursMethods.MySLAU.x[i] * KursMethods.MySLAU.b[i];
                //for (int i = 0; i < N; i++)
                //    sumQ += KursMethods.MySLAUQ.x[i] * KursMethods.MySLAUQ.b[i];

                p -= /*2 **/ sum; pQ -= /*2 **/ sumQ;
                "Запись в файл".Show();
                WriteErrorDataInFile(KursMethods.MySLAU.x, Math.Sqrt(Math.Abs(p)), Math.Sqrt(Math.Abs(pQ)));

                "Ошибка записана".Show();
            }
            public static void Search(bool QsystemIsNeeded = true, SLAUpok SYSTEM = null, SLAUpok SYSTEMQ = null, bool parallel = false)
            {
                $"Система для функции {FuncName[GF - 1]} на кривой {CircleName[CIRCLE - 1]} начала заполняться".Show();
                if (SYSTEM != null)
                {
                    KursMethods.MySLAU.Make(N, SYSTEM.A); //создать систему порядка, равного числу базисных точек
                    KursMethods.MySLAUQ.Make(N, SYSTEMQ.A);
                }
                else
                {
                    KursMethods.MySLAU.Make(N); //создать систему порядка, равного числу базисных точек
                    KursMethods.MySLAUQ.Make(N);
                }


                //if (!parallel)
                if (SYSTEM == null)
                    Parallel.For(0, N, (int i) =>
                    {
                        //for (int i = 0; i < N; i++) //заполнить систему
                        //{
                        KursMethods.MySLAU.b[i] = myCurve.Firstkind(i, N);
                        KursMethods.MySLAU.A[i, i] = myCurve.Firstkind(i, i);

                        for (int j = i + 1; j < N; j++) //так как матрица симметрическая
                        {
                            double tmp = myCurve.Firstkind(i, j);
                            KursMethods.MySLAU.A[i, j] = tmp;
                            KursMethods.MySLAU.A[j, i] = tmp;
                        }
                        $"Система на L = {LMIN_RADIUS} ({(double)(i + 1) * (2 * N - i) / N / (N + 1) * 100}%)".Show();
                        MySLAU.Show();
                        "".Show();
                        //}
                    });
                else
                {
                    //KursMethods.MySLAU.A = SYSTEM.A;
                    for (int i = 0; i < N; i++) //заполнить систему                   
                        KursMethods.MySLAU.b[i] = myCurve.Firstkind(i, N);
                    "Система на L с новым вектором".Show();
                    MySLAU.Show();
                    "".Show();
                }
                //else
                //    Parallel.For(0, N, (int i) =>
                //    {
                //        Parallel.Invoke(
                //            () => { KursMethods.MySLAU.b[i] = myCurve.Firstkind(i, N); },
                //            () => { KursMethods.MySLAU.A[i, i] = myCurve.Firstkind(i, i); });
                //        Parallel.For(i + 1, N, (int j) =>
                //            {
                //                double tmp = myCurve.Firstkind(i, j);
                //                KursMethods.MySLAU.A[i, j] = tmp;
                //                KursMethods.MySLAU.A[j, i] = tmp;
                //            });

                //        "Система на кривой L".Show();
                //        MySLAU.Show();
                //        "".Show();
                //    });

                Method_des(N, baseMethod, MySLAU);
                new Vectors(MySLAU.ErrorMasP).Show();
                //MySLAU.Show();
                //"".Show();

                if (QsystemIsNeeded)
                    if (LMIN_RADIUS == MIN_RADIUS)
                        MySLAUQ = new SLAUpok(MySLAU, N);
                    else
                    {
                        //if (!parallel)
                        if (SYSTEMQ == null)
                            Parallel.For(0, N, (int i) =>
                            {
                                //for (int i = 0; i < N; i++) //заполнить систему
                                //{
                                //KursMethods.MySLAU.x[i] = 1;

                                KursMethods.MySLAUQ.b[i] = myCurveQ.Firstkind(i, N);
                                KursMethods.MySLAUQ.A[i, i] = myCurveQ.Firstkind(i, i);
                                //A[i, i] = KursMethods.MySLAU.A[i, i];
                                for (int j = i + 1; j < N; j++) //так как матрица симметрическая
                                {
                                    double tmp = myCurveQ.Firstkind(i, j);
                                    KursMethods.MySLAUQ.A[i, j] = tmp;
                                    KursMethods.MySLAUQ.A[j, i] = tmp;
                                    //A[i, j] = tmp;
                                    //A[j, i] = tmp;
                                }
                                $"Система на кривой Q ({(double)(i + 1) * (2 * N - i) / N / (N + 1) * 100}%)".Show();
                                MySLAUQ.Show();
                                "".Show();
                                //}
                            });
                        else
                        {

                            //KursMethods.MySLAUQ.A = SYSTEMQ.A;
                            for (int i = 0; i < N; i++) //заполнить систему                   
                                KursMethods.MySLAUQ.b[i] = myCurveQ.Firstkind(i, N);
                            "Система на Q с новым вектором".Show();
                            MySLAUQ.Show();
                            "".Show();
                        }
                        //else
                        //    Parallel.For(0, N, (int i) =>
                        //    {
                        //        Parallel.Invoke(
                        //            () => { KursMethods.MySLAUQ.b[i] = myCurveQ.Firstkind(i, N); },
                        //            () => { KursMethods.MySLAUQ.A[i, i] = myCurveQ.Firstkind(i, i); });
                        //        Parallel.For(i + 1, N, (int j) =>
                        //        {
                        //            double tmp = myCurveQ.Firstkind(i, j);
                        //            KursMethods.MySLAUQ.A[i, j] = tmp;
                        //            KursMethods.MySLAUQ.A[j, i] = tmp;
                        //        });

                        //        "Система на Q".Show();
                        //        MySLAUQ.Show();
                        //        "".Show();
                        //    });
                        Method_des(N, baseMethod, MySLAUQ);
                        new Vectors(MySLAUQ.ErrorMasP).Show();
                    }


                "Системы заполнены и решены".Show();
                //-----------------------------------------решить систему-----------------------------------

                "На L".Show();
                MySLAU.Show();
                "".Show();
                if (QsystemIsNeeded)
                {
                    "На Q".Show();
                    MySLAUQ.Show();
                    "".Show();
                }

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
        public static void Illustrating(bool parallel = true, int countpoint = 150)
        {
            double ep = /*0.01*/(myCurve.b - myCurve.a) / countpoint;

            Program.FORM.chart1.Series[0].Points.Clear();
            Program.FORM.chart1.Series[1].Points.Clear();
            Program.FORM.chart2.Series[0].Points.Clear();
            Program.FORM.chart2.Series[1].Points.Clear();

            Program.FORM.chart1.Series[0].Name = $"V(ρ={FuncName[GF - 1]}) на кривой {CircleName[CIRCLE - 1]}";
            Program.FORM.chart1.Series[0].Points.AddXY(myCurve.a, fi(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку
            Program.FORM.chart2.Series[0].Name = $"ρ = {FuncName[GF - 1]} на кривой {CircleName[CIRCLE - 1]}";
            Program.FORM.chart2.Series[0].Points.AddXY(myCurve.a, fig(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку

            List<double> mas = new List<double>(), mas2 = new List<double>();
            List<double> mas21 = new List<double>(), mas22 = new List<double>();
            int y = 0;
            for (double i = myCurve.a + ep; i <= myCurve.b - ep; i += ep)
            {
                mas.Add(fi(myCurve.Transfer(i)));
                mas2.Add(fig(myCurve.Transfer(i)));

                Program.FORM.chart2.Series[0].Points.AddXY(i, mas2[y]);
                Program.FORM.chart1.Series[0].Points.AddXY(i, mas[y++]);
            }
            // графики сумм m потенциальных функций
            Program.FORM.chart1.Series[1].Name = $"Σcω n = {N}";
            Program.FORM.chart2.Series[1].Name = $"Σcα n = {N}";
            Program.FORM.textBox1.Text += $"Начинается рисование графика для области {CircleName[CIRCLE - 1]} и граничной функции {FuncName[GF - 1]}";
            Program.FORM.textBox1.Text += Environment.NewLine;

            "Вычисляется график аппроксимации".Show();
            if (!parallel)
            {
                Program.FORM.chart1.Series[1].Points.AddXY(myCurve.a, Approx(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку
                Program.FORM.chart2.Series[1].Points.AddXY(myCurve.a, OldApprox(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку
                for (double j = myCurve.a + ep; j <= myCurve.b; j += ep)
                {
                    mas2.Add(OldApprox(myCurve.Transfer(j)));
                    mas.Add(Approx(myCurve.Transfer(j)));
                    Program.FORM.chart2.Series[1].Points.AddXY(j, mas2[y]);
                    Program.FORM.chart1.Series[1].Points.AddXY(j, mas[y++]);
                }

            }
            else
            {
                double[] l = new double[(int)Math.Ceiling((myCurve.b - myCurve.a) / ep + 1)], l2 = new double[l.Length];

                Parallel.For(0, l.Length, (int j) =>
                {
                    l[j] = Approx(myCurve.Transfer(myCurve.a + ep * (j)));
                    l2[j] = OldApprox(myCurve.Transfer(myCurve.a + ep * (j)));
                });
                int q = 0;
                for (double j = myCurve.a; j <= myCurve.b; j += ep)
                {
                    Program.FORM.chart2.Series[1].Points.AddXY(j, l2[q]);
                    Program.FORM.chart1.Series[1].Points.AddXY(j, l[q++]);
                }
                mas2.AddRange(l2);
                mas.AddRange(l);
            }

            double min = mas.Min(), max = mas.Max();
            if (min == 0) min = -5 * ep;
            if (max == 0) max = 5 * ep;
            Program.FORM.chart1.ChartAreas[0].AxisY.Minimum = (min > 0) ? min * (1 - ep) : min * (1 + ep);
            Program.FORM.chart1.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + ep) : max * (1 - ep);

            min = mas2.Min(); max = mas2.Max();
            if (min == 0) min = -5 * ep;
            if (max == 0) max = 5 * ep;
            Program.FORM.chart2.ChartAreas[0].AxisY.Minimum = (min > 0) ? min * (1 - ep) : min * (1 + ep);
            Program.FORM.chart2.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + ep) : max * (1 - ep);


            "График аппроксимации вычислен".Show();

            //запись в файл bmp
            string buf = new string(new char[150]);
            string str, str2, buf2;
            //memset(buf, 0, sizeof(sbyte));
            string d1 = Convert.ToString(GF);
            string d2 = Convert.ToString(CIRCLE);
            string d3 = Convert.ToString(N);
            str = bstr + sl + "График 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";
            str2 = bstr + sl + "График (плотности) 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";


            buf = Convert.ToString(str);
            buf2 = Convert.ToString(str2);
            Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
            Program.FORM.chart2.SaveImage(buf2, System.Drawing.Imaging.ImageFormat.Bmp);
            Program.FORM.chart1.SaveImage(adress + "График 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);
            Program.FORM.chart2.SaveImage(adress + "График 1 (плотности) граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);

            //List<Image> list = new List<Image>();
            //list.Add(Image.FromFile(buf2));list.Add(Image.FromFile(buf));
            string str3 = bstr + sl + "График 1 граничной функции (и плотности) (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";
            //ImageActions.MergerOfImages(Image.FromFile(buf2), Image.FromFile(buf)).Save(str3, System.Drawing.Imaging.ImageFormat.Bmp);

            //ImageActions.SaveRastAndVec(buf, buf2, str3);

            //ImageL.Images.Add(Image.FromFile(buf));
            //Program.FORM.pictureBox1.Image = ImageL.Images[ImageL.Images.Count - 1];
            //Program.FORM.pictureBox1.Refresh();

            if (CIRCLE == 1) For3D(adress + "", filt);
            else For3D2(adress + "", filt);

            StreamWriter st = new StreamWriter(adress + "info.txt");
            st.WriteLine($"Circle = {CircleName[CIRCLE - 1]}, func = {FuncName[GF - 1]}, Qr = {MIN_RADIUS}, count = {d3}.pdf");
            st.WriteLine($"Circle = {CircleName[CIRCLE - 1]}, func = {FuncName[GF - 1]}, Qr = {MIN_RADIUS}, count = {d3} (Diff).html");
            st.Close();

            Process.Start(adress + "Approx3D.R");
        }
        static ImageList ImageL = new ImageList();


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
                Directory.CreateDirectory(Program.Form1.DirectName + dir_Curve_name); //создать папку для кривой

                for (KursMethods.GF = 1; KursMethods.GF <= KGF; KursMethods.GF++)
                {
                    d5 = Convert.ToString(GF);
                    d6 = Convert.ToString(CIRCLE);
                    d7 = Convert.ToString(baseMethod);
                    str2 = "При граничной функции " + d5;
                    dir_func_name = str2;
                    bstr = Program.Form1.DirectName + str1 + sl + str2;
                    chstr = bstr;
                    Directory.CreateDirectory(chstr); //создать в ней папку для функции

                    //SLAUpok.VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);
                    //for(int i=2;i<minN;i++) Desigion(i, KursMethods.GF, KursMethods.CIRCLE);

                    for (int m = minN; m <= maxN; m += cu)
                    {
                        //SLAUpok.VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);
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
            Program.FORM.chart2.Series[0].Points.Clear();
            Program.FORM.chart2.Series[1].Points.Clear();

            //SLAUpok.VALUE_FOR_ULTRA = myCurve.Firstkind(fis); //?
            double[] Errors = new double[N]; //массив погрешностей
            double[] ErrorsD = new double[N];
            double[] ErrorsQ = new double[N];
            double[] ErrorsDQ = new double[N];
            ForFixity.Create(ref Errors, ref ErrorsD, ref ErrorsQ, ref ErrorsDQ); //заполнить массив ошибок
            ForFixity.WriteMassiv(Errors, ErrorsD); //вывести погрешности
            if (zero)
                ForFixity.Show(Errors, ErrorsD, ErrorsQ, ErrorsDQ); //нарисовать ломанную ошибок
        }

        public static void Fixity(SLAUpok.Method A, SLAUpok.Method B)
        { //график функции (число потенциалов)->(расстояние до граничной функции) для методов A и B

            Program.FORM.chart1.Series[0].Points.Clear();
            Program.FORM.chart1.Series[1].Points.Clear();

            //SLAUpok.VALUE_FOR_ULTRA = 10; //?
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
            public static void Show(double[] Errors, double[] ErrorsD, double[] ErrorsQ, double[] ErrorsDQ)
            {
                if (Array.IndexOf(Errors, 0) >= 0 || Array.IndexOf(ErrorsD, 0) >= 0 || Array.IndexOf(ErrorsQ, 0) >= 0 || Array.IndexOf(ErrorsDQ, 0) >= 0)
                    return;

                //Program.FORM.chart1.Series[0].Points.Clear();
                //Program.FORM.chart1.Series[1].Points.Clear();
                Program.FORM.chart1.Series[1].Name = "То же самое, но по границе Q";
                Program.FORM.chart2.Series[1].Name = "То же самое, но по границе Q";
                Program.FORM.chart1.Series[1].Color = Color.Blue;
                Program.FORM.chart2.Series[1].Color = Color.DarkBlue;
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
                Program.FORM.chart1.Series[0].Name = $"Качество аппроксимации функции {FuncName[GF - 1]} на области {CircleName[CIRCLE - 1]} в зависимости от числа базисных функций {N} (L {LMIN_RADIUS})";
                Program.FORM.chart2.Series[0].Color = Color.Green; // задаем цвет линии (красный)
                Program.FORM.chart2.Series[0].Points.AddXY(0, DoubleIntegral(fig, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY)); // устанавливаем курсор на точку
                Program.FORM.chart2.Series[0].Name = $"Качество аппроксимации плотности функции {FuncName[GF - 1]} на области {CircleName[CIRCLE - 1]} в зависимости от числа базисных функций {N} (L {LMIN_RADIUS})";

                Program.FORM.chart1.Series[1].Points.AddXY(0, myCurveQ.Firstkind(N, N)); // устанавливаем курсор на точку
                Program.FORM.chart1.Series[1].Name = $"Качество аппроксимации функции {FuncName[GF - 1]} на области {CircleName[CIRCLE - 1]} в зависимости от числа базисных функций {N} (Q {MIN_RADIUS})";
                Program.FORM.chart2.Series[1].Points.AddXY(0, DoubleIntegral(fig, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY)); // устанавливаем курсор на точку
                Program.FORM.chart2.Series[1].Name = $"Качество аппроксимации плотности функции {FuncName[GF - 1]} на области {CircleName[CIRCLE - 1]} в зависимости от числа базисных функций {N} (Q {MIN_RADIUS})";

                for (int i = 0; i < N; i++)
                {
                    Program.FORM.chart1.Series[0].Points.AddXY(i + 1, Math.Log10(Errors[i]));
                    Program.FORM.chart2.Series[0].Points.AddXY(i + 1, Math.Log10(ErrorsD[i]));
                    Program.FORM.chart1.Series[1].Points.AddXY(i + 1, Math.Log10(ErrorsQ[i]));
                    Program.FORM.chart2.Series[1].Points.AddXY(i + 1, Math.Log10(ErrorsDQ[i]));
                }



                string buf = new string(new char[250]);
                string str, str2, buf2, str0;
                //memset(buf, 0, sizeof(sbyte));
                string d2 = Convert.ToString(N);
                string d3 = Convert.ToString(GF);
                string d4 = Convert.ToString(CIRCLE);
                string d5 = Convert.ToString(GF);
                string d6 = Convert.ToString(baseMethod);
                //str = "График 2 качества аппроксимации в зависимости от числа базисных функций (" + d2 + ").bmp";
                //str0 = "График 2 качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") в зависимости от числа базисных функций (" + d2 + ")";
                str = "График 2 качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").bmp";
                str2 = "График 2 качества аппроксимации плотности функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").bmp";
                buf = Convert.ToString(str);
                buf2 = Convert.ToString(str2);

                //Directory.CreateDirectory(str0);

                //не было комента  Program.FORM.chart1.SaveImage(/*str0+"\\"+*/bstr + sl + buf, System.Drawing.Imaging.ImageFormat.Bmp);

                //не было комента  Program.FORM.chart2.SaveImage(/*str0 + "\\" + */bstr + sl + buf2, System.Drawing.Imaging.ImageFormat.Bmp);
                string str3 = bstr + sl + "График 2 качества аппроксимации функции (" + d3 + ") (и её плотности) на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").bmp";
                //ImageActions.MergerOfImages(Image.FromFile(bstr + sl + buf2), Image.FromFile(bstr + sl + buf)).Save(str3, System.Drawing.Imaging.ImageFormat.Bmp);

                //не было комента   ImageActions.SaveRastAndVec(bstr + sl + buf, bstr + sl + buf2, str3);

                Program.FORM.chart1.SaveImage(adress + "График 2 граничной функции (" + d3 + ") и её приближения на кривой (" + d4 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);
                Program.FORM.chart2.SaveImage(adress + "График 2 (плотности) граничной функции (" + d3 + ") и её приближения на кривой (" + d4 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);
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

                Program.FORM.chart1.Series[0].Name = $"Аппроксимация функции {FuncName[GF - 1]} на области {CircleName[CIRCLE - 1]} методом {A} при числе точек {N}";
                Program.FORM.chart1.Series[0].Color = Color.Red; // задаем цвет линии (красный)
                Program.FORM.chart1.Series[0].Points.AddXY(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку

                for (int i = 0; i < N; i++)
                {
                    Program.FORM.chart1.Series[0].Points.AddXY(i + 1, Math.Log10(ErrorsA[i])); //cout+i+" "+Errors[i]+endl;
                }

                //SetColor(0, 250, 0); // задаем цвет линии (зеленый)
                Program.FORM.chart1.Series[1].Name = $"Аппроксимация функции {FuncName[GF - 1]} на области {CircleName[CIRCLE - 1]} методом {B} при числе точек {N}";
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
            public static double ErrorQ(int k) //частичная погрешность
            {
                double p = myCurveQ.Firstkind(N, N);
                double sum = 0;

                double[] Ax = new double[N];
                SLAUpok.Func_in_matrix.Matrix_power(ref Ax, KursMethods.MySLAUQ.A, KursMethods.MySLAUQ.x, k);
                for (int i = 0; i < k; i++)
                {
                    sum += KursMethods.MySLAUQ.x[i] * Ax[i];
                }
                double EPS = Math.Abs(p - sum);
                return Math.Sqrt(EPS);

            }

            public static void Create(ref double[] Errors, ref double[] ErrorsD, ref double[] ErrorsQ, ref double[] ErrorsDQ, bool parallel = false)
            {
                Program.FORM.textBox1.Text += "Для графика в зависимости от числа точек:";
                Program.FORM.textBox1.Text += Environment.NewLine;
                if (!parallel)
                {
                    //Method_des(N, baseMethod, MySLAU);
                    //Method_des(N, baseMethod, MySLAUQ);
                    Errors = MySLAU.ErrorsMas/*Error(i)*/; //заполнить массив погрешностей
                    ErrorsQ = MySLAUQ.ErrorsMas/*ErrorQ(i)*/; //заполнить массив погрешностей
                    ErrorsD = MySLAU.ErrorMasP;
                    ErrorsDQ = MySLAUQ.ErrorMasP;
                    for (int i = 0; i < N; i++)
                    {
                        //Functional f = (Point x) =>
                        //{
                        //    double sum = 0;

                        //    for (int ii = 1; ii <= i; ii++)
                        //    {
                        //        sum += MySLAU.x[ii - 1] * masPoints[ii - 1].PotentialF((Point)x);
                        //    }
                        //    double s = sum - fig(x);
                        //    return s * s;
                        //};
                        //ErrorsD[i] = DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);
                        //f = (Point x) =>
                        //   {
                        //       double sum = 0;

                        //       for (int ii = 1; ii <= i; ii++)
                        //       {
                        //           sum += MySLAUQ.x[ii - 1] * masPoints[ii - 1].PotentialF((Point)x);
                        //       }
                        //       double s = sum - fig(x);
                        //       return s * s;
                        //   };
                        //ErrorsDQ[i] = DoubleIntegral(f, myCurveQ, myCurveQ.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);

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
                    //(new Vectors(Errors)).Show();
                    //(new Vectors(ErrorsQ)).Show();
                }
                //else
                ////но при поиске наилучшего алгоритма нельзя делать распараллеливание в этом месте
                //Parallel.For(0, N, (int i) =>
                //{
                //    Method_des(i, baseMethod, MySLAU); //решить частичную систему нужным методом
                //    Errors[i] = Error(i); //заполнить массив погрешностей
                //});

                //Errors[N - 1] = RESULT;
            }

            public static void Create(double[] ErrorsA, double[] ErrorsB, SLAUpok.Method A, SLAUpok.Method B)
            {
                Program.FORM.textBox1.Text += "Для графика в зависимости от числа точек:";
                Program.FORM.textBox1.Text += Environment.NewLine;
                for (int i = 0; i < N; i++)
                {
                    Method_des(i, A, MySLAU); //решить частичную систему нужным методом
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
                    Method_des(i, B, MySLAU); //решить частичную систему нужным методом
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
                string d3 = Convert.ToString(GF);
                string d4 = Convert.ToString(CIRCLE);
                string d6 = Convert.ToString(meth);
                str = "Сообщение об ошибке для графика 2 погрешностей (без log) качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").txt";
                buf = Convert.ToString(str);
                StreamWriter fout = new StreamWriter(String.Concat(Program.Form1.DirectName, buf));
                fout.WriteLine("Невозможно построить логарифмический график, поскольку на элементе " + num + "(=0) функция принимает значение -infinity");
                fout.Close();
            }

            public static void WriteMassiv(double[] x, double[] y) //вывод массива погрешностей
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
                StreamWriter fout = new StreamWriter(String.Concat(Program.Form1.DirectName, buf));

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
                if (y.Where(r => r == 0).Count() > 0)
                    zero = true;
                Program.FORM.chart2.Titles[0].Text = "Наилучшая аппроксимация равна " + y.Min() + " на элементе " + (Array.IndexOf(y, y.Min()) + 1);

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
                Directory.CreateDirectory(Program.Form1.DirectName + dir_Curve_name); //создать папку для кривой

                if (minN != maxN)
                    for (KursMethods.GF = 1; KursMethods.GF <= KGF; KursMethods.GF++)
                    {
                        d5 = Convert.ToString(GF);
                        d6 = Convert.ToString(CIRCLE);
                        d7 = Convert.ToString(baseMethod);
                        str2 = "При граничной функции " + d5;
                        dir_func_name = str2;
                        bstr = Program.Form1.DirectName + str1 + sl + str2;
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
                else
                {
                    KursMethods.GF = 1;
                    int m = minN;
                    d5 = Convert.ToString(GF);
                    d6 = Convert.ToString(CIRCLE);
                    d7 = Convert.ToString(baseMethod);
                    str2 = "При граничной функции " + d5;
                    dir_func_name = str2;
                    bstr = Program.Form1.DirectName + str1 + sl + str2;
                    chstr = bstr;
                    Directory.CreateDirectory(chstr); //создать в ней папку для функции
                    Desigion(m, KursMethods.GF, KursMethods.CIRCLE); //заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
                    Fixity(); //график зависимости погрешности аппроксимации от числа базисных точек
                    Program.FORM.chart1.Refresh();
                    for (KursMethods.GF = 2; KursMethods.GF <= KGF; KursMethods.GF++)
                    {
                        d5 = Convert.ToString(GF);
                        d6 = Convert.ToString(CIRCLE);
                        d7 = Convert.ToString(baseMethod);
                        str2 = "При граничной функции " + d5;
                        dir_func_name = str2;
                        bstr = Program.Form1.DirectName + str1 + sl + str2;
                        chstr = bstr;
                        Directory.CreateDirectory(chstr); //создать в ней папку для функции
                        Desigion(m, KursMethods.GF, KursMethods.CIRCLE, true, new SLAUpok(MySLAU), new SLAUpok(MySLAUQ)); //заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
                        "Взята старая система с новым свободным вектором".Show();
                        Fixity(); //график зависимости погрешности аппроксимации от числа базисных точек
                        Program.FORM.chart1.Refresh();
                    }
                }
            }
        }

        //-----------------------------------------------------------


        public static bool ScriptEnded = true;
        public static void LnGraf(int s, int g, int cu, bool QsystemISNeeded = false)
        {
            while (!ScriptEnded) { }

            double[] ErrorsD = new double[s], Errors = new double[s], ErrorsQ = new double[s], ErrorsDQ = new double[s];
            double tmp = LMIN_RADIUS;
            double EPSs = (LMAX_RADIUS - LMIN_RADIUS) / (s - 1);
            int i = 0;

            StreamWriter arg = new StreamWriter("arg2.txt");
            StreamWriter info = new StreamWriter("info2.txt");
            string st = $"s = {s} g = {FuncName[g - 1]} cu = {CircleName[cu - 1]}";
            info.WriteLine(st + ".pdf");
            info.WriteLine(st + "(density).html");
            info.WriteLine(st + "(potential).html");
            info.Close();
            for (int j = 0; j < s; j++)
            {
                arg.WriteLine($"{j + 1} {LMIN_RADIUS + j * EPSs}");
            }
            arg.Close();

            StreamWriter val = new StreamWriter("val2.txt");
            StreamWriter val2 = new StreamWriter("val2l.txt");
            for (; LMIN_RADIUS <= LMAX_RADIUS; LMIN_RADIUS += EPSs)
            {
                Desigion(s, g, cu, QsystemISNeeded);
                ForFixity.Create(ref Errors, ref ErrorsD, ref ErrorsQ, ref ErrorsDQ);
                for (int k = 1; k <= s; k++)
                {
                    if (ErrorsD[k - 1] == 0.0)
                        val.WriteLine("NA");
                    else
                        val.WriteLine(ErrorsD[k - 1]);

                    if (Errors[k - 1] == 0.0)
                        val2.WriteLine("NA");
                    else
                        val2.WriteLine(Errors[k - 1]);
                }
            }

            val.Close();
            val2.Close();
            LMIN_RADIUS = tmp;

            StartProcess("LastGrafic.R");
            // Process.Start("LastGrafic.R");
        }
        public static void StartProcess(string fileName)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.EnableRaisingEvents = true;

            process.Exited += (sender, e) =>
            {
                Console.WriteLine($"Процесс завершен с кодом {process.ExitCode}");
                ScriptEnded = true;
            };
            ScriptEnded = false;
            process.Start();
        }

        /// <summary>
        /// График зависимости качества аппроксимации от радиуса при s функциях и d кривых
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <param name="g"></param>
        /// <param name="cu"></param>
        public static void Quality(int s, int d, int g, int cu, bool QsystemISNeeded = false)
        {
            //------------------------------обнуление данных
            Program.FORM.chart1.Series.Clear();
            Program.FORM.chart1.Series.Add("   ");
            Program.FORM.chart1.Series.Add("  ");
            Program.FORM.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart1.Titles[0].Text = "Кривые, на которых происходит проверка";

            Program.FORM.chart2.Series.Clear();
            Program.FORM.chart2.Series.Add("   ");
            Program.FORM.chart2.Series.Add("  ");
            Program.FORM.chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Program.FORM.chart2.Titles[0].Text = "Кривые, на которых происходит проверка";

            double[] Errors = new double[d]; //массив погрешностей
            double[] ErrorsD = new double[d];
            double EPSs = (LMAX_RADIUS - LMIN_RADIUS) / d;
            Program.FORM.textBox1.Text += $"Для графика в зависимости от радиуса на кривой {cu} при граничной функции {g}:";
            Program.FORM.textBox1.Text += Environment.NewLine;
            int i = 0;
            double tmp = LMIN_RADIUS;
            bool isred = false; //наличие набора с чистым нулём

            //заполнение массива
            for (; LMIN_RADIUS <= LMAX_RADIUS; LMIN_RADIUS += EPSs)
            {
                //SLAUpok.VALUE_FOR_ULTRA = KursMethods.myCurve.Firstkind(KursMethods.fis);
                if ((g != 0) && (cu != 0))
                {
                    Desigion(s, g, cu, QsystemISNeeded);
                }
                else
                {
                    Desigion(s);
                }
                Errors[i] = ForFixity.ReturnError();
                ErrorsD[i] = Density();

                //Color color = Color.Green;
                //ForQuality.Draw_CIRCLE(LMIN_RADIUS, Color.Green); //нарисовать окружность
                if (Errors[i] == 0)
                {
                    Program.FORM.textBox1.Text += "Аппроксимация в машинный ноль на элементе " + (i + 1) + " (радиус " + LMIN_RADIUS + " )" + Environment.NewLine;
                    ForQuality.Draw_CIRCLE(LMIN_RADIUS, Color.Red, Program.FORM.chart1);
                    isred = true;
                }
                else ForQuality.Draw_CIRCLE(LMIN_RADIUS, Color.Green, Program.FORM.chart1);
                if (ErrorsD[i] == 0)
                {
                    Program.FORM.textBox1.Text += "Аппроксимация в машинный ноль на элементе " + (i + 1) + " (радиус " + LMIN_RADIUS + " )" + Environment.NewLine;
                    ForQuality.Draw_CIRCLE(LMIN_RADIUS, Color.Red, Program.FORM.chart2);
                    isred = true;
                }
                else ForQuality.Draw_CIRCLE(LMIN_RADIUS, Color.Green, Program.FORM.chart2);

                ForQuality.Draw_mas(masPoints, Color.Green); //нарисовать точки массива masPoints

                if (i % 10 == 0)
                {
                    Program.FORM.textBox1.Text += "Расстояние при радиусе области ";
                    Program.FORM.textBox1.Text += LMIN_RADIUS;
                    Program.FORM.textBox1.Text += " и радиусе кривой ";
                    Program.FORM.textBox1.Text += LMAX_RADIUS;
                    Program.FORM.textBox1.Text += " равно ";
                    Program.FORM.textBox1.Text += Errors[i];
                    Program.FORM.textBox1.Text += Environment.NewLine;
                    Program.FORM.textBox1.Refresh();
                    Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();
                }
                i++;
                if (i == d) break;
            }
            LMIN_RADIUS = tmp;
            ForQuality.Draw_CIRCLE(LMIN_RADIUS, Color.Gold); //нарисовать краcным изначальную окружность (возможно, надо внутри него для двух чартов ставить)

            Program.FORM.chart1.Refresh(); Program.FORM.chart2.Refresh();
            Program.FORM.textBox1.Text += "------Массив данных записан";
            Program.FORM.textBox1.Text += Environment.NewLine;
            Program.FORM.textBox1.Refresh();
            Program.FORM.textBox1.SelectionStart = Program.FORM.textBox1.Text.Length; Program.FORM.textBox1.ScrollToCaret();

            //записать в файл массив и провести анализ
            string buf = new string(new char[250]), buf1, buf2;
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
            Program.FORM.chart1.Titles[0].Text = "Наилучшая аппроксимация равна " + (min) + " и достигается при радиусе " + (LMIN_RADIUS + (mini + 1) * EPSs);
            if (mins > 0)
            {
                fout.WriteLine("3)Первый скачок " + mins + " с элементa " + mins1 + " на элемент " + mins2);
                fout.WriteLine("4)Максимальный скачок " + maxs + " с элементa " + maxs1 + " на элемент " + maxs2);
            }
            if (!zero)
            {
                fout.Write("Невозможно построить логарифмический график, поскольку на элементе " + j + 1 + " (радиус " + LMIN_RADIUS + j * EPSs + ") функция принимает значение " + Math.Log10(Errors[j]));
            }
            fout.Close();

            //if (isred == zero) throw new Exception("Есть нули, и нет нулей");

            Errors = Errors.Where(x => (x != Double.NaN) && (x > 0)).ToArray();

            //если есть набор с чистым нулём, показать его на графике
            if (isred)
            {
                Program.FORM.chart1.Series.Add($"{s} потенциальных функций...");
                Program.FORM.chart1.Series.Add($"...при граничной функции {g}");
                str = "График 3.2 кривых и базисных потенциалов методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                buf1 = Convert.ToString(buf);

                Program.FORM.chart1.SaveImage(adress + "График 3.2 кривых и базисных потенциалов методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.emf", System.Drawing.Imaging.ImageFormat.Emf);
                //Program.FORM.chart2.SaveImage(adress + "График 2 (плотности) граничной функции (" + d3 + ") и её приближения на кривой (" + d4 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);
            }
            else //нарисовать график, если нет нулей
            {
                //------------------------------обнуление данных
                Program.FORM.chart1.Series.Clear();
                Program.FORM.chart1.Series.Add($" ");
                Program.FORM.chart1.Series.Add($"  ");
                Program.FORM.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                //Program.FORM.chart1.Titles[0].Text = "Кривые, на которых происходит проверка";
                Program.FORM.chart1.Titles[0].Text = "Наилучшая аппроксимация равна " + (min) + " и достигается при радиусе " + (LMIN_RADIUS + (mini + 1) * EPSs);
                //Рисование графика зависимости
                Program.FORM.chart1.Series[0].Name = "Качество аппроксимации функции " + g + " в зависимости от радиуса, (" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ")";

                Program.FORM.chart1.Series[0].Points.Clear();
                Program.FORM.chart1.Series[0].Color = Color.Red; // задаем цвет линии (красный)
                Program.FORM.chart1.Series[0].Points.AddXY(LMIN_RADIUS + EPSs, Math.Log10(Errors[0])); // устанавливаем курсор на точку
                for (int dis = 0; dis < d; dis++)
                    Program.FORM.chart1.Series[0].Points.AddXY(LMIN_RADIUS + (dis + 1) * EPSs, Math.Log10(Errors[dis])); //рисуем ломанную

                Program.FORM.chart1.Refresh();
                str = "График 3.1 качества аппроксимации методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart1.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                buf1 = Convert.ToString(buf);
            }


            isred = ErrorsD.Where(x => x <= 0 || x == Double.NaN).Count() != 0;
            ErrorsD = ErrorsD.Where(x => !(x != x) && (x > 0)).ToArray();
            if (isred)
            {
                Program.FORM.chart2.Series.Add($"{s} потенциальных функций...");
                Program.FORM.chart2.Series.Add($"...при граничной функции {g}");
                str = "График 3.2 аппроксимации плотности методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart2.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                buf2 = Convert.ToString(buf);
            }
            else //нарисовать график, если нет нулей
            {
                //------------------------------обнуление данных
                Program.FORM.chart2.Series.Clear();
                Program.FORM.chart2.Series.Add($" ");
                Program.FORM.chart2.Series.Add($"  ");
                Program.FORM.chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Program.FORM.chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                //Program.FORM.chart1.Titles[0].Text = "Кривые, на которых происходит проверка";
                min = ErrorsD.Min();
                mini = Array.IndexOf(ErrorsD, min);
                Program.FORM.chart2.Titles[0].Text = "Наилучшая аппроксимация равна " + (min) + " и достигается при радиусе " + (LMIN_RADIUS + (mini + 1) * EPSs);
                //Рисование графика зависимости
                Program.FORM.chart2.Series[0].Name = "Качество аппроксимации плотности функции " + g + " в зависимости от радиуса, (" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ")";

                Program.FORM.chart2.Series[0].Points.Clear();
                Program.FORM.chart2.Series[0].Color = Color.Green; // задаем цвет линии (красный)
                Program.FORM.chart2.Series[0].Points.AddXY(LMIN_RADIUS + EPSs, Math.Log10(ErrorsD[0])); // устанавливаем курсор на точку
                for (int dis = 0; dis < d; dis++)
                    Program.FORM.chart2.Series[0].Points.AddXY(LMIN_RADIUS + (dis + 1) * EPSs, Math.Log10(ErrorsD[dis])); //рисуем ломанную

                Program.FORM.chart2.Refresh();
                str = "График 3.1 качества аппроксимации плотности методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
                newstr = bstr + sl + str; //полный адрес
                buf = Convert.ToString(newstr);
                Program.FORM.chart2.SaveImage(buf, System.Drawing.Imaging.ImageFormat.Bmp);
                buf2 = Convert.ToString(buf);
            }
            string str3 = bstr + sl + "График 3 аппроксимации функции и её плотности методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.bmp";
            //ImageActions.MergerOfImages(Image.FromFile(buf2), Image.FromFile(buf1)).Save(str3, System.Drawing.Imaging.ImageFormat.Bmp);
            //ImageActions.SaveRastAndVec(buf1, buf2, str3);

            Program.FORM.chart1.SaveImage(adress + "График 3 кривых и базисных потенциалов методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций.emf", System.Drawing.Imaging.ImageFormat.Emf);
            Program.FORM.chart2.SaveImage(adress + "График 3 (плотности) граничной функции (" + d3 + ") и её приближения на кривой (" + d4 + ").emf", System.Drawing.Imaging.ImageFormat.Emf);
        }

        public class ForQuality
        {
            public static void Draw_CIRCLE(double radius, Color color, System.Windows.Forms.DataVisualization.Charting.Chart chart) //нарисовать окружность
            {
                //SetColor(R, G, B); // задаем цвет линии
                double d = radius - MIN_RADIUS;
                int count = 0;

                if (d > 0)
                {
                    chart.Series.Add("");
                    count = chart.Series.Count - 1;
                    chart.Series[count].Color = color;
                    chart.Series[count].IsVisibleInLegend = false;
                    chart.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }
                else chart.Series[count].Color = color;

                double ep = 0.01;
                CurveK c3 = new CurveK(0, 3 * radius, TestFuncAndCurve.u2h, TestFuncAndCurve.v2h);
                switch (CIRCLE)
                {
                    case 1:
                        chart.Series[count].Points.AddXY(radius, 0); // устанавливаем курсор на точку
                        for (double i = ep; i <= 2 * pi; i += ep)
                        {
                            chart.Series[count].Points.AddXY(radius * Math.Cos(i), radius * Math.Sin(i)); //рисуем CIRCLE
                        }
                        break;
                    case 2: //треугольник
                        chart.Series[count].Points.AddXY(-0.5 * d, -0.5 * d / Math.Sqrt(3));
                        chart.Series[count].Points.AddXY(-0.5 * d + radius, -0.5 * d / Math.Sqrt(3));
                        chart.Series[count].Points.AddXY(-0.5 * d + 0.5 * radius, -0.5 * d / Math.Sqrt(3) + 0.5 * radius * Math.Sqrt(3));
                        chart.Series[count].Points.AddXY(-0.5 * d, -0.5 * d / Math.Sqrt(3));
                        //chart.Series[0].Points.AddXY(c3.Transfer(radius).x, c3.Transfer(radius).y);
                        //chart.Series[0].Points.AddXY(c3.Transfer(2*radius).x, c3.Transfer(2*radius).y);
                        //chart.Series[0].Points.AddXY(c3.Transfer(3*radius).x, c3.Transfer(3*radius).y);
                        break;
                    case 3: //квадрат
                        chart.Series[count].Points.AddXY(-0.5 * d + 0, -0.5 * d + 0);
                        chart.Series[count].Points.AddXY(-0.5 * d, -0.5 * d + radius);
                        chart.Series[count].Points.AddXY(-0.5 * d + radius, -0.5 * d + radius);
                        chart.Series[count].Points.AddXY(-0.5 * d + radius, -0.5 * d + 0);
                        chart.Series[count].Points.AddXY(-0.5 * d + 0, -0.5 * d + 0);
                        break;
                    case 4: //острие
                        chart.Series[count].Points.AddXY(-0.5 * d + 0, -0.25 * d * Math.Sqrt(3) + 0);
                        chart.Series[count].Points.AddXY(-0.5 * d + radius, -0.25 * d * Math.Sqrt(3) + 0);

                        for (double i = ep; i <= pi / 3; i += ep)
                        {
                            chart.Series[count].Points.AddXY(radius * Math.Cos(i) - 0.5 * d, radius * Math.Sin(i) - 0.25 * d * Math.Sqrt(3)); //рисуем CIRCLE
                        }
                        for (double i = 2 * pi / 3; i <= pi; i += ep)
                        {
                            chart.Series[count].Points.AddXY(radius * Math.Cos(i) - 0.5 * d + radius, radius * Math.Sin(i) - 0.25 * d * Math.Sqrt(3)); //рисуем CIRCLE
                        }
                        break;
                }
            }
            public static void Draw_CIRCLE(double radius, Color color)
            {
                Draw_CIRCLE(radius, color, Program.FORM.chart1);
                Draw_CIRCLE(radius, color, Program.FORM.chart2);
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
                for (int i = 0; i < N; i++)
                {
                    Program.FORM.chart2.Series.Add("");
                    int count = Program.FORM.chart2.Series.Count - 1;
                    Program.FORM.chart2.Series[count].Color = color;

                    Program.FORM.chart2.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    Program.FORM.chart2.Series[count].IsVisibleInLegend = false;

                    Program.FORM.chart2.Series[count].Points.AddXY(r[i].x - e, r[i].y - e); // устанавливаем курсор на точку
                    Program.FORM.chart2.Series[count].Points.AddXY(r[i].x + e, r[i].y + e); //рисуем line
                    Program.FORM.chart2.Series[count].Points.AddXY(r[i].x - e, r[i].y + e); // устанавливаем курсор на точку
                    Program.FORM.chart2.Series[count].Points.AddXY(r[i].x + e, r[i].y - e); //рисуем line
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
                Directory.CreateDirectory(Program.Form1.DirectName + dir_Curve_name); //создать папку для кривой

                for (GF = 1; GF <= KGF; GF++)
                {
                    d5 = Convert.ToString(GF);
                    d6 = Convert.ToString(CIRCLE);
                    d7 = Convert.ToString(baseMethod);
                    str2 = "При граничной функции " + d5;
                    dir_func_name = str2;
                    bstr = Program.Form1.DirectName + str1 + sl + str2;
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
       
    }
}
