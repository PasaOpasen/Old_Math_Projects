using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using static МатКлассы2018.Number;
using static МатКлассы2018.FuncMethods;
using PointV = System.Tuple<double, МатКлассы2018.Vectors>;
using VectorNetFunc = System.Collections.Generic.List<System.Tuple<double, МатКлассы2018.Vectors>>;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

/// <summary>
/// Библиотека математических классов, написанная Опасным Пасей (Дмитрией Пасько/Деметрием Паскалём).
/// Начал писать примерно в конце февраля 2018-го, с класса полиномов.
/// С конца января 2019-го пишу продолжение библиотеки (МатКлассы2019),
/// в текущую библиотеку иногда добавляю новый функционал.
/// Сильные стороны библиотеки: классы комплексный чисел, векторов, полиномов,
/// матриц, методов интегрирования, графов (особое внимание), СЛАУ, методы расширения
/// Недостатки: мало где заботился об исключениях, содержимое методов почти не комментрируется,
/// в классе СЛАУ из-за диплома, вышедшего с С++, есть слишком сложные низкоуровневые методы
/// и путаница из-за тесной связи с классом кривых, 
/// класс вероятностей начал из эксперимента и почти ничего не написал,
/// очень много открытых полей и методов,
/// почти не проводил тестирование,
/// но большинство методов использовались в визуальных приложениях
/// и так были отлажены
/// Всё написал сам, кроме 3-5% кода, взятого из открытых источников
/// 
/// ------------Контакты:
/// Telegram: 8 961 519 36 46 (на звонки не отвечаю)
/// Mail:     qtckpuhdsa@gmail.com
/// Discord:  Пася Опасен#3065
/// VK:       https://vk.com/roman_disease
/// Steam:    https://vk.com/away.php?to=https%3A%2F%2Fsteamcommunity.com%2Fid%2FPasaOpasen&cc_key=
///      Активно пользуюсь всеми указанными сервисами
/// </summary>
namespace МатКлассы2018
{
    #region Делегаты и перечисления
    /// <summary>
    /// Действительные функции действительного аргумента
    /// </summary>
    /// <param name="x">Аргумент - действительное число</param>
    /// <returns></returns>
    public delegate double RealFunc(double x);
    /// <summary>
    /// Комплексная функция комплексного аргумента
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate Complex ComplexFunc(Complex x);
    /// <summary>
    /// Действительные функции от точки
    /// </summary>
    /// <param name="x">Аргумент - пара действительных чисел (x,y), реализованная как точка Point</param>
    /// <returns></returns>
    public delegate double Functional(Point x);
    /// <summary>
    /// Действительная функция двух переменных
    /// </summary>
    /// <param name="u"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate double DRealFunc(double x, double u);
    /// <summary>
    /// Комплекснозначная функция двух действительных переменных
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public delegate Complex DComplexFunc(double x, double z);
    /// <summary>
    /// Комплексная функция двух переменных
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public delegate Complex DoubleComplexFunc(Complex a, Complex b);
    /// <summary>
    /// Вектор-функция от вектора и параметра
    /// </summary>
    /// <param name="x"></param>
    /// <param name="u"></param>
    /// <returns></returns>
    public delegate Vectors VRealFunc(double x, Vectors u);
    /// <summary>
    /// Вектор-функция
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate Vectors VectorFunc(double x);
    /// <summary>
    /// Функция из Rn в Rn
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public delegate Vectors VectorToVector(Vectors v);
    /// <summary>
    /// Функция двух векторов, выдающая вектор
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public delegate Vectors TwoVectorToVector(Vectors a, Vectors b);
    /// <summary>
    /// Действительная функция векторного аргумента
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public delegate double AntiVectorFunc(Vectors v);
    /// <summary>
    /// Действительная функция комплексного переменного
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public delegate double RealFuncOfCompArg(Complex z);
    /// <summary>
    /// Действительная функция трёх аргументов, необходимая для вычисления площади сегментов с параметрами tx, ty при радиусе кривой r
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public delegate double TripleFunc(double tx, double ty, double r);
    /// <summary>
    /// Функция многих аргументов
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate double MultiFunc(params double[] x);
    /// <summary>
    /// Действительная функция из какой-то системы функций
    /// </summary>
    /// <param name="x">Аргумент</param>
    /// <param name="k">Номер функции в системе</param>
    /// <returns></returns>
    public delegate double SequenceFunc(double x, int k);
    /// <summary>
    /// Действительная функция от точки из системы функций
    /// </summary>
    /// <param name="z"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public delegate double SeqPointFunc(Point z, int k);
    /// <summary>
    /// Полином из системы полиномов
    /// </summary>
    /// <param name="k"></param>
    /// <returns></returns>
    public delegate Polynom SequencePol(int k);
    /// <summary>
    /// Функция, возвращающая точку в зависимости от параметра
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public delegate Point PointFunc(double t);
    /// <summary>
    /// Функция, возвращающая точку в зависимости от двух параметров
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public delegate Point DPointFunc(double t, double r);

    /// <summary>
    /// Перечисление "род": для криволинейных интегралов, полиномов Чебышёва и т.д.
    /// </summary>
    public enum Kind { FirstKind, SecondKind };
    /// <summary>
    /// Ортогональные функции, ортонормированные, неортогональные
    /// </summary>
    public enum SequenceFuncKind { Orthogonal, Orthonormal, Other };
    #endregion

    /// <summary>
    /// Класс для расширения всяких методов
    /// </summary>
    public static partial class Expendator
    {
        /// <summary>
        /// Перевести действительную функцию комплексного переменного в функционал
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Functional ToFunctional(RealFuncOfCompArg f)
        {
            return (Point z) =>
            {
                return f(new Complex(z.x, z.y));
            };
        }
        /// <summary>
        /// Перевести функционал в действительную функцию комплексного переменного
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static RealFuncOfCompArg ToRealFuncOfCompArg(Functional f)
        {
            return (Complex z) =>
            {
                return f(new Point(z.Re, z.Im));
            };
        }
        /// <summary>
        /// Перевести функционал в функцию комплесного переменного
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ComplexFunc ToCompFunc(Functional f)
        {
            return (Complex z) => f(new Point(z.Re, z.Im));
        }
        /// <summary>
        /// Преобразовать действительную функцию комплексного переменного в комплексную функцию
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ComplexFunc ToCompFunc(RealFuncOfCompArg f) => (Complex z) => f(z);

        /// <summary>
        /// Минимальное из кучи
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double Min(params double[] c)
        {
            double min = Math.Min(c[0], c[1]);
            for (int i = 2; i < c.Length; i++) min = Math.Min(min, c[i]);
            return min;
        }

        /// <summary>
        /// Максимальное из кучи
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double Max(params double[] c)
        {
            double max = Math.Max(c[0], c[1]);
            for (int i = 2; i < c.Length; i++) max = Math.Max(max, c[i]);
            return max;
        }
        /// <summary>
        /// Максимальное из кучи
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int Max(params int[] c)
        {
            int max = Math.Max(c[0], c[1]);
            for (int i = 2; i < c.Length; i++) max = Math.Max(max, c[i]);
            return max;
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static double Min(MultiFunc F)
        {
            return -1;
        }

        public static double Max(MultiFunc F)
        {
            MultiFunc e = (double[] x) => -F(x);
            return -Min(e);
        }

        /// <summary>
        /// Вывести число на консоль
        /// </summary>
        /// <param name="i"></param>
        public static void Show<T>(this T i) => Console.WriteLine(i.ToString());

        public static void Show<T>(this T[] t)
        {
            for (int i = 0; i < t.Length; i++)
                Console.WriteLine(Convert.ToString(t[i]));
        }

        public static double Sum(SequenceFunc f, double x, int N)
        {
            double sum = 0;
            for (int i = 0; i < N; i++)
                sum += f(x, i);
            return sum;
        }

        public static double Abs(this double x) => Math.Abs(x);

        /// <summary>
        /// Более точное среднее арифметическое для двух чисел
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex Average(Complex a, Complex b)
        {
            if (Math.Sign(a.Re) == Math.Sign(b.Re) && Math.Sign(a.Im) == Math.Sign(b.Im))
                return a + (b - a) / 2;
            return (a + b) / 2;
        }
        /// <summary>
        /// Переводит строку в действительное число через конвертер
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ToDouble(this string s)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch (Exception e)
            {
                s = s.Replace('.', ',');
                try
                {
                    return Convert.ToDouble(s);
                }
                catch
                {
                    throw e;
                }
            }
        }

        public static string Swap(this string s, char a, char b)
        {
            var mas = s.ToCharArray();
            int ai = s.IndexOf(a), bi = s.IndexOf(b);
            char tmp = mas[ai];
            mas[ai] = mas[bi];
            mas[bi] = tmp;
            return new string(mas);
        }

        public static double Reverse(this double val)
        {
            return 1.0 / val;
        }
        public static Complex Reverse(this Complex val)
        {
            double abs = val.Abs;
            if (Double.IsNaN(abs) || Double.IsInfinity(abs)) return 0;
            return 1.0 / val;
        }
        public static double Sqr(this double val) => val * val;
        public static Complex Sqr(this Complex val) => val * val;
        public static double Pow(this double v, double deg) => Math.Pow(v, deg);
        public static Complex Pow(this Complex v, double deg) => Complex.Pow(v, deg);

        /// <summary>
        /// Выдаёт массив действительных частей элементов комплексного массива
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double[] ToDoubleMas(this Complex[] c)
        {
            double[] res = new double[c.Length];
            for (int i = 0; i < c.Length; i++)
                res[i] = c[i].Re;
            return res;
        }
        /// <summary>
        /// Переводит произвольный массив в массив действительных чисел через конвертер
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double[] ToDoubleMas<T>(this T[] c)
        {
            double[] res = new double[c.Length];
            for (int i = 0; i < c.Length; i++)
                res[i] = Convert.ToDouble(c[i]);
            return res;
        }

        /// <summary>
        /// Переводит произвольный массив в массив строк через конвертер
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string[] ToStringMas<T>(this T[] c)
        {
            string[] res = new string[c.Length];
            for (int i = 0; i < c.Length; i++)
                res[i] = Convert.ToString(c[i]);
            return res;
        }
        /// <summary>
        /// Конкатенация двух массивов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T[] Union<T>(T[] a, T[] b)
        {
            T[] res = new T[a.Length + b.Length];
            for (int i = 0; i < a.Length; i++)
                res[i] = a[i];
            for (int i = 0; i < b.Length; i++)
                res[i + a.Length] = b[i];
            return res;
        }

        public static T[] Union<T>(params T[][] c)
        {
            //возможно, надо будет как-то преобразовать массив с
            int d2 = c.GetLength(1);
            int len = 0;
            for (int i = 0; i < d2; i++)
                len += c[i].Length;
            T[] res = new T[len];
            len = 0;
            for (int i = 0; i < d2; i++)
                for (int j = 0; j < c[i].Length; j++)
                    res[len++] = c[i][j];

            return res;
        }

        public class Compar : Comparer<double>
        {
            /// <summary>
            /// Компаратор по модулю
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public override int Compare(double x, double y)
            {
                return x.Abs().CompareTo(y.Abs());
            }

        }
        public class ComparPointTres<Tres> : Comparer<Tuple<Point, Tres>>
        {
            public override int Compare(Tuple<Point, Tres> x, Tuple<Point, Tres> y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }

        /// <summary>
        /// Размерность дробной части (количество знаков после запятой)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int DimOfFractionalPath(this double d)
        {
            string s = d.ToString();
            if (s.Contains("E-"))
            {
                int i = s.IndexOf('E');
                int deg = (int)s.Substring(i + 2).ToDouble();
                if (s.Contains(',')) deg--;
                return i + deg - 1;
            }
            else
            {
                int i = s.IndexOf(',');
                if (i <= 0) return 0;
                int deg = s.Substring(i + 1).Length;
                return deg;
            }
        }

        public static decimal ToDecimal(this double i) => Convert.ToDecimal(i);
        public static float ToFloat(this double i) => Convert.ToSingle(i);
        public static double ToDouble(this int i) => (double)i;
        public static int ToInt(this double i) => (int)i;

        /// <summary>
        /// Среднее двух целых чисел (по правилам целочисленного деления)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Average(int a, int b) => (a + b) / 2;
        /// <summary>
        /// Сокращение числа на период
        /// </summary>
        /// <param name="d"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static double ToPeriod(this double d,double period)
        {
            //int k = (d < 0) ? 1 : 0;
            //int f = (int)Math.Floor(d.Abs() / period);
            //return d - Math.Sign(d) * period * (f + k);
            int f = (int)Math.Floor(d / period);
            return d - f * period;
            return d;
        }
    }
    /// <summary>
    /// Парсер для перевода формулы функции в делегат
    /// </summary>
    public class Parser
    {
        private string term = "";
        private static string formula = "";
        /// <summary>
        /// Последняя отредактированная формула, по которой построился делегат
        /// </summary>
        public static string FORMULA => formula;
        private double nam, arg = 0;
        /// <summary>
        /// Конструктор для выражений без переменных (переменная считается нулевой)
        /// </summary>
        /// <param name="str">Строка формулы</param>
        public Parser(string str)
        {
            Clean(str);
            //arg = 0.0;
            nam = Product(term);
        }

        private void Clean(string str)
        {
            //Обработка входной строки
            foreach (char ch in str)//убрать пробелы и всякие знаки, которые не должны быть в формулах
            {
                if (Char.IsLetterOrDigit(ch) || ch == '^' || ch == '*' || ch == '/' || ch == '+' || ch == '-' || ch == ',' || ch == '(' || ch == ')' || ch == '.')
                {
                    term += ch;
                    if (ch == '.') term = term.Substring(0, term.Length - 1) + ',';
                }
            }

            //убрать лишние знаки операций
            DeleteOperators();

            //свести все лишние слова к одной переменной
            ToX();

            //убрать лишние знаки операций в конце
            char chh = term[term.Length - 1];
            while (chh == '^' || chh == '*' || chh == '/' || chh == '+' || chh == '-' || chh == ',' || chh == '(' || chh == ',')
            {
                term = term.Substring(0, term.Length - 1);
                chh = term[term.Length - 1];
            }

            formula = term;
        }

        private void DeleteOperators()
        {
            for (int i = 0; i < term.Length - 1; i++)
            {
                char chh = term[i];
                if (chh == '^' || chh == '*' || chh == '/' || chh == '+' || chh == '-' || chh == ',' || chh == ',')
                {
                    char ch = term[i + 1];
                    while (ch == '^' || ch == '*' || ch == '/' || ch == '+' || ch == '-' || ch == ',' || ch == ',')
                    {
                        if (i + 2 < term.Length)
                            term = term.Substring(0, i + 1) + term.Substring(i + 2, term.Length - i - 2);
                        else
                            term = term.Substring(0, i + 1);
                        if (i + 1 != term.Length) ch = term[i + 1];
                        else ch = ' ';
                    }
                }
                if (chh == ')')
                    if (Char.IsDigit(term[i + 1]))
                        term = term.Substring(0, i + 1) + "*" + term.Substring(i + 1, term.Length - i - 1);

            }
        }
        private void ToX()
        {
            //char[] c=new char[10];
            //for (int i = 0; i < 10; i++)
            //    c[i] = Convert.ToChar(i);

            string[] st = term.Split('+', '-', '^', '*', '/', ',', '(', ')');
            Array.Sort(st);
            st.Show();

            for (int i = 0; i < st.Length; i++)
            {
                string el = st[i];
                if (el.Length != 0)
                {
                    bool b = false;
                    for (int j = 0; j < el.Length; j++)
                        if (!Char.IsDigit(el[j]))
                        {
                            b = true;
                            break;
                        }
                    if (b)
                        if (el != "sin" && el != "cos" && el != "tan" && el != "acos" && el != "asin"
            && el != "atan" && el != "exp" && el != "log" && el != "abs" && el != "pi"
            && el != "sqrt" && el != "sqr" && el != "cube" && el != "x")
                            term = term.Replace(el, "x");
                }

            }
        }

        private static string info = "Для задания собственной функции требуется ввести строкой её формулу (аналитическое выражение). Выражения должны вводиться в точности так же, как если бы это была часть кода на C#; единственное отличие состоит в том, что вместо записей вида Math.Sin(x) используется sin(x). В качестве аргумента должен исльзоваться 'x'; доступные функции: sin, cos, tan, acos, asin, atan, exp, log, abs, sqrt, sqr, cube. Пример записи готовой функции: cos(x)+sin(x/2)-exp(x*log(x))^3. В случае некорректного ввода программа либо исправит формулу, либо вместо исключения будет использовать нулевую функцию.";

        /// <summary>
        /// Информация о том, какие функции считываются при парсинге и как и пользоваться
        /// </summary>
        public static string INFORMATION => info;

        /// <summary>
        /// Конструктор для выражений с переменными (переменная x)
        /// </summary>
        /// <param name="x">Значение переменной</param>
        /// <param name="str">Строка формулы</param>
        public Parser(double x, string str)
        {
            Clean(str);
            arg = x;
            ShowT();
            nam = Product(term);
            ShowT();
        }
        /// <summary>
        /// Конструктор для выражений с переменными (переменная x)
        /// </summary>
        /// <param name="x">Значение переменной</param>
        /// <param name="str">Строка формулы</param>
        public Parser(string s, double x) : this(x, s) { }

        private void ShowT()
        {
            var t = new Tuple<string, double, double>(term, nam, arg);
            Console.WriteLine(t.Item1 + " " + t.Item2 + " " + t.Item3);
        }

        //Метод обработки функций и присваивания значения переменной
        protected double Func(string s)
        {
            double element = 0.0;
            string el = "";
            foreach (char ch in s)
            {
                if (!Char.IsLetter(ch) /*|| ch!='x'*/) break;
                el += ch;
            }
            if (el == "sin") element = Math.Sin(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "cos") element = Math.Cos(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "tan") element = Math.Tan(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "asin") element = Math.Asin(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "acos") element = Math.Acos(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "atan") element = Math.Atan(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "exp") element = Math.Exp(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "ln") element = Math.Log(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "abs") element = Math.Abs(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "sqrt") element = Math.Sqrt(Convert.ToDouble(s.Substring(el.Length)));
            if (el == "sqr") element = Math.Pow(Convert.ToDouble(s.Substring(el.Length)), 2);
            if (el == "cube") element = Math.Pow(Convert.ToDouble(s.Substring(el.Length)), 3);
            if (el == "pi") element = Math.PI;
            if (el != "sin" && el != "cos" && el != "tan" && el != "acos" && el != "asin"
                && el != "atan" && el != "exp" && el != "log" && el != "abs" && el != "pi"
                && el != "sqrt" && el != "sqr" && el != "cube") element = arg;
            return element;
        }
        //Метод возведения в степень
        protected double Power(string s)
        {
            double element;
            string el = "";
            foreach (char ch in s)
            {
                if (ch == '^') break;
                el += ch;
            }
            if (Char.IsLetter(el[0])) element = arg;
            else element = Convert.ToDouble(el);
            if (s.Substring(el.Length + 1) != "")
            {
                element = Math.Pow(element, Element(s.Substring(el.Length + 1)));
            }

            return element;
        }
        //Расчет умножения/деления
        protected double Element(string s)
        {
            double element;
            string el = "";
            foreach (char ch in s)
            {
                if (ch == '*' || ch == '/') break;
                el += ch;
            }
            if (Char.IsLetter(el[0]) && el.IndexOf('^') == -1) element = Func(el);
            else
            {
                if (el.IndexOf('^') == -1) element = Convert.ToDouble(el);
                else element = Power(el);
            }
            if (el.Length < s.Length - 1)
            {
                if (s[el.Length] == '*') element *= Element(s.Substring(el.Length + 1));
                if (s[el.Length] == '/') element /= Element(s.Substring(el.Length + 1));
            }
            return element;
        }
        //Входная точка. Выделение элементов
        protected double Product(string s)
        {
            int co = 0;
            string el = "";
            double element;
            string s1 = s;
            string sstr;
            if (s != "" && (s[0] == '+' || s[0] == '-')) co++;
            for (int i = co; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    el += Breckets(s.Substring(el.Length + co), out sstr);
                    s = el + sstr;
                    co = 0;
                    i = el.Length;
                    if (sstr == "") break;
                }
                if (s[i] == '+' || s[i] == '-') break;
                el += s[i];
            }
            if (s1[0] == '-') element = -Element(el);
            else element = Element(el);
            if (el.Length < s.Length - 1) element += Product(s.Substring(el.Length + co));

            return element;
        }
        //Обработка выражений в скобках
        protected string Breckets(string s, out string sstr)
        {
            int co = 1;
            int open = 1;
            int quit = 0;
            string el = "";
            double element;
            while (open != quit)
            {
                if (s[co] == '(')
                {
                    open++;
                }
                if (s[co] == ')') quit++;
                if (open == quit) break;
                el += s[co];
                co++;
            }
            if (co < s.Length - 1) sstr = s.Substring(co + 1);
            else
            {
                sstr = "";
            }
            element = Product(el);

            return element.ToString();
        }
        //Результат
        private double Nam => nam;
        /// <summary>
        /// Возвращает функцию по формуле этой функции, где переменной является x
        /// </summary>
        /// <param name="s">Формула функции</param>
        /// <returns></returns>
        public static RealFunc GetDelegate(string s)
        {
            Parser p = new Parser(s);

            RealFunc f = (double x) =>
            {
                p.arg = x;
                p.nam = p.Product(p.term);
                return p.Nam;
            };
            return f;
        }
    }

    /// <summary>
    /// Критерии принятия решений в условиях неопределённости
    /// </summary>
    public static class UnderUncertainty
    {
        private static void MAX(Vectors v, out int k)
        {
            double m = v[0];
            k = 0;
            for (int i = 1; i < v.n; i++)
                if (v[i] > m) { m = v[i]; k = i; }
        }
        private static void MIN(Vectors v, out int k)
        {
            double m = v[0];
            k = 0;
            for (int i = 1; i < v.n; i++)
                if (v[i] < m) { m = v[i]; k = i; }
        }

        /// <summary>
        /// Критерий среднего выйгрыша
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="q">Вектор вероятностей</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void AverageGain(Matrix S, out Vectors v, Vectors q = null)
        {
            v = new Vectors(S.n);
            if (q == null)
            {
                q = new Vectors(S.m);
                double w = 1.0 / q.n;
                for (int i = 0; i < q.n; i++) q[i] = w;
            }
            for (int i = 0; i < S.n; i++)
            {
                for (int j = 0; j < S.m; j++) v[i] += S[i, j] * q[j];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию среднего значения с вектором вероятностей " + q.ToString() + " оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
        /// <summary>
        /// Критерий минимакса
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void MiniMax(Matrix S, out Vectors v)
        {
            v = new Vectors(S.n);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                int tmp; MAX(c, out tmp);
                v[i] = S[i, tmp];
            }

            int s; MIN(v, out s);
            Console.WriteLine("По критерию минимакса оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
        /// <summary>
        /// Критерий максимакса
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void MaxiMax(Matrix S, out Vectors v)
        {
            v = new Vectors(S.n);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                int tmp; MAX(c, out tmp);
                v[i] = S[i, tmp];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию максимакса оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
        /// <summary>
        /// Критерий Лапласа
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void Laplas(Matrix S, out Vectors v)
        {
            v = new Vectors(S.n);
            Vectors q = new Vectors(S.n);
            double w = 1.0 / S.m;
            for (int i = 0; i < q.n; i++) q[i] = w;

            for (int i = 0; i < S.n; i++)
            {
                for (int j = 0; j < S.m; j++) v[i] += S[i, j];
                v[i] *= q[i];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию Лапласа оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
        /// <summary>
        /// Критерий Вальда (максимин)
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void Vald(Matrix S, out Vectors v)
        {
            v = new Vectors(S.n);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                int tmp; MIN(c, out tmp);
                v[i] = S[i, tmp];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию Вальда (максимина) оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
        /// <summary>
        /// Критерий Сэвиджа
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void Savage(Matrix S, out Vectors v)
        {
            Vectors v0 = new Vectors(S.n);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                int tmp; MAX(c, out tmp);
                v0[i] = S[i, tmp];
            }
            Matrix M = new Matrix(S.n, S.m);
            for (int i = 0; i < M.n; i++)
                for (int j = 0; j < M.m; j++)
                    M[i, j] = v0[i] - S[i, j];//M.PrintMatrix();

            v = new Vectors(M.n);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = M.GetLine(i);
                int tmp; MAX(c, out tmp);
                v[i] = M[i, tmp];
            }
            int s; MIN(v, out s);
            Console.WriteLine("По критерию Сэвиджа оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
        /// <summary>
        /// Критерий Гурвица
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        /// <param name="a">Коэффициент оптимизма</param>
        public static void Hurwitz(Matrix S, out Vectors v, double a = 0.5)
        {
            v = new Vectors(S.n);
            Vectors l = new Vectors(v), r = new Vectors(v);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                int tmp1, tmp2;
                MAX(c, out tmp1);
                MIN(c, out tmp2);
                l[i] = S[i, tmp1]; r[i] = S[i, tmp2];
                v[i] = a * l[i] + (1 - a) * r[i];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию Гурвица с параметром {2} оптимальным является решение {0} (со значением {1}).", s + 1, v[s], a);
        }
        /// <summary>
        /// Критерий Ходжа-Лемана
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        /// <param name="a">Коэффициент метода (вес)</param>
        /// <param name="q">Вектор вероятностей</param>
        public static void HodgeLeman(Matrix S, out Vectors v, double a = 0.5, Vectors q = null)
        {
            v = new Vectors(S.n);
            if (q == null)
            {
                q = new Vectors(S.m);
                double w = 1.0 / q.n;
                for (int i = 0; i < q.n; i++) q[i] = w;
            }
            Vectors l = new Vectors(v), r = new Vectors(v);
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                int tmp1, tmp2;
                for (int j = 0; j < S.m; j++) l[i] += S[i, j] * q[j];
                MIN(c, out tmp2);
                r[i] = S[i, tmp2];
                v[i] = a * l[i] + (1 - a) * r[i];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию Ходжа-Лемана с параметром {2} и вектором вероятностей {3} оптимальным является решение {0} (со значением {1}).", s + 1, v[s], a, q.ToString());
        }
        /// <summary>
        /// Критерий Гермейера
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void Germeier(Matrix S, out Vectors v, Vectors q = null)
        {
            v = new Vectors(S.n);
            if (q == null)
            {
                q = new Vectors(S.m);
                double w = 1.0 / q.n;
                for (int i = 0; i < q.n; i++) q[i] = w;
            }
            for (int i = 0; i < S.n; i++)
            {
                Vectors c = S.GetLine(i);
                for (int j = 0; j < S.m; j++) c[j] *= q[j];
                int tmp; MIN(c, out tmp);
                v[i] = S[i, tmp];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию Гермейера с вектором вероятностей {2} оптимальным является решение {0} (со значением {1}).", s + 1, v[s], q.ToString());
        }
        /// <summary>
        /// Критерий произведений
        /// </summary>
        /// <param name="S">Матрица решений</param>
        /// <param name="v">Дополнительный столбец</param>
        public static void Powers(Matrix S, out Vectors v)
        {
            v = new Vectors(S.n);
            for (int i = 0; i < S.n; i++)
            {
                v[i] = 1;
                for (int j = 0; j < S.m; j++)
                    v[i] *= S[i, j];
            }

            int s; MAX(v, out s);
            Console.WriteLine("По критерию произведений оптимальным является решение {0} (со значением {1}).", s + 1, v[s]);
        }
    }

    /// <summary>
    /// Методы комбинаторики
    /// </summary>
    public class Combinatorik
    {
        /// <summary>
        /// Число перестановок (факториал)
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static int P(int k)
        {
            if (k < 2) return 1;
            int s = 1;
            for (int i = 2; i <= k; i++) s *= i;
            return s;
        }
        /// <summary>
        /// Перестановки с повторениями
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static int P(params int[] k)
        {
            int sum = 0;
            for (int i = 0; i < k.Length; i++) sum += k[i];
            sum = P(sum);
            for (int i = 0; i < k.Length; i++) sum /= P(k[i]);
            return sum;
        }
        /// <summary>
        /// Число размещений из n по m 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int A(int m, int n)
        {
            int s = 1;
            for (int i = m + 1; i <= n; i++) s *= i;
            return s;
        }
        /// <summary>
        /// Размещения с повторениями
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int AA(int m, int n) { return (int)Math.Pow(n, m); }
        /// <summary>
        /// Число сочетаний из n по m
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int C(int m, int n) { return A(m, n) / P(m); }
        /// <summary>
        /// Сочетания с повторениями
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int CС(int m, int n) { return C(m, n + m - 1); }
    }

    /// <summary>
    /// Вероятности
    /// </summary>
    public class Probability
    {
        /// <summary>
        /// Абстрактный класс случайной величины
        /// </summary>
        public abstract class RandVal
        {
            /// <summary>
            /// Математическое ожидание
            /// </summary>
            public abstract double M { get; }
            /// <summary>
            /// Дисперсия
            /// </summary>
            public abstract double Dis { get; }
        }

        /// <summary>
        /// Дискретная случайная величина
        /// </summary>
        public class DisRandVal : RandVal
        {
            /// <summary>
            /// Значения случайной величины
            /// </summary>
            Vectors X;
            /// <summary>
            /// Значения вероятностей
            /// </summary>
            Vectors p;
            /// <summary>
            /// Функция распределения
            /// </summary>
            RealFunc F = null;

            //Конструкторы

            /// <summary>
            /// Конструктор по массиву вероятностей
            /// </summary>
            /// <param name="a"></param>
            public DisRandVal(double[] a)
            {
                if (!ProbOne(a)) throw new Exception("Сумма элементов в массиве не равна 1");
                X = new Vectors(a.Length);
                p = new Vectors(a.Length);
                for (int i = 0; i < a.Length; i++)
                {
                    X[i] = i + 1;
                    p[i] = a[i];
                }
            }
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            /// <param name="n"></param>
            public DisRandVal(int n)
            {
                double[] a = new double[n];
                X = new Vectors(n); p = new Vectors(n);
                double val = 1.0 / n;
                for (int i = 0; i < n; i++) a[i] = val;
                DisRandVal r = new DisRandVal(a);
                this.X = r.X;
                this.p = r.p;
            }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="r"></param>
            public DisRandVal(DisRandVal r) { X = new Vectors(r.X); p = new Vectors(r.p); }
            /// <summary>
            /// Чтение из файла
            /// </summary>
            /// <param name="fs"></param>
            public DisRandVal(StreamReader fs)
            {
                string s = fs.ReadLine();
                string[] st = s.Split(' ');
                int n = st.Length;
                this.X = new Vectors(n); this.p = new Vectors(n);
                for (int i = 0; i < n; i++) X[i] = Convert.ToDouble(st[i]);
                s = fs.ReadLine();
                st = s.Split(' ');
                for (int i = 0; i < n; i++) p[i] = Convert.ToDouble(st[i]);
                if (!ProbOne(this.p.vector)) throw new Exception("Сумма элементов в массиве не равна 1");
                fs.Close();
            }

            //Свойства
            /// <summary>
            /// Функция распределения дискретной случайной величины
            /// </summary>
            public RealFunc FDist
            {
                get
                {
                    return (double x) =>
                {
                    if (x < this.X[0]) return 0;
                    if (x > this.X[p.n - 1]) return 1;
                    double k = this.p[0];
                    for (int i = 1; i < this.p.n; i++)
                    {
                        if (x <= this.X[i]) return k;
                        k += this.p[i];
                    }
                    return k;
                };
                }
            }

            //методы
            /// <summary>
            /// Подходит ли массив под массив вероятностей
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            private bool ProbOne(double[] k)
            {
                double sum = 0;
                for (int i = 0; i < k.Length; i++) sum += k[i];
                if (sum == 1) return true;
                return false;
            }
            /// <summary>
            /// Проиллюстрировать
            /// </summary>
            public void Show()
            {
                this.X.PrintMatrix();
                this.p.PrintMatrix();
            }
            /// <summary>
            /// Мат. ожидание этой СВ
            /// </summary>
            /// <returns></returns>
            public override double M
            {
                get
                {
                    DisRandVal R = new DisRandVal(this);
                    return DisRandVal.MatExp(R);
                }
            }
            /// <summary>
            /// Дисперсия
            /// </summary>
            public override double Dis
            {
                get { return DisRandVal.Dispersion(this); }
            }
            /// <summary>
            /// Мат. ожидание СВ
            /// </summary>
            /// <param name="R"></param>
            /// <returns></returns>
            public static double MatExp(DisRandVal R)
            {
                double sum = 0;
                for (int i = 0; i < R.X.n; i++) sum += R.X[i] * R.p[i];
                return sum;
            }
            /// <summary>
            /// Дисперсия
            /// </summary>
            /// <param name="R"></param>
            /// <returns></returns>
            public static double Dispersion(DisRandVal R) { return CenM(R, 2); }
            /// <summary>
            /// Начальный момент
            /// </summary>
            /// <param name="R"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static double BegM(DisRandVal R, int n) { return MatExp((R) ^ n); }
            /// <summary>
            /// Центральный момент
            /// </summary>
            /// <param name="R"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static double CenM(DisRandVal R, int n) { return MatExp((R - R.M) ^ n); }
            /// <summary>
            /// По неравенству Чебышева вероятность того, что случайная величина отклонится от мат. ожидания не менее чем на eps
            /// </summary>
            /// <param name="R"></param>
            /// <param name="eps"></param>
            public static void NerCheb(DisRandVal R, double eps) { Console.WriteLine("<= {0}", R.Dis / eps / eps); }

            //операторы
            /// <summary>
            /// Смещение СВ
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <returns></returns>
            public static DisRandVal operator -(DisRandVal A, double m)
            {
                DisRandVal M = new DisRandVal(A);
                for (int i = 0; i < M.X.n; i++) M.X[i] -= m;
                return M;
            }
            /// <summary>
            /// Случайная величина в степени
            /// </summary>
            /// <param name="A"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static DisRandVal operator ^(DisRandVal A, int n)
            {
                DisRandVal M = new DisRandVal(A);
                for (int i = 0; i < M.X.n; i++) M.X[i] = Math.Pow(M.X[i], n);
                return M;
            }
        }

        /// <summary>
        /// Непрерывная случайная величина
        /// </summary>
        public class ConRandVal : RandVal
        {
            /// <summary>
            /// Тип распределения (нормальное, равномерное, пуассоновское, экспоненциальное и т. д.)
            /// </summary>
            public enum BasisDistribution { Normal, Uniform, Puasson, Exp, Other };

            /// <summary>
            /// Вспомогательная функция
            /// </summary>
            private RealFunc x = (double t) => { return t; };

            /// <summary>
            /// Функция распределения
            /// </summary>
            private RealFunc F = null;
            /// <summary>
            /// Плотность распределения
            /// </summary>
            private RealFunc f = null;
            private BasisDistribution TypeValue = BasisDistribution.Other;
            /// <summary>
            /// Пока не известные мат. ожидание и дисперсия
            /// </summary>
            private double? m = null, d = null;

            //Конструкторы
            /// <summary>
            /// Конструктор по функции распределения и плотности распределения
            /// </summary>
            /// <param name="A"></param>
            /// <param name="a"></param>
            public ConRandVal(RealFunc A, RealFunc a) { F = A; f = a; }//по обеим функциям
                                                                       /// <summary>
                                                                       /// Конструктор только по плотности распределению
                                                                       /// </summary>
                                                                       /// <param name="a"></param>
            public ConRandVal(RealFunc a) { f = a;/* F = (double t) => { return DefInteg.};*/ }//по плотности распределения
                                                                                               /// <summary>
                                                                                               /// Конструктор копирования
                                                                                               /// </summary>
                                                                                               /// <param name="S"></param>
            public ConRandVal(ConRandVal S) { this.f = S.f; this.F = S.F; this.x = S.x; this.TypeValue = S.TypeValue; }
            /// <summary>
            /// Конструктор по одному из основных распределений с двумя аргументами
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="m"></param>
            /// <param name="D"></param>
            public ConRandVal(BasisDistribution Type, double m, double D)
            {
                switch (Type)
                {
                    //Нормальное распределение
                    case BasisDistribution.Normal:
                        this.f = (double s) => { return 1.0 / Math.Sqrt(1 * Math.PI * D) * Math.Exp(-1.0 / 2 / D * (s - m) * (s - m)); };
                        this.m = m;
                        this.d = D;
                        this.F = (double x) => { return FuncMethods.DefInteg.Simpson((double t) => { return Math.Exp(-t * t / 2); }, 0, x); };
                        return;
                    //Равномерное распределение
                    case BasisDistribution.Uniform:
                        this.f = (double s) => { return 1.0 / (D - m); };
                        this.m = (D + m) / 2;
                        this.d = (D - m) * (D - m) / 12;
                        this.F = (double s) =>
                        {
                            if (s < m) return 0;
                            if (m < s && s <= D) return (s - m) / (D - m);
                            return 1;
                        };
                        return;
                    //Распределение Пуассона
                    case BasisDistribution.Puasson:
                        int m_new = (int)m;
                        double tmp = Math.Exp(-D);
                        this.f = (double s) => { return Math.Pow(D, m_new) / Combinatorik.P(m_new) * tmp; };
                        this.m = D;
                        this.d = D;
                        return;
                    default:
                        throw new Exception("Такого конструктора не существует");

                }
            }
            /// <summary>
            /// Конструктор по параметру экспоненциального распределния
            /// </summary>
            /// <param name="l"></param>
            public ConRandVal(double l)
            {
                this.f = (double s) =>
                {
                    if (s < 0) return 0;
                    return l * Math.Exp(-l * s);
                };
                this.m = 1 / l;
                this.d = 1 / l / l;

                this.F = (double s) =>
                {
                    if (s < 0) return 0;
                    return 1 - Math.Exp(-l * s);
                };
            }
            /// <summary>
            /// Конструктор нормального распределения по умолчанию
            /// </summary>
            public ConRandVal()
            {
                ConRandVal T = new ConRandVal(BasisDistribution.Normal, 0, 1);
                this.f = T.f; this.m = T.m; this.d = T.d;
            }

            //Операторы
            /// <summary>
            /// Случайная величина в степени
            /// </summary>
            /// <param name="a"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static ConRandVal operator ^(ConRandVal a, int n)
            {
                ConRandVal S = new ConRandVal(a);
                S.x = (double t) => { return Math.Pow(t, n); };
                return S;
            }
            /// <summary>
            /// Сдвиг случайной величины
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <returns></returns>
            public static ConRandVal operator -(ConRandVal A, double m)
            {
                ConRandVal S = new ConRandVal(A);
                S.x = (double t) => { return t - m; };
                return S;
            }

            //Методы
            /// <summary>
            /// Мат. ожидание
            /// </summary>
            /// <param name="R"></param>
            /// <returns></returns>
            public static double MatExp(ConRandVal R)
            {
                RealFunc xf = (double t) => { return R.x(t) * R.f(t); };
                return FuncMethods.DefInteg.ImproperFirstKind(xf);
            }
            /// <summary>
            /// Дисперсия
            /// </summary>
            /// <param name="R"></param>
            /// <returns></returns>
            public static double Dispersion(ConRandVal R) { return /*CenM(R, 2);*/ MatExp(R ^ 2) - R.M * R.M; }
            /// <summary>
            /// Начальный момент
            /// </summary>
            /// <param name="R"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static double BegM(ConRandVal R, int n) { return MatExp((R) ^ n); }
            /// <summary>
            /// Центральный момент
            /// </summary>
            /// <param name="R"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static double CenM(ConRandVal R, int n) { return MatExp((R - R.M) ^ n); }
            /// <summary>
            /// По неравенству Чебышева вероятность того, что случайная величина отклонится от мат. ожидания не менее чем на eps
            /// </summary>
            /// <param name="R"></param>
            /// <param name="eps"></param>
            public static void NerCheb(ConRandVal R, double eps) { Console.WriteLine("<= {0}", R.Dis / eps / eps); }
            /// <summary>
            /// Вывести на консоль информацию о случайной величине
            /// </summary>
            public void Show()
            {
                Console.WriteLine("Мат. ожидание: {0} ; дисперсия: {1} ; тип распределения: {2}", this.M, this.Dis, this.TypeValue);
            }
            /// <summary>
            /// Вероятность попадания случайной величины в интервал
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public double P(double a, double b)
            {
                if (this.F != null) return F(b) - F(a);
                return FuncMethods.DefInteg.Simpson(this.f, a, b);
            }

            //Свойства
            /// <summary>
            /// Мат. ожидание
            /// </summary>
            public override double M
            {
                get
                {
                    if (this.m != null) return (double)this.m;
                    ConRandVal R = new ConRandVal(this);
                    this.m = ConRandVal.MatExp(R);
                    return (double)this.m;
                }
            }
            /// <summary>
            /// Дисперсия
            /// </summary>
            public override double Dis
            {
                get
                {
                    if (this.d != null) return (double)this.d;
                    this.d = ConRandVal.Dispersion(this);
                    return (double)this.d;
                }
            }

            //Константы класса
        }
    }

    /// <summary>
    /// Точки на плоскости
    /// </summary>
    public class Point : IComparable, ICloneable
    {
        //координаты
        /// <summary>
        /// Первая координата точки
        /// </summary>
        public double x = 0;
        /// <summary>
        /// Вторая координата точки
        /// </summary>
        public double y = 0;

        //конструкторы
        /// <summary>
        /// Точка с нулевыми координатами
        /// </summary>
        public Point() { x = 0; y = 0; }
        /// <summary>
        /// Точка с одинаковыми координатами
        /// </summary>
        /// <param name="a"></param>
        public Point(double a) { x = a; y = a; }
        /// <summary>
        /// Точка по своим координатам
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Point(double a, double b) { x = a; y = b; }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="p"></param>
        public Point(Point p) : this(p.x, p.y) { }

        //методы
        /// <summary>
        /// Набор n+1 точек на графике функции f, разбитых равномерно на отрезке от a до b
        /// </summary>
        /// <param name="f"></param>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point[] Points(RealFunc f, int n, double a, double b)
        {
            double h = (b - a) / n;
            Point[] points = new Point[n + 1];
            for (int i = 0; i <= n; i++) points[i] = new Point(a + h * i, f(a + h * i));

            return points;
        }
        //то же самое, только отдельными массивами выводятся первые и вторые координаты точек (сделано для рисования в Chart)
        public static double[] PointsX(RealFunc f, int n, double a, double b)
        {
            Point[] p = new Point[Point.Points(f, n, a, b).Length];
            for (int i = 0; i < p.Length; i++) p[i] = new Point(Point.Points(f, n, a, b)[i]);

            double[] x = new double[p.Length];
            for (int i = 0; i < p.Length; i++) x[i] = p[i].x;
            return x;
        }
        public static double[] PointsY(RealFunc f, int n, double a, double b)
        {
            Point[] p = new Point[Point.Points(f, n, a, b).Length];
            for (int i = 0; i < p.Length; i++) p[i] = new Point(Point.Points(f, n, a, b)[i]);

            double[] y = new double[p.Length];
            for (int i = 0; i < p.Length; i++) y[i] = p[i].y;
            return y;
        }
        /// <summary>
        /// Вывести массив точек, через которые проходит функция
        /// </summary>
        /// <param name="f">Функция, заданная на отрезке</param>
        /// <param name="h">Шаг обхода отрезка</param>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <returns></returns>
        public static Point[] Points(RealFunc f, double h, double a, double b)
        {
            int n = (int)((b - a) / h);
            Point[] points = new Point[n];
            for (int i = 0; i < n; i++) points[i] = new Point(a + h * i, f(a + h * i));

            return points;
        }
        public static double[] PointsX(RealFunc f, double h, double a, double b)
        {
            Point[] p = new Point[Point.Points(f, h, a, b).Length];
            for (int i = 0; i < p.Length; i++) p[i] = new Point(Point.Points(f, h, a, b)[i]);

            double[] x = new double[p.Length];
            for (int i = 0; i < p.Length; i++) x[i] = p[i].x;
            return x;
        }
        public static double[] PointsY(RealFunc f, double h, double a, double b)
        {
            Point[] p = new Point[Point.Points(f, h, a, b).Length];
            for (int i = 0; i < p.Length; i++) p[i] = new Point(Point.Points(f, h, a, b)[i]);

            double[] y = new double[p.Length];
            for (int i = 0; i < p.Length; i++) y[i] = p[i].y;
            return y;
        }
        /// <summary>
        /// Считать массив точек из файла
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static Point[] Points(StreamReader fs)
        {
            string s = fs.ReadLine();
            string[] st = s.Split(' ');
            int n = Convert.ToInt32(st[0]);
            Point[] p = new Point[n];

            for (int k = 0; k < n; k++)
            {
                s = fs.ReadLine();
                st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа
                p[k] = new Point(Convert.ToDouble(st[0]), Convert.ToDouble(st[1]));
            }

            fs.Close();
            return p;
        }


        /// <summary>
        /// Массив точек, через которые проходит функция, по массиву абцисс эти точек
        /// </summary>
        /// <param name="f"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Point[] Points(RealFunc f, double[] c)
        {
            Point[] p = new Point[c.Length];
            for (int i = 0; i < c.Length; i++) p[i] = new Point(c[i], f(c[i]));
            return p;
        }
        /// <summary>
        /// Перевести массив чисел в последовательность точек на плоскости
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Point[] GetSequence(double[] c)
        {
            Point[] p = new Point[c.Length];
            for (int i = 0; i < c.Length; i++) p[i] = new Point(i, c[i]);
            return p;
        }
        /// <summary>
        /// Генерация массива точек по списку точек
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public static Point[] Points(List<Point> L)
        {
            Point[] P = new Point[L.Count];
            for (int i = 0; i < P.Length; i++)
                P[i] = new Point(L[i]);
            return P;
        }

        /// <summary>
        /// Строковое изображение точки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0} , {1})", this.x, this.y);
        }
        /// <summary>
        /// Показать координаты точки на консоли
        /// </summary>
        public void Show() { Console.WriteLine(this.ToString()); }
        /// <summary>
        /// Показать массив точек на консоли
        /// </summary>
        /// <param name="f"></param>
        public static void Show(Point[] f)
        {
            //for (int i = 0; i < f.Length; i++) Console.Write("{0} \t", f[i].x); Console.WriteLine();
            //for (int i = 0; i < f.Length; i++) Console.Write("{0} \t", f[i].y); Console.WriteLine();
            for (int i = 0; i < f.Length; i++) Console.WriteLine(f[i].ToString());
        }

        /// <summary>
        /// Сравнение точек по установленной по умолчанию упорядоченности
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Point a, Point b)
        {
            return (b < a);
        }
        public static bool operator <(Point a, Point b) //функция компоратора
        {
            if (a.y < b.y)
            {
                return true; //cравнение по второй координате
            }
            else if (a.y > b.y)
            {
                return false;
            }
            else
            {
                return a.x < b.x; //если вторые координаты равны, сравнение по первой координате
            }
        }
        //public static bool operator ==(Point a, Point b) => (new Complex(a) - new Complex(b)).Abs == 0;
        public static bool operator ==(Point a, Point b) { /*if (a == null || b == null) return false;bool tmp = false; try { tmp=(a.x == b.x) && (a.y == b.y); } catch(NullReferenceException e){ }return tmp;*/  return (a.x == b.x) && (a.y == b.y); }
        public static bool operator !=(Point a, Point b) { /*if (Convert.IsDBNull((object)b) || Convert.IsDBNull((object)a)) return false;*/ return !(a == b); }

        /// <summary>
        /// Евклидово расстояние между точками
        /// </summary>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Eudistance(Point z, Point w)
        {
            return Math.Sqrt((z.x - w.x) * (z.x - w.x) + (z.y - w.y) * (z.y - w.y));
        }

        public int CompareTo(object obj)
        {
            Point p = (Point)obj;
            if (this.x < p.x) return -1;
            if (this.x == p.x)
            {
                if (this.y < p.y) return -1;
                if (this.y == p.y) return 0;
            }
            return 1;
            //return x.CompareTo(obj);
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return /*point != null &&*/ x == point.x && y == point.y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            //throw new NotImplementedException();
            return (object)new Point(this);
        }

        public static implicit operator Point(Number.Complex e) { return new Point(e.Re, e.Im); }
    }

    /// <summary>
    /// Класс кратных узлов
    /// </summary>
    public class MultipleKnot
    {
        /// <summary>
        /// Абцисса кратного узла
        /// </summary>
        public double x;
        /// <summary>
        /// Массив ординат кратного узла
        /// </summary>
        public double[] y;

        /// <summary>
        /// Полный конструктор
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MultipleKnot(double x, params double[] y)
        {
            this.x = x;
            this.y = new double[y.Length];
            for (int i = 0; i < y.Length; i++) this.y[i] = y[i];
        }

        /// <summary>
        /// Кратность узла
        /// </summary>
        public int Multiplicity { get { return this.y.Length; } }

    }

    /// <summary>
    /// Числовые классы
    /// </summary>
    public static class Number
    {
        /// <summary>
        /// Рациональные числа (числа, представимые в виде m/n)
        /// </summary>
        public class Rational
        {
            /// <summary>
            /// Делимое и делитель в числе
            /// </summary>
            /// <remarks>long нужен для того, чтобы переводить в рациональные числа действительные числа с длинной мантиссой</remarks>
            long m, n;
            /// <summary>
            /// Ноль и единица во множестве рациональных чисел
            /// </summary>
            public static readonly Rational ZERO, ONE;

            //Constructors
            /// <summary>
            /// Ноль по умолчанию
            /// </summary>
            public Rational() { m = 0; n = 1; }
            /// <summary>
            /// Рациональное число по целому числу
            /// </summary>
            /// <param name="a"></param>
            public Rational(long a) { this.m = a; this.n = 1; }
            /// <summary>
            /// Несократимая дробь, эквивалентная частному аргументов
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            public Rational(long a, long b)//несократимой дробью
            {
                if (b < 0) { b = -b; a = -a; }
                long d = Nod(a, b); d = Math.Abs(d);
                m = a / d; n = b / d;
            }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="a"></param>
            public Rational(Rational a) { this.n = a.n; this.m = a.m; }
            /// <summary>
            /// Рациональное число по "действительному" числу
            /// </summary>
            /// <param name="x"></param>
            public Rational(double x) { Rational a = new Rational(ToRational(x)); this.n = a.n; this.m = a.m; }
            static Rational()
            {
                ZERO = new Rational(0, 1);
                ONE = new Rational(1, 1);
            }

            //Methods
            /// <summary>
            /// Наибольший общий делитель
            /// </summary>
            /// <param name="c"></param>
            /// <param name="d"></param>
            /// <returns></returns>
            public static long Nod(long c, long d)
            {
                long p = 0;
                long a = c, b = d;
                if (a < 0) a = -a;//a = Math.Abs(c);
                if (b < 0) b = -b;//b = Math.Abs(d);
                do
                {
                    p = a % b; a = b; b = p;
                } while (b != 0);
                return a;
            }

            /// <summary>
            /// Перевести число в строку, где число имеет вид неправильной дроби
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (this.n == 1) return this.m.ToString();
                return m + "/" + n;
            }
            /// <summary>
            /// Привести число в строку, где оно имеет вид смешанной дроби
            /// </summary>
            /// <returns></returns>
            public string ToStringMixed()
            {
                string s;
                long k = this.m / this.n;
                Rational r = new Rational(this.m - this.n * k, this.n);
                s = String.Format("{0} + {1}", k, r.ToString());
                return s;
            }
            /// <summary>
            /// Вывести на консоль неправильную дробь
            /// </summary>
            public void ShowWrong() { Console.WriteLine(this.ToString()); }
            /// <summary>
            /// Вывести смешанную дробь
            /// </summary>
            public void ShowMixed() { Console.WriteLine(this.ToStringMixed()); }
            /// <summary>
            /// Перевод рационального числа в тип double
            /// </summary>
            /// <returns></returns>
            public double ToDouble() { return ((double)this.m / this.n); }
            /// <summary>
            /// Перевод десятичного числа в несократимую дробь
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public static Rational ToRational(double x)
            {
                if (Math.Abs(x) <= 1e-15) return Rational.ZERO;

                string s = Math.Abs(x).ToString();
                //string s = Convert.ToString(Math.Abs(x));
                int i = 0, n = 0;
                while ((s[i] != ',') && (i < s.Length - 1)) i++;
                if (i == s.Length - 1) return new Rational((long)x);
                while (i + n < s.Length) n++;

                //если период возможен
                if (n >= 8)
                {
                    //---------Проверка на периодичность (полную)
                    Rational u = Rational.ToRational((long)x);
                    long f = u.IntPart;
                    Rational z = new Rational(f);//отделить целую часть

                    //string mant = s.Substring(i, s.Length - 1/*-i*/);//отделить цифры, стоящие после запятой
                    //mant = mant.Substring(1, mant.Length - 1/*-1*/);
                    string mant = s.Substring(i + 1, n - 1);//Console.WriteLine(mant);
                                                            //Console.WriteLine(mant);
                    for (int beg = 0; beg <= n - 6; beg++)//если периоды проверять не с первого символа
                    {
                        int idx = 0;//индекс
                        int cnt = 0;//количество повторений подстроки
                        for (int k = 1; k < (n - beg) / 2 + 1;)//проход по подстрокам всех длин
                        {
                            for (int h = 0; h < n * n; h++)
                            {
                                idx = mant.IndexOf(mant.Substring(beg, k/*+beg*/), idx);
                                if (idx == -1) break;
                                else
                                {
                                    cnt += 1;
                                    idx += mant.Substring(beg, k/*+beg*/).Length;
                                }
                            }
                            if (k * cnt > 2.0 * (n - beg) / 3)//если нашёлся период
                            {
                                //mant = s.Substring(i++, s.Length);
                                long a, b;
                                if (beg > 0)
                                {
                                    a = Convert.ToInt64(mant.Substring(0, k + beg)) - Convert.ToInt64(mant.Substring(0, beg));
                                    b = (long)((Math.Pow(10, mant.Substring(beg, k /*+ beg*/).Length) - 1) * Math.Pow(10, mant.Substring(0, beg).Length));
                                }
                                else
                                {
                                    a = Convert.ToInt64(mant.Substring(0, k));
                                    b = (long)(Math.Pow(10, mant.Substring(beg, k /*+ beg*/).Length) - 1);
                                }
                                Rational r = new Rational(a, b);
                                if (x < 0) r = -r;
                                return z + r;
                            }
                            k++;
                            idx = 0;
                            cnt = 0;
                        }
                    }
                }

                //если периода нет
                return new Rational((long)(x * Math.Pow(10, n)), (long)Math.Pow(10, n));
            }
            /// <summary>
            /// Целая часть числа
            /// </summary>
            public long IntPart { get { return Rational.IntegerPart(this); } }
            /// <summary>
            /// Целая часть числа
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            public static long IntegerPart(Rational t)
            {
                if (t.m >= 0) return t.m / t.n;
                if (t.n == 1) return t.m;
                return t.m / t.n - 1;
            }
            /// <summary>
            /// Дробная часть числа
            /// </summary>
            public Rational FracPart { get { return Rational.FractPart(this); } }
            /// <summary>
            /// Дробная часть числа
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            public static Rational FractPart(Rational t) { return t - t.IntPart; }
            /// <summary>
            /// Является ли дробным
            /// </summary>
            /// <returns></returns>
            public static bool IsFractional(Rational r) { return !(r.FracPart == ZERO); }
            /// <summary>
            /// Является ли дробным
            /// </summary>
            /// <returns></returns>
            public bool IsFract() { return Rational.IsFractional(this); }

            /// <summary>
            /// Показать действительное число в виде смешанной дроби
            /// </summary>
            /// <param name="x"></param>
            public static void Show(double x) { ToRational(x).ShowMixed(); }
            /// <summary>
            /// Показать рациональное число в виде смешанной дроби
            /// </summary>
            /// <param name="x"></param>
            public static void Show(Rational x) { x.ShowMixed(); }
            /// <summary>
            /// Показать комплексное число с рациональными частями
            /// </summary>
            /// <param name="a"></param>
            public static void Show(Complex a) { Console.WriteLine("(" + ToRational(a.Re).ToStringMixed() + ") + (" + ToRational(a.Im).ToStringMixed() + ")i"); }

            public override bool Equals(object obj)
            {
                var rational = obj as Rational;
                return rational != null &&
                       m == rational.m &&
                       n == rational.n &&
                       IntPart == rational.IntPart &&
                       EqualityComparer<Rational>.Default.Equals(FracPart, rational.FracPart);
            }

            public override int GetHashCode()
            {
                var hashCode = 893539880;
                hashCode = hashCode * -1521134295 + m.GetHashCode();
                hashCode = hashCode * -1521134295 + n.GetHashCode();
                hashCode = hashCode * -1521134295 + IntPart.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<Rational>.Default.GetHashCode(FracPart);
                return hashCode;
            }

            //Operators
            public static Rational operator +(Rational a, Rational b) { return new Rational((a.m * b.n + a.n * b.m), (a.n * b.n)); }
            public static Rational operator -(Rational a) { return new Rational(-a.m, a.n); }
            public static Rational operator -(Rational a, Rational b) { return a + (-b); }
            public static Rational operator -(Rational a, long b)
            {
                Rational c = new Rational(b);
                return a - c;
            }
            public static Rational operator *(Rational a, Rational b) { return new Rational(a.m * b.m, a.n * b.n); }
            public static Rational operator /(Rational a, Rational b) { return new Rational(a.m * b.n, a.n * b.m); }
            public static bool operator ==(Rational a, Rational b) { return (a.m == b.m) && (a.n == b.n); }
            public static bool operator !=(Rational a, Rational b) { return !(a == b); }

        }

        /// <summary>
        /// Комплексные числа
        /// </summary>
        public struct Complex : IComparable
        {
            static double _2PI;
            static Complex()
            {
                I = new Complex(0, 1);
                _2PI = 2 * Math.PI;
            }
            //координаты
            /// <summary>
            /// Первая координата точки
            /// </summary>
            private double x;
            /// <summary>
            /// Вторая координата точки
            /// </summary>
            private double y;

            /// <summary>
            /// По действительному числу составить комплексное
            /// </summary>
            /// <param name="a"></param>
            public Complex(double a) { x = a; y = 0; }//по действительному числу
                                                      /// <summary>
                                                      /// Составить комплексное число по паре действительных чисел
                                                      /// </summary>
                                                      /// <param name="a"></param>
                                                      /// <param name="b"></param>
            public Complex(double a, double b) { x = a; y = b; }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="p"></param>
            public Complex(Complex p) { x = p.Re; y = p.Im; }

            //свойства
            /// <summary>
            /// Действительная часть
            /// </summary>
            public double Re
            {
                get { return x; }
                set { x = value; }
            }
            /// <summary>
            /// Мнимая часть
            /// </summary>
            public double Im
            {
                get { return y; }
                set { y = value; }
            }
            /// <summary>
            /// Модуль
            /// </summary>
            public double Abs
            {
                get
                {
                    Complex o = new Complex();
                    Complex t = new Complex(this.Re, this.Im);
                    return Point.Eudistance((Point)t, (Point)o);
                }
            }
            /// <summary>
            /// Аргумент
            /// </summary>
            public double Arg
            {
                get
                {
                    System.Numerics.Complex r = new System.Numerics.Complex(this.x, this.y);
                    return r.Phase;

                    double argument;
                    Point d = new Point();
                    d.x = (this.x < 1e-13) ? 0 : this.x;
                    d.y = (this.y < 1e-13) ? 0 : this.y;

                    if (d.x == 0)
                    {
                        argument = Math.PI / 2 * Math.Sign((sbyte)d.y);
                    }
                    else
                    {
                        if (d.y == 0)
                        {
                            argument = Math.PI * Math.Sign(Math.Sign((sbyte)d.x) - 1);
                        }
                        else
                        {
                            argument = Math.Atan(d.y / d.x) + Math.Sign((sbyte)Math.Abs(d.x) - d.x) * Math.Sign((sbyte)d.y) * Math.PI;
                        }
                    }
                    return argument;
                }
            }
            /// <summary>
            /// Мнимая единица
            /// </summary>
            public static Complex I;

            /// <summary>
            /// Мнимая часть комплексного числа
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            public static double Imag(Complex t) => t.Im;

            /// <summary>
            /// Комплексно-сопряжённое число
            /// </summary>
            /// <returns></returns>
            public Complex Conjugate => new Complex(this.Re, -this.Im);

            /// <summary>
            /// Перевести в строку вида a+bi
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string res = "";

                if (this.Re == 0 && this.Im == 0) res = "0";
                else
                {
                    if (this.Re != 0.0)
                    {
                        res = this.Re.ToString() + " ";
                    }

                    if (this.Im != 0.0)
                    {
                        if (this.Im > 0)
                        {
                            res += "+ " + this.Im.ToString() + "i"; ;
                        }
                        else res += "- " + this.Im.Abs().ToString() + "i";
                    }
                }

                return res;
            }

            /// <summary>
            /// Неявное преобразование действительного числа в комплексное
            /// </summary>
            /// <param name="x"></param>
            public static implicit operator Complex(double x) => new Complex(x, 0);
            /// <summary>
            /// Явное преобразование комплексного числа в действительное (в модуль)
            /// </summary>
            /// <param name="c"></param>
            public static explicit operator double(Complex c) => c.Re;

            public static implicit operator Complex(System.Numerics.Complex c) => new Complex(c.Real, c.Imaginary);

            //Перегруженные операторы сложения
            /// <summary>
            /// Сумма комплексных чисел
            /// </summary>
            /// <param name="c1"></param>
            /// <param name="c2"></param>
            /// <returns></returns>
            public static Complex operator +(Complex c1, Complex c2)
            {
                try { return new Complex(c1.Re + c2.Re, c1.Im + c2.Im); }
                catch (Exception e) { throw new Exception(e.Message); }
            }

            public static Complex operator +(Complex c1, double c2)
            {
                return new Complex(c1.Re + c2, c1.Im);
            }

            public static Complex operator +(double c1, Complex c2)
            {
                return new Complex(c1 + c2.Re, c2.Im);
            }

            //Перегруженные операторы вычитания
            public static Complex operator -(Complex c1, Complex c2)
            {
                try { return new Complex(c1.Re - c2.Re, c1.Im - c2.Im); }
                catch (Exception e) { throw new Exception(e.Message); }
            }
            public static Complex operator -(Complex z) { return new Complex(-z.Re, -z.Im); }

            public static Complex operator -(Complex c1, double c2)
            {
                return new Complex(c1.Re - c2, c1.Im);
            }

            public static Complex operator -(double c1, Complex c2)
            {
                return new Complex(c1 - c2.Re, -c2.Im);
            }

            //Перегруженные операторы умножения
            public static Complex operator *(Complex c1, Complex c2)
            {
                return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Re * c2.Im + c1.Im * c2.Re);
            }

            public static Complex operator *(Complex c1, double c2)
            {
                return new Complex(c1.Re * c2, c1.Im * c2);
            }

            public static Complex operator *(double c1, Complex c2)
            {
                return new Complex(c1 * c2.Re, c1 * c2.Im);
            }

            //Перегруженные операторы деления
            public static Complex operator /(Complex c1, Complex c2)
            {
                double Denominator = c2.Re * c2.Re + c2.Im * c2.Im;
                return new Complex((c1.Re * c2.Re + c1.Im * c2.Im) / Denominator,
                    (c2.Re * c1.Im - c2.Im * c1.Re) / Denominator);
            }

            public static Complex operator /(Complex c1, double c2)
            {
                return new Complex(c1.Re / c2, c1.Im / c2);
            }

            public static Complex operator /(double c1, Complex c2)
            {
                double Denominator = c2.Re * c2.Re + c2.Im * c2.Im;
                return new Complex((c1 * c2.Re) / Denominator, (-c2.Im * c1) / Denominator);
            }

            //логические операторы
            public static bool operator ==(Complex c1, Complex c2)
            {
                return c1.Re == c2.Re && c1.Im == c2.Im;
            }

            public static bool operator !=(Complex c1, Complex c2)
            {
                return c1.Re != c2.Re || c1.Im != c2.Im;
            }

            public static Complex[] Minus(Complex[] r)
            {
                Complex[] res = new Complex[r.Length];
                for (int i = 0; i < res.Length; i++)
                    res[i] = -r[i];
                return res;
            }

            /// <summary>
            /// Совпадение комплексных чисел
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return this == (Complex)obj;
            }

            public override int GetHashCode()
            {
                return this.Re.GetHashCode() + this.Im.GetHashCode();
            }

            /// <summary>
            /// Сумма комплексного вектора с постоянным комклексным числом(покомпонентное сложение)
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static Complex[] Sum(Complex[] x, Complex y)
            {
                Complex[] r = new Complex[x.Length];
                for (int i = 0; i < r.Length; i++)
                    r[i] = x[i] + y;
                return r;
            }
            /// <summary>
            /// Сумма комплексных векторов
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static Complex[] Sum(Complex[] x, Complex[] y)
            {
                Complex[] r = new Complex[x.Length];
                for (int i = 0; i < r.Length; i++)
                    r[i] = x[i] + y[i];
                return r;
            }
            /// <summary>
            /// Произведение комплексного вектора на число
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static Complex[] Mult(Complex x, Complex[] y)
            {
                Complex[] r = new Complex[y.Length];
                for (int i = 0; i < r.Length; i++)
                    r[i] = x * y[i];
                return r;
            }
            /// <summary>
            /// Произведение действительного вектора на комплексное число
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static Complex[] Mult(Complex x, double[] y)
            {
                Complex[] r = new Complex[y.Length];
                for (int i = 0; i < r.Length; i++)
                    r[i] = x * y[i];
                return r;
            }
            /// <summary>
            /// Перевод действительного массива в конмплексный
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public static Complex[] ToComplexMas(double[] x)
            {
                Complex[] c = new Complex[x.Length];
                for (int i = 0; i < c.Length; i++)
                    c[i] = x[i];
                return c;
            }

            /// <summary>
            /// Комплексная экспонента
            /// </summary>
            /// <param name="z"></param>
            /// <returns></returns>
            public static Complex Exp(Complex z)
            {/* z.Arg.Show(); */
                //z = new Complex(z.Re, z.Im.ToPeriod(_2PI));
                return Math.Exp(z.Re) * new Complex(Math.Cos(z.Im), Math.Sin(z.Im));
            }
            /// <summary>
            /// Комплексный синус
            /// </summary>
            /// <param name="z"></param>
            /// <returns></returns>
            public static Complex Sin(Complex z)
            {
                //z = new Complex(z.Re.ToPeriod(_2PI), z.Im);
                return (Exp(I * z) - Exp(-I * z)) / 2 / I;
            }
            /// <summary>
            /// Комплексный косинус
            /// </summary>
            /// <param name="z"></param>
            /// <returns></returns>
            public static Complex Cos(Complex z)
            {
                //z = new Complex(z.Re.ToPeriod(_2PI), z.Im);
                return (Exp(I * z) + Exp(-I * z)) / 2;
            }

            /// <summary>
            /// Многозначный радикал
            /// </summary>
            /// <param name="z"></param>
            /// <param name="k"></param>
            /// <returns></returns>
            public static Complex[] Radical(Complex z, int k)
            {
                Complex[] r = new Complex[k];
                double mod = Math.Pow(z.Abs, 1.0 / k);
                for (int i = 0; i < k; i++)
                    r[i] = mod * Exp(I * (z.Arg + 2 * Math.PI * i) / k);
                return r;

            }
            /// <summary>
            /// Главное значение радикала
            /// </summary>
            /// <param name="z"></param>
            /// <param name="k"></param>
            /// <returns></returns>
            public static Complex Radicalk(Complex z, int k)
            {
                return Math.Pow(z.Abs, 1.0 / k) * Exp(I * z.Arg / k);
                // return Radical(z, k).Where(c => c.Re*c.Im==0).ToArray()[0];
            }
            /// <summary>
            /// Главное значение квадратного корня
            /// </summary>
            /// <param name="z"></param>
            /// <returns></returns>
            public static Complex Sqrt(Complex z) => Math.Sqrt(z.Abs) * Exp(I * z.Arg / 2)/*System.Numerics.Complex.Sqrt(new System.Numerics.Complex(z.Re, z.Im))*/;
            /// <summary>
            /// Поменять мнимую и действительную часть местами, выведя результат
            /// </summary>
            public Complex Swap => new Complex(this.y, this.x);

            public static Complex Sqrt1(Complex z) => new Complex(Math.Sqrt(z.Abs), 0); //Sqrt(z);//
            public static Complex Sqrt2(Complex z) => -I * Sqrt1(z);
            public static Complex SqrtNew(Complex z)
            {
                Complex tmp = Sqrt(z);
                if (tmp.Re >= 0 && tmp.Im <= 0) return tmp;
                // return tmp.Swap.Conjugate;
                return -I * tmp;
            }

            /// <summary>
            /// Возведение в степень
            /// </summary>
            /// <param name="z"></param>
            /// <param name="k"></param>
            /// <returns></returns>
            public static Complex Pow(Complex z, int k) => Math.Pow(z.Abs, k) * Exp(I * k * z.Arg);
            /// <summary>
            /// Возведение в степень
            /// </summary>
            /// <param name="z"></param>
            /// <param name="k"></param>
            /// <returns></returns>
            public static Complex Pow(Complex z, double k) => Math.Pow(z.Abs, k) * Exp(I * k * z.Arg);
            /// <summary>
            /// Гиперболический синус
            /// </summary>
            /// <param name="z"></param>
            /// <returns></returns>
            public static Complex Sh(Complex z)
            {
                //z = new Complex(z.Re, z.Im.ToPeriod(_2PI));
                return 0.5 * (Exp(z) - Exp(-z));
            }
            /// <summary>
            /// Гиперболический косинус
            /// </summary>
            /// <param name="z"></param>
            /// <returns></returns>
            public static Complex Ch(Complex z)
            {
                //z.Show();
                //z = new Complex(z.Re, z.Im.ToPeriod(_2PI));
                return 0.5 * (Exp(z) + Exp(-z));
            }

            public static double VectorNorm(Complex[] p)
            {
                double sum = 0;
                for (int i = 0; i < p.Length; i++) sum += p[i].Abs;
                return sum;
            }

            public static explicit operator Complex(Point p) => new Complex(p.x, p.y);
            public int CompareTo(object obj)
            {
                Complex c = (Complex)obj;
                Point a = new Point(this.Re, this.Im);
                Point b = new Point(c.Re, c.Im);
                return a.CompareTo(b);
                throw new NotImplementedException();
            }

            /// <summary>
            /// Способ отображения комплексного числа в действительное
            /// </summary>
            public enum ComplMode
            {
                Re,
                Im,
                Abs,
                Arg
            }
            /// <summary>
            /// Сумма действительной и мнимой части
            /// </summary>
            public double ReIm => this.Re + this.Im;
        }
    }

    /// <summary>
    /// Класс полиномов вида a+bx+...
    /// </summary>
    public class Polynom
    {
        /// <summary>
        /// Степень полинома
        /// </summary>
        private int? degree;
        /// <summary>
        /// Массив коэффициентов полинома в порядке возрастания степеней
        /// </summary>
        public double[] coef;
        /// <summary>
        /// Массив точек, через которые проходит полином
        /// </summary>
        public Point[] points;

        //конструкторы
        /// <summary>
        /// Никакой полином
        /// </summary>
        public Polynom() { degree = null; coef = null; points = null; }//по умолчанию (хотя зачем?)
                                                                       /// <summary>
                                                                       /// Конструктор копирования
                                                                       /// </summary>
                                                                       /// <param name="p"></param>
        public Polynom(Polynom p)
        {
            this.degree = p.degree;
            this.coef = new double[p.coef.Length];
            this.points = new Point[p.coef.Length];//p.SavePoints();
            for (int i = 0; i <= p.degree; i++) { this.coef[i] = p.coef[i]; /*this.points[i] = p.points[i]; */}
        }
        /// <summary>
        /// Задать полином через массив коэффициентов
        /// </summary>
        /// <param name="c"></param>
        public Polynom(double[] c)
        {
            this.degree = c.Length - 1;
            this.coef = new double[c.Length];
            for (int i = 0; i <= this.degree; i++) this.coef[i] = c[i];
        }
        /// <summary>
        /// Задать полином по вектору коэффициентов
        /// </summary>
        /// <param name="v"></param>
        public Polynom(Vectors v)
        {
            this.degree = v.n - 1;
            this.coef = new double[v.n];
            for (int i = 0; i <= this.degree; i++) this.coef[i] = v[i];
        }
        /// <summary>
        /// Прочитать массив коэффициентов полинома из файла и задать полином
        /// </summary>
        /// <param name="fs"></param>
        public Polynom(StreamReader fs)
        {
            string s = fs.ReadToEnd();
            fs.Close();
            string[] st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа

            this.degree = st.Length - 1;
            this.coef = new double[st.Length];
            for (int i = 0; i <= this.degree; i++) this.coef[i] = Convert.ToDouble(st[i]);
        }
        /// <summary>
        /// Нулевой полином нужной степени
        /// </summary>
        /// <param name="deg"></param>
        public Polynom(int deg)
        {
            this.degree = deg;
            this.coef = new double[deg + 1];
        }
        /// <summary>
        /// Приведённый одночлен по единственному корню (то есть 1(х-х0))
        /// </summary>
        /// <param name="x">Корень одночлена</param>
        public Polynom(double x)
        {
            this.degree = 1;
            this.coef = new double[2];
            //коэффициенты для приведённого полинома первой степени
            this.coef[0] = -x;
            this.coef[1] = 1;
        }
        /// <summary>
        /// Задать полином по старшему коэффициенту и набору его корней
        /// </summary>
        /// <param name="aN"></param>
        /// <param name="x"></param>
        public Polynom(double aN, params double[] x)//суть метода: последовательно умножать многочлен на одночлены
        {
            this.degree = (x.Length);
            this.coef = new double[x.Length + 1];
            //коэффициенты для приведённого полинома первой степени
            this.coef[0] = -x[0];
            this.coef[1] = 1;
            if (x.Length > 1)
            {//вычисление коэффициентов полинома степени k
             //Console.WriteLine("{0}", this.coef[0]);
                for (int k = 2; k <= this.degree; k++)
                {
                    this.coef[k] = this.coef[k - 1];
                    for (int i = k - 1; i > 0; i--) this.coef[i] = this.coef[i - 1] - this.coef[i] * x[k - 1];
                    this.coef[0] *= -x[k - 1];
                }
            }
            //умножение на старший коэффициент
            for (int i = 0; i <= this.degree; i++) this.coef[i] *= aN;
        }
        /// <summary>
        /// Создание полинома по старшему коэффициенту и вектору корней
        /// </summary>
        /// <param name="aN"></param>
        /// <param name="x"></param>
        public Polynom(double aN, Vectors x)
        {
            double[] root = Vectors.ToDoubleMas(x);
            Polynom p = new Polynom(aN, root);
            this.degree = p.degree;
            this.coef = new double[p.coef.Length];
            this.points = new Point[p.coef.Length];//p.SavePoints();
            for (int i = 0; i <= p.degree; i++) this.coef[i] = p.coef[i];
        }
        /// <summary>
        /// Полином (Лагранжа), проходящий через точки массива
        /// </summary>
        /// <param name="p"></param>
        public Polynom(Point[] p)//интерполяционных полином Лагранжа, проходящий через точки из массива p; представляется как сумма n+1 полиномов Pk, для которых известны корни  
        {
            this.degree = p.Length - 1;
            this.coef = new double[p.Length];
            Polynom PL = new Polynom((int)this.degree);//конечный полином
            double[] roots = new double[(int)this.degree];//массив корней

            if (p.Length == 1) PL = Polynom.ToPolynom(p[0].y);//если узел один
            else
            {
                for (int k = 0; k <= this.degree; k++)
                {   //задание корней полиномов Pk
                    for (int i = 0; i < k; i++) roots[i] = p[i].x;
                    for (int i = k + 1; i <= this.degree; i++) roots[i - 1] = p[i].x;

                    Polynom c = new Polynom(1, roots);//приведённый полином

                    double An = p[k].y / c.Value(p[k].x);//вычисление старшего коэффициента
                    Polynom Pk = new Polynom(An, roots);//создание полинома Pk
                    PL += Pk;//прибавление к общему
                }
            }
            this.points = new Point[this.coef.Length];
            for (int k = 0; k <= this.degree; k++) { this.coef[k] = PL.coef[k]; this.points[k] = p[k]; }
        }
        /// <summary>
        /// Интерполяционный полином функции f с n+1 узлами интерполяции (значит, n-й степени) на отрезке от a до b
        /// </summary>
        /// <param name="f">Интерполируемая функция</param>
        /// <param name="n">Степень полинома</param>
        /// <param name="a">Начало отрезна интерполирования</param>
        /// <param name="b">Конец отрезка интерполирования</param>
        public Polynom(RealFunc f, int n, double a, double b)
        {
            Polynom p = new Polynom(Point.Points(f, n, a, b));
            this.degree = p.degree;
            this.coef = new double[(int)(p.degree + 1)];
            for (int k = 0; k <= this.degree; k++) this.coef[k] = p.coef[k];
        }

        //операции
        public static Polynom operator +(Polynom a, Polynom b)//сложение полиномов
        {
            int degree = Math.Max((sbyte)a.degree, (sbyte)b.degree);
            double[] coef = new double[degree + 1];
            for (int i = 0; i <= Math.Min((sbyte)a.degree, (sbyte)b.degree); i++) coef[i] = a.coef[i] + b.coef[i];
            for (int i = Math.Min((sbyte)a.degree, (sbyte)b.degree) + 1; i <= degree; i++)
            {
                if (a.degree > b.degree) coef[i] = a.coef[i];
                else coef[i] = b.coef[i];
            }
            return new Polynom(coef);
        }
        public static Polynom operator +(Polynom a, double Ch) { return a + ToPolynom(Ch); }
        public static Polynom operator -(Polynom a)//унарная операция -
        {
            Polynom p = new Polynom(a);
            for (int i = 0; i <= p.degree; i++) p.coef[i] *= -1;
            return p;
        }
        public static Polynom operator -(Polynom a, Polynom b)//разность полиномов
        {
            return new Polynom(a + (-b));
        }
        public static Polynom operator *(Polynom a, Polynom b)//произведение полиномов
        {
            int degree = (int)(a.degree + b.degree);
            double[] coef = new double[degree + 1];
            for (int i = 0; i <= degree; i++)
            {
                double s = 0;
                for (int k = 0; k <= i; k++) if ((k <= a.degree) && ((i - k) <= b.degree)) s += a.coef[k] * b.coef[i - k];
                coef[i] = s;
            }
            return new Polynom(coef);
        }
        public static Polynom operator *(Polynom t, double x)//произведение полинома с числом
        {
            Polynom e = new Polynom(t);
            for (int i = 0; i <= t.degree; i++) e.coef[i] *= x;
            return e;
        }
        public static Polynom operator *(double x, Polynom t)
        {
            return t * x;
        }
        public static Polynom operator /(Polynom t, double x)//деление полинома на число
        {
            return t * (1.0 / x);
        }
        public static Polynom operator /(Polynom end, Polynom er)//деление полиномов "нацело"
        {
            double[] quotient, remainder;
            Deconv(end.coef, er.coef, out quotient, out remainder);
            return new Polynom(quotient);
        }
        public static Polynom operator %(Polynom end, Polynom er)//остаток от деления полиномов
        {
            double[] quotient, remainder;
            Deconv(end.coef, er.coef, out quotient, out remainder);
            return new Polynom(remainder);
        }
        public static Polynom operator |(Polynom p, int k)//производная полинома
        {
            if (k >= p.degree + 1) return Polynom.ToPolynom(0);
            if (k == 0) return p;

            double[] d = new double[(int)p.degree - k + 1];
            if (k > 0)
                for (int i = 0; i < d.Length; i++)
                    d[i] = p.coef[k + i] * Combinatorik.A(i, k + i);
            else
                for (int i = 0; i < p.coef.Length; i++)
                    d[-k + i] = p.coef[i] / Combinatorik.A(i, -k + i);

            return new Polynom(d);
        }
        public static bool operator !=(Polynom p, Polynom q)
        {
            if (p.degree != q.degree) return true;
            for (int i = 0; i <= p.degree; i++) if (p.coef[i] != q.coef[i]) return true;
            return false;
        }
        public static bool operator ==(Polynom p, Polynom q) { return !(p != q); }

        public override bool Equals(object obj)
        {
            return this == (Polynom)obj;
        }

        public override int GetHashCode()
        {
            double s = 0;
            for (int i = 0; i <= this.degree; i++) s += this.coef[i];
            return (int)s;
        }


        //методы
        private static void Deconv(double[] dividend, double[] divisor, out double[] quotient, out double[] remainder)//деление многочлена на многочлен с выводом остатка и частного
        {
            if (dividend.Last() == 0)
            {
                throw new ArithmeticException("Старший член многочлена делимого не может быть 0");
            }
            if (divisor.Last() == 0)
            {
                throw new ArithmeticException("Старший член многочлена делителя не может быть 0");
            }
            remainder = (double[])dividend.Clone();
            quotient = new double[remainder.Length - divisor.Length + 1];
            for (int i = 0; i < quotient.Length; i++)
            {
                double coeff = remainder[remainder.Length - i - 1] / divisor.Last();
                quotient[quotient.Length - i - 1] = coeff;
                for (int j = 0; j < divisor.Length; j++)
                {
                    remainder[remainder.Length - i - j - 1] -= coeff * divisor[divisor.Length - j - 1];
                }
            }
        }
        /// <summary>
        /// Сохранить в массив точки, через которые проходит полином
        /// </summary>
        private void SavePoints()
        {
            this.points = new Point[this.coef.Length];//Console.WriteLine(this.points[1].x);
            double h = 20.0 / (this.points.Length);
            for (int i = 0; i < this.points.Length; i++)
            {
                this.points[i] = new Point(-10 + h * i, this.Value(-10 + h * i));
                //this.points[i].x = -10 + h * i;
                //this.points[i].y = this.Value(this.points[i].x);
            }
        }
        /// <summary>
        /// Разделённая разность без рекурсии
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static double W(Point[] p)
        {
            double sum = 0, pow = 1;
            for (int j = 0; j < p.Length; j++)
            {
                for (int l = 0; l < p.Length; l++)
                    if (j != l) pow *= p[j].x - p[l].x;
                sum += p[j].y / pow;
                pow = 1;
            }
            return sum;
        }
        /// <summary>
        /// Разделённая разность по массиву точек (с рекурсией)
        /// </summary>
        /// <param name="p">Массив точек</param>
        /// <param name="i">Номер начального элемента в разности</param>
        /// <param name="j">Номер конечного элемента в разности</param>
        /// <returns></returns>
        internal static double W(Point[] p, int i, int j)
        {
            if (j - i == 1) return (p[j].y - p[i].y) / (p[j].x - p[i].x);
            return (W(p, i + 1, j) - W(p, i, j - 1)) / (p[j].x - p[i].x);
        }

        /// <summary>
        /// Вычисление значения в точке по схеме Горнера
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Value(double x)//вычисление значения в точке по схеме Горнера
        {
            double sum = this.coef[(int)this.degree];
            if (this.degree > 0)
            {
                for (int i = (int)(this.degree - 1); i >= 0; i--)
                {
                    sum *= x;
                    sum += this.coef[i];
                }
            }

            return sum;
        }
        /// <summary>
        /// Полином от матрицы
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public SqMatrix Value(SqMatrix A)
        {
            SqMatrix I = SqMatrix.I(A.n);
            SqMatrix sum = this.coef[(int)this.degree] * I;
            if (this.degree > 0)
            {
                for (int i = (int)(this.degree - 1); i >= 0; i--)
                {
                    sum *= A;
                    sum += this.coef[i] * I;
                }
            }

            return sum;
        }
        /// <summary>
        /// Полином от полинома
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public Polynom Value(Polynom A)
        {
            Polynom sum = Polynom.ToPolynom(this.coef[(int)this.degree]);
            if (this.degree > 0)
            {
                for (int i = (int)(this.degree - 1); i >= 0; i--)
                {
                    sum *= A;
                    sum += this.coef[i];
                }
            }

            return sum;
        }

        /// <summary>
        /// Вывод полинома на консоль
        /// </summary>
        public void Show() { Console.WriteLine(this.ToString()); }
        /// <summary>
        /// Вывод полинома с рациональными коэффициентами на консоль
        /// </summary>
        public void ShowRational() { Console.WriteLine(this.ToStringRational()); }
        /// <summary>
        /// Преобразование полинома в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            for (int i = (int)(this.degree); i > 0; i--)
            {
                s += String.Format("{0}x^{1} + ", this.coef[i], i);
            }
            s += String.Format("{0}"/*x^{1}*/, this.coef[0]/*, 0*/);
            return s;
        }
        /// <summary>
        /// Преобразовать полином в строку с отображением рациональных коэффициентов
        /// </summary>
        /// <returns></returns>
        public string ToStringRational()
        {
            string s = "";
            for (int i = (int)(this.degree); i > 0; i--)
            {
                s += String.Format("({0}x^{1}) + ", Number.Rational.ToRational(this.coef[i]), i);
            }
            s += String.Format("{0}"/*x^{1}*/, Number.Rational.ToRational(this.coef[0])/*, 0*/);
            return s;
        }
        /// <summary>
        /// Приведённый полином по текущему полиному
        /// </summary>
        /// <returns></returns>
        public Polynom ToLeadPolynom()
        {
            Polynom p = new Polynom(this);
            double tmp = p.coef[(int)p.degree];
            for (int i = (int)p.degree; i >= 0; i--) p.coef[i] /= tmp;
            return p;
        }
        /// <summary>
        /// Приведённый полином такого-то полинома
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Polynom ToLeadPolynom(Polynom p) { return p.ToLeadPolynom(); }
        /// <summary>
        /// Перевод числа в полином нулевой степени с соответствующим коэффициентом
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Polynom ToPolynom(double x) //перевод числа в полином нулевой степени с соответствующим коэффициентом
        {
            Polynom p = new Polynom(0);
            if (x == 0) return p;
            p.coef[0] = x;
            return p;
        }
        /// <summary>
        /// Перевод массива чисел в полином
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Polynom ToPolynom(double[] x) { return new Polynom(x); }
        /// <summary>
        /// Полином, близкий к производной функции f порядка k
        /// </summary>
        /// <param name="f"></param>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Polynom Derivative(RealFunc f, int n, double a, double b, int k)
        {
            Polynom p = new Polynom(f, n + k, a, b);
            return p | k;
        }
        /// <summary>
        /// Характеристический многочлен заданной матрицы
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static Polynom CharactPol(SqMatrix M)
        {
            Polynom p = new Polynom(M.n);

            double sum;
            SqMatrix A = M;
            //заполнение массива треков
            double[] tr = new double[A.n];
            for (int i = 0; i < A.n; i++)
            {
                tr[i] = A.Track;
                A *= M;
            }

            p.coef[(int)p.degree] = 1 * Math.Pow(-1, A.n);
            int k = 0;
            for (int i = (int)p.degree - 1; i >= 0; i--)
            {
                sum = 0; k++;
                for (int j = 0; j < k; j++) sum += tr[k - j - 1] * p.coef[(int)p.degree - j];
                sum *= -1;
                sum /= k;
                p.coef[i] = sum;
            }

            return p;
        }
        /// <summary>
        /// Добавить ещё одну точку, через которую проходит полином (методами Ньютона)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Polynom AddPoint(Point a)
        {
            Polynom p = new Polynom(this);
            if (p.Value(a.x) == a.y) return p;

            Point[] mas = new Point[p.points.Length + 1];
            double[] xmas = new double[mas.Length - 1];
            for (int i = 0; i < mas.Length - 1; i++)
            {
                mas[i] = p.points[i];
                xmas[i] = mas[i].x;
            }
            mas[mas.Length - 1] = a; //Point.Show(mas);

            Polynom w = new Polynom(Polynom.W(mas), xmas);
            Polynom q = p + w;
            q.points = new Point[mas.Length]; for (int i = 0; i < mas.Length; i++) q.points[i] = mas[i];
            return q;
        }
        /// <summary>
        /// Точное вычисление определённого интеграла по формуле Ньютона-Лейбница
        /// </summary>
        /// <param name="a">Начало отрезка интегрирования</param>
        /// <param name="b">Конец отрезка интегрирования</param>
        /// <returns></returns>
        public double S(double a, double b)
        {
            Polynom p = this | -1;
            return p.Value(b) - p.Value(a);
        }

        //специальные полиномы
        /// <summary>
        /// Интерполяционных полином Лагранжа, проходящий через точки из массива p
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Polynom Lag(Point[] p) { return new Polynom(p); }
        /// <summary>
        /// Интерполяционный полином функции f с n+1 узлами интерполяции (значит, n-й степени) на отрезке от a до b
        /// </summary>
        /// <param name="f"></param>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynom Lag(RealFunc f, int n, double a, double b) { return new Polynom(f, n, a, b); }
        /// <summary>
        /// Вывод полинома Чебышёва соответсвующего рода и степени
        /// </summary>
        /// <param name="r"></param>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static Polynom Cheb(int deg, Kind r = Kind.FirstKind)//полиномы Чебышёва; можно уменьшить код, так как отличаются только полиномы степени 1, но тогда что делать с проверкой на род, которых может быть больше двух?
        {
            if (r == Kind.FirstKind)
            {
                switch (deg)
                {
                    case 0:
                        return Polynom.ToPolynom(1);
                    case 1:
                        return new Polynom(0.0);
                    default:
                        Polynom p = (new Polynom(0.0)) * 2;
                        return p * Polynom.Cheb(deg - 1, r) - Polynom.Cheb(deg - 2, r);
                }
            }
            if (r == Kind.SecondKind)
            {
                switch (deg)
                {
                    case 0:
                        return Polynom.ToPolynom(1);
                    case 1:
                        return (new Polynom(0.0)) * 2;
                    default:
                        Polynom p = (new Polynom(0.0)) * 2;
                        return p * Polynom.Cheb(deg - 1, r) - Polynom.Cheb(deg - 2, r);
                }
            }
            return Polynom.ToPolynom(0);
        }
        /// <summary>
        /// Полином Лежандра
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static Polynom Lezh(int deg)
        {
            switch (deg)
            {
                case 0:
                    return Polynom.ToPolynom(1);
                case 1:
                    return new Polynom(0.0);
                default:
                    Polynom p = (new Polynom(0.0));
                    return p * Polynom.Lezh(deg - 1) * (2 * deg - 1) / deg - Polynom.Lezh(deg - 2) * (deg - 1) / deg;
            }
        }
        /// <summary>
        /// Полиномы Лагерра
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static Polynom Lagerr(int deg)
        {
            switch (deg)
            {
                case 0:
                    return Polynom.ToPolynom(1);
                case 1:
                    return new Polynom(new double[] { 1, -1 });
                default:
                    Polynom p = new Polynom(new double[] { 2 * (deg - 1) + 1, -1 });
                    return p * Polynom.Lagerr(deg - 1) - Polynom.Lagerr(deg - 2) * (deg - 1) * (deg - 1);
            }
        }
        /// <summary>
        /// Полиномы Эрмита (ортогональные)
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static Polynom Hermit(int deg)
        {
            switch (deg)
            {
                case 0:
                    return Polynom.ToPolynom(1);
                case 1:
                    return new Polynom(new double[] { 0, 2 });
                default:
                    Polynom p = new Polynom(new double[] { 0, 2 });
                    return p * Polynom.Hermit(deg - 1) - Polynom.Hermit(deg - 2) * (deg - 1) * 2;
            }
        }
        /// <summary>
        /// Полином Ньютона через разделённые разности по массиву точек
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Polynom Neu(Point[] P/*,out Polynom[] np*/)
        {
            Polynom pol = Polynom.ToPolynom(P[0].y);//Первый элемент суммы полиномов
                                                    //np = new Polynom[P.Length];

            double[][] mas = new double[P.Length][];
            for (int i = 0; i < P.Length; i++)
            {
                mas[i] = new double[i + 1];
                for (int j = 0; j <= i; j++) mas[i][j] = P[j].x;//Заполнить массив массивов корней
            }
            for (int i = 0; i < P.Length - 1; i++)
            {
                pol += new Polynom(W(P, 0, i + 1), mas[i]);//Просуммировать полиномы
                                                           //np[i] = new Polynom(pol);
            }

            return pol;
        }

        /// <summary>
        /// Полином Ньютона через разделённые разности по массиву точек
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Polynom NeuNew(Point[] P, out Polynom[] np, out Polynom[] pn)
        {
            Polynom pol = Polynom.ToPolynom(P[0].y);//Первый элемент суммы полиномов
            np = new Polynom[P.Length];
            pn = new Polynom[P.Length];
            np[0] = new Polynom(pol);
            //pn[pn.Length-1]= new Polynom(pol);


            double[][] mas = new double[P.Length][];
            for (int i = 0; i < P.Length; i++)
            {
                mas[i] = new double[i + 1];
                for (int j = 0; j <= i; j++) mas[i][j] = P[j].x;//Заполнить массив массивов корней
            }
            for (int i = 0; i < P.Length - 1; i++)
            {
                pol += new Polynom(W(P, 0, i + 1), mas[i]);//Просуммировать полиномы
                np[i + 1] = new Polynom(pol);
            }

            pn[pn.Length - 1] = new Polynom(W(P, 0, P.Length - 1), mas[P.Length - 2]);
            for (int i = P.Length - 3; i >= 0; i--) pn[i + 1] = pn[i + 1 + 1] + new Polynom(W(P, 0, i + 1), mas[i]);
            pn[0] = pn[1] + np[0];
            //np[P.Length - 1] = new Polynom(pol);

            return pol;
        }

        /// <summary>
        /// Объект для хранения вспомогательной системы
        /// </summary>
        public static SLAU syst = null;
        /// <summary>
        /// Строковое представление рациональной функции
        /// </summary>
        public static string Rat = null;
        /// <summary>
        /// Интерполяция рациональной функцией по точкам
        /// </summary>
        /// <param name="P">Массив точек</param>
        /// <param name="p">Степень полинома в числителе</param>
        /// <param name="q">Степень полинома в знаменателе</param>
        /// <param name="bq">Старший коэффициент в знаменателе</param>
        /// <returns></returns>
        public static RealFunc R(Point[] P, int p, int q, double bq = 1)
        {
            if (p + q + 1 != P.Length) throw new Exception("Не выполняется равенство p+q+1=n !");
            if (bq == 0) throw new Exception("Старший коэффициент полинома не может быть нулевым!");

            Matrix M = new Matrix(P.Length, P.Length + 1);
            for (int i = 0; i < M.n; i++)
            {
                M[i, P.Length] = P[i].y * bq * Math.Pow(P[i].x, q);
                for (int j = 0; j <= p; j++) M[i, j] = Math.Pow(P[i].x, j);
                for (int j = p + 1; j < M.m - 1; j++) M[i, j] = -P[i].y * Math.Pow(P[i].x, j - p - 1);
            }

            SLAU S = new SLAU(M);
            //S.Show();
            S.Gauss();
            syst = S;
            //S.Show();
            Vectors numerator = new Vectors(p + 1);
            Vectors denominator = new Vectors(q + 1);
            for (int i = 0; i <= p; i++) numerator[i] = S.x[i];
            for (int i = 0; i < q; i++) denominator[i] = S.x[p + 1 + i];
            denominator[q] = bq;
            Polynom num = new Polynom(numerator);
            Polynom den = new Polynom(denominator);
            //num.Show();den.Show();
            Rat = String.Format("({0}) / ({1})", num.ToString(), den.ToString());
            return (double x) => { return num.Value(x) / den.Value(x); };
        }

        /// <summary>
        /// Строковое представление сплайна (массив полиномов)
        /// </summary>
        public static string[] SplinePol = null;
        /// <summary>
        /// Первая и вторая производные сплайна
        /// </summary>
        public static RealFunc DSpline = null, D2Spline = null;
        /// <summary>
        /// Максимальный шаг между двумя соседними точками при интерполяции сплайном
        /// </summary>
        public static double hmax;
        /// <summary>
        /// Интерполяция кубическими сплайнами дефекта 1 по массиву точек
        /// </summary>
        /// <param name="P"></param>
        /// <param name="a">Граничное условие в начале отрезка</param>
        /// <param name="b">Граничное условие в конце отрезка</param>
        /// <param name="is0outcut">Должен ли сплайн равняться 0 вне отрезка задания</param>
        /// <returns></returns>
        public static RealFunc CubeSpline(Point[] P, double a = 0, double b = 0, bool is0outcut = false)
        {
            int n = P.Length - 1;//записать в новую переменную для облегчения
            double[] h = new double[n + 1];
            double[] y = new double[n + 1];

            hmax = P[1].x - P[0].x;

            for (int i = 1; i <= n; i++)
            {
                h[i] = P[i].x - P[i - 1].x;//Заполнение массива длин отрезков
                if (h[i] > hmax) hmax = h[i];
                y[i] = P[i].y - P[i - 1].y;
            }

            //создание, заполнение и решение системы с трёхдиагональной матрицей
            SLAU S = new SLAU(n + 1);
            S.A[0, 0] = -4.0 / h[1]; S.A[0, 1] = -2.0 / h[1]; S.b[0] = -6.0 * y[1] / (h[1] * h[1]) + a;
            S.A[n, n - 1] = 2.0 / h[n]; S.A[n, n] = 4.0 / h[n]; S.b[n] = 6.0 * y[n] / (h[n] * h[n]) + b;
            for (int i = 1; i <= n - 1; i++)
            {
                S.A[i, i - 1] = 1.0 / h[i];
                S.A[i, i] = 2 * (1.0 / h[i] + 1.0 / h[i + 1]);
                S.A[i, i + 1] = 1.0 / h[i + 1];
                S.b[i] = 3 * (y[i] / h[i] / h[i] + y[i + 1] / h[i + 1] / h[i + 1]);
            }
            //S.b[0] = a;
            //S.b[n] = b;
            S.ProRace();
            syst = S;
            //S.Show();

            //создание и заполнение массива полиномов
            Polynom[] mas = new Polynom[n + 1];
            Polynom.SplinePol = new string[n];
            for (int i = 1; i <= n; i++)
            {
                Polynom p1, p2, p3, p4;
                p1 = (new Polynom(1, P[i].x, P[i].x)) * (2 * new Polynom(P[i - 1].x) + h[i]) / Math.Pow(h[i], 3) * P[i - 1].y;
                p2 = (new Polynom(1, P[i - 1].x, P[i - 1].x)) * (-2 * new Polynom(P[i].x) + h[i]) / Math.Pow(h[i], 3) * P[i].y;
                p3 = (new Polynom(1, P[i].x, P[i].x)) * (new Polynom(P[i - 1].x)) / Math.Pow(h[i], 2) * S.x[i - 1];
                p4 = (new Polynom(1, P[i - 1].x, P[i - 1].x)) * (new Polynom(P[i].x)) / Math.Pow(h[i], 2) * S.x[i];
                mas[i] = p1 + p2 + p3 + p4;
                SplinePol[i - 1] = String.Format("[{0};{1}]: {2}", P[i - 1].x, P[i].x, mas[i].ToString());
                //mas[i].Show();
            }
            //mas[0] = mas[1];mas[n + 1] = mas[n];

            //создание производных сплайна
            Polynom[] mas1 = new Polynom[n + 1], mas2 = new Polynom[n + 1];
            for (int i = 1; i <= n; i++)
            {
                mas1[i] = mas[i] | 1;
                mas2[i] = mas1[i] | 1;
            }
            DSpline = (double x) =>
              {
                  if (x <= P[1].x) return mas1[1].Value(x);
                  if (x >= P[n].x) return mas1[n].Value(x);
                  int i = 1;
                  while (x > P[i].x) i++;
                  return mas1[i].Value(x);
              };
            D2Spline = (double x) =>
             {
                 if (x <= P[1].x) return mas2[1].Value(x);
                 if (x >= P[n].x) return mas2[n].Value(x);
                 int i = 1;
                 while (x > P[i].x) i++;
                 return mas2[i].Value(x);
             };

            //создание общей функции и вывод
            return (double x) =>
            {
                if (x <= P[1].x) return (is0outcut) ? 0 : mas[1].Value(x);
                if (x >= P[n].x) return (is0outcut) ? 0 : mas[n].Value(x);
                int i = 1;
                //while (x > P[i].x) i++;
                int i1 = 1, i2 = n;
                //реализация бинарного поиска
                while (i2 - i1 != 1)
                {
                    int tmp = Expendator.Average(i1, i2);
                    if (x > P[tmp].x) i1 = tmp;
                    else i2 = tmp;
                }
                i = i2;
                return mas[i].Value(x);
            };
        }
        /// <summary>
        /// Полиномы Эрмита для набора кратных узлов
        /// </summary>
        /// <param name="mas"></param>
        /// <returns></returns>
        public static Polynom Hermit(params MultipleKnot[] mas)
        {
            int n = -1;//dergee of Pol
            for (int i = 0; i < mas.Length; i++) n += mas[i].Multiplicity;

            SLAU S = new SLAU(n + 1);//S.Show();
            int k = 0;
            for (int i = 0; i < mas.Length; i++)
            {
                for (int j = 0; j < mas[i].Multiplicity; j++)
                {
                    S.b[k + j] = mas[i].y[j];
                    for (int t = 0; t <= n - j; t++)
                    {
                        int s = n - j - t;
                        S.A[k + j, t] = Combinatorik.A(s, j + s) * Math.Pow(mas[i].x, s);
                    }
                }
                k += mas[i].Multiplicity;
            }
            S.GaussSelection();
            //S.Show();
            Array.Reverse(S.x);
            return new Polynom(S.x);
        }
        /// <summary>
        /// Оценка погрешности метода
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="Mn"></param>
        /// <returns></returns>
        private static double wn(Point[] p, double x, double Mn = 1)
        {
            double e = x - p[0].x;
            for (int i = 1; i < p.Length; i++) e *= (x - p[i].x);
            e /= Combinatorik.P(p.Length);
            e *= Mn;
            return Math.Abs(e);
        }
        /// <summary>
        /// Оценка погрешности метода
        /// </summary>
        /// <param name="f"></param>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="Mn"></param>
        /// <returns></returns>
        private static double wn(RealFunc f, int n, double a, double b, double x, double Mn)
        {
            Point[] p = Point.Points(f, n, a, b);
            return wn(p, x, Mn);
        }
        /// <summary>
        /// Оценить погрешность интерполяционного полинома (Лагранжа) в точке при константе
        /// </summary>
        /// <param name="f">Интерполируемая функция</param>
        /// <param name="n">Стень полинома</param>
        /// <param name="a">Начало отрезка интерполирования</param>
        /// <param name="b">Конец отрезка интерполяции</param>
        /// <param name="x">Точка, в которой оценивается погрешность</param>
        /// <param name="Mn">Константа в погрешности</param>
        public static void LagEstimateErr(RealFunc f, int n, double a, double b, double x, double Mn = 0)
        {
            if (Mn <= 0)
            {
                double[] y = Point.PointsX(f, n, a, b);
                double q = Expendator.Min(y);
                double y1 = Math.Min(q, x);
                q = Expendator.Max(y);
                double y2 = Math.Max(q, x);
                //double e = y1 + (y2 - y1) / 2;//середина отрезка
                //Mn = Math.Abs(f(e));
                Mn = FuncMethods.RealFuncMethods.NormC(f, y1, y2);
            }
            Polynom p = new Polynom(f, n, a, b);

            Console.WriteLine("Узлы интерполяции: "); Point.Show(Point.Points(f, n, a, b));

            Console.WriteLine("Полученный полином: "); p.Show();
            Console.WriteLine($"Значение полинома в точке {x} = {p.Value(x)}");
            Console.WriteLine($"Значение функции в точке {x} = {f(x)}");
            double t = Math.Abs(p.Value(x) - f(x));
            Console.WriteLine($"Абсолютная величина погрешности в точке {x} = {t}");
            Console.WriteLine($"Оценка погрешности при Mn = {Mn}: {t} <= {wn(f, n, a, b, x, Mn)}");
        }
        /// <summary>
        /// Показать последовательно значение слагаемых в сумме полинома Ньютона в точке x
        /// </summary>
        /// <param name="h"></param>
        /// <param name="f"></param>
        /// <param name="x"></param>
        public static void ShowNeuNew(Point[] h, RealFunc f, double x)
        {
            Polynom[] u, v;
            Polynom r = Polynom.NeuNew(h, out u, out v);

            Console.WriteLine("Узлы интерполяции:");
            for (int i = 0; i < h.Length; i++)
                h[i].Show();
            Console.WriteLine();

            Console.WriteLine("Значение функции в точке {0} равно {1}", x, f(x));
            Console.WriteLine("Значение сумм полиномов (при суммировании от меньшего к большему):");
            for (int i = 0; i < h.Length; i++)
            {
                Console.WriteLine("P[сумма №{0}](в {1})= {2}", i + 1, x, u[i].Value(x));
            }
            Console.WriteLine("Значение сумм полиномов (при суммировании от большего к меньшему):");
            for (int i = 0; i < h.Length; i++)
            {
                //v[h.Length - 1 - i].Show();
                Console.WriteLine("P[сумма №{0}](в {1})= {2}", i + 1, x, v[h.Length - 1 - i].Value(x));
            }
        }

        /// <summary>
        /// Скалярное произведение между полиномами на отрезке
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double ScalarP(Polynom p, Polynom q, double a, double b)
        {
            Polynom pol = p * q;
            double tmp = 1;
            if (b != a) { tmp /= Math.Abs(b - a); }
            return pol.S(a, b) * tmp;
        }
        /// <summary>
        /// Скалярное произведение между полиномами на отрезке
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double NormL(Polynom x, double a, double b) { return Math.Sqrt(ScalarP(x, x, a, b)); }
        /// <summary>
        /// Расстояние между полиномами на отрезке
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Polynom p, Polynom q, double a, double b) { return NormL(p - q, a, b); }

        /// <summary>
        /// Показать информацию о интерполяции указанной функции методами класса полиномов
        /// </summary>
        /// <param name="f"></param>
        /// <param name="k">Число узлов интерполяции</param>
        /// <param name="a">Начало отрезка с узлами</param>
        /// <param name="b">Конец отрезка с узлами</param>
        /// <param name="p">Степень числителя у рациональной функции</param>
        /// <param name="q">Степень знаменателя у рациональной функции</param>
        /// <param name="bq">Старший коэффициент знаменателя рациональной функции</param>
        public static void PolynomTestShow(RealFunc f, int k, double a = -10, double b = 10, int p = 1, int q = -1, double bq = 1)
        {
            if (q == -1) q = k - 1 - p;
            Point[] P = Point.Points(f, k - 1, a, b);
            Console.WriteLine("Набор узлов интерполяции:"); Point.Show(P);
            Polynom l = Polynom.Lag(P);//l.Show();
            Polynom n = Polynom.Neu(P);
            RealFunc c = Polynom.CubeSpline(P);
            RealFunc r = Polynom.R(P, p, q, bq);

            Console.WriteLine("Погрешности в равномерной норме:");
            Console.WriteLine("У полинома Лагранжа {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, l.Value, a, b));
            Console.WriteLine("У полинома Ньютона {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, n.Value, a, b));
            Console.WriteLine("У рациональной функции {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, r, a, b));
            Console.WriteLine("У кубического сплайна {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, c, a, b));
            Console.WriteLine();
            Console.WriteLine("Погрешности в интегральной норме:");
            Console.WriteLine("У полинома Лагранжа {0}", FuncMethods.RealFuncMethods.NormDistance(f, l.Value, a, b));
            Console.WriteLine("У полинома Ньютона {0}", FuncMethods.RealFuncMethods.NormDistance(f, n.Value, a, b));
            Console.WriteLine("У рациональной функции {0}", FuncMethods.RealFuncMethods.NormDistance(f, r, a, b));
            Console.WriteLine("У кубического сплайна {0}", FuncMethods.RealFuncMethods.NormDistance(f, c, a, b));

        }
        /// <summary>
        /// Показать информацию о интерполяции указанной функции методами класса полиномов
        /// </summary>
        /// <param name="f"></param>
        /// <param name="c">Массив абцисс узлов интерполяции</param>
        /// <param name="a">Начало отрезка с узлами</param>
        /// <param name="b">Конец отрезка с узлами</param>
        /// <param name="p">Степень числителя у рациональной функции</param>
        /// <param name="q">Степень знаменателя у рациональной функции</param>
        /// <param name="bq">Старший коэффициент знаменателя рациональной функции</param>
        public static void PolynomTestShow(RealFunc f, double[] c, double a = 0, double b = 0, int p = 1, int q = -1, double bq = 1)
        {
            if (a == 0 && b == 0) { a = c[0]; b = c[c.Length - 1]; }
            if (q == -1) q = c.Length - 1 - p;
            Point[] P = Point.Points(f, c);
            Console.WriteLine("Набор узлов интерполяции:"); Point.Show(P);
            Polynom l = Polynom.Lag(P);//l.Show();
            Polynom n = Polynom.Neu(P);
            RealFunc cu = Polynom.CubeSpline(P);
            RealFunc r = Polynom.R(P, p, q, bq);

            Console.WriteLine("Погрешности в равномерной норме:");
            Console.WriteLine("У полинома Лагранжа {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, l.Value, a, b));
            Console.WriteLine("У полинома Ньютона {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, n.Value, a, b));
            Console.WriteLine("У рациональной функции {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, r, a, b));
            Console.WriteLine("У кубического сплайна {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, cu, a, b));
            Console.WriteLine();
            Console.WriteLine("Погрешности в интегральной норме:");
            Console.WriteLine("У полинома Лагранжа {0}", FuncMethods.RealFuncMethods.NormDistance(f, l.Value, a, b));
            Console.WriteLine("У полинома Ньютона {0}", FuncMethods.RealFuncMethods.NormDistance(f, n.Value, a, b));
            Console.WriteLine("У рациональной функции {0}", FuncMethods.RealFuncMethods.NormDistance(f, r, a, b));
            Console.WriteLine("У кубического сплайна {0}", FuncMethods.RealFuncMethods.NormDistance(f, cu, a, b));

        }

    }

    //------------------------------------------матрицы
    /// <summary>
    /// Произвольных размеров матрицы
    /// </summary>
    public class Matrix
    {
        //размерность и сам массив
        /// <summary>
        /// Число столбцов в матрице
        /// </summary>
        protected internal int m;
        /// <summary>
        /// Число строк в матрице
        /// </summary>
        protected internal int n;
        /// <summary>
        /// Массив, отождествлённый с матрицей
        /// </summary>
        protected internal double[,] matrix;

        //Свойства-методы
        /// <summary>
        /// Обращение к матрице как к двумерному массиву
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            get { return matrix[i, j]; }
            set { matrix[i, j] = value; }
        }

        /// <summary>
        /// Обращение к матрице как к одномерному массиву (при условии, что в ней число строк либо число столбцов равно 1)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double this[int i]
        {
            get
            {
                if (this.n == 1) return this[0, i];
                else if (this.m == 1) return this[i, 0];
                else throw new Exception(String.Format("Матрица размерности {0}x{1} не конвертируется в вектор, поэтому к её элементам нельзя обращаться как к векторам", n, m));
            }
            set
            {

            }
        }

        /// <summary>
        /// Количество строк в матрице
        /// </summary>
        public int RowCount => this.n;
        /// <summary>
        /// Количество столбцов в матрице
        /// </summary>
        public int ColCount => this.m;

        //Конструктор
        /// <summary>
        /// Матрица (0)
        /// </summary>
        public Matrix()//по умолчанию
        {
            this.n = this.m = 0;
            this.matrix = new double[n, m];
        }

        /// <summary>
        /// Квадратная нулевая матрица
        /// </summary>
        /// <param name="n">Размерность матрицы</param>
        public Matrix(int n)//по размерности (квадратная)
        {
            this.n = this.m = n;
            this.matrix = new double[n, n];
        }
        /// <summary>
        /// Прямоугольная нулевая матрица
        /// </summary>
        /// <param name="n">Число строк</param>
        /// <param name="m"></param>
        public Matrix(int n, int m)
        {
            this.n = n;
            this.m = m;
            this.matrix = new double[n, m];
        }
        /// <summary>
        /// Матрица из файла
        /// </summary>
        /// <param name="fs"></param>
        public Matrix(StreamReader fs)//через файл
        {

            string s = fs.ReadLine();
            string[] st = s.Split(' ', '\t');
            st = st.Where(n => n.Length != 0).ToArray();
            this.n = Convert.ToInt32(st[0]);
            try { this.m = Convert.ToInt32(st[1]); } catch { this.m = this.n; }
            this.matrix = new double[n, m];

            for (int k = 0; k < this.n; k++)
            {
                s = fs.ReadLine();
                s = s.Replace('.', ',');
                st = s.Split(' ', '\t');//в аргументах указывается массив символов, которым разделяются числа
                st = st.Where(n => n.Length != 0).ToArray();
                //st.Show();
                // try
                // {
                for (int i = 0; i < this.m; i++) this.matrix[k, i] = Convert.ToDouble(st[i]);
                //}
                //catch { throw new Exception("Тут"); }
            }


            fs.Close();
        }
        /// <summary>
        /// Генерировние матрицы по массиву её строк
        /// </summary>
        /// <param name="l"></param>
        public Matrix(Vectors[] l)
        {
            this.n = l.Length; this.m = l[0].n;
            for (int i = 0; i < this.n; i++)
                for (int j = 0; j < this.m; j++)
                    this[i, j] = l[i].vector[j];
        }
        /// <summary>
        /// Создать матрицу как вектор-столбец
        /// </summary>
        /// <param name="b"></param>
        public Matrix(double[] b)
        {
            this.n = b.Length;
            this.m = 1;
            this.matrix = new double[n, m];
            for (int i = 0; i < b.Length; i++) this.matrix[i, 0] = b[i];
        }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="M"></param>
        public Matrix(Matrix M)
        {
            this.m = M.m;
            this.n = M.n;
            this.matrix = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    this.matrix[i, j] = M[i, j];
        }
        /// <summary>
        /// Матрица по двумерному массиву
        /// </summary>
        /// <param name="mas"></param>
        public Matrix(double[,] mas)
        {
            this.n = mas.GetLength(0);
            this.m = mas.GetLength(1);
            this.matrix = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    this.matrix[i, j] = mas[i, j];
        }
        /// <summary>
        /// Создать матрицу по её размерности и массиву диагональных элементов
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="m">Число строк</param>
        /// <param name="n">Число столбцов</param>
        public Matrix(double[] mas, int m, int n) : this(m, n)
        {
            for (int i = 0; i < mas.Length; i++)
                this.matrix[i, i] = mas[i];
        }
        /// <summary>
        /// Преобразовать вектор в матрицу
        /// </summary>
        /// <param name="v"></param>
        public Matrix(Vectors v) : this(v.n, 1)
        {
            for (int i = 0; i < v.n; i++)
                this[i, 0] = v[i];
        }

        //методы
        /// <summary>
        /// Задание матрицы с помощью консоли
        /// </summary>
        public /*virtual*/ void CreateMatrix()//задать коэффициенты в матрице через консоль
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write("Введите элемент [" + i.ToString() + ";" + j.ToString() + "]" + "\t");
                    matrix[i, j] = Convert.ToDouble(Console.ReadLine());
                }
            }

        }
        /// <summary>
        /// Вывести матрицу на консоль
        /// </summary>
        public /*virtual*/ void PrintMatrix()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(matrix[i, j].ToString() + " \t");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Число нулей в матрице
        /// </summary>
        protected internal int NullValue
        {
            get
            {
                int val = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (matrix[i, j] == 0) val++;
                    }
                }
                return val;
            }
        }
        /// <summary>
        /// Нулевая ли матрица?
        /// </summary>
        /// <returns></returns>
        public /*virtual*/ bool Nulle()
        {
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < m; j++)
            //    {
            //        if (matrix[i, j] != 0) return false;
            //    }
            //}

            return this.NullValue == n * m;
        }
        /// <summary>
        /// Отнять от строки матрицы другую строку (i-k*j)
        /// </summary>
        /// <param name="i">Номер строки, от которой отнимают</param>
        /// <param name="j">Номер строки, которая отнимается</param>
        /// <param name="val">Коэффициент, на который умножается отнимаемая строка</param>
        public void LinesDiff(int i, int j, double val)
        {
            for (int k = 0; k < this.m; k++) this[i, k] -= val * this[j, k];
        }
        /// <summary>
        /// Отнять от строки матрицы вектор
        /// </summary>
        /// <param name="i"></param>
        /// <param name="c"></param>
        public void MinusVector(int i, Vectors c)
        {
            for (int j = 0; j < this.ColCount; j++)
                this[i, j] -= c[j];
        }

        /// <summary>
        /// Переставить строки
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void LinesSwap(int i, int j)
        {
            for (int k = 0; k < this.m; k++)
            {
                double tmp = this[i, k];
                this[i, k] = this[j, k];
                this[j, k] = tmp;
            }
        }
        /// <summary>
        /// Переставить столбцы
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void ColumnSwap(int i, int j)
        {
            for (int k = 0; k < this.n; k++)
            {
                double tmp = this[k, i];
                this[k, i] = this[k, j];
                this[k, j] = tmp;
            }
        }
        /// <summary>
        /// Удалить столбец матрицы
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Matrix ColumnDelete(int k)
        {
            Matrix A = new Matrix(this.n, this.m - 1);
            for (int i = 0; i < this.n; i++)
                for (int j = 0; j < k; j++)
                    A[i, j] = this[i, j];
            for (int i = 0; i < this.n; i++)
                for (int j = k + 1; j < this.m; j++)
                    A[i, j - 1] = this[i, j];
            return A;
        }
        /// <summary>
        /// Транспонирование матрицы
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static Matrix Transpose(Matrix M)
        {
            Matrix MT = new Matrix(M.m, M.n);
            for (int i = 0; i < MT.n; i++)
                for (int j = 0; j < MT.m; j++) MT[i, j] = M[j, i];
            return MT;
        }
        /// <summary>
        /// Возврат транспонированной матрицы
        /// </summary>
        /// <returns></returns>
        public virtual Matrix Transpose() { return Matrix.Transpose(this); }
        /// <summary>
        /// Норма Фробениуса
        /// </summary>
        public double Frobenius
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < this.n; i++)
                    for (int j = 0; j < this.m; j++)
                        sum += this[i, j] * this[i, j];
                return Math.Sqrt(sum);
            }
        }
        /// <summary>
        /// Перевести строку матрицы в вектор
        /// </summary>
        /// <returns></returns>
        public Vectors GetLine(int k)
        {
            Vectors v = new Vectors(this.m);
            for (int j = 0; j < v.n; j++) v[j] = this[k, j];
            return v;
        }
        /// <summary>
        /// Перевести строку матрицы в вектор
        /// </summary>
        /// <returns></returns>
        public Vectors GetLine(int k, int beg, int end)
        {
            Vectors v = new Vectors(end - beg + 1);
            for (int j = beg; j <= end; j++) v[j - beg] = this[k, j];
            return v;
        }

        /// <summary>
        /// Прибавить ко всеми элементам строки число
        /// </summary>
        /// <param name="line">Номер строки</param>
        /// <param name="val">Число, которое прибавляется</param>
        public void AddByLine(int line, double val)
        {
            line--;
            for (int j = 0; j < this.ColCount; j++)
                this[line, j] += val;
        }
        /// <summary>
        /// Прибавить ко всем элементам столбца число
        /// </summary>
        /// <param name="col">Номер столбца</param>
        /// <param name="val"></param>
        public void AddByColumn(int col, double val)
        {
            col--;
            for (int j = 0; j < this.RowCount; j++)
                this[j, col] += val;
        }
        /// <summary>
        /// Поделить строку в матрице на число
        /// </summary>
        /// <param name="i"></param>
        /// <param name="val"></param>
        public void DivByLine(int i, double val)
        {
            for (int j = 0; j < this.ColCount; j++)
                this[i, j] /= val;
        }

        /// <summary>
        /// Кубическая норма
        /// </summary>
        public double CubeNorm
        {
            get
            {
                double[] mas = new double[this.RowCount];
                for (int i = 0; i < mas.Length; i++)
                    for (int j = 0; j < this.ColCount; j++)
                        mas[i] += Math.Abs(this[i, j]);
                return mas.Max();
            }
        }
        /// <summary>
        /// Октаэдрическая норма
        /// </summary>
        public double OctNorn
        {
            get
            {
                double[] mas = new double[this.ColCount];
                for (int i = 0; i < mas.Length; i++)
                    for (int j = 0; j < this.RowCount; j++)
                        mas[i] += Math.Abs(this[j, i]);
                return mas.Max();
            }
        }
        /// <summary>
        /// Максимальная абсолютная величина в матрице
        /// </summary>
        public double MaxofMod
        {
            get
            {
                double[] mas = new double[this.RowCount];
                for (int i = 0; i < mas.Length; i++)
                {
                    double[] mas2 = new double[this.ColCount];
                    for (int j = 0; j < this.ColCount; j++)
                        mas2[j] = Math.Abs(this[i, j]);
                    mas[i] = mas2.Max();
                }

                return mas.Max();
            }
        }

        public double Min
        {
            get
            {
                Vectors[] v = new Vectors[this.RowCount];
                double[] mas = new double[v.Length];
                for (int i = 0; i < n; i++)
                {
                    v[i] = this.GetLine(i);
                    mas[i] = v[i].Min;
                }
                return mas.Min();
            }
        }
        public double Max => -(-this).Min;

        //операторы
        //сложение
        public static Matrix operator +(Matrix A, Matrix B)
        {
            if ((A.n != B.n) || (A.m != B.m)) return new Matrix();
            Matrix C = new Matrix(A.n, B.m);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < B.m; j++)
                {
                    C[i, j] = A[i, j] + B[i, j];
                }
            }
            return C;
        }

        //вычитание
        public static Matrix operator -(Matrix A, Matrix B)
        {
            if ((A.n != B.n) || (A.m != B.m)) return new Matrix();
            Matrix R = new Matrix(A.n, B.m);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.n; j++)
                {
                    R[i, j] = A[i, j] - B[i, j];
                }
            }
            return R;

        }
        public static Matrix operator -(Matrix A)
        {
            Matrix R = new Matrix(A);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.m; j++)
                {
                    R[i, j] *= -1;
                }
            }
            return R;
        }

        //произведение
        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.m != B.n) return new Matrix();
            Matrix R = new Matrix(A.n, B.m);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < B.m; j++)
                {
                    for (int k = 0; k < B.n; k++)
                    {
                        R[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return R;
        }

        //Умножение на число
        public static Matrix operator *(Matrix A, double Ch)
        {
            Matrix q = new Matrix(A.n, A.m);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.n; j++)
                {
                    q[i, j] = A[i, j] * Ch;
                }
            }
            return q;
        }

        //public static implicit operator SqMatrix(Matrix M) => new SqMatrix(M.matrix);

        /// <summary>
        /// Сингулярное разложение матрицы через библиотеку alglib
        /// </summary>
        /// <param name="A">Исходная матрица</param>
        /// <param name="U">Левая матрица</param>
        /// <param name="w">Вектор сингулярных чисел</param>
        /// <param name="VT">Правая матрица</param>
        public static void SVD(Matrix A, out Matrix U, out double[] w, out Matrix VT)
        {
            double[,] mas = A.matrix, u = new double[1, 1], vt = new double[1, 1];
            w = new double[0];
            bool resjust = alglib.svd.rmatrixsvd(mas, A.RowCount, A.ColCount, 2, 2, 2, ref w, ref u, ref vt);
            U = new Matrix(u);
            VT = new Matrix(vt);
        }
    }

    /// <summary>
    /// Квадратные матрицы
    /// </summary>
    public class SqMatrix : Matrix
    {
        //Конструктор
        /// <summary>
        /// Матрица (0)
        /// </summary>
        public SqMatrix() : base()//по умолчанию
        { }
        /// <summary>
        /// Нулевая квадратная матрица
        /// </summary>
        /// <param name="n">Размерность матрицы</param>
        public SqMatrix(int n) : base(n, n)//по размерности
        { }
        /// <summary>
        /// Считать матрицу из файла
        /// </summary>
        /// <param name="fs"></param>
        public SqMatrix(StreamReader fs) : base(fs)//через файл
        { }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="M"></param>
        public SqMatrix(SqMatrix M)
        {
            this.n = this.m = M.n;
            this.matrix = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) this[i, j] = M[i, j];
        }
        /// <summary>
        /// Создание матрицы по двумерному массиву
        /// </summary>
        /// <param name="S"></param>
        public SqMatrix(int[,] S)
        {
            if (S.GetLength(0) != S.GetLength(1)) throw new Exception("Таблица не является квадратной!");
            this.m = this.n = S.GetLength(0);
            this.matrix = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) this[i, j] = S[i, j];
        }
        /// <summary>
        /// Создание матрицы по двумерному массиву
        /// </summary>
        /// <param name="S"></param>
        public SqMatrix(double[,] S)
        {
            if (S.GetLength(0) != S.GetLength(1)) throw new Exception("Таблица не является квадратной!");
            this.m = this.n = S.GetLength(0);
            this.matrix = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) this[i, j] = S[i, j];
        }
        /// <summary>
        /// Создать матрицу как угловую подматрицу размерности k
        /// </summary>
        /// <param name="A"></param>
        /// <param name="k"></param>
        public SqMatrix(SqMatrix A, int k) : base(k, k)
        {
            for (int i = 0; i < this.n; i++)
                for (int j = 0; j < this.n; j++)
                    this[i, j] = A[i, j];
        }
        /// <summary>
        /// Создать матрицу как угловую подматрицу размерности k
        /// </summary>
        /// <param name="A"></param>
        /// <param name="k"></param>
        public SqMatrix(double[,] A, int k) : base(k, k)
        {
            for (int i = 0; i < this.n; i++)
                for (int j = 0; j < this.n; j++)
                    this[i, j] = A[i, j];
        }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="M"></param>
        public SqMatrix(Matrix M) : base(M) { }
        /// <summary>
        /// Создание матрицы по одномерному массиву
        /// </summary>
        /// <param name="mas"></param>
        public SqMatrix(double[] mas)
        {
            this.n = this.m = (int)Math.Sqrt(mas.Length);
            this.matrix = new double[n, n];
            int y = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) this[i, j] = mas[y++];
        }

        /// <summary>
        /// Единичная матрица
        /// </summary>
        public static SqMatrix I(int n)
        {
            SqMatrix A = new SqMatrix(n);
            for (int i = 0; i < n; i++) A[i, i] = 1;
            return A;
        }

        //методы

        ///// <summary>
        ///// Задать коэффициенты в матрице через консоль
        ///// </summary>
        //public override void CreateMatrix()
        //{

        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            Console.Write("Введите элемент [" + i.ToString() + ";" + j.ToString() + "]" + "\t");
        //            matrix[i, j] = Convert.ToDouble(Console.ReadLine());
        //        }
        //    }

        //}
        ///// <summary>
        ///// Вывести матрицу на консоль
        ///// </summary>
        //public override void PrintMatrix()
        //{
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            Console.Write(matrix[i, j].ToString() + " \t");
        //        }
        //        Console.WriteLine();
        //    }
        //}

        /// <summary>
        /// Диагональная ли матрица?
        /// </summary>
        /// <returns></returns>
        public bool Diagonal()
        {
            uint r = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if ((matrix[i, j] != 0) && (matrix[j, i] != 0)) return false;
                    if (matrix[i, i] != 0) r++;
                }
            }
            if (r > 0) return true;
            return false;
        }
        /// <summary>
        /// Является ли матрица симметрической
        /// </summary>
        /// <returns></returns>
        public bool IsSymmetric()
        {
            for (int i = 0; i < this.n; i++)
                for (int j = 0; j < this.n; j++)
                    if (this[i, j] != this[j, i]) return false;
            return true;
        }
        /// <summary>
        /// Является ли матрица положительно определённой
        /// </summary>
        /// <returns></returns>
        public bool IsPositCertain()
        {
            //if (!this.IsSymmetric()) return false;

            for (int i = 0; i < this.n; i++)
            {
                SqMatrix M = new SqMatrix(this, i + 1);
                double s = M.Det;
                if (s <= 0) return false;
            }
            return true;
        }
        /// <summary>
        /// Является ли матрица тридиагональной
        /// </summary>
        /// <returns></returns>
        public bool IsTreeDiag()
        {
            for (int i = 0; i < this.n; i++)
                for (int j = i + 2; j < this.n; j++)
                    if (this[i, j] != 0 || this[j, i] != 0) return false;
            return true;
        }

        //public override bool Nulle()//нулевая ли матрица?
        //{
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            if (matrix[i, j] != 0) return false;
        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// Единичная матрица
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static SqMatrix E(int n)
        {
            SqMatrix M = new SqMatrix(n);
            for (int i = 0; i < n; i++) M[i, i] = 1;
            return M;
        }
        /// <summary>
        /// Определитель матрицы
        /// </summary>
        public double Det
        {
            get
            {
                SqMatrix matrix = new SqMatrix(this);

                double m = 0;
                for (int j = 0; j < this.n; j++)
                {
                    for (int i = j + 1; i < this.n; i++)
                    {
                        if (matrix[j, j] != 0)
                        {
                            m = matrix[i, j] / matrix[j, j];

                            for (int h = j; h < this.n; h++)
                                matrix[i, h] -= m * matrix[j, h];
                        }
                    }
                }
                m = 1;
                for (int i = 0; i < this.n; i++) m *= matrix[i, i];
                // PrintMatrix();
                return m;
            }
        }
        /// <summary>
        /// Минор элемента матрицы (точнее, алгебраическое дополнение)
        /// </summary>
        /// <returns></returns>
        public double Minor(int i, int j)
        {
            if ((i >= n) || (j >= n)) throw new Exception("Вызов элемента, которого нет в матрице");
            if (this.n <= 1) throw new Exception("Размерность матрицы слишком мала");
            if (this.n == 2) return Math.Pow(-1, i + j) * this[3 - i - 2, 3 - j - 2];
            //int Inew = i--,Jnew=j--;
            SqMatrix M = new SqMatrix(this.n - 1);
            int a = 0, b = 0;
            for (int ii = 0; ii < this.n; ii++)
            {
                if (ii != i)
                {
                    for (int jj = 0; jj < this.n; jj++)
                    {
                        if (jj != j)
                        {
                            M[a, b] = this[ii, jj]; //Console.WriteLine("{0} {1} {2} {3}", a,b,ii,jj);
                            b++;
                        }
                    }
                    a++; b = 0;
                }
            }
            //M.PrintMatrix();
            return Math.Pow(-1, i + 1 + j + 1) * M.Det;

        }

        /// <summary>
        /// Трек матрицы
        /// </summary>
        /// <returns></returns>
        public double Track
        {
            get
            {
                double s = 0;
                for (int i = 0; i < this.n; i++) s += this[i, i];
                return s;
            }
        }
        /// <summary>
        /// Характеристический многочлен заданной матрицы
        /// </summary>
        public Polynom CharactPol
        {
            get { return Polynom.CharactPol(this); }
        }
        /// <summary>
        /// Обратная матрица
        /// </summary>
        public SqMatrix Reverse//из теоремы Гамильтона-Кели
        {
            get
            {
                //if (this.Det == 0) throw new ArithmeticException("Матрица вырождена");
                SqMatrix M = SqMatrix.E(this.n);
                SqMatrix A = new SqMatrix(this);
                Polynom p = this.CharactPol;
                M *= p.coef[1];
                for (int i = 2; i <= n; i++)
                {
                    M += p.coef[i] * A;
                    A *= this;
                }
                M *= -1;
                M /= p.coef[0];
                return M;
            }
        }
        /// <summary>
        /// Обратная матрица по Гауссу
        /// </summary>
        /// <returns></returns>
        public SqMatrix Invert()
        {
            SqMatrix mResult = SqMatrix.E(this.ColCount);
            /*
             * Получать "1" на элементе главной диагонали, а потом
             * Занулять оставшиеся элементы
             * */
            SqMatrix mCur = new SqMatrix(this);
            //mCur.PrintMatrix(); Console.WriteLine();
            //mResult.PrintMatrix(); Console.WriteLine();

            for (int i = 0; i < this.ColCount; i++) //Цикл по строкам сверху-вниз
            {
                //Заединичить вервую строку
                double dItem = mCur[i, i];
                mCur.DivByLine(i, dItem);
                mResult.DivByLine(i, dItem);

                //mCur.PrintMatrix(); Console.WriteLine();
                //mResult.PrintMatrix(); Console.WriteLine();

                Vectors rTmp = mCur.GetLine(i);
                Vectors eTmp = mResult.GetLine(i);
                //Забить нулями вертикаль
                for (int j = 0; j < this.ColCount; j++)
                    if (i != j)
                    {
                        double con = mCur[j, i];
                        mCur.MinusVector(j, rTmp * con);
                        mResult.MinusVector(j, eTmp * con);
                    }
                //mCur.PrintMatrix(); Console.WriteLine();
                //mResult.PrintMatrix(); Console.WriteLine();
            }

            return mResult;
        }
        /// <summary>
        /// Уточнение обратной матрицы
        /// </summary>
        /// <param name="A">Исходная матрица</param>
        /// <param name="Reverse">Обратная марица</param>
        /// <param name="eps">Точность</param>
        /// <returns></returns>
        public static SqMatrix ReverseCorrect(SqMatrix A, SqMatrix Reverse, double eps = 0.001, int stepcount = 1000, bool existnorm = false)
        {
            SqMatrix E = SqMatrix.E(A.RowCount), R = new SqMatrix(Reverse);
            int i = 0;
            if ((E - A * R).CubeNorm < 1 || existnorm)
                while ((E - A * R).CubeNorm > eps && i < stepcount)
                {
                    R *= (2 * E - A * R);
                    //(E - A * R).CubeNorm.Show();
                    i++;
                }
            return R;
        }

        /// <summary>
        /// Квадратная подматрица, порождённая пересечением таких строк и столбцов
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        /// <remarks>Нумерация строк должна начинаться с единицы</remarks>
        public SqMatrix SubMatrix(params int[] m)
        {
            SqMatrix M = new SqMatrix(m.Length);
            for (int i = 0; i < m.Length; i++)
                for (int j = 0; j < m.Length; j++)
                    M[i, j] = this[m[i] - 1, m[j] - 1];
            return M;
        }

        /// <summary>
        /// Замена столбца матрицы на указанный вектор (для метода Крамера)
        /// </summary>
        /// <param name="ColumnNumber">Номер стоблца, начиная с 1</param>
        /// <param name="NewColumn">Сам вектор (если вектор)</param>
        /// <remarks>Если вектор короткий, заменится лишь часть колонны, а если длинный, будет исключение</remarks>
        /// <returns></returns>
        public SqMatrix ColumnSwap(int ColumnNumber, Vectors NewColumn)
        {
            SqMatrix mat = new SqMatrix(this);
            if (ColumnNumber > mat.ColCount || ColumnNumber <= 0 || NewColumn.n > mat.RowCount) throw new Exception("В матрице нет столбца с таким номером либо вектор слишком длинный");
            ColumnNumber--;
            for (int i = 0; i < NewColumn.n; i++)
                mat[i, ColumnNumber] = NewColumn[i];
            return mat;
        }

        public SqMatrix Transpose()
        {
            return new SqMatrix(base.Transpose().matrix);
        }
        /// <summary>
        /// Подобная матрица, если задана ортогональная матрица преобразования
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public SqMatrix ConvertToSimilar(SqMatrix M, bool directly = true)
        {
            if (directly)
                return (SqMatrix)(M.Transpose() * this * M);
            else
                return (SqMatrix)(M * this * M.Transpose());
        }

        //операторы
        //сложение
        public static SqMatrix operator +(SqMatrix A, SqMatrix B)
        {
            SqMatrix C = new SqMatrix(A.n);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.n; j++)
                {
                    C[i, j] = A[i, j] + B[i, j];
                }
            }
            return C;
        }

        //вычитание
        public static SqMatrix operator -(SqMatrix A, SqMatrix B)
        {
            SqMatrix R = new SqMatrix(A.n);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.n; j++)
                {
                    R[i, j] = A[i, j] - B[i, j];
                }
            }
            return R;

        }

        //произведение
        public static SqMatrix operator *(SqMatrix A, SqMatrix B)
        {
            SqMatrix r = new SqMatrix(A.n);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.n; j++)
                {
                    for (int k = 0; k < B.n; k++)
                    {
                        r[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return r;
        }

        //Умножение на число
        public static SqMatrix operator *(SqMatrix A, double Ch)
        {
            SqMatrix q = new SqMatrix(A.n);
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.n; j++)
                {
                    q[i, j] = A[i, j] * Ch;
                }
            }
            return q;
        }
        public static SqMatrix operator *(double Ch, SqMatrix A) { return A * Ch; }
        public static SqMatrix operator /(SqMatrix A, double Ch) { return A * (1 / Ch); }
        public static Vectors operator *(SqMatrix A, Vectors v)
        {
            Matrix V = new Matrix(v);
            Matrix res = A * V;
            Vectors w = new Vectors(v.n);
            for (int i = 0; i < v.n; i++)
                w[i] = res[i, 0];
            return w;
        }

        /// <summary>
        /// Перевод матрицы в одномерный массив
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static double[] ToDoubleMas(SqMatrix M)
        {
            double[] res = new double[M.m * M.m];
            int y = 0;
            for (int i = 0; i < M.n; i++)
                for (int j = 0; j < M.m; j++)
                    res[y++] = M[i, j];
            return res;
        }
    }

    /// <summary>
    /// Квадратные комплексные матрицы
    /// </summary>
    public class CSqMatrix
    {
        /// <summary>
        /// Действительные и мнимые части комплексной матрицы
        /// </summary>
        private SqMatrix Re
        {
            get
            {
                SqMatrix res = new SqMatrix(matr.GetLength(0));
                for (int i = 0; i < res.RowCount; i++)
                    for (int j = 0; j < res.RowCount; j++)
                        res[i, j] = matr[i, j].Re;
                return res;
            }
        }
        private SqMatrix Im
        {
            get
            {
                SqMatrix res = new SqMatrix(matr.GetLength(0));
                for (int i = 0; i < res.RowCount; i++)
                    for (int j = 0; j < res.RowCount; j++)
                        res[i, j] = matr[i, j].Im;
                return res;
            }
        }

        //public SqMatrix RE
        //{
        //    get
        //    {
        //        if(needretr)
        //        for (int i = 0; i < RowCount; i++)
        //            for (int j = 0; j < RowCount; j++)
        //                Re[i, j] = matr[i, j].Re;
        //        return Re;
        //    }
        //}
        //public SqMatrix IM
        //{
        //    get
        //    {
        //        if (needretr)
        //            for (int i = 0; i < RowCount; i++)
        //                for (int j = 0; j < RowCount; j++)
        //                    Im[i, j] = matr[i, j].Im;
        //        return Im;
        //    }
        //}

        private Complex[,] matrtmp = null;
        private bool needretr = false;
        private Complex[,] matr
        {
            get
            {
                if (matrtmp == null)
                {
                    var matrtmp = new Complex[Re.n, Re.m];
                    for (int i = 0; i < matrtmp.GetLength(0); i++)
                        for (int j = 0; j < matrtmp.GetLength(1); j++)
                            matrtmp[i, j] = new Complex(Re[i, j], Im[i, j]);
                }
                return matrtmp;
            }
            set
            {
                needretr = true;
                matrtmp = new Complex[value.GetLength(0), value.GetLength(1)];
                for (int i = 0; i < matrtmp.GetLength(0); i++)
                    for (int j = 0; j < matrtmp.GetLength(1); j++)
                        matrtmp[i, j] = new Complex(value[i, j]);
            }
        }

        /// <summary>
        /// Число строк
        /// </summary>
        public int RowCount => Re.RowCount;
        /// <summary>
        /// Число столбцов
        /// </summary>
        public int ColCount => Re.ColCount;

        /// <summary>
        /// Кубическая норма матрицы как сумма кубических норма её действительной и мнимой части
        /// </summary>
        public double CubeNorm => Re.CubeNorm + Im.CubeNorm;

        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public Complex this[int i, int j]
        { get { return Re[i, j] + Complex.I * Im[i, j]; } set { this.Re[i, j] = value.Re; this.Im[i, j] = value.Im; } }

        private static Complex[,] mas(SqMatrix A, SqMatrix B)
        {
            var res = new Complex[A.RowCount, A.ColCount];
            for (int i = 0; i < A.RowCount; i++)
                for (int j = 0; j < A.ColCount; j++)
                    res[i, j] = A[i, j] + Complex.I * B[i, j];
            return res;
        }
        /// <summary>
        /// Конструктор по двумерному комплексному массиву
        /// </summary>
        /// <param name="m"></param>
        public CSqMatrix(Complex[,] m)
        {
            //double[,] re = new double[m.GetLength(0), m.GetLength(1)], im = new double[m.GetLength(0), m.GetLength(1)];
            //for (int i = 0; i < m.GetLength(0); i++)
            //    for (int j = 0; j < m.GetLength(1); j++)
            //    {
            //        re[i, j] = m[i, j].Re;
            //        im[i, j] = m[i, j].Im;
            //    }
            //Re = new SqMatrix(re);
            //Im = new SqMatrix(im);
            matr = m;
        }
        /// <summary>
        /// Конструктор по двумерному действительному массиву
        /// </summary>
        /// <param name="m"></param>
        public CSqMatrix(double[,] m)
        {
            //double[,] re = new double[m.GetLength(0), m.GetLength(1)], im = new double[m.GetLength(0), m.GetLength(1)];
            Complex[,] tmp = new Complex[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < m.GetLength(0); i++)
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    //re[i, j] = m[i, j];
                    //im[i, j] = 0;
                    tmp[i, j] = new Complex(m[i, j], 0);
                }
            //Re = new SqMatrix(re);
            //Im = new SqMatrix(im);
            matr = tmp;
        }
        /// <summary>
        /// Конструктор по действительной и мнимой части матрицы
        /// </summary>
        /// <param name="R"></param>
        /// <param name="I"></param>
        public CSqMatrix(SqMatrix R, SqMatrix I) : this(mas(R, I)) { }
        /// <summary>
        /// Копирование комплексной матрицы
        /// </summary>
        /// <param name="M"></param>
        public CSqMatrix(CSqMatrix M) : this(M.Re, M.Im) { }

        //public CSqMatrix(CVectors v)
        //{
        //    this.Re =new SqMatrix( v.Re);
        //    this.Im = new SqMatrix(v.Im);
        //}

        /// <summary>
        /// Определитель матрицы
        /// </summary>
        public Complex Det
        {
            get
            {
                CSqMatrix matrix = new CSqMatrix(this);

                Complex m = 0;
                for (int j = 0; j < this.RowCount; j++)
                {
                    for (int i = j + 1; i < this.ColCount; i++)
                    {
                        if (matrix[j, j] != 0)
                        {
                            m = matrix[i, j] / matrix[j, j];

                            for (int h = j; h < this.ColCount; h++)
                                matrix[i, h] -= m * matrix[j, h];
                        }
                    }
                }
                m = 1;
                for (int i = 0; i < this.RowCount; i++) m *= matrix[i, i];
                // PrintMatrix();
                return m;
            }
        }
        /// <summary>
        /// Преобразование матрицы в комплексный массив
        /// </summary>
        /// <param name="M"></param>
        public static explicit operator Complex[,] (CSqMatrix M) => mas(M.Re, M.Im);
        /// <summary>
        /// Выдать матрицу в консоль
        /// </summary>
        public void PrintMatrix()
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                Console.Write("|| ");
                for (int j = 0; j < this.ColCount; j++)
                    Console.Write("\t" + this[i, j] + " ");
                Console.WriteLine("\t||");
            }
        }
        /// <summary>
        /// Замена колонны указанным вектором (для метода Крамера)
        /// </summary>
        /// <param name="ColumnNumber">Номер колонны</param>
        /// <param name="newColumn">Новая колонна</param>
        /// <returns></returns>
        public CSqMatrix ColumnSwap(int ColumnNumber, CVectors newColumn)
        {
            SqMatrix R = this.Re.ColumnSwap(ColumnNumber, newColumn.Re);
            SqMatrix I = this.Im.ColumnSwap(ColumnNumber, newColumn.Im);
            return new CSqMatrix(R, I);
        }
        /// <summary>
        /// Вернуть строку матрицы
        /// </summary>
        /// <param name="k">Номер строки, начиная от 0</param>
        /// <returns></returns>
        public CVectors GetLine(int k)
        {
            CVectors ew = new CVectors(this.ColCount);
            for (int i = 0; i < ew.Degree; i++)
                ew[i] = new Complex(this[k, i]);
            return ew;
        }

        /// <summary>
        /// Поделить строку в матрице на число
        /// </summary>
        /// <param name="i"></param>
        /// <param name="val"></param>
        public void DivByLine(int i, Complex val)
        {
            for (int j = 0; j < this.ColCount; j++)
                matr[i, j] /= val;
        }
        /// <summary>
        /// Отнять от строки матрицы вектор
        /// </summary>
        /// <param name="i"></param>
        /// <param name="c"></param>
        public void MinusVector(int i, CVectors c)
        {
            for (int j = 0; j < this.ColCount; j++)
                matr[i, j] -= c[j];
        }

        public void Show()
        {
            string s;
            for (int i = 0; i < RowCount; i++)
            {
                s = $"||{matr[i, 0]}";
                for (int j = 1; j < ColCount; j++)
                    s += $"\t {matr[i, j]}";
                s += "||";
                s.Show();
            }
        }

        /// <summary>
        /// Обратная матрица по Гауссу
        /// </summary>
        /// <returns></returns>
        public CSqMatrix Invert()
        {
            CSqMatrix mResult = SqMatrix.E(this.ColCount);
            /*
             * Получать "1" на элементе главной диагонали, а потом
             * Занулять оставшиеся элементы
             * */
            CSqMatrix mCur = new CSqMatrix(this);
            //mCur.PrintMatrix(); Console.WriteLine();
            //mResult.Show(); Console.WriteLine();

            for (int i = 0; i < this.ColCount; i++) //Цикл по строкам сверху-вниз
            {
                //Заединичить вервую строку
                Complex dItem = new Complex(mCur[i, i]);
                mCur.DivByLine(i, dItem);
                mResult.DivByLine(i, dItem);

                //mCur.PrintMatrix(); Console.WriteLine();
                //mResult.Show(); Console.WriteLine();

                CVectors rTmp = mCur.GetLine(i);
                CVectors eTmp = mResult.GetLine(i);
                //Забить нулями вертикаль
                for (int j = 0; j < this.ColCount; j++)
                    if (i != j)
                    {
                        Complex con = new Complex(mCur[j, i]);
                        mCur.MinusVector(j, rTmp * con);
                        mResult.MinusVector(j, eTmp * con);
                    }
                //mCur.PrintMatrix(); Console.WriteLine();
                //mResult.PrintMatrix(); Console.WriteLine();
            }

            return mResult;
        }
        /// <summary>
        /// Уточнение обратной матрицы
        /// </summary>
        /// <param name="A">Исходная матрица</param>
        /// <param name="Reverse">Обратная марица</param>
        /// <param name="eps">Точность</param>
        /// <returns></returns>
        public static CSqMatrix ReverseCorrect(CSqMatrix A, CSqMatrix Reverse, double eps = 0.001, int stepcount = 1000, bool existnorm = false)
        {
            CSqMatrix E = new CSqMatrix(SqMatrix.E(A.RowCount)), R = new CSqMatrix(Reverse), Rold;
            int i = 0;
            double epsold = (E - A * R).CubeNorm, epsnew = epsold;
            //if (epsold < 1 || existnorm)
            while (epsnew > eps && i < stepcount)
            {
                Rold = new CSqMatrix(R);
                R *= (2 * E - A * R);
                //(E - A * R).CubeNorm.Show();
                epsold = epsnew;
                epsnew = (E - A * R).CubeNorm;
                if (epsnew > epsold) return Rold;
                i++;
            }
            return R;
        }
        /// <summary>
        /// Track матрицы
        /// </summary>
        public Complex Track
        {
            get
            {
                Complex sum = 0;
                for (int i = 0; i < this.RowCount; i++)
                    sum += this[i, i];
                return sum;
            }
        }


        public static CSqMatrix operator +(CSqMatrix A, CSqMatrix B) => new CSqMatrix(A.Re + B.Re, A.Im + B.Im);
        public static CSqMatrix operator -(CSqMatrix A) => new CSqMatrix(new SqMatrix((-A.Re).matrix), new SqMatrix((-A.Im).matrix));
        public static CSqMatrix operator -(CSqMatrix A, CSqMatrix B) => A + (-B);
        public static CSqMatrix operator *(CSqMatrix A, CSqMatrix B) => new CSqMatrix(A.Re * B.Re - A.Im * B.Im, A.Re * B.Im + B.Re * A.Im);
        public static CVectors operator *(CSqMatrix A, CVectors x)
        {
            if (A.ColCount != x.Degree) throw new Exception("Размерность матрицы и вектора не совпадают");
            CVectors res = new CVectors(x.Degree);
            for (int i = 0; i < res.Degree; i++)
                res[i] = A.GetLine(i) * x;
            return res;
        }
        public static CSqMatrix operator *(Complex c, CSqMatrix A)
        {
            CSqMatrix R = new CSqMatrix(A);
            for (int i = 0; i < R.ColCount; i++)
                for (int j = 0; j < R.RowCount; j++)
                    R[i, j] *= c;
            return R;
        }
        public static CSqMatrix operator *(CSqMatrix A, Complex c) => c * A;
        public static implicit operator CSqMatrix(SqMatrix sq)
        {
            CSqMatrix res = new CSqMatrix(sq.matrix);
            return res;
        }
    }

#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
    /// <summary>
    /// Обычные векторы
    /// </summary>
    public class Vectors
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
    {
        /// <summary>
        /// Размерность вектора
        /// </summary>
        public int n;
        /// <summary>
        /// Массив, с которым отождествляется вектор
        /// </summary>
        protected internal double[] vector;
        /// <summary>
        /// Размерность вектора
        /// </summary>
        public int Deg => n;
        /// <summary>
        /// Массив, соответствующий вектору
        /// </summary>
        public double[] DoubleMas => vector;

        //Свойства-методы
        /// <summary>
        /// Использование вектора как массива
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double this[int i]
        {
            get { return vector[i]; }
            set { vector[i] = value; }
        }
        /// <summary>
        /// Среднее арифметическое значений в векторе
        /// </summary>
        public double ArithmeticAv
        {
            get
            {
                double sun = 0;
                for (int i = 0; i < n; i++) sun += this[i];
                return sun / this.n;
            }
        }
        /// <summary>
        /// Отклонение элемента от среднего значения
        /// </summary>
        /// <param name="i">Номер элемента</param>
        /// <returns></returns>
        public double Av(int i) { return Math.Abs(this[i] - this.ArithmeticAv); }
        /// <summary>
        /// Среднее отклонение значений в векторе
        /// </summary>
        public double Average
        {
            get
            {
                double sun = 0;
                for (int i = 0; i < n; i++) sun += this.Av(i);
                return sun / this.n;
            }
        }
        /// <summary>
        /// Относительная погрешность значений в векторе
        /// </summary>
        public double RelAc
        {
            get { if (this.ArithmeticAv != 0) return this.Average / this.ArithmeticAv; else throw new DivideByZeroException("Деление на 0!"); }
        }
        /// <summary>
        /// Вектор отклонений от среднего значения в векторе
        /// </summary>
        public Vectors RelAcVec
        {
            get
            {
                Vectors v = new Vectors(this.n);
                for (int i = 0; i < v.n; i++) v[i] = this.Av(i);
                return v;
            }
        }
        /// <summary>
        /// Вектор квадратов отклонений от среднего значения в векторе
        /// </summary>
        public Vectors RelAcSqr
        {
            get
            {
                Vectors v = new Vectors(this.n);
                for (int i = 0; i < v.n; i++) v[i] = this.Av(i) * this.Av(i);
                return v;
            }
        }
        /// <summary>
        /// Вывести истинное значение величины на консоль
        /// </summary>
        public void TrueValShow() { Console.WriteLine(this.ArithmeticAv + " +/- " + this.Average); }
        /// <summary>
        /// Показать всю информацию о векторе как о реализации величины
        /// </summary>
        public void TrueValShowFull()
        {
            Console.WriteLine("Исходный вектор:");
            this.Show();
            Console.WriteLine("Среднее арифметическое значений в векторе равно " + this.ArithmeticAv);
            Console.WriteLine("Среднее отклонение значений в векторе равно " + this.Average);
            Console.WriteLine("Относительная погрешность значений в векторе равна " + this.RelAc);

            Console.WriteLine("Вектор отклонений от среднего значения:"); this.RelAcVec.Show();
            Console.WriteLine("Вектор квадратов отклонений от среднего значения:"); this.RelAcSqr.Show();
            Console.WriteLine("Истинное значение величины:"); this.TrueValShow();

        }
        /// <summary>
        /// Максимальное значение
        /// </summary>
        public double Max
        {
            get
            {
                double t = vector[0];
                for (int i = 1; i < n; i++)
                    if (vector[i] > t) t = vector[i];
                return t;
            }
        }
        /// <summary>
        /// Минимальное значение
        /// </summary>
        public double Min
        {
            get
            {
                double t = vector[0];
                for (int i = 1; i < n; i++)
                    if (vector[i] < t) t = vector[i];
                return t;
            }
        }
        /// <summary>
        /// Отсортированный вектор
        /// </summary>
        public Vectors Sort
        {
            get
            {
                Vectors e = new Vectors(this);
                Array.Sort(e.vector);
                return e;
            }
        }


        //Конструктор
        /// <summary>
        /// Вектор (0)
        /// </summary>
        public Vectors()//по умолчанию
        {
            this.n = 0;
            this.vector = new double[n];
        }
        /// <summary>
        /// Нулевой вектор
        /// </summary>
        /// <param name="n">Размерность вектора</param>
        public Vectors(int n)//по размерности
        {
            this.n = n;
            this.vector = new double[n];
        }
        /// <summary>
        /// Вектор, заполненный одинаковыми числами
        /// </summary>
        /// <param name="n">Размерность вектора</param>
        /// <param name="c">Конпонент вектора</param>
        public Vectors(int n, double c) : this(n)
        {
            for (int i = 0; i < n; i++) this[i] = c;
        }
        /// <summary>
        /// Считать вектор из файла
        /// </summary>
        /// <param name="fs"></param>
        public Vectors(StreamReader fs)//через файл
        {
            string s = fs.ReadLine();
            string[] st = s.Split(' ');
            this.n = st.Length;
            this.vector = new double[n];
            for (int i = 0; i < this.n; i++) this.vector[i] = Convert.ToDouble(st[i]);
            fs.Close();
        }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="V"></param>
        public Vectors(Vectors V)//копирования
        {
            this.n = V.n;
            this.vector = new double[V.n];
            for (int i = 0; i < V.n; i++) this[i] = V[i];
        }
        /// <summary>
        /// Задать вектор перечислением координат или массивом
        /// </summary>
        /// <param name="x"></param>
        public Vectors(params double[] x)
        {
            this.n = x.Length;
            vector = new double[x.Length];
            for (int i = 0; i < x.Length; i++) this.vector[i] = x[i];
        }
        /// <summary>
        /// Задать вектор массивом целых чисел
        /// </summary>
        /// <param name="c"></param>
        public Vectors(int[] c)
        {
            this.n = c.Length;
            vector = new double[c.Length];
            for (int i = 0; i < c.Length; i++) this.vector[i] = c[i];
        }
        /// <summary>
        /// Задать вектор по вектору-столбцу
        /// </summary>
        /// <param name="M"></param>
        public Vectors(Matrix M)
        {
            this.n = M.n;
            this.vector = new double[this.n];
            for (int i = 0; i < M.n; i++) this.vector[i] = M[i, 0];
        }
        /// <summary>
        /// Вектор как кусок кругого вектора
        /// </summary>
        /// <param name="v">Образец</param>
        /// <param name="a">Коэффициент начала из образца</param>
        /// <param name="b">Коэффициент конца из образца</param>
        public Vectors(Vectors v, int a, int b)
        {
            this.n = b - a + 1;
            this.vector = new double[n];
            for (int i = 0; i < n; i++) this[i] = v[a + i];
        }

        public static implicit operator Vectors(double[] x) => new Vectors(x);
        public static explicit operator double[] (Vectors v) => ToDoubleMas(v);
        public static explicit operator Complex[] (Vectors v)
        {
            Complex[] res = new Complex[v.Deg];
            for (int i = 0; i < res.Length; i++)
                res[i] = new Complex(v[i]);
            return res;
        }

        //методы
        /// <summary>
        /// Перевести вектор в массив чисел
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double[] ToDoubleMas(Vectors x)
        {
            double[] r = new double[x.n];
            for (int i = 0; i < x.n; i++) r[i] = x[i];
            return r;
        }
        /// <summary>
        /// Содержится ли указанное число в векторе
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool Contain(double x)
        {
            for (int i = 0; i < this.n; i++)
                if (this[i] == x) return true;
            return false;
        }
        /// <summary>
        /// Слияние нескольких векторов со состыковкой крайних вершин (конец предыдущего вектора должен совпадать с началом последующего)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vectors Merge(params Vectors[] v)
        {
            int l = 0;
            for (int i = 0; i < v.Length; i++) l += v[i].n;
            l -= v.Length - 1;
            Vectors r = new Vectors(l);
            r[0] = v[0].vector[0];
            int k = 0;
            for (int i = 0; i < v.Length; i++)
            {
                for (int j = 1; j < v[i].n; j++)
                    r[k + j] = v[i].vector[j];
                k += v[i].n - 1;
            }

            return r;
        }
        /// <summary>
        /// Описывает ли вектор простой цикл (первая и последняя вершины должны повторяться, но больше в нём не должно содержаться повторяющихся вершин)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsSimpleCycle(Vectors v)
        {
            if (v[0] != v[v.n - 1]) return false;
            double[] x = new double[v.n - 1];
            for (int i = 0; i < x.Length; i++) x[i] = v[i + 1];
            Array.Sort(x);
            for (int i = 0; i < x.Length - 1; i++)
                if (x[i] == x[i + 1]) return false;

            return true;
        }

        /// <summary>
        /// Задать коэффициенты через консоль
        /// </summary>
        public virtual void CreateMatrix()
        {

            for (int i = 0; i < n; i++)
            {
                Console.Write("Введите элемент [" + i.ToString() + "]" + "\t");
                vector[i] = Convert.ToDouble(Console.ReadLine());
            }

        }
        /// <summary>
        /// Вывести вектор на консоль
        /// </summary>
        public virtual void PrintMatrix()
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write(vector[i].ToString() + " \t");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Нулевой ли вектор?
        /// </summary>
        /// <returns></returns>
        public virtual bool Nulle()
        {
            for (int i = 0; i < n; i++)
            {
                if (vector[i] != 0) return false;
            }

            return true;
        }
        /// <summary>
        /// Перевод вектора в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "( ";
            for (int i = 0; i < this.n; i++) s += String.Format("\t{0} ", this[i]);
            s += "\t)";
            return s;
        }
        /// <summary>
        /// Перевод вектора (все координаты которого увеличены на 1) в строку
        /// </summary>
        /// <returns></returns>
        public string ToStringPlusOne()
        {
            string s = "( ";
            for (int i = 0; i < this.n; i++) s += String.Format("\t{0} ", this[i] + 1);
            s += "\t)";
            return s;
        }
        /// <summary>
        /// Перевод вектора в строку с рациональными числами
        /// </summary>
        /// <returns></returns>
        public string ToRationalString()
        {
            string s = "( ";
            for (int i = 0; i < this.n; i++) s += String.Format("{0} ", Number.Rational.ToRational(this[i]));
            s += ")";
            return s;
        }
        /// <summary>
        /// Перевод вектора в строку с рациональными числами с табуляцией
        /// </summary>
        /// <returns></returns>
        public string ToRationalStringTab()
        {
            string s = "( ";
            for (int i = 0; i < this.n; i++) s += String.Format("\t{0} ", Number.Rational.ToRational(this[i]));
            s += "\t)";
            return s;
        }
        /// <summary>
        /// Вывести вектор на консоль
        /// </summary>
        public void Show() { Console.WriteLine(this.ToString()); }
        /// <summary>
        /// Вывести вектор (все координаты которого увеличены на 1) на консоль
        /// </summary>
        public void ShowPlusOne() { Console.WriteLine(this.ToStringPlusOne()); }

        /// <summary>
        /// Вывести вектор в рациональном виде на консоль
        /// </summary>
        public void ShowRational() { Console.WriteLine(this.ToRationalString()); }
        private void ShowRational(StreamWriter sf) { sf.WriteLine(this.ToRationalString()); }
        /// <summary>
        /// Вывести вектор в рациональном виде на консоль с табуляцией
        /// </summary>
        public void ShowRationalTab() { Console.WriteLine(this.ToRationalStringTab()); }
        private void ShowRationalTab(StreamWriter sf) { sf.WriteLine(this.ToRationalStringTab()); }
        /// <summary>
        /// Вывести массив векторов на консоль
        /// </summary>
        /// <param name="lines"></param>
        public static void Show(Vectors[] lines)
        {
            Console.Write(" \t\tb "); for (int i = 1; i < lines[0].n; i++) Console.Write("\tx[{0}] ", i);
            Console.WriteLine();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                Console.Write("x[{0}]\t", Vectors.NumberColumn(lines, i));
                lines[i].Show();
            }
            Console.Write("L\t"); lines[lines.Length - 1].Show();
        }
        /// <summary>
        /// Вывести массив векторов в файл
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sf"></param>
        private static void Show(Vectors[] lines, StreamWriter sf)
        {
            sf.Write(" \t\tb "); for (int i = 1; i < lines[0].n; i++) sf.Write("\tx[{0}] ", i);
            sf.WriteLine();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                sf.Write("x[{0}]\t", Vectors.NumberColumn(lines, i));
                lines[i].Show();
            }
            sf.Write("L\t"); lines[lines.Length - 1].Show();
        }
        /// <summary>
        /// Вывести информацию об векторе в файл
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fs"></param>
        public static void ShowInfo(Vectors v, StreamWriter fs)
        {
            fs.Write("Исходный вектор: "); fs.WriteLine(v.ToString());
            fs.Write("Вектор отклонений от среднего значения: "); fs.WriteLine(v.RelAcVec.ToString());
            fs.Write("Вектор квадратов отклонений: "); fs.WriteLine(v.RelAcSqr.ToString());
            fs.Write("Истинное значение элементов в векторе: "); fs.WriteLine(v.ArithmeticAv + " +/- " + v.Average);
            fs.Close();
        }
        private static int NumberColumn(Vectors[] l, int k)
        {
            for (int j = 1; j < l[0].n; j++)
                if (l[k].vector[j] == 1)
                {
                    int t = 0;
                    for (int i = 0; i < l.Length; i++)
                        if (l[i].vector[j] == 0) t++;
                    if (t == l.Length - 1) return j;

                }
            return 0;
        }

        /// <summary>
        /// Вывести массив векторов c рациональными коэффициентами на консоль
        /// </summary>
        /// <param name="lines"></param>
        public static void ShowRational(Vectors[] lines)
        {
            //for (int i = 0; i < lines.Length; i++) lines[i].ShowRational();
            Console.Write(" \t\tb "); for (int i = 1; i < lines[0].n; i++) Console.Write("\tx[{0}] ", i);
            Console.WriteLine();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                Console.Write("x[{0}]\t", Vectors.NumberColumn(lines, i));
                lines[i].ShowRationalTab();
            }
            Console.Write("L\t"); lines[lines.Length - 1].ShowRationalTab();
        }

        /// <summary>
        /// Нахождение максимума функционала методами симплекс-таблицы, считанной из файла
        /// </summary>
        /// <param name="result">Вектор вида (0,0,1,0,1), показывающий, какие переменные являются базисными</param>
        /// <param name="fs">Файл с матрицей (симплекс-таблицей) размера (x.Lenth+1)X(xbase.Lenth+1) (образец)</param>
        /// <returns>Наибольшее значение функционала, который задан последней строкой таблицы</returns>
        /// <remarks>Не предусматривается возможность вырожденного решения</remarks>
        public static double SimpleSimplex(ref Vectors result, StreamReader fs)
        {
            //чтение данных
            int k = 0;//число единиц
            for (int i = 0; i < result.n; i++) if (result[i] != 0) k++;
            Vectors[] lines = new Vectors[k + 1];
            for (int i = 0; i < k + 1; i++)
            {
                lines[i] = new Vectors(result.n + 1);
                string s = fs.ReadLine();
                string[] st = s.Split(' ');
                for (int j = 0; j < lines[i].n; j++) lines[i].vector[j] = Convert.ToDouble(st[j]);
            }
            fs.Close();
            Console.WriteLine("Исходная сиплекс-таблица:");
            Vectors.Show(lines);
            Console.WriteLine("Вектор, указывающий на базис:");
            result.ShowRational(); Console.WriteLine(); Console.WriteLine();

            //Решение
            int it_num = 1;
            while (Vectors.ExistColumn(lines) != 0)
            {
                if (Vectors.ExistInfinity(lines) != 0)
                {
                    Console.WriteLine("Функция неограничена на области допустимых решений (столбец {0} таблицы полностью отрицателен)", Vectors.ExistInfinity(lines) + 1);
                    return Double.PositiveInfinity;
                }
                int m = Vectors.ExistColumn(lines);
                Console.WriteLine("На итерации {0} разрещающим выбран элемент на пересечении строки {1} и столбца {2}", it_num, Vectors.FindPosElem(lines, m) + 1, m + 1);
                Vectors.Transform(ref lines, Vectors.FindPosElem(lines, m), m, ref result);
                Console.WriteLine("Сиплекс-таблица после итерации {0}:", it_num);
                //Vectors.Show(lines);
                Vectors.ShowRational(lines);
                Console.WriteLine();
                it_num++;
                //result.Show(); Console.WriteLine(); Console.WriteLine();
            }
            //отредактировать вектор решения
            //int t = 0;

            //for (int j = 1; j < lines[0].n; j++)
            //{
            //    int zero = 0, one = 0;
            //    for (int i = 0; i < lines.Length; i++)
            //        if (lines[i].vector[j] == 0) zero++;
            //        else if (lines[i].vector[j] == 1) one++;
            //    if (zero == lines.Length - 1 && one == 1) result[j - 1] = 1;
            //    else result[j - 1] = 0;
            //}

            //for (int i = 0; i < result.n; i++) { if (result[i] != 0) { result[i] = lines[t].vector[0]; t++; } }

            result = new Vectors(Vectors.GetSolutionVec(lines));
            //result.Show();
            //Console.WriteLine("Вектор решения {0}", result.ToString());
            Console.WriteLine("Вектор решения {0}", result.ToRationalString());
            Console.WriteLine("Оптимальное значение функции равно {0}", lines[k].vector[0]);

            return lines[k].vector[0];
        }
        /// <summary>
        /// Нахождение максимума функционала методами симплекс таблицы, заданной массивом векторов
        /// </summary>
        /// <param name="result">Вектор вида (0,0,1,0,1), показывающий, какие переменные являются базисными</param>
        /// <param name="lines">Симплекс-таблица, заданная массивом векторов</param>
        /// <returns></returns>
        public static double SimpleSimplex(ref Vectors result, ref Vectors[] lines)
        {
            Console.WriteLine("Исходная сиплекс-таблица:");
            Vectors.ShowRational(lines);
            Console.WriteLine("Вектор, указывающий на базис:");
            result.ShowRational(); Console.WriteLine(); Console.WriteLine();

            //Решение
            int it_num = 1;
            while (Vectors.ExistColumn(lines) != 0)
            {
                if (Vectors.ExistInfinity(lines) != 0)
                {
                    Console.WriteLine("Функция неограничена на области допустимых решений (столбец {0} таблицы полностью отрицателен)", Vectors.ExistInfinity(lines) + 1);
                    return Double.PositiveInfinity;
                }
                int m = Vectors.ExistColumn(lines);
                int n = Vectors.FindPosElem(lines, m);
                //Console.WriteLine("+");
                Console.WriteLine("На итерации {0} разрещающим выбран элемент на пересечении строки {1} и столбца {2}", it_num, n + 1, m + 1);
                //Console.WriteLine("++");
                Vectors.Transform(ref lines, Vectors.FindPosElem(lines, m), m, ref result);
                Console.WriteLine("Сиплекс-таблица после итерации {0}:", it_num);
                //Vectors.Show(lines);
                Vectors.ShowRational(lines);
                Console.WriteLine();
                it_num++;
                //result.Show(); Console.WriteLine(); Console.WriteLine();
            }

            //отредактировать вектор решения
            //int t = 0;

            //for (int j = 1; j < lines[0].n; j++)
            //{
            //    int zero = 0, one = 0;
            //    for (int i = 0; i < lines.Length; i++)
            //        if (lines[i].vector[j] == 0) zero++;
            //        else if (lines[i].vector[j] == 1) one++;
            //    if (zero == lines.Length - 1 && one == 1) result[j - 1] = 1;
            //    else result[j - 1] = 0;
            //}

            //for (int i = 0; i < result.n; i++) { if (result[i] != 0) { result[i] = lines[t].vector[0]; t++; } }

            result = new Vectors(Vectors.GetSolutionVec(lines));

            //result.Show();
            //Console.WriteLine("Вектор решения {0}", result.ToString());
            Console.WriteLine("Вектор решения {0}:", result.ToRationalString());
            Console.WriteLine("Оптимальное значение функции равно: {0}", lines[lines.Length - 1].vector[0]);

            return lines[lines.Length - 1].vector[0];
        }
        /// <summary>
        /// Проверяет симплекс-таблицу на наличие столбца с положительным элементом, где коэффициент при линейной форме отрицателен
        /// </summary>
        /// <param name="l"></param>
        /// <returns>Номер найденного столбца или 0, если таковой отсутствует</returns>
        /// <remarks>Выбирается стобец с наибольшей по модулю отрицательной частью в конце</remarks>
        private static int ExistColumn(Vectors[] l)
        {
            int k = 0;
            double max = 0;
            for (int j = 1; j < l[0].n; j++)
                if (l[l.Length - 1].vector[j] < 0)
                    if (Math.Abs(l[l.Length - 1].vector[j]) > Math.Abs(max))
                    {
                        max = l[l.Length - 1].vector[j];
                        k = j;
                    }
            //if (k >= 0)
            for (int i = 0; i < l.Length - 1; i++)
                if (l[i].vector[k] > 0)
                {
                    return k;
                }

            return k;
            //return 0;
        }
        /// <summary>
        /// Найти позицию элемента, который годится под центр в симплекс-таблице
        /// </summary>
        /// <param name="l">Симплекс-таблица</param>
        /// <param name="k">Номер столбца, где этот элемент ищется</param>
        /// <returns>Номер строки, где этот элемент находится</returns>
        private static int FindPosElem(Vectors[] l, int k)
        {
            int fix = 0;//Номер столбца, с которым сравниваем
            int t = 0;
            while (l[t].vector[k] <= 0) t++;
            double min = l[t].vector[fix] / l[t].vector[k];//может возникнуть ошибка деления на 0
            int u = t;
            for (int i = t + 1; i < l.Length - 1; i++)
            {
                if (l[i].vector[k] > 0)
                {
                    double tmp = l[i].vector[fix] / l[i].vector[k];
                    if (tmp < min)
                    {
                        min = tmp;
                        u = i;
                    }
                }
            }
            //Console.WriteLine("{0} ---- {1}", u, k);
            return u;
            //return 0;
        }
        /// <summary>
        /// Преобразует симплекс-таблицу по годному элементу в центре
        /// </summary>
        /// <param name="l">Симплекс-таблица</param>
        /// <param name="k">Номер строки, где находится элемент</param>
        /// <param name="m">Номер столбца, где находится элемент</param>
        private static void Transform(ref Vectors[] l, int k, int m, ref Vectors r)
        {
            //Vectors.Show(l);r.Show();Console.WriteLine(l[k].vector[m]);
            l[k] /= l[k].vector[m];//сделать в главной строке коэффициент 1
            for (int i = 0; i < k; i++) l[i] -= l[k] * l[i].vector[m];//отнимать эту строку от остальных
            for (int i = k + 1; i < l.Length; i++) l[i] -= l[k] * l[i].vector[m];
        }
        /// <summary>
        /// Проверяет симплекс-таблицу на наличие столбца, характеризующего отсутствие решения
        /// </summary>
        /// <param name="l"></param>
        /// <returns>Номер такого столбца и 0 при его отсутствии</returns>
        private static int ExistInfinity(Vectors[] l)
        {
            int k = 0;
            for (int j = 1; j < l[0].n; j++)
                if (l[l.Length - 1].vector[j] < 0)
                {
                    for (int i = 0; i < l.Length - 1; i++)
                        if (l[i].vector[j] < 0) k++;
                    if (k == l.Length - 1) return j;
                    k = 0;
                }

            return k;
        }
        /// <summary>
        /// (Старый) Поиск целочисленного решения задачи линейного программирования, заданной симплекс-таблицей
        /// </summary>
        /// <param name="result">Вектор вида (0,0,1,0,1), показывающий, какие переменные являются базисными</param>
        /// <param name="fs">Файл с матрицей (симплекс-таблицей) размера (x.Lenth+1)X(xbase.Lenth+1)</param>
        /// <returns></returns>
        public static double SimplexInteger(ref Vectors result, StreamReader fs)
        {
            //Считать таблицу
            //чтение данных
            int k = 0;//число единиц
            for (int i = 0; i < result.n; i++) if (result[i] != 0) k++;
            Vectors[] lines = new Vectors[k + 1];
            for (int i = 0; i < k + 1; i++)
            {
                lines[i] = new Vectors(result.n + 1);
                string s = fs.ReadLine();
                string[] st = s.Split(' ');
                for (int j = 0; j < lines[i].n; j++) lines[i].vector[j] = Convert.ToDouble(st[j]);
            }
            fs.Close();
            //Найти решение обычным симплекс-методом
            double end = Vectors.SimpleSimplex(ref result, ref lines);
            //Vectors.Show(lines); //не решает, потому что double =3, (int)3=2...

            if (Vectors.IsIntDesigion(lines)) return end;
            int y = 0;
            //Если решение не целочисленное, то пока оно не является целочисленным и пока его разумно искать...
            while ((!Vectors.IsIntDesigion(lines)) && (!Vectors.ImpossibleFoundIntDesigion(lines)) && (y <= 10))
            {
                Console.WriteLine();
                Console.WriteLine("-------------------------Итерация метода Гомори номер {0}", y + 1);
                //Создаётся новая симплекс-таблица
                Vectors[] newlines = new Vectors[lines.Length + 1];
                for (int i = 0; i < lines.Length - 1; i++)//Переписать строки до строки с функционалом
                {
                    newlines[i] = new Vectors(lines[0].n + 1);
                    for (int j = 0; j < lines[0].n; j++) newlines[i].vector[j] = lines[i].vector[j];
                    newlines[i].vector[lines[0].n] = 0;
                }
                int tmp = Vectors.NumberOfNotIntDes(lines);//Номер строки, где решение не целочисленное
                Console.WriteLine("----------За образец новой строки взята строка {0}", tmp + 1);
                newlines[lines.Length - 1] = new Vectors(lines[0].n + 1);
                newlines[lines.Length] = new Vectors(lines[0].n + 1);

                //Записать новую строку
                for (int j = 0; j < lines[0].n; j++) newlines[lines.Length - 1].vector[j] = -(/*lines[tmp].vector[j] - (int)lines[tmp].vector[j]*/Number.Rational.ToRational(lines[tmp].vector[j]).FracPart.ToDouble());
                newlines[lines.Length - 1].vector[lines[0].n] = 1;
                //Переписать строку функции с изменением знака
                for (int j = 0; j < lines[0].n; j++) newlines[lines.Length].vector[j] = -lines[lines.Length - 1].vector[j];
                newlines[lines.Length].vector[lines[0].n] = 0;
                //Записать новый вектор result
                Vectors newresult = new Vectors(result.n + 1);
                for (int i = 0; i < result.n; i++)
                {
                    if (result[i] == 0) newresult[i] = result[i];
                    else newresult[i] = 1;
                }
                newresult[result.n] = /*newlines[lines.Length - 1].vector[0]*/1;

                //Console.WriteLine("--------Новая таблица создана");

                //Переписать новые данные в старые
                result = new Vectors(newresult);
                lines = new Vectors[newlines.Length];
                for (int i = 0; i < newlines.Length; i++) lines[i] = new Vectors(newlines[i]);
                //Console.WriteLine("--------Новая таблица перезаписана");
                //Console.WriteLine("-----------Новое использование сиплекс-метода-----------");

                //Решить двойственным симплекс-методом
                end = Vectors.SimpleSimplex(ref result, ref lines);
                y++;
            }

            Console.WriteLine("Целочисленное решение найдено либо целочисленного решения не существует");

            //Вернуть значение
            Console.WriteLine("Конечная симплекс-таблица: "); Vectors.ShowRational(lines);
            Console.Write("Вектор решения:"); result.ShowRational();
            return -end;
        }
        /// <summary>
        /// Проверка на то, содержится ли в симплекс-таблице целочисленное решение
        /// </summary>
        /// <param name="l"></param>
        /// <returns>True, если содержится</returns>
        private static bool IsIntDesigion(Vectors[] l)
        {/*Math.Truncate(l[i].vector[0])!= l[i].vector[0]*/
            for (int i = 0; i < l.Length - 1; i++)
                if (/*l[i].vector[0] != (int)l[i].vector[0]*/ Number.Rational.ToRational(l[i].vector[0]).IsFract())
                {
                    //Console.WriteLine("{0} != {1}", l[i].vector[0], Number.Rational.ToRational(l[i].vector[0]).IntPart);
                    return false;
                }
            return true;
        }
        /// <summary>
        /// Возвращает номер оптимальной строки, где решение не является целочисленными
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private static int NumberOfNotIntDes(Vectors[] l)
        {
            double t, max = 0;
            int k = 0;
            bool an = false;
            for (int i = 0; i < l.Length - 1; i++)
                if (Number.Rational.ToRational(l[i].vector[0]).IsFract())
                {
                    an = true;
                    t = Number.Rational.ToRational(l[i].vector[0]).FracPart.ToDouble();
                    if (t > max) { max = t; k = i; }
                }
            if (an) return k;
            throw new Exception("Решение является целочисленным!");
        }
        /// <summary>
        /// Проверка невозможности нахождения целочисленного решения
        /// </summary>
        /// <param name="l"></param>
        /// <returns>True, если целочисленное решение найти невозможно</returns>
        /// <remarks>Целочисленного решения не существует, если в какой-то столбце имеется дробный свободный коэффициент, а все остальные коэффициенты - целые</remarks>
        private static bool ImpossibleFoundIntDesigion(Vectors[] l)
        {
            for (int i = 0; i < l.Length - 1; i++)
                if (/*!*/Number.Rational.ToRational(l[i].vector[0]).IsFract())
                {
                    int k = 1;
                    for (int j = 1; j < l[0].n; j++)
                        if (l[i].vector[j] == (int)l[i].vector[j]) k++;
                    if (k == l[0].n) return true;
                }
            return false;
        }
        /// <summary>
        /// Получить вектор решения по симплекс-таблице
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static Vectors GetSolutionVec(Vectors[] lines)
        {
            //int t = 0;
            Vectors r = new Vectors(lines[0].n - 1);
            int ii = 0;

            for (int j = 1; j < lines[0].n; j++)
            {
                int zero = 0, one = 0;
                for (int i = 0; i < lines.Length; i++)
                    if (lines[i].vector[j] == 0) zero++;
                    else if (lines[i].vector[j] == 1) { one++; ii = i; }
                if (zero == lines.Length - 1 && one == 1) { r[j - 1] = lines[ii].vector[0]; ii = 0; }
                else r[j - 1] = 0;
            }

            //for (int i = 0; i < r.n; i++)
            //{
            //    if (r[i] != 0)
            //    {
            //        r[i] = lines[t].vector[0];
            //        t++;
            //    }
            //}
            return r;
        }


        /// <summary>
        /// Вывести вектор в файл
        /// </summary>
        private void Show(StreamWriter sf) { sf.WriteLine(this.ToString()); }
        /// <summary>
        /// Вывести массив векторов c рациональными коэффициентами в файл
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sf"></param>
        private static void ShowRational(Vectors[] lines, StreamWriter sf)
        {
            //for (int i = 0; i < lines.Length; i++) lines[i].ShowRational();
            sf.Write(" \t\tb "); for (int i = 1; i < lines[0].n; i++) sf.Write("\tx[{0}] ", i);
            sf.WriteLine();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                sf.Write("x[{0}]\t", Vectors.NumberColumn(lines, i));
                lines[i].ShowRationalTab(sf);
            }
            sf.Write("L\t"); lines[lines.Length - 1].ShowRationalTab(sf);
        }
        /// <summary>
        /// Нахождение максимума функционала методами симплекс-таблицы, считанной из файла, с выводом в файл
        /// </summary>
        /// <param name="result">Вектор вида (0,0,1,0,1), показывающий, какие переменные являются базисными</param>
        /// <param name="fs">Файл с матрицей (симплекс-таблицей) размера (x.Lenth+1)X(xbase.Lenth+1) (образец)</param>
        /// <param name="sf"></param>
        /// <returns>Наибольшее значение функционала, который задан последней строкой таблицы</returns>
        /// <remarks>Не предусматривается возможность вырожденного решения</remarks>
        private static double SimpleSimplex(ref Vectors result, StreamReader fs, StreamWriter sf)
        {
            //чтение данных
            int k = 0;//число единиц
            for (int i = 0; i < result.n; i++) if (result[i] != 0) k++;
            Vectors[] lines = new Vectors[k + 1];
            for (int i = 0; i < k + 1; i++)
            {
                lines[i] = new Vectors(result.n + 1);
                string s = fs.ReadLine();
                string[] st = s.Split(' ');
                for (int j = 0; j < lines[i].n; j++) lines[i].vector[j] = Convert.ToDouble(st[j]);
            }
            fs.Close();
            sf.WriteLine("Исходная сиплекс-таблица:");
            Vectors.Show(lines, sf);
            sf.WriteLine("Вектор, указывающий на базис:");
            result.ShowRational(sf); sf.WriteLine(); sf.WriteLine();

            //Проверка на отрицательные свободные члены и их исправление
            while (Vectors.ExistColumnDual(lines) != -1)
            {
                int m = Vectors.ExistColumnDual(lines);
                int n = Vectors.FindPosElemDual(lines, m);
                sf.WriteLine("Для избавления от отрицательных свободных коэффициентов в базисных переменных разрещающим выбран элемент на пересечении строки {0} и столбца {1}", m + 1, n + 1);
                Vectors.Transform(ref lines, m, n, ref result);
                sf.WriteLine("Сиплекс-таблица после избавления от отрицательных свободных коэффициентов в базисных переменных");
                //Vectors.Show(lines);
                Vectors.ShowRational(lines, sf);
                //sf.WriteLine();
            }

            //Решение
            int it_num = 1;
            while (Vectors.ExistColumn(lines) != 0)
            {
                if (Vectors.ExistInfinity(lines) != 0)
                {
                    sf.WriteLine("Функция неограничена на области допустимых решений (столбец {0} таблицы полностью отрицателен)", Vectors.ExistInfinity(lines) + 1);
                    return Double.PositiveInfinity;
                }
                int m = Vectors.ExistColumn(lines);
                sf.WriteLine("На итерации {0} разрещающим выбран элемент на пересечении строки {1} и столбца {2}", it_num, Vectors.FindPosElem(lines, m) + 1, m + 1);
                Vectors.Transform(ref lines, Vectors.FindPosElem(lines, m), m, ref result);
                sf.WriteLine("Сиплекс-таблица после итерации {0}:", it_num);
                //Vectors.Show(lines);
                Vectors.ShowRational(lines, sf);
                sf.WriteLine();
                it_num++;
                //result.Show(); sf.WriteLine(); sf.WriteLine();
            }

            result = new Vectors(Vectors.GetSolutionVec(lines));
            //result.Show();
            //sf.WriteLine("Вектор решения {0}", result.ToString());
            sf.WriteLine("Вектор решения {0}", result.ToRationalString());
            sf.WriteLine("Оптимальное значение функции равно {0}", lines[k].vector[0]);

            return lines[k].vector[0];
        }
        /// <summary>
        /// Нахождение максимума функционала методами симплекс таблицы, заданной массивом векторов
        /// </summary>
        /// <param name="result">Вектор вида (0,0,1,0,1), показывающий, какие переменные являются базисными</param>
        /// <param name="lines">Симплекс-таблица, заданная массивом векторов</param>
        /// <param name="sf"></param>
        /// <returns></returns>
        public static double SimpleSimplex(ref Vectors result, ref Vectors[] lines, StreamWriter sf)
        {
            sf.WriteLine("Исходная сиплекс-таблица:");
            Vectors.ShowRational(lines, sf);
            sf.WriteLine("Вектор, указывающий на базис:");
            result.ShowRational(sf); sf.WriteLine(); sf.WriteLine();

            //Проверка на отрицательные свободные члены и их исправление
            while (Vectors.ExistColumnDual(lines) != -1)
            {
                int m = Vectors.ExistColumnDual(lines);
                int n = Vectors.FindPosElemDual(lines, m);
                sf.WriteLine($"Для избавления от отрицательных свободных коэффициентов в базисных переменных разрещающим выбран элемент на пересечении строки {m + 1} и столбца {n + 1}");
                Vectors.Transform(ref lines, m, n, ref result);
                sf.WriteLine("Сиплекс-таблица после избавления от отрицательных свободных коэффициентов в базисных переменных");
                //Vectors.Show(lines);
                Vectors.ShowRational(lines, sf);
                //sf.WriteLine();
            }

            //Решение
            int it_num = 1;
            while (Vectors.ExistColumn(lines) != 0)
            {
                if (Vectors.ExistInfinity(lines) != 0)
                {
                    sf.WriteLine("Функция неограничена на области допустимых решений (столбец {0} таблицы полностью отрицателен)", Vectors.ExistInfinity(lines) + 1);
                    sf.Close();
                    return Double.PositiveInfinity;
                }
                int m = Vectors.ExistColumn(lines);
                int n = Vectors.FindPosElem(lines, m);
                //sf.WriteLine("+");
                sf.WriteLine("На итерации {0} разрещающим выбран элемент на пересечении строки {1} и столбца {2}", it_num, n + 1, m + 1);
                //sf.WriteLine("++");
                Vectors.Transform(ref lines, Vectors.FindPosElem(lines, m), m, ref result);
                sf.WriteLine("Сиплекс-таблица после итерации {0}:", it_num);
                //Vectors.Show(lines);
                Vectors.ShowRational(lines, sf);
                sf.WriteLine();
                it_num++;
                //result.Show(); sf.WriteLine(); sf.WriteLine();
            }
            result = new Vectors(Vectors.GetSolutionVec(lines));

            //result.Show();
            //sf.WriteLine("Вектор решения {0}", result.ToString());
            sf.WriteLine("Вектор решения {0}:", result.ToRationalString());
            sf.WriteLine("Оптимальное значение функции равно: {0}", lines[lines.Length - 1].vector[0]);

            return lines[lines.Length - 1].vector[0];
        }
        /// <summary>
        /// Поиск целочисленного решения задачи линейного программирования, заданной симплекс-таблицей
        /// </summary>
        /// <param name="result">Вектор вида (0,0,1,0,1), показывающий, какие переменные являются базисными</param>
        /// <param name="fs">Файл с матрицей (симплекс-таблицей) размера (x.Lenth+1)X(xbase.Lenth+1)</param>
        /// <param name="sf"></param>
        /// <returns></returns>
        public static double SimplexInteger(ref Vectors result, StreamReader fs, StreamWriter sf)
        {
            //Считать таблицу
            //чтение данных
            int k = 0;//число единиц
            for (int i = 0; i < result.n; i++) if (result[i] != 0) k++;
            Vectors[] lines = new Vectors[k + 1];
            for (int i = 0; i < k + 1; i++)
            {
                lines[i] = new Vectors(result.n + 1);
                string s = fs.ReadLine();
                string[] st = s.Split(' ');
                for (int j = 0; j < lines[i].n; j++) lines[i].vector[j] = Convert.ToDouble(st[j]);
            }
            fs.Close();
            //Найти решение обычным симплекс-методом
            double end = Vectors.SimpleSimplex(ref result, ref lines, sf);

            if (Vectors.IsIntDesigion(lines) || end == Double.PositiveInfinity) { sf.Close(); return end; }
            int y = 0;
            sf.WriteLine("-------------->Оптимальное решение найдено. Начинается решение двойственной задачи с целью поиска целочисленности.");
            sf.WriteLine(); sf.WriteLine();

            //Если решение не целочисленное, то пока оно не является целочисленным и пока его разумно искать...
            while ((!Vectors.IsIntDesigion(lines)) && (!Vectors.ImpossibleFoundIntDesigion(lines)) && (y <= 15))
            {
                sf.WriteLine();
                sf.WriteLine("----------------------------------->Итерация метода Гомори номер {0}", y + 1);
                //Создаётся новая симплекс-таблица
                Vectors[] newlines = new Vectors[lines.Length + 1];
                for (int i = 0; i < lines.Length - 1; i++)//Переписать строки до строки с функционалом
                {
                    newlines[i] = new Vectors(lines[0].n + 1);
                    for (int j = 0; j < lines[0].n; j++) newlines[i].vector[j] = lines[i].vector[j];
                    newlines[i].vector[lines[0].n] = 0;
                }
                int tmp = Vectors.NumberOfNotIntDes(lines);//Номер строки, где решение не целочисленное
                sf.WriteLine("------------------------->За образец новой строки взята строка {0}", tmp + 1);
                newlines[lines.Length - 1] = new Vectors(lines[0].n + 1);
                newlines[lines.Length] = new Vectors(lines[0].n + 1);

                //Записать новую строку
                for (int j = 0; j < lines[0].n; j++) newlines[lines.Length - 1].vector[j] = -(/*lines[tmp].vector[j] - (int)lines[tmp].vector[j]*/Number.Rational.ToRational(lines[tmp].vector[j]).FracPart.ToDouble());
                newlines[lines.Length - 1].vector[lines[0].n] = 1;
                //Переписать строку функции с изменением знака, если итерация первая (и без изменения знака в противном случае)
                for (int j = 0; j < lines[0].n; j++)
                    if (y + 1 == 1) newlines[lines.Length].vector[j] = -lines[lines.Length - 1].vector[j];
                    else newlines[lines.Length].vector[j] = lines[lines.Length - 1].vector[j];
                newlines[lines.Length].vector[lines[0].n] = 0;
                //Записать новый вектор result
                Vectors newresult = new Vectors(result.n + 1);
                for (int i = 0; i < result.n; i++)
                {
                    if (result[i] == 0) newresult[i] = result[i];
                    else newresult[i] = 1;
                }
                newresult[result.n] = /*newlines[lines.Length - 1].vector[0]*/1;

                //sf.WriteLine("--------Новая таблица создана");

                //Переписать новые данные в старые
                result = new Vectors(newresult);
                lines = new Vectors[newlines.Length];
                for (int i = 0; i < newlines.Length; i++) lines[i] = new Vectors(newlines[i]);
                //sf.WriteLine("--------Новая таблица перезаписана");
                //sf.WriteLine("-----------Новое использование сиплекс-метода-----------");

                //Решать двойственным симплекс-методом
                //end = Vectors.SimpleSimplex(ref result, ref lines, sf);
                end = Vectors.SimplexDual(ref result, ref lines, sf, y + 1);
                y++;
            }

            sf.WriteLine();
            sf.WriteLine("----->------>--------> Целочисленное решение найдено либо целочисленного решения не существует");

            //Вернуть значение
            sf.WriteLine("Конечная симплекс-таблица: "); Vectors.ShowRational(lines, sf);
            sf.Write("Вектор решения:"); result.ShowRational(sf);
            sf.WriteLine($"Оптимальное значение функции равно {-lines[lines.Length - 1].vector[0]}");
            sf.Close();
            return end;
        }
        private static double SimplexDual(ref Vectors result, ref Vectors[] lines, StreamWriter sf, int y)
        {
            sf.WriteLine("Исходная сиплекс-таблица до избавления от отрицательных свободных коэффициентов в базисе:");
            Vectors.ShowRational(lines, sf);
            int k = 1;
            //if
            while (Vectors.ExistColumnDual(lines) != -1)
            {
                int m = Vectors.ExistColumnDual(lines);
                int n = Vectors.FindPosElemDual(lines, m);
                //sf.WriteLine("+");
                sf.WriteLine("На итерации {0}.{1} метода Гомори для избавления от отрицательных свободных коэффициентов в базисных переменных разрещающим выбран элемент на пересечении строки {2} и столбца {3}", y, k, m + 1, n + 1);
                //sf.WriteLine("++");
                Vectors.Transform(ref lines, m, n, ref result);
                sf.WriteLine("Сиплекс-таблица после итерации {0}.{1} алгоритма Гомори:", y, k);
                //Vectors.Show(lines);
                Vectors.ShowRational(lines, sf);
                //sf.WriteLine();
                k++;
            }
            if (!Vectors.NotExistColumn(lines))//если среди коэффициентов функционала есть положительные
            {
                result = new Vectors(Vectors.GetSolutionVec(lines));
                sf.WriteLine("Использование обычного симплекс-метода после {0}.{1} итерации алгоритма Гомори:", y, k);
                Vectors.SimpleSimplexD(ref result, ref lines, sf);
                //Vectors.SimpleSimplex(ref result, ref lines, sf);
            }
            result = new Vectors(Vectors.GetSolutionVec(lines));

            //sf.WriteLine("Вектор решения {0}:", result.ToRationalString());
            //sf.WriteLine("Оптимальное значение функции равно: {0}", -lines[lines.Length - 1].vector[0]);

            return -lines[lines.Length - 1].vector[0];
        }
        /// <summary>
        /// Проверяет симплекс-таблицу на наличие столбца (то есть строки, так как решается двойственная задача) с положительным элементом, где коэффициент при линейной форме отрицателен
        /// </summary>
        /// <param name="l"></param>
        /// <returns>Номер найденного столбца или 0, если таковой отсутствует</returns>
        /// <remarks>Выбирается стобец с наибольшей по модулю отрицательной частью в конце</remarks>
        private static int ExistColumnDual(Vectors[] l)
        {
            int k = -1;
            double max = 0;
            for (int j = 0; j < l.Length - 1; j++)
                if (l[j].vector[0] < 0)
                    if (Math.Abs(l[j].vector[0]) > Math.Abs(max))
                    {
                        max = l[j].vector[0];
                        k = j;
                    }
            if (k >= 0)
                for (int i = 1; i < l[0].n; i++)
                    if (l[k].vector[i] < 0) return k;
            return -1;
        }
        /// <summary>
        /// Найти позицию элемента, который годится под центр в симплекс-таблице (при решении двойственной задачи)
        /// </summary>
        /// <param name="l">Симплекс-таблица</param>
        /// <param name="k">Номер столбца, где этот элемент ищется</param>
        /// <returns>Номер строки, где этот элемент находится</returns>
        private static int FindPosElemDual(Vectors[] l, int k)
        {
            int t = 1;
            while (l[k].vector[t] >= 0) t++;
            double min = Math.Abs(l[l.Length - 1].vector[t] / l[k].vector[t]);//может возникнуть ошибка деления на 0
            int u = t;
            for (int i = t + 1; i < l[0].n - 1; i++)
            {
                if (l[k].vector[i] < 0)
                {
                    double tmp = Math.Abs(l[l.Length - 1].vector[i] / l[k].vector[i]);
                    if (tmp < min)
                    {
                        min = tmp;
                        u = i;
                    }
                }
            }
            return u;
        }
        private static double SimpleSimplexD(ref Vectors result, ref Vectors[] lines, StreamWriter sf)
        {
            sf.WriteLine("Исходная сиплекс-таблица:");
            Vectors.ShowRational(lines, sf);
            sf.WriteLine("Вектор, указывающий на базис:");
            result.ShowRational(sf); sf.WriteLine(); sf.WriteLine();

            //Решение
            int it_num = 1;
            while (Vectors.ExistColumnM(lines) != 0)
            {
                if (Vectors.ExistInfinity(lines) != 0)
                {
                    sf.WriteLine("Функция неограничена на области допустимых решений (столбец {0} таблицы полностью отрицателен)", Vectors.ExistInfinity(lines) + 1);
                    return Double.PositiveInfinity;
                }
                int m = Vectors.ExistColumnM(lines);
                int n = Vectors.FindPosElemM(lines, m);
                //sf.WriteLine("+");
                sf.WriteLine("На итерации {0} разрещающим выбран элемент на пересечении строки {1} и столбца {2}", it_num, n + 1, m + 1);
                //sf.WriteLine("++");
                Vectors.Transform(ref lines, n, m, ref result);
                sf.WriteLine("Сиплекс-таблица после итерации {0}:", it_num);
                //Vectors.Show(lines);
                Vectors.ShowRational(lines, sf);
                sf.WriteLine();
                it_num++;
            }

            result = new Vectors(Vectors.GetSolutionVec(lines));
            sf.WriteLine("Вектор решения {0}:", result.ToRationalString());
            sf.WriteLine("Оптимальное значение функции равно: {0}", -lines[lines.Length - 1].vector[0]);

            return -lines[lines.Length - 1].vector[0];
        }
        private static int ExistColumnM(Vectors[] l)
        {
            int k = 0;
            double max = 0;
            for (int j = 1; j < l[0].n; j++)
                if (l[l.Length - 1].vector[j] > 0)
                    if (Math.Abs(l[l.Length - 1].vector[j]) > Math.Abs(max))
                    {
                        max = l[l.Length - 1].vector[j];
                        k = j;
                    }
            for (int i = 0; i < l.Length - 1; i++)
                if (l[i].vector[k] < 0)
                {
                    return k;
                }
            return 0;
        }
        private static int FindPosElemM(Vectors[] l, int k)
        {
            int fix = 0;//Номер столбца, с которым сравниваем
            int t = 0;
            while (l[t].vector[k] <= 0) t++;
            double min = l[t].vector[fix] / l[t].vector[k];//может возникнуть ошибка деления на 0
            int u = t;
            for (int i = t + 1; i < l.Length - 1; i++)
            {
                if (l[i].vector[k] > 0)
                {
                    double tmp = l[i].vector[fix] / l[i].vector[k];
                    if (tmp < min)
                    {
                        min = tmp;
                        u = i;
                    }
                }
            };
            return u;
        }
        /// <summary>
        /// Проверяет симплекс-таблицу на отсутствие столбца с положительным элементом, где коэффициент при линейной форме отрицателен
        /// </summary>
        /// <param name="l"></param>
        /// <returns>Номер найденного столбца или 0, если таковой отсутствует</returns>
        /// <remarks>Выбирается стобец с наибольшей по модулю отрицательной частью в конце</remarks>
        private static bool NotExistColumn(Vectors[] l)
        {
            for (int j = 1; j < l[0].n; j++)
                if (l[l.Length - 1].vector[j] > 0)
                {
                    return false;
                }

            return true;
        }

        /// <summary>
        /// Перевод массива строк в матрицу
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static Matrix LinesToMatrix(Vectors[] l)
        {
            Matrix A = new Matrix(l.Length, l[0].n);
            for (int i = 0; i < A.n; i++)
                for (int j = 0; j < A.m; j++)
                    A[i, j] = l[i].vector[j];
            return A;
        }
        /// <summary>
        /// Перевод матрицы в массив строк
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Vectors[] MatrixToLines(Matrix A)
        {
            Vectors[] l = new Vectors[A.n];
            for (int i = 0; i < l.Length; i++)
            {
                l[i] = new Vectors(A.m);
                for (int j = 0; j < A.m; j++) l[i].vector[j] = A[i, j];
            }
            return l;
        }
        /// <summary>
        /// Содержат ли векторы общий элемент
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ExistIntersection(Vectors a, Vectors b)
        {
            for (int i = 0; i < a.n; i++)
                for (int j = 0; j < b.n; j++)
                    if (a[i] == b[j]) return true;
            return false;
        }

        //операторы
        //сложение
        public static Vectors operator +(Vectors A, Vectors B)
        {
            Vectors C = new Vectors(A.n);
            for (int i = 0; i < A.n; i++)
            {
                C[i] = A[i] + B[i];
            }
            return C;
        }

        public static Vectors operator +(Vectors a, Double b)
        {
            return a + new Vectors(a.n, b);
        }
        //вычитание
        public static Vectors operator -(Vectors A, Vectors B)
        {
            Vectors R = new Vectors(A.n);
            for (int i = 0; i < A.n; i++)
            {
                R[i] = A[i] - B[i];
            }
            return R;

        }
        public static Vectors operator -(Vectors A)
        {
            Vectors R = new Vectors(A.n);
            for (int i = 0; i < A.n; i++)
            {
                R[i] = -A[i];
            }
            return R;

        }

        public static Vectors operator -(Vectors v, double c)
        {
            Vectors r = new Vectors(v);
            for (int i = 0; i < r.n; i++)
                r[i] -= c;
            return r;
        }

        //произведение (скалярное)
        public static double operator *(Vectors A, Vectors B)
        {
            double sum = 0;
            for (int i = 0; i < A.n; i++)
            {
                sum += A[i] * B[i];

            }
            return sum;
        }

        //Умножение на число
        public static Vectors operator *(Vectors A, double Ch)
        {
            Vectors q = new Vectors(A.n);
            for (int i = 0; i < A.n; i++)
            {
                q[i] = A[i] * Ch;
            }
            return q;
        }
        public static Vectors operator *(double Ch, Vectors A) { return A * Ch; }
        //деление на число
        public static Vectors operator /(Vectors A, double Ch)
        {
            if (Ch == 0) throw new DivideByZeroException("Деление на ноль!");
            return A * (1.0 / Ch);
        }

        //public static bool operator ==(Vectors a, Vectors b)
        //{
        //    if (a.n != b.n) throw new Exception("Векторы не совпадают по длине!");
        //    for (int i = 0; i < a.n; i++)
        //        if (a[i] != b[i]) return false;
        //    return true;
        //}
        //public static bool operator !=(Vectors a,Vectors b) { return !(a==b); }

        /// <summary>
        /// Сравнение всех элементов с числом
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Ch"></param>
        /// <returns></returns>
        public static bool operator ==(Vectors a, double Ch)
        {
            for (int i = 0; i < a.n; i++)
                if (a[i] != Ch) return false;
            return true;
        }
        public static bool operator !=(Vectors a, double b) { return !(a == b); }
        public static bool operator >(Vectors a, double Ch)
        {
            for (int i = 0; i < a.n; i++)
                if (a[i] <= Ch) return false;
            return true;
        }
        public static bool operator <(Vectors a, double Ch)
        {
            for (int i = 0; i < a.n; i++)
                if (a[i] >= Ch) return false;
            return true;
        }

        public bool Equals(object vv)
        {
            Vectors v = vv as Vectors;
            if (this.n != v.n) return false;
            for (int i = 0; i < v.n; i++)
                if (this.vector[i] != v[i]) return false;
            return true;
        }

        /// <summary>
        /// Евклидова норма вектора
        /// </summary>
        public double EuqlidNorm
        {
            get
            {
                double s = 0;
                for (int i = 0; i < this.n; i++)
                    s += vector[i] * vector[i];
                return Math.Sqrt(s) / this.Deg;
            }
        }

        /// <summary>
        /// Вектор модулей
        /// </summary>
        public Vectors AbsVector
        {
            get
            {
                Vectors r = new Vectors(this);
                for (int i = 0; i < r.n; i++)
                    r[i] = r[i].Abs();
                return r;
            }
        }
        /// <summary>
        /// Максимальный элемент по модулю
        /// </summary>
        public double MaxAbs => AbsVector.Max;
        /// <summary>
        /// Минимальный элемент по модулю
        /// </summary>
        public double MinAbs => AbsVector.Min;
        /// <summary>
        /// Усреднённый вектор
        /// </summary>
        public Vectors ToAver
        {
            get
            {
                Vectors res = new Vectors(this);
                double a = res.ArithmeticAv;
                for (int i = 0; i < res.n; i++)
                    res[i] -= a;
                return res;
            }
        }
        /// <summary>
        /// Вектор, делённый на своё среднее
        /// </summary>
        public Vectors ToAverDel
        {
            get
            {
                Vectors res = new Vectors(this);
                double a = res.ArithmeticAv;
                for (int i = 0; i < res.n; i++)
                    res[i] /= a;
                return res;
            }
        }
        /// <summary>
        /// Смешение векторов abcd и xyz в вектор axbyczd
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vectors Mix(Vectors a, Vectors b)
        {
            Vectors v = new Vectors(a.n + b.n);
            int aa = 0, bb = 0, i = 0;
            while (i < v.n)
            {
                if (aa < a.n) { v[i] = a[aa++]; i++; }
                if (bb < b.n) { v[i] = b[bb++]; i++; }

            }
            return v;
        }
        /// <summary>
        /// Объединение с другим вектором, вдобавок сортировка
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public void UnionWith(Vectors v)
        {
            double[] mas = Expendator.Union(this.DoubleMas, v.DoubleMas);
            mas = mas.Distinct().ToArray();
            Array.Sort(mas);//new Vectors(mas).Show();
            this.n = mas.Length;
            this.vector = new double[mas.Length];
            for (int i = 0; i < mas.Length; i++)
                this.vector[i] = mas[i];
            //this.Show();
        }
        public static Vectors Union(Vectors a, Vectors b){Vectors tmp=new Vectors(a);tmp.UnionWith(b);return new Vectors(tmp); }

        public override int GetHashCode()
        {
            var hashCode = 1350185542;
            hashCode = hashCode * -1521134295 + n.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double[]>.Default.GetHashCode(vector);
            hashCode = hashCode * -1521134295 + Deg.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double[]>.Default.GetHashCode(DoubleMas);
            hashCode = hashCode * -1521134295 + ArithmeticAv.GetHashCode();
            hashCode = hashCode * -1521134295 + Average.GetHashCode();
            hashCode = hashCode * -1521134295 + RelAc.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vectors>.Default.GetHashCode(RelAcVec);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vectors>.Default.GetHashCode(RelAcSqr);
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vectors>.Default.GetHashCode(Sort);
            hashCode = hashCode * -1521134295 + EuqlidNorm.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vectors>.Default.GetHashCode(AbsVector);
            hashCode = hashCode * -1521134295 + MaxAbs.GetHashCode();
            hashCode = hashCode * -1521134295 + MinAbs.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vectors>.Default.GetHashCode(ToAver);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vectors>.Default.GetHashCode(ToAverDel);
            return hashCode;
        }
    }
    /// <summary>
    /// Комплексные векторы
    /// </summary>
    public class CVectors : IComparer
    {
        private Complex[] mas;
        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Complex this[int i]
        {
            get { return mas[i]; }
            set
            {
                mas[i] = value;
            }
        }
        /// <summary>
        /// Комплексный массив, отождествлённый с вектором
        /// </summary>
        public Complex[] ComplexMas
        {
            get
            {
                Complex[] res = new Complex[this.Degree];
                for (int i = 0; i < res.Length; i++)
                    res[i] = new Complex(mas[i]);
                return res;
            }
        }
        /// <summary>
        /// Действительная часть вектора
        /// </summary>
        public Vectors Re
        {
            get
            {
                Vectors v = new Vectors(this.mas.Length);
                for (int i = 0; i < v.n; i++)
                    v[i] = mas[i].Re;
                return v;
            }
        }
        /// <summary>
        /// Комплексная часть вектора
        /// </summary>
        public Vectors Im
        {
            get
            {
                Vectors v = new Vectors(this.mas.Length);
                for (int i = 0; i < v.n; i++)
                    v[i] = mas[i].Im;
                return v;
            }
        }
        /// <summary>
        /// Размерность вектора
        /// </summary>
        public int Degree => mas.Length;
        /// <summary>
        /// Модуль вектора как сумма евклидовых норм действительной и мнимой части
        /// </summary>
        public double Abs => this.Re.EuqlidNorm + this.Im.EuqlidNorm;

        /// <summary>
        /// Коплексный вектор по комплексному массиву 
        /// </summary>
        /// <param name="mas"></param>
        public CVectors(Complex[] mas)
        {
            this.mas = new Complex[mas.Length];
            for (int i = 0; i < mas.Length; i++)
                this.mas[i] = new Complex(mas[i]);
        }
        /// <summary>
        /// Комплексный вектор по действительному массиву
        /// </summary>
        /// <param name="mas"></param>
        public CVectors(double[] mas)
        {
            this.mas = new Complex[mas.Length];
            for (int i = 0; i < mas.Length; i++)
                this.mas[i] = new Complex(mas[i], 0);
        }
        /// <summary>
        /// Комплексный вектор по действительной и мнимой части
        /// </summary>
        /// <param name="R"></param>
        /// <param name="I"></param>
        public CVectors(Vectors R, Vectors I) : this(R.DoubleMas)
        {
            for (int i = 0; i < I.Deg; i++)
                mas[i] += Complex.I * I[i];
        }
        /// <summary>
        /// Пустой комплексный вектор указанной размерности
        /// </summary>
        /// <param name="k"></param>
        public CVectors(int k) : this(new double[k]) { }
        /// <summary>
        /// Копирование коплексного вектора
        /// </summary>
        /// <param name="v"></param>
        public CVectors(CVectors v) : this(v.ComplexMas) { }

        /// <summary>
        /// Комплексно сопряженный вектор
        /// </summary>
        public CVectors Conjugate
        {
            get
            {
                CVectors v = new CVectors(this.Degree);
                for (int i = 0; i < v.Degree; i++)
                    v[i] = this[i].Conjugate;
                return v;
            }
        }

        /// <summary>
        /// Перевод в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "(";
            for (int i = 0; i < this.Degree - 1; i++)
                s += this[i].ToString() + " ";
            s += this[Degree - 1].ToString() + ")";
            return s;
        }

        int IComparer.Compare(object x, object y)
        {
            CVectors a = x as CVectors;
            CVectors b = y as CVectors;
            for (int i = 0; i < a.Degree; i++)
            {
                int val = a[i].CompareTo(b[i]);
                if (val != 0) return val;
            }
            return 0;
        }

        /// <summary>
        /// Преобразование комплексного вектора в действительный (по действительной части)
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Vectors(CVectors v) => v.Re;
        /// <summary>
        /// Преобразование действительной вектора в комплексный
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator CVectors(Vectors v) => new CVectors(v.DoubleMas);

        /// <summary>
        /// Пустой комплексный вектор
        /// </summary>
        public static CVectors EmptyVector => new CVectors(new Complex[] { });

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="q"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Complex operator *(CVectors q, CVectors w)
        {
            Complex sum = 0;
            for (int i = 0; i < q.Degree; i++)
                sum += q[i] * w[i];
            return sum;
        }

        public static CVectors operator +(CVectors q, CVectors w) => new CVectors(q.Re + w.Re, q.Im + w.Im);
        public static CVectors operator -(CVectors q, CVectors w) => new CVectors(q.Re - w.Re, q.Im - w.Im);
        public static CVectors operator -(CVectors q) => new CVectors(-q.Re, -q.Im);
        public static CVectors operator *(CVectors v, Complex c)
        {
            CVectors res = new CVectors(v);
            for (int i = 0; i < v.Degree; i++)
                res[i] *= c;
            return res;
        }
        public static CVectors operator *(Complex c, CVectors v) => v * c;
        public static CVectors operator /(CVectors v,Complex c)
        {
            CVectors res = new CVectors(v);
            for (int i = 0; i < res.Degree; i++)
                res /= c;
            return res;
        }

    }

    /// <summary>
    /// Класс графов
    /// </summary>
    public class Graphs
    {
        /// <summary>
        /// Тип графа
        /// </summary>
        public enum Type { Full, Zero };
        /// <summary>
        /// Перечисление цветов
        /// </summary>
        private enum Color { White, Gray, Black };

        private class ML
        {
            public Matrix M;
            public List<int> L;

            public ML(Matrix A) { this.M = new Matrix(A); L = new List<int>(); }
            public ML(Matrix A, List<int> Li) { this.M = new Matrix(A); L = new List<int>(Li); }
            public ML(ML A) { this.M = new Matrix(A.M); L = new List<int>(A.L); }

            /// <summary>
            /// Смежно ли ребро j вершине в конце списка цепи
            /// </summary>
            /// <param name="j"></param>
            /// <returns></returns>
            public bool HasEdge(int j)
            {
                int tmp = this.L[L.Count - 1];
                if (this.M[tmp, j] != 0)
                    for (int i = 0; i < this.M.n; i++)
                        if (this.M[i, j] != 0 && i != tmp)
                            return true;
                return false;
            }

            /// <summary>
            /// Произвести шаг в поиске цепи
            /// </summary>
            /// <param name="j"></param>
            /// <returns></returns>
            public ML Step(int j)
            {
                Matrix A = new Matrix();
                List<int> L = new List<int>(this.L);
                //if(L.Count==0) L.
                int tmp = this.L[L.Count - 1]/*, tmp2 = tmp */;
                //if (L.Count >= 2) tmp2 = this.L[L.Count - 2];

                for (int i = 0; i < this.M.n; i++)
                    if (this.M[i, j] != 0 && i != tmp /*&& i!=tmp2*/)
                    {
                        L.Add(i);
                        A = new Matrix(this.M.ColumnDelete(j));
                        return new ML(A, L);
                    }
                throw new Exception("Ввиду проверки на смежность до этого места не должно было дойти");
            }

            public void Show()
            {
                this.M.PrintMatrix();
                Console.Write("List: ");
                for (int i = 0; i < this.L.Count; i++) Console.Write((L[i] + 1) + " ");
                Console.WriteLine();
                Console.WriteLine();
            }

            public static void Test(Graphs g)
            {
                ML a = new ML(g.B);
                a.L.Add(0);
                Console.WriteLine(a.HasEdge(1));
            }

        }
        /// <summary>
        /// Класс вершин графа
        /// </summary>
        public class Vertex
        {
            public int x, y;//координаты вершины на плоскости
            public int color = 0;

            public Vertex(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        /// <summary>
        /// Класс рёбер графа
        /// </summary>
        public class Edge : IComparable
        {
            /// <summary>
            /// Какие вершины соединяет ребро (инцидентные ребру)
            /// </summary>
            public int v1, v2;
            /// <summary>
            /// Длина ребра
            /// </summary>
            public double length = 1;

            /// <summary>
            /// Создать ребро по инцидентным вершинам
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            public Edge(int v1, int v2)
            {
                this.v1 = v1;//Math.Min(v1,v2);
                this.v2 = v2; //Math.Max(v1, v2);
            }
            /// <summary>
            /// Создать ребро по инцидентным вершинам и его длине
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <param name="s"></param>
            public Edge(int v1, int v2, double s) { this.v1 = v1/*Math.Min(v1, v2)*/; this.v2 = /*Math.Max(v1, v2)*/v2; this.length = s; }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="g"></param>
            public Edge(Edge g) { this.v1 = g.v1; this.v2 = g.v2; this.length = g.length; }

            public override string ToString()
            {
                return (this.v1 + 1).ToString() + "-" + (this.v2 + 1).ToString();
            }
            /// <summary>
            /// Вывести ребро на консоль
            /// </summary>
            public void Show()
            { Console.WriteLine(this.v1 + " - " + this.v2); }

            public int CompareTo(object obj)
            {
                Edge e = (Edge)obj;
                if (this < e) return -1;
                if (this == e) return 0;
                return 1;
            }

            //int IComparable.CompareTo(object obj)
            //{
            //    if (v1 < v2) return v1.CompareTo(obj);
            //    return v2.CompareTo(obj);
            //    throw new NotImplementedException();
            //}

            /// <summary>
            /// Совпадение рёбер. Рёбра считаются совпадающими, если они имеют одинаковую длину и одинаковые концевые вершины
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator ==(Edge a, Edge b)
            {
                return ((a.v1 == b.v1) && (a.v2 == b.v2) || (a.v1 == b.v2) && (a.v2 == b.v1)) && (a.length == b.length);
            }
            public static bool operator !=(Edge a, Edge b)
            { return !(a == b); }
            public static bool operator <(Edge a, Edge b)
            {
                Point ap = new Point(a.v1, a.v2);
                Point bp = new Point(b.v1, b.v2);
                return ap < bp;
            }
            public static bool operator >(Edge a, Edge b)
            {
                Point ap = new Point(a.v1, a.v2);
                Point bp = new Point(b.v1, b.v2);
                return ap > bp;
            }
        }

        /// <summary>
        /// Число вершин в графе
        /// </summary>
        private int p = 0;
        /// <summary>
        /// Число рёбер в графе
        /// </summary>
        private int? q = null;
        /// <summary>
        /// Матрица смежности
        /// </summary>
        private SqMatrix A;
        /// <summary>
        /// Список валетностей вершин
        /// </summary>
        private Vectors degrees = null;
        /// <summary>
        /// Тип графа
        /// </summary>
        private Type Prop;
        /// <summary>
        /// Каталог циклов
        /// </summary>
        private List<string> catalogCycles;
        /// <summary>
        /// Каталог вершин
        /// </summary>
        public List<Vertex> Ver;
        /// <summary>
        /// Каталог рёбер
        /// </summary>
        public List<Edge> Ed;
        /// <summary>
        /// Каталог простых цепей
        /// </summary>
        private List<string> Chains;
        /// <summary>
        /// Каталог маршрутов
        /// </summary>
        private List<string> Routes;
        /// <summary>
        /// Список независимых подмножеств вершин графа
        /// </summary>
        private List<Vectors> IndepSubsets;
        /// <summary>
        /// Список максимальных независимых вершин подмножества
        /// </summary>
        public List<Vectors> GreatestIndepSubsets;
        /// <summary>
        /// Список доминирующих подмножеств
        /// </summary>
        private List<Vectors> DominSubsets;
        /// <summary>
        /// Список минимальных доминирующих подмножеств
        /// </summary>
        public List<Vectors> MinimalDominSubsets;
        /// <summary>
        /// Число доминирования
        /// </summary>
        public int DominationNumber = 0;
        /// <summary>
        /// Число вершинного покрытия
        /// </summary>
        public int VCoatingNumber = 0;
        /// <summary>
        /// Число рёберного покрытия
        /// </summary>
        private int ECoatingNumber = 0;
        /// <summary>
        /// Число кликового покрытия
        /// </summary>
        public int CliquesNumber = 0;
        /// <summary>
        /// Число паросочетаний
        /// </summary>
        public int MatchingNumber = 0;
        /// <summary>
        /// Ядро графа
        /// </summary>
        private List<Vectors> Kernel;
        /// <summary>
        /// Список вершинных покрытий
        /// </summary>
        public List<Vectors> VCoatingSubsets;
        /// <summary>
        /// Список минимальных вершинных покрытий
        /// </summary>
        public List<Vectors> MinimalVCoatingSubsets;
        /// <summary>
        /// Список рёберных покрытий
        /// </summary>
        private List<List<Edge>> ECoatingSubsets;
        /// <summary>
        /// Список минимальных рёберных покрытий
        /// </summary>
        private List<List<Edge>> MinimalECoatingSubsets;
        /// <summary>
        /// Множества клик графа
        /// </summary>
        public List<Vectors> CliquesSubsets;
        /// <summary>
        /// Множество максимальных клик графа
        /// </summary>
        public List<Vectors> MaximalCliquesSubsets;
        /// <summary>
        /// Множество наибольших клик графа
        /// </summary>
        public List<Vectors> GreatestCliquesSubsets;
        /// <summary>
        /// Список паросочетаний
        /// </summary>
        private List<List<Edge>> MatchingSubsets;
        /// <summary>
        /// Список максимальных паросочетаний
        /// </summary>
        private List<List<Edge>> MaximalMatchingSubsets;
        /// <summary>
        /// Список наибольших паросочетаний
        /// </summary>
        private List<List<Edge>> GreatestMatchingSubsets;
        /// <summary>
        /// Эйлеровы циклы
        /// </summary>
        private List<string> EulerCycles;
        /// <summary>
        /// Эйлеровы цепи
        /// </summary>
        private List<string> EulerChains;

        /// <summary>
        /// Полный граф с 5 вершинами
        /// </summary>
        public static Graphs K5;
        /// <summary>
        /// Полный двудольный граф K(3,3)
        /// </summary>
        public static Graphs K3_3;

        //Конструкторы
        /// <summary>
        /// Пустой граф указанной размерности
        /// </summary>
        /// <param name="n"></param>
        public Graphs(int n) { this.A = new SqMatrix(n); this.p = n; this.q = 0; this.Prop = Type.Zero; GenerateCatalogs(); }
        /// <summary>
        /// Граф указанной размерности по типу
        /// </summary>
        /// <param name="n"></param>
        /// <param name="t"></param>
        public Graphs(int n, Graphs.Type t)
        {

            if (t == Graphs.Type.Zero) { this.A = new SqMatrix(n); this.p = n; this.Prop = Type.Zero; }
            if (t == Graphs.Type.Full)
            {
                this.A = new SqMatrix(n);
                for (int i = 0; i < n; i++)
                    for (int j = i + 1; j < n; j++)
                    {
                        A[i, j] = 1;
                        A[j, i] = 1;
                    }
                this.p = n; this.Prop = Type.Full;
                this.q = this.p * (this.p - 1) / 2;
            }

            GenerateCatalogs();//Console.WriteLine(12);
        }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="G"></param>
        public Graphs(Graphs G) { this.A = G.A; this.p = G.p; this.q = G.q; GenerateCatalogs(); }
        /// <summary>
        /// Конструктор по матрице смежности
        /// </summary>
        /// <param name="M"></param>
        public Graphs(SqMatrix M) { this.A = new SqMatrix(M); p = M.n; q = (M.n * M.n - M.NullValue) / 2; GenerateCatalogs(); }
        /// <summary>
        /// Конструктор по матрице смежности, расположенной в файле
        /// </summary>
        /// <param name="fs"></param>
        public Graphs(StreamReader fs) { SqMatrix M = new SqMatrix(fs); this.A = new SqMatrix(M); p = M.n; q = (M.n * M.n - M.NullValue) / 2; GenerateCatalogs(); }
        /// <summary>
        /// Создание графа по количеству вершин и набору рёбер
        /// </summary>
        /// <param name="n"></param>
        /// <param name="mas"></param>
        public Graphs(int n, Edge[] mas)
        {
            this.p = n;
            this.A = new SqMatrix(n);
            for (int i = 0; i < mas.Length; i++)
            {
                this.A[mas[i].v1, mas[i].v2] = mas[i].length;
                this.A[mas[i].v2, mas[i].v1] = mas[i].length;
            }
            GenerateCatalogs();
        }
        /// <summary>
        /// Создание графа по количеству вершин и списку рёбер
        /// </summary>
        /// <param name="n"></param>
        /// <param name="mas"></param>
        public Graphs(int n, List<Edge> mas)
        {
            this.p = n;
            this.A = new SqMatrix(n);
            for (int i = 0; i < mas.Count; i++)
            {
                this.A[mas[i].v1, mas[i].v2] = mas[i].length;
                this.A[mas[i].v2, mas[i].v1] = mas[i].length;
            }
            GenerateCatalogs();
        }
        /// <summary>
        /// Полный k-дольный граф
        /// </summary>
        /// <param name="k"></param>
        public Graphs(params int[] k)
        {
            int sum = 0;
            for (int i = 0; i < k.Length; i++) sum += k[i];
            Graphs g = new Graphs(sum);
            sum = 0;
            for (int i = 0; i < k.Length; i++)//по всем долям
            {
                for (int j = k[i] + sum; j < g.p; j++)//по нетронутым вершинам вне долей
                    for (int z = 0; z < k[i]; z++)//сделать смежную с каждой вершиной доли
                        g = g.IncludeEdges(new Edge(z, j));
                sum += k[i];
            }

            this.A = g.A; this.p = g.p; this.q = g.q; GenerateCatalogs();
        }

        static Graphs()
        {
            K5 = new Graphs(5, Graphs.Type.Full);
            K3_3 = new Graphs(3, 3);
        }

        //Cвойства
        /// <summary>
        /// Вектор валетностей графа
        /// </summary>
        public Vectors Deg
        {
            get
            {
                if (this.degrees == null)
                {
                    Vectors v = new Vectors(p);
                    for (int i = 0; i < p; i++)
                    {
                        int sum = 0;
                        for (int j = 0; j < p; j++)
                        {
                            sum += (int)A[i, j];
                        }
                        v[i] = sum;
                        //sum = 0;
                    }
                    this.degrees = v;
                    return v;
                }
                return this.degrees;
            }
        }
        /// <summary>
        /// Вектор передаточных чисел
        /// </summary>
        public Vectors P
        {
            get
            {
                Vectors v = new Vectors(this.p);
                SqMatrix M = new SqMatrix(this.Dist);
                for (int i = 0; i < this.p; i++)
                    for (int j = 0; j < this.p; j++)
                        v[i] += M[i, j];
                return v;
            }
        }
        /// <summary>
        /// Матрица Кирхгофа данного графа
        /// </summary>
        public SqMatrix Kirhg
        {
            get
            {
                SqMatrix D = new SqMatrix(this.p);
                for (int i = 0; i < this.p; i++) D[i, i] = this.Deg[i];
                return D - this.A;
            }
        }
        /// <summary>
        /// Число рёбер в графе
        /// </summary>
        public int Edges
        {
            get
            {
                if (/*this.Prop == Graphs.Type.Zero*/this.q != null) return (int)this.q;
                int s = 0;
                for (int i = 0; i < this.p; i++)
                    for (int j = i + 1; j < this.p; j++)
                        if (A[i, j] == 1) s++;
                this.q = s;
                return s;
            }
        }
        /// <summary>
        /// Дополнительный граф
        /// </summary>
        /// <returns></returns>
        public Graphs Addition { get { return Graphs.Additional(this); } }
        /// <summary>
        /// Матрица достижимости графа
        /// </summary>
        public SqMatrix Acces
        {
            get
            {
                int i = 0;
                SqMatrix Sum = this.A, Tmp = this.A;
                while ((Sum.NullValue > 0) && (i < 5))//5 итераций могут быть лишними...
                {
                    //Sum.PrintMatrix(); Console.WriteLine();
                    Tmp *= this.A;
                    Sum += Tmp;
                    i++;
                }

                for (int k = 0; k < this.p; k++)
                    for (int j = 0; j < this.p; j++) if (Sum[k, j] != 0) Sum[k, j] = 1;
                return Sum;
            }
        }
        /// <summary>
        /// Список рёбер графа
        /// </summary>
        public string SetOfEdges
        {
            get
            {
                string s = "( ";
                for (int i = 0; i < this.p; i++)
                    for (int j = i; j < this.p; j++)
                        if (this.A[i, j] != 0)
                        {
                            s += String.Format("{0}-{1} ", i + 1, j + 1);
                        }
                s += ")";
                return s;
            }
        }
        /// <summary>
        /// Матрица инцидентности графа
        /// </summary>
        public Matrix B
        {
            get
            {
                Matrix M = new Matrix(this.p, this.Edges);
                for (int i = 0; i < this.p; i++)
                    for (int j = 0; j < this.p; j++)
                        if (this.A[i, j] != 0)
                        {
                            if (j < i) M[i, this.NumberEd(j, i) - 1] = 1;
                            else M[i, this.NumberEd(i, j) - 1] = 1;
                        }

                return M;
            }
        }
        /// <summary>
        /// Матрица расстояний связного графа
        /// </summary>
        public SqMatrix Dist
        {
            get
            {
                if (!Graphs.Connectivity(this)) throw new Exception("Граф не является связным!");

                SqMatrix S = new SqMatrix(this.A);
                SqMatrix Tmp = new SqMatrix(S);
                SqMatrix M = this.A * this.A;
                int t = 2;
                while (Graphs.IsCvasFull(S))
                {
                    S = Graphs.MinDis(Tmp, M, t);
                    Tmp = new SqMatrix(S);//?
                                          //Console.WriteLine(t);
                                          //M.PrintMatrix();
                                          //Console.WriteLine();
                                          //S.PrintMatrix();
                                          //Console.WriteLine();
                                          //Console.WriteLine();
                    M *= this.A;
                    t++;
                }
                //for (int i = 0; i < S.n; i++) S[i, i] = 0;
                return S;
            }
        }
        private static SqMatrix MinDis(SqMatrix S, SqMatrix G, int t)
        {
            SqMatrix H = new SqMatrix(S.n);
            for (int i = 0; i < S.n; i++)
                for (int j = 0; j < S.n; j++)
                {
                    //if(S[i, j] > 0 && G[i,j]> 0) H[i, j] = Math.Min(G[i, j], S[i, j]);
                    //else H[i, j] = G[i, j];
                    if ((i != j) && (S[i, j] == 0) && (G[i, j] != 0)) H[i, j] = t;
                    else H[i, j] = S[i, j];
                }
            return H;
        }
        /// <summary>
        /// Содержит ли матрица нули где-то вне главной диагонали
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        private static bool IsCvasFull(SqMatrix S)
        {
            for (int i = 0; i < S.n; i++)
                for (int j = 0; j < S.n; j++)
                    if ((i != j) && (S[i, j] == 0)) return true;
            return false;
        }
        /// <summary>
        /// Расстояние между двумя вершинами
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int Distance(int i, int j) { return (int)this.Dist[i, j]; }
        /// <summary>
        /// Вектор эксцентриситетов
        /// </summary>
        public Vectors Eccentricity
        {
            get
            {
                Vectors v = new Vectors(this.p);
                SqMatrix M = this.Dist;
                for (int i = 0; i < this.p; i++)
                {
                    int max = (int)M[i, 0];
                    for (int j = 1; j < this.p; j++)
                        if (M[i, j] > max) max = (int)M[i, j];
                    v[i] = max;
                }
                return v;
            }
        }
        /// <summary>
        /// Радиус графа
        /// </summary>
        public int Radius
        {
            get
            {
                Vectors v = this.Eccentricity;
                int min = (int)v[0];
                for (int i = 1; i < v.n; i++)
                    if (v[i] < min) min = (int)v[i];
                return min;
            }
        }
        /// <summary>
        /// Диаметр графа
        /// </summary>
        public int Diameter
        {
            get
            {
                Vectors v = this.Eccentricity;
                int max = (int)v[0];
                for (int i = 1; i < v.n; i++)
                    if (v[i] > max) max = (int)v[i];
                return max;
            }
        }
        /// <summary>
        /// Номер вершины, которую можно взять за центр графа
        /// </summary>
        public int Center
        {
            get
            {
                Vectors v = this.Eccentricity;
                int min = (int)v[0];
                int k = 0;
                for (int i = 1; i < v.n; i++)
                    if (v[i] < min) { min = (int)v[i]; k = i; }
                return k + 1;
            }
        }
        /// <summary>
        /// Обхват графа
        /// </summary>
        public int G
        {
            get
            {
                int k = 0, min = catalogCycles[k].Length;
                for (int i = 1; i < catalogCycles.Count; i++)
                    if (catalogCycles[i].Length < min)
                    {
                        min = catalogCycles[i].Length;
                        k = i;
                    }
                Console.WriteLine("Цикл длины {0}: {1}", (min - 1) / 2, catalogCycles[k]);
                return (min - 1) / 2;
            }
        }
        /// <summary>
        /// Окружение графа
        /// </summary>
        public int C
        {
            get
            {
                int k = 0, max = catalogCycles[k].Length;
                for (int i = 1; i < catalogCycles.Count; i++)
                    if (catalogCycles[i].Length > max)
                    {
                        max = catalogCycles[i].Length;
                        k = i;
                    }
                Console.WriteLine("Цикл длины {0}: {1}", (max - 1) / 2, catalogCycles[k]);
                return (max - 1) / 2;
            }
        }
        /// <summary>
        /// Периферии графа
        /// </summary>
        public Vectors Peripherys
        {
            get
            {
                Vectors v = this.Eccentricity;
                int k = 0;
                for (int i = 0; i < this.p; i++)
                    if (v[i] == this.Diameter) k++;
                Vectors r = new Vectors(k);
                k = 0;
                for (int i = 0; i < this.p; i++)
                    if (v[i] == this.Diameter)
                    {
                        r[k] = i;
                        k++;
                    }
                return r;
            }
        }
        /// <summary>
        /// Медианы графа
        /// </summary>
        public Vectors Medians
        {
            get
            {
                double min = sums(0);
                int k = 1;
                for (int i = 1; i < this.p; i++)
                    if (sums(i) < min)
                    {
                        min = sums(i);
                        k = 1;
                    }
                    else if (sums(i) == min) k++;

                Vectors v = new Vectors(k);
                k = 0;
                for (int i = 0; i < this.p; i++)
                    if (sums(i) == min)
                    {
                        v[k] = i;
                        k++;
                    }
                return v;
            }
        }
        /// <summary>
        /// Число компонент связности
        /// </summary>
        public int ComponCount
        {
            get
            {
                int[] used = new int[this.p];
                for (int i = 0; i < this.p; ++i) used[i] = 0;

                void dfs(int start, int s)
                {
                    used[start] = s;
                    for (int v = 0; v < this.p; ++v)
                        if (this.A[start, v] != 0 && used[v] == 0)
                            dfs(v, s);
                }
                int Ncomp = 0;
                for (int i = 0; i < this.p; ++i)
                    if (used[i] == 0)
                        dfs(i, ++Ncomp);
                return Ncomp;
            }
        }
        /// <summary>
        /// Цикломатическое число
        /// </summary>
        public int CyclomaticN
        {
            get { return this.Edges + this.ComponCount - this.p; }
        }
        /// <summary>
        /// Список мостов
        /// </summary>
        public List<Edge> Bridges
        {
            get
            {
                List<Edge> t = new List<Edge>();

                for (int i = 0; i < this.Ed.Count; i++)
                {
                    Graphs g = this.DeleteEdges(this.Ed[i]);
                    if (g.ComponCount > 1) t.Add(this.Ed[i]);
                }

                return t;
            }
        }
        /// <summary>
        /// Список точек сочленения
        /// </summary>
        public List<Vertex> JointPoints
        {
            get
            {
                List<Vertex> t = new List<Vertex>();

                for (int i = 0; i < this.Ver.Count; i++)
                {
                    Graphs g = this.DeleteVertexes(i + 1);
                    if (g.ComponCount > 1) t.Add(this.Ver[i]);
                }

                return t;
            }
        }
        /// <summary>
        /// Вектор точек сочленения
        /// </summary>
        public Vectors JointVect
        {
            get
            {
                List<Vertex> t = new List<Vertex>();
                Vectors v = new Vectors(this.JointPoints.Count);
                int k = 0;
                for (int i = 0; i < this.Ver.Count; i++)
                {
                    Graphs g = this.DeleteVertexes(i + 1);
                    if (g.ComponCount > 1)
                    {
                        t.Add(this.Ver[i]);
                        v[k] = i + 1;
                        k++;
                    }
                }

                return v;
            }
        }
        /// <summary>
        /// Реберная связность
        /// </summary>
        public int Lambda
        {
            get
            {
                double min = Double.PositiveInfinity;
                for (int i = 0; i < this.p; i++)
                    for (int j = i + 1; j < this.p; j++)
                    {
                        int k = this.ChainsNotCroosedEdges(i, j).Count;
                        min = Math.Min(min, k);
                    }

                return (int)min;
            }
        }
        /// <summary>
        /// Вершинная связность
        /// </summary>
        public int Kappa
        {
            get
            {
                double min = Double.PositiveInfinity;
                for (int i = 0; i < this.p; i++)
                    for (int j = i + 1; j < this.p; j++)
                    {
                        int k = this.ChainsNotCroosedVertex(i, j).Count;
                        min = Math.Min(min, k);
                    }

                return (int)min;
            }
        }
        /// <summary>
        /// Хроматическое число графа
        /// </summary>
        public int ChromaticNumber
        {
            get
            {
                Polynom p = this.Xpolymon();
                for (int i = 1; i < this.p; i++)
                {
                    double tmp = p.Value(i);
                    Console.WriteLine("\tP({0})={1}", i, tmp);
                    if (tmp > 0/*== (int)tmp*/) return i;
                }
                return this.p;
            }
        }
        /// <summary>
        /// Плотность графа
        /// </summary>
        public double Density
        {
            get
            {
                return (double)this.Edges * 2 / this.p / (this.p - 1);
            }
        }
        /// <summary>
        /// Матрица кликов графа
        /// </summary>
        public Matrix CliquesMatrix
        {
            get
            {
                Matrix M = new Matrix(this.CliquesSubsets.Count, this.p);
                for (int i = 0; i < M.n; i++)
                    for (int j = 0; j < this.CliquesSubsets[i].n; j++)
                        M[i, (int)this.CliquesSubsets[i].vector[j] - 1] = 1;
                return M;
            }
        }
        /// <summary>
        /// Граф клик исходного графа
        /// </summary>
        public Graphs CliquesGraph
        {
            get
            {
                int size = this.MaximalCliquesSubsets.Count;
                SqMatrix M = new SqMatrix(size);

                for (int i = 0; i < size; i++)
                    for (int j = i + 1; j < size; j++)
                        if (Vectors.ExistIntersection(this.MaximalCliquesSubsets[i], this.MaximalCliquesSubsets[j]))//если оба множества имеют непустое пересечение
                        {
                            M[i, j] = 1;
                            M[j, i] = 1;
                        }

                return new Graphs(M);
            }
        }




        //Методы
        /// <summary>
        /// Список непересекающихся по рёбрам путей из i в j
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        /// <remarks>Вершины в аргументах считаются с нуля</remarks>
        private List<string> ChainsNotCroosedEdges(int a, int b)
        {
            List<string> line = new List<string>(this.Chains);

            //for (int i = 0; i < line.Count; i++) Console.WriteLine(line[i]);

            //отсев по концам
            for (int i = 0; i < line.Count; i++)
            {
                string s = line[i];
                int q = (int)Char.GetNumericValue(s[0]) - 1, w = (int)Char.GetNumericValue(s[s.Length - 1]) - 1;
                if (q == a && w == b || q == b && w == a) ;
                else
                {
                    line.RemoveAt(i);
                    i--;
                }

            }
            //for (int i = 0; i < line.Count; i++) Console.WriteLine(line[i]);
            //отсев по повторениям рёбер
            for (int i = 0; i < line.Count; i++)
            {
                string s = line[i];
                for (int j = 0; j < s.Length - 2; j += 2)
                {
                    //выделение ребра
                    string s1 = s.Substring(j, 3);
                    char[] arr = s1.ToCharArray();
                    Array.Reverse(arr);
                    string s2 = new string(arr);
                    //отсев остальных цепей с таким ребром
                    for (int k = i + 1; k < line.Count; k++)
                        if (line[k].IndexOf(s1) > -1 || line[k].IndexOf(s2) > -1)
                        {
                            line.RemoveAt(k);
                            k--;
                        }
                }
            }
            //for (int i = 0; i < line.Count; i++) Console.WriteLine(line[i]);
            return line;
        }
        /// <summary>
        /// Список непересекающихся по вершинам путей из i в j
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        /// <remarks>Вершины в аргументах считаются с нуля</remarks>
        private List<string> ChainsNotCroosedVertex(int a, int b)
        {
            List<string> line = new List<string>(this.Chains);

            //for (int i = 0; i < line.Count; i++) Console.WriteLine(line[i]);

            //отсев по концам
            for (int i = 0; i < line.Count; i++)
            {
                string s = line[i];
                int q = (int)Char.GetNumericValue(s[0]) - 1, w = (int)Char.GetNumericValue(s[s.Length - 1]) - 1;
                if (q == a && w == b || q == b && w == a) ;
                else
                {
                    line.RemoveAt(i);
                    i--;
                }

            }
            //for (int i = 0; i < line.Count; i++) Console.WriteLine(line[i]);
            //отсев по повторениям вершин
            for (int i = 0; i < line.Count; i++)
            {
                string s = line[i];
                for (int j = 2; j < s.Length - 2; j += 2)
                {
                    //выделение ребра
                    string s1 = s.Substring(j, 1);
                    //отсев остальных цепей с таким ребром
                    for (int k = i + 1; k < line.Count; k++)
                        if (line[k].IndexOf(s1) > -1)
                        {
                            line.RemoveAt(k);
                            k--;
                        }
                }
            }
            //for (int i = 0; i < line.Count; i++) Console.WriteLine(line[i]);
            return line;
        }


        /// <summary>
        /// Заполнить каталоги
        /// </summary>
        private void GenerateCatalogs()
        {
            Chains = new List<string>();
            Routes = new List<string>();
            catalogCycles = new List<string>();
            Ver = new List<Vertex>();
            Ed = new List<Edge>();
            IndepSubsets = new List<Vectors>();
            GreatestIndepSubsets = new List<Vectors>();
            DominSubsets = new List<Vectors>();
            MinimalDominSubsets = new List<Vectors>();
            Kernel = new List<Vectors>();
            VCoatingSubsets = new List<Vectors>();
            MinimalVCoatingSubsets = new List<Vectors>();
            ECoatingSubsets = new List<List<Edge>>();
            MinimalECoatingSubsets = new List<List<Edge>>();
            CliquesSubsets = new List<Vectors>();
            GreatestCliquesSubsets = new List<Vectors>();
            MaximalCliquesSubsets = new List<Vectors>();
            MaximalMatchingSubsets = new List<List<Edge>>();
            MatchingSubsets = new List<List<Edge>>();
            GreatestMatchingSubsets = new List<List<Edge>>();

            int R = 220;

            double h = 2 * Math.PI / this.p;
            for (int i = 0; i < this.p; i++)
            {
                int x = (int)(Math.Cos(i * h) * R) + 300;
                int y = (int)(Math.Sin(i * h) * R) + 260;
                Ver.Add(new Vertex(x, y));
                for (int j = i; j < this.p; j++)
                    if (this.A[i, j] != 0)
                        Ed.Add(new Edge(i, j));
            }
            cyclesSearch();
            chainsSearch();
            IndSub();
            //DominSub();
            //this.CliquesSub();
        }

        private void routsSearch(int t)
        {
            int[] color = new int[Ver.Count];
            for (int i = 0; i < Ver.Count; i++)
                for (int j = 0; j < Ver.Count; j++)
                {
                    for (int k = 0; k < Ver.Count; k++)
                        color[k] = 1;
                    DFSrouts(i, j, Ed, color, (i + 1).ToString(), t);
                }

            for (int i = 0; i < Routes.Count; i++)
            {
                char[] arr = Routes[i].ToCharArray();
                Array.Reverse(arr);
                string b = new string(arr);
                for (int j = i + 1; j < Routes.Count; j++)
                    if (Routes[j] == Routes[i] || Routes[j] == b) { Routes.RemoveAt(j); j--; }
            }
        }
        private void DFSrouts(int u, int endV, List<Edge> Ed, int[] color, string s, int t)
        {

            if ((s.Length - 1) / 2 == t)
            {
                Routes.Add(s);
                return;
            }
            for (int w = 0; w < Ed.Count; w++)
            {
                if (color[Ed[w].v2] == 1 && Ed[w].v1 == u)
                {
                    DFSrouts(Ed[w].v2, endV, Ed, color, s + "-" + (Ed[w].v2 + 1).ToString(), t);
                    //color[Ed[w].v2] = 1;
                }
                else if (color[Ed[w].v1] == 1 && Ed[w].v2 == u)
                {
                    DFSrouts(Ed[w].v1, endV, Ed, color, s + "-" + (Ed[w].v1 + 1).ToString(), t);
                    //color[Ed[w].v1] = 1;
                }
            }
        }
        private void chainsSearch()
        {
            int[] color = new int[Ver.Count];
            for (int i = 0; i < Ver.Count - 1; i++)
                for (int j = i + 1; j < Ver.Count; j++)
                {
                    for (int k = 0; k < Ver.Count; k++)
                        color[k] = 1;
                    DFSchain(i, j, Ed, color, (i + 1).ToString());
                }
        }
        private void DFSchain(int u, int endV, List<Edge> Ed, int[] color, string s)
        {
            //вершину не следует перекрашивать, если u == endV (возможно в endV есть несколько путей)
            if (u != endV)
                color[u] = 2;
            else
            {
                Chains.Add(s);
                return;
            }
            for (int w = 0; w < Ed.Count; w++)
            {
                if (color[Ed[w].v2] == 1 && Ed[w].v1 == u)
                {
                    DFSchain(Ed[w].v2, endV, Ed, color, s + "-" + (Ed[w].v2 + 1).ToString());
                    color[Ed[w].v2] = 1;
                }
                else if (color[Ed[w].v1] == 1 && Ed[w].v2 == u)
                {
                    DFSchain(Ed[w].v1, endV, Ed, color, s + "-" + (Ed[w].v1 + 1).ToString());
                    color[Ed[w].v1] = 1;
                }
            }
        }
        private void cyclesSearch()
        {
            int[] color = new int[Ver.Count];
            for (int i = 0; i < Ver.Count; i++)
            {
                for (int k = 0; k < Ver.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>();
                cycle.Add(i + 1);
                DFScycle(i, i, Ed, color, -1, cycle);
            }
        }
        private void DFScycle(int u, int endV, List<Edge> E, int[] color, int unavailableEdge, List<int> cycle)
        {
            if (catalogCycles.Count == 40)
            {
                //Console.WriteLine("Число циклов достигло cорока! поиск прекращён!");
                return;
            }

            //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
            if (u != endV)
                color[u] = 2;
            else if (cycle.Count >= 2)
            {
                cycle.Reverse();
                string s = cycle[0].ToString();
                for (int i = 1; i < cycle.Count; i++)
                    s += "-" + cycle[i].ToString();
                bool flag = false; //есть ли палиндром для этого цикла графа в List<string> catalogCycles?
                for (int i = 0; i < catalogCycles.Count; i++)
                    if (catalogCycles[i].ToString() == s)
                    {
                        flag = true;
                        break;
                    }
                if (!flag)
                {
                    cycle.Reverse();
                    s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    catalogCycles.Add(s);
                }
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v2 + 1);
                    DFScycle(E[w].v2, endV, E, color, w, cycleNEW);
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v1 + 1);
                    DFScycle(E[w].v1, endV, E, color, w, cycleNEW);
                    color[E[w].v1] = 1;
                }
            }
        }
        private double sums(int i)
        {
            double s = 0;
            for (int j = 0; j < this.p; j++) s += this.Dist[i, j];
            return s;
        }
        private void IndSub()
        {
            int k = 0;
            Vectors v = new Vectors();
            List<int> un = new List<int>(0);//список вершин этого множества
            for (int c = 0; c < this.p; c++)
            {
                un = new List<int>(0);//список вершин этого множества
                un.Add(c);
                for (int i = 0; i < this.p; i++)
                    if (i != c && this.IsNotRelated(un, i))//если вершина не смежная вершинам данного множества
                        un.Add(i);
                //if (un.Count > k)
                //{
                //k = un.Count;
                v = new Vectors(un.Count);
                for (int i = 0; i < v.n; i++) v[i] = un[i] + 1;
                IndepSubsets.Add(v);
                k = v.n;
                //}
            }
            k--;
            while (k > 0)
            {
                for (int c = 0; c < this.p; c++)
                {
                    un = new List<int>(0);//список вершин этого множества
                    un.Add(c);
                    for (int i = 0; i < this.p; i++)
                        if (i != c && this.IsNotRelated(un, i) && un.Count < k)//если вершина не смежная вершинам данного множества
                            un.Add(i);
                    v = new Vectors(un.Count);
                    for (int i = 0; i < v.n; i++) v[i] = un[i] + 1;
                    IndepSubsets.Add(v);
                }
                k--;
            }
            //отсеивание повторов
            for (int i = 0; i < IndepSubsets.Count - 1; i++)
                for (int j = i + 1; j < IndepSubsets.Count; j++)
                    if (IndepSubsets[i].Sort.Equals(IndepSubsets[j].Sort))
                    {
                        IndepSubsets.RemoveAt(j);
                        j--;
                    }
        }
        private static Vectors ToVectors(List<int> L)
        {
            Vectors v = new Vectors(L.Count);
            for (int i = 0; i < L.Count; i++) v[i] = L[i] + 1;
            return v;
        }
        /// <summary>
        /// Сгенерировать список доминирующих множеств
        /// </summary>
        public void DominSub()
        {
            List<List<int>> tmp = new List<List<int>>();//промежуточный список
            List<int> zero = new List<int>();
            for (int i = 0; i < this.p; i++) zero.Add(i);
            tmp.Add(zero);//добавить в список множество из всех вершин графа (оно считается доминирующим)

            while (tmp.Count > 0)//пока в промежуточном множестве есть элементы
            {
                List<int> R = new List<int>(tmp[0]);
                this.DominSubsets.Add(ToVectors(R));//занести это множество в список доминирующих
                int k = 0;
                for (int i = 0; i < R.Count; i++)//по очереди для каждой вершины
                {
                    List<int> Ri = new List<int>(R);
                    Ri.RemoveAt(i);//создать копию того множества, но без этой вершины
                    if (!this.IsDomination(Ri)) k++;//если получилось не доминирующее подмножество, запомнить это
                    else tmp.Add(Ri);//иначе занести в промежуточный список
                }
                if (k == R.Count) this.MinimalDominSubsets.Add(ToVectors(R));//если после удаления каждой вершины из множества получалось не доминирующее множество, добавить его в список минимальных
                tmp.RemoveAt(0);
            }
            //отсеивание повторов
            for (int i = 0; i < this.DominSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.DominSubsets.Count; j++)
                    if (DominSubsets[j].Sort.Equals(DominSubsets[i].Sort))
                    {
                        DominSubsets.RemoveAt(j);
                        j--;
                    }
            for (int i = 0; i < this.MinimalDominSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MinimalDominSubsets.Count; j++)
                    if (MinimalDominSubsets[j].Sort.Equals(MinimalDominSubsets[i].Sort))
                    {
                        MinimalDominSubsets.RemoveAt(j);
                        j--;
                    }
            GenerateKernel();
        }
        private void GenerateKernel()
        {
            for (int i = 0; i < this.DominSubsets.Count; i++)//из доминирующих подмножеств выбираются независимые
            {
                bool tmp = true;
                for (int j = 0; j < this.DominSubsets[i].n - 1; j++)
                    for (int k = j + 1; k < this.DominSubsets[i].n; k++)
                        if (this.A[(int)DominSubsets[i].vector[j] - 1, (int)DominSubsets[i].vector[k] - 1] != 0)
                        {
                            tmp = false;
                            break;
                        }
                if (tmp) Kernel.Add(this.DominSubsets[i]);
            }
        }
        /// <summary>
        /// Сгенерировать список вершинных покрытий
        /// </summary>
        public void VCoatingSub()
        {
            List<List<int>> tmp = new List<List<int>>();//промежуточный список
            List<int> zero = new List<int>();
            for (int i = 0; i < this.p; i++) zero.Add(i);
            tmp.Add(zero);//добавить в список множество из всех вершин графа (оно считается вершинным покрытием)

            while (tmp.Count > 0)//пока в промежуточном множестве есть элементы
            {
                List<int> R = new List<int>(tmp[0]);
                this.VCoatingSubsets.Add(ToVectors(R));//занести это множество в список вершинных покрытий
                int k = 0;
                for (int i = 0; i < R.Count; i++)//по очереди для каждой вершины
                {
                    List<int> Ri = new List<int>(R);
                    Ri.RemoveAt(i);//создать копию того множества, но без этой вершины
                    if (!this.IsVertexCoating(Ri)) k++;//если получилось не вершинное покрытие, запомнить это
                    else tmp.Add(Ri);//иначе занести в промежуточный список
                }
                if (k == R.Count) this.MinimalVCoatingSubsets.Add(ToVectors(R));//если после удаления каждой вершины из множества получалось не вершинное покрытие, добавить его в список минимальных
                tmp.RemoveAt(0);
            }
            //отсеивание повторов
            for (int i = 0; i < this.VCoatingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.VCoatingSubsets.Count; j++)
                    if (VCoatingSubsets[j].Sort.Equals(VCoatingSubsets[i].Sort))
                    {
                        VCoatingSubsets.RemoveAt(j);
                        j--;
                    }
            for (int i = 0; i < this.MinimalVCoatingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MinimalVCoatingSubsets.Count; j++)
                    if (MinimalVCoatingSubsets[j].Sort.Equals(MinimalVCoatingSubsets[i].Sort))
                    {
                        MinimalVCoatingSubsets.RemoveAt(j);
                        j--;
                    }
        }

        private static bool Equ(List<Edge> a, List<Edge> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
        private void ECoatingSub()
        {
            List<List<Edge>> tmp = new List<List<Edge>>();//промежуточный список
            List<Edge> zero = new List<Edge>();
            for (int i = 0; i < this.Edges; i++) zero.Add(this.Ed[i]);
            tmp.Add(zero);//добавить в список множество из всех рёбер графа (оно считается рёберным покрытием)

            try
            {
                while (tmp.Count > 0)//пока в промежуточном множестве есть элементы
                {
                    int g = tmp.Count - 1;
                    List<Edge> R = new List<Edge>(tmp[g]);
                    this.ECoatingSubsets.Add(R);//занести это множество в список рёберных покрытий
                    int k = 0/*,iter=0*/;

                    void EdCoat(int i)
                    {

                        //Console.WriteLine($"----------Начало итерации {i} при tmp.Count={g}");
                        List<Edge> Ri = new List<Edge>(R);
                        Ri.RemoveAt(i);//создать копию того множества, но без этого ребра
                        if (!this.IsEdgeCoating(Ri)) k++;//если получилось не рёберное покрытие, запомнить это
                        else tmp.Add(Ri);//иначе занести в промежуточный список
                                         //Console.WriteLine($"----------Конец итерации {i} при tmp.Count={g}");
                                         //iter++;
                    }
                    Parallel.For(0, R.Count - 1, EdCoat);
                    //if(iter==R.Count)
                    //      {
                    if (k == R.Count) this.MinimalECoatingSubsets.Add(R);//если после удаления каждого ребра из множества получалось не рёберное покрытие, добавить его в список минимальных
                    tmp.RemoveAt(g);
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("РАСПАРАЛЛЕЛИВАНИЕ ПРИВЕЛО К ИСКЛЮЧЕНИЮ '" + e.Message + "'. ВОЗМОЖНА ПОТЕРЯ НЕКОТОРЫХ ДАННЫХ. ЕСЛИ ВАЖНО ИМЕТЬ ВСЕ ДАННЫЕ О РЁБЕРНЫХ ПОКРЫТИЯХ, ПОПРОБУЙТЕ ПЕРЕЗАПУСТИТЬ ПРИЛОЖЕНИЕ С ДРУГИМ РЕЖИМОМ ОПЕРАЦИИ");
                //ShowEdgeListofL(this.ECoatingSubsets);
            }

            //отсеивание повторов
            for (int i = 0; i < this.ECoatingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.ECoatingSubsets.Count; j++)
                {
                    List<Edge> a = new List<Edge>(ECoatingSubsets[j]);
                    List<Edge> b = new List<Edge>(ECoatingSubsets[i]);
                    a.Sort();
                    b.Sort();
                    if (Graphs.Equ(a, b))
                    {
                        ECoatingSubsets.RemoveAt(j);
                        j--;
                    }
                }

            for (int i = 0; i < this.MinimalECoatingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MinimalECoatingSubsets.Count; j++)
                {
                    List<Edge> a = new List<Edge>(MinimalECoatingSubsets[j]);
                    List<Edge> b = new List<Edge>(MinimalECoatingSubsets[i]);
                    a.Sort();
                    b.Sort();
                    if (Graphs.Equ(a, b))
                    {
                        MinimalECoatingSubsets.RemoveAt(j);
                        j--;
                    }
                }
        }

        private void ECoatingSubFull()
        {
            List<List<Edge>> tmp = new List<List<Edge>>();//промежуточный список
            List<Edge> zero = new List<Edge>();
            for (int i = 0; i < this.Edges; i++) zero.Add(this.Ed[i]);
            tmp.Add(zero);//добавить в список множество из всех рёбер графа (оно считается рёберным покрытием)

            while (tmp.Count > 0)//пока в промежуточном множестве есть элементы
            {
                List<Edge> R = new List<Edge>(tmp[0]);
                this.ECoatingSubsets.Add(R);//занести это множество в список рёберных покрытий
                int k = 0;
                for (int i = 0; i < R.Count; i++)//по очереди для каждого ребра
                {
                    List<Edge> Ri = new List<Edge>(R);
                    Ri.RemoveAt(i);//создать копию того множества, но без этого ребра
                    if (!this.IsEdgeCoating(Ri)) k++;//если получилось не рёберное покрытие, запомнить это
                    else tmp.Add(Ri);//иначе занести в промежуточный список
                }

                if (k == R.Count) this.MinimalECoatingSubsets.Add(R);//если после удаления каждого ребра из множества получалось не рёберное покрытие, добавить его в список минимальных
                tmp.RemoveAt(0);
            }

            //отсеивание повторов
            for (int i = 0; i < this.ECoatingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.ECoatingSubsets.Count; j++)
                {
                    List<Edge> a = new List<Edge>(ECoatingSubsets[j]);
                    List<Edge> b = new List<Edge>(ECoatingSubsets[i]);
                    a.Sort();
                    b.Sort();
                    if (Graphs.Equ(a, b))
                    {
                        ECoatingSubsets.RemoveAt(j);
                        j--;
                    }
                }

            for (int i = 0; i < this.MinimalECoatingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MinimalECoatingSubsets.Count; j++)
                {
                    List<Edge> a = new List<Edge>(MinimalECoatingSubsets[j]);
                    List<Edge> b = new List<Edge>(MinimalECoatingSubsets[i]);
                    a.Sort();
                    b.Sort();
                    if (Graphs.Equ(a, b))
                    {
                        MinimalECoatingSubsets.RemoveAt(j);
                        j--;
                    }
                }
        }
        //private void EdCoat(List<Edge> R,int k, List<List<Edge>> tmp,int i)
        //{
        //    List<Edge> Ri = new List<Edge>(R);
        //    Ri.RemoveAt(i);//создать копию того множества, но без этого ребра
        //    if (!this.IsEdgeCoating(Ri)) k++;//если получилось не рёберное покрытие, запомнить это
        //    else tmp.Add(Ri);//иначе занести в промежуточный список
        //}

        private List<List<int>> GetDCliques()
        {
            List<int> zero = new List<int>();
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < this.p; i++)
                for (int j = i + 1; j < this.p; j++)
                    if (this.A[i, j] != 0)
                    {
                        zero.Add(i);
                        zero.Add(j);
                        result.Add(zero);
                        //Console.WriteLine(zero[0] + " " + zero[1]);
                        zero.Clear();

                    }
            //for (int i = 0; i < result.Count; i++)
            //{
            //    for (int j = 0; j < result[i].Count; j++)
            //        Console.Write(result[i].ElementAt(j) + "+");
            //    Console.WriteLine();
            //}
            return result;
        }
        /// <summary>
        /// Сгенерировать список кликов графа
        /// </summary>
        public void CliquesSub()
        {
            List<List<int>> tmp = new List<List<int>>(this.GetDCliques());//промежуточный список, изначально состоящий из всех пар смежных вершин

            while (tmp.Count > 0)//пока в промежуточном множестве есть элементы
            {
                //if(tmp[0].Count>=1)
                //{
                List<int> R = new List<int>(tmp[0]); //Console.WriteLine(R[0] + " " + R[1]);
                Vectors v = Graphs.ToVectors(R);
                if (v.n >= 2) this.CliquesSubsets.Add(v);//занести это множество в список клик; из-за какого-то бага сначала появляются векторы с менее чем двумя компонентами
                int k = 0;

                for (int i = 0; i < this.p; i++)//по очереди для каждой вершины в графе
                    if (!v.Contain(i + 1))//если эта вершина ещё не содержится в клике
                    {
                        List<int> Ri = new List<int>(R);
                        Ri.Add(i);//создать копию того множества, но c новой вершиной
                        Vectors c = Graphs.ToVectors(Ri);
                        Graphs g = this.SubGraph(c);

                        if (!Graphs.IsFull(g)) k++;//если получился не клик, запомнить это
                        else tmp.Add(Ri);//иначе занести в промежуточный список
                    }

                if (k + R.Count == this.p) this.MaximalCliquesSubsets.Add(v);//если после добавления каждой вершины вне множества получался не клик, добавить его в список максимальных
                                                                             //}

                tmp.RemoveAt(0);
            }

            //отсеивание повторов
            for (int i = 0; i < this.CliquesSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.CliquesSubsets.Count; j++)
                    if (CliquesSubsets[j].Sort.Equals(CliquesSubsets[i].Sort))
                    {
                        CliquesSubsets.RemoveAt(j);
                        j--;
                    }
            for (int i = 0; i < this.MaximalCliquesSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MaximalCliquesSubsets.Count; j++)
                    if (MaximalCliquesSubsets[j].Sort.Equals(MaximalCliquesSubsets[i].Sort))
                    {
                        MaximalCliquesSubsets.RemoveAt(j);
                        j--;
                    }

            int t = 0;
            //выделить наибольшие клики
            for (int i = 0; i < this.MaximalCliquesSubsets.Count; i++)
                if (MaximalCliquesSubsets[i].n > t) t = MaximalCliquesSubsets[i].n;
            this.CliquesNumber = t;
            for (int i = 0; i < this.MaximalCliquesSubsets.Count; i++)
                if (MaximalCliquesSubsets[i].n == t) GreatestCliquesSubsets.Add(MaximalCliquesSubsets[i]);
        }

        private static bool ContainEdge(Edge e, List<Edge> L)
        {
            for (int i = 0; i < L.Count; i++)
                if (L[i] == e) return true;
            return false;
        }
        private static bool IsMatching(List<Edge> L)
        {
            for (int i = 0; i < L.Count - 1; i++)
                for (int j = i + 1; j < L.Count; j++)
                    if (L[i].v1 == L[j].v1 || L[i].v1 == L[j].v2 || L[i].v2 == L[j].v1 || L[i].v2 == L[j].v2)
                        return false;
            return true;
        }

        /// <summary>
        /// Сгенерировать список паросочетаний
        /// </summary>
        public void MatchingSub()
        {
            List<List<Edge>> tmp = new List<List<Edge>>();//промежуточный список
            List<Edge> zero = new List<Edge>();
            for (int i = 0; i < this.Edges; i++)
            {
                zero.Add(this.Ed[i]);
                tmp.Add(zero);//добавить в список по каждому ребру (ребро считается паросочетанием)
                zero.Clear();
            }


            while (tmp.Count > 0)//пока в промежуточном множестве есть элементы
            {
                List<Edge> R = new List<Edge>(tmp[0]);
                if (R.Count > 0) this.MatchingSubsets.Add(R);//занести это множество в список паросочетаний
                int k = 0;
                for (int i = 0; i < this.Edges; i++)//по очереди для каждого ребра
                    if (!Graphs.ContainEdge(this.Ed[i], R))
                    {
                        List<Edge> Ri = new List<Edge>(R);
                        Ri.Add(this.Ed[i]);//создать копию того множества, но с новым ребром
                        if (!Graphs.IsMatching(Ri)) k++;//если получилось не паросочетание, запомнить это
                        else tmp.Add(Ri);//иначе занести в промежуточный список
                    }
                if (k + R.Count == this.Edges) this.MaximalMatchingSubsets.Add(R);//если после добавления каждого ребра из множества получалось не паросочетание, добавить его в список максимальных
                tmp.RemoveAt(0);
            }
            //отсеивание повторов
            for (int i = 0; i < this.MatchingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MatchingSubsets.Count; j++)
                {
                    List<Edge> a = new List<Edge>(MatchingSubsets[j]);
                    List<Edge> b = new List<Edge>(MatchingSubsets[i]);
                    a.Sort();
                    b.Sort();
                    if (Graphs.Equ(a, b))
                    {
                        MatchingSubsets.RemoveAt(j);
                        j--;
                    }
                }

            for (int i = 0; i < this.MaximalMatchingSubsets.Count - 1; i++)
                for (int j = i + 1; j < this.MaximalMatchingSubsets.Count; j++)
                {
                    List<Edge> a = new List<Edge>(MaximalMatchingSubsets[j]);
                    List<Edge> b = new List<Edge>(MaximalMatchingSubsets[i]);
                    a.Sort();
                    b.Sort();
                    if (Graphs.Equ(a, b))
                    {
                        MaximalMatchingSubsets.RemoveAt(j);
                        j--;
                    }
                }

            int t = 0;
            //выделить наибольшие паросочетания
            for (int i = 0; i < this.MaximalMatchingSubsets.Count; i++)
                if (MaximalMatchingSubsets[i].Count > t) t = MaximalMatchingSubsets[i].Count;
            this.MatchingNumber = t;
            for (int i = 0; i < this.MaximalMatchingSubsets.Count; i++)
                if (MaximalMatchingSubsets[i].Count == t) GreatestMatchingSubsets.Add(MaximalMatchingSubsets[i]);
        }


        /// <summary>
        /// Номер ребра, соответствующего заданному элементу матрицы смежности
        /// </summary>
        /// <param name="ii"></param>
        /// <param name="jj"></param>
        /// <returns></returns>
        private int NumberEd(int ii, int jj)
        {
            int z = 0;
            for (int i = 0; i < this.p; i++)
                for (int j = i; j < this.p; j++)
                    if (this.A[i, j] != 0)
                    {
                        z++;
                        if ((i == ii) && (j == jj)) return z;
                    }
            throw new Exception("Элемент не обозначает ребро или не существует");
        }

        /// <summary>
        /// Проверка на изоморфизм графов
        /// </summary>
        /// <param name="Q"></param>
        /// <param name="W"></param>
        /// <returns>True, если граф связен</returns>
        public static bool Isomorphism(Graphs Q, Graphs W) { return (Q.A.CharactPol == W.A.CharactPol); }//совпадение характеристических многочленов матриц
                                                                                                         /// <summary>
                                                                                                         /// Проверка на связность графа
                                                                                                         /// </summary>
                                                                                                         /// <param name="Q"></param>
                                                                                                         /// <returns></returns>
        public static bool Connectivity(Graphs Q)//Проверка через сумму ряда степений матрицы смежности
        {
            if (Q.A.NullValue == 0) return true;
            //Q.Acces.PrintMatrix();
            if (Q.Acces.NullValue == 0) return true;
            return false;
        }
        /// <summary>
        /// Число вершин графа с нечётными степенями
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private static int CountN(Graphs g)
        {
            int k = 0;
            Vectors v = g.Deg;
            for (int i = 0; i < v.n; i++)
                if (v[i] % 2 != 0) k++;
            return k;
        }
        /// <summary>
        /// Гомеоморфизм графов
        /// </summary>
        /// <param name="Q"></param>
        /// <param name="W"></param>
        /// <returns></returns>
        public static bool Gomeomorphism(Graphs Q, Graphs W) { return (Graphs.CountN(Q) == Graphs.CountN(W)); }
        /// <summary>
        /// Пример графа, гомеоморфного данному (посередине ребра поставлена вершина степени 2)
        /// </summary>
        /// <returns></returns>
        public Graphs GomeoExample()
        {
            if (this.Edges < 1) throw new Exception("Нельзя постоить граф, гомеоморфный пустому!");
            Graphs g = new Graphs(this.p + 1, this.Ed);
            //int a = g.Ed[0].v1, b = g.Ed[0].v2;
            //g.Ed.RemoveAt(0);
            Edge w = new Edge(g.Ed[0]);
            g = g.DeleteEdges(w);
            //g.Ed.Add(new Edge(a, g.p - 1));
            //g.Ed.Add(new Edge(b, g.p - 1));
            Edge r = new Edge(g.p - 1, w.v1);
            Edge t = new Edge(g.p - 1, w.v2);
            //r.Show();
            //t.Show();
            g = g.IncludeEdges(r, t);

            return g;
        }
        /// <summary>
        /// Пример первообразного графа (вершина степени 2 заменена ребром)
        /// </summary>
        public Graphs OrigExample()
        {
            if (this.Edges < 2) throw new Exception("Нельзя постоить граф, первообразный от графа без вершин валентности 2!");
            Vectors v = this.Deg;
            int k = 0;
            bool f = true;
            int[] s = new int[2];

            while (f)
            {
                while ((int)v[k] != 2)
                {
                    k++;
                    if (k == v.n)
                    {
                        Console.WriteLine("Нельзя построить первообразный граф, так как исходный граф не содержит вершин степени 2 либо для всякой вершины степени 2 обе смежные ей смежны и друг другу.");
                        Console.WriteLine("В этом случае выводится исходных граф.");
                        return this;
                    }
                }

                int t = 0;
                for (int i = 0; i < this.p; i++)
                    if (this.A[k, i] != 0) { s[t] = i; t++; }
                if (A[s[0], s[1]] == 0) f = false;
                else k++;
            }
            //Console.WriteLine("{0} {1} {2}", k + 1, s[0] + 1, s[1] + 1);
            Graphs g = new Graphs(this);
            g = g.IncludeEdges(new Edge(s[0], s[1]));
            g = g.DeleteVertexes(k + 1);
            return g;
        }

        /// <summary>
        /// Полнота графа
        /// </summary>
        /// <param name="E"></param>
        /// <returns></returns>
        public static bool IsFull(Graphs E)
        {
            //if (E.p <= 1) return false;
            Vectors r = E.Deg;
            for (int i = 0; i < r.n; i++)
                if (r[i] != E.p - 1) return false;
            return true;
        }
        /// <summary>
        /// Регулярность графа
        /// </summary>
        /// <param name="E"></param>
        /// <returns></returns>
        public static bool IsRegular(Graphs E)
        {
            Vectors r = E.Deg;
            int t = (int)r[0];
            for (int i = 1; i < r.n; i++)
                if (r[i] != t) return false;
            return true;
        }
        /// <summary>
        /// Самодополнительность графа
        /// </summary>
        /// <param name="E"></param>
        /// <returns></returns>
        public static bool IsSelfAdditional(Graphs E)
        {
            Vectors one = new Vectors(E.Deg);
            Vectors two = new Vectors(E.Addition.Deg);
            Array.Sort(one.vector);
            Array.Sort(two.vector);
            for (int i = 0; i < one.n; i++)
                if (one[i] != two[i])
                {
                    Console.WriteLine("Граф не самодополнительный, так как не выполняется необходимое условие: список валентностей его вершин не совпадает со списком валентности вершин дополнительного графа");
                    return false;
                }
            return Graphs.Isomorphism(E, E.Addition);
        }

        /// <summary>
        /// Вывести кратчайшую цепь между двумя вершинами (Алгоритм Дейкстры)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns>Вектор как перечисление вершин в цикле, но отчёт вершин начинается с первой</returns>
        public Vectors Chain(int s, int t)
        {
            //заполнение начальных данных
            Vectors r = new Vectors(this.p);//вектор расстояний
            Vectors pr = new Vectors(this.p);//вектор предшественников
            bool[] l = new bool[this.p];//поставлена ли пометка
            for (int i = 0; i < this.p; i++)
            {
                r[i] = Double.PositiveInfinity;
                pr[i] = -1;
                l[i] = false;
            }
            r[s] = 0; l[s] = true;
            int p = s;
            int k = 0;

            //цикл выполняется, пока не найдётся цепь от s до t
            while (!l[t])
            {
                double tmp = Double.PositiveInfinity;
                //помечать вершины и находить минимальное значение
                for (int i = 0; i < this.p; i++)
                    if (!l[i] && this.A[i, p] != 0)
                        if (r[i] > r[p] + this.A[i, p])
                        {
                            r[i] = r[p] + this.A[i, p];
                            pr[i] = p;

                        }

                for (int i = 0; i < this.p; i++) if (r[i] < tmp && !l[i]) tmp = r[i];


                //найти минимальную вершину среди всех помеченных
                for (int i = 0; i < this.p; i++)
                    if (!l[i] && r[i] == tmp /*&& this.A[i, p] != 0*/)
                    {
                        l[i] = true;//пометить вершину
                        p = i;//перейти на эту вершину
                        break;
                    }
                k++;//счётчик по циклам 
                    //r.Show();
                    //pr.Show();
                    //    Console.WriteLine();
            }


            //преобразовать полученные данные в цепь
            k = 0;
            int e = -1; int q = t;
            while (q != s)
            {
                e = (int)pr[q];
                q = e;
                k++;
            }

            double[] m = new double[k + 1];
            m[k] = t + 1;
            q = t;
            for (int i = k - 1; i >= 0; i--)
            { m[i] = pr[q] + 1; q = (int)(m[i] - 1); }

            return new Vectors(m);
        }
        /// <summary>
        /// Является ли маршрут цепью
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsChain(string s)
        {
            for (int i = 0; i < s.Length - 2; i += 2)
            {
                string a = s.Substring(i, 3);
                char[] arr = a.ToCharArray();
                Array.Reverse(arr);
                string b = new string(arr);

                //Console.WriteLine(a + " " + b);
                if (s.IndexOf(a, i + 1) > -1 || s.IndexOf(b, i + 1) > -1) return false;
            }

            return true;
        }
        /// <summary>
        /// Является ли маршрут простой цепью
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsSimpleChain(string s)
        {
            if (IsChain(s))
            {
                string[] st = s.Split('-');
                int n = st.Length;
                double[] vector = new double[n];
                for (int i = 0; i < n; i++) vector[i] = Convert.ToDouble(st[i]);
                Array.Sort(vector);
                for (int i = 0; i < n - 1; i++)
                    if (vector[i] == vector[i + 1]) return false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Является ли маршрут циклом
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsCycle(string s)
        {
            if (IsChain(s))
            {
                string[] st = s.Split('-');
                int n = st.Length;
                double[] vector = new double[n];
                for (int i = 0; i < n; i++) vector[i] = Convert.ToDouble(st[i]);
                if (vector[0] != vector[n - 1]) return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Вывести все простые циклы в графе
        /// </summary>
        public void ShowAllCycles()
        {
            string[] s = new string[catalogCycles.Count];
            catalogCycles.CopyTo(s);
            for (int i = 0; i < s.Length; i++) Console.WriteLine(s[i]);
        }
        private bool ContainCycle()
        {
            //Vectors stack = new Vectors(this.p);
            //Vectors stack2 = new Vectors(this.p);
            //Color[] color = new Color[this.p];
            //int sp = 0, sp2 = 0, cc = 0;

            //for (int i = 0; i < this.p; i++) color[i] = Color.White;
            //bool rm = false, Found = false;
            //DFS(0, ref color, ref stack, ref stack2, ref Found, ref sp, ref rm, ref cc, ref sp2);
            //stack.Show();
            //stack2.Show();
            //return Found;
            return this.catalogCycles.Count > 0;
        }
        /// <summary>
        /// Является ли граф ациклическим
        /// </summary>
        /// <returns></returns>
        public bool IsAcyclic() { return !this.ContainCycle(); }
        /// <summary>
        /// Вывести один цикл указанной длины
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Цикл указанной длины или нулевой вектор в случае отсутствия такого цикла</returns>
        public string GetCycleExample(int t)
        {
            int k = 0;
            while ((catalogCycles[k].Length - 1) / 2 != t) k++;
            return catalogCycles[k];
        }

        /// <summary>
        /// Вывести на консоль все простые циклы указанной длины либо сообщение об их отсутствии
        /// </summary>
        /// <param name="t"></param>
        public void ShowAllCycles(int t)
        {
            if ((t <= 2) || (t > this.p))
            {
                Console.WriteLine("Простых циклов указанной длины не может существовать");
                return;
            }
            int cn = 0;//число найденных циклов

            //for (int i = 0; i < this.p; i++)
            //    for (int j = i + 1; j < this.p; j++)//Проход по всевозможным неповторяющимся парам вершин в графе
            //    {
            //        Vectors ij = this.Chain(i, j);//зафиксировать кратчайшую цепь между этими вершинами
            //        //ij.Show();
            //        if ((ij.n - 1) + 2 <= t)//если цепь ij имеет не настолько большую длину, чтобы цикла не могло быть
            //            for (int k = j + 1; k < this.p; k++)//по всем третьим вершинам
            //                if (!ij.Contain(k + 1))//если цепь ij не проходит через k (в Chain отчёт ведётся от 1, а не от 0) 
            //                {
            //                    Vectors jk = this.Chain(j, k);
            //                    Vectors ki = this.Chain(k, i);
            //                    if (ij.n - 1 + jk.n - 1 + ki.n - 1 == t)//если число рёбер в их объединении равно указанной длине цикла
            //                    {
            //                        Vectors v = Vectors.Merge(ij, jk, ki);//создать цикл
            //                        //v.ShowPlusOne();
            //                        if (Vectors.IsSimpleCycle(v))//если получился простой цикл
            //                        {
            //                            v.Show();
            //                            cn++;
            //                        }
            //                    }
            //                }

            //    }
            for (int k = 0; k < catalogCycles.Count; k++)
                if ((catalogCycles[k].Length - 1) / 2 == t)
                {
                    Console.WriteLine(catalogCycles[k]);
                    cn++;
                }

            if (cn == 0) Console.WriteLine("Простых циклов указанной длины не существует");
            //----------------------------------------
            ////Vectors v = new Vectors(t+1);//вектор, отображающий цикл
            //Vectors color = new Vectors(this.p);//"цвета" вершин графа
            //Vectors r = new Vectors(this.p);//Вектор предшественников
            //for (int i = 0; i < r.n; i++) r[i] = -1;//Изначально вершины не имеют предшественников

            //for (int i = 0; i < this.p; i++)//цикл начинается по каждой вершине
            //{
            //    int k = 0;//длина цикла в графе
            //    if (this.Deg[i] >= 2)//если эта вершина соединена хотя бы с двумя другими
            //    {
            //        color[i] = 1;//отметить вершину как корневую

            //        NewIt(ref color, ref r);//отметить вершины, смежные корневой вершине
            //        k++;

            //        //color.Show();
            //        //r.Show();
            //        //Console.WriteLine("----------i= {0}, k= {1}",i,k);

            //        while (k <= t - 1)//пока число рёбер меньше, чем должно быть в цикле
            //        {
            //            NewIt(ref color, ref r);//отметить вершины, смежные крайним вершинам
            //            k++;

            //            //color.Show();
            //            //r.Show();
            //            //Console.WriteLine("------i= {0}, k= {1}", i, k);

            //            if (!ExistC(color)) break;//если все вершины уже отмеченные, покинуть while{}
            //        }
            //        ShowCycles(color, r);
            //        for (int e = 0; e < this.p; e++) { color[e] = 0; r[e] = -1; }
            //    }

            //}
        }
        private void NewIt(ref Vectors c, ref Vectors r)
        {
            int max = (int)c[0];
            for (int i = 1; i < this.p; i++)
                if (c[i] > max) max = (int)c[i];//найти максимальный элемент в массиве

            for (int i = 0; i < this.p; i++)
                if (c[i] == max)//если найден крайний элемент
                {
                    for (int j = 0; j < this.p; j++)
                        if (this.A[i, j] == 1 && c[j] == 0)//если элемент j смежный крайнему и ещё не был в цепи
                        {
                            c[j] = max + 1;//отметить смежную вершину
                            r[j] = i;//отметить предшественника
                        }
                }
        }
        private bool ExistC(Vectors c)
        {
            for (int i = 0; i < c.n; i++)
                if (c[i] == 0) return true;
            return false;
        }
        /// <summary>
        /// Вывести все циклы, исходящие из массива цветов
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        private void ShowCycles(Vectors c, Vectors r)
        {
            int max = (int)c[0], k = 0;
            for (int i = 1; i < c.n; i++)
            {
                if (c[i] > max) max = (int)c[i];//найти значение максимума в массиве цветов
                if (c[i] == 1) k = i;//найти индекс корневого элемента
            }
            for (int i = 0; i < c.n; i++)
                if (c[i] == max && Related(i, k))//если вершина - самая крайняя и смежна с корневой (то цикл есть)
                {
                    Vectors v = new Vectors(c.n + 1);
                    v[0] = v[c.n] = k;
                    v[c.n - 1] = i;
                    for (int j = c.n - 2; j > 0; j--)
                        v[j] = r[(int)v[j + 1]];//восстановить вектор по массиву предшественников
                    v.Show();
                }

        }
        private bool Related(int i, int j) { return !(this.A[i, j] == 0); }
        /// <summary>
        /// Является ли граф двудольным
        /// </summary>
        /// <returns></returns>
        public bool IsBichromatic(out Vectors v)
        {
            int[] color = new int[this.p];
            for (int i = 0; i < this.p; ++i) color[i] = 0;
            bool Two = false;
            SqMatrix g = new SqMatrix(this.A);

            void dfs(int start)
            {
                for (int u = 0; u < this.p; ++u)
                    if (g[start, u] != 0)
                    {
                        if (color[u] == 0)
                        {
                            color[u] = 3 - color[start];
                            dfs(u);
                        }
                        else if (color[u] == color[start])
                            Two = false;
                    }
            }
            Two = true;
            for (int i = 0; i < this.p; ++i)
                if (color[i] == 0)
                {
                    color[i] = 1;
                    dfs(i);
                }
            v = new Vectors(color);
            return Two;
        }
        /// <summary>
        /// Результат теоремы Кёнига
        /// </summary>
        public void KoenigTheorem()
        {
            for (int i = 0; i < catalogCycles.Count; i++)
            {
                int k = (catalogCycles[i].Length - 1) / 2;
                if (k % 2 == 1)
                {
                    Console.WriteLine("Так как граф содержит цикл нечётной длины, он не является двудольным. Пример такого цикла: " + catalogCycles[i]);
                    return;
                }
            }
            Console.WriteLine("Так как граф не содержит циклов нечётной длины, он является двудольным");
        }

        /// <summary>
        /// Число компонент связности
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public int CompCount(out Vectors vec)
        {
            int[] used = new int[this.p];
            for (int i = 0; i < this.p; ++i) used[i] = 0;

            void dfs(int start, int s)
            {
                used[start] = s;
                for (int v = 0; v < this.p; ++v)
                    if (this.A[start, v] != 0 && used[v] == 0)
                        dfs(v, s);
            }

            int Ncomp = 0;
            for (int i = 0; i < this.p; ++i)
                if (used[i] == 0)
                    dfs(i, ++Ncomp);


            vec = new Vectors(used);
            return Ncomp;
        }

        /// <summary>
        /// Дополнение графа
        /// </summary>
        /// <param name="E"></param>
        /// <returns></returns>
        public static Graphs Additional(Graphs E)
        {
            Graphs F = new Graphs(E.p, Graphs.Type.Full);
            SqMatrix M = F.A - E.A;
            for (int i = 0; i < E.p; i++) M[i, i] = 0;
            return new Graphs(M);
        }
        /// <summary>
        /// Граф, порождённый вершинами из массива k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Graphs SubGraph(params int[] k)
        {
            return new Graphs(this.A.SubMatrix(k));
        }
        /// <summary>
        /// Граф, порождённый вершинами вектора
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Graphs SubGraph(Vectors v)
        {
            int[] k = new int[v.n];
            for (int i = 0; i < v.n; i++) k[i] = (int)v[i];
            return new Graphs(this.A.SubMatrix(k));
        }

        /// <summary>
        /// Подграф, порождённый удалением заданных вершин
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Graphs DeleteVertexes(params int[] k)
        {
            int[] s = new int[this.p - k.Length];
            int tmp1 = 0, tmp2 = 0;
            Array.Sort(k);
            for (int i = 0; i < this.p; i++)
                if (tmp1 < k.Length && k[tmp1] == i + 1) tmp1++;//!!!!!!!!!!!!!!!!!Тут сначала проверка условия на возможность обратиться к элементу массива
                else { s[tmp2] = i + 1; tmp2++; }
            return this.SubGraph(s);
        }
        /// <summary>
        /// Создать подграф, порождённый удалением заданных рёбер
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Graphs DeleteEdges(params Edge[] s)
        {
            SqMatrix M = new SqMatrix(this.A);
            for (int i = 0; i < s.Length; i++)
            {
                M[s[i].v1, s[i].v2] = 0;
                M[s[i].v2, s[i].v1] = 0;
            }
            return new Graphs(M);
        }
        /// <summary>
        /// Создать граф добавлением заданных рёбер
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Graphs IncludeEdges(params Edge[] s)
        {
            SqMatrix M = new SqMatrix(this.A);
            for (int i = 0; i < s.Length; i++)
            {
                M[s[i].v1, s[i].v2] = 1;
                M[s[i].v2, s[i].v1] = 1;
            }
            return new Graphs(M);
        }
        /// <summary>
        /// Пример остова в графе (создаётся разрушением циклов)
        /// </summary>
        /// <returns></returns>
        public Graphs GetSpanningTree()
        {
            Graphs g = new Graphs(this);
            while (g.ContainCycle())
            {
                string s = g.catalogCycles[0];
                //Edge e = new Edge(Convert.ToInt16(s[0])-1, Convert.ToInt16(s[2])-1);
                Edge e = new Edge((int)Char.GetNumericValue(s[0]) - 1, (int)Char.GetNumericValue(s[2]) - 1);
                g = g.DeleteEdges(e);

                //g.A.PrintMatrix();e.Show();//Console.WriteLine(s);
            }
            return g;
        }
        /// <summary>
        /// Является ли граф деревом
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool IsTree(Graphs g)
        {
            return (g.IsAcyclic() && (g.ComponCount == 1));
        }
        /// <summary>
        /// Код Прюфера для дерева
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Vectors Pryufer(Graphs g)
        {
            if (!Graphs.IsTree(g)) throw new Exception("Граф не является деревом!");
            Vectors v = new Vectors(g.p - 2);
            int ind = 0;
            while (ind < g.p - 2)
            {
                int k = g.p, j = k;
                for (int i = 0; i < g.p; i++)
                    if (g.Deg[i] == 1 && i < k) k = i; //выбирается лист с минимальным номером
                for (int i = 0; i < g.p; i++)
                    if (g.A[i, k] != 0) { j = i; break; }//Находится смежная ему вершина
                Edge e = new Edge(k, j);
                Console.WriteLine($"----Удаление ребра {k + 1}-{j + 1}");
                g = g.DeleteEdges(e);//ребро удаляется
                Console.WriteLine($"----Добавление в массив вершины {j + 1}");
                v[ind] = j + 1;
                ind++;
            }
            return v;
        }
        /// <summary>
        /// Распаковка кода Прюфера
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Graphs PryuferUnpacking(Vectors v)
        {
            Vectors count = new Vectors(v.n + 2);//массив вершин
            for (int i = 0; i < count.n; i++) count[i] = i;
            Edge[] edges = new Edge[v.n + 1];//массив рёбер

            //распаковка вектора
            int ind = 0;
            Console.WriteLine($"В пустой граф из {v.n + 2} вершин добавляются рёбра.");
            Console.WriteLine($"Конец ребра - первая слева вершина в коде, ещё не бывшая концом ребра (не удалённая).");
            Console.WriteLine($"Начало ребра - минимальная по номеру вершина, не упоминающаяся в коде и ещё не бывшая началом ребра (не удалённая).");
            while (ind < v.n)
            {
                int k = (v.n + 3);
                for (int i = 0; i < count.n; i++)
                    if (!v.Contain(count[i] + 1) && count[i] < k) { k = (int)count[i]; break; }
                Console.Write($"Список доступных вершин ({3 * v.n + 1} - не доступна): "); count.ShowPlusOne();
                Console.Write($"Список неиспользованных вершин в коде ({-1} - использованная): "); v.Show();

                Console.WriteLine($"----Добавление ребра {k + 1}-{(int)v[ind]}");
                edges[ind] = new Edge(k, (int)v[ind] - 1);
                count[k] = 3 * v.n;
                v[ind] = -1;
                ind++;

                //count.ShowPlusOne();
                //v.Show();
                //Console.WriteLine(k);
            }

            //добавление последнего ребра
            int t = 0;
            Vectors m = new Vectors(2);
            for (int i = 0; i < count.n; i++)
                if (count[i] != 3 * v.n) { m[t] = count[i]; t++; }
            //m.ShowPlusOne();
            Console.Write($"Список доступных вершин ({3 * v.n + 1} - не доступна): "); count.ShowPlusOne();
            Console.WriteLine($"----Добавление последнего ребра {(int)m[0] + 1}-{(int)m[1] + 1} (ребро между двумя доступными вершинами)");
            edges[ind] = new Edge((int)m[0], (int)m[1]);

            return new Graphs(count.n, edges);
        }
        /// <summary>
        /// Сожержится ли в графе заданное ребро
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ContainThisEdge(Edge e)
        {
            return (this.A[e.v1, e.v2] != 0);
        }
        /// <summary>
        /// Выдать номер ребра в каталоге рёбер
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private int GetNumderOfEdge(Edge e)
        {
            int k = 0;
            while (this.Ed[k] != e) k++;
            return k;
        }

        /// <summary>
        /// Матрица фундаментальных циклов относительно выбранного остова
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public Matrix FundamentalCycles(Graphs g)
        {
            if (!Graphs.IsTree(g)) throw new Exception("Аргумент не является остовом!!!");

            Matrix R = new Matrix(this.CyclomaticN, this.Edges);//объявление матрицы
            int k = 0;
            int[,] p = new int[2, this.CyclomaticN];//перестановка для замены столбцов в конце

            for (int i = 0; i < this.Edges; i++)//проход по всем рёбрам
                if (!g.ContainThisEdge(this.Ed[i]))//если остов не содержит это ребро
                {
                    p[0, k] = i + 1;
                    Console.WriteLine($"-Добавить в остов ребро с концевыми вершинами {this.Ed[i].v1 + 1} и {this.Ed[i].v2 + 1}");
                    Graphs G = g.IncludeEdges(this.Ed[i]);//добавить в него ребро
                    string s = G.catalogCycles[0];
                    Console.WriteLine($"---Добавить в цикл {k + 1} ");
                    for (int j = 0; j < s.Length - 2; j += 2)
                    {
                        int u = (int)Char.GetNumericValue(s[j]), v = (int)Char.GetNumericValue(s[j + 2]);//зафиксировать концы рёбер
                        Edge e = new Edge(u - 1, v - 1);//создать ребро
                        R[k, this.GetNumderOfEdge(e)] = 1;//в строке этго цикла поставить 1 в столбце ребра, входящего в циклы
                                                          //if (p[1, k] == 0 && e!= this.Ed[i]) p[1, k] = this.GetNumderOfEdge(e) + 1;
                        Console.WriteLine($"-------Добавлено ребро с концевыми вершинами {u} и {v} (место {k + 1} {this.GetNumderOfEdge(e) + 1})");
                    }
                    k++;//перейти к следующему циклу
                }
            Console.WriteLine("Полученная матрица:"); R.PrintMatrix();

            Console.WriteLine("Если последовательно переставить местами столбцы:");
            //for(int i=0;i<this.CyclomaticN;i++)
            //{
            //for (int t = 0; t < this.Edges; t++)//проход по всем рёбрам
            //    if (g.ContainThisEdge(this.Ed[t]))
            //    {
            //        p[1, i] = t + 1;
            //        break;
            //    }
            int w = 0;
            for (int t = 0; t < this.CyclomaticN; t++)
                if (t + 1 == p[0, t]) { p[1, t] = t + 1; w++; }
                else
                {
                    p[1, t] = w + 1;
                    Console.WriteLine(p[0, t] + " c " + p[1, t]);
                    R.ColumnSwap(p[0, t] - 1, p[1, t] - 1);
                    p[0, t] = w + 1;
                    w++;
                }
            //}
            //Console.WriteLine();
            Console.WriteLine("Получим матрицу в ^красивом^ виде");

            return R;
        }
        /// <summary>
        /// Матрица фундаментальных разрезов
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        private Matrix BasisSection(Matrix M)
        {
            Matrix R = new Matrix(this.Edges - this.CyclomaticN, this.Edges);

            for (int i = 0; i < R.n; i++)
                for (int j = 0; j < this.CyclomaticN; j++)
                    R[i, j] = M[j, i + this.CyclomaticN];
            for (int i = 0; i < R.n; i++) R[i, i + this.CyclomaticN] = 1;

            return R;
        }
        /// <summary>
        /// Содержит ли цепь/цикл ребро
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool ContainEdge(string s, Edge e)
        {
            string s1 = (e.v1 + 1).ToString() + "-" + (e.v2 + 1).ToString();
            string s2 = (e.v2 + 1).ToString() + "-" + (e.v1 + 1).ToString();
            int ind1 = s.IndexOf(s1), ind2 = s.IndexOf(s2);
            if (ind1 > -1 || ind2 > -1) return true;
            return false;
        }
        /// <summary>
        /// Содержит ли цепь/цикл вершину
        /// </summary>
        /// <param name="s"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static bool ContainVer(string s, int k)
        {
            int ind1 = s.IndexOf((k + 1).ToString());
            if (ind1 > -1) return true;
            return false;
        }
        /// <summary>
        /// Содержит ли цепь/цикл ребро только единожды
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool ContainEdgeUnique(string s, Edge e)
        {
            bool result = false;
            string s1 = (e.v1 + 1).ToString() + "-" + (e.v2 + 1).ToString();
            string s2 = (e.v2 + 1).ToString() + "-" + (e.v1 + 1).ToString();
            int ind1 = s.IndexOf(s1), ind2 = s.IndexOf(s2);//Console.WriteLine(s1 + " " + s2+" "+ind1.ToString()+" "+ind2.ToString());
            if (ind1 > -1 || ind2 > -1)//если ребро есть
                if (ind1 == -1 || ind2 == -1)//если ребро пока одно
                {
                    int ind11 = s.IndexOf(s1, Math.Max(ind1, ind2) + 1);
                    int ind22 = s.IndexOf(s2, Math.Max(ind1, ind2) + 1);
                    //Console.WriteLine(s1 + " " + s2+" "+ind11.ToString()+" "+ind22.ToString());
                    if (ind11 < 0 && ind22 < 0) result = true;//если дальше этого ребра нет
                }

            return result;
        }
        /// <summary>
        /// Содержит ли цепь/цикл вершину только единожды
        /// </summary>
        /// <param name="s"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static bool ContainVerUnique(string s, int k)
        {
            bool result = false;
            int ind1 = s.IndexOf((k + 1).ToString());
            if (ind1 > -1)//если вершина есть
            {
                int ind11 = s.IndexOf((k + 1).ToString(), ind1 + 1);
                if (ind11 == -1)//если вершина встретилась только один раз
                    result = true;
            }

            return result;
        }
        /// <summary>
        /// Объединение мостов-блоков графа (исходный граф без мостов)
        /// </summary>
        /// <returns></returns>
        public Graphs BridgeBlocks()
        {
            Graphs g = new Graphs(this);
            Edge[] e = new Edge[this.Bridges.Count];
            for (int i = 0; i < e.Length; i++) e[i] = new Edge(this.Bridges[i]);
            g = g.DeleteEdges(e);
            return g;
        }
        /// <summary>
        /// Граф блоков-точек сочленения (максимальный подграф без точек сочленения)
        /// </summary>
        /// <returns></returns>
        public Graphs JointBlock()
        {
            if (this.JointVect.n == 0) return this;
            Graphs g = new Graphs(this);
            Vectors v = new Vectors(g.JointVect);
            //for (int i = 0; i < v.n; i++) v[i] = (int)g.JointVect[i];
            for (int i = 0; i < g.p; i++)
                for (int j = 0; j < v.n; j++)
                    g = g.DeleteEdges(new Edge((int)v[j] - 1, i));//удалить рёбра из точек сочленения
                                                                  //g = g.DeleteVertexes(v);
            Vectors s;
            int tmp = g.CompCount(out s);//узнать распределение по компонентам связности
                                         //v.Show();
                                         //s.Show();
            int max = 0;
            double maxval = 0;
            //найти наибольшую компоненту
            for (int i = 0; i < s.n - 1; i++)
            {
                int k = 1;
                for (int j = i + 1; j < s.n; j++)
                    if (s[j] == s[i]) k++;
                if (k > max)
                {
                    max = k;
                    maxval = s[i];
                }
            }
            //Console.WriteLine(max + " " + maxval);
            //найти вершины, которые не входят в наибольшую компоненту и не являются точками сочленения
            int[] r = new int[this.p - v.n - max];
            int t = 0;
            for (int i = 0; i < g.p; i++)
                if (s[i] != maxval && !v.Contain(i + 1))
                {
                    r[t] = i + 1;
                    //Console.WriteLine((r[t]+1) + " ");
                    t++;
                }

            Graphs w = this.DeleteVertexes(r);
            if (w.JointVect.n == 0) return w;//если теперь нет точек сочленения, вернуть ответ
            return w.JointBlock();//иначе рекурсия
        }


        /// <summary>
        /// Исследование графа на планарность
        /// </summary>
        public void AboutPlanarity()
        {
            if (this.p > 3)
            {
                Console.WriteLine("Пусть f - число граней графа. Тогда имеет место эйлерова характеристика для плоскости:");
                Console.WriteLine("\tДля связного планарного графа справедливо: p-q+f=2");
                Console.WriteLine("Отсюда получается необходимых признак планарности:");
                Console.WriteLine("\tДля связного планарного графа с р>3: q<=3p-6.");
                if (this.Edges > 3 * this.p - 6)
                    Console.WriteLine("Так как для этого графа q>3p-6 ({0}>{1}), то граф не планарный.", this.Edges, 3 * this.p - 6);
                else
                {
                    Console.WriteLine("Так как для этого графа q<=3p-6 ({0}<={1}), граф может быть планарным.", this.Edges, 3 * this.p - 6);
                    Console.WriteLine("И его потенциальное число граней f=2-p+q=2-{0}+{1}={2}", this.p, this.Edges, 2 - this.p + this.Edges);
                }

                Console.WriteLine("По теореме Понтрягина-Куратовского: ");
                if (this.p >= 5)
                {
                    for (int a = 0; a < this.p; a++)
                        for (int b = a + 1; b < this.p; b++)
                            for (int c = b + 1; c < this.p; c++)
                                for (int d = c + 1; d < this.p; d++)
                                    for (int e = d + 1; e < this.p; e++)
                                    {
                                        Graphs g = this.SubGraph(a + 1, b + 1, c + 1, d + 1, e + 1);
                                        if (Graphs.Isomorphism(g, Graphs.K5))
                                        {
                                            Console.WriteLine("\t Так как исходный граф содержит подграф, изоморвный К5 (на вершинах {0}, {1}, {2}, {3}, {4})", a + 1, b + 1, c + 1, d + 1, e + 1);
                                            Console.WriteLine("\t Исходный граф не планарен.");
                                            return;
                                        }
                                        if (this.p >= 6)
                                            for (int f = e + 1; f < this.p; f++)
                                            {
                                                Graphs y = this.SubGraph(a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
                                                if (Graphs.Isomorphism(y, Graphs.K3_3))
                                                {
                                                    Console.WriteLine("\t Так как исходный граф содержит подграф, изоморвный К(3,3) (на вершинах {0}, {1}, {2}, {3}, {4}, {5})", a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
                                                    Console.WriteLine("\t Исходный граф не планарен.");
                                                    return;
                                                }
                                            }
                                    }
                    Console.WriteLine("\t Так как граф не содержит подграфов, изоморфных K5, K(3,3), он планарен.");
                }
                else
                    Console.WriteLine("\t Так как в графе меньше 5 вершин, он не содержит подграфов, изоморфных K5, K(3,3), поэтому планарен.");
            }

        }

        /// <summary>
        /// Сгенерировать все эйлеровы циклы и цепи
        /// </summary>
        private void FillEuler()
        {
            //ML.Test(this);

            List<ML> tmp = new List<ML>();
            tmp.Add(new ML(this.B));//добавить в промежуточное множество матрицу инцидентности
            tmp[0].L.Add(0);//добавить первую вершину

            int t = 0;
            while (tmp.Count > 0)//пока в этом множестве есть элементы
            {
                ML R = new ML(tmp[0]);//взять первый
                t++;
                //if (t % 1000 == 0) { R.M.PrintMatrix(); Console.WriteLine(tmp.Count); }
                //R.Show();

                for (int j = 0; j < R.M.m; j++)//для каждого ребра
                {
                    if (R.HasEdge(j))
                    {
                        ML Rj = new ML(R.Step(j));//удалить ребро и поместить в цикл
                        if (Rj.M.m == 0)//если матрица не имеет столбцов, сгенерировать цепь
                        {
                            string s = "";
                            for (int i = 0; i < Rj.L.Count - 1; i++) s += String.Format("{0}-", Rj.L[i] + 1);
                            s += String.Format("{0}", Rj.L[Rj.L.Count - 1] + 1);
                            double a = Char.GetNumericValue(s[0]), b = Char.GetNumericValue(s[s.Length - 1]);
                            if (a == b) this.EulerCycles.Add(s);
                            else this.EulerChains.Add(s);
                        }
                        else
                        {
                            int k = 0;
                            for (int i = 0; i < Rj.M.m; i++)
                                if (!Rj.HasEdge(i)) k++;//если нельзя идти дальше, запомнить
                            if (k != Rj.M.m)//если с матрицей можно дальше работать, занести её в список
                                tmp.Add(Rj);
                        }
                    }
                }
                tmp.RemoveAt(0);
            }
        }

        /// <summary>
        /// Проверка графа на наличие эйлерова цикла/цепи с выводом
        /// </summary>
        public void IsEuler()
        {
            List<string> Z = new List<string>();
            for (int i = 0; i < this.catalogCycles.Count; i++)
            {
                int k = 0;
                string s = this.catalogCycles[i];
                for (int j = 0; j < this.Ed.Count; j++)
                    if (Graphs.ContainEdgeUnique(s, this.Ed[j]))
                        k++;//если ребро входит в цикл единожды
                if (k == this.Edges) Z.Add(this.catalogCycles[i]);//если каждое ребро входит в цикл единожды
            }

            if (Z.Count > 0)
            {
                Console.WriteLine("Граф является эйлеровым. Его эйлеровы (вдобавок простые) циклы:");
                for (int i = 0; i < Z.Count; i++) Console.WriteLine(Z[i]);
            }
            else
            {

                Vectors v = new Vectors(this.Deg);
                int sum = 0, ind = 0;
                for (int i = 0; i < v.n; i++) { sum += (int)v[i] % 2; ind++; }
                if (sum == 0)
                {
                    this.EulerChains = new List<string>();
                    this.EulerCycles = new List<string>();
                    FillEuler();

                    Console.WriteLine("Граф является эйлеровым. Его эйлеровы циклы:");
                    for (int i = 0; i < EulerCycles.Count; i++) Console.WriteLine(EulerCycles[i]);
                }
                else
                {
                    Console.WriteLine("Граф не является эйлеровым, так как содержит вершину нечётной степени.");

                    if (ind > 2) Console.WriteLine("...и не содержит эйлеровы цепи (не является полуэйлеровым, поскольку имеет более двух ({0}) вершин с нечётными степенями).", 1);
                    //List<string> C = new List<string>();
                    //for (int i = 0; i < this.Chains.Count; i++)
                    //{
                    //    int k = 0;
                    //    string s = this.Chains[i];
                    //    for (int j = 0; j < this.Ed.Count; j++)
                    //        if (Graphs.ContainEdgeUnique(s, this.Ed[j])) k++;//если ребро входит в цепь единожды
                    //    if (k == this.Edges) C.Add(this.Chains[i]);//если каждое ребро входит в цикл единожды
                    //}
                    else
                    {
                        Console.WriteLine("...но содержит эйлеровы цепи (является полуэйлеровым):");
                        for (int i = 0; i < EulerChains.Count; i++) Console.WriteLine(EulerChains[i]);
                    }

                }
            }
        }
        /// <summary>
        /// Проверка графа на наличие гамильтонова цикла/цепи с выводом
        /// </summary>
        public void IsHamilton()
        {
            List<string> Z = new List<string>();
            for (int i = 0; i < this.catalogCycles.Count; i++)
            {
                int k = 0;
                string s = this.catalogCycles[i].Substring(1);
                for (int j = 0; j < this.p; j++)
                    if (Graphs.ContainVerUnique(s, j))
                        k++;//если вершина входит в цикл единожды
                if (k == this.p) Z.Add(this.catalogCycles[i]);//если каждая вершина входит в цикл единожды
            }

            if (Z.Count > 0)
            {
                Console.WriteLine("Граф является гамильтоновым. Его гамильтоновы циклы:");
                for (int i = 0; i < Z.Count; i++) Console.WriteLine(Z[i]);
            }
            else
            {
                Console.WriteLine("Граф не является гамильтоновым.");

                List<string> C = new List<string>();
                for (int i = 0; i < this.Chains.Count; i++)
                {
                    int k = 0;
                    string s = this.Chains[i];
                    for (int j = 0; j < this.p; j++)
                        if (Graphs.ContainVerUnique(s, j)) k++;//если вершина входит в цепь единожды
                    if (k == this.p) C.Add(this.Chains[i]);//если каждая вершина входит в цепь единожды
                }
                if (C.Count > 0)
                {
                    Console.WriteLine("...но содержит гамильтоновы простые цепи:");
                    for (int i = 0; i < C.Count; i++) Console.WriteLine(C[i]);
                }
                else
                    Console.WriteLine("...и не содержит гамильтоновы простые цепи.");
            }
        }

        private bool IsNotRelated(List<int> l, int p)
        {
            for (int i = 0; i < l.Count; i++)
                if (this.A[l[i], p] != 0) return false;
            return true;
        }
        private bool IsNotRelated(Vectors l, int p)
        {
            for (int i = 0; i < l.n; i++)
                if (this.A[(int)l[i] - 1, p] != 0) return false;
            return true;
        }

        /// <summary>
        /// Число независимости графа
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int IndependenceNumber(out Vectors v)
        {
            int k = 0;
            v = new Vectors();
            List<int> un = new List<int>(0);//список вершин этого множества
            for (int c = 0; c < this.p; c++)
            {
                un = new List<int>(0);//список вершин этого множества
                un.Add(c);
                for (int i = 0; i < this.p; i++)
                    if (i != c && this.IsNotRelated(un, i))//если вершина не смежная вершинам данного множества
                        un.Add(i);
                if (un.Count > k)
                {
                    k = un.Count;
                    v = new Vectors(un.Count);
                    for (int i = 0; i < v.n; i++) v[i] = un[i] + 1;
                }
            }
            return v.n;
        }
        /// <summary>
        /// Вывести все независимые подмножетсва вершин
        /// </summary>
        public void ShowIndepSubSets()
        {
            for (int i = 0; i < this.IndepSubsets.Count; i++) IndepSubsets[i].Show();
        }
        /// <summary>
        /// Вывести все наибольшие независимые подмножества
        /// </summary>
        public void ShowGreatestIndepSubSets()
        {
            for (int i = 0; i < this.IndepSubsets.Count; i++)
            {
                bool tmp = true;
                for (int j = 0; j < this.p; j++)
                    if (IndepSubsets[i].Contain(j + 1) || !this.IsNotRelated(IndepSubsets[i], j)) ;
                    else { tmp = false; break; }//если вершина не входит в подмножество и не смежна ни одной вершине из подмножества
                if (tmp)
                {
                    IndepSubsets[i].Show();
                    GreatestIndepSubsets.Add(IndepSubsets[i]);
                }
            }
        }

        /// <summary>
        /// Выдать вектор раскраски графа
        /// </summary>
        /// <returns></returns>
        public Vectors GetColouring()
        {
            int k = 0;//число цветов
            Vectors color = new Vectors(this.p);//результат
                                                //List<int> uncol = new List<int>(this.p);//список нераскрашенных вершин
                                                //for (int i = 0; i < uncol.Count; i++) uncol[i] = i;

            while (color.Contain(0))//пока есть нераскрашенные вершины
            {
                k++;
                int c = 0;//номер нераскашенной вершины
                while (color[c] > 0) c++;
                color[c] = k;
                //List<int> unsm = new List<int>(0);//списко не смежных и не раскрашенных
                List<int> un = new List<int>(0);//список вершин этого цвета
                un.Add(c);
                for (int i = 0; i < this.p; i++)
                    if (color[i] == 0 && this.IsNotRelated(un, i))//если вершина не смежная вершинам данного цвета и не раскрашенная
                    {
                        color[i] = k;
                        un.Add(i);
                    }
                //unsm.Add(i);//сгенерировать список
                //for(int i=unsm.Count-1;i>=0;i--)
                //{
                //    color[unsm[i]] = k;
                //}
            }

            return color;
        }
        /// <summary>
        /// Отсортировать вершины по невозврастанию их валентностей
        /// </summary>
        /// <param name="l"></param>
        private void SortDeg(ref List<int> l)
        {
            Vectors v = new Vectors(this.Deg);
            for (int i = 0; i < l.Count; i++)
                for (int j = 0; j < l.Count - i - 1; j++)
                    if (v[l[j]] < v[l[j + 1]])//один шаг степени
                    {
                        int tmp = l[j];
                        l[j] = l[j + 1];
                        l[j + 1] = tmp;
                    }
                    else if (v[l[j]] == v[l[j + 1]])//второй шаг степени
                    {
                        double v1 = 0, v2 = 0;
                        for (int k = 0; k < l.Count; k++)
                        {
                            if (this.A[l[j], l[k]] != 0) v1 += v[l[k]];
                            if (this.A[l[j + 1], l[k]] != 0) v2 += v[l[k]];
                        }
                        if (v1 < v2)
                        {
                            int tmp = l[j];
                            l[j] = l[j + 1];
                            l[j + 1] = tmp;
                        }
                    }
        }
        /// <summary>
        /// Выдать вектор (предположительно) минимальной раскраски графа (алгоритм с двухшаговыми степенями)
        /// </summary>
        /// <returns></returns>
        public Vectors GetModifColouring()
        {
            int k = 0;//число цветов
            Vectors color = new Vectors(this.p);//результат
            List<int> uncol = new List<int>();//список нераскрашенных вершин
            for (int i = 0; i < this.p; i++) uncol.Add(i);
            //for (int i = 0; i < uncol.Count; i++)
            //    Console.WriteLine(uncol[i] + " " + this.Deg[uncol[i]]);
            //Console.WriteLine();
            SortDeg(ref uncol);
            //for (int i = 0; i < uncol.Count; i++)
            //    Console.WriteLine(uncol[i] + " " + this.Deg[uncol[i]]);


            while (/*uncol.Count > 0*/color.Contain(0))//пока есть нераскрашенные вершины
            {
                k++;
                int c = uncol[0];//номер нераскашенной вершины
                color[c] = k;
                List<int> un = new List<int>(0);//список вершин этого цвета
                un.Add(c);
                uncol.RemoveAt(0);
                for (int i = 0; i < uncol.Count; i++)
                    if (color[uncol[i]] == 0 && this.IsNotRelated(un, uncol[i]))//если вершина не смежная вершинам данного цвета и не раскрашенная
                    {
                        color[uncol[i]] = k;
                        un.Add(uncol[i]);
                        uncol.RemoveAt(i);
                        i--;
                    }
                SortDeg(ref uncol);
            }

            return color;
        }

        /// <summary>
        /// Дать оценки хроматическому числу
        /// </summary>
        public void EstimateChromaticNumder()
        {
            Console.WriteLine("Обозначим хроматическое число за X. Тогда:");
            double min = (double)this.p * this.p / (this.p * this.p - 2 * this.Edges);
            Console.WriteLine("\t1.1) Х имеет нижнюю оценку: p*p/(p*p-2q), т. е. X >= {0}/({0}-{1}) = {2}", this.p * this.p, 2 * this.Edges, min);

            Console.WriteLine("\t1.2) Х всегда => 1");
            min = Math.Max(1, min);

            Vectors e;
            int b = this.IndependenceNumber(out e);
            Console.WriteLine("\t1.3) Х имеет нижнюю оценку (через число независимости): p/b, т. е. X >= {0}/{1} = {2}", this.p, b, (double)this.p / b);
            min = Math.Max(b, min);

            if (!this.A.Nulle())
            {
                Console.WriteLine("\t1.4) Так как граф не пустой (значит, в нём есть смежные вершины), то Х => 2");
                min = Math.Max(2, min);
            }

            for (int i = 0; i < this.catalogCycles.Count; i++)
                if (((this.catalogCycles[i].Length - 1) / 2) % 2 == 1)
                {
                    Console.WriteLine("\t1.5) Так как граф имеет цикл нечётной длины, то Х > 2");
                    min = Math.Max(3, min);
                    break;
                }


            double max = this.p;
            Console.WriteLine("\t2.1) Х не превышает числа вершин, т. е. X <= " + max);
            Console.WriteLine("\t2.2) Х имеет верхнюю оценку (по числу независимости): p-b+1, т. е. X <= " + (this.p - b + 1));
            max = Math.Min(this.p - b + 1, max);

            Vectors v = this.Deg;
            if (!Graphs.IsFull(this) && this.p >= 3)
            {
                Console.WriteLine("\t2.3) Раз граф не полный и p>=3 ({0}>=3), то по теореме Брукса Х не превышает наибольшую валентность, т. е. X <= {1}", this.p, v.Max);
                max = Math.Min(max, v.Max);
            }
            else
            {
                Console.WriteLine("\t2.3) Х не превышает наибольшую валентность в графе более чем на 1, т. е. X <= {0} + 1 = {1}", v.Max, v.Max + 1);
                max = Math.Min(max, v.Max + 1);
            }

            Graphs ее = new Graphs(this.BridgeBlocks());
            v = ее.GetModifColouring();
            Console.Write("\t Раскрасим граф рёберных блоков (можно брать и вершинные блоки): "); v.Show();
            Console.WriteLine("\t2.4) Так как граф рёберных блоков {0}-раскрашиваем, то исходный граф {0}-раскрашиваем и X<={0}", v.Max);
            max = Math.Min(max, v.Max);

            if (this.ComponCount == 1 && ((this.p - 1) <= this.Edges) && (this.Edges <= ((double)this.p / 2 * (this.p - 1))))
            {
                Console.WriteLine("\t Так как граф связен и в нём выполнено условие: (p-1)<=q<=p(p-1)/2 ({0}<={1}<={2}),", this.p - 1, this.Edges, (double)this.p / 2 * (this.p - 1));
                double t = (3 + Math.Sqrt(9 + 8 * (this.Edges - this.p))) / 2;
                Console.WriteLine("\t2.5) Верхняя оценка для Х: (3+(9+8(q-p))^0.5)/2, т. е. X <= {0}", t);
                max = Math.Min(max, t);
            }

            v = new Vectors((int)(max - min) + 1);
            for (int i = 0; i < v.n; i++) v[i] = (int)(min + i);
            Console.Write("\t3.1) Тогда возможные значения Х: "); v.Show();

            if (v.n == 1) Console.WriteLine("\t3.2) Значит, хроматическое число равно " + v[0]);
            else
            {
                Vectors m = this.GetModifColouring();
                Console.WriteLine("\t3.2) Предположительно (поскольку не существует универсального алгоритма минимальной раскраски: ), хроматическое число равно " + m.Max);
            }
        }

        internal static Polynom Degree(int n)
        {
            Polynom p = new Polynom((int)n);
            p.coef[n] = 1;
            return p;
        }
        private static Polynom FactorialDeg(int n)
        {
            Vectors v = new Vectors(n);
            for (int i = 0; i < n; i++) v[i] = i;
            return new Polynom(1, v);
        }
        private void GetRel(out int i, out int j)
        {
            i = 0; j = 0;
            //Vectors v = new Vectors(this.Deg);
            //while (v[i] != v.Max) { i++;j++; }

            for (int n = 0; n < this.p; n++)
                for (int m = 0; m < this.p; m++)
                    if (this.A[n, m] != 0 /*&&v[m]<v[j]Math.Min(v[m],v[n])<Math.Min(v[i],v[j])*/)
                    {
                        i = n;
                        j = m;
                        return;
                    }
        }
        private Graphs Delete(int i, int j)
        {
            Edge e = new Edge(i, j);
            return new Graphs(this.DeleteEdges(e));
        }
        private Graphs PullTogether(int i, int j)
        {
            Graphs g = new Graphs(this);
            List<Edge> v = new List<Edge>(0);
            for (int k = 0; k < this.p; k++)
                if (k != i && g.A[k, j] != 0)
                    v.Add(new Edge(i, k));//к i добавляются рёбра от смежных с j
            Edge[] e = new Edge[v.Count];
            for (int k = 0; k < e.Length; k++) e[k] = v[k];
            g = g.IncludeEdges(e);
            g = g.DeleteVertexes(j + 1);//j удаляется
                                        //g.A.PrintMatrix();
            return g;
        }
        /// <summary>
        /// Является ли граф пустым
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool IsNulle(Graphs g)
        {
            return g.A.Nulle();
        }

        /// <summary>
        /// Хроматический полином графа
        /// </summary>
        /// <returns></returns>
        public Polynom Xpolymon()
        {
            if (Graphs.IsFull(this)) return Graphs.FactorialDeg(this.p);
            if (Graphs.IsNulle(this)) return Graphs.Degree(this.p);

            Polynom pol1 = new Polynom(this.p);
            Polynom pol2 = new Polynom(this.p);
            List<Graphs> Positive = new List<Graphs>();//графы, которые дадут сумму
            List<Graphs> Negative = new List<Graphs>();//графы, которые дадут разность
                                                       //разбить исходный граф на разность
            int a, b;
            this.GetRel(out a, out b);
            Positive.Add(this.Delete(a, b));
            Negative.Add(this.PullTogether(a, b));
            //Positive[0].A.PrintMatrix();
            //Negative[0].A.PrintMatrix();
            //Graphs.FactorialDeg(3).Show();

            //выполнять разбиение и заполнение полинома
            while (Positive.Count > 0 || Negative.Count > 0)
            {
                //Console.WriteLine(Positive.Count + " " + Negative.Count);
                for (int i = 0; i < Positive.Count; i++)
                {
                    if (Graphs.IsFull(Positive[i]))
                    { pol1 += Graphs.FactorialDeg(Positive[i].p); PolCount++; }
                    else if (Graphs.IsNulle(Positive[i]))
                    { pol1 += Graphs.Degree(Positive[i].p); PolCount++; }
                    else
                    {
                        Positive[i].GetRel(out a, out b);
                        Positive.Add(Positive[i].Delete(a, b));
                        Negative.Add(Positive[i].PullTogether(a, b));
                    }
                    Positive.RemoveAt(i); i--;
                }
                //Console.WriteLine(Positive.Count + " " + Negative.Count);
                for (int i = 0; i < Negative.Count; i++)
                {
                    if (Graphs.IsFull(Negative[i]))
                    { pol2 += Graphs.FactorialDeg(Negative[i].p); PolCount++; }
                    else if (Graphs.IsNulle(Negative[i]))
                    { pol2 += Graphs.Degree(Negative[i].p); PolCount++; }
                    else
                    {
                        Negative[i].GetRel(out a, out b);
                        Negative.Add(Negative[i].Delete(a, b));
                        Positive.Add(Negative[i].PullTogether(a, b));
                    }
                    Negative.RemoveAt(i); i--;
                }
            }
            //pol1.Show();
            //pol2.Show();

            return pol1 - pol2;
        }
        private int PolCount = 0;

        private bool IsDomination(List<int> L)
        {
            Vectors v = new Vectors(L.Count);
            for (int i = 0; i < v.n; i++) v[i] = L[i];
            //проверка для всех вершин
            for (int i = 0; i < this.p; i++)
                if (!v.Contain(i))//если вершина не входит в множество
                {
                    bool ret = false;
                    for (int j = 0; j < v.n; j++)
                        if (this.A[i, (int)v[j]] != 0)
                        {
                            ret = true;//вершина смежна какой-то вершине из множества
                            break;//можно прекратить проверку для этой вершины
                        }
                    if (!ret) return false;//если какая-то вершина не смежна всем вершинам множества, это не доминирующее множество
                }
            return true;
        }
        /// <summary>
        /// Показать доминирующие подмножества вершин с шагом step
        /// </summary>
        /// <param name="step"></param>
        public void ShowDominSub(int step)
        {
            for (int i = 0; i < this.DominSubsets.Count; i += step)
                DominSubsets[i].Show();
        }
        /// <summary>
        /// Показать минимальные доминирующие подмножества вершин
        /// </summary>
        public void ShowMinDominSub()
        {
            for (int i = 0; i < this.MinimalDominSubsets.Count; i++)
                MinimalDominSubsets[i].Show();
        }
        /// <summary>
        /// Показать наименьшие доминирующие подмножества
        /// </summary>
        public void ShowSmallestDominSub()
        {
            int k = this.MinimalDominSubsets[0].n;
            for (int i = 1; i < MinimalDominSubsets.Count; i++)
                if (k > MinimalDominSubsets[i].n)
                    k = MinimalDominSubsets[i].n;
            this.DominationNumber = k;
            for (int i = 0; i < this.MinimalDominSubsets.Count; i++)
                if (MinimalDominSubsets[i].n == k)
                    MinimalDominSubsets[i].Show();
        }

        /// <summary>
        /// Показать список векторов
        /// </summary>
        /// <param name="L"></param>
        public static void ShowVectorsList(List<Vectors> L)
        {
            for (int i = 0; i < L.Count; i++)
                L[i].Show();
        }
        /// <summary>
        /// Показать список векторов с шагом по списку
        /// </summary>
        /// <param name="L"></param>
        /// <param name="k"></param>
        public static void ShowVectorsList(List<Vectors> L, int k)
        {
            for (int i = 0; i < L.Count; i += k)
                L[i].Show();
        }
        /// <summary>
        /// Показать список рёбер с шагом по списку
        /// </summary>
        /// <param name="L"></param>
        /// <param name="k"></param>
        public static void ShowEdgeListofL(List<List<Edge>> L, int k = 1)
        {
            for (int i = 0; i < L.Count; i += k)
            {
                for (int j = 0; j < L[i].Count; j++) Console.Write(L[i][j].ToString() + " ");
                Console.WriteLine();
            }

        }


        /// <summary>
        /// Показать наименьшие вершинные покрытия
        /// </summary>
        public void ShowSmallestVCoatingSub()
        {
            int k = this.MinimalVCoatingSubsets[0].n;
            for (int i = 1; i < MinimalVCoatingSubsets.Count; i++)
                if (k > MinimalVCoatingSubsets[i].n)
                    k = MinimalVCoatingSubsets[i].n;
            this.VCoatingNumber = k;
            for (int i = 0; i < this.MinimalVCoatingSubsets.Count; i++)
                if (MinimalVCoatingSubsets[i].n == k)
                    MinimalVCoatingSubsets[i].Show();
        }
        /// <summary>
        /// Показать наименьшие рёберные покрытия
        /// </summary>
        public void ShowSmallestECoatingSub()
        {
            if (this.MinimalECoatingSubsets != null && this.MinimalECoatingSubsets.Count != 0)
            {
                int k = this.MinimalECoatingSubsets[0].Count;
                for (int i = 1; i < MinimalECoatingSubsets.Count; i++)
                    if (k > MinimalECoatingSubsets[i].Count)
                        k = MinimalECoatingSubsets[i].Count;
                this.ECoatingNumber = k;
                for (int i = 0; i < this.MinimalECoatingSubsets.Count; i++)
                    if (MinimalECoatingSubsets[i].Count == k)
                    {
                        for (int j = 0; j < MinimalECoatingSubsets[i].Count; j++) Console.Write(MinimalECoatingSubsets[i][j].ToString() + " ");
                        Console.WriteLine();
                    }
            }
        }

        /// <summary>
        /// Являются ли ребро и вершина инцидентными друг другу
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool IsIncidental(Edge e, int k)
        {
            return (e.v1 == k || e.v2 == k);
        }

        /// <summary>
        /// Образует ли множество вершинное покрытие
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool IsVertexCoating(List<int> v)
        {
            for (int i = 0; i < this.Ed.Count; i++)//проход по всем рёбрам
            {
                int k = 0;
                for (int j = 0; j < v.Count; j++)//по всем вершинам в множестве
                    if (!this.IsIncidental(this.Ed[i], v[j]))/*(this.Ed[i].v1 != v[j] && this.Ed[i].v2 != v[j])*/ k++;//если ребро не инцидентно этой вершине, запомнить
                if (k == v.Count) return false;//если ребро не инцидентно всем вершинам, то множество не образует вершинное покрытие
            }
            return true;
        }
        /// <summary>
        /// Образует ли множество рёберное покрытие
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        private bool IsEdgeCoating(List<Edge> L)
        {
            for (int i = 0; i < this.p; i++)//проход по всем вершинам
            {
                int k = 0;
                for (int j = 0; j < L.Count; j++)//по всем рёбрам в множестве
                    if (!this.IsIncidental(L[j], i))/*(this.Ed[i].v1 != v[j] && this.Ed[i].v2 != v[j])*/ k++;//если вершина не инцидентна этому ребру, запомнить
                if (k == L.Count) return false;//если вершина не инцидентна ни одному из рёбер, то множество не образует рёберное покрытие
            }
            return true;
        }
        private void Lemma()
        {
            Console.WriteLine("Вершины с нечётными степенями:");
            int k = 0;
            for (int i = 0; i < this.p; i++)
                if (this.Deg[i] % 2 == 1)
                {
                    k++;
                    Console.WriteLine("Вершина {0} (степень {1});", i + 1, this.Deg[i]);
                }
            Console.WriteLine("Всего вершин с нечётными степенями {0}, то есть чётное число", k);
        }
        private void EulerTeo()
        {
            Console.WriteLine("Теорема Эйлера: сумма степеней вершин равна удвоенному числу рёбер. Действительно:");
            Vectors v = this.Deg;
            string s = v[0].ToString();
            double sum = v[0];
            if (v.n > 1)
                for (int i = 1; i < v.n; i++)
                {
                    s += String.Format(" + {0}", v[i]);
                    sum += v[i];
                }
            Console.WriteLine(s + " = " + sum + " = 2q = 2*" + this.Edges + " = " + 2 * this.Edges);
        }

        public void ShowCheck0()
        {
            Console.WriteLine("ДЕМОНСТРАЦИЯ РАБОТЫ БИБЛИОТЕКИ ГРАФОВ (Дм. ПА.). ВЕРШИНЫ ГРАФА НУМЕРУЮТСЯ, НАЧИНАЯ С 1");

            Console.WriteLine("Текущие дата и время: " + DateTime.Now);
            Console.WriteLine("ОБОЗНАЧЕНИЯ: p - число вершин, q - число рёбер, k - число компонент связности");
            Console.WriteLine();
            Console.WriteLine();
        }
        public void ShowCheck1()
        {
            Console.WriteLine("Матрица смежности графа:"); this.A.PrintMatrix();
            Console.WriteLine("Число вершин графа равно {0}, число рёбер равно {1}", this.p, this.Edges);
            Console.Write("Список валентностей вершин:"); this.Deg.Show();
            this.Lemma();
            this.EulerTeo();
            if (Graphs.IsFull(this)) Console.WriteLine("Граф полный");
            else if (Graphs.IsRegular(this)) Console.WriteLine("Граф не полный, но регулярный");
            else Console.WriteLine("Граф нерегулярный");

            Console.WriteLine();
            if (!(Graphs.IsFull(this)))
            {
                Console.WriteLine("Дополнение графа содержит p(p-1)/2-q={1}*{2}/2-{3}={0} рёбер", this.Addition.Edges, this.Deg.n, this.Deg.n - 1, this.Edges);
                Console.Write("Список валентностей вершин дополнения:"); this.Addition.Deg.Show();
                Console.WriteLine("Матрица смежности дополнения:"); this.Addition.A.PrintMatrix();
            }
            if (Graphs.IsSelfAdditional(this)) Console.WriteLine("Граф является самодополнительным");
            else Console.WriteLine("Граф не является самодополнительным");

            Console.WriteLine();
            Console.WriteLine("Перечисление рёбер графа: " + this.SetOfEdges);
            Console.WriteLine("Соответствующая матрица инцидентности графа:"); this.B.PrintMatrix();
            Console.WriteLine();
            Console.WriteLine("СВЯЗЬ МЕЖДУ МАТРИЦАМИ. Матрица инцидентности выводится по верхнему/нижнему треугольнику матрицы смежности. ");
            Console.WriteLine("Матрица смежности выводится из матрицы инцидентности: ");
            Console.WriteLine("\tA = B*B.Transpose - Deg (диагональная матрица с валентностями вершин по диагонали).");
            Console.WriteLine("\tДействительно. B*B.Transpose:");
            Matrix M = this.B * this.B.Transpose(); M.PrintMatrix();
            Console.WriteLine("\tB*B.Transpose - Deg:");
            Vectors v = new Vectors(this.Deg);
            for (int i = 0; i < this.p; i++)
                M[i, i] -= v[i];
            M.PrintMatrix();

        }
        public void ShowCheck2()
        {
            Console.WriteLine();
            Console.WriteLine("Матрица смежности одного из гомеоморфных графов:"); this.GomeoExample().A.PrintMatrix();
            Console.WriteLine("Матрица смежности одного из первообразных графов:"); this.OrigExample().A.PrintMatrix();
            Console.WriteLine();
        }
        public void ShowCheck3()
        {
            if (this.ContainCycle())
            {
                Console.WriteLine("Все циклы графа длины {0}:", this.p / 2); this.ShowAllCycles(this.p / 2);
                Console.WriteLine($"Обхват графа = {this.G}; окружение графа: {this.C}");
            }
            else Console.WriteLine("Граф ациклический");
        }
        public void ShowCheck4()
        {
            Vectors p;
            Console.WriteLine();
            if (this.IsBichromatic(out p))
            {
                Console.WriteLine("Граф является двудольным, распределение вершин по долям: ");
                p.Show();
            }
            else Console.WriteLine("Граф не является двудольным");
            Console.WriteLine("Результат теоремы Кёнига:"); this.KoenigTheorem();
        }
        public void ShowCheck5()
        {
            Vectors p;
            Console.WriteLine();
            Console.WriteLine($"Число компонент связности в графе = {this.CompCount(out p)}");
            if (this.CompCount(out p) > 1)
            {
                Console.WriteLine("Распределение вершин по компонентам связности: ");
                p.Show();
                Console.WriteLine("Метрические характеристики в несвязном графе не рассматриваются.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Матрица достижимости графа:"); this.Acces.PrintMatrix();
                Console.WriteLine();
                Console.WriteLine("Матрица расстояний графа:"); this.Dist.PrintMatrix();
                Console.WriteLine();
                Console.WriteLine("Массив эксцентриситетов:"); this.Eccentricity.Show();
                Console.WriteLine("Передаточные числа: "); this.P.Show();
                Console.WriteLine("Радиус графа = {0}; диаметр графа = {1}; один из центров графа: {2}", this.Radius, this.Diameter, this.Center);
                Console.WriteLine("Периферии графа (вершины с эксцентриситетом, равным диаметру графа): "); this.Peripherys.ShowPlusOne();
                Console.WriteLine("Медианы графа (вершины с эксцентриситетом, равным радиусу графа): "); this.Medians.ShowPlusOne();
                Console.WriteLine();


                Console.WriteLine("Пример кратчайшей цепи между двумя случайными вершинами:");
                Random rnd = new Random(); int z = rnd.Next(0, this.p); int x = -1; while (x == -1 || x == z) x = rnd.Next(0, this.p);
                this.Chain(z, x).Show();
            }
        }
        public void ShowCheck6()
        {
            Console.WriteLine();
            Graphs r = this.SubGraph(1, 2, 3, 4);
            Console.WriteLine("Матрица смежности подграфа, порождённого вершинами 1, 2, 3, 4:"); r.A.PrintMatrix();
            SqMatrix M = new SqMatrix(r.A), MM = new SqMatrix(M);
            Console.WriteLine("Квадрат матрицы смежности подграфа, порождённого вершинами 1, 2, 3, 4:");
            MM *= M; MM.PrintMatrix();
            Console.WriteLine("Куб матрицы смежности подграфа, порождённого вершинами 1, 2, 3, 4:");
            MM *= M; MM.PrintMatrix();

            int sum = 0;
            for (int i = 0; i < 4; i++)
            {
                sum += (int)MM[i, i];
                for (int j = i + 1; j < 4; j++)
                    sum += 2 * (int)MM[i, j];
            }
            Console.WriteLine("Полусумма всех элементов в кубе той матрицы смежности (число маршрутов длины 3): " + sum / 2);

            r.routsSearch(3);
            Console.WriteLine("Все маршруты длины {0} (всего {1}) подграфа, порождённого вершинами 1, 2, 3, 4:", 3, r.Routes.Count);
            for (int i = 0; i < r.Routes.Count; i++) Console.WriteLine(r.Routes[i]);
            Console.WriteLine("Из них - цепи (нет повторов рёбер):");
            for (int i = 0; i < r.Routes.Count; i++)
                if (Graphs.IsChain(r.Routes[i])) Console.WriteLine(r.Routes[i]);
            Console.WriteLine("Из них - простые цепи (нет повторов рёбер и вершин):");
            for (int i = 0; i < r.Routes.Count; i++)
                if (Graphs.IsSimpleChain(r.Routes[i])) Console.WriteLine(r.Routes[i]);
            Console.WriteLine("Из них - циклы (цепи с одинаковыми концами):");
            for (int i = 0; i < r.Routes.Count; i++)
                if (Graphs.IsCycle(r.Routes[i])) Console.WriteLine(r.Routes[i]);
        }
        public void ShowCheck7()
        {
            Console.WriteLine();
            Console.WriteLine($"Цикломатическое число графа (число рёбер с количеством компонент связности без числа вершин) = {this.CyclomaticN}");
            Vectors p;
            Console.WriteLine();
            Console.WriteLine();
            if (this.ComponCount == 1)
            {
                Console.WriteLine("Матрица Кирхгофа для данного графа:");
                this.Kirhg.PrintMatrix();
                Console.WriteLine("Количество остовов = {0}", this.Kirhg.Minor(1, 1));
                Console.WriteLine("Матрица смежности одного из них (полученного разрушением циклов):");
                this.GetSpanningTree().A.PrintMatrix();
                Console.WriteLine();
                Console.WriteLine("Его код Прюфера (генерация с итогом): ");
                p = Graphs.Pryufer(this.GetSpanningTree());
                p.Show();
                Console.WriteLine();
                Console.WriteLine("Матрица смежности распаковки (генерация рёбер с итогом):");
                Graphs.PryuferUnpacking(p).A.PrintMatrix();

                Console.WriteLine();
                Console.WriteLine("Матрица фундаментальных циклов графа (с выводом):");
                Matrix M = this.FundamentalCycles(this.GetSpanningTree());
                M.PrintMatrix();
                Console.WriteLine();
                Console.WriteLine("Матрица базисных разрезов графа:");
                this.BasisSection(M).PrintMatrix();
            }
            else Console.WriteLine("Циклы в несвязном графе не рассматриваются.");
        }
        public void ShowCheck8()
        {
            Vectors p;
            if (this.ComponCount == 1)
            {
                Console.Write("Точки сочленения: ");
                if (this.JointPoints.Count == 0) Console.WriteLine("таких точек нет");
                else { for (int i = 0; i < this.JointPoints.Count; i++) Console.Write(this.JointVect[i] + " "); Console.WriteLine(); }
                Console.Write("Мосты графа: ");
                if (this.Bridges.Count == 0) Console.WriteLine("отсутствуют");
                else { Console.WriteLine(); for (int i = 0; i < this.Bridges.Count; i++) Console.WriteLine((this.Bridges[i].v1 + 1) + " " + (this.Bridges[i].v2 + 1)); }
                Console.WriteLine();
                Console.WriteLine($"Рёберная связность графа = {this.Lambda} (граф рёберно-{this.Lambda}-связный)");
                Console.WriteLine($"Вершинная связность графа = {this.Kappa} (граф {this.Kappa}-связный)");
                Console.WriteLine("Матрица смежности объединения мостовых блоков (исходный граф без мостов):"); this.BridgeBlocks().A.PrintMatrix();
                Console.WriteLine("Матрица смежности сочленённого блока (максимальный подграф в исходном графе без точек сочленения):");
                this.JointBlock().A.PrintMatrix();
            }
            else
                Console.WriteLine("Характеристики связности в несвязном графе не рассматриваются.");
        }
        public void ShowCheck9()
        {
            Console.WriteLine();
            this.AboutPlanarity();
        }
        public void ShowCheck10()
        {
            Console.WriteLine();
            this.IsEuler();
            this.IsHamilton();
        }
        public void ShowCheck11()
        {
            Vectors p;
            Console.WriteLine();
            //Console.WriteLine("Независимые (внутренне устойчивые) подмножества вершин графа (подмножества наибольшей длины - максимальные, любые подмножества этих подмножеств - тоже независимые подмножества):"); this.ShowIndepSubSets();
            //Console.WriteLine("Наибольшие независимые подмножества (^наибольшие^ значит, что каждая вершина графа вне этого подмножества смежна вершине в подмножестве):"); this.ShowGreatestIndepSubSets();
            //Console.Write("-----------> Число независимости графа = {0}. Вершины максимального множества: ", this.IndependenceNumber(out p));
            int t = this.IndependenceNumber(out p);
            //p.Show();
            p = this.GetColouring();
            Console.WriteLine();
            Console.Write("Одна из раскрасок графа:"); p.Show();
            p = this.GetModifColouring();
            Console.Write("Модифицированная раскраска графа:"); p.Show();
            this.EstimateChromaticNumder();
            Polynom pol = this.Xpolymon();
            Console.Write("Хроматический полином получен по теореме: P(G) = P(G-uv)-P(G, где u~v).");
            Console.WriteLine("Таким образом, полином графа получается из длинной суммы полиномов более простых графов, учитывая то, что полиномы пустых и полных графов известны.");
            Console.Write("Хроматический полином (окончательно как сумма {0} полиномов): ", this.PolCount); pol.Show();
            Console.WriteLine("-----------> Это значит, что хроматическое число X = {0}", this.ChromaticNumber);
        }
        public void ShowCheck12()
        {
            Vectors p;
            Console.WriteLine();
            Console.WriteLine("Независимые (внутренне устойчивые) подмножества вершин графа (подмножества наибольшей длины - максимальные, любые подмножества этих подмножеств - тоже независимые подмножества):"); this.ShowIndepSubSets();
            Console.WriteLine("Наибольшие независимые подмножества (^наибольшие^ значит, что каждая вершина графа вне этого подмножества смежна вершине в подмножестве):"); this.ShowGreatestIndepSubSets();
            Console.Write("-----------> Число независимости графа = {0}. Вершины максимального множества: ", this.IndependenceNumber(out p));
            p.Show();
            Console.WriteLine();
            this.DominSub();
            Console.WriteLine("Доминирующие (внешне устойчивые) множества (записано каждое третье):"); this.ShowDominSub(3);
            Console.WriteLine("Минимальные (не содержащие в себе других) доминирующие множества:"); this.ShowMinDominSub();
            Console.WriteLine("Наименьшие (по мощности) доминирующие  множества:"); this.ShowSmallestDominSub();
            Console.WriteLine("-----------> Число доминирования равно {0}", this.DominationNumber);
            Console.WriteLine();
            Console.WriteLine("Ядро графа (множества вершин, одновременно внутренне и внешне устойчивые):"); Graphs.ShowVectorsList(this.Kernel);
        }
        public void ShowCheck13()
        {
            Console.WriteLine();
            this.VCoatingSub();
            Console.WriteLine("Вершинные покрытия графа (записано каждое третье):"); Graphs.ShowVectorsList(this.VCoatingSubsets, 3);
            Console.WriteLine("Минимальные (не содержащие в себе других) вершинные покрытия:"); Graphs.ShowVectorsList(this.MinimalVCoatingSubsets);
            Console.WriteLine("Наименьшие (по мощности) вершинные покрытия:"); this.ShowSmallestVCoatingSub();
            Console.WriteLine("-----------> Число вершинного покрытия равно {0}", this.VCoatingNumber);
            Console.WriteLine();
            this.ECoatingSub();
            Console.WriteLine("Рёберные покрытия графа (записано каждое пятое):"); Graphs.ShowEdgeListofL(this.ECoatingSubsets, 5);
            Console.WriteLine("Минимальные (не содержащие в себе других) рёберные покрытия (записано каждое третье):"); Graphs.ShowEdgeListofL(this.MinimalECoatingSubsets, 3);
            Console.WriteLine("Наименьшие (по мощности) рёберные покрытия:"); this.ShowSmallestECoatingSub();
            Console.WriteLine("-----------> Число рёберного покрытия равно {0}", this.ECoatingNumber);
            Console.WriteLine();
        }
        public void ShowCheck13Full()
        {
            Console.WriteLine();
            this.VCoatingSub();
            Console.WriteLine("Вершинные покрытия графа (записано каждое третье):"); Graphs.ShowVectorsList(this.VCoatingSubsets, 3);
            Console.WriteLine("Минимальные (не содержащие в себе других) вершинные покрытия:"); Graphs.ShowVectorsList(this.MinimalVCoatingSubsets);
            Console.WriteLine("Наименьшие (по мощности) вершинные покрытия:"); this.ShowSmallestVCoatingSub();
            Console.WriteLine("-----------> Число вершинного покрытия равно {0}", this.VCoatingNumber);
            Console.WriteLine();
            this.ECoatingSubFull();
            Console.WriteLine("Рёберные покрытия графа (записано каждое пятое):"); Graphs.ShowEdgeListofL(this.ECoatingSubsets, 5);
            Console.WriteLine("Минимальные (не содержащие в себе других) рёберные покрытия (записано каждое третье):"); Graphs.ShowEdgeListofL(this.MinimalECoatingSubsets, 3);
            Console.WriteLine("Наименьшие (по мощности) рёберные покрытия:"); this.ShowSmallestECoatingSub();
            Console.WriteLine("-----------> Число рёберного покрытия равно {0}", this.ECoatingNumber);
            Console.WriteLine();
        }
        public void ShowCheck14()
        {
            Console.WriteLine();
            this.CliquesSub();
            Console.WriteLine("Клики графа (вершины, дающие полные подграфы; записан каждый второй):"); Graphs.ShowVectorsList(this.CliquesSubsets, 2);
            Console.WriteLine("Макcимальные (не содержащиеся в других) клики:"); Graphs.ShowVectorsList(this.MaximalCliquesSubsets);
            Console.WriteLine("Наибольшие (по мощности) клики:"); Graphs.ShowVectorsList(this.GreatestCliquesSubsets);
            Console.WriteLine("-----------> Число кликового покрытия равно {0}", this.CliquesNumber);
            Console.WriteLine("-----------> Рёберная плотность графа равна 2q/(p(p-1) = 2*{0}/({1}*{2}) = {3}", this.Edges, this.p, this.p - 1, this.Density);
            Console.WriteLine("Матрица кликов графа:"); this.CliquesMatrix.PrintMatrix();
            Console.WriteLine("-----------> Плотность графа (размерность графа клик) = {0} ", this.CliquesGraph.A.ColCount);
            Console.WriteLine("Матрица смежности графа клик:"); this.CliquesGraph.A.PrintMatrix();

            Console.WriteLine();
            this.MatchingSub();
            Console.WriteLine("Паросочетания графа (записано каждое второе):"); Graphs.ShowEdgeListofL(this.MatchingSubsets, 2);
            Console.WriteLine("Макcимальные (не содержащиеся в других) паросочетания:"); Graphs.ShowEdgeListofL(this.MaximalMatchingSubsets);
            Console.WriteLine("Наибольшие (по мощности) паросочетания:"); Graphs.ShowEdgeListofL(this.GreatestMatchingSubsets);
            Console.WriteLine("-----------> Число паросочетания равно {0}", this.MatchingNumber);
        }

        /// <summary>
        /// Показать информацию о графе в консоли
        /// </summary>
        public void ShowInfoConsole()
        {
            ShowCheck0();
            ShowCheck1();
            ShowCheck2();
            ShowCheck3();
            ShowCheck4();
            ShowCheck5();
            ShowCheck6();
            ShowCheck7();
            ShowCheck8();
            ShowCheck9();
            ShowCheck10();
            ShowCheck11();
            ShowCheck12();
            ShowCheck13();
            ShowCheck14();
        }
        /// <summary>
        /// Вывести информацию о графе в файл
        /// </summary>
        public void ShowInfoFile()
        {
            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            this.ShowInfoConsole();
            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");
        }
    }

    /// <summary>
    /// Методы, связанные с функциями
    /// </summary>
    public static class FuncMethods
    {
        #region Внутренние функции
        /// <summary>
        /// Система мономов
        /// </summary>
        public static SequenceFunc Monoms = (double x, int i) => { return Math.Pow(x, i); };
        /// <summary>
        /// Система мономов как полиномы
        /// </summary>
        public static SequencePol Monom = (int i) => Graphs.Degree(i);

        /// <summary>
        /// Система многочленов Лежандра, ортогональных на отрезке
        /// </summary>
        public static SequenceFunc Lezhandrs(double a, double b)
        {
            return (double x, int k) =>
            {
                double s = (2 * x - 2 * a) / (b - a) - 1;
                Polynom p = Polynom.Lezh(k);
                //double s = ((x + 1) / 2 - a) / (b - a);
                //double s = (x * (b - a) + a) * 2 - 1;

                return p.Value(s);
            };
            //return (double x, int k) => Polynom.Lezh(k).Value(x*(b-a)+a);
        }
        /// <summary>
        /// Система многочленов Лежандра как многочленов, ортогональных на отрезке
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SequencePol Lezhandr(double a, double b)
        {
            //Polynom s = new Polynom(new double[] { (1.0/2-a)/(b-a),1.0/2/(b-a)});
            Polynom s = new Polynom(new double[] { -2 * a / (b - a) - 1, 2.0 / (b - a) });
            return (int k) =>
            {
                Polynom p = Polynom.Lezh(k);
                return p.Value(s);
            };
            //return (double x, int k) => Polynom.Lezh(k).Value(x*(b-a)+a);
        }

        /// <summary>
        /// Система многочленов Чебышева, ортогональных на отрезке
        /// </summary>
        public static SequenceFunc Chebs(double a, double b)
        {
            return (double x, int k) =>
            {
                double s = (2 * x - 2 * a) / (b - a) - 1;
                Polynom p = Polynom.Cheb(k);
                return p.Value(s);
            };
        }
        /// <summary>
        /// Система многочленов Чебышева как многочленов, ортогональных на отрезке
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SequencePol Cheb(double a, double b)
        {
            Polynom s = new Polynom(new double[] { -2 * a / (b - a) - 1, 2 / (b - a) });
            return (int k) =>
            {
                Polynom p = Polynom.Cheb(k);
                return p.Value(s);
            };
        }

        /// <summary>
        /// Система многочленов Лагерра, ортогональных на отрезке
        /// </summary>
        public static SequenceFunc Lagerrs(double a, double b) { return null; }
        /// <summary>
        /// Система многочленов Эрмита, ортогональных на отрезке
        /// </summary>
        public static SequenceFunc Hermits(double a, double b) { return null; }
        /// <summary>
        /// Тригонометрическая система, ортогональная на отрезке
        /// </summary>
        public static SequenceFunc TrigSystem(double a, double b)
        {
            double l = (b - a) / 2, s2l = Math.Sqrt(2 * l), sl = Math.Sqrt(l);
            return (double x, int k) =>
            {
                k++;
                if (k == 1) return 1.0 / s2l;
                if (k % 2 == 0) return Math.Cos(k / 2 * x * Math.PI / l) / sl;
                return Math.Sin((k - 1) / 2 * x * Math.PI / l) / sl;
            };
        }

        private static double Haar(double x, int k, int m)
        {
            double val = 0, sq = Math.Pow(2, m);
            if (x == 1) x -= 0.0000001;

            if (x >= (double)k / sq && x < ((double)k + 0.5) / sq) val = Math.Sqrt(sq);
            else if (x >= ((double)k + 0.5) / sq && x </*=*/ ((double)k + 1) / sq) val = -Math.Sqrt(sq);

            return val;
        }
        /// <summary>
        /// Система функций Хаара, ортогональных на отрезке
        /// </summary>
        public static SequenceFunc HaarSystem(double a, double b)
        {
            return (double t, int n) =>
            {
                double x = (t - a) / (b - a);
                n++;
                double val = 1;
                if (n >= 2)
                {
                    int m = (int)Math.Log(n - 1, 2);
                    int k = n - 1 - (int)Math.Pow(2, m);
                    val = Haar(x, k, m);
                }
                return val;
            };
        }
        #endregion

        /// <summary>
        /// Сеточная функция
        /// </summary>
        public class NetFunc
        {
            /// <summary>
            /// Список узлов
            /// </summary>
            private List<Point> Knots;
            /// <summary>
            /// Интерполяционный многочлен Лагранжа для этой сеточной функции
            /// </summary>
            private RealFunc Lag = null;
            /// <summary>
            /// Интерполяционная рациональная функция для этой сеточной функции
            /// </summary>
            private RealFunc R = null;
            /// <summary>
            /// Интерполяционный кубический сплайн для этой сеточной функции
            /// </summary>
            private RealFunc CubeSpline = null;
            /// <summary>
            /// Значение сеточной функции в конце области определения
            /// </summary>
            /// <returns></returns>
            public double LastVal() => this[this.CountKnots - 1];
            /// <summary>
            /// Последний аргумент сеточной функции
            /// </summary>
            /// <returns></returns>
            public double LastArg() => Arg(this.CountKnots - 1);
            /// <summary>
            /// Массив значений сеточной функции
            /// </summary>
            /// <returns></returns>
            public double[] Values
            {
                get
                {
                    Point[] p = this.Points;
                    double[] res = new double[p.Length];
                    for (int i = 0; i < p.Length; i++)
                        res[i] = p[i].y;
                    return res;
                }

            }

            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            public NetFunc() { this.Knots = new List<Point>(); }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="f"></param>
            public NetFunc(NetFunc f) { this.Knots = new List<Point>(f.Knots); }
            /// <summary>
            /// Генерация сеточной функции по списку точек
            /// </summary>
            /// <param name="L"></param>
            public NetFunc(List<Point> L) { this.Knots = new List<Point>(L); this.Refresh(); }
            /// <summary>
            /// Генерация сеточной функции по массиву точек
            /// </summary>
            /// <param name="P"></param>
            public NetFunc(Point[] P)
            {
                this.Knots = new List<Point>();
                for (int i = 0; i < P.Length; i++) this.Knots.Add(P[i]);
                this.Refresh();
            }
            /// <summary>
            /// Генерация сеточной функции по массиву точек, расположенному в файле
            /// </summary>
            /// <param name="fs"></param>
            public NetFunc(StreamReader fs)
            {
                Point[] P = Point.Points(fs);
                this.Knots = new List<Point>();
                for (int i = 0; i < P.Length; i++) this.Knots.Add(P[i]);
                this.Refresh();
            }
            /// <summary>
            /// Генерация сеточной функции по точкам от действительной функции
            /// </summary>
            /// <param name="f">Действительная функция</param>
            /// <param name="n">Число точек</param>
            /// <param name="a">Начало отрезка интерполяции</param>
            /// <param name="b">Конец отрезка интерполяции</param>
            public NetFunc(RealFunc f, int n, double a, double b)
            {
                Point[] P = Point.Points(f, n, a, b);
                this.Knots = new List<Point>();
                for (int i = 0; i < P.Length; i++) this.Knots.Add(P[i]);
            }
            /// <summary>
            /// Генерация сеточной функции по действительной фунции и набору абцисс
            /// </summary>
            /// <param name="f"></param>
            /// <param name="c"></param>
            public NetFunc(RealFunc f, double[] c)
            {
                Point[] P = Point.Points(f, c);
                this.Knots = new List<Point>();
                for (int i = 0; i < P.Length; i++) this.Knots.Add(P[i]);
                this.Refresh();
            }
            /// <summary>
            /// Задание сеточной функции по массиву узлов и массиву значений в узлах
            /// </summary>
            /// <param name="arg"></param>
            /// <param name="val"></param>
            public NetFunc(double[] arg, double[] val)
            {
                List<Point> p = new List<Point>();
                for (int i = 0; i < arg.Length; i++)
                    p.Add(new Point(arg[i], val[i]));
                this.Knots = new List<Point>(p); this.Refresh();
            }
            /// <summary>
            /// Усреднённая сеточная функция с условием, что все сеточные функции определены на одной и той же сетке
            /// </summary>
            /// <param name="mas"></param>
            public NetFunc(NetFunc[] mas)
            {
                this.Knots = new List<Point>(mas.Length);
                for (int i = 0; i < this.Knots.Count; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < mas.Length; j++)
                        sum += mas[j].Knots[i].y;
                    this.Knots[i] = new Point(mas[0].Knots[i].x, sum / mas.Length);
                }
            }

            private void Refresh()
            {
                Knots.Sort();
                for (int i = 0; i < this.CountKnots - 1; i++)
                    if (this.Knots[i].x == this.Knots[i + 1].x)
                    {
                        this.Delete(i);
                        i--;
                    }
            }
            /// <summary>
            /// Значение сеточной функции в такой-то точке её сетки
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public double this[int i]
            {
                get
                {
                    return this.Knots[i].y;
                }
            }
            /// <summary>
            /// Аргумент сеточной функции в таком-то узле её сетки
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public double Arg(int i)
            { return this.Knots[i].x; }
            /// <summary>
            /// Массив аргументов сеточной функции
            /// </summary>
            public double[] Arguments
            {
                get
                {
                    double[] c = new double[this.Knots.Count];
                    for (int i = 0; i < c.Length; i++)
                        c[i] = this.Knots[i].x;
                    return c;
                }
            }

            /// <summary>
            /// Количество узлов
            /// </summary>
            public int CountKnots
            {
                get { return Knots.Count; }
            }
            /// <summary>
            /// Минимальный аргумент
            /// </summary>
            public double MinArg
            {
                get { return this.Knots[0].x; }
            }
            /// <summary>
            /// Максимальный аргумент
            /// </summary>
            public double MaxArg
            {
                get { return this.Knots[CountKnots - 1].x; }
            }
            /// <summary>
            /// Интерполяционный полином Лагранжа этой сеточной функции
            /// </summary>
            public RealFunc Lagrange
            {
                get
                {
                    if (this.Lag != null) return this.Lag;

                    Point[] P = Point.Points(this.Knots);
                    Polynom Pol = Polynom.Lag(P);
                    this.Lag = Pol.Value;
                    return Pol.Value;
                }
            }
            /// <summary>
            /// Интерполяционный кубический сплайн этой сеточной функции
            /// </summary>
            public RealFunc Spline
            {
                get
                {
                    if (this.CubeSpline != null) return this.CubeSpline;

                    Point[] P = Point.Points(this.Knots);
                    RealFunc Pol = Polynom.CubeSpline(P);
                    this.CubeSpline = Pol;
                    return Pol;
                }
            }
            /// <summary>
            /// Интерполяционная рациональная функция этой сеточной функции
            /// </summary>
            public RealFunc RatFunc(int p, int q)
            {
                if (this.R != null) return this.R;

                Point[] P = Point.Points(this.Knots);
                RealFunc Pol = Polynom.R(P, p, q);
                this.R = Pol;
                return Pol;
            }
            /// <summary>
            /// Массив узлов сеточной функции
            /// </summary>
            public Point[] Points
            {
                get
                {
                    Point[] p = new Point[this.CountKnots];
                    for (int i = 0; i < p.Length; i++) p[i] = new Point(this.Knots[i]);
                    return p;
                }
            }

            /// <summary>
            /// Добавить узел в функцию
            /// </summary>
            /// <param name="p"></param>
            public void Add(Point p)
            {
                Knots.Add(p);
                /*this.CountKnots++;*/
                Knots.Sort();
                for (int i = 0; i < this.CountKnots - 1; i++)
                    if (this.Knots[i].x == this.Knots[i + 1].x)
                    {
                        this.Delete(i);
                        i--;
                    }
            }
            /// <summary>
            /// Удалить элемент из списка
            /// </summary>
            /// <param name="k"></param>
            public void Delete(int k) { this.Knots.RemoveAt(k); /*this.CountKnots--;*/ }
            /// <summary>
            /// Удалить элемент с указанной абциссой
            /// </summary>
            /// <param name="x"></param>
            public void Delete(double x)
            {
                if (this.Knots.Exists((Point p) => p.x == x))
                {
                    Point a = this.Knots.Find((Point p) => { return p.x == x; });
                    int k = this.Knots.IndexOf(a);
                    this.Delete(k);
                }
            }

            /// <summary>
            /// Очистить список узлов
            /// </summary>
            public void Clear() { this.Knots.Clear(); this.R = null; this.Lag = null; this.CubeSpline = null; }
            /// <summary>
            /// Значение сеточной функции в точке, наиболее близкой к точке x
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public double Value(double x)
            {
                Point a = new Point(Knots[0]), b = new Point(a);
                if (this.Knots.Exists((Point p) => p.x == x))
                {
                    a = this.Knots.Find((Point p) => { return p.x == x; });
                    return a.y;
                }

                double k = Math.Abs(x - Knots[0].x);
                while (a != null)
                {
                    a = this.Knots.Find((Point p) => { return Math.Abs(p.x - x) < k; });
                    if (a != null) b = new Point(a);
                    k = Math.Abs(a.x - x);
                }
                return b.y;
            }

            /// <summary>
            /// Вывести массив узлов на консоль
            /// </summary>
            public void Show()
            {
                Point[] P = new Point[this.CountKnots];
                for (int i = 0; i < this.CountKnots; i++) P[i] = new Point(Knots[i]);
                Point.Show(P);
            }

            /// <summary>
            /// Скалярное произведение сеточных функций
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double ScalarP(NetFunc a, NetFunc b)
            {
                if (a.Knots.Count != b.Knots.Count) throw new Exception("Сеточные функции имеют разную размерность!");
                double sum = 0;
                for (int i = 0; i < a.Knots.Count; i++) sum += a[i] * b[i];
                sum /= a.Knots.Count;
                return sum;
            }
            /// <summary>
            /// Скалярное произведение сеточной и действительной функции
            /// </summary>
            /// <param name="a"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static double ScalarP(NetFunc a, RealFunc f)
            {
                double[] c = a.Arguments;
                NetFunc b = new NetFunc(f, c);
                return ScalarP(a, b);
            }
            /// <summary>
            /// Скалярное произведение сеточной и действительной функции
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <returns></returns>
            public static double ScalarP(RealFunc f, NetFunc a) { return NetFunc.ScalarP(a, f); }
            /// <summary>
            /// Скалярное произведение двух действителььных функций на сетке
            /// </summary>
            /// <param name="f"></param>
            /// <param name="g"></param>
            /// <param name="c"></param>
            /// <returns></returns>
            public static double ScalarP(RealFunc f, RealFunc g, double[] c)
            {
                NetFunc a = new NetFunc(f, c);
                NetFunc b = new NetFunc(g, c);
                return ScalarP(a, b);
            }
            /// <summary>
            /// Норма сеточной функции
            /// </summary>
            /// <param name="a"></param>
            /// <returns></returns>
            public double Norm(NetFunc a) { return Math.Sqrt(NetFunc.ScalarP(a, a)); }
            /// <summary>
            /// Расстояние между сеточными функциями
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double Distance(NetFunc a, NetFunc b)
            {
                if (a.Knots.Count != b.Knots.Count) throw new Exception("Сеточные функции имеют разную размерность!");
                double sum = 0;
                for (int i = 0; i < a.Knots.Count; i++) sum += (a[i] - b[i]) * (a[i] - b[i]);
                sum /= a.Knots.Count;
                return Math.Sqrt(sum);
            }
            /// <summary>
            /// Расстояние между сеточной функцией и действительной функцией
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double Distance(NetFunc a, RealFunc b)
            {
                NetFunc c = new NetFunc(b, a.Arguments);
                return Distance(a, c);
            }
        }

        /// <summary>
        /// Класс решения ОДУ
        /// </summary>
        public static class ODU
        {
            public enum Method
            {
                /// <summary>
                /// Метод Эйлера
                /// </summary>
                E1,
                /// <summary>
                /// Метод Эйлера с пересчётом
                /// </summary>
                E2,
                /// <summary>
                /// Метод Хойна
                /// </summary>
                H,
                /// <summary>
                /// Метод Рунге-Кутты 3 порядка
                /// </summary>
                RK3,
                /// <summary>
                /// Метод Рунге-Кутты 4 порядка
                /// </summary>
                RK4,
                /// <summary>
                /// Правило трёх восьмых
                /// </summary>
                P38,
                /// <summary>
                /// Метод Фельдберга
                /// </summary>
                F,
                /// <summary>
                /// Метод Ческино
                /// </summary>
                C
            }
            private static SqMatrix E1, E2, H, RK3, Rk4, P38;
            private static Matrix F, C;
            private static int MaxStepChange = 2000;
            static ODU()
            {
                E1 = new SqMatrix(2);
                E1[1, 1] = 1;
                E2 = new SqMatrix(3);
                E2[1, 0] = 0.5; E2[1, 1] = 0.5; E2[2, 2] = 1;
                H = new SqMatrix(4);
                H[1, 0] = 1.0 / 3; H[1, 1] = 1.0 / 3; H[2, 0] = 2.0 / 3; H[2, 2] = 2.0 / 3; H[3, 1] = 1.0 / 4; H[3, 3] = 3.0 / 4;
                RK3 = new SqMatrix(4);
                RK3[1, 0] = 0.5; RK3[1, 1] = 0.5; RK3[2, 0] = 1; RK3[2, 2] = 1; RK3[3, 1] = 1.0 / 6; RK3[3, 2] = 4.0 / 6; RK3[3, 3] = 1.0 / 6;
                Rk4 = new SqMatrix(5);
                Rk4[1, 0] = 0.5; Rk4[1, 1] = 0.5; Rk4[2, 0] = 0.5; Rk4[2, 2] = 0.5; Rk4[3, 0] = 1; Rk4[3, 3] = 1; Rk4[4, 1] = 1.0 / 6; Rk4[4, 2] = 2.0 / 6; Rk4[4, 3] = 2.0 / 6; Rk4[4, 4] = 1.0 / 6;
                P38 = new SqMatrix(5);
                P38[1, 0] = 1.0 / 3; P38[1, 1] = 1.0 / 3; P38[2, 0] = 2.0 / 3; P38[2, 1] = -1.0 / 3; P38[2, 2] = 1; P38[3, 0] = 1; P38[3, 1] = 1; P38[3, 2] = -1; P38[3, 3] = 1; P38[4, 1] = 1.0 / 8; P38[4, 2] = 3.0 / 8; P38[4, 3] = 3.0 / 8; P38[4, 4] = 1.0 / 8;
                F = new Matrix(5, 4);
                F[1, 0] = 1; F[1, 1] = 1; F[2, 0] = 0.5; F[2, 1] = 0.25; F[2, 2] = 0.25; F[3, 1] = 0.5; F[3, 2] = 0.5; F[4, 1] = 1.0 / 6; F[4, 2] = 1.0 / 6; F[4, 3] = 4.0 / 6;
                C = new Matrix(6, 5);
                C[1, 0] = 0.25; C[1, 1] = 0.25; C[2, 0] = 0.5; C[2, 2] = 0.5; C[3, 0] = 1; C[3, 1] = 1; C[3, 3] = -2; C[3, 4] = 2; C[4, 1] = 1; C[4, 2] = -2; C[4, 3] = 2; C[5, 1] = 1.0 / 6; C[5, 3] = 4.0 / 6; C[5, 4] = 1.0 / 6;
            }

            private static void Get2Tmp(ref double tmp, ref double tmpp, double step, Matrix A, double[] k, DRealFunc f, double u, double t, int r)
            {
                tmp = 0; tmpp = 0;
                for (int i = 0; i < k.Length; i++)
                {
                    double tmp2 = 0;
                    for (int j = 0; j < i; j++)
                        tmp2 += A[i, j + 1] * k[j];
                    k[i] = f(t + A[i, 0] * step, u + tmp2 * step);
                    tmp += A[r, i + 1] * k[i];
                    if (A.RowCount != A.ColCount) tmpp += A[r + 1, i + 1] * k[i];
                }
            }

            /// <summary>
            /// Решение приведённого ОДУ первого порядка
            /// </summary>
            /// <param name="f">Свободная функция переменных u и x, где u - искомая функция</param>
            /// <param name="begin">Начальный аргумент по задаче Коши</param>
            /// <param name="end">Конечный аргумент</param>
            /// <param name="step">Шаг интегрирования</param>
            /// <param name="M">Метод поиска решения</param>
            /// <param name="begval">Значение функции при начальном аргументе</param>
            /// <param name="eps">Допустимый уровень расчётных погрешностей</param>
            /// <returns></returns>
            public static NetFunc ODUsearch(DRealFunc f, double begin = 0, double end = 10, double step = 0.01, Method M = Method.E1, double begval = 1, double eps = 0.0001, bool controllingstep = false)
            {
                double thisstep = step;
                NetFunc res = new NetFunc();
                res.Add(new Point(begin, begval));
                step *= Math.Sign(end - begin);

                Matrix A;
                switch (M)
                {
                    case Method.E1:
                        A = E1;
                        break;
                    case Method.E2:
                        A = E2;
                        break;
                    case Method.H:
                        A = H;
                        break;
                    case Method.RK3:
                        A = RK3;
                        break;
                    case Method.RK4:
                        A = Rk4;
                        break;
                    case Method.P38:
                        A = P38;
                        break;
                    case Method.F:
                        A = F;
                        break;
                    default:
                        A = C;
                        break;
                }

                int r = A.RowCount - 1;
                if (A.RowCount != A.ColCount) r--;

                while (begin </*=*/end)
                {
                    double u = res.LastVal();
                    double t = res.LastArg();

                    double[] k = new double[r];
                    double tmp = 0, tmpp = 0;
                    int stepchange = 0;

                    //double[] h2 = new double[2];

                    //Get2Tmp(ref tmp, ref tmpp, step/2, A, k, f, u, t, r);
                    //double uh1 = u + tmp * step/2;
                    //Get2Tmp(ref tmp, ref tmpp, step / 2, A, k, f, uh1, t+step/2, r);
                    //double uh2 = uh1 + tmp * step/2;


                    Get2Tmp(ref tmp, ref tmpp, step, A, k, f, u, t, r);
                    double val1 = u + step * tmp, val2 = u + step * tmpp;
                    double R = 0.2 * Math.Abs(val1 - val2);



                    if (controllingstep && stepchange <= MaxStepChange)
                    {
                        Get2Tmp(ref tmp, ref tmpp, step / 2, A, k, f, u, t, r);
                        double uh1 = u + tmp * step / 2;
                        Get2Tmp(ref tmp, ref tmpp, step / 2, A, k, f, uh1, t + step / 2, r);
                        double uh2 = uh1 + tmp * step / 2;

                        int p;
                        switch (M)
                        {
                            case Method.E1:
                                p = 1;
                                break;
                            case Method.E2:
                                p = 2;
                                break;
                            case Method.H:
                                p = 3;
                                break;
                            case Method.RK3:
                                p = 3;
                                break;
                            case Method.RK4:
                                p = 4;
                                break;
                            case Method.P38:
                                p = 4;
                                break;
                            case Method.F:
                                p = 3;
                                break;
                            default:
                                p = 4;
                                break;
                        }
                        double RR;
                        if (A.RowCount != A.ColCount)
                            RR = Math.Abs((uh2 - val2) / (1 - 1.0 / Math.Pow(2, p)));
                        else
                            RR = Math.Abs((uh2 - val1) / (1 - 1.0 / Math.Pow(2, p)));

                        /*if (RR < eps / 64) { step *= 2; stepchange++; }
                        else */
                        if (RR > eps) { step /= 2; stepchange++; }
                        else
                        {
                            begin += step;
                            step = thisstep;//возврат к исходному шагу
                            if (A.RowCount != A.ColCount) res.Add(new Point(begin, val2));
                            else res.Add(new Point(begin, val1));
                        }
                    }
                    else if (A.RowCount != A.ColCount && stepchange <= MaxStepChange)
                        if (R > eps)
                        {
                            step /= 2;
                            stepchange++;
                        }
                        else if (R <= eps / 64)
                        {
                            begin += step;
                            res.Add(new Point(begin, val2));
                            step *= 2;
                            stepchange++;
                        }
                        else
                        {
                            begin += step;
                            res.Add(new Point(begin, val2));
                        }
                    else
                    {
                        begin += step;
                        res.Add(new Point(begin, val1));
                    }

                    if (Math.Abs(end - begin) < step) step = Math.Abs(end - begin);
                }

                return res;
            }
            /// <summary>
            /// Решение приведённого ОДУ первого порядка
            /// </summary>
            /// <param name="f">Свободная функция переменных u и x, где u - искомая функция</param>
            /// <param name="begin">Начальный аргумент по задаче Коши</param>
            /// <param name="end">Конечный аргумент</param>
            /// <param name="stepcount">Количество шагов интегрирования</param>
            /// <param name="M">Метод поиска решения</param>
            /// <param name="begval">Значение функции при начальном аргументе</param>
            public static NetFunc ODUsearch(DRealFunc f, double begin = 0, double end = 10, int stepcount = 50, Method M = Method.E1, double begval = 1, double eps = 0.00001)
            {
                double step = (end - begin) / (stepcount);
                return ODUsearch(f, begin, end, step, M, begval);
            }
            private static void Get2Tmp(ref Vectors tmp, ref Vectors tmpp, double step, Matrix A, Vectors[] k, VRealFunc f, Vectors u, double t, int r)
            {
                tmp = new Vectors(tmp.Deg);
                tmpp = new Vectors(tmp.Deg);
                for (int i = 0; i < k.Length; i++)
                {
                    Vectors tmp2 = new Vectors(tmp.Deg);
                    for (int j = 0; j < i; j++)
                        tmp2 += A[i, j + 1] * k[j];
                    k[i] = f(t + A[i, 0] * step, u + tmp2 * step);
                    tmp += A[r, i + 1] * k[i];
                    if (A.RowCount != A.ColCount) tmpp += A[r + 1, i + 1] * k[i];
                }
            }
            /// <summary>
            /// Решение системы ОДУ первого порядка
            /// </summary>
            /// <param name="f">Свободная функция переменных u и x, где u - искомая функция</param>
            /// <param name="begin">Начальный аргумент по задаче Коши</param>
            /// <param name="end">Конечный аргумент</param>
            /// <param name="step">Шаг интегрирования</param>
            /// <param name="M">Метод поиска решения</param>
            /// <param name="begval">Значение функции при начальном аргументе</param>
            /// <param name="eps">Допустимый уровень расчётных погрешностей</param>
            /// <returns></returns>
            public static VectorNetFunc ODUsearch(VRealFunc f, Vectors begval, double begin = 0, double end = 10, double step = 0.01, Method M = Method.E1, double eps = 0.0001, bool controllingstep = false)
            {
                //$"Вызов функции от вектора {begval}".Show();
                double thisstep = step;
                VectorNetFunc res = new VectorNetFunc();

                res.Add(new PointV(begin, begval));
                step *= Math.Sign(end - begin);

                Matrix A;
                switch (M)
                {
                    case Method.E1:
                        A = E1;
                        break;
                    case Method.E2:
                        A = E2;
                        break;
                    case Method.H:
                        A = H;
                        break;
                    case Method.RK3:
                        A = RK3;
                        break;
                    case Method.RK4:
                        A = Rk4;
                        break;
                    case Method.P38:
                        A = P38;
                        break;
                    case Method.F:
                        A = F;
                        break;
                    default:
                        A = C;
                        break;
                }

                int r = A.RowCount - 1;
                if (A.RowCount != A.ColCount) r--;

                while (begin </*=*/end)
                {
                    Vectors u = res.Last().Item2;
                    double t = res.Last().Item1;

                    Vectors[] k = new Vectors[r];
                    Vectors tmp = new Vectors(u.Deg), tmpp = new Vectors(u.Deg);
                    int stepchange = 0;

                    //double[] h2 = new double[2];

                    //Get2Tmp(ref tmp, ref tmpp, step/2, A, k, f, u, t, r);
                    //double uh1 = u + tmp * step/2;
                    //Get2Tmp(ref tmp, ref tmpp, step / 2, A, k, f, uh1, t+step/2, r);
                    //double uh2 = uh1 + tmp * step/2;


                    Get2Tmp(ref tmp, ref tmpp, step, A, k, f, u, t, r);
                    Vectors val1 = u + step * tmp, val2 = u + step * tmpp;
                    double R = 0.2 * (val1 - val2).EuqlidNorm;


                    if (controllingstep && stepchange <= MaxStepChange)
                    {
                        Get2Tmp(ref tmp, ref tmpp, step / 2, A, k, f, u, t, r);
                        Vectors uh1 = u + tmp * step / 2;
                        Get2Tmp(ref tmp, ref tmpp, step / 2, A, k, f, uh1, t + step / 2, r);
                        Vectors uh2 = uh1 + tmp * step / 2;

                        int p;
                        switch (M)
                        {
                            case Method.E1:
                                p = 1;
                                break;
                            case Method.E2:
                                p = 2;
                                break;
                            case Method.H:
                                p = 3;
                                break;
                            case Method.RK3:
                                p = 3;
                                break;
                            case Method.RK4:
                                p = 4;
                                break;
                            case Method.P38:
                                p = 4;
                                break;
                            case Method.F:
                                p = 3;
                                break;
                            default:
                                p = 4;
                                break;
                        }
                        double RR;
                        if (A.RowCount != A.ColCount)
                            RR = ((uh2 - val2) / (1 - 1.0 / Math.Pow(2, p))).EuqlidNorm;
                        else
                            RR = ((uh2 - val1) / (1 - 1.0 / Math.Pow(2, p))).EuqlidNorm;

                        /*if (RR < eps / 64) { step *= 2; stepchange++; }
                        else */
                        if (RR > eps) { step /= 2; stepchange++; }
                        else
                        {
                            begin += step;
                            step = thisstep;//возврат к исходному шагу
                            if (A.RowCount != A.ColCount) res.Add(new PointV(begin, val2));
                            else res.Add(new PointV(begin, val1));
                        }
                    }
                    else if (A.RowCount != A.ColCount && stepchange <= MaxStepChange)
                        if (R > eps)
                        {
                            step /= 2;
                            stepchange++;
                        }
                        else if (R <= eps / 64)
                        {
                            begin += step;
                            res.Add(new PointV(begin, val2));
                            step *= 2;
                            stepchange++;
                        }
                        else
                        {
                            begin += step;
                            res.Add(new PointV(begin, val2));
                        }
                    else
                    {
                        begin += step;
                        res.Add(new PointV(begin, val1));
                    }

                    if (Math.Abs(end - begin) < step) step = Math.Abs(end - begin);
                }

                return res;
            }

            /// <summary>
            /// Решение задачи о стрельбе
            /// </summary>
            /// <param name="f">Свободная функция в системе ОДУ</param>
            /// <param name="F">Функция из граничных условий</param>
            /// <param name="alp">Вектор альфа начального приближения при поиске корня</param>
            /// <param name="list">Промежуточный список вектор-функций</param>
            /// <param name="vlist">Промежуточный список векторов</param>
            /// <param name="netlist">Промежуточный список вектор-функций (так как делегаты передаются плохо)</param>
            /// <param name="begin">Начало отрезка задания аргумента</param>
            /// <param name="end">Конец отрезка задания аргумента</param>
            /// <param name="stepcount">Число шагов при решении ОДУ</param>
            /// <param name="M">Метод решения ОДУ</param>
            /// <param name="eps">Погрешность</param>
            /// <param name="l">Коэффициент для метода итераций</param>
            /// <param name="controlstep">Нужно ли следить за шагом при решении ОДУ</param>
            /// <returns></returns>
            public static VectorFunc ShootQu(VRealFunc f, TwoVectorToVector F, Vectors alp, out List<VectorFunc> list, out List<Vectors> vlist, out List<VectorNetFunc> netlist, double begin = 0, double end = 10, int stepcount = 50, Method M = Method.RK4, double eps = 1e-5, double l = 0.1, bool controlstep = false)
            {
                list = new List<VectorFunc>(); vlist = new List<Vectors>(); netlist = new List<VectorNetFunc>();
                double step = (end - begin) / (stepcount - 1);
                VRealFunc u = (double t, Vectors v) =>
                {
                    var k = ODUsearch(f, v, begin, end, step, M, eps, controlstep);
                    if (t == end) return k.Last().Item2;
                    double arg = begin; int i = 1;
                    while (arg < t) arg = k[i++].Item1;
                    return k[--i].Item2;
                };

                VectorToVector FF = (Vectors v) => F(v, u(end, v));
                Vectors xn;
                if (FF(alp).EuqlidNorm > eps)
                {
                    Vectors fx0 = FF(alp);//fx0.Show();
                    vlist.Add(fx0);
                    list.Add((double t) => u(t, new Vectors(fx0)));
                    VectorNetFunc tmp = new VectorNetFunc();
                    for (int i = 0; i < stepcount; i++)
                    {
                        double arg = begin + i * step;
                        tmp.Add(new Tuple<double, Vectors>(arg, list.Last()(arg)));
                    }
                    netlist.Add(tmp);
                    VectorToVector ef = (Vectors v) => v - l * FF(v);

                    //double stnorm, stvector, neunorm, neuvector;
                    bool delay = true;
                    while ((ef(fx0) - fx0).EuqlidNorm > eps && delay)
                    {
                        xn = ef(fx0) - fx0;
                        Console.WriteLine($"Norm f(v)-v ={xn.EuqlidNorm}");//fx0.Show();
                        fx0 = ef(fx0);
                        vlist.Add(fx0);//fx0.Show();
                        list.Add(new VectorFunc((double t) => u(t, fx0)));
                        //$"list({3}) = {list.Last()(3)}".Show();
                        tmp = new VectorNetFunc();
                        for (int i = 0; i < stepcount; i++)
                        {
                            double arg = begin + i * step;
                            tmp.Add(new Tuple<double, Vectors>(arg, list.Last()(arg)));
                        }
                        netlist.Add(tmp);
                        if ((ef(fx0) - fx0 - xn).EuqlidNorm < eps / 1e4) delay = false;
                    }
                }
                else
                {
                    vlist.Add(alp);
                    list.Add((double t) => u(t, new Vectors(alp)));

                    VectorNetFunc tmp = new VectorNetFunc();
                    for (int i = 0; i < stepcount; i++)
                    {
                        double arg = begin + i * step;
                        tmp.Add(new Tuple<double, Vectors>(arg, list.Last()(arg)));
                    }
                    netlist.Add(tmp);
                }

                return list.Last();

            }

            /// <summary>
            /// Решение задачи Штурма-Лиувилля
            /// </summary>
            /// <param name="g">Функция внутри второй производной</param>
            /// <param name="h">Функция при первой производной</param>
            /// <param name="s">Функция при искомой функции</param>
            /// <param name="f">Свободная функция</param>
            /// <param name="a">Начало отрезка</param>
            /// <param name="b">Конец отрезка</param>
            /// <param name="N">Число шагов</param>
            /// <param name="A"></param>
            /// <param name="B"></param>
            /// <param name="C"></param>
            /// <param name="D"></param>
            /// <param name="A1"></param>
            /// <param name="B1"></param>
            /// <param name="C1"></param>
            /// <param name="D1"></param>
            /// <returns></returns>
            public static NetFunc SchLiuQu(RealFunc g, RealFunc h, RealFunc s, RealFunc f, out double nevaska, double a = 0, double b = 10, int N = 50, double A = 1, double B = 1, double D = 1, double A1 = 1, double B1 = 1, double D1 = 1, bool firstkind = true)
            {
                double[] hn = new double[N + 1], fn = new double[N + 1], sn = new double[N + 1], tn = new double[N + 1], an = new double[N + 1], bn = new double[N + 1], cn = new double[N + 1], dn = new double[N + 1];
                double t = (b - a) / N;

                for (int i = 0; i < N + 1; i++)
                {
                    double arg = a + i * t;
                    tn[i] = arg;
                    hn[i] = h(arg);
                    fn[i] = f(arg);
                    sn[i] = s(arg);
                    an[i] = (g(arg - t / 2) / t - hn[i] / 2) / t;
                    cn[i] = (g(arg + t / 2) / t + hn[i] / 2) / t;
                    bn[i] = an[i] + cn[i] - sn[i];//поставил sn вместо hn
                                                  //bn[i] = (g(arg + t / 2) - g(arg - t / 2)) / t / t - sn[i];
                    dn[i] = fn[i];
                }

                double k1 = 0, k2 = 0;

                if (firstkind)
                {
                    bn[0] = A / t - B; cn[0] = A / t; dn[0] = D;
                    an[N] = -A1 / t; bn[N] = -A1 / t - B1; dn[N] = D1;
                }
                else
                {
                    bn[0] = 3 * A / 2 / t - B; cn[0] = 2 * A / t; k1 = -A / 2 / t;
                    bn[N] = -3 * A1 / 2 / t - B1; an[N] = -2 * A1 / t; k2 = A1 / 2 / t;
                }

                dn[0] = D;
                dn[N] = D1;

                SLAU S = new SLAU(N + 1);
                S.A[0, 0] = -bn[0];
                S.A[0, 1] = cn[0]; S.A[0, 2] = k1;
                S.A[N, N - 1] = an[N]; S.A[N, N - 2] = k2;
                S.A[N, N] = -bn[N];
                S.b[0] = dn[0]; S.b[N] = dn[N];
                for (int i = 1; i < N; i++)
                {
                    S.A[i, 0 + i - 1] = an[i];
                    S.A[i, 1 + i - 1] = -bn[i];
                    S.A[i, 2 + i - 1] = cn[i];
                    S.b[i] = dn[i];
                }

                S.Show(); "".Show();

                double c1 = S.A[0, 2] / S.A[1, 2], c2 = S.A[N, N - 2] / S.A[N - 1, N - 2];
                for (int i = 0; i < 3; i++)
                {
                    S.A[0, i] -= S.A[1, i] * c1;
                    S.A[N, N - i] -= S.A[N - 1, N - i] * c2;
                }
                S.b[0] -= S.b[1] * c1; S.b[N] -= S.b[N - 1] * c2;

                //S.Show(); "".Show();

                S.ProRace(); S.Show(); nevaska = S.Nevaska;

                NetFunc res = new NetFunc();
                for (int i = 0; i < N + 1; i++)
                    res.Add(new Point(tn[i], S.x[i]));
                return res;
            }

            /// <summary>
            /// Решение уравнения теплопроводности явной либо неявной схемой
            /// </summary>
            /// <param name="f">Свободная фунция из уравнения</param>
            /// <param name="f1">Функция из первого краевого условия</param>
            /// <param name="f2">Функция из второго краевого условия</param>
            /// <param name="u0">Функция из начальных условий</param>
            /// <param name="u">Искомая функция (нужна для вычисления точности самого решения)</param>
            /// <param name="a">Коэффициент при второй производной</param>
            /// <param name="A1"></param>
            /// <param name="B1"></param>
            /// <param name="A2"></param>
            /// <param name="B2"></param>
            /// <param name="x0">Начало отрезка по пространству</param>
            /// <param name="X">Конец отрезка по пространству</param>
            /// <param name="t0">Начало отрезка по времени</param>
            /// <param name="T">Конец отрезка по времени</param>
            /// <param name="xcount">Число шагов по пространству</param>
            /// <param name="tcount">Число шагов по времени</param>
            /// <param name="accuracy">Выводимая точность</param>
            /// <param name="explict">Использовать явную схему либо нет</param>
            /// <returns></returns>
            public static List<NetFunc> TU(DRealFunc f, RealFunc f1, RealFunc f2, RealFunc u0, DRealFunc u, double a, double A1, double B1, double A2, double B2, double x0, double X, double t0, double T, int xcount, int tcount, out double accuracy, bool explict = true, bool thirdkind = true)
            {
                List<NetFunc> res = new List<NetFunc>();
                double h = (X - x0) / (xcount - 1);
                double tau = (T - t0) / (tcount - 1);
                double[] x = new double[xcount], t = new double[tcount];
                double[] value = new double[xcount];

                for (int i = 0; i < xcount; i++)
                    x[i] = x0 + i * h;
                for (int i = 0; i < tcount; i++)
                    t[i] = t0 + i * tau;

                for (int i = 0; i < xcount; i++)
                    value[i] = u0(x[i]);
                res.Add(new NetFunc(x, value));

                double th = tau / h / h;
                double h1 = A1 + h * B1, h2 = A2 + h * B2; //(h1*h1.Reverse()).Show();

                if (explict)
                    for (int i = 1; i < tcount; i++)
                    {
                        for (int j = 1; j < xcount - 1; j++)
                            value[j] = res[i - 1].Values[j] + a * th * (res[i - 1].Values[j - 1] - 2 * res[i - 1].Values[j] + res[i - 1].Values[j + 1]) + tau * f(t[i/*-1*/], x[j]);
                        if (thirdkind)
                        {
                            value[0] = (A1 * value[1] + h * f1(t[i])) / h1;
                            value[xcount - 1] = (A2 * value[xcount - 2] + h * f2(t[i])) / h2;
                        }
                        else
                        {
                            value[0] = f1(t[i]) / B1;
                            value[xcount - 1] = f2(t[i]) / B2;
                        }

                        res.Add(new NetFunc(x, value));
                        //new Vectors (res.Last().Values).Show();
                    }
                else
                {
                    SLAU s = new SLAU(xcount);

                    for (int i = 1; i < tcount; i++)
                    {
                        if (thirdkind)
                        {
                            s.A[0, 0] = h1; s.A[0, 1] = -A1; s.b[0] = h * f1(t[i]);
                            s.A[xcount - 1, xcount - 1] = h2; s.A[xcount - 1, xcount - 2] = -A2; s.b[xcount - 1] = h * f2(t[i]);
                        }
                        else
                        {
                            s.A[0, 0] = B1; s.b[0] = f1(t[i]);
                            s.A[xcount - 1, xcount - 1] = B2; s.b[xcount - 1] = f2(t[i]);
                        }


                        for (int j = 1; j < xcount - 1; j++)
                        {
                            s.A[j, j - 2 + 1] = -a * th;
                            s.A[j, j - 2 + 2] = a * 2 * th + 1;
                            s.A[j, j - 2 + 3] = -a * th;
                            s.b[j] = res[i - 1].Values[j - 2 + 2] + tau * f(t[i/*-1*/], x[j]);
                        }

                        s.ProRace();
                        // "".Show();
                        //s.Show();
                        value = s.x;
                        res.Add(new NetFunc(x, value));
                    }
                }

                accuracy = 0;
                double[,] ac = new double[tcount, xcount];
                for (int i = 0; i < t.Length; i++)
                    for (int j = 0; j < x.Length; j++)
                        ac[i, j] = Math.Abs(u(t[i], x[j]) - res[i].Values[j]);
                accuracy = new Matrix(ac).Max;

                return res;
            }
        }

        //-------------------------------------интегралы
        /// <summary>
        /// Определённые интегралы
        /// </summary>
        public static class DefInteg
        {
            /// <summary>
            /// Шаг при интегрировании
            /// </summary>
            public static readonly double STEP = 0.0001;
            /// <summary>
            /// Оценка точности при интегрировании
            /// </summary>
            public static double EPS = STEP / 200/*Double.Epsilon*1E+58*/;
            /// <summary>
            /// Количество узлов
            /// </summary>
            public static int n = 20;
            /// <summary>
            /// Количество итераций при поиске корней многочлена
            /// </summary>
            public static int iter_count = 100;
            /// <summary>
            /// Количество шагов при интегрировании
            /// </summary>
            public static int h_Count = 5000;
            /// <summary>
            /// Шаги интегрирования в кратном интеграле по умолчанию
            /// </summary>
            public static double stx = 0.001, sty = 0.005;
            /// <summary>
            /// Число колец по умолчанию
            /// </summary>
            public static int countY = 100;

            /// <summary>
            /// Методы подсчёта интеграда
            /// </summary>
            public enum Method
            {
                /// <summary>
                /// Метод средних прямоугольников
                /// </summary>
                MiddleRect,
                /// <summary>
                /// Метод трапеций
                /// </summary>
                Trapez,
                /// <summary>
                /// Метод Симпсона
                /// </summary>
                Simpson,
                /// <summary>
                /// Метод Гаусса
                /// </summary>
                Gauss,
                /// <summary>
                /// Метод Мелера (Эрмита)
                /// </summary>
                Meler,
                /// <summary>
                /// Метода Гаусса-Кронрода(по 15 точкам)
                /// </summary>
                GaussKronrod15,
                /// <summary>
                /// Метод Гаусса-Кронрода по 61 точке
                /// </summary>
                GaussKronrod61,
                /// <summary>
                /// Метод Гаусса-Кронрода по 61 точкам на основе процедуры с фортрана
                /// </summary>
                GaussKronrod61fromFortran,
                /// <summary>
                /// Метод Гаусса-Кронрода по 61 точке с эмпирическим делением отрезка
                /// </summary>
                GaussKronrod61Empire

            };
            /// <summary>
            /// Критерии подсчёта интеграла
            /// </summary>
            public enum Criterion
            {
                /// <summary>
                /// Число шагов (узлов)
                /// </summary>
                StepCount,
                /// <summary>
                /// Точность
                /// </summary>
                Accuracy,
                /// <summary>
                /// Разбиение на несколько отрезков
                /// </summary>
                SegmentCount
            };

            /// <summary>
            /// Нахождение определённого интеграла методом средних прямоугольников
            /// </summary>
            /// <param name="F">Действительная функция действительного аргумента</param>
            /// <param name="a">Первая точка промежутка интегрирования</param>
            /// <param name="b">Последняя точка промежутка интегрирования</param>
            /// <returns></returns>
            public static double MiddleRect(RealFunc F, double a, double b)//метод средних прямоугольников
            {
                double result = 0, h = STEP;
                //h = (b - a) / n; //Шаг сетки

                for (int i = 0; i < (int)((b - a) / h); i++)
                {
                    result += F(a + h * (i + 0.5)); //вычисляем в средней точке и добавляем в сумму
                }
                result *= h;
                return result;
            }
            /// <summary>
            /// Нахождение определённого интеграла методом трапеций
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double Trapez(RealFunc f, double a, double b)//метод трапеций
            {
                double h = STEP, result = 0;
                int k = ((int)((b - a) / h) /*-1*/);

                result += (f(a) + f(b)) / 2;

                for (int i = 1; i < k; i++) result += f(a + i * h);

                return h * result;
            }
            /// <summary>
            /// Нахождение определённого интеграла методом Симпсона
            /// </summary>
            /// <param name="F"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double Simpson(RealFunc F, double a, double b)//метод Симпсона
            {
                double step = (b - a) / (double)h_Count;
                double S = 0, x, h = step;
                int i = 1;
                //отрезок [a, b] разобьем на N частей
                //h = (b - a) / N;
                x = a + h;
                while (i < h_Count && x < b)
                {
                    S += 4 * F(x); //Console.WriteLine($"x={x}  4F(x)={4*F(x)}");
                    x += h; i++;
                    //проверяем не вышло ли значение x за пределы полуинтервала [a, b)
                    if (x >= b || i == h_Count)
                    {
                        S = (h / 3) * (S + F(a) + F(b));

                        return S;
                    }

                    S += 2 * F(x); //Console.WriteLine($"x={x}  2F(x)={2 *F(x)}");
                    x += h; i++;
                }
                //Console.WriteLine(S +" "+b);
                S = (h / 3) * (S + F(a) + F(b));

                return S;
            }
            /// <summary>
            /// Нахождение определённого интеграла через квадратурные формулы Гаусса
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="a">Начало отрезка итегрирования</param>
            /// <param name="b">Конец отрезка интегрирования</param>
            /// <returns></returns>
            public static double Gauss(RealFunc f, double a, double b)
            {
                RealFunc fi = (double t) => { return f((a + b) / 2 + (b - a) / 2 * t); };//замена координат
                double sum = 0;
                Polynom p = Polynom.Lezh(DefInteg.n);//полином Лежандра
                Vectors root;
                if (RootGauss == null || RootGauss.Deg != n)
                {
                    root = new Vectors(n);

                    for (int i = 1; i <= DefInteg.n; i++)//поиск корней многочлена Лежандра
                    {
                        root[i - 1] = Math.Cos(Math.PI * (4 * i - 1) / (4 * n + 2));
                        for (int j = 0; j < iter_count; j++)
                        {
                            Polynom tmp = p | 1;
                            root[i - 1] -= p.Value(root[i - 1]) / tmp.Value(root[i - 1]);
                        }
                    }
                    RootGauss = root;
                }
                else root = new Vectors(RootGauss);


                //root.Show();

                for (int i = 1; i <= DefInteg.n; i++)//суммирование
                {
                    //поиск приведённого многочлена
                    Polynom Ap = (new Polynom(1, root)) / (new Polynom(root[i - 1]));

                    sum += Ap.S(-1, 1) * fi(root[i - 1]) / Ap.Value(root[i - 1]);
                }

                return (b - a) / 2 * sum;
            }

            private static Vectors RootMeler = null;
            private static Vectors RootGauss = null;
            /// <summary>
            /// Нахождение определённого интеграла через квадратурные формулы Мелера
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="a">Начало отрезка итегрирования</param>
            /// <param name="b">Конец отрезка интегрирования</param>
            /// <returns></returns>
            public static double Meler(RealFunc f, double a, double b)
            {
                RealFunc fi = (double t) => { return f((a + b) / 2 + (b - a) / 2 * t); };//замена координат
                double sum = 0;
                Vectors root;
                int n = DefInteg.n;
                //if (RootMeler == null || RootMeler.Deg!=n)
                //{
                root = new Vectors(n);

                for (int i = 1; i <= n; i++)//поиск корней многочлена Чебышева
                {
                    root[i - 1] = Math.Cos(Math.PI * (2 * i - 1) / (2 * n));
                }
                // RootMeler = root;
                //}
                //else root = new Vectors(RootMeler);


                //root.Show();

                for (int i = 1; i <= n; i++)//суммирование
                {
                    sum += fi(root[i - 1]) * Math.Sqrt(1 - root[i - 1] * root[i - 1]);
                }

                return (b - a) / 2 * Math.PI / n * sum;
            }
            /// <summary>
            /// Класс методов, связанных с вычислением интеграла квадратурами Гаусса-Кронрода
            /// </summary>
            public static class GaussKronrod
            {
                static GaussKronrod()
                {
                    MyGKInit();
                    GK_7_15_init();
                    List<string> r = new List<string>(); r.Add("");
                    MasListDinnInfo[0] = new List<string>(r);
                    MasListDinnInfo[1] = new List<string>(r);
                    MasListDinnInfo[2] = new List<string>(r);
                    MasListDinnInfo[3] = new List<string>(r);
                }
                /// <summary>
                /// Процедурная реализация вектор-функции комплексного аргумента
                /// </summary>
                /// <param name="x">Аргумент</param>
                /// <param name="y">Вектор значений</param>
                /// <param name="N">Размерность вектора значений</param>
                public delegate void Myfunc(Complex x, ref Complex[] y, int N);
                /// <summary>
                /// Комплексная вектор-функция
                /// </summary>
                /// <param name="x"></param>
                /// <param name="N"></param>
                /// <returns></returns>
                public delegate Complex[] ComplexVectorFunc(Complex x, int N);

                /// <summary>
                /// Количество узлов при интегрировании
                /// </summary>
                public enum NodesCount
                {
                    GK15, GK21, GK31, GK41, GK51, GK61
                }

                private static double[] GetNew(double[] x)
                {
                    double[] res = new double[x.Length + 1];
                    for (int i = 1; i <= x.Length; i++)
                        res[i] = x[i - 1];
                    return res;
                }
                /// <summary>
                /// Выбрать метод в зависимости от узлов
                /// </summary>
                /// <param name="c"></param>
                internal static void ChooseGK(NodesCount c)
                {
                    switch (c)
                    {
                        //case NodesCount.GK15:
                        //    GK_nodes = GetNew(x15);
                        //    K_weights = GetNew(wkronrod15);
                        //    G_weights = GetNew(wgauss15);
                        //    Nodes = 15;
                        //    break;
                        //case NodesCount.GK21:
                        //    GK_nodes = GetNew(x21);
                        //    K_weights = GetNew(wkronrod21);
                        //    G_weights = GetNew(wgauss21);
                        //    Nodes = 21;
                        //    break;
                        //case NodesCount.GK31:
                        //    GK_nodes = GetNew(x31);
                        //    K_weights = GetNew(wkronrod31);
                        //    G_weights = GetNew(wgauss31);
                        //    Nodes = 31;
                        //    break;
                        //case NodesCount.GK41:
                        //    GK_nodes = GetNew(x41);
                        //    K_weights = GetNew(wkronrod41);
                        //    G_weights = GetNew(wgauss41);
                        //    Nodes = 41;
                        //    break;
                        //case NodesCount.GK51:
                        //    GK_nodes = GetNew(x51);
                        //    K_weights = GetNew(wkronrod51);
                        //    G_weights = GetNew(wgauss51);
                        //    Nodes = 51;
                        //    break;
                        //case NodesCount.GK61:
                        //    GK_nodes = GetNew(x61);
                        //    K_weights = GetNew(wkronrod61);
                        //    G_weights = GetNew(wgauss61);
                        //    Nodes = 61;
                        //    break;
                        case NodesCount.GK15:
                            GK_nodes = _x15;
                            K_weights = _wkronrod15;
                            G_weights = _wgauss15;
                            Nodes = 15;
                            break;
                        case NodesCount.GK21:
                            GK_nodes = _x21;
                            K_weights = _wkronrod21;
                            G_weights = _wgauss21;
                            Nodes = 21;
                            break;
                        case NodesCount.GK31:
                            GK_nodes = _x31;
                            K_weights = _wkronrod31;
                            G_weights = _wgauss31;
                            Nodes = 31;
                            break;
                        case NodesCount.GK41:
                            GK_nodes = _x41;
                            K_weights = _wkronrod41;
                            G_weights = _wgauss41;
                            Nodes = 41;
                            break;
                        case NodesCount.GK51:
                            GK_nodes = _x51;
                            K_weights = _wkronrod51;
                            G_weights = _wgauss51;
                            Nodes = 51;
                            break;
                        case NodesCount.GK61:
                            GK_nodes = _x61;
                            K_weights = _wkronrod61;
                            G_weights = _wgauss61;
                            Nodes = 61;
                            break;
                    }
                }

                /// <summary>
                /// Размерность
                /// </summary>
                static int Nodes = 15, Nodes61 = 61;
                static bool Key = false;
                static double RV_eps_step_increment, norm_param = 1;
                static double[] GK_nodes, K_weights, G_weights, GK_nodes61, K_weights61, G_weights61;
                static double h = 0.1;
                static double eps = 0.001;

                static void GK_7_15_init()
                {
                    GK_nodes = new double[Nodes + 1];
                    K_weights = new double[Nodes + 1];
                    G_weights = new double[Nodes + 1];

                    GK_nodes[1] = -0.991455371120813; GK_nodes[2] = -0.949107912342759;
                    GK_nodes[3] = -0.864864423359769; GK_nodes[4] = -0.741531185599394;
                    GK_nodes[5] = -0.586087235467691; GK_nodes[6] = -0.405845151377397;
                    GK_nodes[7] = -0.207784955007898;
                    GK_nodes[8] = 0;
                    GK_nodes[9] = 0.207784955007898; GK_nodes[10] = 0.405845151377397;
                    GK_nodes[11] = 0.586087235467691; GK_nodes[12] = 0.741531185599394;
                    GK_nodes[13] = 0.864864423359769; GK_nodes[14] = 0.949107912342759;
                    GK_nodes[15] = 0.991455371120813;
                    K_weights[1] = 0.022935322010529; K_weights[2] = 0.063092092629979;
                    K_weights[3] = 0.104790010322250; K_weights[4] = 0.140653259715525;
                    K_weights[5] = 0.169004726639267; K_weights[6] = 0.190350578064785;
                    K_weights[7] = 0.204432940075298;
                    K_weights[8] = 0.209482141084728;
                    K_weights[9] = 0.204432940075298; K_weights[10] = 0.190350578064785;
                    K_weights[11] = 0.169004726639267; K_weights[12] = 0.140653259715525;
                    K_weights[13] = 0.104790010322250; K_weights[14] = 0.063092092629979;
                    K_weights[15] = 0.022935322010529;

                    G_weights[1] = 0; G_weights[2] = 0.129484966168870;
                    G_weights[3] = 0; G_weights[4] = 0.279705391489277;
                    G_weights[5] = 0; G_weights[6] = 0.381830050505119;
                    G_weights[7] = 0;
                    G_weights[8] = 0.417959183673469;
                    G_weights[9] = 0; G_weights[10] = 0.381830050505119;
                    G_weights[11] = 0; G_weights[12] = 0.279705391489277;
                    G_weights[13] = 0; G_weights[14] = 0.129484966168870;
                    G_weights[15] = 0;
                    RV_eps_step_increment = 1e-8;

                    //my 61
                    //GK_nodes61 = new double[Nodes61 + 1];
                    //K_weights61 = new double[Nodes61 + 1];
                    //G_weights61 = new double[Nodes61 + 1];
                    //for(int i=1;i<=61;i++)
                    //{
                    //    GK_nodes61[i] = x61[i-1];
                    //    K_weights61[i] = wkronrod61[i-1];
                    //    G_weights61[i] = wgauss61[i-1];
                    //}
                }
                /// <summary>
                /// Интеграл по коченому отрезку
                /// </summary>
                /// <param name="int_func">Интегрируемая функция</param>
                /// <param name="a">Начало отрезна интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <param name="int_h">Начальный шаг</param>
                /// <param name="eps">Используемая погрешность</param>
                /// <param name="ret_arr">Вектор значений интеграла (результат)</param>
                /// <param name="N">Размерность вектора значений</param>
                public static void GK_adaptive_int(Myfunc int_func, Complex a, Complex b, double int_h, double eps, ref Complex[] ret_arr, int N)
                {
                    double eps_out = 0;
                    Complex t_i_h, t_x;
                    Complex[] ret_arr_0 = new Complex[N];
                    ret_arr = new Complex[N];
                    t_x = new Complex(a);
                    t_i_h = new Complex(int_h, 0);

                    if (Math.Abs(Complex.Imag(b) - Complex.Imag(a)) > eps)
                    {
                        if (Complex.Imag(b) < Complex.Imag(a))
                            t_i_h = new Complex(0, -int_h);
                        else
                            t_i_h = new Complex(0, int_h);
                    }

                    while (Math.Abs((double)(b - t_x)) > eps)
                    {
                        if (Math.Abs((double)(t_x + t_i_h)) > Math.Abs((double)b)) t_i_h = b - t_x;
                        GK_int(int_func, t_x, t_x + t_i_h, ref ret_arr_0, ref eps_out, N);//if(!Double.IsNaN(ret_arr_0[0].Abs))ret_arr_0.Show();

                        if (eps_out > eps)
                        {
                            t_i_h = t_i_h * 0.5;
                        }
                        else
                        {
                            try
                            {
                                ret_arr = Complex.Sum(ret_arr, ret_arr_0);
                                t_x = t_x + t_i_h;
                                //Console.WriteLine(t_x);
                                if (eps_out < 1e-3 * eps) t_i_h = t_i_h * 1.75;

                            }
                            catch (Exception e) { throw new Exception(e.Message); }
                            Vectors v = new Vectors(N);
                            for (int i = 0; i < v.n; i++) v[i] = Math.Abs((double)ret_arr_0[i]);
                            if ((v.Max) / norm_param < 2.5e-5) return;
                        }
                    }

                    int_h = Math.Abs((double)t_i_h);
                }
                /// <summary>
                /// Интеграл по коченому отрезку
                /// </summary>
                /// <param name="int_func">Интегрируемая функция</param>
                /// <param name="a">Начало отрезна интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <param name="int_h">Начальный шаг</param>
                /// <param name="eps">Используемая погрешность</param>
                /// <param name="N">Размерность вектора значений</param>
                public static Complex[] Integral(Myfunc int_func, Complex a, Complex b, double int_h, double eps, int N)
                {
                    Complex[] y = new Complex[0];
                    try { GK_adaptive_int(int_func, a, b, int_h, eps, ref y, N); } catch { throw new Exception("тут"); }
                    return y;
                }
                /// <summary>
                /// Интеграл по прямой
                /// </summary>
                /// <param name="int_func">Интегрируемая функция</param>
                /// <param name="a">Начало отрезна интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <param name="int_h">Начальный шаг</param>
                /// <param name="eps">Используемая погрешность</param>
                /// <param name="N">Размерность вектора значений</param>
                public static Complex IntegralInf(ComplexFunc f, Complex a, Complex b, int tt = 100, int n = 15, ComplexFunc delta = null, int maxmult = 12, double vareps = 1e-6, double maxarg = 140)
                {
                    double epss = vareps;
                    Complex sum = MySuperGaussKronrod(f, a, b, delta, tt, n);//a.Show();b.Show();int_h.Show();eps.Show();
                    Complex p = new Complex(sum); if (Double.IsNaN(sum.Re)) $"Такая байда {sum} \t f({a})={f(a)} \t f({b})={f(b)} \t d({a})={delta(a)} \t d({b})={delta(b)}".Show();

                    Complex ab = b - a;//ab.Show();
                    int t = 1;
                    while (t <= maxmult && b.Abs < maxarg)//идти по всем множителям шага
                    {
                        //Console.WriteLine($"p={p} t={t} norm={p.Abs}");
                        Complex s1 = MySuperGaussKronrod(f, a - ab, a, delta, tt, n), s2 = MySuperGaussKronrod(f, b, b + ab, delta, tt, n);
                        p = s1 + s2;
                        a -= ab;
                        b += ab;
                        ab *= 2;
                        sum += p; if (Double.IsNaN(sum.Re)) $"Такая байда {sum} \t f({a})={f(a)} \t f({b})={f(b)} \t d({a})={delta(a)} \t d({b})={delta(b)}".Show();
                        while (p.Abs > epss && b.Abs < maxarg)//пока добавление больше погрешности
                        {
                            //Console.WriteLine($"p={p} t={t} norm={p.Abs}");
                            s1 = MySuperGaussKronrod(f, a - ab, a, delta, tt, n); s2 = MySuperGaussKronrod(f, b, b + ab, delta, tt, n);
                            p = s1 + s2;
                            a -= ab;
                            b += ab;
                            sum += p; if (Double.IsNaN(sum.Re)) $"Такая байда {sum} \t f({a})={f(a)} \t f({b})={f(b)} \t d({a})={delta(a)} \t d({b})={delta(b)}".Show();
                        }
                        //p = sum;//t.Show();
                        t++;
                    }

                    return sum;
                }
                /// <summary>
                /// Интеграл по отрезку от a до +inf
                /// </summary>
                /// <param name="f"></param>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <param name="n"></param>
                /// <param name="maxmult"></param>
                /// <param name="vareps"></param>
                /// <returns></returns>
                public static Complex IntegralHalfInf(ComplexFunc f, double a, double begH, int tt = 100, int n = 15, ComplexFunc delta = null, int maxmult = 12, double vareps = 1e-6, double h = 0.2)
                {
                    double epss = vareps;
                    Complex sum = MySuperGaussKronrod(f, a, a + begH, delta, tt, n, h);//a.Show();b.Show();int_h.Show();eps.Show();
                    Complex p = new Complex(sum); if (Double.IsNaN(sum.Re)) { $"1 Такая байда {sum} \t f({a})={f(a)} \t f({a + begH})={f(a + begH)} \t d({a})={delta(a)} \t d({a + begH})={delta(a + begH)}".Show(); Console.ReadKey(); }

                    int t = 1; a += begH;
                    while (t <= maxmult && a < tt)//идти по всем множителям шага
                    {
                        //Console.WriteLine($"p={p} t={t} norm={p.Abs}");
                        p = MySuperGaussKronrod(f, a, a + begH, delta, tt, n, h); if (Double.IsNaN(p.Re)) { $"2 Такая байда {p} \t f({a})={f(a)} \t f({a + begH})={f(a + begH)} \t d({a})={delta(a)} \t d({a + begH})={delta(a + begH)}".Show(); Console.ReadKey(); }
                        a += begH;
                        begH *= 2;
                        sum += p;
                        while (p.Abs > epss && a < tt)//пока добавление больше погрешности
                        {
                            //Console.WriteLine($"p={p} t={t} norm={p.Abs}");
                            p = MySuperGaussKronrod(f, a, a + begH, delta, tt, n, h);
                            if (Double.IsNaN(p.Re) || Double.IsInfinity(p.Re))
                            {
                                $"3 При суммировании интегралов промужуточный интеграл равен = {MySuperGaussKronrod(f, a, a + begH, delta, tt, n, h)} \t f({a})={f(a)} \t f({a + begH})={f(a + begH)} \t d({a})={delta(a)} \t d({a + begH})={delta(a + begH)}".Show();
                                Console.ReadKey();
                            }
                            a += begH;
                            begH *= 2;
                            sum += p;
                        }
                        //p = sum;//t.Show();
                        t++;
                    }

                    return sum;
                }
                /// <summary>
                /// Определённый интеграл
                /// </summary>
                /// <param name="f">Интегрируемая функция</param>
                /// <param name="a">Начало отрезка интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <returns></returns>
                public static double Integral(RealFunc f, double a, double b)
                {
                    Myfunc z = (Complex x, ref Complex[] t, int N) => { t = new Complex[1]; t[0] = f(x.Re); };
                    Complex[] y = Integral(z, a, b, h, eps, 1);
                    return y[0].Re;
                }
                /// <summary>
                /// Несобственный интеграл по вещественной оси
                /// </summary>
                /// <param name="f"></param>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <returns></returns>
                public static double IntegralInf(ComplexFunc f, double a, double b, ComplexFunc delta = null, int t = 100, int n = 15, double vareps = 1e-6, double maxarg = 150)
                {
                    Complex y = IntegralInf(f, new Complex(a), new Complex(b), t, n, delta, 3, vareps, maxarg);
                    return y.Re;
                }

                static void GK_adaptive_int_inf(Myfunc int_func, Complex a, Complex b, ref double int_h, ref double eps, ref Complex[] ret_arr, int N)
                {
                    int i, ipr;
                    bool it;
                    double[] temp_arr = new double[N /*+ 1*/];
                    double t1, eps10, pm, pt, t, int_h_1, t_h;
                    Complex t_x_a, t_x_b;
                    Complex[] ret_arr_0 = new Complex[N /*+ 1*/];
                    ret_arr = new Complex[N /*+ 1*/];
                    t_x_b = new Complex(a);
                    t = Math.Abs((double)(b - a));
                    eps10 = eps * 10;
                    ipr = 0;
                    it = true;
                    while (it)
                    {
                        t_x_a = new Complex(t_x_b); t_x_b = t_x_b + int_h; t1 = Math.Abs((double)(t_x_b - a));
                        if (t1 > t)
                        {
                            t_x_b = b;
                            it = false;
                        }
                        int_h_1 = int_h;
                        GK_adaptive_int(int_func, t_x_a, t_x_b, int_h_1, eps, ref ret_arr_0, N);
                        Vectors v = new Vectors(N);

                        for (int j = 0; j < v.n; j++) v[j] = Math.Abs((double)ret_arr_0[j /*+ 1*/]);
                        if ((v.Min < 1e-10) && (Math.Abs((double)t_x_a) > 50.0) && (v.Max < 1e-7)) return;
                        try
                        {
                            if (v.Max / norm_param < 1e-8) return;

                            t_h = Math.Abs((double)(t_x_b - t_x_a));
                            temp_arr = (double[])(v / t_h);

                            ret_arr = Complex.Sum(ret_arr, ret_arr_0);

                            if (Math.Abs(int_h) < 10 * Math.Abs(int_h_1))
                            {
                                int_h = 4 * int_h;
                            }
                            else int_h = 4 * int_h_1;
                            // at infinity
                            pm = 0;
                            for (i = 0/*1*/; i </*=*/ N; i++)
                            {
                                if (Math.Abs((double)ret_arr[i]) > 1e-15)
                                {
                                    pt = Math.Abs((double)(temp_arr[i] * t1 / ret_arr[i]));
                                    if (pt > pm) pm = pt;
                                }
                            }

                            if (pm < eps10)
                            {
                                ipr = ipr + 1;
                                if (ipr > 4) return;
                            }
                            else
                            {
                                ipr = 0;
                            }
                        }
                        catch { throw new Exception(""); }
                    }
                }

                static void GK_int(Myfunc int_func, Complex a, Complex b, ref Complex[] ret_arr, ref double eps_out, int N)
                {
                    //ChooseGK(nodesCount);

                    // implicit none;
                    int i;
                    Complex[] GK_nodes_arb = new Complex[Nodes + 1], K_weights_arb = new Complex[Nodes + 1], G_weights_arb = new Complex[Nodes + 1];
                    Complex[,] temp_arr = new Complex[N + 1, 3];

                    GK_nodes_arb = Complex.Sum(Complex.Mult(0.5 * (b - a), GK_nodes), 0.5 * (b + a));
                    K_weights_arb = Complex.Mult((b - a), (double[])((Vectors)K_weights * 0.5));
                    G_weights_arb = Complex.Mult((b - a), (double[])((Vectors)G_weights * 0.5));

                    for (i = 1; i <= Nodes; i++)
                    {
                        int_func(GK_nodes_arb[i], ref ret_arr, N);
                        //ret_arr.Show();
                        //K_weights_arb.Show();
                        //temp_arr[0, 0].Show();

                        for (int j = 1; j <= N; j++)
                        {
                            try
                            {
                                //Console.WriteLine(j + " <= " + N);
                                temp_arr[j, 1] += K_weights_arb[i] * ret_arr[j - 1];
                                temp_arr[j, 2] += G_weights_arb[i] * ret_arr[j - 1];
                            }
                            catch (Exception e) { throw new Exception(e.Message); }
                        }

                    }

                    Vectors v = new Vectors(N);
                    for (int j = 1; j <= N; j++)
                        v[j - 1] = Math.Abs((double)(temp_arr[j, 1] - temp_arr[j, 2]));
                    eps_out = v.Max;
                    for (int j = 1; j <= N; j++)
                        ret_arr[j - 1] = new Complex(temp_arr[j, 1]);
                }

                public static List<string>[] MasListDinnInfo = new List<string>[4];
                /// <summary>
                /// Информация о последнем найденном с помощью DINN5_GK интеграле 
                /// </summary>
                private static List<string> LastListOfDINN5GK;
                /// <summary>
                /// Базовый DINN с фортрана
                /// </summary>
                /// <param name="CF"></param>
                /// <param name="t1"></param>
                /// <param name="t2"></param>
                /// <param name="t3"></param>
                /// <param name="t4"></param>
                /// <param name="tm"></param>
                /// <param name="tp"></param>
                /// <param name="eps"></param>
                /// <param name="pr"></param>
                /// <param name="gr"></param>
                /// <param name="N"></param>
                /// <param name="Rd"></param>
                /// <remarks>
                /// ! Программа вычисления N интегралов по полубесконечному контуру 
                ///!           в случае обратной волны 
                ///!
                ///!  subroutine СF(u, s, n) - подынтегральные функции; u - аргумент(complex16),
                ///!                   s(n) - массив значений функций в точке u(complex16),
                ///!                   n - число интегралов(integer)
                ///! [t1, t2],[t3, t4] - участки отклонения контура вниз(real8)
                ///!         [t2, t3] - участок отклонения контура вверх(real8)
                ///! tm,tp > 0 - величины отклонения контура вниз и вверх(real8)
                ///! (если нет обратной волны, то следует положить t2 = t3 = t1, tp = 0 
                ///!  обход полюсов при этом будет только снизу на участке[t1, t4]
                ///!  с отклонением на tm)
                ///! eps -  отн.погрешность,  pr - начальный шаг,
                ///! N- число интегралов(integer)
                ///! Rd(N) - выход: массив значений интегралов
                /// </remarks>
                static void DINN5_GK(Myfunc CF, double t1, double t2, double t3, double t4, double tm, double tp, double eps, double pr, double gr, int N, out Complex[] Rd)
                {
                    // implicit none
                    int i;
                    double int_h;
                    Complex a, b;
                    Complex[] sb = new Complex[N];

                    //GK_7_15_init();
                    Rd = new Complex[N];
                    if (t1 < t4)
                    {
                        // [0, t1]
                        a = 0; b = t1; int_h = 0.25 * Math.Abs((double)(b - a));
                        GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                        Rd = Complex.Sum(Rd, sb);//Rd.Show();
                        //LastListOfDINN5GK.Add($"\tНа участке [0,t1] GK_adaptive_int = {sb[0]}");
                        if (t3 - t2 < eps)
                        { // no inverse poles case
                            a = b; b = new Complex(t1, -tm); int_h = 0.25 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            // LastListOfDINN5GK.Add($"\tНа участке [t1,t1-tm*i] GK_adaptive_int = {sb[0]}");

                            a = b; b = new Complex(t4, -tm); int_h = 0.05 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            //LastListOfDINN5GK.Add($"\tНа участке [t1-tm*i,t4-tm*i] GK_adaptive_int = {sb[0]}");

                            a = b; b = t4; int_h = 0.25 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            //LastListOfDINN5GK.Add($"\tНа участке [t4-tm*i,t4] GK_adaptive_int = {sb[0]}");
                        }
                        else
                        {
                            if (t2 - t1 > eps)
                            {
                                // first deviation from below

                                a = b; b = new Complex(t1, -tm); int_h = 0.5 * Math.Abs((double)(b - a));
                                GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                                Rd = Complex.Sum(Rd, sb);/*Rd.Show();*/
                                //LastListOfDINN5GK.Add($"\tНа участке [t1,-tm*i] GK_adaptive_int = {sb[0]}");

                                a = b; b = new Complex(t2, -tm); int_h = 0.5 * Math.Abs((double)(b - a));
                                GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                                Rd = Complex.Sum(Rd, sb);
                                //LastListOfDINN5GK.Add($"\tНа участке [-tm,t2-tm*i] GK_adaptive_int = {sb[0]}");
                            }

                            // diviation from above

                            a = b; b = new Complex(t2, tp); int_h = 0.5 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            //LastListOfDINN5GK.Add($"\tНа участке [t1 либо t2-tm*i,t2+tp] GK_adaptive_int = {sb[0]}");

                            a = b; b = new Complex(t3, tp); int_h = 0.5 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            // LastListOfDINN5GK.Add($"\tНа участке [t2+tp,t3+tp*i] GK_adaptive_int = {sb[0]}");

                            a = b; b = new Complex(t3, -tm); int_h = 0.5 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            //LastListOfDINN5GK.Add($"\tНа участке [t3+tp*i,t3-tm*i] GK_adaptive_int = {sb[0]}");

                            // second diviation from below
                            a = b; b = new Complex(t4, -tm); int_h = 0.25 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            //LastListOfDINN5GK.Add($"\tНа участке [t3-tm*i,t4-tm*i] GK_adaptive_int = {sb[0]}");

                            a = b; b = t4; int_h = 0.5 * Math.Abs((double)(b - a));
                            GK_adaptive_int(CF, a, b, int_h, eps, ref sb, N);
                            Rd = Complex.Sum(Rd, sb);
                            //LastListOfDINN5GK.Add($"\tНа участке [t4-tm*i,t4] GK_adaptive_int = {sb[0]}");
                        }
                        Vectors v = new Vectors(N);
                        for (i = 0; i < v.n; i++)
                            v[i] = Math.Abs((double)Rd[i]);
                        norm_param = v.Max;
                        // Console.WriteLine(norm_param);
                    }

                    if (gr > t4)
                    {
                        a = t4;
                        b = gr;
                        int_h = 0.33; //0.25*Math.Abs((double)(b-a));
                        if (Key)
                        {
                            GK_adaptive_int_inf(CF, a, b, ref int_h, ref eps, ref sb, N);
                            //LastListOfDINN5GK.Add($"\tНа участке gr > t4 GK_adaptive_int_inf = {sb[0]}");
                        }
                        else
                        {
                            GK_adaptive_int(CF, a, b, int_h, eps * 10, ref sb, N);
                            //LastListOfDINN5GK.Add($"\tНа участке gr > t4 GK_adaptive_int = {sb[0]}");

                        }
                        Rd = Complex.Sum(Rd, sb);

                    }
                    //LastListOfDINN5GK.Add("");
                    //for(int o=0;o<3;o++)
                    //MasListDinnInfo[o] = new List<string>(MasListDinnInfo[o+1]);
                    //MasListDinnInfo[3] = LastListOfDINN5GK;
                }
                public static Complex[] DINN5_GK(ComplexVectorFunc f, double t1, double t2, double t3, double t4, double tm, double tp, double eps, double pr, double gr, int N, NodesCount nodesCount = NodesCount.GK15)
                {
                    ChooseGK(nodesCount);
                    Complex[] Result;
                    Myfunc ff = (Complex x, ref Complex[] y, int n) => y = f(x, n);
                    DINN5_GK(ff, t1, t2, t3, t4, tm, tp, eps, pr, gr, N, out Result);
                    return Result;
                }
                /// <summary>
                /// Подсчёт несобственного комплексного интеграла от 0 до inf с учётом полюсов
                /// </summary>
                /// <param name="f">Функция комплексного переменного</param>
                /// <param name="t1"></param>
                /// <param name="t2"></param>
                /// <param name="t3"></param>
                /// <param name="t4"></param>
                /// <param name="tm">Величина отклонения контура вниз</param>
                /// <param name="tp">Величина отклонения контура вверх</param>
                /// <param name="eps">Погрешность</param>
                /// <param name="pr">Начальный шаг</param>
                /// <param name="gr">Верхний предел</param>
                /// <remarks>
                /// ВЗЯТО С ДОКУМЕНТАЦИИ ОТ ФОРТРАНА
                /// ! [t1,t2],[t3,t4] - участки отклонения контура вниз (real8)
                ///!         [t2, t3] - участок отклонения контура вверх(real8)
                ///! tm,tp > 0 - величины отклонения контура вниз и вверх(real8)
                ///! (если нет обратной волны, то следует положить t2 = t3 = t1, tp = 0 
                ///!  обход полюсов при этом будет только снизу на участке[t1, t4]
                ///!  с отклонением на tm)
                /// </remarks>
                /// <returns></returns>
                public static Complex DINN_GK(ComplexFunc f, double t1, double t2, double t3, double t4, double tm, double tp = 0, double eps = 1e-4, double pr = 1e-2, double gr = 1e4, NodesCount nodesCount = NodesCount.GK15)
                {
                    ComplexVectorFunc ff = (Complex x, int n) => new Complex[] { f(x) };
                    return DINN5_GK(ff, t1, t2, t3, t4, tm, tp, eps, pr, gr, 1, nodesCount)[0];
                }
                /// <summary>
                /// Несобственный интеграл с нулевыми параметрами (если не надо делать обход контура)
                /// </summary>
                /// <param name="f"></param>
                /// <returns></returns>
                public static Complex DINN_GKwith0(ComplexFunc f, double eps = 1e-4, NodesCount nodesCount = NodesCount.GK15) => DINN_GK(f, 0, 0, 0, 0, 0, 0, eps, nodesCount: nodesCount);
                /// <summary>
                /// Несобственный интеграл по всей оси от конкретно этой функции
                /// </summary>
                /// <param name="f"></param>
                /// <returns></returns>
                public static Complex DINN_GKwith0Full(ComplexFunc f, double eps = 1e-4, NodesCount nodesCount = NodesCount.GK15)
                {
                    ComplexFunc f3 = t => f(t) + f(-t);
                    return DINN_GKwith0(f3, eps, nodesCount);
                }

                static void GK_adaptive_int_RealV(Myfunc int_func, double a, double b, double int_h, double eps, Complex[] ret_arr, int N)
                {
                    // implicit none;
                    double eps_out = 0, t_i_h, t_x;
                    Complex[] ret_arr_0 = new Complex[N + 1];

                    t_x = a;
                    t_i_h = int_h;
                    while (b - t_x > eps)
                    {
                        if (t_x + t_i_h > b) t_i_h = b - t_x;

                        GK_int_RealV(int_func, t_x, t_x + t_i_h, ret_arr_0, eps_out, N);
                        if (eps_out > eps)
                        {
                            t_i_h = t_i_h * 0.5;
                        }
                        else
                        {
                            ret_arr = Complex.Sum(ret_arr, ret_arr_0);
                            t_x = t_x + t_i_h;
                            //   Console.WriteLine(t_x);
                            if (eps_out < RV_eps_step_increment) t_i_h = t_i_h * 1.5;
                        }
                    }

                    int_h = t_i_h;
                }

                static void GK_int_RealV(Myfunc int_func, double a, double b, Complex[] ret_arr, double eps_out, int N)
                {
                    // implicit none;
                    int i;
                    double[] GK_nodes_arb = new double[Nodes + 1], K_weights_arb = new double[Nodes + 1], G_weights_arb = new double[Nodes + 1];

                    Complex[,] temp_arr = new Complex[N + 1, 3];


                    GK_nodes_arb = (double[])((Vectors)(GK_nodes) * 0.5 * (b - a) + 0.5 * (b + a));
                    K_weights_arb = (double[])((Vectors)(K_weights) * 0.5 * (b - a));
                    G_weights_arb = (double[])((Vectors)(G_weights) * 0.5 * (b - a));

                    for (i = 1; i <= Nodes; i++)
                    {
                        int_func(GK_nodes_arb[i], ref ret_arr, N);
                        for (int j = 1; j <= N; j++)
                        {
                            temp_arr[j, 1] = temp_arr[j, 1] + K_weights_arb[i] * ret_arr[j - 1];
                            temp_arr[j, 2] = temp_arr[j, 2] + G_weights_arb[i] * ret_arr[j - 1];
                        }
                    }

                    for (i = 0; i < N; i++)
                        ret_arr[i] = temp_arr[i, 1] - temp_arr[i, 2];

                    Vectors v = new Vectors(N);
                    for (i = 0; i < N; i++)
                        v[i] = ret_arr[i].Abs;
                    eps_out = (200 * v.Max) * 1.5;
                    for (i = 0; i < N; i++)
                        ret_arr[i] = temp_arr[i, 1];
                }

                private static double[] x15, x21, x31, x41, x51, x61, wgauss15, wgauss21, wgauss31, wgauss41, wgauss51, wgauss61, wkronrod15, wkronrod21, wkronrod31, wkronrod41, wkronrod51, wkronrod61, _x15, _x21, _x31, _x41, _x51, _x61, _wgauss15, _wgauss21, _wgauss31, _wgauss41, _wgauss51, _wgauss61, _wkronrod15, _wkronrod21, _wkronrod31, _wkronrod41, _wkronrod51, _wkronrod61;
                private static int ng15 = 4, ng21 = 5, ng31 = 8, ng41 = 10, ng51 = 13, ng61 = 15;
                private static void MyGKInit()
                {
                    x15 = new double[15];
                    x21 = new double[21];
                    x31 = new double[31];
                    x41 = new double[41];
                    x51 = new double[51];
                    x61 = new double[61];
                    wgauss15 = new double[15];
                    wgauss21 = new double[21];
                    wgauss31 = new double[31];
                    wgauss41 = new double[41];
                    wgauss51 = new double[51];
                    wgauss61 = new double[61];
                    wkronrod15 = new double[15];
                    wkronrod21 = new double[21];
                    wkronrod31 = new double[31];
                    wkronrod41 = new double[41];
                    wkronrod51 = new double[51];
                    wkronrod61 = new double[61];

                    wgauss15[0] = 0.129484966168869693270611432679082;
                    wgauss15[1] = 0.279705391489276667901467771423780;
                    wgauss15[2] = 0.381830050505118944950369775488975;
                    wgauss15[3] = 0.417959183673469387755102040816327;
                    x15[0] = 0.991455371120812639206854697526329;
                    x15[1] = 0.949107912342758524526189684047851;
                    x15[2] = 0.864864423359769072789712788640926;
                    x15[3] = 0.741531185599394439863864773280788;
                    x15[4] = 0.586087235467691130294144838258730;
                    x15[5] = 0.405845151377397166906606412076961;
                    x15[6] = 0.207784955007898467600689403773245;
                    x15[7] = 0.000000000000000000000000000000000;
                    wkronrod15[0] = 0.022935322010529224963732008058970;
                    wkronrod15[1] = 0.063092092629978553290700663189204;
                    wkronrod15[2] = 0.104790010322250183839876322541518;
                    wkronrod15[3] = 0.140653259715525918745189590510238;
                    wkronrod15[4] = 0.169004726639267902826583426598550;
                    wkronrod15[5] = 0.190350578064785409913256402421014;
                    wkronrod15[6] = 0.204432940075298892414161999234649;
                    wkronrod15[7] = 0.209482141084727828012999174891714;

                    wgauss21[0] = 0.066671344308688137593568809893332;
                    wgauss21[1] = 0.149451349150580593145776339657697;
                    wgauss21[2] = 0.219086362515982043995534934228163;
                    wgauss21[3] = 0.269266719309996355091226921569469;
                    wgauss21[4] = 0.295524224714752870173892994651338;
                    x21[0] = 0.995657163025808080735527280689003;
                    x21[1] = 0.973906528517171720077964012084452;
                    x21[2] = 0.930157491355708226001207180059508;
                    x21[3] = 0.865063366688984510732096688423493;
                    x21[4] = 0.780817726586416897063717578345042;
                    x21[5] = 0.679409568299024406234327365114874;
                    x21[6] = 0.562757134668604683339000099272694;
                    x21[7] = 0.433395394129247190799265943165784;
                    x21[8] = 0.294392862701460198131126603103866;
                    x21[9] = 0.148874338981631210884826001129720;
                    x21[10] = 0.000000000000000000000000000000000;
                    wkronrod21[0] = 0.011694638867371874278064396062192;
                    wkronrod21[1] = 0.032558162307964727478818972459390;
                    wkronrod21[2] = 0.054755896574351996031381300244580;
                    wkronrod21[3] = 0.075039674810919952767043140916190;
                    wkronrod21[4] = 0.093125454583697605535065465083366;
                    wkronrod21[5] = 0.109387158802297641899210590325805;
                    wkronrod21[6] = 0.123491976262065851077958109831074;
                    wkronrod21[7] = 0.134709217311473325928054001771707;
                    wkronrod21[8] = 0.142775938577060080797094273138717;
                    wkronrod21[9] = 0.147739104901338491374841515972068;
                    wkronrod21[10] = 0.149445554002916905664936468389821;

                    wgauss31[0] = 0.030753241996117268354628393577204;
                    wgauss31[1] = 0.070366047488108124709267416450667;
                    wgauss31[2] = 0.107159220467171935011869546685869;
                    wgauss31[3] = 0.139570677926154314447804794511028;
                    wgauss31[4] = 0.166269205816993933553200860481209;
                    wgauss31[5] = 0.186161000015562211026800561866423;
                    wgauss31[6] = 0.198431485327111576456118326443839;
                    wgauss31[7] = 0.202578241925561272880620199967519;
                    x31[0] = 0.998002298693397060285172840152271;
                    x31[1] = 0.987992518020485428489565718586613;
                    x31[2] = 0.967739075679139134257347978784337;
                    x31[3] = 0.937273392400705904307758947710209;
                    x31[4] = 0.897264532344081900882509656454496;
                    x31[5] = 0.848206583410427216200648320774217;
                    x31[6] = 0.790418501442465932967649294817947;
                    x31[7] = 0.724417731360170047416186054613938;
                    x31[8] = 0.650996741297416970533735895313275;
                    x31[9] = 0.570972172608538847537226737253911;
                    x31[10] = 0.485081863640239680693655740232351;
                    x31[11] = 0.394151347077563369897207370981045;
                    x31[12] = 0.299180007153168812166780024266389;
                    x31[13] = 0.201194093997434522300628303394596;
                    x31[14] = 0.101142066918717499027074231447392;
                    x31[15] = 0.000000000000000000000000000000000;
                    wkronrod31[0] = 0.005377479872923348987792051430128;
                    wkronrod31[1] = 0.015007947329316122538374763075807;
                    wkronrod31[2] = 0.025460847326715320186874001019653;
                    wkronrod31[3] = 0.035346360791375846222037948478360;
                    wkronrod31[4] = 0.044589751324764876608227299373280;
                    wkronrod31[5] = 0.053481524690928087265343147239430;
                    wkronrod31[6] = 0.062009567800670640285139230960803;
                    wkronrod31[7] = 0.069854121318728258709520077099147;
                    wkronrod31[8] = 0.076849680757720378894432777482659;
                    wkronrod31[9] = 0.083080502823133021038289247286104;
                    wkronrod31[10] = 0.088564443056211770647275443693774;
                    wkronrod31[11] = 0.093126598170825321225486872747346;
                    wkronrod31[12] = 0.096642726983623678505179907627589;
                    wkronrod31[13] = 0.099173598721791959332393173484603;
                    wkronrod31[14] = 0.100769845523875595044946662617570;
                    wkronrod31[15] = 0.101330007014791549017374792767493;

                    wgauss41[0] = 0.017614007139152118311861962351853;
                    wgauss41[1] = 0.040601429800386941331039952274932;
                    wgauss41[2] = 0.062672048334109063569506535187042;
                    wgauss41[3] = 0.083276741576704748724758143222046;
                    wgauss41[4] = 0.101930119817240435036750135480350;
                    wgauss41[5] = 0.118194531961518417312377377711382;
                    wgauss41[6] = 0.131688638449176626898494499748163;
                    wgauss41[7] = 0.142096109318382051329298325067165;
                    wgauss41[8] = 0.149172986472603746787828737001969;
                    wgauss41[9] = 0.152753387130725850698084331955098;
                    x41[0] = 0.998859031588277663838315576545863;
                    x41[1] = 0.993128599185094924786122388471320;
                    x41[2] = 0.981507877450250259193342994720217;
                    x41[3] = 0.963971927277913791267666131197277;
                    x41[4] = 0.940822633831754753519982722212443;
                    x41[5] = 0.912234428251325905867752441203298;
                    x41[6] = 0.878276811252281976077442995113078;
                    x41[7] = 0.839116971822218823394529061701521;
                    x41[8] = 0.795041428837551198350638833272788;
                    x41[9] = 0.746331906460150792614305070355642;
                    x41[10] = 0.693237656334751384805490711845932;
                    x41[11] = 0.636053680726515025452836696226286;
                    x41[12] = 0.575140446819710315342946036586425;
                    x41[13] = 0.510867001950827098004364050955251;
                    x41[14] = 0.443593175238725103199992213492640;
                    x41[15] = 0.373706088715419560672548177024927;
                    x41[16] = 0.301627868114913004320555356858592;
                    x41[17] = 0.227785851141645078080496195368575;
                    x41[18] = 0.152605465240922675505220241022678;
                    x41[19] = 0.076526521133497333754640409398838;
                    x41[20] = 0.000000000000000000000000000000000;
                    wkronrod41[0] = 0.003073583718520531501218293246031;
                    wkronrod41[1] = 0.008600269855642942198661787950102;
                    wkronrod41[2] = 0.014626169256971252983787960308868;
                    wkronrod41[3] = 0.020388373461266523598010231432755;
                    wkronrod41[4] = 0.025882133604951158834505067096153;
                    wkronrod41[5] = 0.031287306777032798958543119323801;
                    wkronrod41[6] = 0.036600169758200798030557240707211;
                    wkronrod41[7] = 0.041668873327973686263788305936895;
                    wkronrod41[8] = 0.046434821867497674720231880926108;
                    wkronrod41[9] = 0.050944573923728691932707670050345;
                    wkronrod41[10] = 0.055195105348285994744832372419777;
                    wkronrod41[11] = 0.059111400880639572374967220648594;
                    wkronrod41[12] = 0.062653237554781168025870122174255;
                    wkronrod41[13] = 0.065834597133618422111563556969398;
                    wkronrod41[14] = 0.068648672928521619345623411885368;
                    wkronrod41[15] = 0.071054423553444068305790361723210;
                    wkronrod41[16] = 0.073030690332786667495189417658913;
                    wkronrod41[17] = 0.074582875400499188986581418362488;
                    wkronrod41[18] = 0.075704497684556674659542775376617;
                    wkronrod41[19] = 0.076377867672080736705502835038061;
                    wkronrod41[20] = 0.076600711917999656445049901530102;

                    wgauss51[0] = 0.011393798501026287947902964113235;
                    wgauss51[1] = 0.026354986615032137261901815295299;
                    wgauss51[2] = 0.040939156701306312655623487711646;
                    wgauss51[3] = 0.054904695975835191925936891540473;
                    wgauss51[4] = 0.068038333812356917207187185656708;
                    wgauss51[5] = 0.080140700335001018013234959669111;
                    wgauss51[6] = 0.091028261982963649811497220702892;
                    wgauss51[7] = 0.100535949067050644202206890392686;
                    wgauss51[8] = 0.108519624474263653116093957050117;
                    wgauss51[9] = 0.114858259145711648339325545869556;
                    wgauss51[10] = 0.119455763535784772228178126512901;
                    wgauss51[11] = 0.122242442990310041688959518945852;
                    wgauss51[12] = 0.123176053726715451203902873079050;
                    x51[0] = 0.999262104992609834193457486540341;
                    x51[1] = 0.995556969790498097908784946893902;
                    x51[2] = 0.988035794534077247637331014577406;
                    x51[3] = 0.976663921459517511498315386479594;
                    x51[4] = 0.961614986425842512418130033660167;
                    x51[5] = 0.942974571228974339414011169658471;
                    x51[6] = 0.920747115281701561746346084546331;
                    x51[7] = 0.894991997878275368851042006782805;
                    x51[8] = 0.865847065293275595448996969588340;
                    x51[9] = 0.833442628760834001421021108693570;
                    x51[10] = 0.797873797998500059410410904994307;
                    x51[11] = 0.759259263037357630577282865204361;
                    x51[12] = 0.717766406813084388186654079773298;
                    x51[13] = 0.673566368473468364485120633247622;
                    x51[14] = 0.626810099010317412788122681624518;
                    x51[15] = 0.577662930241222967723689841612654;
                    x51[16] = 0.526325284334719182599623778158010;
                    x51[17] = 0.473002731445714960522182115009192;
                    x51[18] = 0.417885382193037748851814394594572;
                    x51[19] = 0.361172305809387837735821730127641;
                    x51[20] = 0.303089538931107830167478909980339;
                    x51[21] = 0.243866883720988432045190362797452;
                    x51[22] = 0.183718939421048892015969888759528;
                    x51[23] = 0.122864692610710396387359818808037;
                    x51[24] = 0.061544483005685078886546392366797;
                    x51[25] = 0.000000000000000000000000000000000;
                    wkronrod51[0] = 0.001987383892330315926507851882843;
                    wkronrod51[1] = 0.005561932135356713758040236901066;
                    wkronrod51[2] = 0.009473973386174151607207710523655;
                    wkronrod51[3] = 0.013236229195571674813656405846976;
                    wkronrod51[4] = 0.016847817709128298231516667536336;
                    wkronrod51[5] = 0.020435371145882835456568292235939;
                    wkronrod51[6] = 0.024009945606953216220092489164881;
                    wkronrod51[7] = 0.027475317587851737802948455517811;
                    wkronrod51[8] = 0.030792300167387488891109020215229;
                    wkronrod51[9] = 0.034002130274329337836748795229551;
                    wkronrod51[10] = 0.037116271483415543560330625367620;
                    wkronrod51[11] = 0.040083825504032382074839284467076;
                    wkronrod51[12] = 0.042872845020170049476895792439495;
                    wkronrod51[13] = 0.045502913049921788909870584752660;
                    wkronrod51[14] = 0.047982537138836713906392255756915;
                    wkronrod51[15] = 0.050277679080715671963325259433440;
                    wkronrod51[16] = 0.052362885806407475864366712137873;
                    wkronrod51[17] = 0.054251129888545490144543370459876;
                    wkronrod51[18] = 0.055950811220412317308240686382747;
                    wkronrod51[19] = 0.057437116361567832853582693939506;
                    wkronrod51[20] = 0.058689680022394207961974175856788;
                    wkronrod51[21] = 0.059720340324174059979099291932562;
                    wkronrod51[22] = 0.060539455376045862945360267517565;
                    wkronrod51[23] = 0.061128509717053048305859030416293;
                    wkronrod51[24] = 0.061471189871425316661544131965264;
                    wkronrod51[25] = 0.061580818067832935078759824240055;

                    wgauss61[0] = 0.007968192496166605615465883474674;
                    wgauss61[1] = 0.018466468311090959142302131912047;
                    wgauss61[2] = 0.028784707883323369349719179611292;
                    wgauss61[3] = 0.038799192569627049596801936446348;
                    wgauss61[4] = 0.048402672830594052902938140422808;
                    wgauss61[5] = 0.057493156217619066481721689402056;
                    wgauss61[6] = 0.065974229882180495128128515115962;
                    wgauss61[7] = 0.073755974737705206268243850022191;
                    wgauss61[8] = 0.080755895229420215354694938460530;
                    wgauss61[9] = 0.086899787201082979802387530715126;
                    wgauss61[10] = 0.092122522237786128717632707087619;
                    wgauss61[11] = 0.096368737174644259639468626351810;
                    wgauss61[12] = 0.099593420586795267062780282103569;
                    wgauss61[13] = 0.101762389748405504596428952168554;
                    wgauss61[14] = 0.102852652893558840341285636705415;
                    x61[0] = 0.999484410050490637571325895705811;
                    x61[1] = 0.996893484074649540271630050918695;
                    x61[2] = 0.991630996870404594858628366109486;
                    x61[3] = 0.983668123279747209970032581605663;
                    x61[4] = 0.973116322501126268374693868423707;
                    x61[5] = 0.960021864968307512216871025581798;
                    x61[6] = 0.944374444748559979415831324037439;
                    x61[7] = 0.926200047429274325879324277080474;
                    x61[8] = 0.905573307699907798546522558925958;
                    x61[9] = 0.882560535792052681543116462530226;
                    x61[10] = 0.857205233546061098958658510658944;
                    x61[11] = 0.829565762382768397442898119732502;
                    x61[12] = 0.799727835821839083013668942322683;
                    x61[13] = 0.767777432104826194917977340974503;
                    x61[14] = 0.733790062453226804726171131369528;
                    x61[15] = 0.697850494793315796932292388026640;
                    x61[16] = 0.660061064126626961370053668149271;
                    x61[17] = 0.620526182989242861140477556431189;
                    x61[18] = 0.579345235826361691756024932172540;
                    x61[19] = 0.536624148142019899264169793311073;
                    x61[20] = 0.492480467861778574993693061207709;
                    x61[21] = 0.447033769538089176780609900322854;
                    x61[22] = 0.400401254830394392535476211542661;
                    x61[23] = 0.352704725530878113471037207089374;
                    x61[24] = 0.304073202273625077372677107199257;
                    x61[25] = 0.254636926167889846439805129817805;
                    x61[26] = 0.204525116682309891438957671002025;
                    x61[27] = 0.153869913608583546963794672743256;
                    x61[28] = 0.102806937966737030147096751318001;
                    x61[29] = 0.051471842555317695833025213166723;
                    x61[30] = 0.000000000000000000000000000000000;
                    wkronrod61[0] = 0.001389013698677007624551591226760;
                    wkronrod61[1] = 0.003890461127099884051267201844516;
                    wkronrod61[2] = 0.006630703915931292173319826369750;
                    wkronrod61[3] = 0.009273279659517763428441146892024;
                    wkronrod61[4] = 0.011823015253496341742232898853251;
                    wkronrod61[5] = 0.014369729507045804812451432443580;
                    wkronrod61[6] = 0.016920889189053272627572289420322;
                    wkronrod61[7] = 0.019414141193942381173408951050128;
                    wkronrod61[8] = 0.021828035821609192297167485738339;
                    wkronrod61[9] = 0.024191162078080601365686370725232;
                    wkronrod61[10] = 0.026509954882333101610601709335075;
                    wkronrod61[11] = 0.028754048765041292843978785354334;
                    wkronrod61[12] = 0.030907257562387762472884252943092;
                    wkronrod61[13] = 0.032981447057483726031814191016854;
                    wkronrod61[14] = 0.034979338028060024137499670731468;
                    wkronrod61[15] = 0.036882364651821229223911065617136;
                    wkronrod61[16] = 0.038678945624727592950348651532281;
                    wkronrod61[17] = 0.040374538951535959111995279752468;
                    wkronrod61[18] = 0.041969810215164246147147541285970;
                    wkronrod61[19] = 0.043452539701356069316831728117073;
                    wkronrod61[20] = 0.044814800133162663192355551616723;
                    wkronrod61[21] = 0.046059238271006988116271735559374;
                    wkronrod61[22] = 0.047185546569299153945261478181099;
                    wkronrod61[23] = 0.048185861757087129140779492298305;
                    wkronrod61[24] = 0.049055434555029778887528165367238;
                    wkronrod61[25] = 0.049795683427074206357811569379942;
                    wkronrod61[26] = 0.050405921402782346840893085653585;
                    wkronrod61[27] = 0.050881795898749606492297473049805;
                    wkronrod61[28] = 0.051221547849258772170656282604944;
                    wkronrod61[29] = 0.051426128537459025933862879215781;
                    wkronrod61[30] = 0.051494729429451567558340433647099;

                    int n = 15, ng = ng15;
                    // copy nodes
                    for (int i = n - 1; i >= n / 2; i--)
                    {
                        x15[i] = -x15[n - 1 - i];
                        wkronrod15[i] = wkronrod15[n - 1 - i];
                    }
                    // copy Gauss weights
                    for (int i = ng - 1; i >= 0; i--)
                    {
                        wgauss15[n - 2 - 2 * i] = wgauss15[i];
                        wgauss15[1 + 2 * i] = wgauss15[i];
                    }
                    for (int i = 0; i <= n / 2; i++)
                    {
                        wgauss15[2 * i] = 0;
                    }

                    n = 21; ng = ng21;
                    // copy nodes
                    for (int i = n - 1; i >= n / 2; i--)
                    {
                        x21[i] = -x21[n - 1 - i];
                        wkronrod21[i] = wkronrod21[n - 1 - i];
                    }
                    // copy Gauss weights
                    for (int i = ng - 1; i >= 0; i--)
                    {
                        wgauss21[n - 2 - 2 * i] = wgauss21[i];
                        wgauss21[1 + 2 * i] = wgauss21[i];
                    }
                    for (int i = 0; i <= n / 2; i++)
                    {
                        wgauss21[2 * i] = 0;
                    }

                    n = 31; ng = ng31;
                    // copy nodes
                    for (int i = n - 1; i >= n / 2; i--)
                    {
                        x31[i] = -x31[n - 1 - i];
                        wkronrod31[i] = wkronrod31[n - 1 - i];
                    }
                    // copy Gauss weights
                    for (int i = ng - 1; i >= 0; i--)
                    {
                        wgauss31[n - 2 - 2 * i] = wgauss31[i];
                        wgauss31[1 + 2 * i] = wgauss31[i];
                    }
                    for (int i = 0; i <= n / 2; i++)
                    {
                        wgauss31[2 * i] = 0;
                    }

                    n = 41; ng = ng41;
                    // copy nodes
                    for (int i = n - 1; i >= n / 2; i--)
                    {
                        x41[i] = -x41[n - 1 - i];
                        wkronrod41[i] = wkronrod41[n - 1 - i];
                    }
                    // copy Gauss weights
                    for (int i = ng - 1; i >= 0; i--)
                    {
                        wgauss41[n - 2 - 2 * i] = wgauss41[i];
                        wgauss41[1 + 2 * i] = wgauss41[i];
                    }
                    for (int i = 0; i <= n / 2; i++)
                    {
                        wgauss41[2 * i] = 0;
                    }

                    n = 51; ng = ng51;
                    // copy nodes
                    for (int i = n - 1; i >= n / 2; i--)
                    {
                        x51[i] = -x51[n - 1 - i];
                        wkronrod51[i] = wkronrod51[n - 1 - i];
                    }
                    // copy Gauss weights
                    for (int i = ng - 1; i >= 0; i--)
                    {
                        wgauss51[n - 2 - 2 * i] = wgauss51[i];
                        wgauss51[1 + 2 * i] = wgauss51[i];
                    }
                    for (int i = 0; i <= n / 2; i++)
                    {
                        wgauss51[2 * i] = 0;
                    }

                    n = 61; ng = ng61;
                    // copy nodes
                    for (int i = n - 1; i >= n / 2; i--)
                    {
                        x61[i] = -x61[n - 1 - i];
                        wkronrod61[i] = wkronrod61[n - 1 - i];
                    }
                    // copy Gauss weights
                    for (int i = ng - 1; i >= 0; i--)
                    {
                        wgauss61[n - 2 - 2 * i] = wgauss61[i];
                        wgauss61[1 + 2 * i] = wgauss61[i];
                    }
                    for (int i = 0; i <= n / 2; i++)
                    {
                        wgauss61[2 * i] = 0;
                    }


                    _x15 = GetNew(x15);
                    _x21 = GetNew(x21);
                    _x31 = GetNew(x31);
                    _x41 = GetNew(x41);
                    _x51 = GetNew(x51);
                    _x61 = GetNew(x61);
                    _wgauss15 = GetNew(wgauss15);
                    _wgauss21 = GetNew(wgauss21);
                    _wgauss31 = GetNew(wgauss31);
                    _wgauss41 = GetNew(wgauss41);
                    _wgauss51 = GetNew(wgauss51);
                    _wgauss61 = GetNew(wgauss61);
                    _wkronrod15 = GetNew(wkronrod15);
                    _wkronrod21 = GetNew(wkronrod21);
                    _wkronrod31 = GetNew(wkronrod31);
                    _wkronrod41 = GetNew(wkronrod41);
                    _wkronrod51 = GetNew(wkronrod51);
                    _wkronrod61 = GetNew(wkronrod61);
                }
                /// <summary>
                /// Взятый с alglib метод Гаусса-Кронрода с выбором числа точек
                /// </summary>
                /// <param name="f"></param>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <param name="n"></param>
                /// <returns></returns>
                public static Complex MySimpleGaussKronrod(ComplexFunc f, Complex a, Complex b, int n = 61, bool ChooseStepByCompareRes = false, int MaxDivCount = 3)
                {
                    double[] x;
                    double[] wkronrod;
                    double[] wgauss;
                    double eps = 1.0E-32;
                    int ng = 0;
                    int[] p1 = new int[0];
                    int[] p2 = new int[0];

                    //if (!(n == 15 | n == 21 | n == 31 | n == 41 | n == 51 | n == 61)) throw new Exception("GKQNodesTbl: incorrect N!");

                    switch (n)
                    {
                        case 61:
                            x = x61;
                            wkronrod = wkronrod61;
                            wgauss = wgauss61;
                            break;
                        case 15:
                            x = x15;
                            wkronrod = wkronrod15;
                            wgauss = wgauss15;
                            break;

                        case 31:
                            x = x31;
                            wkronrod = wkronrod31;
                            wgauss = wgauss31;
                            break;
                        case 41:
                            x = x41;
                            wkronrod = wkronrod41;
                            wgauss = wgauss41;
                            break;
                        case 51:
                            x = x15;
                            wkronrod = wkronrod51;
                            wgauss = wgauss51;
                            break;
                        case 21:
                            x = x21;
                            wkronrod = wkronrod21;
                            wgauss = wgauss21;
                            break;
                        default:
                            throw new Exception("GKQNodesTbl: incorrect N! (n должно быть 15/21/31/41/51/61)");
                    }

                 ComplexFunc t = (Complex r) => (a + (r + 1) / 2 * (b - a));
                    Complex sumKR = new Complex(0), sumGS = new Complex(0);
                    for (int i = 0; i < n; i++)
                    {
                        Complex tmp = f(t(x[i])); if (tmp == null) tmp.Show();
                        if (Double.IsNaN(tmp.Re) || Double.IsInfinity(tmp.Re)) throw new Exception($"Значение функции Nan или Inf во время интегрирования: f({t(x[i])}) = {f(t(x[i]))}");//Console.WriteLine($"Значение функции Nan или Inf во время интегрирования: f({t(x[i])}) = {f(t(x[i]))}");
                        sumKR += wkronrod[i] * tmp;
                        sumGS += wgauss[i] * tmp;
                    }

                    if(!ChooseStepByCompareRes) return sumKR / 2 * (b - a);

                    if (MaxDivCount>0&&(sumGS - sumKR).Abs > /*sumKR.Abs / 1000*/1e-4)
                       return MySimpleGaussKronrod(f, a, a + (b-a) / 2, n,true,MaxDivCount-1) + MySimpleGaussKronrod(f,a+(b-a)/2,b, n,true, MaxDivCount - 1);
                    else
                    return sumKR / 2 * (b - a);
                }
                /// <summary>
                /// Взятый с alglib метод Гаусса-Кронрода с выбором числа точек
                /// </summary>
                /// <param name="f"></param>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <param name="n"></param>
                /// <returns></returns>
                public static double MySimpleGaussKronrod(RealFunc f, double a, double b, int n = 61, bool ChooseStepByCompareRes = false, int MaxDivCount = 3)
                {
                    return MySimpleGaussKronrod((Complex t) => f(t.Re), new Complex(a), new Complex(b), n,ChooseStepByCompareRes,MaxDivCount).Re;
                }
                /// <summary>
                /// Метод Гаусса-Кронрода с выбором числа точек
                /// </summary>
                /// <param name="f">Интегрируемая функция</param>
                /// <param name="a">Начало отрезка интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <param name="n">Число точек в методе Гаусса-Кронрода</param>
                /// <param name="ChooseStepByCompareRes">Требуется ли пересчитывать с меньшими отрезками, если разница между методами Гаусса и Гаусса-Кронрода существенная</param>
                /// <param name="MaxDivCount">Сколько ещё раз можно поделить отрезок (максимальное число делений)</param>
                /// <returns></returns>
                public static CVectors MySimpleGaussKronrod(Func<Complex,CVectors> f, Complex a, Complex b, int n = 61, bool ChooseStepByCompareRes=true,int MaxDivCount=3)
                {
                    double[] x;
                    double[] wkronrod;
                    double[] wgauss;
                    double eps = 1.0E-32;
                    int ng = 0;
                    int[] p1 = new int[0];
                    int[] p2 = new int[0];

                    //if (!(n == 15 | n == 21 | n == 31 | n == 41 | n == 51 | n == 61)) throw new Exception("GKQNodesTbl: incorrect N!");

                    switch (n)
                    {
                        case 61:
                            x = x61;
                            wkronrod = wkronrod61;
                            wgauss = wgauss61;
                            break;
                        case 15:
                            x = x15;
                            wkronrod = wkronrod15;
                            wgauss = wgauss15;
                            break;

                        case 31:
                            x = x31;
                            wkronrod = wkronrod31;
                            wgauss = wgauss31;
                            break;
                        case 41:
                            x = x41;
                            wkronrod = wkronrod41;
                            wgauss = wgauss41;
                            break;
                        case 51:
                            x = x15;
                            wkronrod = wkronrod51;
                            wgauss = wgauss51;
                            break;
                        case 21:
                            x = x21;
                            wkronrod = wkronrod21;
                            wgauss = wgauss21;
                            break;
                        default:
                            throw new Exception("GKQNodesTbl: incorrect N! (n должно быть 15/21/31/41/51/61)");
                    }


                    ComplexFunc t = (Complex r) => (a + (r + 1) / 2 * (b - a));
                    CVectors sumKR = new CVectors(f((a+b)/2).Degree), sumGS = new CVectors(sumKR.Degree);
                    for (int i = 0; i < n; i++)
                    {
                        CVectors tmp = f(t(x[i])); if (tmp == null) tmp.Show();
                        sumKR += wkronrod[i] * tmp;
                        sumGS += wgauss[i] * tmp;
                    }
                    if(!ChooseStepByCompareRes)
                        return sumKR / 2 * (b - a);

                    if (MaxDivCount>0 && (sumGS - sumKR).Abs > sumKR.Abs / 1000)
                        return MySimpleGaussKronrod(f, a, a + (b - a) / 2, n,true,MaxDivCount-1) + MySimpleGaussKronrod(f, a + (b - a) / 2, b, n,true,  MaxDivCount-1);
                     else
                    return sumKR / 2 * (b - a);
                }

                /// <summary>
                /// Метод Гаусса-Кронрода, который вместо отрезка делает обход контура, если на отрезке есть полюса
                /// </summary>
                /// <param name="f">Интегрируемая функция</param>
                /// <param name="a">Начало отрезка интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <param name="delta">Знаменатель интегрируемой функции</param>
                /// <param name="t">Предполагаемый радиус отрезка, вне которого полюсов нет</param>
                /// <param name="n">Число узлов интегрировани</param>
                /// <param name="h">Отклонение контура</param>
                /// <returns></returns>
                public static Complex MySuperGaussKronrod(ComplexFunc f, Complex a, Complex b, ComplexFunc delta = null, int t = 100, int n = 61, double h = 0.2)
                {
                    if (delta == null || Math.Max(b.Re, -a.Re) > t) return MySimpleGaussKronrod(f, a, b, n);//если знаменатель неизвестен либо отрезок вне зоны нахождения предполагаемых полюсов, интегрировать по-обычному
                    if (a.Im != 0 && a.Im != b.Im) return MySimpleGaussKronrod(f, a, b, n);//если отрезок не на вещественной оси, интегрировать
                    if (a.Re * b.Re < 0 || delta(0) != 0) return MySuperGaussKronrod(f, a, 0, delta, t, n, h) + MySuperGaussKronrod(f, 0, b, delta, t, n, h);//если концы отрезка по обе стороны от 0, разбить на 2
                    var tmp = Optimization.Neu(delta, a, b);//tmp.Show();//найти корни на отрезке (надо знать, существуют или нет)
                    if (a.Re < 0) h *= -1;//если отрезок слева от 0, брать обход снизу
                    if (tmp == null) return MySimpleGaussKronrod(f, a, b, n);//если нет полюсов, решать по-обычному
                    else
                        return MySimpleGaussKronrod(f, a, a + h, n) + MySimpleGaussKronrod(f, a + h, b + h, n) + MySimpleGaussKronrod(f, b + h, b, n);//иначе обойти контур
                }

                /// <summary>
                /// Метод Гаусса-Кронрода, использующий параллельные вычисления за счёт разбиения отрезка интегрирования на несколько частей
                /// </summary>
                /// <param name="f">Интегрируемая функция</param>
                /// <param name="a">Начало отрезка интегрирования</param>
                /// <param name="b">Конец отрезка интегрирования</param>
                /// <param name="n">Число точек в методе</param>
                /// <param name="count">На сколько отрезков разбиватся исходный отрезок</param>
                /// <returns></returns>
                public static Complex ParallelGaussKronrod(ComplexFunc f, Complex a, Complex b, int n = 61, int count = 5)
                {
                    Complex[] mas = new Complex[count];
                    Complex step = (b - a) / (count - 1);
                    Parallel.For(1, count, i => mas[i] = MySimpleGaussKronrod(f, a + (i - 1) * step, a + i * step, n));
                    Array.Sort(mas);
                    Complex sum = 0;
                    for (int i = 0; i < count; i++)
                        sum += mas[i];
                    return sum;
                }
            }

            private delegate double FUNC(RealFunc f, double a, double b);
            /// <summary>
            /// Подсчёт определённого интеграла выбранными методом и относительно выбранного критерия
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="a">Начало отрезка интегрирования</param>
            /// <param name="b">Конец отрезка интегрирования</param>
            /// <param name="M">Метод подсчёта интеграла</param>
            /// <param name="C">Критерий подсчёта интеграла (указанное число шагов/точность/разбиение интеграла на сумму интегралов по нескольким частям отрезка интегрирования)</param>
            /// <param name="count">Число шагов при интегрировании</param>
            /// <param name="eps">Точность интеграла</param>
            /// <param name="seqcount">На сколько частей разбивается отрезок</param>
            /// <returns></returns>
            public static double DefIntegral(RealFunc f, double a, double b, DefInteg.Method M, DefInteg.Criterion C, int count = 15, double eps = 0.0001, int seqcount = 2, bool GetEPS = false, int n = 15)
            {
                //GaussKronrod.ChooseGK(GaussKronrod.NodesCount.GK15);

                DefInteg.h_Count = 5000;
                DefInteg.n = 20;
                DefInteg.EPS = STEP / 200;

                FUNC Met, TMP = (RealFunc q, double w, double e) => GaussKronrod.MySimpleGaussKronrod(q, w, e, n), TMP2 = (RealFunc q, double w, double e) => GaussKronrod.MySimpleGaussKronrod(q, w, e, 61);
                FUNC Gmet = (RealFunc q, double w, double e) => { return GaussKronrod.Integral(q, w, e); };
                FUNC GEmp = (RealFunc q, double w, double e) => { return GaussKronrod.MySimpleGaussKronrod(q, w, e,61,true); };
                switch (M)
                {
                    case Method.MiddleRect:
                        Met = MiddleRect;
                        break;
                    case Method.Trapez:
                        Met = Trapez;
                        break;
                    case Method.Simpson:
                        Met = Simpson;
                        break;
                    case Method.Meler:
                        Met = Meler;
                        break;
                    case Method.GaussKronrod15:
                        //Met = GaussKronrod.Integral;
                        Met = TMP;
                        break;
                    case Method.GaussKronrod61:
                        //Met = GaussKronrod.Integral;
                        Met = TMP2;
                        break;
                    case Method.GaussKronrod61fromFortran:
                        Met = Gmet;
                        break;
                    case Method.GaussKronrod61Empire:
                        Met = GEmp;
                        break;
                    default: //Method.Gauss:
                        Met = Gauss;
                        break;
                }
                if (C == Criterion.StepCount)
                {
                    if (M == Method.Gauss || M == Method.Meler)
                    {
                        DefInteg.n = count;
                        DefInteg.h_Count = DefInteg.n;
                        DefInteg.EPS = Double.NaN;
                        return Met(f, a, b);
                    }
                    DefInteg.h_Count = count;
                    //DefInteg.h_Count *= 2;
                    double _I1 = Met(f, a, b);
                    if (GetEPS)
                    {
                        DefInteg.h_Count *= 2;
                        double _I2 = Met(f, a, b);
                        DefInteg.EPS = Math.Abs(_I1 - _I2);
                        DefInteg.h_Count /= 2/*4*/;
                    }
                    return _I1;
                }
                if (C == Criterion.Accuracy)
                {
                    if (M == Method.Gauss || M == Method.Meler)
                    {
                        DefInteg.EPS = eps;
                        DefInteg.n = 20;
                        //DefInteg.n += 5;
                        double I11 = Met(f, a, b);
                        DefInteg.n += 5;
                        double I22 = Met(f, a, b);

                        DefInteg.n = 20;
                        while (Math.Abs(I11 - I22) >= DefInteg.EPS)
                        {
                            DefInteg.n += 5;
                            //DefInteg.n += 5;
                            I11 = Met(f, a, b);
                            DefInteg.n += 5;
                            I22 = Met(f, a, b);
                            DefInteg.n -= 5/*10*/;
                        }

                        //DefInteg.n += 5/*10*/;
                        DefInteg.h_Count = DefInteg.n;
                        DefInteg.EPS = Math.Abs(I11 - I22);
                        return I11;
                    }

                    DefInteg.EPS = eps;
                    DefInteg.h_Count = count;
                    //DefInteg.h_Count *= 2;
                    double I1 = Met(f, a, b);
                    DefInteg.h_Count *= 2;
                    double I2 = Met(f, a, b);

                    DefInteg.h_Count = count;
                    while (Math.Abs(I1 - I2) >= DefInteg.EPS)
                    {
                        DefInteg.h_Count *= 2;
                        //DefInteg.h_Count *= 2;
                        I1 = Met(f, a, b);
                        DefInteg.h_Count *= 2;
                        I2 = Met(f, a, b);
                        DefInteg.h_Count /= 2/*4*/;
                    }

                    //DefInteg.h_Count *= 4;
                    return I1;
                }
                if (C == Criterion.SegmentCount)
                {
                    double h = (b - a) / seqcount, s = 0;
                    DefInteg.h_Count = count;
                    DefInteg.n = count; DefInteg.EPS = Double.NaN;
                    for (int i = 1; i <= seqcount; i++) s += Met(f, a + (i - 1) * h, a + i * h);
                    return s;
                }
                return 0;
            }
            /// <summary>
            /// Подсчёт кратного интеграла
            /// </summary>
            /// <param name="f">Функционал под интегралом</param>
            /// <param name="c">Кривая, являющаяся границей области интегрирования</param>
            /// <param name="S">Функция трёх переменных, задающая зависимость площади куска от параметров tx, ty и радиуса</param>
            /// <param name="M">Метод обычного интегрирования</param>
            /// <param name="C">Критерий интегрирования</param>
            /// <param name="count">Число шагов (тоже не требуется)</param>
            /// <param name="eps">Точность (требуется при определённом критерии интегрирования)</param>
            /// <param name="parallel">Требуется ли считать интегралы по кольцам параллельно</param>
            /// <param name="tx">Шаг "по кольцу"</param>
            /// <param name="ty">Шаг "по радиусу фигуры"</param>
            /// <returns></returns>
            public static double DoubleIntegral(Functional f, Curve c, TripleFunc S, DefInteg.Method M = DefInteg.Method.Simpson, double tx = 0.01, double ty = 0.01, DefInteg.Criterion C = DefInteg.Criterion.StepCount, int count = 20, double eps = 0.001, bool parallel = true, bool makesort = true, double rmin = 0)
            {
                double sum = 0;//окончательный результат
                int countk = (int)Math.Floor((c.BaseRadius - rmin) / ty);//число колец
                //if (countk < 1) return 0;
                if (!parallel)
                    for (int k = 0; k < countk; k++)//проход по кольцам
                    {
                        double rk = c.BaseRadius - ty * (k + 0.5) + rmin;//радиус кольца (контура посередине)
                        double lk = c.End(rk)/*2 * Math.PI * rk*/;//"длина" кольца (требуется для подсчёта числа шагов при поиске определённого интеграла)
                        Curve cc = new Curve(c.a, c.a + c.End(rk), c.U, c.V, rk);//кривая, по которой будет интегрирование
                        RealFunc tmp = (double x) => f(cc.Transfer(x));//отображение кольца в отрезок, который отображается в действительную функцию
                        double s = DefIntegral(tmp, cc.a, cc.b, M, C, (int)Math.Floor(lk / tx));//интеграл по отрезку(кругу) с нужным числом шагов
                        if (M != DefInteg.Method.GaussKronrod15 && M != DefInteg.Method.GaussKronrod61 && M != DefInteg.Method.GaussKronrod61fromFortran)//если метод требует разбиения отрезка интегрирования на части
                            sum += s * (S(tx, ty, rk) - S(tx, ty, rmin)) / tx;//умножение интеграла на отношение площади к шагу
                        else sum += s * (S(cc.b - cc.a, ty, rk) - S(cc.b - cc.a, ty, rmin)) / (cc.b - cc.a);//если интегрирования проводится сразу по всему кольцу
                    }
                else//такие же вычисления, но параллельно
                {
                    double[] mas = new double[countk];
                    Parallel.For(0, countk, (int k) =>
                      {
                          double rk = c.BaseRadius - ty * (k + 0.5) + rmin;//радиус кольца (окружности посередине)
                          double lk = c.End(rk)/*2 * Math.PI * rk*/;//длина кольца
                          Curve cc = new Curve(c.a, c.a + c.End(rk), c.U, c.V, rk);//кривая, по которой будет интегрирование
                          RealFunc tmp = (double x) => f(cc.Transfer(x));//отображение кольца в отрезок, который отображается в действительную функцию
                          double s = DefIntegral(tmp, cc.a, cc.b, M, C, (int)Math.Floor(lk / tx));//интеграл по отрезку(кругу) с нужным числом шагов
                          if (M != DefInteg.Method.GaussKronrod15 && M != DefInteg.Method.GaussKronrod61 && M != DefInteg.Method.GaussKronrod61fromFortran)
                              mas[k] = s * (S(tx, ty, rk) - S(tx, ty, rmin)) / tx;//умножение интеграла на отношение площади к шагу
                          else mas[k] = s * (S(cc.b - cc.a, ty, rk) - S(cc.b - cc.a, ty, rmin)) / (cc.b - cc.a);
                          //mas[k].Show();
                          //(S(tx, ty, rk) - S(tx, ty, rmin)).Show();
                      });

                    if (makesort) Array.Sort(mas, new Expendator.Compar());
                    sum = mas.Sum();
                }
                return sum;
            }
            /// <summary>
            /// Подсчёт кратного интеграла
            /// </summary>
            /// <param name="f">Функционал под интегралом</param>
            /// <param name="c">Граница области интегрирования</param>
            /// <param name="S">Функция, определяющая площадь сегмента</param>
            /// <param name="M">Метод интегрирования</param>
            /// <param name="tx">Шаг по кольцу</param>
            /// <param name="cy">Число колец</param>
            /// <param name="C">Критерий интегрирования</param>
            /// <param name="count"></param>
            /// <param name="eps"></param>
            /// <returns></returns>
            public static double DoubleIntegral(Functional f, Curve c, TripleFunc S, DefInteg.Method M = DefInteg.Method.Simpson, double tx = 0.01, int cy = 15, DefInteg.Criterion C = DefInteg.Criterion.StepCount, int count = 20, double eps = 0.001, bool parallel = true, bool makesort = true, double rmin = 0)
            {
                double ty = c.BaseRadius / cy;//ty.Show();
                                              //Math.Floor(1.03).Show();
                return DoubleIntegral(f, c, S, M, tx, ty, C, count, eps, parallel, makesort, rmin);
            }
            /// <summary>
            /// Подсчёт двойного интеграла на области, описываемой кривой при бесконечном радиусе
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="c">Кривая</param>
            /// <param name="S">Функция площади</param>
            /// <param name="M">Метод обычного интегрирования</param>
            /// <param name="tx">Шаг внутри кольца</param>
            /// <param name="cy">Число колец</param>
            /// <param name="C">Критерий интегрирования</param>
            /// <param name="count">Число шагов</param>
            /// <param name="eps">Погрешность</param>
            /// <param name="parallel">Нужно ли распараллеливание</param>
            /// <param name="makesort">Нужна ли сортировка данных</param>
            /// <param name="Rstep">Шаг по радиусу</param>
            /// <returns></returns>
            /// <remarks>Тестирование на вейвлетах показало, что увеличение радиуса колец увеличивает точность вычислений на доли процентов, зато время работы увеличивается на 20-30%, то есть вариант неоптимален</remarks>
            public static double DoubleIntegralInf(Functional f, Curve c, TripleFunc S, DefInteg.Method M = DefInteg.Method.GaussKronrod61, double tx = 0.01, int cy = 100, DefInteg.Criterion C = DefInteg.Criterion.StepCount, int count = 20, double eps = 1e-8, bool parallel = true, bool makesort = true, double Rstep = 3, double Rmax = Double.PositiveInfinity, int min_iter = 1, int changestepcount = 0)
            {
                List<double> tmp = new List<double>();
                double i = 1;
                int mk = 0;
                do
                {
                    while (true)
                    {
                        c.BaseRadius = i * Rstep;
                        tmp.Add(DoubleIntegral(f, c, c.S, M, tx, cy, C, count, eps, parallel, makesort, (i - 1) * Rstep));
                        if (i >= min_iter)
                            if (tmp.Last().Abs() < eps || i * Rstep >= Rmax) break;
                        i++;
                    }
                    mk++;
                    Rstep *= i;
                    //cy *= 2;
                    i = 2;
                }
                while (mk <= changestepcount + 1);

                if (makesort) tmp.Sort(new Expendator.Compar());
                return tmp.Sum();
            }
            /// <summary>
            /// Подсчёт двойного интеграла в правой полуплоскости
            /// </summary>
            /// <param name="f"></param>
            /// <param name="M"></param>
            /// <param name="tx"></param>
            /// <param name="cy"></param>
            /// <param name="C"></param>
            /// <param name="count"></param>
            /// <param name="eps"></param>
            /// <param name="parallel"></param>
            /// <param name="makesort"></param>
            /// <param name="Rstep"></param>
            /// <returns></returns>
            public static double DoubleIntegralIn_IandIV(Functional f, DefInteg.Method M = DefInteg.Method.GaussKronrod61, double tx = 0.01, int cy = 100, DefInteg.Criterion C = DefInteg.Criterion.StepCount, int count = 20, double eps = 1e-8, bool parallel = true, bool makesort = true, double Rstep = 0.1, double Rmax = Double.PositiveInfinity, int min_iter = 1, int changestepcount = 0)
            {
                Curve c = new Curve(-Math.PI / 2, Math.PI / 2, (double t, double r) => r * Math.Cos(t), (double t, double r) => r * Math.Sin(t), Rstep);
                c.S = (double x, double y, double r) => x * y * r;
                c.End = (double r) => Math.PI / 2;

                return DoubleIntegralInf(f, c, c.S, M, tx, cy, C, count, eps, parallel, makesort, Rstep, Rmax, min_iter, changestepcount);
            }
            /// <summary>
            /// Подсчёт двойного интеграла для всей плоскости
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="M">Метод интегрирования</param>
            /// <param name="tx">Шаг по кольцу</param>
            /// <param name="cy">Число колец</param>
            /// <param name="C">Критерий (не следует менять)</param>
            /// <param name="count">Число отрезков разбиения (не надо трогать)</param>
            /// <param name="eps">Точность</param>
            /// <param name="parallel">Вычислять ли параллельно</param>
            /// <param name="makesort">Делать ли сортировку перед суммированием</param>
            /// <param name="Rstep">Шаг по радиусу колец</param>
            /// <param name="Rmax">Максимальный радиус</param>
            /// <param name="min_iter">Минимальное число итераций, которые требуется выполнить</param>
            /// <param name="changestepcount">Сколько раз изменять шаг (изменять шаг непроизводительно)</param>
            /// <param name="a">Первая полуось эллипса</param>
            /// <param name="b">Вторая полуось эллипса</param>
            /// <returns></returns>
            public static double DoubleIntegralIn_FULL(Functional f, DefInteg.Method M = DefInteg.Method.GaussKronrod61, double tx = 0.01, int cy = 100, DefInteg.Criterion C = DefInteg.Criterion.StepCount, int count = 20, double eps = 1e-10, bool parallel = true, bool makesort = true, double Rstep = 0.1, double Rmax = Double.PositiveInfinity, int min_iter = 5, int changestepcount = 0, double a = 1, double b = 1)
            {
                b /= a;
                a = 1;

                Curve c = new Curve(0, 2 * Math.PI, (double t, double r) => a * r * Math.Cos(t), (double t, double r) => b * r * Math.Sin(t), Rstep);
                //c.u = (double t) => c.BaseRadius * Math.Cos(t);
                //c.v = (double t) => c.BaseRadius * Math.Sin(t);
                c.S = (double x, double y, double r) => x * y * r * a * b;
                c.End = (double r) => 2 * Math.PI;
                return DoubleIntegralInf(f, c, c.S, M, tx, cy, C, count, eps, parallel, makesort, Rstep, Rmax, min_iter, changestepcount);
            }

            /// <summary>
            /// Вариация метода Монте-Карло
            /// </summary>
            public enum MonteKarloEnum
            {
                /// <summary>
                /// Обычный
                /// </summary>
                Usual,
                /// <summary>
                /// Геометрический
                /// </summary>
                Geo
            }
            /// <summary>
            /// Подсчёт определённого интеграла методом Монте-Карло
            /// </summary>
            /// <returns></returns>
            public static double MonteKarlo(MultiFunc F, MonteKarloEnum e = MonteKarloEnum.Usual, params Point[] p)
            {
                int c = DefInteg.n;//число точек
                double[][] tmp = new double[c][];//массив точек на области задания функции
                double[] mas = new double[c];//массив случайных значений функции
                double[][] masG = new double[c][];//массив точек
                double sum = 0, s = 1.0;
                Random rand = new Random(Environment.TickCount);

                for (int i = 0; i < c; i++)
                {
                    tmp[i] = GetRandomPoint(rand, p);
                    //Vectors v = new Vectors(tmp[i]);v.Show();
                    mas[i] = F(tmp[i]);
                    //Console.WriteLine(mas[i]);
                    //Random r = new Random(Environment.TickCount);
                    double t = Expendator.Min(F) + (Expendator.Max(F) - Expendator.Min(F)) * rand.NextDouble();

                    //Point[] t = new Point[p.Length + 1];
                    //t[p.Length] = new Point(Expendator.Min(F), Expendator.Max(F));
                    //for (int j = 0; j < p.Length; j++) t[j] = new Point(p[j]);
                    //masG[i] = GetRandomPoint(t);

                    masG[i] = new double[p.Length + 1];
                    masG[i][p.Length] = t;
                    for (int j = 0; j < p.Length; j++) masG[i][j] = tmp[i][j];
                    //Vectors v = new Vectors(masG[i]); v.Show(); Console.WriteLine("\tзначение функции---->"+mas[i]);
                }
                for (int i = 0; i < p.Length; i++)
                    s *= Math.Abs(p[i].x - p[i].y);

                switch (e)
                {
                    case MonteKarloEnum.Usual:
                        for (int i = 0; i < c; i++)
                            sum += mas[i];
                        sum /= c;
                        sum *= s;
                        return sum;
                    case MonteKarloEnum.Geo:
                        int k = 0;
                        for (int i = 0; i < c; i++)
                            if (masG[i][p.Length] <= mas[i]) k++;
                        sum = (double)(s * k) / c * Math.Abs(Expendator.Min(F) - Expendator.Max(F));
                        return sum;
                    default:
                        return 0;
                }
            }
            private static double[] GetRandomPoint(Random rand, params Point[] p)
            {
                double[] d = new double[p.Length];

                for (int i = 0; i < d.Length; i++)
                {
                    //double ra = (double)rand.Next(0, 1000) / 1000;
                    d[i] = p[i].x + (p[i].y - p[i].x) * /*ra*/rand.NextDouble();
                }
                return d;
            }

            /// <summary>
            /// Несобственный интеграл от минус бесконечности до бесконечности
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            public static double ImproperFirstKind(RealFunc f)
            {
                double t_max = 10, t_step = 1;
                double t = 1;//длина шага в сумме интегралов; когда t большое или маленькое, интеграл считается слишком неточно - единица более-менее подходит для начального шага
                double beg = 0, end = t, kp = Simpson(f, beg, end), km = Simpson(f, -end, -beg), sum = 0;

                for (; t < t_max; t += t_step)
                    do
                    {
                        beg += t; end += t;
                        sum += kp /*+ km*/;
                        kp = Simpson(f, beg, end);
                        //km = Simpson(f, -end, -beg);
                    } while ((kp > EPS) /*|| (km > EPS)*/);


                t = 1; beg = 0; end = t;
                for (; t < t_max; t += t_step)
                    do
                    {
                        beg += t; end += t;
                        sum += /*kp + */km;
                        //kp = Simpson(f, beg, end);
                        km = Simpson(f, -end, -beg);
                    } while (/*(kp > EPS) ||*/ (km > EPS));

                return sum;

            }
            /// <summary>
            /// Несобственный интеграл на отрезке от a до бесконечности
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <returns></returns>
            public static double ImproperFirstKindInf(RealFunc f, double a)
            {
                double t_max = 100, t_step = 1;
                double t = 1;//длина шага в сумме интегралов; когда t большое или маленькое, интеграл считается слишком неточно - единица более-менее подходит для начального шага
                double beg = a, end = a + t, kp = Simpson(f, beg, end), sum = 0;

                for (; t < t_max; t += t_step)
                    do
                    {
                        beg += t; end += t;
                        sum += kp;
                        kp = Simpson(f, beg, end);
                    } while (kp > EPS);

                return sum;
            }

            /// <summary>
            /// Демонстрация посчёта интегралов разными методами
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            public static void Demonstration(RealFunc f, double a, double b)
            {
                Console.WriteLine("Интеграл методом средних прямоугольников = \t" + MiddleRect(f, a, b));
                Console.WriteLine("Интеграл методом трапеций = \t" + Trapez(f, a, b));
                Console.WriteLine("Интеграл методом Симпсона = \t" + Simpson(f, a, b));
                Console.WriteLine("Интеграл методом Гаусса = \t" + Gauss(f, a, b));
                Console.WriteLine("Интеграл методом Мелера = \t" + Meler(f, a, b));
            }
            /// <summary>
            /// Демонстрация подсчёта кратных интегралов
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="c">Граница области интегрирования</param>
            /// <param name="S">Функция площади</param>
            /// <param name="pog">Массив погрешностей от разных методов</param>
            /// <param name="time">Массив временных затрат от разных методов</param>
            /// <param name="tx">Шаг интегрирования</param>
            /// <param name="cy">Шаг по кольцу</param>
            /// <param name="existsolve">Выводить на консоль интегралы (либо погрешности)</param>
            /// <param name="integ">Истинное значение интеграла</param>
            /// <param name="s">Строка, в которой записана интегрируемая функция</param>
            public static void Demonstration(Functional f, Curve c, TripleFunc S, out double[] pog, out TimeSpan[] time, double tx = 0.01, int cy = 15, bool existsolve = true, double integ = 0, string s = "")
            {
                $"\tРезультат подсчёта двойного интеграла при числе колец {cy} и шаге по кольцу {tx} для функции {s}: ".Show();
                double[] res = new double[6];
                time = new TimeSpan[6];
                DateTime t = DateTime.Now;
                res[0] = DoubleIntegral(f, c, S, Method.MiddleRect, tx, cy); time[0] = (DateTime.Now - t); t = DateTime.Now;
                res[1] = DoubleIntegral(f, c, S, Method.Trapez, tx, cy); time[1] = (DateTime.Now - t); t = DateTime.Now;
                res[2] = DoubleIntegral(f, c, S, Method.Simpson, tx, cy); time[2] = (DateTime.Now - t); t = DateTime.Now;
                res[3] = DoubleIntegral(f, c, S, Method.Meler, tx, cy); time[3] = (DateTime.Now - t); t = DateTime.Now;
                res[4] = DoubleIntegral(f, c, S, Method.GaussKronrod15, tx, cy); time[4] = (DateTime.Now - t); t = DateTime.Now;
                res[5] = DoubleIntegral(f, c, S, Method.GaussKronrod61, tx, cy); time[5] = (DateTime.Now - t); t = DateTime.Now;
                pog = new double[6];
                for (int i = 0; i < 6; i++)
                {
                    pog[i] = Math.Abs(integ - res[i]);
                    //time[i] = time[i].Seconds;
                }


                if (existsolve)
                {
                    "-----------Отличие от истинного решения при подсчёте".Show();
                    Console.WriteLine($"Методом средних прямоугольников (время {time[0].TotalSeconds}) =\t" + pog[0]);
                    Console.WriteLine($"Методом трапеций (время {time[1].TotalSeconds}) =\t" + pog[1]);
                    Console.WriteLine($"Методом Симпсона (время {time[2].TotalSeconds}) =\t" + pog[2]);
                    Console.WriteLine($"Методом Мелера (время {time[3].TotalSeconds}) =\t" + pog[3]);
                    Console.WriteLine($"Методом Гаусса-Кронрода (время {time[4].TotalSeconds}) (15 точек) =\t" + pog[4]);
                    Console.WriteLine($"Методом Гаусса-Кронрода (время {time[5].TotalSeconds}) (61 точка) =\t" + pog[5]);
                }
                else
                {
                    Console.WriteLine($"Методом средних прямоугольников =\t" + res[0]);
                    Console.WriteLine($"Методом трапеций (время {time[1].TotalSeconds}) =\t" + res[1]);
                    Console.WriteLine($"Методом Симпсона (время {time[2].TotalSeconds}) =\t" + res[2]);
                    Console.WriteLine($"Методом Мелера (время {time[3].TotalSeconds}) =\t" + res[3]);
                    Console.WriteLine($"Методом Гаусса-Кронрода (время {time[4].TotalSeconds}) (15 точек) =\t" + res[4]);
                    Console.WriteLine($"Методом Гаусса-Кронрода (время {time[5].TotalSeconds}) (61 точка) =\t" + res[5]);
                }
                "".Show();
            }
            /// <summary>
            /// Оформление результатов кратного интегрирования разными методами в Excel
            /// </summary>
            /// <param name="f">Интегрируемая функция</param>
            /// <param name="c">Граница области</param>
            /// <param name="S">Функция площади</param>
            /// <param name="cy">Массив значений количества колец</param>
            /// <param name="tx">Шаг по кольцу</param>
            /// <param name="integ">Истинное значение интеграла</param>
            public static void DemostrationToExcel(Functional f, Curve c, TripleFunc S, int[] cy, double tx = 0.01, double integ = 0)
            {
                double[] pog, zeit;
                TimeSpan[] time;
                Vectors[] tmp = new Vectors[cy.Length];
                for (int i = 0; i < cy.Length; i++)
                {
                    Demonstration(f, c, S, out pog, out time, tx, cy[i], integ: integ);
                    zeit = new double[time.Length];
                    for (int j = 0; j < zeit.Length; j++)
                        zeit[j] = time[j].TotalSeconds;

                    double[] t = Expendator.Union(new double[] { cy[i] }, Vectors.Mix(new Vectors(zeit), new Vectors(pog)).DoubleMas);
                    tmp[i] = new Vectors(t);
                }
                string[] st = new string[] { "", "Средние прямоугольники", "", "Трапеции", "", "Симпсон", "", "Мелер", "", "Гаусс-Кронрод 15", "", "Гаусс-Кронрод 61", "" };
                ИнтеграцияСДругимиПрограммами.CreatTableInExcel(st, tmp);
            }

            /// <summary>
            /// Класс методов с вычетами
            /// </summary>
            public static class Residue
            {
                public static double eps = 1e-6;

                /// <summary>
                /// Производная функции со вторым порядком точности
                /// </summary>
                /// <param name="f"></param>
                /// <param name="z"></param>
                /// <returns></returns>
                public static Complex Derivative(ComplexFunc f, Complex z) => (f(z + eps) - f(z - eps)) / (2 * eps);

                /// <summary>
                /// Вычет в точке (простом полюсе) у функции, представимой в виде частного, где полюс есть нуль знаменателя
                /// </summary>
                /// <param name="g">Числитель функции</param>
                /// <param name="d">Знаменатель функции</param>
                /// <param name="z">Полюс (простой)</param>
                /// <returns></returns>
                private static Complex Res(ComplexFunc g, ComplexFunc d, Complex z) => g(z) / Derivative(d, z);
                /// <summary>
                /// Сумма вычетов функции по набору полюсов
                /// </summary>
                /// <param name="g">Числитель функции</param>
                /// <param name="d">Знаменатель функции</param>
                /// <param name="qe">Дополнительный множитель, не содержащий особенностей в заданных полюсах</param>
                /// <param name="mas">Массив полюсов</param>
                /// <returns></returns>
                public static Complex ResSum(ComplexFunc g, ComplexFunc d, ComplexFunc qe, Complex[] mas)
                {
                    //mas.Show();
                    Complex sum = 0;
                    for (int i = 0; i < mas.Length; i++)
                    {
                        sum += Res(g, d, mas[i]) * qe(mas[i]); //(qe(mas[i])).Show();
                    }
                    return sum;
                }
            }

        }

        /// <summary>
        /// Аппроксимация функций системой функций на отрезке
        /// </summary>
        /// <param name="f">Функция, которую надо аппроксимировать</param>
        /// <param name="p">Функция, зависящая от действительного аргумента и номера</param>
        /// <param name="kind">Класс функций в системе (ортогональные/ортонормальные/другие)</param>
        /// <param name="n">Число функций из системы</param>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <returns></returns>
        public static RealFunc Approx(RealFunc f, SequenceFunc p, SequenceFuncKind kind, int n, double a, double b, bool ultra = false)
        {
            double[] c = new double[n];
            RealFunc[] fi = new RealFunc[n];
            //for (int i = 0; i < n; i++)
            //{
            //    fi[i] = new RealFunc((double x) => { return p(x, i); });
            //    //Console.WriteLine(fi[i](3));
            //}
            if (ultra)
            {
                SLAU T = new SLAU(p, f, n, a, b, kind);//T.Show();
                T.begin = a; T.end = b; T.f = f; T.p = p;
                T.UltraHybrid(T.Size, kind);
                return (double x) =>
                {
                    double sum = 0;
                    for (int i = 0; i < n; i++)
                    {
                        //fi[i] = new RealFunc((double t) => { return p(t, i); });
                        sum += T.x[i] * p(x, i);
                    }
                    return sum;
                };
            }
            else
            {
                switch (kind)
                {
                    case SequenceFuncKind.Orthogonal:
                        for (int i = 0; i < n; i++)
                        {
                            fi[i] = new RealFunc((double x) => { return p(x, i); });
                            c[i] = RealFuncMethods.ScalarPower(f, fi[i], a, b) / RealFuncMethods.ScalarPower(fi[i], fi[i], a, b);//Console.WriteLine(c[3]);
                        }
                        return (double x) =>
                        {
                            double sum = 0;
                            for (int i = 0; i < n; i++)
                            {
                                //fi[i] = new RealFunc((double t) => { return p(t, i); });
                                sum += c[i] * p(x, i);
                            }
                            return sum;
                        };
                    case SequenceFuncKind.Orthonormal:
                        for (int i = 0; i < n; i++)
                        {
                            fi[i] = new RealFunc((double x) => { return p(x, i); });
                            c[i] = RealFuncMethods.ScalarPower(f, fi[i], a, b) * (b - a);
                        }
                        return (double x) =>
                        {
                            double sum = 0;
                            for (int i = 0; i < n; i++)
                            {
                                //fi[i] = new RealFunc((double t) => { return p(t, i); });
                                sum += c[i] * p(x, i);
                            }
                            return sum;
                        };
                    case SequenceFuncKind.Other:
                        SLAU T = new SLAU(p, f, n, a, b);//T.Show();
                        T.begin = a; T.end = b; T.f = f; T.p = p;
                        //T.GaussSpeedyMinimize(T.Size);
                        T.GaussSelection();
                        //T.Show();
                        return (double x) =>
                        {
                            double sum = 0;
                            for (int i = 0; i < n; i++)
                                sum += T.x[i] * p(x, i);

                            return sum;
                        };
                    default:
                        return (double x) => { return x; };
                }
            }



        }
        /// <summary>
        /// Аппроксимация функций системой полиномов на отрезке
        /// </summary>
        /// <param name="f">Функция, которую надо аппроксимировать</param>
        /// <param name="p">Полином из некоторой системы</param>
        /// <param name="kind">Класс функций в системе (ортогональные/ортонормальные/другие)</param>
        /// <param name="n">Число функций из системы</param>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <returns></returns>
        public static RealFunc Approx(RealFunc f, SequencePol p, SequenceFuncKind kind, int n, double a, double b, bool ulrta = false)
        {
            double[] c = new double[n];
            if (ulrta)
            {
                SLAU T = new SLAU(p, f, n, a, b, kind);//T.Show();
                T.begin = a; T.end = b; T.f = f; T.p = (double x, int j) => p(j).Value(x);
                T.UltraHybrid(T.Size, kind);
                return (double x) =>
                {
                    double sum = 0;
                    for (int i = 0; i < n; i++)
                    {
                        //fi[i] = new RealFunc((double t) => { return p(t, i); });
                        sum += T.x[i] * p(i).Value(x);
                    }
                    return sum;
                };
            }
            else
            {
                switch (kind)
                {
                    case SequenceFuncKind.Orthogonal:
                        for (int i = 0; i < n; i++)
                        {
                            SLAU Y = new SLAU(p, f, n, a, b);//Y.Show();
                            c[i] = RealFuncMethods.ScalarPower(f, p(i).Value, a, b) / Polynom.ScalarP(p(i), p(i), a, b);//Console.WriteLine(c[3]);
                        }
                        return (double x) =>
                        {
                            double sum = 0;
                            for (int i = 0; i < n; i++)
                            {
                                //fi[i] = new RealFunc((double t) => { return p(t, i); });
                                sum += c[i] * p(i).Value(x);
                            }
                            return sum;
                        };
                    case SequenceFuncKind.Orthonormal:
                        for (int i = 0; i < n; i++)
                        {
                            c[i] = RealFuncMethods.ScalarPower(f, p(i).Value, a, b) * (b - a);
                        }
                        return (double x) =>
                        {
                            double sum = 0;
                            for (int i = 0; i < n; i++)
                            {
                                //fi[i] = new RealFunc((double t) => { return p(t, i); });
                                sum += c[i] * p(i).Value(x);
                            }
                            return sum;
                        };
                    case SequenceFuncKind.Other:
                        SLAU T = new SLAU(p, f, n, a, b);//T.Show();
                        T.begin = a; T.end = b; T.f = f; T.p = (double x, int j) => p(j).Value(x);
                        //T.Holets(T.Size);
                        //var st = new Vectors(T.Size-1);
                        //for (int k = 1; k < T.Size; k++) st[k-1]=(new SqMatrix(T.A, k)).Det;"".Show();st.Show();
                        T.GaussSelection();
                        //T.GaussSpeedyMinimize(T.Size);
                        //T.Show();
                        return (double x) =>
                        {
                            double sum = 0;
                            for (int i = 0; i < n; i++)
                            {
                                //fi[i] = new RealFunc((double t) => { return p(t, i); });
                                sum += T.x[i] * p(i).Value(x);
                            }
                            return sum;
                        };
                    default:
                        return (double x) => { return x; };
                }
            }
        }
        /// <summary>
        /// Аппроксимация сеточной функции системой функций
        /// </summary>
        /// <param name="f">Сеточная функция, которую требуется аппроксимировать</param>
        /// <param name="p">Аппроксимирующие функции из некоторой системы</param>
        /// <param name="kind">Характер аппроксимирующих функций (ортогональные/ортонормальные/неортогональные)</param>
        /// <param name="n">Размерность системы (по умолчанию совпадает с размерностью сеточной функции)</param>
        /// <returns></returns>
        public static RealFunc Approx(NetFunc f, SequenceFunc p, SequenceFuncKind kind, int n = 0)
        {
            if (n == 0) n = f.CountKnots;
            double[] c = new double[n];
            RealFunc[] fi = new RealFunc[n];
            //switch (kind)
            //{
            //    case SequenceFuncKind.Orthogonal:
            //        for (int i = 0; i < n; i++)
            //        {
            //            fi[i] = new RealFunc((double x) => { return p(x, i); });
            //            c[i] = NetFunc.ScalarP(f, fi[i]) / NetFunc.ScalarP(fi[i], fi[i],f.Arguments);//Console.WriteLine(c[3]);
            //        }
            //        return (double x) =>
            //        {
            //            double sum = 0;
            //            for (int i = 0; i < n; i++)
            //            {
            //                sum += c[i] * p(x, i);
            //            }
            //            return sum;
            //        };
            //    case SequenceFuncKind.Orthonormal:
            //        for (int i = 0; i < n; i++)
            //        {
            //            fi[i] = new RealFunc((double x) => { return p(x, i); });
            //            c[i] = NetFunc.ScalarP(f, fi[i])*f.CountKnots;
            //        }
            //        return (double x) =>
            //        {
            //            double sum = 0;
            //            for (int i = 0; i < n; i++)
            //            {
            //                //fi[i] = new RealFunc((double t) => { return p(t, i); });
            //                sum += c[i] * p(x, i);
            //            }
            //            return sum;
            //        };
            //    case SequenceFuncKind.Other:
            SLAU T = new SLAU(p, f, n);
            T.GaussSelection();//T.Show();
            return (double x) =>
            {
                double sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += T.x[i] * p(x, i);
                }
                return sum;
            };
            //    default:
            //        return (double x) => { return x; };
            //}
        }

        public static RealFunc ApproxForLezhandr(RealFunc f, RealFunc[] masL, double a, double b)
        {
            int n = masL.Length;
            double[] c = new double[n];
            for (int i = 0; i < n; i++)
                c[i] = RealFuncMethods.ScalarPower(f, masL[i], a, b) / RealFuncMethods.ScalarPower(masL[i], masL[i], a, b);//Console.WriteLine(c[3]);

            return (double x) =>
            {
                double sum = 0;
                for (int i = 0; i < n; i++)
                    sum += c[i] * masL[i](x);
                return sum;
            };
        }

        /// <summary>
        /// Продемонстрировать аппроксимацию сеточной функции (созданной по действительной) системой функций
        /// </summary>
        /// <param name="f">Действительная функция (по которой строится сеточная)</param>
        /// <param name="c">Узлы (абциссы) для сеточной функции</param>
        /// <param name="p">Аппроксимирующие функции из некоторой системы</param>
        /// <param name="kind">Характер аппроксимирующих функций (ортогональные/ортонормальные/неортогональные)</param>
        /// <param name="n">Размерность системы (по умолчанию совпадает с размерностью сеточной функции)</param>
        /// <returns></returns>
        public static void ShowApprox(RealFunc f, double[] c, SequenceFunc p, SequenceFuncKind kind, int n = 0)
        {
            NetFunc g = new NetFunc(f, c);
            Console.WriteLine("Точки сеточной функции:"); g.Show();

            if (n == 0) n = c.Length;
            RealFunc ap = FuncMethods.Approx(g, p, kind, n);


            Console.WriteLine("Размерность системы равна {0}, размерность сеточной функции равна {1}", n, c.Length);
            Console.WriteLine("Аппроксимация исходной функции полученной функцией");
            Console.WriteLine("\t(в интегральной среднеквадратичной норме) равна {0}", FuncMethods.RealFuncMethods.NormDistance(f, ap, g.MinArg, g.MaxArg));
            Console.WriteLine("\t(в равномерной норме) равна {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, ap, g.MinArg, g.MaxArg));
            Console.WriteLine("Аппроксимация сеточной функции полученной функцией");
            Console.WriteLine("\t(в дискретной среднеквадратичной норме) равна {0}", FuncMethods.NetFunc.Distance(g, ap));
        }
        /// <summary>
        /// Продемонстрировать аппроксимацию только сеточной функции системой функций
        /// </summary>
        /// <param name="g">Сеточная функция</param>
        /// <param name="p">Аппроксимирующие функции из некоторой системы</param>
        /// <param name="kind">Характер аппроксимирующих функций (ортогональные/ортонормальные/неортогональные)</param>
        /// <param name="n">Размерность системы (по умолчанию совпадает с размерностью сеточной функции)</param>
        /// <returns></returns>
        public static void ShowApprox(NetFunc g, SequenceFunc p, SequenceFuncKind kind, int n = 0)
        {
            Console.WriteLine("Точки сеточной функции:"); g.Show();

            if (n == 0) n = g.CountKnots;
            RealFunc ap = FuncMethods.Approx(g, p, kind, n);

            Console.WriteLine("Размерность системы равна {0}, размерность сеточной функции равна {1}", n, g.CountKnots);
            Console.WriteLine("Аппроксимация сеточной функции полученной функцией");
            Console.WriteLine("\t(в дискретной среднеквадратичной норме) равна {0}", FuncMethods.NetFunc.Distance(g, ap));
        }

        /// <summary>
        /// Продемонстрировать аппроксимацию действительной функции системой функций
        /// </summary>
        /// <param name="f"></param>
        /// <param name="p"></param>
        /// <param name="kind"></param>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void ShowApprox(RealFunc f, SequenceFunc p, SequenceFuncKind kind, int n, double a, double b)
        {
            RealFunc ap = FuncMethods.Approx(f, p, kind, n, a, b);

            Console.WriteLine("Размерность системы равна {0}.", n);
            Console.WriteLine("Аппроксимация исходной функции полученной функцией");
            Console.WriteLine("\t(в интегральной среднеквадратичной норме) равна {0}", FuncMethods.RealFuncMethods.NormDistance(f, ap, a, b));
            Console.WriteLine("\t(в равномерной норме) равна {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, ap, a, b));
        }
        /// <summary>
        /// Продемонстрировать аппроксимацию действительной функции системой функций
        /// </summary>
        /// <param name="f"></param>
        /// <param name="p"></param>
        /// <param name="kind"></param>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void ShowApprox(RealFunc f, SequencePol p, SequenceFuncKind kind, int n, double a, double b)
        {
            RealFunc ap = FuncMethods.Approx(f, p, kind, n, a, b);

            Console.WriteLine("Размерность системы равна {0}.", n);
            Console.WriteLine("Аппроксимация исходной функции полученной функцией");
            Console.WriteLine("\t(в интегральной среднеквадратичной норме) равна {0}", FuncMethods.RealFuncMethods.NormDistance(f, ap, a, b));
            Console.WriteLine("\t(в равномерной норме) равна {0}", FuncMethods.RealFuncMethods.NormDistanceС(f, ap, a, b));
        }

        /// <summary>
        /// Методы для действительных функций
        /// </summary>
        public static class RealFuncMethods
        {
            /// <summary>
            /// Стандартное скалярное произведение функций
            /// </summary>
            /// <param name="f"></param>
            /// <param name="g"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double ScalarPower(RealFunc f, RealFunc g, double a, double b)
            {
                RealFunc F = (double e) => { return f(e) * g(e); };
                //double tmp = DefInteg.Simpson(F, a, b);
                double tmp = DefInteg.GaussKronrod.MySimpleGaussKronrod(F, a, b);
                if (b != a) { tmp /= Math.Abs(b - a); }
                return tmp;
            }
            /// <summary>
            /// Норма L(2)[a,b] функции через скалярное произведение
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double NormScalar(RealFunc f, double a, double b) { return Math.Sqrt(ScalarPower(f, f, a, b)); }
            /// <summary>
            /// Расстояние между функциями по норме L(2)[a,b]
            /// </summary>
            /// <param name="f"></param>
            /// <param name="g"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double NormDistance(RealFunc f, RealFunc g, double a, double b)
            {
                RealFunc t = (double x) => { return f(x) - g(x); };
                return NormScalar(t, a, b);
            }
            /// <summary>
            /// Норма функции в пространстве С[a,b]
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double NormC(RealFunc f, double a, double b)
            {
                double h = DefInteg.STEP;
                double max = Math.Abs(f(a));
                for (double t = a + h; t <= b; t += h)
                    if (Math.Abs(f(t)) > max) max = Math.Abs(f(t));
                return max;
            }
            /// <summary>
            /// Расстояние между функциями по норме С[a,b]
            /// </summary>
            /// <param name="f"></param>
            /// <param name="g"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double NormDistanceС(RealFunc f, RealFunc g, double a, double b)
            {
                RealFunc t = (double x) => { return f(x) - g(x); };
                return NormC(t, a, b);
            }
        }

        /// <summary>
        /// Матричные и векторные функции
        /// </summary>
        public class MatrixFunc<T1, T2>
        {
            public delegate Matrix Delegate(T1 t1, T2 t2);
            /// <summary>
            /// Главная функция экземпляра класса
            /// </summary>
            public Delegate Value;

            /// <summary>
            /// Нулевая матрица указанной размерности
            /// </summary>
            /// <param name="i"></param>
            /// <param name="j"></param>
            public MatrixFunc(int i, int j)
            {
                this.Value = (T1 t, T2 r) => new Matrix(i, j);
            }

            /// <summary>
            /// Создание матричной функции по делегату
            /// </summary>
            /// <param name="s"></param>
            public MatrixFunc(Delegate s) { this.Value = new Delegate(s); }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="w"></param>
            public MatrixFunc(MatrixFunc<T1, T2> w) : this(w.Value) { }

            /// <summary>
            /// Вектор-функция по массиву функций
            /// </summary>
            /// <param name="F"></param>
            public MatrixFunc(Func<T1, T2, double>[] F)
            {
                this.Value = (T1 t1, T2 t2) =>
                  {
                      Matrix M = new Matrix(F.Length, 1);
                      for (int i = 0; i < F.Length; i++)
                          M[i, 0] = F[i](t1, t2);
                      return M;
                  };
            }
            /// <summary>
            /// Матричная функция по массиву функций
            /// </summary>
            /// <param name="F"></param>
            public MatrixFunc(Func<T1, T2, double>[,] F)
            {
                this.Value = (T1 t1, T2 t2) =>
                {
                    Matrix M = new Matrix(F.GetLength(0), F.GetLength(1));
                    for (int i = 0; i < F.GetLength(0); i++)
                        for (int j = 0; j < F.GetLength(1); j++)
                            M[i, j] = F[i, j](t1, t2);
                    return M;
                };
            }

            /// <summary>
            /// Значение экземпляра класса на аргументах
            /// </summary>
            /// <param name="t1"></param>
            /// <param name="t2"></param>
            /// <returns></returns>
            public Matrix this[T1 t1, T2 t2]
            {
                get { return this.Value(t1, t2); }
            }

            public static MatrixFunc<T1, T2> operator +(MatrixFunc<T1, T2> A, MatrixFunc<T1, T2> B)
            {
                Delegate t = (T1, T2) =>
                  {
                      Matrix a = A[T1, T2];
                      Matrix b = B[T1, T2];
                      return a + b;
                  };
                return new MatrixFunc<T1, T2>(t);
            }
            public static MatrixFunc<T1, T2> operator -(MatrixFunc<T1, T2> A)
            {
                Delegate t = (T1, T2) =>
                {
                    return -A[T1, T2];
                };
                return new MatrixFunc<T1, T2>(t);
            }
            public static MatrixFunc<T1, T2> operator -(MatrixFunc<T1, T2> A, MatrixFunc<T1, T2> B) { return A + (-B); }
            public static MatrixFunc<T1, T2> operator *(MatrixFunc<T1, T2> A, MatrixFunc<T1, T2> B)
            {
                Delegate t = (T1, T2) =>
                {
                    Matrix a = A[T1, T2];
                    Matrix b = B[T1, T2];
                    return a * b;
                };
                return new MatrixFunc<T1, T2>(t);
            }

            //здесь ещё нужен метод интегрировния (в результате будет матрица)
        }

        /// <summary>
        /// Методы оптимизации
        /// </summary>
        public static class Optimization
        {
            /// <summary>
            /// Точность методов
            /// </summary>
            public static /*readonly*/ double EPS = 1e-12;
            /// <summary>
            /// Шаг
            /// </summary>
            public static /*readonly*/ double STEP = 0.0001;
            /// <summary>
            /// Максимальный шаг поиска корня
            /// </summary>
            public static double s = 2;
            //static double h=0;
            /// <summary>
            /// Контроль над тем, модифицируется функция в методе или нет
            /// </summary>
            public enum ModifyFunction
            {
                /// <summary>
                /// Да, модифицировать функцию
                /// </summary>
                Yes,
                /// <summary>
                /// Нет, не модифицировать функцию
                /// </summary>
                No
            };

            //private delegate Complex? MiniSearch(ComplexFunc f, double h, double q);
            private delegate Complex? MiniSearch(ComplexFunc f, Complex h, Complex q);
            private static Complex[] ForRootsSearching(ComplexFunc ff, Complex a, Complex b, MiniSearch M, double s = 0, ModifyFunction Mod = ModifyFunction.No)
            {
                var roots = new List<Complex?>();
                //double h = a.Re;
                Complex h = a;
                if (s <= 0) s = Optimization.s;
                //double step = s;
                Complex step = (b - a) / (b - a).Abs * s;
                ComplexFunc f = new ComplexFunc(ff);

                while (/*h <= b.Re - STEP*/(h - b + (b - a) / (b - a).Abs * STEP).Abs >= EPS && (b - h).Abs < (b - h + step).Abs)//пройти по всему отрезку
                {
                    step = s;
                    while (/*step >= STEP*/(step.Abs - STEP) >= EPS /*&& h <= b.Re - STEP*/)//пока шаг не достаточно маленький
                    {
                        while (!ExistRootConidtion(f, h, h + step) && /*h <= b.Re - STEP*/(h - b + (b - a) / (b - a).Abs * STEP).Abs >= EPS && (b - h).Abs < (b - h + step).Abs)
                        {
                            //Console.WriteLine($"!ExistRootConidtion(f, h, h + step){!ExistRootConidtion(f, h, h + step)} {h} <= {b.Re - STEP}");
                            h += step;
                            //Console.WriteLine($"h={h} \t|f(h)|={ff(h).Abs} \tstep={step}");  /*f(h).Show();h.Show(); */
                        }//доходить до корня
                        step /= 2;
                        //ExistRootConidtion(f, h, h + step).Show();
                    }
                    // try
                    //{
                    if (ExistRootConidtion(f, h, h + 2 * step))//во избежание повторов корней
                    {
                        //"+++++++++Добралось до поиска корня++++++++++".Show();
                        try { roots.Add(M(f, h, h + 2 * step)); } catch { $"Исключение на поиске корня в отрезке [{h} , {h + 2 * step}]".Show(); }
                        //if (roots.Last() != null) Console.WriteLine($"корень={roots.Last()} значение в корне по модулю = {ff((Complex)roots.Last()).Abs}");
                        if (Mod == ModifyFunction.Yes)
                        {
                            f = (Complex z) =>
                              {
                                  Complex p = new Complex(1, 0);
                                  for (int i = 0; i < roots.Count; i++)
                                      if (roots[i] != null)
                                          p *= (Complex)(z - roots[i]);
                                  return ff(z) / p;
                              };
                            h -= 2 * step;
                        }
                    }

                    h += 2 * step;//если ищутся кратные корни, надо проходить участок заново, а иначе идти дальше
                                  //}
                                  //catch(Exception e) { throw new Exception(e.Message); }
                }

                //преобразовать массив корней
                roots.RemoveAll(t => t == null);
                var c = new Complex[roots.Count];
                for (int i = 0; i < c.Length; i++)
                    c[i] = (Complex)roots[i];
                Array.Sort(c);
                if (Mod == ModifyFunction.No) c.Distinct();//если кратные корни не могут искаться, отсеять повторы
                return c;
            }

            private static bool ExistRootConidtion(ComplexFunc f, Complex a, Complex b)
            {
                Complex x1 = f(a), x2 = f(b);
                return (Math.Sign((x1.Re) * (x2.Re)) <= 0 && (Math.Sign((x1.Im) * (x2.Im)) <= 0));
                //return (Math.Sign((x1.Re/Math.Abs(x1.Re)) * (x2.Re / Math.Abs(x2.Re))) <= 0 && (Math.Sign((x1.Im / Math.Abs(x1.Im)) * (x2.Im / Math.Abs(x2.Im)))<= 0))/*|| x1.Im * x2.Im==0|| x1.Re * x2.Re==0*/;
            }
            /// <summary>
            /// Метод бисекции(дихотомии)
            /// </summary>
            /// <param name="f">Комплексная функция</param>
            /// <param name="a">Начало отрезка поиска корня</param>
            /// <param name="b">Конец отрезка поиска корня</param>
            /// <returns></returns>
            public static Complex[] Bisec(ComplexFunc f, Complex a, Complex b, double s = 0, ModifyFunction Mod = ModifyFunction.No)
            {
                return ForRootsSearching(f, a, b, MiniBisec, s, Mod);
            }
            private static Complex? MiniBisec(ComplexFunc f, double a, double b) { return MiniBisec(f, (Complex)a, (Complex)b); }
            private static Complex? MiniBisec(ComplexFunc f, Complex a, Complex b)
            {
                Complex v = new Complex();
                //if (!ExistRootConidtion(f, a, b)) throw new Exception("Между этими точками не существует корня!");
                if (!ExistRootConidtion(f, a, b)) return null;
                v = Expendator.Average(a, b);
                while (f(v).Abs > EPS && (b - a).Abs > EPS)
                {
                    //Console.WriteLine($"\t a={a} \tb={b} \tf(a)={f(a)} \tf(b)={f(b)}");
                    //Console.WriteLine($"\t a-b={a-b} \tv={v} f(v)={f(v)}");
                    //Console.WriteLine();

                    if (ExistRootConidtion(f, v, b))
                        a = v;
                    else
                        b = v;
                    v = Expendator.Average(a, b);
                }
                return v;
            }

            internal class PointC
            {
                internal Complex x, y;
                internal PointC(Complex x, Complex y) { this.x = x; this.y = y; }
            }

            private static Complex W(PointC[] p)
            {
                Complex sum = new Complex(0, 0), pow = new Complex(1, 0);
                for (int j = 0; j < p.Length; j++)
                {
                    for (int l = 0; l < p.Length; l++)
                        if (j != l) pow *= p[j].x - p[l].x;
                    sum += p[j].y / pow;
                    pow = 1;
                }
                return sum;
            }
            /// <summary>
            /// Разделённая разность по массиву точек (с рекурсией)
            /// </summary>
            /// <param name="p">Массив точек</param>
            /// <param name="i">Номер начального элемента в разности</param>
            /// <param name="j">Номер конечного элемента в разности</param>
            /// <returns></returns>
            private static Complex W(PointC[] p, int i, int j)
            {
                if (j - i == 1) return (p[j].y - p[i].y) / (p[j].x - p[i].x);
                return (W(p, i + 1, j) - W(p, i, j - 1)) / (p[j].x - p[i].x);
            }

            /// <summary>
            /// Поиск всех корней комплексной функции с действительной частью на указанном действительном отрезке
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static Complex[] FullMuller(ComplexFunc f, Complex a, Complex b, double s = 0, ModifyFunction Mod = ModifyFunction.No)
            {
                return ForRootsSearching(f, a, b, ForFullMuller, s, Mod);
                //var roots = new List<Complex?>();
                //double h = a.Re;
                //double step = s;

                //while (h <= b.Re - STEP)//пройти по всему отрезку
                //{
                //    //Console.WriteLine($"{h} <= {b.Re - STEP}");
                //    step = s;
                //    while (step > 10*STEP /*&& h <= b.Re - STEP*/)//пока шаг не достаточно маленький
                //    {
                //        while (!ExistRootConidtion(f, h, h + step) && h <= b.Re - STEP)
                //        {
                //            h += step;
                //            //Console.WriteLine($"h={h} f(h)={f(h)}");  /*f(h).Show();h.Show(); Console.WriteLine($"{h} <= {b.Re - STEP}");*/
                //        }//доходить до корня
                //        step /= 2;

                //    } 
                //    roots.Add(ForFullMuller(f, h, h + STEP));
                //    if (roots.Last() != null) Console.WriteLine($"корень={roots.Last()} значение в корне = {f((Complex)roots.Last())}");
                //    h += STEP;
                //}

                //roots.RemoveAll(t => t == null);
                //var c = new Complex[roots.Count];
                //for (int i = 0; i < c.Length; i++)
                //    c[i] = (Complex)roots[i];
                //return c;
            }

            private static Complex? ForFullMuller(ComplexFunc f, /*double*/ Complex h1, /*double*/ Complex h2)
            {
                double step = (h2 - h1).Abs;
                Complex z1 = new Complex(h1.Re, 0), z2 = new Complex(h1.Re, -step / 10), z3 = new Complex(h1.Re, step / 10);
                // try
                //{
                Complex res = Muller(f, z1, z2, z3);
                /*if (res.Re >= h1 && res.Re <= h2)*/
                return res;

                //}
                //catch(Exception e) { throw new Exception(e.Message); }

                return null;
            }

            /// <summary>
            /// Метод Мюллера (парабол) поиска нуля функции
            /// </summary>
            /// <param name="f">Функция комплексного переменного</param>
            /// <param name="x1">Первая точка</param>
            /// <param name="x2">Вторая точка</param>
            /// <param name="x3">Третья точка</param>
            /// <returns></returns>
            public static Complex Muller(ComplexFunc f, Complex x1, Complex x2, Complex x3)
            {
                PointC[] mas = new PointC[]
                {
                    new PointC(x1,f(x1)),
                    new PointC(x2,f(x2)),
                    new PointC(x3,f(x3))
                };
                Complex w = W(new PointC[] { mas[2], mas[1] }) + W(new PointC[] { mas[2], mas[0] }) - W(new PointC[] { mas[1], mas[0] });
                Complex f123 = W(mas);
                Complex fk = f(x3);
                Complex xk1;
                Complex[] roots = new Complex[2];

                do
                {
                    try
                    {
                        roots[0] = x3 - 2 * fk / (w + Complex.Sqrt(w * w - 4 * fk * f123));
                    }
                    catch (Exception e) { throw new Exception(e.Message); }
                    roots[1] = x3 - 2 * fk / (w - Complex.Sqrt(w * w - 4 * fk * f123));
                    xk1 = MinUnder(roots, x3);

                    x1 = x2;
                    x2 = x3;
                    x3 = xk1;
                    mas = new PointC[]
                      {
                    new PointC(x1,f(x1)),
                    new PointC(x2,f(x2)),
                    new PointC(x3,f(x3))
                    };
                    fk = f(x3);
                    f123 = W(mas);
                    w = W(new PointC[] { mas[2], mas[1] }) + W(new PointC[] { mas[2], mas[0] }) - W(new PointC[] { mas[1], mas[0] });
                } while (fk.Abs > EPS && (x3 - x2).Abs > EPS /*&&(fk.Abs<f(x2).Abs)*/);

                return xk1;

            }
            /// <summary>
            /// Наименее отклоняющаяся от комплексного числа точка из массива 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="z"></param>
            /// <returns></returns>
            private static Complex MinUnder(Complex[] x, Complex z)
            {
                double v = (x[0] - z).Abs;
                Complex val = x[0];
                for (int i = 0; i < x.Length; i++)
                    if ((x[i] - z).Abs < v)
                    {
                        v = (x[i] - z).Abs;
                        val = x[i];
                    }
                return val;
            }

            /// <summary>
            /// Метод хорд поиска корня
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static Complex[] Chord(ComplexFunc f, Complex a, Complex b, double s = 0, ModifyFunction Mod = ModifyFunction.No) { return ForRootsSearching(f, a, b, ForChord, s, Mod); }
            private static Complex? ForChord(ComplexFunc f, Complex x0, Complex x1)
            {
                Complex xn = new Complex();
                Complex fx0 = f(x0);
                do
                {
                    Complex fx1 = f(x1);
                    xn = x1 - fx1 / (fx1 - fx0) * (x1 - x0);
                    x0 = x1;
                    x1 = xn;
                } while (/*f(xn).Abs > EPS &&*/ (x0 - x1).Abs > EPS);
                return xn;
            }
            private static Complex? ForChord(ComplexFunc f, double x0, double x1) => ForChord(f, (Complex)x0, (Complex)x1);

            private static Complex Derivative(ComplexFunc f, Complex x) => (f(x + EPS) - f(x)) / EPS;
            /// <summary>
            /// Вариация метода
            /// </summary>
            public enum Variety
            {
                /// <summary>
                /// Простейшая вариация метода
                /// </summary>
                Simple,
                /// <summary>
                /// Традиционная вариация метода
                /// </summary>
                Usual
            }
            /// <summary>
            /// Метод Ньютона поиска корня
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static Complex[] Neu(ComplexFunc f, Complex a, Complex b, Variety va = Variety.Usual, double s = 0, ModifyFunction Mod = ModifyFunction.No)
            {
                Complex? F(ComplexFunc q, /*double*/ Complex w, /*double*/ Complex e) => ForNeu(f, w, e, va);
                return ForRootsSearching(f, a, b, F, s, Mod);
            }
            private static Complex? ForNeu(ComplexFunc f, Complex a, Complex b, Variety va = Variety.Usual)
            {
                Complex x0 = b, xn;
                if (va == Variety.Usual) do
                    {
                        xn = x0 - f(x0) / Derivative(f, x0);
                        x0 = xn;
                    } while (f(xn).Abs > EPS);
                else
                {
                    Complex der = Derivative(f, x0);
                    do
                    {
                        xn = x0 - f(x0) / der;
                        x0 = xn;
                    } while (f(xn).Abs > EPS);
                }
                return xn;
            }
            private static Complex? ForNeu(ComplexFunc f, double x0, double x1, Variety va = Variety.Usual) => ForNeu(f, (Complex)x0, (Complex)x1, va);

            /// <summary>
            /// Комбинированный метод поиска корня
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static Complex[] ChordNeu(ComplexFunc f, Complex a, Complex b, double s = 0, ModifyFunction Mod = ModifyFunction.No) { return ForRootsSearching(f, a, b, ForChordNeu, s, Mod); }
            private static Complex? ForChordNeu(ComplexFunc f, Complex a, Complex b)
            {
                Complex xl0 = a, xr0 = b, xl, xr;
                do
                {
                    xr = xr0 - f(xr0) / Derivative(f, xl0);
                    xl = xl0 - f(xl0) / (f(xr0) - f(xl0)) * (xr0 - xl0);
                    xr0 = xr; xl0 = xl;
                } while ((xl - xr).Abs > EPS);
                return Expendator.Average(xr, xl);
            }
            private static Complex? ForChordNeu(ComplexFunc f, double x0, double x1) => ForChordNeu(f, (Complex)x0, (Complex)x1);

            private static Complex[] GlobalMin(ComplexFunc f, Complex[] p)
            {
                Complex c = p[0];
                for (int i = 1; i < p.Length; i++)
                    if (f(p[i]).Abs < f(c).Abs)
                        c = p[i];

                return p.Where(n => f(n).Abs == f(c).Abs).ToArray();
            }
            private static Complex[] GlobalMax(ComplexFunc f, Complex[] p)
            {
                var r = new List<Complex>();

                Complex c = p[0];
                for (int i = 1; i < p.Length; i++)
                    if (f(p[i]).Abs > f(c).Abs)
                        c = p[i];

                return r.Where(n => f(n).Abs == f(c).Abs).ToArray();
            }
            private static bool ExistMin(ComplexFunc f, Complex a, Complex b)
            {
                //Complex x1 = Derivative(f, a), x2 = Derivative(f, b);
                //    return (Math.Sign((x1.Re) * (x2.Re)) <= 0 && (Math.Sign((x1.Im) * (x2.Im)) <= 0));
                Complex ff(Complex z) => Derivative(f, z);
                return ff(a).Re <= 0 && ff(b).Re > 0;
            }

            private delegate Complex? FuncMinSearch(ComplexFunc f, Complex h, Complex q);
            /// <summary>
            /// Поиск экстремумов функции указанным методом
            /// </summary>
            /// <param name="ff">Функция комплексного переменного</param>
            /// <param name="a">Начало отрезка поиска</param>
            /// <param name="b">Конец отрезка поиска</param>
            /// <param name="M">Метод локального поиска</param>
            /// <param name="s">Максимальный шаг поиска</param>
            /// <returns></returns>
            public static Complex[] MinSearch(RealFuncOfCompArg ff, Complex a, Complex b, MinimumVar M, double s = 0)
            {
                var roots = new List<Complex?>();
                double h = a.Re;
                if (s <= 0) s = Optimization.s;
                double step = s;
                ComplexFunc f = Expendator.ToCompFunc(ff);

                while (h <= b.Re - STEP)//пройти по всему отрезку
                {
                    step = s;
                    while (step >= STEP /*&& h <= b.Re - STEP*/)//пока шаг не достаточно маленький
                    {
                        while (!ExistMin(f, h, h + step) && h <= b.Re - STEP && Derivative(f, h).Abs > EPS)
                        {
                            h += step;
                            //Console.WriteLine($"h={h} \t|f`(h)|={Derivative(f,h).Abs} \tstep={step}");  /*f(h).Show();h.Show(); */
                        }//доходить до корня
                        step /= 2;
                        //ExistRootConidtion(f, h, h + step).Show();
                    }
                    //"Тут0".Show();
                    //ExistMin(f, -Math.PI/2-STEP, -Math.PI / 2 + STEP).Show();
                    // try
                    //{
                    if (ExistMin(f, h, h + 2 * step))//во избежание повторов корней
                    {
                        //"+++++++++Добралось до поиска корня++++++++++".Show();
                        roots.Add(MinimumSearch(ff, h, h + 2 * step, M));
                        //if (roots.Last() != null) Console.WriteLine($"корень={roots.Last()} значение в корне по модулю = {ff((Complex)roots.Last()).Abs}");
                    }

                    h += 2 * step;//если ищутся кратные корни, надо проходить участок заново, а иначе идти дальше
                                  //}
                                  //catch(Exception e) { throw new Exception(e.Message); }
                }

                //преобразовать массив минимумов
                roots.RemoveAll(t => t == null);
                var c = new Complex[roots.Count];
                for (int i = 0; i < c.Length; i++)
                    c[i] = (Complex)roots[i];
                Array.Sort(c);
                c.Distinct();
                return c;
            }
            /// <summary>
            /// Методы поиска минимума
            /// </summary>
            public enum MinimumVar
            {
                /// <summary>
                /// Метод золотого сечения
                /// </summary>
                GoldSection
            }
            /// <summary>
            /// Поиск минимума функции указанным методом
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="M"></param>
            /// <returns></returns>
            public static Complex? MinimumSearch(RealFuncOfCompArg f, Complex a, Complex b, MinimumVar M)
            {
                Complex? k;
                switch (M)
                {
                    default:
                        k = GoldSect(f, a, b);
                        break;
                }
                return k;
            }
            /// <summary>
            /// Метод золотого сечения поиска минимума функции на указанном отрезке
            /// </summary>
            /// <param name="f"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static Complex? GoldSect(RealFuncOfCompArg f, Complex a, Complex b)
            {
                ComplexFunc ff = Expendator.ToCompFunc(f);
                //"Тут".Show();
                if (Derivative(ff, a).Abs < EPS) return a;
                if (Derivative(ff, b).Abs < EPS) return b;

                double e = 2.0 / (3 + Math.Sqrt(5));
                Complex x2 = a + e * (b - a), x3 = b - e * (b - a);
                //if (f(x2).Abs > f(a).Abs || f(x3).Abs > f(b).Abs) return null;

                if (!(Derivative(ff, a).Re < 0 && Derivative(ff, b).Re > 0)) return null;

                while ((b - a).Abs > EPS && NotExter(f, a, b, x2, x3))
                {
                    //if (f(x2).Abs < EPS) return x2;
                    //if (f(x3).Abs < EPS) return x3;

                    Complex[] mas = new Complex[] { a, x2, x3, b }; //mas.Show();Console.WriteLine();
                    Complex t1 = GlobalMin(Expendator.ToCompFunc(f), mas)[0];
                    Complex[] newmas = mas.Where(n => n != t1).ToArray();
                    Complex t2 = GlobalMin(Expendator.ToCompFunc(f), newmas)[0];
                    //newmas = mas.Where(n => n != t2).ToArray();
                    //a = newmas[0];b = newmas[1];
                    if (t1 == a && t2 == b || t2 == a && t1 == b) return Expendator.Average(a, b);
                    a = t1; b = t2;
                    x2 = a + e * (b - a); x3 = b - e * (b - a);
                }

                return Exter(f, a, b, x3, x2);
                //return Expendator.Average(x2, x3);
            }

            private static bool NotExter(RealFuncOfCompArg f, params Complex[] z)
            {
                ComplexFunc ff = Expendator.ToCompFunc(f);
                for (int i = 0; i < z.Length; i++)
                    if (Derivative(ff, z[i]).Abs < EPS)
                        return false;
                return true;
            }
            /// <summary>
            /// Вернуть из нескольких чисел наиболее близкую к экстремуму точку
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            private static Complex Exter(RealFuncOfCompArg f, params Complex[] z)
            {
                ComplexFunc ff = Expendator.ToCompFunc(f);
                Complex tmp = z[0];
                for (int i = 1; i < z.Length; i++)
                    if (Derivative(ff, z[i]).Abs < Derivative(ff, tmp).Abs)
                        tmp = z[i];
                return tmp;
            }

            /// <summary>
            /// Презентация работы группы методов поиска корня
            /// </summary>
            /// <param name="f">Функция комплексного переменного</param>
            /// <param name="a">Начало отрезка поиска</param>
            /// <param name="b">Конец отрезка поиска</param>
            /// <param name="s">Максимальный шаг поиска</param>
            /// <param name="Mod">Искать/не искать кратные корни</param>
            public static void Presentaion(ComplexFunc f, Complex a, Complex b, double s = 0, ModifyFunction Mod = ModifyFunction.No)
            {
                Console.WriteLine("----------------Презентация работы группы методов поиска корня------------");
                Console.WriteLine($"\tОтрезок поиска корня: [{a};{b}]");
                if (s != 0) Console.WriteLine($"\tМаксимальный шаг при поиске: {s}");
                else Console.WriteLine($"\tМаксимальный шаг при поиске: {Optimization.s}");
                if (Mod == ModifyFunction.No) Console.WriteLine("\t----Ищутся разные корни");
                else Console.WriteLine("\t----Ищутся все корни, включая кратные");

                Complex[] k = Bisec(f, a, b, s, Mod); GetText(f, k, "бисекции");
                k = FullMuller(f, a, b, s, Mod); GetText(f, k, "Мюллера");
                k = Chord(f, a, b, s, Mod); GetText(f, k, "хорд");
                k = Neu(f, a, b, Variety.Simple, s, Mod); GetText(f, k, "Ньютона (урощённый)");
                k = Neu(f, a, b, Variety.Usual, s, Mod); GetText(f, k, "Ньютона (стандартный)");
                k = ChordNeu(f, a, b, s, Mod); GetText(f, k, "комбинированным хорд-Ньютона");

            }
            private static void GetText(ComplexFunc f, Complex[] k, string s)
            {
                Console.WriteLine("------Корни, найденные методом {0}:", s);
                for (int i = 0; i < k.Length; i++)
                    try { Console.WriteLine($"Корень: {k[i]} ; значение в корне (по модулю): {f(k[i]).Abs}"); } catch { "вось".Show(); }
                //k.Show();
            }

            /// <summary>
            /// Ключевая точка
            /// </summary>
            public enum CriticalPoint
            {
                /// <summary>
                /// Корень
                /// </summary>
                Root,
                /// <summary>
                /// Минимум
                /// </summary>
                Minimum
            }

            /// <summary>
            /// Методы поиска корня
            /// </summary>
            public enum RootSearchMethod
            {
                /// <summary>
                /// Метод бисекции
                /// </summary>
                Bisec,
                /// <summary>
                /// Метод парабол
                /// </summary>
                Muller,
                /// <summary>
                /// Метод хорд
                /// </summary>
                Chord,
                /// <summary>
                /// Метод Ньютона
                /// </summary>
                Neu,
                /// <summary>
                /// Комбинированный метод хорд-Ньютона
                /// </summary>
                ChordNeu
            }
            /// <summary>
            /// Поиск корня комплексной функции в прямоугольнике
            /// </summary>
            /// <param name="ff">Комплексная функция</param>
            /// <param name="a">Правый верхний угол прямоугольника</param>
            /// <param name="b">Левый верхний угол прямоугольника</param>
            /// <param name="c">Левый нижний угол прямоугольника</param>
            /// <param name="d">Правый нижний угол прямоугольника</param>
            /// <param name="M">Метод поиска корня на отрезке</param>
            /// <param name="s">Шаг в методе поиска корня на отрезке</param>
            /// <param name="sh">Шаг параллельных прямых по прямоугольнику</param>
            /// <returns></returns>
            public static Complex[] SearchRoots(ComplexFunc ff, Complex a, Complex b, Complex c, Complex d, RootSearchMethod M = RootSearchMethod.ChordNeu, double s = 0, double sh = 0.1)
            {
                if (s == 0) s = Optimization.s;
                if (sh == 0) sh = Optimization.s;
                var res = new List<Complex>();
                Complex st = (d - a) / (d - a).Abs * sh;
                Complex sr = (a - b) / (a - b).Abs * sh / (d - a).Abs;
                MiniSearch S;
                switch (M)
                {
                    case RootSearchMethod.Bisec:
                        S = MiniBisec;
                        break;
                    case RootSearchMethod.Chord:
                        S = ForChord;
                        break;
                    case RootSearchMethod.ChordNeu:
                        S = ForChordNeu;
                        break;
                    case RootSearchMethod.Neu:
                        S = /*ForNeu*/(ComplexFunc f, Complex at, Complex bt) => ForNeu(f, at, bt, Variety.Usual);
                        break;
                    default:
                        S = ForFullMuller;
                        break;

                }
                res.AddRange(ForRootsSearching(ff, a, b, S, s));//res.Show();
                Complex x = a, y = b;
                int i = 0;
                while ((x - d).Abs > EPS && (d - x + st).Abs > (x - d).Abs)
                {
                    x += st;//x.Show();
                    y += sr;
                    i++; if (i == 1000) { x.Show(); i = 0; }
                    res.AddRange(ForRootsSearching(ff, b + x - a, x, S, s));
                    //res.AddRange(ForRootsSearching(ff, y, c+y-b, S, s));
                }
                res = new List<Complex>(res.Distinct());
                return res.ToArray();
            }


            private static void Halfc(ComplexFunc F, double tmin, double tmax, double step, double eps, int Nmax, out double[] dz, int Nx = 0)
            {
                int it, ir, ii;
                double t1, t2, rf1, rf2, if1, if2, epsf, signr, signi, u1, u2, u, sgr, sgi, rfu1, rfu2, ifu1, ifu2, rfu, ifu;
                dz = new double[Nmax + 1];
                Complex ft;
                Nx = 0; it = 1; epsf = eps / 1e7;

                t1 = tmin; ft = F(t1);
                rf1 = ft.Re; if1 = ft.Im;

                g1: t2 = t1 + step;
                if (t2 > tmax)
                {
                    t2 = tmax; it = -1;
                }

                ft = F(t2); rf2 = ft.Re; if2 = ft.Im;

                if (Math.Abs(rf2) < epsf)
                { signr = -1; ir = -1; }
                else
                { signr = rf1 * rf2; ir = 1; }

                if (Math.Abs(if2) < epsf)
                { signi = -1; ii = -1; }
                else
                { signi = if1 * if2; ii = 1; }

                if ((signr < 0) && (signi < 0))
                {
                    if ((ir < 0) && (ii < 0))
                    {
                        Nx += 1;
                        dz[Nx] = t2;//F(t2).Show(); этот не косячит
                        goto g2;
                    }

                    u1 = t1; rfu1 = rf1; ifu1 = if1;

                    u2 = t2; rfu2 = rf2; ifu2 = if2;

                    g3: u = (u1 + u2) / 2; ft = F(u);

                    rfu = ft.Re; ifu = ft.Im;

                    sgr = rfu1 * rfu; sgi = ifu1 * ifu;

                    if ((ir > 0) && (ii > 0) && (sgr * sgi < 0)) goto g2;
                    //else
                    if (ir > 0)
                    {
                        if (sgr > 0)
                        {
                            u1 = u; rfu1 = rfu; ifu1 = ifu;
                        }
                        else
                        {
                            u2 = u; rfu2 = rfu; ifu2 = ifu;
                        }
                    }
                    else if (sgi > 0)
                    {
                        u1 = u; rfu1 = rfu; ifu1 = ifu;
                    }
                    else
                    {
                        u2 = u; rfu2 = rfu; ifu2 = ifu;
                    }

                    if (Math.Abs(u1 - u2) > eps) goto g3;
                    //if (F(u).Abs < 1) { Nx += 1; dz[Nx] = u; }
                    Nx += 1; dz[Nx] = u; //F(u).Show();//этот выбор иногда косячит
                }
                g2: t1 = t2; rf1 = rf2; if1 = if2;
                if ((Nx < Nmax) && (it > 0)) goto g1;

            }
            /// <summary>
            /// Массив действительных корней комплексной функции на отрезке
            /// </summary>
            /// <param name="f">Функция</param>
            /// <param name="tmin">Начало отрезка</param>
            /// <param name="tmax">Конец отрезка</param>
            /// <param name="step">Шаг</param>
            /// <param name="eps">Точность</param>
            /// <param name="Nmax">Число итераций</param>
            /// <returns></returns>
            public static Vectors Halfc(ComplexFunc f, double tmin, double tmax, double step, double eps, int Nmax = 10)
            {
                double[] res;
                Halfc(f, tmin, tmax, step, eps, Nmax, out res);
                res = res.Distinct().ToArray();
                Array.Sort(res);
                return new Vectors(res);
            }

            
        }

        /// <summary>
        /// Класс интегральных преобразований
        /// </summary>
        public static class IntegralTransformations
        {
            public static ComplexFunc Furier(RealFunc f)
            {
                return (Complex w) =>
                {
                    ComplexFunc fe = (Complex x) => f(x.Re) * Complex.Exp(Complex.I * x * w);
                    return DefInteg.GaussKronrod.IntegralInf(fe, -20, 20);
                };
            }
            public static RealFunc FurierRevers(ComplexFunc f, ComplexFunc delta = null, int t = 100)
            {
                return (double x) =>
                {
                    ComplexFunc fe = (Complex w) => f(w) * Complex.Exp(-Complex.I * x * w);
                    return 1.0 / (2 * Math.PI) * DefInteg.GaussKronrod.IntegralInf(fe, -1, 1, delta, t);
                };
            }

            public static void Test(RealFunc f, double arg)
            {
                ComplexFunc f1 = Furier(f); //f1(arg).Abs.Show();
                RealFunc f2 = FurierRevers(f1); f2(arg).Show();
                (f(arg) - f2(arg)).Show();
            }
        }
    }

    /// <summary>
    /// Класс СЛАУ с методами их решения
    /// </summary>
    public class SLAU
    {
        private int size;
        /// <summary>
        /// Размерность системы
        /// </summary>
        public int Size => size;

        /// <summary>
        /// Матрица системы
        /// </summary>
        public double[,] A;
        /// <summary>
        /// Свободный вектор системы
        /// </summary>
        public double[] b;
        /// <summary>
        /// Вектор решения
        /// </summary>
        public double[] x;

        /// <summary>
        /// Невязка решения
        /// </summary>
        public static double? NEVA = null;

        /// <summary>
        /// Класс функций для элементов системы (написано в процедурном стиле, перенёс из курсача, так как матрица системы - это массив массивов)
        /// </summary>
        public static class Func_in_matrix
        {
            /// <summary>
            /// Частичное произведение матрицы на вектор
            /// </summary>
            /// <param name="Ax"></param>
            /// <param name="a"></param>
            /// <param name="x"></param>
            /// <param name="k"></param>
            public static void Matrix_power(ref double[] Ax, double[,] a, double[] x, int k)
            {
                for (int i = 0; i < k; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < k; j++)
                    {
                        sum += a[i, j] * x[j];
                    }
                    Ax[i] = sum;
                }
            }
            /// <summary>
            /// Разность двух векторов
            /// </summary>
            /// <param name="r"></param>
            /// <param name="Ax"></param>
            /// <param name="b"></param>
            /// <param name="t"></param>
            public static void Vector_difference(ref double[] r, double[] Ax, double[] b, int t)
            {
                for (int i = 0; i < t; i++)
                {
                    r[i] = Ax[i] - b[i];
                }
            }
            /// <summary>
            /// Классическое скалярное произведение двух векторов
            /// </summary>
            /// <param name="r"></param>
            /// <param name="rr"></param>
            /// <param name="t"></param>
            /// <returns></returns>
            public static double Scalar_power(double[] r, double[] rr, int t)
            {
                double s = 0;
                for (int i = 0; i < t; i++)
                {
                    s += r[i] * rr[i];
                }
                return s;
            }
            /// <summary>
            /// Умножение вектора на скаляр
            /// </summary>
            /// <param name="s"></param>
            /// <param name="tau"></param>
            /// <param name="r"></param>
            /// <param name="t"></param>
            public static void Vector_on_scalar(ref double[] s, double tau, double[] r, int t)
            {
                for (int i = 0; i < t; i++)
                {
                    s[i] = tau * r[i];
                }
            }
            /// <summary>
            /// Сумма векторов
            /// </summary>
            /// <param name="sum"></param>
            /// <param name="s"></param>
            /// <param name="x"></param>
            /// <param name="t"></param>
            public static void Vector_sum(ref double[] sum, double[] s, double[] x, int t)
            {
                for (int i = 0; i < t; i++)
                {
                    sum[i] = x[i] + s[i];
                }
            }
            /// <summary>
            /// Присваивание одному вектору другого
            /// </summary>
            /// <param name="x"></param>
            /// <param name="s"></param>
            /// <param name="t"></param>
            public static void Vector_assingment(ref double[] x, double[] s, int t)
            {
                Vector_on_scalar(ref x, 1, s, t);
            }
        }

        /// <summary>
        /// Функция частичной невязки
        /// </summary>
        /// <param name="A"></param>
        /// <param name="x"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double Nev(double[,] A, double[] x, double[] b, int t)
        {
            double[] Ax = new double[t];
            Func_in_matrix.Matrix_power(ref Ax, A, x, t);
            double s = 0;
            for (int i = 0; i < t; i++)
            {
                s += ((Ax[i] - b[i]) * (Ax[i] - b[i]));
            }
            return Math.Sqrt(s);
        }
        /// <summary>
        /// Частичная невязка используемой системы
        /// </summary>
        /// <param name="t">Размерность подсистемы</param>
        /// <returns></returns>
        public double Nev(int t) { return SLAU.Nev(this.A, this.x, this.b, t); }
        public double Nevaska => Nev(this.size);
        /// <summary>
        /// Невязка системы
        /// </summary>
        public double Discrep
        {
            get { return this.Nev(this.size); }
        }

        //public double Error(int k) //частичная погрешность
        //{
        //    double p = myCurve.Firstkind(N, N);
        //    double sum = 0;

        //    double[] Ax = new double[N];
        //    Func_in_matrix.Matrix_power(Ax, A, x, k);
        //    for (int i = 0; i < k; i++)
        //    {
        //        sum += x[i] * Ax[i];
        //    }
        //    double EPS = Math.Abs(p - sum);
        //    return Math.Sqrt(EPS);
        //}

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SLAU() { Make(1); }
        /// <summary>
        /// Прочитать систему из файла
        /// </summary>
        /// <param name="fs"></param>
        public SLAU(StreamReader fs)//конструктор через файл
        {
            string s = fs.ReadLine();
            try//standart
            {
                string[] st = s.Split(' ');
                //if(st.Length==2 &&)
                Make(Convert.ToInt32(st[0]));

                for (int k = 0; k < this.size; k++)
                {
                    s = fs.ReadLine();
                    st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа
                    for (int i = 0; i < this.size; i++) this.A[k, i] = Convert.ToDouble(st[i]);
                }
                s = fs.ReadLine();
                //for (int k = 0; k < this.size; k++) { s = fs.ReadLine(); st = s.Split(' '); this.x[k] = Convert.ToDouble(st[0]); }
                //s = fs.ReadLine();
                for (int k = 0; k < this.size; k++) { s = fs.ReadLine(); st = s.Split(' '); this.b[k] = Convert.ToDouble(st[0]); }
            }
            catch
            {
                string[] st = s.Split(' ', '|', '\t');
                st = st.Where(n => n.Length > 0).ToArray();
                try//with vector x
                {
                    Make(Convert.ToInt32(st.Length - 2));
                    for (int i = 0; i < this.size; i++) this.A[0, i] = Convert.ToDouble(st[i]);
                    this.x[0] = Convert.ToDouble(st[st.Length - 2]);
                    this.b[0] = Convert.ToDouble(st[st.Length - 1]);
                    for (int k = 1; k < this.size; k++)
                    {
                        s = fs.ReadLine();
                        st = s.Split(' ', '|', '\t');//в аргументах указывается массив символов, которым разделяются числа
                        st = st.Where(n => n.Length > 0).ToArray();
                        for (int i = 0; i < this.size; i++) this.A[k, i] = Convert.ToDouble(st[i]);
                        this.x[k] = Convert.ToDouble(st[st.Length - 2]);
                        this.b[k] = Convert.ToDouble(st[st.Length - 1]);
                    }
                    s = fs.ReadLine();
                    st = s.Split(' ', '|', '\t');
                    st = st.Where(n => n.Length > 0).ToArray();

                    if (st.Length != 0) throw new Exception("Вектор x отсутствует");
                }
                catch//without vector x
                {
                    fs.BaseStream.Position = 0;
                    s = fs.ReadLine();
                    st = s.Split(' ', '|', '\t');
                    st = st.Where(n => n.Length > 0).ToArray();
                    //st.Show();
                    Make(Convert.ToInt32(st.Length - 1));
                    for (int i = 0; i < this.size; i++) this.A[0, i] = Convert.ToDouble(st[i]);
                    this.b[0] = Convert.ToDouble(st[st.Length - 1]);
                    for (int k = 1; k < this.size; k++)
                    {
                        s = fs.ReadLine();
                        st = s.Split(' ', '|', '\t');//в аргументах указывается массив символов, которым разделяются числа
                        st = st.Where(n => n.Length > 0).ToArray();
                        for (int i = 0; i < this.size; i++) this.A[k, i] = Convert.ToDouble(st[i]);
                        this.b[k] = Convert.ToDouble(st[st.Length - 1]);
                    }
                }

            }
            finally { fs.Close(); }
        }

        /// <summary>
        /// Создать нулевую систему заданной размерности
        /// </summary>
        /// <param name="k"></param>
        public SLAU(int k) { Make(k); }//создать систему такой размерности
                                       /// <summary>
                                       /// Задать систему по её расширенной матрице
                                       /// </summary>
                                       /// <param name="M"></param>
        public SLAU(Matrix M)
        {
            SLAU g = new SLAU(M.n);
            for (int i = 0; i < M.n; i++)
            {
                g.b[i] = M[i, M.m - 1];
                for (int j = 0; j < M.m - 1; j++) g.A[i, j] = M[i, j];
            }

            size = M.n;
            A = new double[size, size];
            b = new double[size];
            x = new double[size];

            this.A = g.A;
            this.b = g.b;
            this.x = g.x;
        }
        /// <summary>
        /// Создать систему, заполненную скалярными произведениями
        /// </summary>
        /// <param name="p">Функция из некоторой системы</param>
        /// <param name="f">Действительная функция (которую требуется аппроксимирвать)</param>
        /// <param name="k">Количество используемых функций системы</param>
        /// <param name="a">Начало отрезка интегрирования</param>
        /// <param name="b">Конец отрезка интегрирования</param>
        public SLAU(SequenceFunc p, RealFunc f, int k, double a, double b, SequenceFuncKind kind = SequenceFuncKind.Other)
        {
            Make(k);
            RealFunc[] fi = new RealFunc[k];
            for (int i = 0; i < k; i++)
            {
                fi[i] = new RealFunc((double x) => p(x, i));
                //Console.WriteLine(fi[i](3)+" "+p(3,i));
                this.b[i] = FuncMethods.RealFuncMethods.ScalarPower(f, fi[i], a, b);
            }
            switch (kind)
            {
                case SequenceFuncKind.Other:
                    for (int i = 0; i < k; i++)
                    {
                        fi[i] = new RealFunc((double x) => p(x, i));//массив функций дохуя неустойчив и выдаёт какую-то дичь! функции всегда надо определять заново!!!!!!
                        A[i, i] = FuncMethods.RealFuncMethods.ScalarPower(fi[i], fi[i], a, b);
                        for (int j = i + 1; j < k; j++)
                        {
                            fi[j] = new RealFunc((double x) => p(x, j));
                            A[i, j] = FuncMethods.RealFuncMethods.ScalarPower(fi[j], fi[i], a, b);
                            A[j, i] = A[i, j];
                        }
                    }
                    break;
                case SequenceFuncKind.Orthogonal:
                    for (int i = 0; i < k; i++)
                    {
                        fi[i] = new RealFunc((double x) => p(x, i));
                        A[i, i] = FuncMethods.RealFuncMethods.NormScalar(fi[i], a, b);
                    }

                    break;
                default:
                    for (int i = 0; i < k; i++)
                    {
                        A[i, i] = 1.0 / (b - a);
                        //this.b[i] *= (b - a);
                    }
                    break;
            }
            //this.Show();
        }
        /// <summary>
        /// Создать систему, заполненную скалярными произведениями по сетке
        /// </summary>
        /// <param name="p">Функция из некоторой системы</param>
        /// <param name="f">Сеточная функция (которую требуется аппроксимирвать)</param>
        public SLAU(SequenceFunc p, FuncMethods.NetFunc f, int k = 0)
        {
            if (k == 0) k = f.CountKnots;
            Make(k);
            RealFunc[] fi = new RealFunc[k];
            //for (int i = 0; i < k; i++) fi[i] = new RealFunc((double x) => p(x, i));
            for (int i = 0; i < k; i++)
            {
                fi[i] = new RealFunc((double x) => p(x, i));
                //Console.WriteLine(fi[i](3)+" "+p(3,i));
                this.b[i] = FuncMethods.NetFunc.ScalarP(f, fi[i]);
                A[i, i] = FuncMethods.NetFunc.ScalarP(fi[i], fi[i], f.Arguments);
                for (int j = i + 1; j < k; j++)
                {
                    fi[j] = new RealFunc((double x) => p(x, j));
                    A[i, j] = FuncMethods.NetFunc.ScalarP(fi[j], fi[i], f.Arguments);
                    A[j, i] = A[i, j];
                }
            }
        }
        /// <summary>
        /// Создать систему, заполненную скалярными произведениями
        /// </summary>
        /// <param name="p">Полином из некоторой системы</param>
        /// <param name="f">Действительная функция (которую требуется аппроксимирвать)</param>
        /// <param name="k">Количество используемых функций системы</param>
        /// <param name="a">Начало отрезка интегрирования</param>
        /// <param name="b">Конец отрезка интегрирования</param>
        public SLAU(SequencePol p, RealFunc f, int k, double a, double b, SequenceFuncKind kind = SequenceFuncKind.Other)
        {
            Make(k);
            RealFunc[] fi = new RealFunc[k];
            for (int i = 0; i < k; i++)
            {
                fi[i] = p(i).Value;
                this.b[i] = FuncMethods.RealFuncMethods.ScalarPower(f, fi[i], a, b);
            }
            switch (kind)
            {
                case SequenceFuncKind.Other:
                    for (int i = 0; i < k; i++)
                    {
                        fi[i] = p(i).Value;
                        A[i, i] = FuncMethods.RealFuncMethods.ScalarPower(fi[i], fi[i], a, b);
                        for (int j = i + 1; j < k; j++)
                        {
                            fi[j] = p(j).Value;
                            A[i, j] = FuncMethods.RealFuncMethods.ScalarPower(fi[j], fi[i], a, b);
                            A[j, i] = A[i, j];
                        }
                    }
                    break;
                case SequenceFuncKind.Orthogonal:
                    for (int i = 0; i < k; i++)
                    {
                        fi[i] = p(i).Value;
                        A[i, i] = FuncMethods.RealFuncMethods.ScalarPower(fi[i], fi[i], a, b);
                    }
                    break;
                default:
                    for (int i = 0; i < k; i++)
                        A[i, i] = 1;
                    break;
            }
        }

        /// <summary>
        /// Создать систему как подсистему исходной системы с заданной разменостью 
        /// </summary>
        /// <param name="M"></param>
        /// <param name="t">Размерность подсистемы</param>
        public SLAU(SLAU M, int t)
        {
            this.size = t;
            A = new double[size, size];
            b = new double[size];
            x = new double[size];

            for (int i = 0; i < size; i++)
            {
                b[i] = M.b[i];
                x[i] = M.x[i];
                for (int j = 0; j < size; j++) A[i, j] = M.A[i, j];
            }
        }

        public SLAU(SLAU M) : this(M, M.Size) { }
        /// <summary>
        /// Задание СЛАУ по матрице и свободному вектору
        /// </summary>
        /// <param name="M"></param>
        /// <param name="b"></param>
        public SLAU(SqMatrix M, Vectors b)
        {
            Make(b.n);
            for (int i = 0; i < b.n; i++)
            {
                for (int j = 0; j < b.n; j++)
                    this.A[i, j] = M[i, j];
                this.b[i] = b[i];
            }
        }

        /// <summary>
        /// Создание двумерного и одномерных динамических массивов с заданной размерностью
        /// </summary>
        /// <param name="k"></param>
        public virtual void Make(int k)
        {
            size = k;
            A = new double[size, size];
            //for (int i = 0; i < size; i++)
            //{
            //    A[i] = new double[size];
            //}
            b = new double[size];
            x = new double[size];
            UltraCount = 0;
            ultraval = -1;
            ErrorsMas = new double[k];
            ErrorMasP = new double[k];
            //for (int i = 0; i < size; i++)
            //{
            //    x[i] = 0;
            //}
        }
        /// <summary>
        /// Решение методом прогонки системы с трёхдиагональной матрицей
        /// </summary>
        public void ProRace()
        {
            SqMatrix t = new SqMatrix(this.A);
            //if (!t.IsTreeDiag()) throw new Exception("Матрица не тридиагональная!");
            int n = this.size;
            double[] a = new double[n + 1];
            double[] b = new double[n + 1];
            double[] c = new double[n + 1];
            double[] alp = new double[n + 1];
            double[] bet = new double[n + 1];

            //Заполнение массивов диагоналей
            b[1] = this.A[1 - 1, 1 - 1];
            c[1] = this.A[1 - 1, 2 - 1];
            for (int i = 2; i < n; i++)
            {
                b[i] = this.A[i - 1, i - 1];
                c[i] = this.A[i - 1, i + 1 - 1];
                a[i] = this.A[i - 1, i - 1 - 1];
            }
            b[n] = this.A[n - 1, n - 1];
            a[n] = this.A[n - 1, n - 1 - 1];

            //Vectors t = new Vectors(c);t.Show();

            //прямой ход 
            alp[2] = -c[1] / b[1];
            bet[2] = this.b[1 - 1] / b[1];
            for (int i = 2; i < n; i++)
            {
                double val = b[i] + a[i] * alp[i];
                alp[i + 1] = -c[i] / val;
                bet[i + 1] = (-a[i] * bet[i] + this.b[i - 1]) / val;
            }

            //обратный ход 
            this.x[n - 1] = (-a[n] * bet[n] + this.b[n - 1]) / (b[n] + a[n] * alp[n]);
            for (int i = n - 1; i >= 1; i--)
            {
                this.x[i - 1] = alp[i + 1] * this.x[i + 1 - 1] + bet[i + 1];
            }
        }

        /// <summary>
        /// Метод Гаусса (частичный)
        /// </summary>
        /// <param name="t">Количество строк, с которыми происходит преобразование</param>
        /// <param name="getdiv">Надо ли делить на среднее по модулю в строке</param>
        public void Gauss(int t, bool getdiv = false)
        {
            if (A[0, 0] < 1e-4) getdiv = true;

            //создание вспомогательной матрицы системы
            double[,] matrix = new double[size, size + 1];
            //for (int i = 0; i < size; i++)
            //{
            //    matrix[i] = new double[size + 1];
            //}
            //присваивание её элементам нужных значений
            for (int i = 0; i < size; i++)
            {
                matrix[i, size] = b[i];
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = A[i, j];
                }
            }

            double m; //промежуточный множитель

            Vectors res = new Vectors(size);
            if (getdiv)
            {
                Vectors v = new Vectors(size);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                        v[j] = Math.Abs(matrix[i, j]);
                    res[i] = v.ArithmeticAv;
                    for (int j = 0; j < size + 1; j++)
                        matrix[i, j] /= res[i];
                }

            }

            //прямой ход (без вывода матрицы, потому что работает)
            for (int j = 0; j < t; j++)
            {

                for (int i = j + 1; i < t; i++)
                {
                    m = matrix[i, j] / matrix[j, j];

                    for (int _h = j; _h < t; _h++)
                    {
                        matrix[i, _h] -= m * matrix[j, _h];
                    }
                    matrix[i, size] -= matrix[j, size] * m;
                }
            }

            //обратный ход		
            for (int j = t - 1; j >= 0; j--)
            {
                z2:
                for (int i = j - 1; i >= 0; i--)
                {
                    if (matrix[j, j] == 0)
                    {
                        j--;
                        goto z2;
                    }
                    m = matrix[i, j] / matrix[j, j];

                    matrix[i, size] -= matrix[j, size] * m;
                    matrix[i, j] -= m * matrix[j, j];
                }
            }

            //заполнение решения
            for (int i = 0; i < t; i++)
            {
                x[i] = matrix[i, size] / matrix[i, i];
            }
            if (getdiv)
                for (int i = 0; i < t; i++)
                    x[i] *= res[i];

            NEVA = Nev(A, x, b, t); //невязка фиксируется
        }
        /// <summary>
        /// Стандартный метод Гаусса
        /// </summary>
        public void Gauss() { Gauss(this.size); }
        /// <summary>
        /// Метод Гаусса, годный и при нулевых коэффициентах в системе
        /// </summary>
        public void GaussSelection()
        {
            Matrix S = new Matrix(this.size, this.size + 1);
            for (int j = 0; j < this.size; j++)
            {
                for (int i = 0; i < this.size; i++) S[i, j] = this.A[i, j];
                S[j, this.size] = this.b[j];
            }

            //S.LinesDiff(2, 1, 2); S.PrintMatrix();

            for (int j = 0; j < this.size; j++)
            {
                int k = j;
                if (S[k, j] == 0)//если ведущий элемент равен нулю, поменять эту строку местами с ненулевой
                {
                    int h = k;
                    while (S[h, j] == 0) h++;
                    S.LinesSwap(k, h);
                }

                while (S[k, j] == 0 && k < this.size - 1) k++;//найти ненулевой элемент
                int l = k + 1;
                if (k != this.size - 1) while (l != this.size) { S.LinesDiff(l, k, S[l, j] / S[k, j]); l++; } //отнимать от строк снизу
                                                                                                              //S.PrintMatrix();Console.WriteLine();
                l = k - 1;
                if (k != 0) while (l != -1) { S.LinesDiff(l, k, S[l, j] / S[k, j]); l--; }//отнимать от строк сверху
            }

            for (int i = 0; i < this.size; i++) this.x[i] = S[i, this.size] / S[i, i];
        }
        public void GaussSelection(int t)
        {
            SLAU s = new SLAU(t);
            for (int i = 0; i < t; i++)
            {
                s.b[i] = this.b[i];
                s.A[i, i] = this.A[i, i];
                for (int j = i + 1; j < t; j++)
                {
                    s.A[i, j] = this.A[i, j];
                    s.A[j, i] = this.A[i, j];
                }
            }
            s.GaussSelection();
            for (int i = 0; i < t; i++)
                this.x[i] = s.x[i];
        }
        /// <summary>
        /// Решение уравнения Ах=b методом Холецкого, присвоение вектору х значений решения
        /// </summary>
        /// <param name="z"></param>
        public void Holets(int z)
        {
            SqMatrix M = new SqMatrix(this.A);
            if (!M.IsPositCertain() || !M.IsSymmetric()) throw new Exception("Матрица не симметрическая или не положительно определённая!"); "".Show();

            //создание вспомогательной матрицы
            SqMatrix t = new SqMatrix(z);

            //прямой ход метода
            t[0, 0] = Math.Sqrt(A[0, 0]);
            if (z == 1)
            {
                x[0] = b[0] / A[0, 0];
                return;
            }

            for (int j = 1; j < z; j++)
            {
                t[0, j] = A[0, j] / t[0, 0];
            }


            for (int i = 1; i < z; i++)
            {
                double sum = 0;
                for (int k = 0; k < i; k++)
                {
                    sum += t[k, i] * t[k, i];
                }
                t[i, i] = Math.Sqrt(/*Math.Abs*/(A[i, i] - sum)); //без модуля не получается

                for (int j = i + 1; j < z; j++)
                {
                    sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += t[k, i] * t[k, j];
                    }
                    t[i, j] = (A[i, j] - sum) / t[i, i];
                }
            }

            //t.PrintMatrix();

            //обратный ход метода     
            double[] y = new double[z];
            y[0] = b[0] / t[0, 0];
            for (int i = 1; i < z; i++)
            {
                double sum = 0;
                for (int k = 0; k <= i - 1; k++)
                {
                    sum += t[k, i] * y[k];
                }
                y[i] = (b[i] - sum) / t[i, i];
            }

            x[z - 1] = y[z - 1] / t[z - 1, z - 1];
            for (int i = z - 2; i >= 0; i--)
            {
                double sum = 0;
                for (int k = i + 1; k < z; k++)
                {
                    sum += t[i, k] * x[k];
                }
                x[i] = (y[i] - sum) / t[i, i];
            }

            NEVA = Nev(A, x, b, z);
        }
        /// <summary>
        /// Решение уравнения Ах=b методом Якоби (простой итерации), присвоение вектору х значений решения
        /// </summary>
        /// <param name="t"></param>
        public void Jak(int t, double eps = 0.000001, int maxit = 0)
        {
            //создать диагональное преобладание
            SqMatrix M = new SqMatrix(this.A);
            SqMatrix A = new SqMatrix(M * M.Transpose());
            Matrix bb = new Matrix(this.b);
            Vectors b = new Vectors(M.Transpose() * bb);
            //for (int i = 0; i < t; i++)
            //{
            //    x[i] = 0; //первое приближение - свободный вектор
            //}
            double E;
            double EPSJ = eps;
            double NE = Nev(this.A, this.x, this.b, t);
            int num = 0; //переменные, связанные с количеством итераций
            int maxI = maxit;
            if (maxI == 0) maxI = t * t;
            while ((Nev(this.A, this.x, this.b, t) > EPSJ) && (num <= maxI))
            {
                NE = Nev(this.A, this.x, this.b, t);
                //cout<<NE<<endl;
                for (int i = 0; i < t; i++)
                {
                    E = 0;
                    for (int j = 0; j < t; j++)
                    {
                        z1:
                        if (j == i)
                        {
                            j++;
                            if (j != t) goto z1;
                        }
                        else
                        {
                            E += A[i, j] * x[j];
                        }
                    }
                    x[i] = (b[i] - E) / A[i, i];
                }
                num++;
            }

            NEVA = Nev(this.A, x, this.b, t);
        }

        public static readonly double EPSS = 0;
        /// <summary>
        /// Метод наискорейшего спуска со свободным вектором в качестве первого приближения
        /// </summary>
        /// <param name="t"></param>
        public void Speedy(int t, double eps = 0.000001, int maxit = 2000)
        {
            for (int i = 0; i < t; i++)
            {
                x[i] = b[i]; //первое приближение - свободный вектор
            }
            double E = Nev(A, x, b, t);
            double EPSJ = eps;
            int num = 0; //переменные, связанные с количеством итераций
            int maxI = maxit;
            double[] Ax = new double[t];
            double[] r = new double[t];
            double[] Ar = new double[t];
            double[] s = new double[t];
            double[] sum = new double[t];
            while ((Nev(A, x, b, t) > EPSJ) && (num <= maxI)) //пока 1) невязка большая, 2) шагов ещё не много, 3) невязка убывает
            {
                Func_in_matrix.Matrix_power(ref Ax, A, x, t); //произведение матрицы на вектор Ax=A*x
                Func_in_matrix.Vector_difference(ref r, Ax, b, t); //разность двух векторов r=Ax-b
                Func_in_matrix.Matrix_power(ref Ar, A, r, t); //Ar=A*r
                double tau = Func_in_matrix.Scalar_power(r, r, t) / Func_in_matrix.Scalar_power(Ar, r, t); //скалярное произведение двух векторов tau=(r,r)/(Ar,r)
                Func_in_matrix.Vector_on_scalar(ref s, -tau, r, t); //умножение вектора на скаляр s=-tau*r
                Func_in_matrix.Vector_sum(ref sum, s, x, t); //сумма векторов sum=x+s=x-tau*r...

                E = Nev(A, x, b, t); //фиксируем невязку
                Func_in_matrix.Vector_assingment(ref x, sum, t); //присваивание одному вектору другого
                num++;
            }
            NEVA = Nev(A, x, b, t);
        }
        /// <summary>
        /// Метод наискорейшего спуска без начального присвоения (используется вектор х, изначально нулевой либо изменёный в другом методе)
        /// </summary>
        /// <param name="t"></param>
        private void SpeedyNext(int t)
        {
            double E = Nev(A, x, b, t);
            double EPSJ = EPSS;
            int num = 0; //переменные, связанные с количеством итераций
            int maxI = 1000;
            double[] Ax = new double[t];
            double[] r = new double[t];
            double[] Ar = new double[t];
            double[] s = new double[t];
            double[] sum = new double[t];

            SqMatrix a = new SqMatrix(this.A);
            //if(a.CubeNorm<1)
            while ((Nev(A, x, b, t) > EPSJ) && (num <= maxI) && (Nev(A, x, b, t) <= E)) //пока 1) невязка большая, 2) шагов ещё не много, 3) невязка убывает
            {
                //$"{Nev(A, x, b, t)} > {EPSJ} && {num} <= {maxI} && {Nev(A, x, b, t)}<={E} ".Show();
                E = Nev(A, x, b, t);
                Func_in_matrix.Matrix_power(ref Ax, A, x, t); //произведение матрицы на вектор Ax=A*x
                Func_in_matrix.Vector_difference(ref r, Ax, b, t); //разность двух векторов r=Ax-b
                Func_in_matrix.Matrix_power(ref Ar, A, r, t); //Ar=A*r
                double tau = Func_in_matrix.Scalar_power(r, r, t) / Func_in_matrix.Scalar_power(Ar, r, t); //скалярное произведение двух векторов tau=(r,r)/(Ar,r)
                Func_in_matrix.Vector_on_scalar(ref s, -tau, r, t); //умножение вектора на скаляр s=-tau*r
                Func_in_matrix.Vector_sum(ref sum, s, x, t); //сумма векторов sum=x+s=x-tau*r...
                                                             //E = Nev(A, x, b, t); //фиксируем невязку
                Func_in_matrix.Vector_assingment(ref x, sum, t); //присваивание одному вектору другого
                num++;
            }
        }

        /// <summary>
        /// Покоординатная минимизация коэффициентов
        /// </summary>
        /// <param name="t"></param>
        public void Minimize_coef(int t, int count = 50) //
        {
            Console.Write("Точность аппроксимации при числе функций ");
            Console.Write(t);
            Console.Write(" =");
            Console.WriteLine();
            Console.Write("до использования покоординатной минимизации:\t");
            Console.Write(Error(t) + $" ({this.Nev(t)}) - в скобках указана невязка системы");
            Console.WriteLine();
            double sum = 0;
            int r = count;
            for (int k = 1; k <= r; k++)
            {
                for (int i = t - 1; i >= 0; i--)
                {
                    for (int j = 0; j < t; j++)
                    {
                        if (j != i)
                        {
                            sum += x[j] * A[i, j];
                        }
                    }
                    x[i] = (b[i] - sum) / A[i, i];
                    sum = 0;
                }
                Console.Write("после использования покоординатной минимизации ");
                Console.Write(k);
                Console.Write(" раз:\t");
                Console.Write(Error(t) + $" ({this.Nev(t)})");
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Метод Гаусса с последующим улучшением решения методом наискорейшего спуска
        /// </summary>
        /// <param name="t"></param>
        public void GaussSpeedy(int t)
        {
            //Gauss(t);
            GaussSelection(t);
            Vectors v = new Vectors(this.x);

            if (A[0, 0] > 1)
            {
                double tmp = Nev(t);
                SpeedyNext(t);
                if (tmp < Nev(t))
                    for (int i = 0; i < t; i++)
                        x[i] = v[i];
            }

            try
            {
                SLAU tmpp = new SLAU(this, t);
                tmpp.Correction();

                for (int i = 0; i < t; i++)
                    this.x[i] = tmpp.x[i];
            }
            finally
            {
                NEVA = Nev(A, x, b, t);
            }
        }
        /// <summary>
        /// Метод Гаусса + метод наискорейшего спуска + метод поокрдинатной минимизации
        /// </summary>
        /// <param name="t"></param>
        public void GaussSpeedyMinimize(int t) //гибридный с использованием покоординатной минимизации
        {
            //Gauss(t);
            //SpeedyNext(t);
            GaussSpeedy(t);
            Minimize_coef(t);
            NEVA = Nev(A, x, b, t);
        }

        //public static double VALUE_FOR_ULTRA = 10;
        //public void UltraHybrid(int t) //гибридный с координатной минимизацией по последней координате
        //{
        //    double[] c = new double[t];
        //    for (int i = 0; i < t - 1; i++)
        //    {
        //        c[i] = x[i];
        //    }

        //    double sum = 0;
        //    GaussSpeedy(t);

        //    if ((VALUE_FOR_ULTRA < Error(t)) && (t >= 1)) //если погрешность выросла - исправить это
        //    {
        //        for (int i = 0; i < t - 1; i++)
        //        {
        //            x[i] = c[i];
        //        }
        //        x[t - 1] = 0;
        //        if (t != 0)
        //        {
        //            for (int j = 0; j < t - 1; j++)
        //            {
        //                sum += x[j] * A[t - 1,j];
        //            }
        //            x[t - 1] = (b[t - 1] - sum) / A[t - 1,t - 1];
        //            sum = 0;

        //            double tmp1 = Error(t);

        //            if (VALUE_FOR_ULTRA < tmp1) //погрешность опять выросла
        //            {
        //                for (int i = 0; i < t - 1; i++)
        //                {
        //                    x[i] = c[i];
        //                }
        //                x[t - 1] = 0;
        //            }
        //        }
        //    }
        //    VALUE_FOR_ULTRA = Error(t);
        //    NEVA = Nev(A, x, b, t);
        //}

        //перечисление методов
        /// <summary>
        /// Перечисление методов решения системы
        /// </summary>
        public enum Method
        {
            Gauss,
            Holets,
            Jak,
            Speedy,
            GaussSpeedy,
            GaussSpeedyMinimize,
            UltraHybrid
        }

        /// <summary>
        /// Вывести систему на консоль
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write("||");
                for (int j = 0; j < size - 1; j++) Console.Write("{0} \t", A[i, j]);
                Console.WriteLine("{0}\t|| \t||{1}|| \t{2}", A[i, size - 1], x[i], b[i]);
            }
        }
        /// <summary>
        /// Вывести систему c рациональными числами на консоль
        /// </summary>
        public void ShowRational()
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write("||");
                for (int j = 0; j < size - 1; j++) Console.Write("{0} \t", Number.Rational.ToRational(A[i, j]));
                Console.WriteLine("{0}\t|| \t||{1}|| \t{2}", Number.Rational.ToRational(A[i, size - 1]), Number.Rational.ToRational(x[i]), Number.Rational.ToRational(b[i]));
            }

        }

        /// <summary>
        /// Вывести систему, её решение и невязки от разных методов
        /// </summary>
        public void ShowErrors()
        {
            Console.WriteLine("Система, решённая универсальным методом Гаусса:");
            this.GaussSelection();
            this.Show();
            Console.WriteLine("");
            Console.WriteLine("----------Невязки при использовании разных методов:");
            Console.WriteLine("Погрешность универсального метода Гаусса = {0}", this.Discrep);
            this.Gauss();
            Console.WriteLine("Погрешность экономного метода Гаусса = {0}", this.Discrep);

            SqMatrix M = new SqMatrix(this.A);
            if (M.IsPositCertain() && M.IsSymmetric())
            {
                this.Holets(this.size);
                Console.WriteLine("Погрешность метода Холетского = {0}", this.Discrep);
            }
            if (M.IsTreeDiag())
            {
                this.ProRace();
                Console.WriteLine("Погрешность метода прогонки = {0}", this.Discrep);
            }

            SqMatrix D = new SqMatrix(this.size), T = new SqMatrix(this.A);
            for (int i = 0; i < this.size; i++) D[i, i] = this.A[i, i];
            if (D.Det != 0)
            {


                SqMatrix B = SqMatrix.I(this.size) - D.Reverse * (T);

                if (B.Frobenius < 1)
                {
                    this.Jak(this.size);
                    Console.WriteLine("Погрешность метода Якоби = {0}", this.Discrep);
                }
            }

            this.Speedy(this.size);
            Console.WriteLine("Погрешность метода наискорейшего спуска = {0}", this.Discrep);
            this.GaussSpeedy(this.size);
            Console.WriteLine("Погрешность метода Гаусса с улучшением наискорейшим спуском (гибридный метод) = {0}", this.Discrep);
            this.Minimize_coef(this.size);
            if (M.IsPositCertain() && M.IsSymmetric())
            {
                Console.WriteLine("Погрешность метода покоординатной минимизации (10-минимизация) = {0}", this.Discrep);
                this.GaussSpeedyMinimize(this.size);
                Console.WriteLine("Погрешность метода Гаусса с улучшением наискорейшим спуском и покоординатной минимизацией (гипергибридный метод) = {0}", this.Discrep);
            }
        }


        public double[] ErrorsMas, ErrorMasP;
        public RealFunc f = null;
        public SequenceFunc p = null;
        public double begin = 0, end = 0;
        //public double Error(int k) //частичная погрешность
        //{
        //    double p = FuncMethods.RealFuncMethods.NormScalar(f, begin, end);
        //    double sum = 0;

        //    double[] Ax = new double[this.Size];
        //    Func_in_matrix.Matrix_power(ref Ax, A, x, k);
        //    for (int i = 0; i < k; i++)
        //    {
        //        sum += x[i] * Ax[i];
        //    }
        //    double EPS = Math.Abs(p - sum);
        //    return Math.Sqrt(EPS);
        //}
        public double Error(int k) //частичная погрешность
        {
            RealFunc po = (double xx) =>
              {
                  double sum = 0;
                  for (int i = 0; i < k; i++)

                      sum += x[i] * p(xx, i);
                  return sum;
              };
            return FuncMethods.RealFuncMethods.NormDistance(f, po, begin, end);
        }

        private double ultraval = -1;
        public double VALUE_FOR_ULTRA { get { if (ultraval == -1) ultraval = FuncMethods.RealFuncMethods.NormScalar(f, begin, end); return ultraval; } set { ultraval = value; } }
        public void Make(int k, double[,] AMAS)
        {
            Make(k);
            for (int i = 0; i < this.Size; i++)
                for (int j = 0; j < this.Size; j++)
                    this.A[i, j] = AMAS[i, j];
        }

        /// <summary>
        /// Число, которое показывает, какая часть системы уже была решена ультра-гибридом
        /// </summary>
        public int UltraCount = 0;
        public void UltraHybrid(int t, SequenceFuncKind kind = SequenceFuncKind.Other)
        {
            //UltraCount.Show();
            if (UltraCount == 0)//если вообще не решалось
            {
                //"Вошло".Show();
                x[0] = b[0] / A[0, 0];  //x[0].Show();
                                        //if (kind == SequenceFuncKind.Orthonormal) x[0] *= end - begin;
                VALUE_FOR_ULTRA = Error(1);
                ErrorsMas[0] = VALUE_FOR_ULTRA;
                RealFunc ff = (double xx) =>
                {
                    double sum = this.x[0] * p(xx, 0);
                    double s = sum - f(xx);
                    return s * s;
                };
                ErrorMasP[0] = FuncMethods.RealFuncMethods.NormScalar(ff, begin, end);

                UltraCount++;
                for (int i = UltraCount + 1; i <= t; i++)
                    UltraHybridLast(i, kind);
                UltraCount = t;
            }
            else if (UltraCount == t - 1)//если надо решить только по последней координате
            {
                UltraHybridLast(t, kind);
                UltraCount++;
            }
            else
            {
                for (int i = UltraCount + 1; i <= t; i++)
                    UltraHybridLast(i, kind);
                UltraCount = t;
            }
        }
        /// <summary>
        /// Ультра-гибридный метод суперского решения по последней координате
        /// </summary>
        /// <param name="t"></param>
        public void UltraHybridLast(int t, SequenceFuncKind kind = SequenceFuncKind.Other) //гибридный с координатной минимизацией по последней координате
        {
            double[] c = new double[t];
            for (int i = 0; i < t - 1; i++)
                c[i] = x[i];
            Vectors mk1 = new Vectors(c), mk2 = new Vectors(c);

            double sum = 0;
            switch (kind)
            {
                case SequenceFuncKind.Other:
                    GaussSpeedy(t);
                    break;
                //case SequenceFuncKind.Orthonormal:
                //    for (int i = 0; i < t; i++)
                //        x[i] = b[i]*(end-begin);
                //    break;
                default:
                    for (int i = 0; i < t; i++)
                        x[i] = b[i] / A[i, i];
                    break;
            }


            double tmp = Error(t);
            if (VALUE_FOR_ULTRA < tmp) //если погрешность выросла - исправить это, потому что новое решение не годится
            {
                $"{VALUE_FOR_ULTRA} < {tmp} при t = {t} (до покоординатной минимизации результата СПИДГАУССА)".Show();
                //покоординатная минимизация результата СПИДГАУССА
                for (int k = 0; k <= t - 1; k++)
                {
                    for (int j = 0; j < k; j++)
                        sum += x[j] * A[k, j];
                    for (int j = k + 1; j < t - 1; j++)
                        sum += x[j] * A[k, j];
                    x[k] = (b[k] - sum) / A[k, k];
                    sum = 0;
                }

                tmp = Error(t);
                if (VALUE_FOR_ULTRA < tmp)
                {
                    $"{VALUE_FOR_ULTRA} < {tmp} при t = {t} (до полно покоординатной минимизации вектора с1 с2 ... 0)".Show();
                    for (int i = 0; i < t; i++)//исправили, теперь пробуем новый метод                 
                        x[i] = c[i];

                    //покоординатная минимизация
                    for (int k = 0; k <= t - 1; k++)
                    {
                        for (int j = 0; j < k; j++)
                            sum += x[j] * A[k, j];
                        for (int j = k + 1; j < t - 1; j++)
                            sum += x[j] * A[k, j];
                        x[k] = (b[k] - sum) / A[k, k];
                        sum = 0;
                    }


                    double tmp1 = Error(t);
                    if (VALUE_FOR_ULTRA < tmp1) //погрешность опять выросла - тогда просто оставляем 0 на конце
                    {
                        $"{VALUE_FOR_ULTRA} < {tmp1} при t = {t} (до полно покоординатной минимизации на конце)".Show();
                        for (int i = 0; i < t; i++)//исправили, теперь пробуем новый метод                 
                            x[i] = c[i];
                        for (int j = 0; j < t - 1; j++)
                        {
                            sum += x[j] * A[t - 1, j];
                        }
                        x[t - 1] = (b[t - 1] - sum) / A[t - 1, t - 1];
                        sum = 0;
                        tmp = Error(t);
                        if (VALUE_FOR_ULTRA < tmp)
                        {
                            for (int i = 0; i < t - 1; i++)
                            {
                                x[i] = c[i];
                            }
                            x[t - 1] = 0;
                        }
                        else
                        {
                            $"Погрешность уменьшена МИНИМАКОЙ НА КОНЦЕ на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} %".Show();
                            VALUE_FOR_ULTRA = tmp;
                            Vectors v = new Vectors(this.x);
                            $"\t Новая аппроксимация {VALUE_FOR_ULTRA}; текущий вектор решения: {v.ToString()}".Show();
                        }

                    }
                    else
                    {
                        $"Погрешность уменьшена ПОЛНОЙ МИНИМАКОЙ на {(VALUE_FOR_ULTRA - tmp1) / VALUE_FOR_ULTRA * 100} %".Show();
                        VALUE_FOR_ULTRA = tmp1;
                        Vectors v = new Vectors(this.x);
                        $"\t Новая аппроксимация {VALUE_FOR_ULTRA}; текущий вектор решения: {v.ToString()}".Show();
                    }
                }
                else
                {
                    $"Погрешность уменьшена МИНИМАКОЙ СПИДГАУССА на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} %".Show();
                    VALUE_FOR_ULTRA = tmp;
                    Vectors v = new Vectors(this.x);
                    $"\t Новая аппроксимация {VALUE_FOR_ULTRA}; текущий вектор решения: {v.ToString()}".Show();
                }
            }
            else
            {
                $"Погрешность уменьшена СПИДГАУССОМ на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} %".Show();
                VALUE_FOR_ULTRA = tmp;
                Vectors v = new Vectors(this.x);
                $"\t Новая аппроксимация {VALUE_FOR_ULTRA}; текущий вектор решения: {v.ToString()}".Show();
            }

            ErrorsMas[t - 1] = VALUE_FOR_ULTRA;
            RealFunc ff = (double x) =>
            {
                sum = 0;

                for (int ii = 1; ii <= t; ii++)
                {
                    sum += this.x[ii - 1] * p(x, ii - 1);
                }
                double s = sum - f(x);
                return s * s;
            };
            ErrorMasP[t - 1] = FuncMethods.RealFuncMethods.NormScalar(ff, begin, end);
            //UltraCount = t;
            NEVA = Nev(A, x, b, t); "".Show();
        }

        /// <summary>
        /// Уточнение решения СЛАУ по Уилкинсону
        /// </summary>
        /// <param name="eps">Норма невязки, до которой нужно уточнять</param>
        /// <param name="maxcount">Максимальное число итераций</param>
        public void Correction(double eps = 0.0001, int maxcount = 10000)
        {
            SqMatrix A = new SqMatrix(this.A);
            SqMatrix AR = SqMatrix.ReverseCorrect(A, A.Reverse);
            Vectors b = new Vectors(this.b);

            double nev = Nevaska;
            int s = 0;
            while (Nevaska > eps && s <= maxcount && Nevaska < nev)
            {
                nev = Nevaska;
                Vectors xs = new Vectors(this.x);
                Vectors d = AR * (b - A * xs);
                xs += d;
                this.x = Vectors.ToDoubleMas(xs);
            }
        }
    }

    /// <summary>
    /// Класс кодирования-декодирования
    /// </summary>
    public static class Coding
    {
        static Coding()
        {
            TempList = new List<string>();
            HofTable = new Turn();
            DoubleCharTable = new List<Tuple<double, char>>();
        }

        /// <summary>
        /// Набор промежуточной информации
        /// </summary>
        public static List<string> TempList = new List<string>();

        /// <summary>
        /// Таблица сопоставления символам двоичных чисел
        /// </summary>
        public static Turn HofTable;

        /// <summary>
        /// Набор возможных символов
        /// </summary>
        public static string abc = "1234567890-+qwertyuiopasdfghjkl; zxcvbnm,.йцукенгшщзхъфывапролджэячсмитьбюЦУКЕНГШЩЗФВАПРОЛДЯЧСМИТБЮХЖЭQWERTYUIOPASDFGHJKLZXCVBNM";
        //public static string abc = "1234567890-+=qwertyuiopasdfghjkl; zxcvbnm,.ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬБЮХЪЖЭQWERTYUIOPASDFGHJKLZXCVBNM";

        /// <summary>
        /// Рандомизированный алфавит
        /// </summary>
        public static string randabc;
        /// <summary>
        /// Таблица для шифра Полибия
        /// </summary>
        public static char[,] TablePolib;
        /// <summary>
        /// Таблица для шифра Плейфера
        /// </summary>
        public static char[,] TablePlay;
        /// <summary>
        /// Таблица для маршрутной перестановки
        /// </summary>
        public static char[,] TableVert;
        /// <summary>
        /// Массив принятых частот символов
        /// </summary>
        private static CharFreq[] FreqMas = new CharFreq[34];

        /// <summary>
        /// Попытка взлома по частоте символов
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Hacking(string s)
        {
            //массив принятых частот (отсортированный в порядке убывания)
            FreqMas[1] = new CharFreq('о', 0.10983);
            FreqMas[2] = new CharFreq('е', 0.08483);
            FreqMas[3] = new CharFreq('а', 0.07998);
            FreqMas[4] = new CharFreq('и', 0.07367);
            FreqMas[5] = new CharFreq('н', 0.067);
            FreqMas[6] = new CharFreq('т', 0.06318);
            FreqMas[7] = new CharFreq('с', 0.05473);
            FreqMas[8] = new CharFreq('р', 0.04746);
            FreqMas[9] = new CharFreq('в', 0.04533);
            FreqMas[10] = new CharFreq('л', 0.04343);
            FreqMas[11] = new CharFreq('к', 0.03486);
            FreqMas[12] = new CharFreq('м', 0.03203);
            FreqMas[13] = new CharFreq('д', 0.02977);
            FreqMas[14] = new CharFreq('п', 0.02804);
            FreqMas[15] = new CharFreq('у', 0.02615);
            FreqMas[16] = new CharFreq('я', 0.02001);
            FreqMas[17] = new CharFreq('ы', 0.01898);
            FreqMas[18] = new CharFreq('ь', 0.01735);
            FreqMas[19] = new CharFreq('г', 0.01687);
            FreqMas[20] = new CharFreq('з', 0.01641);
            FreqMas[21] = new CharFreq('б', 0.01592);
            FreqMas[22] = new CharFreq('ч', 0.0145);
            FreqMas[23] = new CharFreq('й', 0.01208);
            FreqMas[24] = new CharFreq('х', 0.00966);
            FreqMas[25] = new CharFreq('ж', 0.0094);
            FreqMas[26] = new CharFreq('ш', 0.00718);
            FreqMas[27] = new CharFreq('ю', 0.00639);
            FreqMas[28] = new CharFreq('ц', 0.00486);
            FreqMas[29] = new CharFreq('щ', 0.00361);
            FreqMas[30] = new CharFreq('э', 0.00331);
            FreqMas[31] = new CharFreq('ф', 0.00267);
            FreqMas[32] = new CharFreq('ъ', 0.00037);
            FreqMas[33] = new CharFreq('ё', 0.00013);


            s = new string(s.Where(n => n != ' ' && n != ',' && n != '.' && n != ';' && n != ':' && n != '(' && n != ')' && n != '!' && n != '?' && n != '-' && n != '—' && n != '…' && n != '\n' && n != '\t').ToArray()).ToLower();//убрать из строки все пробелы, точки, запятые, все буквы заменить на маленькие
            TempList.Add($"Исправленная исходная строка: {s}");

            var smas = CharFreq.GetMas(s);
            //smas=smas.Reverse().ToArray();//реверсировать массив (чтоб было по убыванию)

            TempList.Add("----------------Сортированный массив частот в тексте:");
            for (int i = 0; i < smas.Length; i++)
                TempList.Add($"\tsmas[{i}] = {smas[i].simbol} \t{smas[i].frequency}");

            string tmp = "";
            for (int i = 0; i < smas.Length; i++)//перевести отсортированный массив в строку
                tmp += smas[i].simbol;
            tmp = new string(tmp.Reverse().ToArray());//tmp.Show();
            TempList.Add("----------------Строка символов текста по убыванию частоты: " + tmp);

            TempList.Add($"----------------Перевод символов строки:");
            string res = "";
            for (int i = 0; i < s.Length; i++)//перевести каждый символ в строке, зная частоту
                try
                {
                    int k = tmp.IndexOf(s[i]);
                    res += FreqMas[k + 1].simbol;
                    TempList.Add($"\ts[{i}] = {s[i]} \t(частота {smas[smas.Length - 1 - k].frequency}) \t------> \t{FreqMas[k + 1].simbol} (принятая частота {FreqMas[k + 1].frequency})");
                }
                catch { throw new Exception("Походу неизвестный символ"); }

            return res;
        }

        private class CharFreq : IComparable
        {
            public char simbol;
            public double frequency = 0;
            public CharFreq(char c) { simbol = c; }
            public CharFreq(char c, double v) : this(c) { frequency = v; }

            public int CompareTo(object obj)
            {
                CharFreq t = (CharFreq)obj;
                return frequency.CompareTo(t.frequency);
            }

            public static CharFreq[] GetMas(string s)
            {
                var mas = s.Distinct().ToArray();//взять массив разных символов
                CharFreq[] smas = new CharFreq[mas.Length];
                for (int i = 0; i < mas.Length; i++)//перевести массив символов в массив объектов класса
                {
                    smas[i] = new CharFreq(mas[i]);
                    double c = 0;
                    for (int j = 0; j < s.Length; j++)
                        if (s[j] == mas[i])
                            c++;
                    smas[i].frequency = c / s.Length;
                }
                Array.Sort(smas);//сортировать по частоте
                return smas;
            }
        }

        /// <summary>
        /// Шифр цезаря
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Zesar(string s, bool encode = true, int key = 4)
        {
            TempList.Add($"--------------Исходный алфавит: {abc}");
            TempList.Add($"--------------Ключ: {key}");

            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                int k = abc.IndexOf(s[i]);
                if (k >= 0)
                {
                    k += (encode) ? key : -key;
                    if (k > abc.Length) k -= abc.Length;
                    if (k < 0) k += abc.Length;
                    res += abc[k];
                }
            }
            return res;
        }

        private static void GenerateRandabc()
        {
            randabc = new string(abc.ToArray());
            for (int i = 0; i < abc.Length; i++)
            {
                Random r = new Random();
                int a = (r.Next(abc.Length) + i) % abc.Length;
                //r = new Random();
                int b = r.Next(i) % abc.Length;
                // a.Show();b.Show();"".Show();
                randabc = randabc.Swap(randabc[a], randabc[b]);
            }
        }
        /// <summary>
        /// Простая замена
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Simple(string s, bool encode = true, string key = "")
        {
            //если ключ нулевой и нужна кодировка, сгенерировать случайный ключ
            if (key.Length == 0)
                if (encode)
                {
                    GenerateRandabc();
                    key = randabc;
                    //key.Show();
                }
                else//если нужна декодировка, просто присвоить ключу сгенерированный алфавит
                {
                    key = randabc;
                    //key.Show();
                }
            //abc.Show();
            TempList.Add($"--------------Ключ: {key}");

            string res = "";
            if (encode)
                for (int i = 0; i < s.Length; i++)
                {
                    int k = abc.IndexOf(s[i]);
                    if (k >= 0) res += key[k];
                }
            else
                for (int i = 0; i < s.Length; i++)
                {
                    int k = key.IndexOf(s[i]);
                    if (k >= 0) res += abc[k];
                }

            return res;
        }

        private static void GenerateTable(string key)
        {
            TablePolib = new char[19, 10];//таблица -9 -8 -7 ... -1 0 1 ... 9 на 0 ... 9
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 10; j++)
                    if (i * 10 + j < abc.Length)
                        TablePolib[i, j] = key[i * 10 + j];

        }
        private static Point IndexOf(char[,] table, char k)
        {
            for (int i = 0; i < table.GetLength(0); i++)
                for (int j = 0; j < table.GetLength(1); j++)
                    if (table[i, j] == k)
                        return new Point(i, j);
            return new Point(0, 0);
        }
        /// <summary>
        /// Шифр Полибия
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Polib(string s, bool encode = true, string key = "")
        {
            //если ключ нулевой и нужна кодировка, сгенерировать случайный ключ
            if (key.Length == 0)
                if (encode)
                {
                    GenerateRandabc();
                    key = randabc;
                    //key.Show();
                }
                else//если нужна декодировка, просто присвоить ключу сгенерированный алфавит
                {
                    key = randabc;
                    //key.Show();
                }
            TempList.Add($"--------------Ключ: {key}");
            GenerateTable(key);//Сгенерировать таблицу по ключу

            TempList.Add($"--------------Таблица шифра Полибия:");
            for (int i = 0; i < 19; i++)
            {
                string tmp = "";
                for (int j = 0; j < 10; j++)
                    tmp += TablePolib[i, j].ToString();
                TempList.Add(tmp);
            }

            string res = "";
            if (encode)//кодировка
                for (int i = 0; i < s.Length; i++)
                {
                    Point p = IndexOf(TablePolib, s[i]);
                    res += (p.x - 9).ToString() + p.y.ToString() + ' ';
                }
            else//декодировка
            {
                string[] mas = s.Split(' ');
                mas = mas.Where(n => n.Length > 0).ToArray();
                //for (int i = 0; i < mas.Length; i++) Console.Write(mas[i] + "   ");
                for (int i = 0; i < mas.Length; i++)
                    if (mas[i].Length == 2)
                    {
                        int a = Convert.ToInt32(mas[i][0].ToString()), b = Convert.ToInt32(mas[i][1].ToString());
                        res += TablePolib[a + 9, b];
                        //res.Show();
                    }
                    else//=3
                    {
                        int a = Convert.ToInt32(mas[i][1].ToString()), b = Convert.ToInt32(mas[i][2].ToString());
                        res += TablePolib[-a + 9, b];
                        //res.Show();
                    }
            }
            return res;
        }

        private static void GenerateTablePlay(string key)
        {
            TablePlay = new char[(int)Math.Ceiling((decimal)abc.Length / key.Length), key.Length];
            int l = key.Length;
            key += new string(abc.Where(n => key.IndexOf(n) < 0).ToArray());//key.Show();
            for (int i = 0; i < (int)Math.Ceiling((decimal)abc.Length / l); i++)
                for (int j = 0; j < l; j++)
                    if (i * l + j < key.Length) TablePlay[i, j] = key[i * l + j];
                    else TablePlay[i, j] = '`';

        }
        /// <summary>
        /// Шифр Плейфера
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Playfer(string s, bool encode = true, string key = "abcd")
        {
            key = new string(key.Distinct().ToArray());//убрать из ключа повторы
            if (key.Length == 0) key = "abcd";//страховка при нулeвом ключе
                                              //key.Show();
            TempList.Add($"--------------Ключ: {key}");
            GenerateTablePlay(key);//сгенерировать таблицу по ключу
            TempList.Add("--------------Таблица шифра Плейфера");
            for (int i = 0; i < TablePlay.GetLength(0); i++)
            {
                string tmp = "";
                for (int j = 0; j < TablePlay.GetLength(1); j++)
                    tmp += TablePlay[i, j].ToString();
                TempList.Add(tmp);
            }

            string res = "";
            if (encode)
            {
                for (int i = 0; i < s.Length - 1; i++)
                    if (s[i] == s[i + 1] && i % 2 == 0)
                        s = s.Insert(i + 1, "+");//вставить особый символ между повторами символов в строке
                if (s.Length % 2 != 0) s += "+";//если не хватает пар, добавить символ в конец
                                                //s.Show();
                TempList.Add($"--------------Исправленные исходный текст: {s}");

                for (int i = 0; i < s.Length; i += 2)
                {
                    Point a = IndexOf(TablePlay, s[i]);
                    Point b = IndexOf(TablePlay, s[i + 1]);
                    if (a.x == b.x)
                    {
                        try { res += TablePlay[(int)a.x, (int)a.y + 1]; }
                        catch { res += TablePlay[(int)a.x, 0]; }
                        try { res += TablePlay[(int)b.x, (int)b.y + 1]; }
                        catch { res += TablePlay[(int)b.x, 0]; }
                    }
                    else if (a.y == b.y)
                    {
                        try { res += TablePlay[(int)a.x + 1, (int)a.y]; }
                        catch { res += TablePlay[0, (int)a.y]; }
                        try { res += TablePlay[(int)b.x + 1, (int)b.y]; }
                        catch { res += TablePlay[0, (int)b.y]; }
                    }
                    else
                    {
                        res += TablePlay[(int)a.x, (int)b.y];
                        res += TablePlay[(int)b.x, (int)a.y];
                    }
                }
            }

            else
            {
                for (int i = 0; i < s.Length; i += 2)
                {
                    Point a = IndexOf(TablePlay, s[i]);
                    Point b = IndexOf(TablePlay, s[i + 1]);
                    if (a.x == b.x)
                    {
                        try { res += TablePlay[(int)a.x, (int)a.y - 1]; }
                        catch { res += TablePlay[(int)a.x, TablePlay.GetLength(1) - 1]; }
                        try { res += TablePlay[(int)b.x, (int)b.y - 1]; }
                        catch { res += TablePlay[(int)b.x, TablePlay.GetLength(1) - 1]; }
                    }
                    else if (a.y == b.y)
                    {
                        try { res += TablePlay[(int)a.x - 1, (int)a.y]; }
                        catch { res += TablePlay[TablePlay.GetLength(0) - 1, (int)a.y]; }
                        try { res += TablePlay[(int)b.x - 1, (int)b.y]; }
                        catch { res += TablePlay[TablePlay.GetLength(0) - 1, (int)b.y]; }
                    }
                    else
                    {
                        res += TablePlay[(int)a.x, (int)b.y];
                        res += TablePlay[(int)b.x, (int)a.y];
                    }
                }
                res = new string(res.ToCharArray().Where(n => n != '+').ToArray());
            }

            return res;
        }

        private static string UseKey(string key, bool end = false)
        {
            //перевести строку в массив чисел
            int[] mas = new int[key.Length];
            for (int i = 0; i < mas.Length; i++)
                mas[i] = Convert.ToInt32(key[i].ToString()) - 1;

            //заполнить новую таблицу по ключу
            char[,] table = new char[TableVert.GetLength(0), TableVert.GetLength(1)];
            for (int j = 0; j < mas.Length; j++)
                for (int i = 0; i < TableVert.GetLength(0); i++)
                    table[i, mas[j]] = TableVert[i, j];

            //перевести таблицу в фразу
            string res = "";
            for (int i = 0; i < TableVert.GetLength(0); i++)
                for (int j = 0; j < TableVert.GetLength(1); j++)
                    res += table[i, j];

            if (end) return new string(res.Where(n => n != '`').ToArray());
            return res;
        }
        private static string ModKey(string key)
        {
            char[] res = new char[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                int tmp = Convert.ToInt32(key[i].ToString()) - 1;
                res[tmp] = (i + 1).ToString()[0];
            }
            //res.Show();
            return new string(res);
        }
        /// <summary>
        /// Маршрутная (вертикальная) перестановка
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Vert(string s, bool encode = true, string key = "34125")
        {
            //if (key.Length == 0) key = new string(randabc.ToCharArray());
            TableVert = new char[(int)Math.Ceiling((decimal)s.Length / key.Length), key.Length];
            for (int i = 0; i < (int)Math.Ceiling((decimal)s.Length / key.Length); i++)
                for (int j = 0; j < key.Length; j++)
                    if (i * key.Length + j < s.Length) TableVert[i, j] = s[i * key.Length + j];
                    else TableVert[i, j] = '`';//сгенерировать таблицу по фразе

            TempList.Add($"--------------Таблица маршрутной перестановки");
            for (int i = 0; i < TableVert.GetLength(0); i++)
            {
                string tmp = "";
                for (int j = 0; j < TableVert.GetLength(1); j++)
                    tmp += TableVert[i, j];
                TempList.Add(tmp);
            }

            if (encode)
                return UseKey(key, false);//кодировать в другую фразу

            string k = ModKey(key);//иначе изменить ключ и декодировать
            return UseKey(k, true);
        }


        public class Turn : IComparable
        {
            public List<Tuple<char, string>> SymbolList;
            public double value;
            public Turn()
            {
                this.SymbolList = null;
                this.value = 0;
            }
            public Turn(char c, string s, double v)
            {
                this.SymbolList = new List<Tuple<char, string>>();
                SymbolList.Add(new Tuple<char, string>(c, s));
                this.value = v;
            }
            public int CompareTo(object obj)
            {
                Turn t = (Turn)obj;
                return t.value.CompareTo(this.value);

            }
            public int Count => SymbolList.Count;

            private void AddInString(int a, int b)
            {
                for (int i = 0; i < a; i++)
                    this.SymbolList[i] = new Tuple<char, string>(this.SymbolList[i].Item1, String.Concat("0", this.SymbolList[i].Item2));
                for (int i = a; i < a + b; i++)
                    this.SymbolList[i] = new Tuple<char, string>(this.SymbolList[i].Item1, String.Concat("1", this.SymbolList[i].Item2));
            }
            public static Turn operator +(Turn t1, Turn t2)
            {
                List<Tuple<char, string>> list = new List<Tuple<char, string>>();
                list.AddRange(t1.SymbolList);
                list.AddRange(t2.SymbolList);
                Turn res = new Turn();
                res.SymbolList = list;
                res.value = t1.value + t2.value;
                res.AddInString(t1.SymbolList.Count, t2.SymbolList.Count);
                return res;
            }
        }

        /// <summary>
        /// Таблица сопоставления действительным числам из кодировки символа Unicode
        /// </summary>
        public static List<Tuple<double, char>> DoubleCharTable = new List<Tuple<double, char>>();
        /// <summary>
        /// Перевод массива чисел в строку символов Unicode (поддерживает около 130 000 разных символов) c параллельным заполнением таблицы соответствия
        /// </summary>
        /// <param name="mas"></param>
        /// <returns></returns>
        public static string DoubleMasToUnicodeString(double[] mas)
        {
            var tmas = mas.Distinct().ToArray();//убрать из массива повторы
            Array.Sort(tmas);
            string res = "";
            //заполнить таблицу
            DoubleCharTable = new List<Tuple<double, char>>();

            int y = 0;
            for (int i = 0; i < tmas.Length; i++)
            {
                char tmp = Convert.ToChar(y++);
                while (/*tmp.Equals(null)||Char.IsSeparator(tmp)||Char.IsWhiteSpace(tmp)*/!Char.IsLetterOrDigit(tmp)) tmp = Convert.ToChar(y++);
                DoubleCharTable.Add(new Tuple<double, char>(tmas[i], tmp));
            }


            //записать результат
            for (int i = 0; i < mas.Length; i++)
            {
                int k = Array.IndexOf(tmas, mas[i]);
                res += DoubleCharTable[k].Item2;
            }

            return res;
        }
        /// <summary>
        /// Перевод строки в массив чисел с помощью таблицы соответствия
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double[] StringToDoubleMas(string s)
        {
            double[] res = new double[s.Length];
            //взять из таблицы массив символов
            char[] mas = new char[DoubleCharTable.Count];
            for (int i = 0; i < mas.Length; i++) mas[i] = DoubleCharTable[i].Item2;
            //записать результат
            for (int i = 0; i < res.Length; i++)
            {
                int k = Array.IndexOf(mas, s[i]);
                res[i] = DoubleCharTable[k].Item1;
            }
            return res;
        }
        ///// <summary>
        ///// Сортировщик очереди
        ///// </summary>
        ///// <param name="s"></param>
        ///// <param name="r"></param>
        ///// <returns></returns>
        //private static int comparer(this Tuple<List<char>, string, double> s, Tuple<List<char>, string, double> r)
        //{
        //    return s.Item3.CompareTo(r.Item3);
        //}
        /// <summary>
        /// Кодирование по алгоритму Хоффмана
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Hoffman(string s, bool encode = true)
        {
            string res = "";

            if (encode)
            {
                var constmas = CharFreq.GetMas(s);//набор символов с частотой
                constmas.Reverse();
                var mas = new List<Turn>();//список пар нескольких символов и их суммарной частоты
                TempList.Add("-----Исходный набор символов:");
                for (int i = 0; i < constmas.Length; i++)
                {
                    mas.Add(new Turn(constmas[i].simbol, "", constmas[i].frequency));
                    TempList.Add($"symbol[{i}] = {constmas[i].simbol} \t{constmas[i].frequency}");
                }

                //генерируется таблица
                while (mas.Count > 1)
                {
                    mas.Sort();
                    mas.Reverse();
                    for (int i = 0; i < mas.Count; i++)
                    {
                        string t = "";
                        for (int j = 0; j < mas[i].SymbolList.Count; j++)
                            t += $"{mas[i].SymbolList[j].Item1}({mas[i].SymbolList[j].Item2})";
                        TempList.Add($"{t} \t{mas[i].value}");
                    }

                    mas[0] += mas[1];
                    mas.RemoveAt(1);
                    TempList.Add("");
                    //вдруг ускорит поиск
                    mas.Sort();
                    mas.Reverse();
                }

                HofTable = mas[0];

                //генерируется вывод
                string tmp = "";
                for (int i = 0; i < HofTable.Count; i++)
                    tmp += HofTable.SymbolList[i].Item1;
                for (int i = 0; i < s.Length; i++)
                    res += HofTable.SymbolList[tmp.IndexOf(s[i])].Item2 + " ";
                //res += HofTable.SymbolList[Array.BinarySearch(tmp.ToArray(),s[i])].Item2 + " ";
            }
            else
            {
                //генерируется вывод
                var tmp = s.Split(' ').Where(n => n.Length > 0).ToArray();
                TempList.Add($"Декодируемая строка без пробелов:");
                for (int i = 0; i < tmp.Length; i++)
                {
                    TempList.Add($"tmp[{i}] = {tmp[i]}");
                    for (int j = 0; j < HofTable.Count; j++)
                        if (tmp[i] == HofTable.SymbolList[j].Item2)
                        {
                            res += HofTable.SymbolList[j].Item1;
                            break;
                        }
                }

            }
            return res;
        }
        /// <summary>
        /// Кодировани набора чисел в набор двоичных чисел в два этапа (массив чисел -> строка символов -> строка двоичных чисел)
        /// </summary>
        /// <param name="mas"></param>
        /// <returns></returns>
        public static string HoffmanNumberEncode(double[] mas) => Hoffman(DoubleMasToUnicodeString(mas), true);
        /// <summary>
        /// Перевод набора двоичных чисел в массив строк в два этапа
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double[] HoffmanNunderDecode(string s) => StringToDoubleMas(Hoffman(s, false));

        static char[] characters => abc.ToArray();
        //= new char[] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
        //                                            'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
        //                                            'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
        //                                            'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
        //                                            '8', '9', '0' };

        private static int N => characters.Length; //длина алфавита
                                                   /// <summary>
                                                   /// Кодирование шифром Виженера
                                                   /// </summary>
                                                   /// <param name="input"></param>
                                                   /// <param name="keyword"></param>
                                                   /// <returns></returns>
        public static string VigenereEncode(string input, string keyword)
        {
            //input = input.ToUpper();
            //keyword = keyword.ToUpper();

            string result = "";

            int keyword_index = 0, i = 1;
            TempList.Add($"Ключ = {keyword}");
            foreach (char symbol in input)
            {
                int c = (Array.IndexOf(characters, symbol) +
                    Array.IndexOf(characters, keyword[keyword_index])) % N;

                result += characters[c];

                TempList.Add($"(p[{i}] + k[{i}]) mod {N} = ({Array.IndexOf(characters, symbol)} + {Array.IndexOf(characters, keyword[keyword_index])}) % {N} = {characters[c]}");

                keyword_index++; i++;

                if ((keyword_index + 1) == keyword.Length)
                    keyword_index = 0;
            }

            return result;
        }
        /// <summary>
        /// Декодирование шифром Виженера
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string VigenereDecode(string input, string keyword)
        {
            //input = input.ToUpper();
            //keyword = keyword.ToUpper();

            string result = "";

            int keyword_index = 0, i = 1;
            TempList.Add($"Ключ = {keyword}");
            foreach (char symbol in input)
            {
                int p = (Array.IndexOf(characters, symbol) + N -
                    Array.IndexOf(characters, keyword[keyword_index])) % N;

                result += characters[p];

                TempList.Add($"(p[{i}] + {N} - k[{i}]) mod {N} = ({Array.IndexOf(characters, symbol)} + {N} - {Array.IndexOf(characters, keyword[keyword_index])}) % {N} = {characters[p]}");

                keyword_index++; i++;

                if ((keyword_index + 1) == keyword.Length)
                    keyword_index = 0;
            }

            return result;
        }
        /// <summary>
        /// Генерация случайного шифра по длине и спец. числу
        /// </summary>
        /// <param name="lenght"></param>
        /// <param name="startSeed"></param>
        /// <returns></returns>
        public static string Generate_Pseudorandom_KeyWord(int lenght, int startSeed)
        {
            Random rand = new Random(startSeed);

            string result = "";

            for (int i = 0; i < lenght; i++)
                result += characters[rand.Next(0, characters.Length)];

            return result;
        }

        static List<Tuple<char, BitArray>> AlphaBit = null;//массив битовых кодировок алфавита
        static int bitlenth => (int)Math.Ceiling(Math.Log(N, 2));//нужное число бит
        static void GetAlphaBit()//заполнение алфавита
        {
            AlphaBit = new List<Tuple<char, BitArray>>();
            for (int i = 0; i < abc.Length; i++)
            {
                BitArray arr = new BitArray(bitlenth, false);//пустой массив
                string code = Convert.ToString(i, 2);//двоичный код числа
                string nol = new string('0', bitlenth - code.Length);
                code = String.Concat(nol, code);
                for (int j = 0; j < bitlenth; j++)
                    if (code[code.Length - 1 - j] == '1')//перевод строки кода в массив битов
                        arr[bitlenth - 1 - j] = true;
                AlphaBit.Add(new Tuple<char, BitArray>(abc[i], arr));
            }
        }
        static BitArray[] bitkey;//ключ
        static void GetBitArr(int length)//генерация ключа
        {
            Random r = new Random();
            bitkey = new BitArray[length];
            for (int i = 0; i < length; i++)
            {
                bool[] mas = new bool[bitlenth];
                for (int j = 0; j < mas.Length; j++)
                    mas[j] = (r.Next() % 2 == 1) ? true : false;
                bitkey[i] = new BitArray(mas);
            }
        }
        static BitArray[] WordToBitMas(string s)
        {
            BitArray[] res = new BitArray[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                int ind = abc.IndexOf(s[i]);
                res[i] = new BitArray(AlphaBit[ind].Item2);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            return res;
        }
        static string BitToString(BitArray t) => BitMasToSting(new BitArray[] { t });
        static string BitMasToWord(BitArray[] arr)
        {
            string res = "";
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < AlphaBit.Count; j++)
                    if (BitToString(arr[i]) == BitToString(AlphaBit[j].Item2))
                    {
                        res += AlphaBit[j].Item1;
                        //Console.WriteLine(AlphaBit[j].Item1.ToString() + " = " + BitMasToSting(new BitArray[] { AlphaBit[j].Item2 })+" = "+ BitMasToSting(new BitArray[] { arr[i] }));
                        //res.Show();
                        break;
                    }
            return res;
        }
        public static string BitMasToSting(BitArray[] arr)//перевод массива бит в строку из 0 и 1
        {
            string res = "";
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                    if (arr[i][j])
                        res += "1";
                    else res += "0";
                res += " ";
            }
            return res;
        }
        public static BitArray[] StringToBitMas(string s)//перевод строки из 0 и 1 в массив бит
        {
            string[] st = s.Split(' ').Where(n => n.Length != 0).ToArray();
            BitArray[] res = new BitArray[st.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new BitArray(st[i].Length);
                for (int j = 0; j < st[i].Length; j++)
                    if (st[i][j] == '1')
                        res[i][j] = true;
                    else res[i][j] = false;
            }
            return res;
        }
        static BitArray[] XORBitMas(BitArray[] a, BitArray[] b)
        {
            BitArray[] c = new BitArray[a.Length];
            for (int i = 0; i < a.Length; i++)
                c[i] = a[i].Xor(b[i]);
            return c;
        }
        /// <summary>
        /// Шифр Вернама
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Vernam(string s, bool encode = true)
        {
            if (AlphaBit == null) GetAlphaBit();
            TempList.Add("Алфавит:");
            for (int i = 0; i < AlphaBit.Count; i++)
                TempList.Add('\t' + AlphaBit[i].Item1.ToString() + " = " + BitMasToSting(new BitArray[] { AlphaBit[i].Item2 }));

            if (encode)
            {
                TempList.Add($"Исходное слово:\t {s}");
                GetBitArr(s.Length); TempList.Add($"Ключ:\t {BitMasToSting(bitkey)}");
                BitArray[] wordToBit = WordToBitMas(s); TempList.Add($"Слово в битах:\t {BitMasToSting(wordToBit)}");
                BitArray[] newWordTiBit = XORBitMas(wordToBit, bitkey); TempList.Add($"Шифр в битах:\t {BitMasToSting(newWordTiBit)}"); //BitMasToSting(newWordTiBit).Show();
                return BitMasToWord(newWordTiBit);
            }
            else
            {
                TempList.Add($"Исходное слово:\t {s}");
                BitArray[] code = WordToBitMas(s); TempList.Add($"Слово (шифр) в битах:\t {BitMasToSting(code)}");
                BitArray[] newcode = XORBitMas(code, bitkey); TempList.Add($"(Искомое) слово в битах:\t {BitMasToSting(newcode)}");
                return BitMasToWord(newcode);
            }
        }
    }

    //-----------------------------------------чисто для курсача
    /// <summary>
    /// Класс базисных точек
    /// </summary>
    public class BasisPoint : Point
    {
        public BasisPoint() : base(0) { }
        public BasisPoint(double a) : base(a) { }
        public BasisPoint(double a, double b) : base(a, b) { }
        public BasisPoint(Point p) : base(p) { }

        /// <summary>
        /// Функция базисного потенциала, сцепленного с точкой z
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public double PotentialF(BasisPoint z)
        {
            return Math.Log(1.0 / Point.Eudistance(this, z));
        }
    }

    /// <summary>
    /// Класс плоских кривых 
    /// </summary>
    public class Curve
    {
        /// <summary>
        /// Параметризации координат в зависимости от параметра и радиуса
        /// </summary>
        public DRealFunc U = (double t, double r) => 0, V = (double t, double r) => 0;

        /// <summary>
        /// Поля, рассчитанные под параметризацию границы области (на случай, если границу области нужно задать немного иначе, потому что функция имеет особенности)
        /// </summary>
        private RealFunc uuu = null, vvv = null;

        /// <summary>
        /// Свойства, выдающие параметризацию границы области
        /// </summary>
        public RealFunc u { get { if (uuu == null) return (double t) => U(t, this.radius); return (double t) => uuu(t); } set { uuu = value; } }
        public RealFunc v { get { if (vvv == null) return (double t) => V(t, this.radius); return (double t) => vvv(t); } set { vvv = value; } }

        /// <summary>
        /// Площадь сегмента
        /// </summary>
        public TripleFunc S;
        /// <summary>
        /// Начальное значение параметра
        /// </summary>
        public double a;
        /// <summary>
        /// Конечное значение параметра
        /// </summary>
        public double b;

        /// <summary>
        /// Значение шага интегрирования для этой кривой
        /// </summary>
        protected double _h = FuncMethods.DefInteg.STEP;
        /// <summary>
        /// Число шагов при интегрировании
        /// </summary>
        protected int M = ITER_INTEG; //число шагов
                                      /// <summary>
                                      /// Базовый радиус кривой
                                      /// </summary>
        protected double radius = 3;
        /// <summary>
        /// Базовый радиус кривой
        /// </summary>
        public double BaseRadius
        {
            get { return radius; }
            set { radius = value; }
        }
        /// <summary>
        /// Функция, выдающая нужную длину отрезка параметризации в зависимости от радиуса, поскольку иногда отрезок изменения параметра t зависит от r
        /// </summary>
        public RealFunc End;

        /// <summary>
        /// Количество шагов при интегрировании по умолчанию
        /// </summary>
        public static int ITER_INTEG = 5000;

        [Obsolete]
        /// <summary>
        /// Задать число шагов
        /// </summary>
        /// <param name="MM"></param>
        protected void Get_h(int MM)
        {
            M = MM;
            _h = (b - a) / M; //присвоение шагу конкретного значения
        }

        /// <summary>
        /// Возврат точки на кривой по значению параметра
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public BasisPoint Transfer(double t)
        {
            if (!GetTRANSFER)
            {
                //"Begin".Show();
                BasisPoint point = new BasisPoint();
                point.x = this.u(t);
                point.y = this.v(t);//point.Show();//"End".Show();
                return point;
            }
            return (BasisPoint)Trans(t, radius);
        }

        private bool GetTRANSFER = false;
        private DPointFunc Trans;
        /// <summary>
        /// Свойство, выдающее и принимающее делегат, отображающий параметры в точку на кривой
        /// </summary>
        public DPointFunc TRANSFER
        {
            get
            {
                if (GetTRANSFER) return Trans;
                else return (double t, double r) => Transfer(t);
            }
            set
            {
                Trans = (double t, double r) => value(t, r);
                GetTRANSFER = true;
            }
        }


        /// <summary>
        /// Кривая по своей параметризации
        /// </summary>
        /// <param name="a0">Начальное значение параметра</param>
        /// <param name="b0">Конечное значение параметра</param>
        /// <param name="uu">Отображение в первую координату</param>
        /// <param name="vv">Отображение во вторую координату</param>
        public Curve(double a0, double b0, RealFunc uu, RealFunc vv)
        {
            a = a0;
            b = b0;
            u = uu;
            v = vv;
        }
        /// <summary>
        /// Кривая по своей параметризации и базовому радиусу
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="b0"></param>
        /// <param name="uu"></param>
        /// <param name="vv"></param>
        /// <param name="BASEradius"></param>
        public Curve(double a0, double b0, RealFunc uu, RealFunc vv, double BASEradius) : this(a0, b0, uu, vv)
        {
            radius = BASEradius;
        }
        /// <summary>
        /// Кривая по всем своим параметрам
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="b0"></param>
        /// <param name="uu"></param>
        /// <param name="vv"></param>
        /// <param name="BASEradius"></param>
        /// <param name="uuu"></param>
        /// <param name="vvv"></param>
        public Curve(double a0, double b0, RealFunc uu, RealFunc vv, double BASEradius, DRealFunc uuu, DRealFunc vvv, TripleFunc T, RealFunc end) : this(a0, b0, uu, vv, BASEradius)
        {
            this.U = uuu;
            this.V = vvv;
            this.S = T;
            this.End = end;
        }
        /// <summary>
        /// Кривая по своей параметризации и базовому радиусу
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="b0"></param>
        /// <param name="uu"></param>
        /// <param name="vv"></param>
        /// <param name="BaseRadius"></param>
        public Curve(double a0, double b0, DRealFunc uu, DRealFunc vv, double BaseRadius)
        {
            a = a0; b = b0;
            U = uu; V = vv;
            radius = BaseRadius;
        }
        /// <summary>
        /// Кривая, вырожденная в начало координат (все поля - нули)
        /// </summary>
        public Curve()
        {
            a = 0;
            b = 0;
        }
        /// <summary>
        /// Конструктор кривой, которая задаётся не через пары параметрических функций 
        /// </summary>
        /// <param name="a0">Начало отрезка параметризации</param>
        /// <param name="b0">Конец отрезка параметризации</param>
        /// <param name="Tr">Функция, выдающая точку на кривой в зависимости от параметра и радиуса кривой</param>
        /// <param name="BaseRad">Базовый радиус</param>
        /// <param name="T">Площадь сегмента</param>
        /// <param name="end">Возврат конца отрезка параметризации в зависимости от радиуса</param>
        public Curve(double a0, double b0, DPointFunc Tr, double BaseRad, TripleFunc T, RealFunc end)
        {
            a = a0; b = b0;
            S = T;
            Trans = Tr;
            radius = BaseRad;
            End = new RealFunc(end);
        }

        [Obsolete]
        /// <summary>
        /// Вычисление криволинейного интеграла первого рода по этой кривой от функции f методом трапеции 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public double FirstkindTr(Functional f)//(в программе не используется)
        {
            Get_h(ITER_INTEG);
            double value = 0;
            for (int k = 0; k <= M - 1; k++)
            {
                value += f(Transfer((a + (k + 1) * _h + a + (k) * _h) / 2)) * BasisPoint.Eudistance(Transfer(a + (k + 1) * _h), Transfer(a + (k) * _h));
            }
            return value;
        }

        /// <summary>
        /// Вычисление криволинейного интеграла первого рода по этой кривой от функции f методом Гаусса 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public double Firstkind(Functional f)
        {
            RealFunc h = (double t) => f(this.Transfer(t));
            return FuncMethods.DefInteg.GaussKronrod.Integral(h, this.a, this.b);
        }
    }

    /// <summary>
    /// Класс всяких полезных штук для работы с другими программами
    /// </summary>
    public class ИнтеграцияСДругимиПрограммами
    {
        private static string CharForExcel(char begchar, int count)
        {
            int tmp = (count - Convert.ToInt32('A') + Convert.ToInt32(begchar));//tmp.Show();
            int tmp2 = tmp / 26;
            string ress = "";
            while (tmp2 > 0)
            {
                char s = (tmp2 > 0) ? Convert.ToChar(Convert.ToInt32('A') + tmp2 % 26 - 1) : default(char);
                ress = s.ToString() + ress;
                tmp2 /= 26;
            }
            tmp %= 26;
            string res = (Convert.ToChar(Convert.ToInt32('A') + tmp)).ToString();
            //if (tmp2 > 0)
            res = ress + res;
            //res.Show();
            return res;
        }

        /// <summary>
        /// Создать в Excel таблицу, по которой можно построить 3D поверхность
        /// </summary>
        /// <param name="f">Функция двух переменных</param>
        /// <param name="x0">Начало отрезка по первому аргументу</param>
        /// <param name="X">Конец отрезка по первому аргументу</param>
        /// <param name="xcount">Число шагов по первому аргументу</param>
        /// <param name="y0">Начало отрезка по второму аргументу</param>
        /// <param name="Y">Конец отрезка по второму аргументу</param>
        /// <param name="ycount">Число шагов по второму аргументу</param>
        public static void CreatTableInExcel(DRealFunc f, double x0, double X, int xcount, double y0, double Y, int ycount)
        {
            Application application = new Application();
            application.Workbooks.Add(Type.Missing);
            Worksheet sheet = (Worksheet)application.Sheets[1];
            var t = Type.Missing;

            double hx = (X - x0) / (xcount - 1), hy = (Y - y0) / (ycount - 1);

            for (int i = 0; i < ycount; i++)
            {
                sheet.Range[CharForExcel('B', i) + "2", t].Value2 = y0 + i * hy;
            }
            for (int i = 0; i < xcount; i++)
            {
                sheet.Range["A" + (3 + i).ToString(), t].Value2 = x0 + i * hx;
            }

            //for (int j = 0; j < xcount; j++)
            Parallel.For(0, xcount, (int j) =>
            {
                for (int i = 0; i < ycount; i++)
                    sheet.Range[CharForExcel('B', i) + (3 + j).ToString(), t].Value2 = f(sheet.Range["A" + (3 + j).ToString(), t].Value2, sheet.Range[CharForExcel('B', i) + "2", t].Value2);
            });

            sheet.Range["A2", CharForExcel('B', ycount) + (3 + xcount).ToString()].Activate();

            Excel.Chart chart = sheet.ChartObjects().Add(10, 10, 500, 500).Chart;
            chart.ChartType = Excel.XlChartType.xl3DArea;
            chart.SetSourceData(sheet.Range["A2", CharForExcel('B', ycount) + (3 + xcount).ToString()]);

            application.Visible = true;
        }
        /// <summary>
        /// Создать в Excel таблицу, по которой можно построить серию графиков функциё одной переменной
        /// </summary>
        /// <param name="args">Массив аргументов</param>
        /// <param name="values">Массив векторов значений</param>
        public static void CreatTableInExcel<T>(T[] args, params Vectors[] values)
        {
            Application application = new Application();
            application.Workbooks.Add(Type.Missing);
            Worksheet sheet = (Worksheet)application.Sheets[1];
            var t = Type.Missing;

            for (int i = 0; i < args.Length; i++)
            {
                sheet.Range["A" + (3 + i).ToString(), t].Value2 = args[i].ToString();
            }

            for (int i = 0; i < values.Length; i++)
                for (int j = 0; j < values[i].n; j++)
                    sheet.Range[CharForExcel('B', i) + (3 + j).ToString(), t].Value2 = (values[i][j]);

            sheet.Range["A2", CharForExcel('B', values.Length) + (3 + values[0].n).ToString()].Activate();
            application.Visible = true;
        }
        /// <summary>
        /// Создать в Excel таблицу, по таблице double
        /// </summary>
        public static void CreatTableInExcel(double[][] mas)
        {
            Application application = new Application();
            application.Workbooks.Add(Type.Missing);
            Worksheet sheet = (Worksheet)application.Sheets[1];
            var t = Type.Missing;

            for (int j = 0; j < mas.GetLength(0); j++)
                for (int i = 0; i < mas.GetLength(1); i++)
                    sheet.Range[CharForExcel('B', i) + (3 + j).ToString(), t].Value2 = mas[j][i];

            sheet.Range["A2", CharForExcel('B', mas.GetLength(1)) + (3 + mas.GetLength(0)).ToString()].Activate();
            application.Visible = true;
        }
        /// <summary>
        /// Создать таблицу по словарю Точка-Комплексное число
        /// </summary>
        /// <typeparam name="Targ"></typeparam>
        /// <param name="dic"></param>
        public static async void CreatTableInExcel(ConcurrentDictionary<Point, Lazy<Complex>> dic, string name = "Name", Complex.ComplMode mode = Complex.ComplMode.Abs)
        {
            Func<Complex, double> f;
            switch (mode)
            {
                case Complex.ComplMode.Abs:
                    f = c => c.Abs;
                    break;
                case Complex.ComplMode.Re:
                    f = c => c.Re;
                    break;
                case Complex.ComplMode.Im:
                    f = c => c.Im;
                    break;
                default:
                    f = c => c.Arg;
                    break;
            }

            var d1 = dic.Keys.ToArray();
            var d2 = dic.Values.ToArray();

            Application application = new Application();
            application.Workbooks.Add(Type.Missing);
            Worksheet sheet = (Worksheet)application.Sheets[1];
            sheet.Name = name;
            var t = Type.Missing;
            //sheet.Range[sheet.Cells[1, 1], sheet.Cells[d1.Count()+3, d1.Count() + 3]].NumberFormat = "Экспоненциальный";
            application.Visible = true;

            var point = new Point[d1.Length];
            for (int i = 0; i < d1.Length; i++)
                point[i] = new Point(d1[i]);

            Array.Sort(point);
            List<double> x = new List<double>(0);
            List<double> y = new List<double>(0);
            await Task.Run(() =>
            {

                for (int i = 0; i < d1.Length; i++)
                {
                    int a = Array.IndexOf(d1, point[i]);
                    int ix = x.IndexOf(point[i].x), iy = y.IndexOf(point[i].y);
                    if (ix < 0)
                    {
                        ix = x.Count;
                        x.Add(point[i].x);
                    }
                    if (iy < 0)
                    {
                        iy = y.Count;
                        y.Add(point[i].y);
                    }
                    sheet.Range[CharForExcel('B', ix) + "2", t].Value2 = point[i].x;
                    sheet.Range["A" + (3 + iy).ToString(), t].Value2 = point[i].y;

                    string tmp = f(d2[a].Value).ToFloat().ToString();
                    sheet.Cells[3 + iy, 1 + ix] = /*f(*/d2[a].Value/*)*/;
                }
            }
            );

            //Область сортировки             
            Microsoft.Office.Interop.Excel.Range range = sheet.get_Range("A3", CharForExcel('B', x.Count) + (3 + y.Count).ToString());
            //По какому столбцу сортировать
            Microsoft.Office.Interop.Excel.Range rangeKey = sheet.get_Range("A3");
            //Добавляем параметры сортировки
            sheet.Sort.SortFields.Add(rangeKey);
            sheet.Sort.SetRange(range);
            sheet.Sort.Orientation = Microsoft.Office.Interop.Excel.XlSortOrientation.xlSortColumns;
            sheet.Sort.SortMethod = Microsoft.Office.Interop.Excel.XlSortMethod.xlPinYin;
            sheet.Sort.Apply();
            //Очищаем фильтр
            sheet.Sort.SortFields.Clear();

            sheet.Range["A2", CharForExcel('B', x.Count) + (3 + y.Count).ToString()].Activate();

        }
        /// <summary>
        /// Создать таблицу и графики по комплексной функции двух действительных переменных
        /// </summary>
        /// <param name="f">Функция</param>
        /// <param name="x0"></param>
        /// <param name="X"></param>
        /// <param name="xcount">Число шагов по оси Х</param>
        /// <param name="y0"></param>
        /// <param name="Y"></param>
        /// <param name="ycount">Число шагов по оси Y</param>
        /// <param name="dic">Дополнительный словарь значений</param>
        /// <param name="withgraph">Нужно ли вдобавок рисовать графики</param>
        public static async void CreatTableInExcel(DComplexFunc f, double x0, double X, int xcount, double y0, double Y, int ycount, ConcurrentDictionary<Point, Lazy<Complex>> dic = null, bool withgraph = true)
        {
            FixProcesBefore();

            //задание приложения
            Application application = new Application();
            application.SheetsInNewWorkbook = 4;
            application.Workbooks.Add(Type.Missing);
            Worksheet sheetRe = (Worksheet)application.Sheets[1], sheetIm = (Worksheet)application.Sheets[2], sheetAbs = (Worksheet)application.Sheets[3], sheetArg = (Worksheet)application.Sheets[4];
            var t = Type.Missing;

            sheetRe.Name = "Re";
            sheetIm.Name = "Im";
            sheetAbs.Name = "Abs";
            sheetArg.Name = "Arg";

            //переменные
            double hx = (X - x0) / (xcount - 1), hy = (Y - y0) / (ycount - 1);
            string vs;
            double vd;
            List<double> x = new List<double>(), y = new List<double>();


            await Task.Run(() =>
            {
                //строка и столбец аргументов
                for (int i = 0; i < ycount; i++)
                {
                    vs = CharForExcel('B', i) + "2";
                    vd = y0 + i * hy;
                    y.Add(vd);
                    sheetRe.Range[vs, t].Value2 = vd;
                    sheetIm.Range[vs, t].Value2 = vd;
                    sheetAbs.Range[vs, t].Value2 = vd;
                    sheetArg.Range[vs, t].Value2 = vd;
                }
                for (int i = 0; i < xcount; i++)
                {
                    vs = "A" + (3 + i).ToString();
                    vd = x0 + i * hx;
                    x.Add(vd);
                    sheetRe.Range[vs, t].Value2 = vd;
                    sheetIm.Range[vs, t].Value2 = vd;
                    sheetAbs.Range[vs, t].Value2 = vd;
                    sheetArg.Range[vs, t].Value2 = vd;
                }

                //заполнение значений в таблице
                //for (int j = 0; j < xcount; j++)
                Parallel.For(0, xcount, (int j) =>
                {
                    for (int i = 0; i < ycount; i++)
                    {
                        var vss = CharForExcel('B', i) + (3 + j).ToString();
                        Complex tmp = f(x[j], y[i]);
                        sheetRe.Range[vss, t].Value2 = tmp.Re;
                        sheetIm.Range[vss, t].Value2 = tmp.Im;
                        sheetAbs.Range[vss, t].Value2 = tmp.Abs;
                        sheetArg.Range[vss, t].Value2 = tmp.Arg;
                    }
                });

                //если есть дополнительные значения из словаря
                if (dic != null)
                {
                    ConcurrentDictionary<Point, Lazy<Complex>> dicc = new ConcurrentDictionary<Point, Lazy<Complex>>(dic);
                    //считать ключи и значения
                    var d1 = dicc.Keys.ToArray();
                    var d2 = dicc.Values.ToArray();
                    var point = new Point[d1.Length];
                    for (int i = 0; i < d1.Length; i++)
                        point[i] = new Point(d1[i]);
                    Array.Sort(point);
                    //записать точку в таблицу, если там её не было
                    for (int i = 0; i < d1.Length; i++)
                    {
                        int a = Array.IndexOf(d1, point[i]);
                        int ix = x.IndexOf(point[i].x), iy = y.IndexOf(point[i].y);
                        int ixt = ix, iyt = iy;
                        if (ix < 0)
                        {
                            ix = x.Count;
                            x.Add(point[i].x);
                        }
                        if (iy < 0)
                        {
                            iy = y.Count;
                            y.Add(point[i].y);
                        }
                        if (ixt * iyt <= 0)
                        {

                            for (int j = 1; j < 4; j++)
                            {
                                Worksheet sheet = (Worksheet)application.Sheets[j];
                                sheet.Range[CharForExcel('B', ix) + "2", t].Value2 = point[i].x;
                                sheet.Range["A" + (3 + iy).ToString(), t].Value2 = point[i].y;
                            }
                            Complex tmp = d2[a].Value;
                            sheetRe.Cells[3 + iy, 1 + ix] = tmp.Re;
                            sheetIm.Cells[3 + iy, 1 + ix] = tmp.Im;
                            sheetAbs.Cells[3 + iy, 1 + ix] = tmp.Abs;
                            sheetArg.Cells[3 + iy, 1 + ix] = tmp.Arg;
                        }
                    }

                    //сортировать
                    for (int i = 1; i <= 4; i++)
                    {
                        Worksheet sheet = (Worksheet)application.Sheets[i];
                        //Область сортировки             
                        Microsoft.Office.Interop.Excel.Range range = sheet.get_Range("A3", CharForExcel('B', x.Count) + (3 + y.Count).ToString());
                        //По какому столбцу сортировать
                        Microsoft.Office.Interop.Excel.Range rangeKey = sheet.get_Range("A3");
                        //Добавляем параметры сортировки
                        sheet.Sort.SortFields.Add(rangeKey);
                        sheet.Sort.SetRange(range);
                        sheet.Sort.Orientation = Microsoft.Office.Interop.Excel.XlSortOrientation.xlSortColumns;
                        sheet.Sort.SortMethod = Microsoft.Office.Interop.Excel.XlSortMethod.xlPinYin;
                        sheet.Sort.Apply();
                        //Очищаем фильтр
                        sheet.Sort.SortFields.Clear();

                        //Добавляем параметры сортировки
                        range = sheet.get_Range("B2", CharForExcel('B', x.Count) + (3 + y.Count).ToString());
                        rangeKey = sheet.get_Range("B2");
                        sheet.Sort.SortFields.Add(rangeKey);
                        sheet.Sort.SetRange(range);
                        sheet.Sort.Orientation = Microsoft.Office.Interop.Excel.XlSortOrientation.xlSortRows;
                        sheet.Sort.SortMethod = Microsoft.Office.Interop.Excel.XlSortMethod.xlPinYin;
                        sheet.Sort.Apply();
                        //Очищаем фильтр
                        sheet.Sort.SortFields.Clear();
                    }
                }

                //создать графики
                if (withgraph)
                {
                    vs = CharForExcel('B', ycount) + (3 + xcount).ToString();
                    for (int i = 1; i <= 4; i++)
                    {
                        Worksheet sheet = (Worksheet)application.Sheets[i];
                        //sheet.Range["A2", CharForExcel('B', ycount) + (3 + xcount).ToString()].Activate();
                        Excel.Chart chart = sheet.ChartObjects().Add(10, 10, 800, 800).Chart;
                        chart.ChartType = Excel.XlChartType.xl3DArea;
                        chart.SetSourceData(sheet.Range["A2", vs]);
                    }
                }
            });

            application.Visible = true;
            //application.ThisWorkbook.Save();
            //application.ThisWorkbook.Saved = true;
            //application.ThisWorkbook.SaveCopyAs($"Документ от {DateTime.Now}");
            //application.Quit();
            //application.ActiveWorkbook.Close(0);
            //application.Workbooks.Close();
            //application.Quit();
        }

        //private void ClearMemory(Application excelApp)
        //{
        //    Process excelProcess = Process.GetProcessesByName("EXCEL")[0];
        //    if (!excelProcess.CloseMainWindow())
        //    {
        //        excelProcess.Kill();
        //    }
        //    excelApp.DisplayAlerts = false;
        //    excelApp.ActiveWorkbook.Close(0);
        //    excelApp.Quit();
        //    Marshal.ReleaseComObject(excelApp);
        //}
        static Process[] processesBefore;
        static void FixProcesBefore() => processesBefore = Process.GetProcessesByName("excel");
        public static void Kill()
        {

            // Get Excel processes after opening the file.
            Process[] processesAfter = Process.GetProcessesByName("excel");

            // Now find the process id that was created, and store it.
            int processID = 0;
            foreach (Process process in processesAfter)
            {
                if (!processesBefore.Select(p => p.Id).Contains(process.Id))
                {
                    processID = process.Id;
                }
            }

            // And now kill the process.
            if (processID != 0)
            {
                Process process = Process.GetProcessById(processID);
                //process.Close();
                process.Kill();
            }
        }
    }
}

