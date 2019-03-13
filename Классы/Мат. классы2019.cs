using System;
using System.Diagnostics;
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
using МатКлассы2018;
using Excel = Microsoft.Office.Interop.Excel;
using static Computator.NET.Core.NumericalCalculations.FunctionRoot;

namespace МатКлассы2019
{
    /// <summary>
    /// Комплексная функция многих переменных
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public delegate Complex CnToCFunction(CVectors v);
    /// <summary>
    /// Матричная функция от векторного аргумента
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public delegate CSqMatrix CVecToCMatrix(CVectors v);
    /// <summary>
    /// Функция R->C
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public delegate Complex RToC(double x);

    public delegate double MethodR(Func<double, double> f, double beg, double end, double eps, uint N);

    /// <summary>
    /// Класс расширений для всяких методов
    /// </summary>
    public static class Expendator2
    {
        /// <summary>
        /// Преобразование метода ()=>void в пригодный для использования после оператора await
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Task ToTask(this Action t) => Task.Run(t);
        /// <summary>
        /// Преобразование метода ()=>T в пригодный для использования после оператора await
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks>Функцию типа f(x,y)=>T, x=fixX, y=fixY, нужно вызывать примерно так: await( ()=>f(fixX,fixY) ).ToTask()</remarks>
        public static Task ToTask<T>(this Func<T> t) => Task.Run(t);

        /// <summary>
        /// Кубический сплайн по сеточной функции с коэффициентами условий на границе
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="is0outcut">Должна ли функция равняться 0 вне отрезка задания</param>
        /// <returns></returns>
        public static RealFunc ToSpline(this NetFunc f, double a = 0, double b = 0, bool is0outcut = true) => Polynom.CubeSpline(f.Points, a, b, is0outcut);

        /// <summary>
        /// Сумма комплексного массива
        /// </summary>
        /// <param name="mas"></param>
        /// <returns></returns>
        public static Complex Sum(this Complex[] mas)
        {
            Complex sum = 0;
            for (int i = 0; i < mas.Length; i++)
                sum += mas[i];
            return sum;
        }
        /// <summary>
        /// Вывести пустую строку
        /// </summary>
        public static void EmptyLine() => "".Show();

        /// <summary>
        /// Примерный максимум модуля функции на отрезке
        /// </summary>
        /// <param name="f"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static double MaxApproxAbs(ComplexFunc f, double beg, double end, double step = 0.01)
        {
            double m(double c) => f(c).Abs;
            double max = m(beg), tmp;
            while (beg <= end)
            {
                beg += step;
                tmp = m(beg);
                if (tmp > max)
                    max = tmp;  //max.Show();           
            }
            return max;
        }

        /// <summary>
        /// Объединение двух массивов с удалением одного из двух близких (ближе eps) друг к другу элементов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static double[] Distinct(double[] a, double[] b, double eps = 1e-6)
        {
            double[] m = Expendator.Union(a, b);
            Array.Sort(m);
            List<double> l = new List<double>();
            l.Add(m[0]);
            int k = 0;
            for (int i = 1; i < m.Length; i++)
                if (m[i] - m[k] >= eps)
                {
                    l.Add(m[i]);
                    k = i;
                }
            return l.ToArray();
        }

        /// <summary>
        /// Деление комплексного массива на комплексное число
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="coef"></param>
        /// <returns></returns>
        public static Complex[] Div(this Complex[] mas, Complex coef)
        {
            Complex[] res = new Complex[mas.Length];
            for (int i = 0; i < mas.Length; i++)
                res[i] = mas[i] / coef;
            return res;
        }

        /// <summary>
        /// Записать массив векторов в файл
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mas"></param>
        public static void WriteInFile(string name="1", params Vectors[] mas)
        {
            StreamWriter f = new StreamWriter(name + ".txt");
            for(int i = 0; i < mas.Length; i++)
            {
                for (int j = 0; j < mas[i].Deg - 1; j++)
                    f.Write(mas[i][j] + " ");
                f.WriteLine(mas[i][mas[i].Deg - 1]);
            }
            f.Close();
        }

        /// <summary>
        /// Запустить процесс и выполнить какие-то действия по его окончанию
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="act"></param>
        public static void StartProcess(string fileName,Action act)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.EnableRaisingEvents = true;

            process.Exited += (sender, e) => act();
            process.Start();
        }
    }

    /// <summary>
    /// Класс функции, осуществляющей отображение Ck -> Cn
    /// </summary>
    public class CkToCnFunc
    {
        /// <summary>
        /// Делегат, отождествляемый с унитарным отображением
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public delegate CVectors VecToVec(CVectors v);

        private CnToCFunction[] FuncMas;
        private VecToVec func = null;

        /// <summary>
        /// Размерность области значений
        /// </summary>
        public int EDimention => FuncMas.Length;

        /// <summary>
        /// Значение функции от вектора через индексатор
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public CVectors this[CVectors v]
        {
            get
            {
                if (func == null)
                {
                    CVectors res = new CVectors(EDimention);
                    for (int i = 0; i < EDimention; i++)
                        res[i] = FuncMas[i](v);
                    return res;
                }
                else
                    return func(v);
            }
        }
        /// <summary>
        /// Функция отдельного измерения
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public CnToCFunction this[int i] => new CnToCFunction(FuncMas[i]);
        /// <summary>
        /// Метод, возвращающий значение функции от вектора
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public CVectors Value(CVectors v) => this[v];
        /// <summary>
        /// Значение функции от вектора
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public CVectors Value(params Complex[] c)
        {
            CVectors v = new CVectors(c);
            return this[v];
        }

        /// <summary>
        /// Задание функции как совокупности комплексных функций многих переменных
        /// </summary>
        /// <param name="mas"></param>
        public CkToCnFunc(params CnToCFunction[] mas)
        {
            FuncMas = new CnToCFunction[mas.Length];
            for (int i = 0; i < FuncMas.Length; i++)
                FuncMas[i] = new CnToCFunction(mas[i]);
        }
        /// <summary>
        /// Задать унитарную функции как произведение унитарной функции на комплексную матрицу
        /// </summary>
        /// <param name="M"></param>
        /// <param name="F"></param>
        public CkToCnFunc(CSqMatrix M, CkToCnFunc F)
        {
            this.FuncMas = new CnToCFunction[F.EDimention];
            for (int i = 0; i < this.EDimention; i++)
                this.FuncMas[i] = (CVectors v) => M.GetLine(i) * F.Value(v);
        }
        /// <summary>
        /// Задать унитарную функции как произведение унитарной функции на кматричную функцию
        /// </summary>
        /// <param name="M"></param>
        /// <param name="F"></param>
        public CkToCnFunc(CVecToCMatrix M, CkToCnFunc F)
        {
            this.FuncMas = null;
            func = (CVectors v) =>
              {
                  CSqMatrix Mat = M(v);
                  CVectors Vec = F.Value(v);
                  CVectors res = new CVectors(Vec.Degree);

                  for (int i = 0; i < this.EDimention; i++)
                      res[i] = new Complex(Mat.GetLine(i) * Vec);
                  return res;
              };

        }
        /// <summary>
        /// Задать функцию через делегат отображения
        /// </summary>
        /// <param name="f"></param>
        public CkToCnFunc(VecToVec f) { this.func = new VecToVec(f); }

        /// <summary>
        /// Тип каррирования
        /// </summary>
        public enum CarringType
        {
            /// <summary>
            /// По первым аргументам
            /// </summary>
            FirstArgs,
            /// <summary>
            /// По последним аргументам
            /// </summary>
            LastArgs
        }
        /// <summary>
        /// Каррирование отображения в соответствии с параметрами
        /// </summary>
        /// <param name="C">Параметр каррирования</param>
        /// <param name="c">Фиксированные аргументы</param>
        /// <returns></returns>
        public CkToCnFunc CarrByFirstOrLastArgs(CarringType C = CarringType.LastArgs, params Complex[] c)
        {
            if (func == null)
            {
                CnToCFunction[] mas = new CnToCFunction[EDimention];

                switch (C)
                {
                    case CarringType.FirstArgs:
                        for (int i = 0; i < mas.Length; i++)
                            mas[i] = (CVectors v) => this.FuncMas[i](new CVectors(Expendator.Union(c, v.ComplexMas)));
                        break;
                    default:
                        for (int i = 0; i < mas.Length; i++)
                            mas[i] = (CVectors v) => this.FuncMas[i](new CVectors(Expendator.Union(v.ComplexMas, c)));
                        break;
                }
                return new CkToCnFunc(mas);
            }
            else
            {
                VecToVec h;
                switch (C)
                {
                    case CarringType.FirstArgs:
                        h = (CVectors v) => func(new CVectors(Expendator.Union(c, v.ComplexMas)));
                        break;
                    default:
                        h = (CVectors v) => func(new CVectors(Expendator.Union(v.ComplexMas, c)));
                        break;
                }
                return new CkToCnFunc(h);
            }

        }

        /// <summary>
        /// Интеграл от отображения по одному аргументу (другие зафиксированы)
        /// </summary>
        /// <param name="beforeArg">Фиксированные аргументы до изменяемого</param>
        /// <param name="afterArg">Фиксированные аргументы после изменяемого</param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="tm"></param>
        /// <param name="tp"></param>
        /// <param name="eps"></param>
        /// <param name="pr"></param>
        /// <param name="gr"></param>
        /// <returns></returns>
        public CVectors IntegralAmoutOneArg(CVectors beforeArg, CVectors afterArg, double t1, double t2, double t3, double t4, double tm, double tp, double eps, double pr, double gr)
        {
            FuncMethods.DefInteg.GaussKronrod.ComplexVectorFunc tmp = (Complex x, int N) => this.Value(Expendator.Union(beforeArg.ComplexMas, new Complex[] { x }, afterArg.ComplexMas)).ComplexMas;
            return new CVectors(FuncMethods.DefInteg.GaussKronrod.DINN5_GK(tmp, t1, t2, t3, t4, tm, tp, eps, pr, gr, this.EDimention));
        }
    }
    /// <summary>
    /// Класс комплексных СЛАУ
    /// Отличие от действительного случая в том, что реализация происходит через матрицы и векторы, а не через массивы
    /// Из-за комплексных чисел все методы надо переписывать
    /// </summary>
    public class CSLAU
    {
        private CSqMatrix A;
        private CVectors x, b;

        /// <summary>
        /// Размерность системы
        /// </summary>
        public int Dim => x.Degree;

        /// <summary>
        /// Конструктор по матрице и свободному вектору
        /// </summary>
        /// <param name="M"></param>
        /// <param name="v"></param>
        public CSLAU(CSqMatrix M, CVectors v)
        {
            x = new CVectors(v.Degree);
            b = new CVectors(v);
            A = new CSqMatrix(M);
        }

        private void GetDet(out Complex det)
        {
            det = A.Det;
            if (det == 0) throw new Exception("Матрица системы вырождена!");
        }

        /// <summary>
        /// Решение системы методом Крамера
        /// </summary>
        public void KramerSolve()
        {
            GetDet(out Complex det);
            for (int i = 0; i < this.Dim; i++)
                x[i] = A.ColumnSwap(i + 1, b).Det / det;
        }
        /// <summary>
        /// Метод Гаусса, годный и при нулевых коэффициентах в системе
        /// </summary>
        public void GaussSelection()
        {
            CMatrix S = new CMatrix(this.Dim, this.Dim + 1);
            for (int j = 0; j < this.Dim; j++)
            {
                for (int i = 0; i < this.Dim; i++) S[i, j] = this.A[i, j];
                S[j, this.Dim] = this.b[j];
            }

            for (int j = 0; j < this.Dim; j++)
            {
                int k = j;
                if (S[k, j] == 0)//если ведущий элемент равен нулю, поменять эту строку местами с ненулевой
                {
                    int h = k;
                    while (S[h, j] == 0) h++;
                    S.LinesSwap(k, h);
                }

                while (S[k, j] == 0 && k < this.Dim - 1) k++;//найти ненулевой элемент
                int l = k + 1;
                if (k != this.Dim - 1) while (l != this.Dim) { S.LinesDiff(l, k, S[l, j] / S[k, j]); l++; } //отнимать от строк снизу
                                                                                                            //S.PrintMatrix();Console.WriteLine();
                l = k - 1;
                if (k != 0) while (l != -1) { S.LinesDiff(l, k, S[l, j] / S[k, j]); l--; }//отнимать от строк сверху
            }

            for (int i = 0; i < this.Dim; i++) this.x[i] = S[i, this.Dim] / S[i, i];
        }
        private class CMatrix
        {
            private Matrix R, I;
            public Complex this[int i, int j]
            {
                get
                {
                    return R[i, j] + Complex.I * I[i, j];
                }
                set
                {
                    R[i, j] = value.Re;
                    I[i, j] = value.Im;
                }
            }

            public CMatrix(int n, int m)
            {
                R = new Matrix(n, m);
                I = new Matrix(n, m);

            }

            public void LinesSwap(int a, int b)
            {
                R.LinesSwap(a, b);
                I.LinesSwap(a, b);
            }
            public void LinesDiff(int a, int b, Complex coef)
            {
                for (int k = 0; k < R.m; k++) this[a, k] -= coef * this[b, k];
            }
        }

        /// <summary>
        /// Вывести систему на консоль
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < this.Dim; i++)
            {
                string s = "||";
                for (int j = 0; j < this.Dim - 1; j++)
                    s += "\t" + A[i, j].ToString() + " ";
                s += "\t" + A[i, this.Dim - 1].ToString() + "|| \t" + x[i].ToString() + " \t||" + b[i].ToString() + "||";
                s.Show();
            }
        }
    }

    /// <summary>
    /// Класс прямых на плоскости (Ax+By+C=0)
    /// </summary>
    public class Line2D
    {
        /// <summary>
        /// Коэффициент при X
        /// </summary>
        public double A = 0;
        /// <summary>
        /// Коэффициент при Y
        /// </summary>
        public double B = 0;
        /// <summary>
        /// Свободный коэффициент
        /// </summary>
        public double C = 0;

        /// <summary>
        /// Тип прямой
        /// </summary>
        public enum Type { ParallelOx, ParallelOy, Other }
        /// <summary>
        /// Тип прямой
        /// </summary>
        public Type LineType
        {
            get
            {
                if (A == 0) return Type.ParallelOx;
                if (B == 0) return Type.ParallelOy;
                else return Type.Other;
            }
        }
        /// <summary>
        /// Тип взаимного отношения прямых
        /// </summary>
        public enum Mode { Parallel, Perpendicular }
        /// <summary>
        /// Тип уравнения прямой
        /// </summary>
        public enum EquType { Normal, Other }
        /// <summary>
        /// Тангенс угла наклона
        /// </summary>
        public double Corner_k
        {
            get
            {
                if (LineType == Type.ParallelOx) return 0;
                if (LineType == Type.ParallelOy) return Double.PositiveInfinity;
                return -A / B;
            }
        }

        /// <summary>
        /// Задать прямую по её коэффициентам из уравнения ax+by+c=0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Line2D(double a, double b, double c, EquType t = EquType.Other)
        {
            A = a;
            B = b;
            C = c;
            Modif(t);
        }
        /// <summary>
        /// Задать прямую проходящую через две точки
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public Line2D(Point p1, Point p2, EquType t = EquType.Other) : this(p2.y - p1.y, p1.x - p2.x, p1.y * (p2.x - p1.x) - p1.x * (p2.y - p1.y), t) { }
        /// <summary>
        /// Задать прямую, находящуюся относительно указанной прямой и указанной точки в заданном соотношении
        /// </summary>
        /// <param name="line"></param>
        /// <param name="p"></param>
        /// <param name="m"></param>
        public Line2D(Line2D line, Point p, Mode m, EquType t = EquType.Other)
        {
            if (m == Mode.Parallel)
            {
                A = line.A; B = line.B;
                C = -A * p.x - B * p.y;
            }
            else
            {
                A = -line.B;
                B = line.A;
                C = -line.A * p.y + line.B * p.x;
            }
            Modif(t);
        }
        /// <summary>
        /// Задать прямую через коэффициенты уравнения из y=ax+b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Line2D(double a, double b, EquType t = EquType.Other) : this(-a, 1, -b, t) { }
        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="l"></param>
        public Line2D(Line2D l) { A = l.A; B = l.B; C = l.C; }

        private void Modif(EquType t = EquType.Other)
        {
            if (A == 0 && B == 0) throw new Exception("Главные коэффициенты оба равны 0!");
            if (A <= 0 && B <= 0 && C <= 0) { A *= -1; B *= -1; C *= -1; }
            if (t == EquType.Other)
            {
                if (LineType == Type.Other) GiveDivisionByNOD();
            }
            else
            {
                double d = new Complex(A, B).Abs;
                A /= d; B /= d; C /= d;
            }


        }
        private void GiveDivisionByNOD()
        {
            int a = A.DimOfFractionalPath(), b = B.DimOfFractionalPath(), c = C.DimOfFractionalPath();
            int max = Expendator.Max(a, b, c);
            for (int i = 0; i < max; i++)
            {
                A *= 10; B *= 10; C *= 10;
            }
            max = (int)Number.Rational.Nod((long)A, (long)B);
            A /= max;
            B /= max;
            C /= max;
        }

        /// <summary>
        /// Функция, соответствующая прямой, от аргумента x, кроме того случая, когда B=0
        /// </summary>
        public RealFunc Func
        {
            get
            {
                if (A == 0) return (double x) => -C / B;
                if (B == 0) return (double x) => -C / A;
                return (double x) => -(A * x + C) / B;
            }
        }

        /// <summary>
        /// Точка пересечения двух прямых либо null, когда прямые не пересекаются или совпадают
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point InterSecPoint(Line2D a, Line2D b)
        {
            if (IsParallel(a, b)) return null;
            SLAU s = new SLAU(new Matrix(new double[,] { { a.A, a.B, -a.C }, { b.A, b.B, -b.C } }));
            s.GaussSelection();
            return new Point(s.x[0], s.x[1]);
        }
        /// <summary>
        /// Угол между прямыми
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Corner(Line2D x, Line2D y)
        {
            if (IsPerpendicular(x, y)) return Math.PI / 2;
            return Math.Atan2(x.A * y.B - x.B * y.A, x.A * y.A + x.B * y.B);
        }
        /// <summary>
        /// Расстояние от точки до прямой
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double Distance(Point p) => form(p) / ((Complex)p).Abs;

        private double form(Point p) => A * p.x + B * p.y + C;

        public static bool IsPerpendicular(Line2D x, Line2D y) => x.A * y.A + x.B * y.B == 0;
        public static bool IsParallel(Line2D a, Line2D b) => a.A / b.A == a.B / b.B;
        public static bool operator ==(Line2D a, Line2D b)
        {
            return (a.A == b.A) && (a.B == b.B) && (a.C == b.C);
        }
        public static bool operator !=(Line2D a, Line2D b) => !(a == b);

        public override string ToString()
        {
            string b = (B < 0) ? "-" : "+", c = (C < 0) ? "-" : "+";
            string s = $"{A}x {b} {B.Abs()}y {c} {C.Abs()} = 0";
            return s;
        }

        public override bool Equals(object obj)
        {
            var d = obj as Line2D;
            return d != null &&
                   A == d.A &&
                   B == d.B &&
                   C == d.C;
        }

        public override int GetHashCode()
        {
            var hashCode = 793064651;
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + C.GetHashCode();
            return hashCode;
        }
    }
    /// <summary>
    /// Класс отрезков на плоскости
    /// </summary>
    public class Cut
    {
        private Point First, Second;
        private Line2D line;


        /// <summary>
        /// Первая точка на прямой (начало отрезка)
        /// </summary>
        public Point FirstPoint => new Point(First);
        /// <summary>
        /// Вторая точка на прямой (конец отрезка)
        /// </summary>
        public Point SecondPoint => new Point(Second);
        /// <summary>
        /// Прямая, на которой лежит отрезок
        /// </summary>
        public Line2D Line => new Line2D(line);
        /// <summary>
        /// Длина отрезка
        /// </summary>
        public double Length => Point.Eudistance(First, Second);

        /// <summary>
        /// Задать отрезок по его концам
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Cut(Point first, Point second)
        {
            this.line = new Line2D(first, second, Line2D.EquType.Normal);
            First = new Point(first);
            Second = new Point(second);
        }

        /// <summary>
        /// Выводит точку на отрезке, соответствующую естественному параметру
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Point Transfer(double t)
        {
            if (t > this.Length) throw new Exception("Параметр выходит за границы");
            double k = line.Corner_k;
            if (k == 0) return new Point(First.x + t * Math.Sign(Second.x - First.x), First.y);
            if (k == Double.PositiveInfinity) return new Point(First.x, First.y + t * Math.Sign(Second.y - First.y));



            //если прямая не параллельна какой-либо оси          
            double cor = Math.Atan(k);
            double x = First.x + t * Math.Cos(cor);
            return new Point(x, line.Func(x));
        }

    }
    /// <summary>
    /// Многоугольник на плоскости
    /// </summary>
    public class Polygon
    {
        private Point center;
        private Point[] vertexes;
        private double corner, r, a;

        private Line2D[] lines;
        private PointFunc[] transfs;
        private void MakeLines()
        {
            lines = new Line2D[VertCount - 1];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new Line2D(vertexes[i], vertexes[i + 1]);
                transfs[i] = new PointFunc(new Cut(vertexes[i], vertexes[i + 1]).Transfer);
            }

        }

        /// <summary>
        /// Центр многоугольника
        /// </summary>
        public Point Center => new Point(center);
        /// <summary>
        /// Вершины многоугольника
        /// </summary>
        public Point[] Vertexes
        {
            get
            {
                Point[] res = new Point[vertexes.Length];
                for (int i = 0; i < res.Length; i++)
                    res[i] = new Point(vertexes[i]);
                return res;

            }
        }
        /// <summary>
        /// Количество вершин многоугольника
        /// </summary>
        public int VertCount => vertexes.Length;
        /// <summary>
        /// Периметр многоугольника
        /// </summary>
        public double Perimeter => a * VertCount;
        /// <summary>
        /// Длина стороны многоугольника
        /// </summary>
        public double CutLength => a;
        /// <summary>
        /// Площадь многоугольника
        /// </summary>
        public double S => VertCount * r * r / 2 * Math.Sin(corner);

        /// <summary>
        /// Создание многоугольника по его параметрам
        /// </summary>
        /// <param name="center">Центр многоугольника</param>
        /// <param name="vertcount">Число вершин</param>
        /// <param name="sidelenght">Длина стороны</param>
        /// <param name="somecorner">Угол между осью Х и отрезком, соединяющим центр многоугольника с какой-то его вершиной</param>
        public Polygon(Point center, int vertcount = 3, Double sidelenght = 1, double somecorner = 0)
        {
            int n = vertcount;
            corner = 2 * Math.PI / n;
            r = sidelenght / Math.Sqrt(2 * (1 - Math.Cos(corner)));
            this.center = new Point(center);
            this.vertexes = new Point[n];
            for (int i = 0; i < n; i++)
                vertexes[i] = new Point(center.x + r * Math.Cos(somecorner + i * corner), center.y + r * Math.Sin(somecorner + i * corner));
            a = sidelenght;
            MakeLines();
        }

        /// <summary>
        /// Возвращает точку на кривой в зависимости от значения естественного параметра
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Point Transfer(double t)
        {
            double div = t / a;
            int tmp = (int)Math.Ceiling(div);
            return transfs[tmp](t - a * tmp);

        }

        [Obsolete]
        /// <summary>
        /// Преобразование многоугольника в кривую (для интегрирования) 
        /// </summary>
        /// <param name="p"></param>
        /// <remarks>Преобразование красивое, но не очень хорошее, так как многие данные придётся вычислять повторно</remarks>
        public static implicit operator Curve(Polygon p) => new Curve(0, p.Perimeter,
            (double t) => p.Transfer(t).x, (double t) => p.Transfer(t).y, p.a,
         (double t, double r) => new Polygon(p.Center, p.VertCount, r, p.corner).Transfer(t).x,
         (double t, double r) => new Polygon(p.Center, p.VertCount, r, p.corner).Transfer(t).y,
         (double a, double b, double r) =>
         {
             Polygon t1 = new Polygon(p.Center, p.VertCount, p.a + b / 2, p.corner);
             Polygon t2 = new Polygon(p.Center, p.VertCount, p.a - b / 2, p.corner);
             return (t1.S - t2.S) * a / p.Perimeter;
         },
         (double t) => t * p.VertCount);
        /// <summary>
        /// Перевод многоугольника в кривую по более оптимальному алгоритму, рассчитанному на интегрирование
        /// </summary>
        public Curve ToCurve => new Curve(0, this.Perimeter, (double t, double r) => new Polygon(this.Center, this.VertCount, r, this.corner).Transfer(t), this.a,
                     (double a, double b, double r) =>
                     {
                         Polygon t1 = new Polygon(this.Center, this.VertCount, this.a + b / 2, this.corner);
                         Polygon t2 = new Polygon(this.Center, this.VertCount, this.a - b / 2, this.corner);
                         return (t1.S - t2.S) * a / this.Perimeter;
                     },
         (double t) => t * this.VertCount);
    }


    /// <summary>
    /// Класс методов суммирования
    /// </summary>
    public static class Sums
    {
        /// <summary>
        /// Сумма частичного ряда
        /// </summary>
        /// <param name="n0">Начальный номер члена ряда</param>
        /// <param name="nmax">Конечный номер члена ряда</param>
        /// <param name="f">Функция, сопоставляющая член ряда по номеру</param>
        /// <returns></returns>
        public static Complex Sum(int n0, int nmax, Func<int, Complex> f)
        {
            int tmp = nmax - n0 + 1;
            Complex[] mas = new Complex[tmp];
            for (int i = 0; i < tmp; i++)
                mas[i] = f(n0 + i);
            Array.Sort(mas);
            return mas.Sum();
        }
        /// <summary>
        /// Сумма бесконечного ряда
        /// </summary>
        /// <param name="n0">Начальный номер члена ряда</param>
        /// <param name="f">Функция, сопоставляющая член ряда номеру</param>
        /// <param name="eps">Точность</param>
        /// <param name="ndo">Минимальное количество членов, которые должны быть суммированы</param>
        /// <returns></returns>
        public static Complex Sum(int n0, Func<int, Complex> f, double eps = 1e-8, int ndo = 10, int ndomax = 100)
        {
            Complex sum = Sum(n0, n0 + ndo, f);
            int i = 1;
            Complex sum2 = 0;
            Complex tmp = f(n0 + ndo + i);
            do
            {
                sum2 += tmp;
                i++;
                tmp = f(ndo + i);
            }
            while (tmp.Abs >= eps && i + ndo <= ndomax);
            return sum + sum2;
        }
        /// <summary>
        /// Сумма ряда от -inf до inf
        /// </summary>
        /// <param name="f">Функция, сопоставляющая член ряда номеру</param>
        /// <param name="eps">Точность</param>
        /// <param name="ndo">Минимальное количество членов, которые должны быть суммированы в одну сторону</param>
        /// <returns></returns>
        public static Complex Sum(Func<int, Complex> f, double eps = 1e-8, int ndo = 10)
        {
            Func<int, Complex> f2 = (int n) => f(-n);
            Complex sum1 = 0, sum2 = 0;
            Parallel.Invoke(() => sum1 = Sum(0, f, eps, ndo), () => sum2 = Sum(1, f2, eps, ndo));
            return sum1 + sum2;
        }

        /// <summary>
        /// Сумма частичного ряда
        /// </summary>
        /// <param name="n0">Начальный номер члена ряда</param>
        /// <param name="nmax">Конечный номер члена ряда</param>
        /// <param name="f">Функция, сопоставляющая член ряда по номеру</param>
        /// <returns></returns>
        public static CVectors Sum(int n0, int nmax, Func<int, CVectors> f)
        {
            int tmp = nmax - n0 + 1;
            CVectors[] mas = new CVectors[tmp];
            for (int i = 0; i < tmp; i++)
                mas[i] = f(n0 + i);
            Array.Sort(mas);
            CVectors sum = new CVectors(mas[0].Degree);
            for (int i = 0; i < sum.Degree; i++)
                sum += mas[i];
            return sum;
        }
        /// <summary>
        /// Сумма бесконечного ряда
        /// </summary>
        /// <param name="n0">Начальный номер члена ряда</param>
        /// <param name="f">Функция, сопоставляющая член ряда номеру</param>
        /// <param name="eps">Точность</param>
        /// <param name="ndo">Минимальное количество членов, которые должны быть суммированы</param>
        /// <returns></returns>
        public static CVectors Sum(int n0, Func<int, CVectors> f, double eps = 1e-8, int ndo = 10, int ndomax = 1000)
        {
            CVectors sum = Sum(n0, n0 + ndo, f);
            int i = 1;
            CVectors sum2 = new CVectors(sum.Degree);
            CVectors tmp = f(n0 + ndo + i);
            do
            {
                sum2 += tmp;
                i++;
                tmp = f(ndo + i);
            }
            while (tmp.Abs >= eps && i + ndo <= ndomax);
            return sum + sum2;
        }
        /// <summary>
        /// Сумма ряда от -inf до inf
        /// </summary>
        /// <param name="f">Функция, сопоставляющая член ряда номеру</param>
        /// <param name="eps">Точность</param>
        /// <param name="ndo">Минимальное количество членов, которые должны быть суммированы в одну сторону</param>
        /// <returns></returns>
        public static CVectors Sum(Func<int, CVectors> f, double eps = 1e-8, int ndo = 10)
        {
            Func<int, CVectors> f2 = (int n) => f(-n);
            CVectors sum1 = null, sum2 = null;
            Parallel.Invoke(() => sum1 = Sum(0, f, eps, ndo), () => sum2 = Sum(1, f2, eps, ndo));
            return sum1 + sum2;
        }
    }

    /// <summary>
    /// Класс с вейвлетным преобразованием
    /// </summary>
    public class Wavelet
    {
        private static double sqrt2pi = Math.Sqrt(2 * Math.PI);
        private static readonly ComplexFunc sigma = (Complex z) => Sums.Sum(1, n => Complex.Sin(Math.PI * z * n * n) / n / n, eps);

        /// <summary>
        /// Масштабный множитель
        /// </summary>
        public double k;
        /// <summary>
        /// Частота
        /// </summary>
        public double w;
        /// <summary>
        /// Материнский вейвлет
        /// </summary>
        private RToC Mother;
        /// <summary>
        /// Фурье-образ материнского вейвлета
        /// </summary>
        private ComplexFunc FMother;

        private Wavelets type;
        /// <summary>
        /// Тип исходного вейвлета
        /// </summary>
        public Wavelets Type => this.type;

        /// <summary>
        /// Половина длины отрезка интегрирования
        /// </summary>
        private double N = 20;

        public static double eps = 1e-15;

        /// <summary>
        /// Перечисление доступных вейвлетов
        /// </summary>
        public enum Wavelets
        {
            /// <summary>
            /// Гауссов вейвлет первого порядка
            /// </summary>
            WAVE,
            /// <summary>
            /// Мексиканская шляпа
            /// </summary>
            MHAT,
            /// <summary>
            /// "difference of gaussians"
            /// </summary>
            DOG,
            /// <summary>
            /// "Littlewood & Paley"
            /// </summary>
            LP,
            /// <summary>
            /// Хаар-вейвлет
            /// </summary>
            HAAR,
            /// <summary>
            /// Французская шляпа
            /// </summary>
            FHAT,
            /// <summary>
            /// Вейвлет Морле
            /// </summary>
            Morlet
        }

        /// <summary>
        /// Создание вейвлета по масштабному множителю с указанием вейвлета из перечисления
        /// </summary>
        /// <param name="W"></param>
        /// <param name="w"></param>
        /// <param name="k"></param>
        public Wavelet(Wavelets W = Wavelets.MHAT, double k = -1, double ww = 1)
        {
            this.k = k;
            this.type = W;
            switch (W)
            {
                case Wavelets.WAVE:
                    this.Mother = (double t) => -t * Math.Exp(-t.Sqr() / 2);
                    this.FMother = (Complex w) => Complex.I * w * sqrt2pi * Complex.Exp(-w * w / 2);
                    break;
                case Wavelets.MHAT:
                    this.Mother = (double t) => { double sqr = t.Sqr(); return (1 - sqr) * Math.Exp(-sqr / 2); };
                    this.FMother = (Complex w) => -w * w * sqrt2pi * Complex.Exp(-w * w / 2);
                    break;
                case Wavelets.DOG:
                    this.Mother = (double t) => { double sqr = t.Sqr(); return Math.Exp(-sqr / 2) - 0.5 * Math.Exp(-sqr / 8); };
                    this.FMother = (Complex w) => sqrt2pi * (Complex.Exp(-w * w / 2) - Complex.Exp(-2 * w * w));
                    break;
                case Wavelets.LP:
                    this.Mother = t => { double pt = t * Math.PI; return (Math.Sin(2 * pt) - Math.Sin(pt)) / pt; };
                    this.FMother = (Complex w) => { if (w.Abs <= 2 * Math.PI && w.Abs >= Math.PI) return 1.0 / sqrt2pi; return 0; };
                    break;
                case Wavelets.HAAR:
                    this.Mother = t =>
                    {
                        if (t >= 0)
                        {
                            if (t <= 0.5) return 1;
                            if (t <= 1) return -1;
                            return 0;
                        }
                        return 0;
                    };
                    this.FMother = (Complex w) => 4 * Complex.I * Complex.Exp(Complex.I * w / 2) / w * Complex.Sin(w / 4).Sqr();
                    break;
                case Wavelets.FHAT:
                    this.Mother = t =>
                    {
                        double q = t.Abs();
                        if (q <= 1.0 / 3) return 1;
                        if (q <= 1) return -0.5;
                        return 0;
                    };
                    this.FMother = (Complex w) => 4 * Complex.Sin(w / 3).Pow(3) / w;
                    break;
                case Wavelets.Morlet:
                    this.Mother = t => Math.Exp(-t.Sqr() / 2) * Complex.Exp(Complex.I * w * t);
                    this.FMother = (Complex w) => sigma(w) * sqrt2pi * Complex.Exp(-(w - this.w).Sqr() / 2);
                    break;
            }
        }

        /// <summary>
        /// Функция, получившаяся при последнем анализе 
        /// </summary>
        public DComplexFunc LastAnalResult = null;
        private Memoize<Point, Complex> MemofLastAnal = null;
        /// <summary>
        /// Набор значений последнего результата анализа
        /// </summary>
        public ConcurrentDictionary<Point, Lazy<Complex>> Dic => MemofLastAnal.dic;

        /// <summary>
        /// Вейвлет-образ указанной функции
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public DComplexFunc GetAnalys(RealFunc f)
        {
            DComplexFunc s = (double a, double b) =>
             {
                 DefInteg.GaussKronrod.NodesCount count = DefInteg.GaussKronrod.NodesCount.GK61;

                 ComplexFunc F1 = (Complex t) => f(t.Re) * this.Mother((t.Re - b) / a).Conjugate;
                 ComplexFunc F2 = (Complex t) => f(-t.Re) * this.Mother((-t.Re - b) / a).Conjugate;
                 double con = 1.0 / Math.Sqrt(a.Abs()) /*Math.Pow(a.Abs(), this.k/2)*/;
                 Complex t1 = 0, t2 = 0;
                 Parallel.Invoke(
                     () => t1 = DefInteg.GaussKronrod.DINN_GK(F1, 0, 0, 0, 0, 0, 0, eps: eps, nodesCount: count),
                     () => t2 = DefInteg.GaussKronrod.DINN_GK(F2, 0, 0, 0, 0, 0, 0, eps: eps, nodesCount: count)
                     );
                 //return con*DefInteg.GaussKronrod.ParallelGaussKronrod(F1,-N,N,61,10);
                 return con * (t1 + t2);
             };
            MemofLastAnal = new Memoize<Point, Complex>((Point p) => s(p.x, p.y));
            LastAnalResult = new DComplexFunc((double a, double b) => MemofLastAnal.Value(new Point(a, b)));
            return LastAnalResult;
        }
        /// <summary>
        /// Обратное вейвлет-преобразование указанной функции
        /// </summary>
        /// <param name="F"></param>
        /// <returns></returns>
        public RealFunc GetSyntesis(DComplexFunc F = null)
        {
            //вычисление коэффициента С
            //надо добавить какое-нибудь ограничение на <inf 
            Complex C;
            if (this.Type == Wavelets.LP) C = Math.Log(2) / Math.PI;//2*DefInteg.GaussKronrod.MySimpleGaussKronrod((Complex w) =>
            //     {
            //         Complex fi = this.FMother(w);
            //         return fi.Abs.Sqr() / w.Abs;
            //     }, Math.PI, 2 * Math.PI);
            else
                C = DefInteg.GaussKronrod.DINN_GKwith0Full((Complex w) =>
                {
                    if (w == 0) return 0;
                    //Complex fi = DefInteg.GaussKronrod.DINN_GKwith0Full(t=>this.Mother(t.Re)*Complex.Exp(-Complex.I*w*t.Re));//fi.Show();
                    Complex fi = this.FMother(w);
                    return fi.Abs.Sqr() / w.Abs;
                }, eps: eps);
            C *= Math.Sqrt(2);
            //C.Show();
            //задание промежуточных переменных
            if (F != null)
            {
                Memoize<Point, Complex> f = new Memoize<Point, Complex>((Point p) => F(p.x, p.y));
                //выдача двойного интеграла
                return (double t) => (DefInteg.DoubleIntegralIn_FULL((Point p) => (this.Mother((t - p.y) / p.x) * f.Value(p)/*F(p.x, p.y)*/ / p.x / p.x/*Math.Pow(p.x.Abs(), this.k+3)*/).Re, eps: eps, parallel: true, M: DefInteg.Method.GaussKronrod61, changestepcount: 0, a: 1, b: 10) / C).Re;
            }
            else
                return (double t) => (DefInteg.DoubleIntegralIn_FULL((Point p) => (this.Mother((t - p.y) / p.x) * LastAnalResult(p.x, p.y)/*F(p.x, p.y)*/ / p.x / p.x/*Math.Pow(p.x.Abs(), this.k+3)*/).Re, eps: eps, parallel: true, M: DefInteg.Method.GaussKronrod61, changestepcount: 0, a: 1, b: 10) / C).Re;
        }
    }

    /// <summary>
    /// Мемоизированная функция
    /// </summary>
    /// <typeparam name="TVal">Класс аргумента</typeparam>
    /// <typeparam name="TResult">Класс результата</typeparam>
    public class Memoize<TVal, TResult> //where TVal : class, struct//, ICloneable
    {
        private Func<TVal, TResult> M;
        /// <summary>
        /// Текущий словарь
        /// </summary>
        internal ConcurrentDictionary<TVal, Lazy<TResult>> dic => pr._cache;
        /// <summary>
        /// Число элементов в словаре
        /// </summary>
        public int Lenght => dic.Count;
        private CustomProvider<TVal, TResult> pr;

        /// <summary>
        /// Конструктор по обычной функции
        /// </summary>
        /// <param name="Memoize">Исходная функция</param>
        /// <param name="First">Точка, в которой можно посчитать первое значение функции (чтобы не было пустого словаря)</param>
        public Memoize(Func<TVal, TResult> Memoize)
        {
            M = new Func<TVal, TResult>(Memoize);
            pr = new CustomProvider<TVal, TResult>();
            pr.RunLongRunningOperation = new Func<TVal, TResult>(Memoize);
            //if(First!=null)dic.TryAdd(First, M(First));
        }

        /// <summary>
        /// Делегат, возвращающий оптимизированную за счёт мемоизации функцию
        /// </summary>
        public Func<TVal, TResult> Value => (TVal val) => pr.RunOperationOrGetFromCache(val);

        private class CustomProvider<S, OperationResult> //where S : class, struct//, ICloneable
        {
            public readonly ConcurrentDictionary<S, Lazy<OperationResult>> _cache = new ConcurrentDictionary<S, Lazy<OperationResult>>();

            public OperationResult RunOperationOrGetFromCache(S operationId)
            {
                return _cache.GetOrAdd(operationId,//(S)operationId.Clone(),
                    id => new Lazy<OperationResult>(() => RunLongRunningOperation(id))).Value;
            }

            internal Func<S, OperationResult> RunLongRunningOperation;
        }

    }

    /// <summary>
    /// Класс оптимизации функции двух аргументов
    /// </summary>
    public static class OptimizationDCompFunc
    {
        /// <summary>
        /// Поиск минимума функции на прямоугольнике, чьи стороны параллельны осям координат
        /// </summary>
        /// <param name="f">Оптимизируемая функция</param>
        /// <param name="x0"></param>
        /// <param name="X"></param>
        /// <param name="y0"></param>
        /// <param name="Y"></param>
        /// <param name="nodescount">Корень из числа точек, берущихся в прямоугольнике (нижняя граница, потому что если прямоугольник слишком далёк от квадрата, надо брять другое соотношение)</param>
        /// <param name="eps">Погрешность поиска</param>
        /// <param name="ogr">Через сколько максимально итераций нужно закончить цикл, если последние ogr итераций подряд точка максимума не изменялась</param>
        /// <returns></returns>
        public static Tuple<double, double> GetMaxOnRectangle(Func<double, double, double> f, double x0, double X, double y0, double Y, int nodescount = 10, double eps = 1e-7, int ogr = 3, bool useGradient = false, bool parallel = true)
        {
            double max = f(x0, y0);//max.Show();
            Tuple<double, double> res = new Tuple<double, double>(x0, y0);
            double x = (X - x0).Abs(), y = (Y - y0).Abs();
            int nodescI, nodescJ;
            if (x > y)
            {
                nodescJ = nodescount;
                nodescI = (int)(nodescount * (x / y));
            }
            else
            {
                nodescI = nodescount;
                nodescJ = (int)(nodescount * (x / y));
            }
            double[,] mas = new double[nodescI, nodescJ];
            int k = 0;

            while (((X - x0) * (Y - y0)).Abs() > eps && k <= ogr)
            {
                double xstep = (X - x0) / (nodescI /*+ 1*/);
                double ystep = (Y - y0) / (nodescJ /*+ 1*/);

                k++;
                if (!parallel)
                    for (int i = 0; i < nodescI; i++)
                        for (int j = 0; j < nodescJ; j++)
                        {
                            mas[i, j] = f(x0 + xstep * i, y0 + ystep * j);
                            if (mas[i, j] > max)
                            {
                                k = 0;
                                max = mas[i, j];//max.Show();
                                res = new Tuple<double, double>(x0 + xstep * i, y0 + ystep * j);
                            }
                        }
                else
                {
                    //параллельная версия
                    double[] maxmas = new double[nodescI];
                    Tuple<double, double>[] resmas = new Tuple<double, double>[nodescI];
                    for (int i = 0; i < nodescI; i++)
                    {
                        maxmas[i] = max;
                        resmas[i] = new Tuple<double, double>(res.Item1, res.Item2);
                    }

                    Parallel.For(0, nodescI, (int i) =>
                    {
                        for (int j = 0; j < nodescJ; j++)
                        {
                            mas[i, j] = f(x0 + xstep * i, y0 + ystep * j);
                            if (mas[i, j] > maxmas[i])
                            {
                                k = 0;
                                maxmas[i] = mas[i, j];//max.Show();
                                resmas[i] = new Tuple<double, double>(x0 + xstep * i, y0 + ystep * j);
                            }
                        }
                    });

                    max = maxmas.Max();
                    int tmp = Array.IndexOf(maxmas, max);
                    res = new Tuple<double, double>(resmas[tmp].Item1, resmas[tmp].Item2);
                }



                //double x = x0 + (X - x0) / 2;
                //double y = y0 + (Y - y0) / 2;
                //if (res.Item1 > x) x0 = x;
                //else X = x;
                //if (res.Item2 > y) y0 = y;
                //else Y = y;
                x = (X - x0) / 2;
                double x2 = x / 2, p1p = res.Item1 + x2, p1m = res.Item1 - x2;
                y = (Y - y0) / 2;
                double y2 = y / 2, p2p = res.Item2 + y2, p2m = res.Item2 - y2;

                if (p1m < x0) X = x0 + x;
                else if (p1p > X) x0 = X - x;
                else
                {
                    x0 = p1m;
                    X = p1p;
                }
                if (p2m < y0) Y = y0 + y;
                else if (p2p > Y) y0 = Y - y;
                else
                {
                    y0 = p2m;
                    Y = p2p;
                }

            }

            if (useGradient)
            {
                Complex point = new Complex(res.Item1, res.Item2);
                ComplexFunc cf = (Complex a) => f(a.Re, a.Im);
                Gradient(cf, ref point, eps: eps);
                res = new Tuple<double, double>(point.Re, point.Im);
            }

            return res;
        }

        /// <summary>
        /// Метод градиентного спуска к максимуму по модулю от функции
        /// </summary>
        /// <param name="f">Функция комплексного переменного</param>
        /// <param name="point">Начальная точка</param>
        /// <param name="alp">Коэффициент метода</param>
        /// <param name="maxcount">Максимальное число итераций</param>
        /// <param name="eps">Погрешность</param>
        private static void Gradient(ComplexFunc f, ref Complex point, double alp = 0.01, int maxcount = 100, double eps = 1e-14)
        {
            DefInteg.Residue.eps = eps;
            Complex gr = DefInteg.Residue.Derivative(f, point);
            Complex fp = f(point);
            int count = 0;
            while (gr.Abs > eps && count <= maxcount && alp > 10 * eps)
            {
                Complex p2 = point - alp * gr;
                Complex fp2 = f(p2);
                if (fp2.Abs > fp.Abs)
                {
                    point = new Complex(p2);
                    fp = new Complex(fp2);
                    gr = DefInteg.Residue.Derivative(f, p2);
                }
                else
                {
                    alp /= 2;
                }
                count++;
            }
            //point = new Complex(fp);
        }
    }

    /// <summary>
    /// Класс методов поиска корней
    /// </summary>
    public static class Roots
    {
        /// <summary>
        /// Простой поиск корней комплексной функции на действительном отрезке методом дихотомии
        /// </summary>
        /// <param name="f"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static Vectors MyHalfc(ComplexFunc f, double beg, double end, double step = 0.01, double eps = 1e-12)
        {
            List<double> list = new List<double>();
            double a = beg, a2 = beg + step, b = end, t1, t2, s;
            Complex fa = f(a), fa2 = f(a2), fc;
            while (a < b)
            {
                if (fa.Abs < eps) list.Add(a);
                else
                if (Math.Sign(fa.Re) * Math.Sign(fa2.Re) <= 0 && Math.Sign(fa.Im) * Math.Sign(fa2.Im) <= 0)//написал условие именно так, чтобы избежать переполнения
                {
                    t1 = a; t2 = a2;
                    while (t2 - t1 > eps)
                    {
                        s = (t1 + t2) / 2;
                        fc = f(s);//fc.Show();
                        if (fc.Abs < eps)
                            break;
                        if (Math.Sign(fa.Re) * Math.Sign(fc.Re) <= 0 && Math.Sign(fa.Im) * Math.Sign(fc.Im) <= 0)
                            t2 = s;
                        else t1 = s;
                    }
                    s = (t1 + t2) / 2;
                    if (f(s).Abs < 3) list.Add(s);
                }
                a = a2;
                a2 += step;
                fa = new Complex(fa2);
                fa2 = f(a2);
            }
            return new Vectors(list.ToArray());
        }
        /// <summary>
        /// Простой поиск корней как поиск минимумов модуля функции (которые должны быть равны 0)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static Vectors RootsByMinAbs(ComplexFunc f, double beg, double end, double step = 0.01, double eps = 1e-12)
        {
            RealFunc fabs = (double c) => f(c).Abs;
            List<double> list = new List<double>();

            double a = beg, a2 = beg + step, t1, t2, s, d1, d2, ds;
            double fa = fabs(a), fa2 = fabs(a2), fc;

            double der(double c) => fabs(c + eps) - fabs(c - eps);

            while (a < end)
            {
                d1 = der(a);
                d2 = der(a2);
                //$"{d1} {d2}".Show();
                if (fa < eps) list.Add(a);
                else if (fa2 < eps) list.Add(a2);
                else if (d1 < 0 && d2 > 0)
                {
                    t1 = a; t2 = a2;
                    while (t2 - t1 > eps)
                    {
                        //$"{d1} {d2}".Show();
                        s = (t1 + t2) / 2;
                        fc = fabs(s);//fc.Show();
                        ds = der(s);
                        if (fc < eps) break;
                        if (ds > 0)
                        { t2 = s; d2 = ds; }
                        else { t1 = s; d1 = ds; }
                    }
                    s = (t1 + t2) / 2;
                    list.Add(s);
                }

                a = a2;
                a2 += step;
                fa = fa2;
                fa2 = fabs(a2);
            }

            for (int i = 0; i < list.Count; i++)
                if (fabs(list[i]) > eps)
                {
                    list.RemoveAt(i);
                    i--;
                }

            return new Vectors(list.Distinct().ToArray());
        }

        /// <summary>
        /// Метод локального поиска корня
        /// </summary>
        public enum MethodRoot
        {
            Brent,
            Broyden,
            Bisec,
            Secant,
            NewtonRaphson,
            /// <summary>
            /// Комбинация методов Brent, Secant и Broyden
            /// </summary>
            Combine
            //Halfc
        }
        /// <summary>
        /// Поиск корней одним из специальных методов
        /// </summary>
        /// <param name="f"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <param name="eps"></param>
        /// <param name="m">Метод</param>
        /// <param name="withMuller">Дополнять ли корни корнями метода парабол</param>
        /// <returns></returns>
        public static Vectors OtherMethod(ComplexFunc f, double beg, double end, double step = 0.01, double eps = 1e-12, MethodRoot m = MethodRoot.Brent, bool withMuller = false)
        {
            List<double> list = new List<double>();
            Func<double, double> func = (double c) => f(c).ReIm;
            double a = beg, b = beg + step;
            Complex fa = f(a), fb = f(b);
            MethodR g;
            //MethodR half = (Func<double, double> ff, double begf, double endf, double epsf, uint N) => Optimization.Halfc((double c) => f(c).ReIm, begf, endf, step, eps);
            switch (m)
            {
                case MethodRoot.Brent:
                    g = BrentMethod;
                    break;
                case MethodRoot.Bisec:
                    g = bisectionMethod;
                    break;
                case MethodRoot.Secant:
                    g = secantMethod;
                    break;
                case MethodRoot.NewtonRaphson:
                    g = secantNewtonRaphsonMethod;
                    break;
                default:
                    g = BroydenMethod;
                    break;
            }

            if (withMuller)
                while (a < end)
                {
                    if (fa.Re * fb.Re <= 0 && fa.Im * fb.Im <= 0)
                    {
                        if (m == MethodRoot.Combine)
                        {
                            list.Add(BrentMethod(func, a, b, 1e-12, N: 1000));
                            list.Add(BroydenMethod(func, a, b, 1e-12, N: 1000));
                            list.Add(secantMethod(func, a, b, 1e-12, N: 1000));
                        }
                        else
                            list.Add(g(func, a, b, 1e-12, N: 1000));
                        //Optimization.Muller(f, a, new Complex((a + b) / 2, 0), new Complex(b, 0)).Re.Show();
                        list.Add(Optimization.Muller(f, a, new Complex((a + b) / 2, 0.01), new Complex((a + b) / 2, -0.01)).Re);
                    }

                    a = b;
                    b += step;
                    fa = new Complex(fb);
                    fb = f(b);
                }
            else
                while (a < end)
                {
                    if (fa.Re * fb.Re <= 0 && fa.Im * fb.Im <= 0)
                    {
                        if (m == MethodRoot.Combine)
                        {
                            list.Add(BrentMethod(func, a, b, 1e-12, N: 1000));
                            list.Add(BroydenMethod(func, a, b, 1e-12, N: 1000));
                            list.Add(secantMethod(func, a, b, 1e-12, N: 1000));
                        }
                        else
                            list.Add(g(func, a, b, 1e-12, N: 1000));
                    }


                    a = b;
                    b += step;
                    fa = new Complex(fb);
                    fb = f(b);
                }

            //new Vectors(list.ToArray()).Show();

            return new Vectors(list.Distinct().Where(n => !Double.IsNaN(n)&& f(n).Abs <= eps && n >= beg && n <= end).ToArray());
        }
    }

    /// <summary>
    /// Класс, обеспечивающий исследование колебаний
    /// </summary>
    public static class Waves
    {
        /// <summary>
        /// Нормаль
        /// </summary>
        public class Normal2D
        {
            /// <summary>
            /// Вектор нормали
            /// </summary>
            public Point n;
            /// <summary>
            /// Позиция приложения
            /// </summary>
            public Point Position;
            private double coef = 1;

            /// <summary>
            /// Создание вектора нормали к точке на окружности
            /// </summary>
            /// <param name="center">Центр окружности</param>
            /// <param name="position">Декартовы координаты точки на окружности</param>
            /// <param name="coefficent">Коэффициент умножения нормали</param>
            public Normal2D(Point center, Point position, double coefficent = 1)
            {
                double dx = position.x - center.x;
                double dy = position.y - center.y;
                double div = Math.Sqrt(dx * dx + dy * dy);
                this.coef = coefficent;
                this.n = new Point(dx / div * coef, dy / div * coef);
                this.Position = new Point(position);
            }
            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="coef"></param>
            /// <param name="normal"></param>
            /// <param name="position"></param>
            public Normal2D(double coef,Point normal,Point position)
            {
                this.n = new Point(normal.x*coef,normal.y*coef);
                this.Position = new Point(position);
                this.coef = coef;
            }
            /// <summary>
            /// Возвращает массив нормалей как массив точек
            /// </summary>
            /// <param name="mas"></param>
            /// <returns></returns>
            public static Point[] NormalsToPoins(Normal2D[] mas)
            {
                Point[] res = new Point[mas.Length];
                for (int i = 0; i < mas.Length; i++)
                    res[i] = new Point(mas[i].n);
                return res;
            }

            /// <summary>
            /// Умножить нормаль на число
            /// </summary>
            /// <param name="s"></param>
            /// <param name="d"></param>
            /// <returns></returns>
            public static Normal2D operator *(Normal2D s, double d) => new Normal2D(s.coef * d, s.n, s.Position);
        }

        /// <summary>
        /// Окружность (пьезоэлемент)
        /// </summary>
        public class Circle
        {
            /// <summary>
            /// Центр окружности
            /// </summary>
            public Point center;
            /// <summary>
            /// Радиус окружности
            /// </summary>
            public double radius;
            public Circle(Point center, double radius) { this.center = new Point(center); this.radius = radius; }

            /// <summary>
            /// Возврат нормали в точке по аргументу
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Normal2D GetNormal(double arg,double len=1)
            {
                Point pos = new Point(center.x + radius * Math.Cos(arg), center.y + radius * Math.Sin(arg));
                return new Normal2D(center, pos, len);
            }

            /// <summary>
            /// Возвращает массив точек на окружности
            /// </summary>
            /// <param name="args">Углы точек относительно центра окружности и оси X</param>
            /// <param name="weights">Веса точек (по умолчанию единичные)</param>
            /// <returns></returns>
            public Normal2D[] GetNormalsOnCircle(double[] args, double[] weights = null)
            {
                Normal2D[] res = new Normal2D[args.Length];
                for (int i = 0; i < res.Length; i++)
                    res[i] = new Normal2D(this.center, new Point(center.x + radius * Math.Cos(args[i]), center.y + radius * Math.Sin(args[i])), (weights == null) ? 1 : weights[i]);
                return res;
            }
            /// <summary>
            /// Возвращает массив равномерно рассположенных по окружности нормалей
            /// </summary>
            /// <param name="count"></param>
            /// <param name="weights"></param>
            /// <returns></returns>
            public Normal2D[] GetNormalsOnCircle(int count, double[] weights = null)
            {
                double h = 2 * Math.PI / (count - 1);
                double[] args = new double[count];
                for (int i = 0; i < count; i++)
                    args[i] = i * h;
                //$"{args[0]} {args[count-1]}".Show();
                return GetNormalsOnCircle(args, weights);
            }

            /// <summary>
            /// Записать поле (массивы аргументов x,y) и массивы значений Re ur, Im ur, Abs ur, Re uz, Im uz, Abs us в файл (чтобы потом нарисовать графики)
            /// </summary>
            /// <param name="filename">Имя файла</param>
            /// <param name="title">То, что должно быть позже написано над графиками</param>
            /// <param name="F">Функция (x,y,normal) -> (ur, uz)</param>
            /// <param name="circle">Окружность, относительно которой всё происходит</param>
            /// <param name="x0">Начало отрезка по х</param>
            /// <param name="X">Конец отрезка по х</param>
            /// <param name="xcount">Число точек по х</param>
            /// <param name="y0">Начало отрезка по у</param>
            /// <param name="Y">Конец отрезка по у</param>
            /// <param name="ycount">Число точек по у</param>
            /// <param name="k">Массив для отслеживания прогресса</param>
            public static void FieldToFile(string filename,Func<double,double,Point,Point/*[]*/,Tuple<Complex,Complex>> F,Circle circle, double x0,double X,int xcount,double y0,double Y,int ycount, IProgress<int> progress/*ref int[] k*/,System.Threading.CancellationToken token ,string title="",int normalscount=100)
            {
                double[] x = new double[xcount], y = new double[ycount];
               int[] k = new int[xcount * ycount];
                double hx = (X - x0) / (xcount - 1), hy = (Y - y0) / (ycount - 1);

                for (int i = 0; i < xcount; i++)
                    x[i] = x0 + i * hx;
                for (int i = 0; i < ycount; i++)
                    y[i] = y0 + i * hy;

                Complex[,] ur = new Complex[xcount, ycount], uz = new Complex[xcount, ycount];
                //Point[] Nmas = Waves.Normal2D.NormalsToPoins(circle.GetNormalsOnCircle(normalscount));

                //нахождение массивов
                Parallel.For(0, xcount, (int i) => {
               // for(int i=0;i<xcount;i++)
                    for(int j=0;j<ycount;j++)
                    {
                        if (token.IsCancellationRequested) return;
                        if(Point.Eudistance(new Point(x[i],y[j]),circle.center)>=circle.radius)//больше или равно, потому что в массивах изначально нули
                        {
                            Normal2D n = new Normal2D(circle.center, new Point(x[i], y[j]));
                            var tmp = F(x[i], y[j], circle.center,/*Nmas*/n.n);//if (Double.IsNaN((tmp.Item1 + tmp.Item2).Abs) || Double.IsInfinity((tmp.Item1 + tmp.Item2).Abs)) tmp = new Tuple<Complex, Complex>(0, 0);
                            ur[i, j] = new Complex(tmp.Item1);
                            uz[i, j] = new Complex(tmp.Item2);
                        }
                        else//иначе типа NA
                        {
                            ur[i, j] = new Complex(Double.NaN);
                            uz[i, j] = new Complex(Double.NaN);
                        }
                        k[(i) * ycount + j] = 1;

                        progress.Report(k.Sum());
                    }     
 });
                //-------------------------------------------------------------------------------------
                //запись в файлы
                StreamWriter fs = new StreamWriter(filename);
                string se = filename.Substring(0, filename.Length-4);//-.txt
                StreamWriter ts = new StreamWriter(se+"(title).txt");
                //StreamWriter ds = new StreamWriter(se + "(dim).txt");
                StreamWriter xs = new StreamWriter(se + "(x).txt");
                StreamWriter ys = new StreamWriter(se + "(y).txt");

                ts.WriteLine($"{title}");
                //fs.WriteLine($"dim {xcount} {ycount}");

                xs.WriteLine("x");
                for (int i = 0; i < xcount; i++)
                    xs.WriteLine(x[i]);

                ys.WriteLine("y");
                for (int i = 0; i < ycount; i++)
                    ys.WriteLine(y[i]);

                fs.WriteLine("urRe urIm urAbs uzRe uzIm uzAbs");
                for (int i = 0; i < xcount; i++)
                    for (int j = 0; j < ycount; j++)
                        if (Double.IsNaN(ur[i, j].Abs) || Double.IsNaN(uz[i, j].Abs))
                            fs.WriteLine("NA NA NA NA NA NA");
                        else
                            fs.WriteLine($"{ur[i, j].Re} {ur[i, j].Im} {ur[i, j].Abs} {uz[i, j].Re} {uz[i, j].Im} {uz[i, j].Abs}");

                fs.Close();
                ts.Close();
                xs.Close();
                ys.Close();
            }
        }

        /// <summary>
        /// Окружность с вырезом, представимым как круг с центром на большой окружности
        /// </summary>
        public class DCircle
        {
            /// <summary>
            /// Окружности
            /// </summary>
            private Circle circle1, circle2;
            /// <summary>
            /// Аргумент, определяющий положение центра меньшей окружности
            /// </summary>
            private double arg;
            /// <summary>
            /// Центральные половинные углы окружностей в радианах
            /// </summary>
            private double alp1, alp2;

            public double Radius => circle1.radius;
            public Point Center => circle1.center;

            /// <summary>
            /// Окружность с вырезом
            /// </summary>
            /// <param name="center">Центр большей окружности</param>
            /// <param name="diam1">Диамерт большей окружности</param>
            /// <param name="diam2">Диаметр меньшей окружности</param>
            /// <param name="arg">Угол в радианах, определяющий положение центра меньшей окружности</param>
            public DCircle(Point center, double diam1=1.6,double diam2=0.5, double arg = 4.8)
            {                
                this.arg = arg;

                double r1 = diam1 / 2, r2 = diam2 / 2;
                circle1 = new Circle(center, r1);
                circle2 = new Circle(new Point(/*-*/center.x + r1 /** Math.Cos(arg)*/, /*-*/center.y + 0/*r1 * Math.Sin(arg)*/), r2);

                double tmp = r2 / r1;
                alp1 = Math.Acos(1.0-0.5*tmp*tmp);//alp1.Show();
                alp2 = Math.Acos(0.5 * tmp);//alp2.Show();
            }

            /// <summary>
            /// Содержит ли окружность с вырезом точку в своей внутренности
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public bool ContainPoint(Point p)
            {
                p = p.Turn(circle1.center, -arg);
                return Point.Eudistance(circle1.center, p) < circle1.radius && Point.Eudistance(circle2.center, p) > circle2.radius;
            }

            /// <summary>
            /// Возвращает нормаль для точки на плоскости
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public Point GetNormal(Point p,double len=0.1,double eps=0.01)
            {
                p = p.Turn(circle1.center,-arg);
                Point nil = new Point(0);
                double corner = new Complex(p.x-circle1.center.x, p.y-circle1.center.y).Arg;//corner.Show();
                if (corner < -alp1-eps || corner > alp1+eps) 
                    return circle1.GetNormal(corner,len).n.Turn(nil, arg);//sec1.Show();sec2.Show();

                double d1 = Point.Eudistance(circle1.center, p); //circle2.center.Show();
                double d2 = Point.Eudistance(circle2.center, p);

                if (d2 == 0) return new Point(Math.Cos(arg)*len, Math.Sin(arg)*len);
                Normal2D res=new Normal2D(circle2.center, p,len);//res.n.Show(); circle1.radius.Show();
                if (d1 > circle1.radius+eps)
                    return res.n.Turn(nil, arg);
                return (res * (-1)).n.Turn(nil, arg);
            }

            /// <summary>
            /// Возвращает массив точек и массив нормалей в этих точках
            /// </summary>
            /// <param name="n1">Число точек на большей окружности</param>
            /// <param name="n2">Число точек на меньшей окружности</param>
            /// <returns></returns>
            public Tuple<Point[],Point[]> DrawMasses(int n1=100, int n2 = 10,double len=0.1,double eps=0.00001)
            {
                double h1 = (Math.PI * 2 - 2 * alp1-2*eps) / (n1 - 1);
                double h2 = (2 * alp2) / (n2 - 1),tmp;

                Point[] p = new Point[n1 + n2];
                Point[] n = new Point[n1 + n2];
                for(int i = 0; i < n1; i++)
                {
                    tmp = alp1 + eps + i * h1;
                    p[i] = new Point(circle1.radius * Math.Cos(tmp)+circle1.center.x, circle1.radius * Math.Sin(tmp)+circle1.center.y).Turn(circle1.center, arg);
                    n[i] = GetNormal(p[i],0.05*circle1.radius);
                }
                for(int i = 0; i < n2; i++)
                {
                    tmp =-Math.PI+ alp2 - i * h2;
                    p[n1 + i] = new Point(circle2.radius * Math.Cos(tmp) + circle2.center.x, circle2.radius * Math.Sin(tmp)+ circle2.center.y).Turn(circle1.center, arg);
                    n[n1 + i] = GetNormal(p[n1 + i],0.2*circle2.radius);
                }
                return new Tuple<Point[], Point[]>(p, n);
            }
        }

        /// <summary>
        /// Поворот точки на угол
        /// </summary>
        /// <param name="p"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        public static Point Turn(this Point p,Point center,double corner)
            {
                double sin = Math.Sin(corner);
                double cos = Math.Cos(corner);
            Point t = new Point(p.x - center.x, p.y - center.y);
                return new Point(t.x * cos - t.y * sin+ center.x, t.x * sin + t.y * cos+ center.y);
            }
    }

    /// <summary>
    /// Некоторые специальные функции
    /// </summary>
    public static class SpecialFunctions
    {
        /// <summary>
        /// Функция Бесселя
        /// </summary>
        /// <param name="a">Порядок</param>
        /// <param name="x">Аргумент</param>
        /// <returns></returns>
        public static Complex MyBessel(double a, Complex x)
        {
            if (x.Im.Abs() < 1e-13) return Computator.NET.Core.Functions.SpecialFunctions.BesselJν(a, x.Re);
            ComplexFunc f = (Complex c) => Complex.Cos(a * c - x * Complex.Sin(c));
            return FuncMethods.DefInteg.GaussKronrod.MySimpleGaussKronrod(f, 0, Math.PI, 61, true, 5) / Math.PI;
        }
    }
}