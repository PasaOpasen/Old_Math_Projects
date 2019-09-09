using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МатКлассы;
using Point = МатКлассы.Point;
using static МатКлассы.BeeHiveAlgorithm;

namespace Распознавание_цифр
{
    /// <summary>
    /// Вспомогательные методы
    /// </summary>
    public static class Expend
    {
        //static SqMatrix[] samples;
        static Expend()
        {
            //samples = new SqMatrix[15];
            //for(int i=1;i<=15;i++)
            //{
            //    string s = FuncTo2(i),s0="";
            //    for (int k = 0; k < 4 - s.Length; i++)
            //        s0 += "0";
            //    s = s0 + s;
            //    samples[i - 1] = new SqMatrix(new double[,] { { s[0].ToInt32(), s[1].ToInt32() }, { s[2].ToInt32(), s[3].ToInt32() } });
            //}
        }
        public static int count = 0;

        static string FuncTo2(int number)
        {
            return (number == 1) ? "1" : FuncTo2(number >>= 1) + (number & 1);
        }

        static bool[,] BoolMas;

        static bool[,] ToBoolMas(this Bitmap bitmap)
        {
            bool[,] res = new bool[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Height; i++)
                for (int j = 0; j < bitmap.Width; j++)
                {
                    res[j, i] = bitmap.IsBlack(j, i);
                    //$"{res[i, j]} {bitmap.GetPixel(j, i).ToArgb()}".Show();
                }
            return res;
        }
        static bool IsBlack(this Bitmap bitmap, int i, int j) => bitmap.GetPixel(i, j).ToArgb() != -1;
        static bool IsBlack(int i, int j) => BoolMas[i, j];

        /// <summary>
        /// Получает множество точек с картинки
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        internal static PointSet[] GetPointSets(Bitmap bitmap)
        {
            List<PointSet> list = new List<PointSet>();
            BoolMas = bitmap.ToBoolMas();

            //"Есть".Show();
            for (int i = 0; i < bitmap.Height; i++)
                for (int j = 0; j < bitmap.Width; j++)
                    if (BoolMas[j, i])
                    {
                        //$"{i} {j}".Show();
                        Point p = new Point(i, j);
                        bool tmp = true;
                        for (int k = 0; k < list.Count; k++)
                            if (list[k].Contains(p))
                            {
                                tmp = false;
                                break;
                            }

                        if (tmp)
                        {
                            PointSet ps = new PointSet();
                            AddInSet(bitmap, ref ps, i, j);
                            list.Add(ps);
                        }
                    }
            //for (int i = 0; i < list.Count; i++) list[i].Center.Show();
            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// Возвращает границы множеств
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        internal static PointSet[] GetCurvePointSets(Bitmap bitmap)
        {
            Program.form.toolStripLabel1.Text = "Начинается распознавание пиксельных множеств";
            PointSet[] ps = GetPointSets(bitmap);
            PointSet[] res = new PointSet[ps.Length];
            Program.form.toolStripLabel1.Text = "Происходит выделение границ цифр";
            for (int k = 0; k < res.Length; k++)
            {
                res[k] = new PointSet();
                Point p;
                int x, y;
                for (int i = 0; i < ps[k].Set.Count; i++)
                {
                    p = ps[k].Set[i].dup;
                    y = p.x.ToInt32();
                    x = p.y.ToInt32();
                    if (!(BoolMas[x - 1, y - 1] && BoolMas[x + 1, y + 1] && BoolMas[x + 1, y - 1] && BoolMas[x - 1, y + 1]))
                        res[k].TryToAdd(p);
                }

                for (int i = 0; i < res[k].Set.Count; i++)
                    bitmap.SetPixel(res[k].Set[i].y.ToInt32(), res[k].Set[i].x.ToInt32(), Color.Yellow);
            }

            bitmap.Save($"border{Expend.count}.png", System.Drawing.Imaging.ImageFormat.Png);
            return res;
        }
        /// <summary>
        /// Возвращает векторы из преобразования Хау
        /// Каждый вектор соответствует точечному множеству
        /// Вектор показывает число прямых с аргументами (0, меньше 90 ,90, больше 90)
        /// </summary>
        /// <param name="ps">Массив граничных точек для каждого множества</param>
        /// <param name="n">Число тестируемых углов alpha</param>
        /// <param name="len">Сколько пикселей подряд считаются прямой</param>
        /// <param name="eps">Точка считается лежащей на прямой, если её расстояние до прямой не больше eps</param>
        /// <returns></returns>
        internal static Tuple<Vectors[], Vectors[]> GetHauVectors(PointSet[] ps, int n = 40, int len = 6, double eps = 1e-3)
        {
            Vectors[] res = new Vectors[ps.Length];

            double[] alp = Expendator.Seq(0, Math.PI, n).Union(new double[] { Math.PI / 2 }).ToArray();
            n = alp.Length;

            Program.form.toolStripLabel1.Text = "Происходит распознавание через преобразование Хау";
            //проход по всем множествам
            Parallel.For(0, res.Length, (int k) =>
            {
                res[k] = new Vectors(4);
                var di = ps[k].Range;
                int hp = (di[3] - di[2]);
                //double[] p = Expendator.Seq(0, hp, hp + 1);
                //Matrix mat = new Matrix(hp + 1, n);
                hp = ps[k].Diametr.ToInt32();
                double[] p = Expendator.Seq(0, hp, hp + 1);
                Matrix mat = new Matrix(hp + 1, n);

                //проход по матрице
                for (int i = 0; i < hp + 1; i++)
                    for (int j = 0; j < n; j++)
                        for (int l = 0; l < ps[k].Set.Count; l++)
                            if ((p[i] - (ps[k].Set[l].x - di[0]) * Math.Cos(alp[j]) - (ps[k].Set[l].y - di[2]) * Math.Sin(alp[j])).Abs() < eps)
                                mat[i, j]++;


                for (int i = 0; i < hp + 1; i++)
                    for (int j = 0; j < n; j++)
                        if (mat[i, j] >= len)
                        {
                            if (alp[j] == 0) res[k][0]++;
                            else if ((alp[j] - Math.PI / 2).Abs() < eps) res[k][2]++;
                            else if (alp[j] < Math.PI / 2) res[k][1]++;
                            else res[k][3]++;
                        }


                //res[k]=res[k].Normalizing;

            });
            return new Tuple<Vectors[], Vectors[]>(res, PointSet.GetCountCurves(ps));
        }

        /// <summary>
        /// Рекурсивная функция для создания множества точек
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="set"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        static void AddInSet(Bitmap bitmap, ref PointSet set, int i, int j)
        {
            if (IsBlack(j, i) && !set.Contains(new Point(i, j)))
            {
                set.TryToAdd(new Point(i, j));

                AddInSet(bitmap, ref set, i + 1, j);
                AddInSet(bitmap, ref set, i, j + 1);
                AddInSet(bitmap, ref set, i - 1, j);
                AddInSet(bitmap, ref set, i, j - 1);
            }
        }

        /// <summary>
        /// Возвращает набор векторов, которые можно сравнить с образцами.
        /// Каждый вектор соответствует одному символу на изображении
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        internal static Tuple<Vectors[], Vectors[]> GetVectors(Bitmap bitmap)
        {
            Program.form.toolStripLabel1.Text = "Начато распознавание пиксельных множеств";
            var ps = GetPointSets(bitmap); PointSet.DrawRect(ps, ref bitmap);
            Vectors[] res = new Vectors[ps.Length];

            Program.form.toolStripLabel1.Text = "Вычисляются эйлеровы характеристики";
            Parallel.For(0, ps.Length, (int k) =>
            {
                //for(int k = 0; k < ps.Length; k++)
                //{
                res[k] = new Vectors(15);
                int a11, a12, a21, a22;

                var range = ps[k].Range;
                for (int i = range[0]; i < range[1]; i++ /*+= 2*/)
                    for (int j = range[2]; j < range[3]; j++ /*+= 2*/)
                    {
                        a11 = (IsBlack(j, i)) ? 1 : 0;
                        a12 = (IsBlack(j + 1, i)) ? 1 : 0;
                        a21 = (IsBlack(j, i + 1)) ? 1 : 0;
                        a22 = (IsBlack(j + 1, i + 1)) ? 1 : 0;
                        if (a11 + a22 + a12 + a21 != 0)
                            res[k][a22 + 2 * a21 + 4 * a12 + 8 * a11 - 1]++;
                    }

                res[k] = res[k].Normalizing;
                //}
            });
            return new Tuple<Vectors[], Vectors[]>(res, GetPercents(ps));
        }

        internal static Vectors[] GetPercents(PointSet[] ps)
        {
            Vectors[] res = new Vectors[ps.Length];

            Program.form.toolStripLabel1.Text = "Вычисляются процентные характеристики";
            Parallel.For(0, ps.Length, (int k) =>
            {

                res[k] = new Vectors(8);
                var range = ps[k].Range;//new Vectors(range).Show();
                int x, y, step = (range[1] - range[0]) / 4;

                for (int i = 0; i < ps[k].Set.Count; i++)
                {
                    x = ps[k].Set[i].x.ToInt();
                    y = ps[k].Set[i].y.ToInt();

                    if (y < (range[2] + range[3]) / 2) y = 0;
                    else y = 4;

                    for (int j = 0; j < 4; j++)
                        if (x < range[0] + (j + 1) * step)
                        {
                            x = j;
                            break;
                        }
                    if (x > 3) x = 3;

                    res[k][x + y]++;
                }

                res[k] /= ps[k].Set.Count;
            });
            return res;
        }

        /// <summary>
        /// Получить вектор яркостей изображения
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="n">На сколько строк делить изображение</param>
        /// <param name="m">На сколько столбцов делить изображение</param>
        /// <returns></returns>
        public static Vectors BitmapToVector(Bitmap bitmap, int n, int m)
        {
            Matrix A = new Matrix(n, m), tmp;
            int x = bitmap.Height / n, y = bitmap.Width / m;

            //for (int i = 0; i < bitmap.Height; i++)
            //    for (int j = 0; j < bitmap.Width; j++)
            //        if(bitmap.GetPixel(j, i).ToArgb()!=-1)
            //        bitmap.GetPixel(j, i).ToArgb().Show();

            for (int ix = 0; ix < n - 1; ix++)
                for (int iy = 0; iy < m - 1; iy++)
                {
                    tmp = new Matrix(x, y);
                    for (int i = 0; i < x; i++)
                        for (int j = 0; j < y; j++)
                            tmp[i, j] = bitmap.GetPixel(iy * y + j, ix * x + i).ToArgb();
                    //tmp.PrintMatrix();
                    A[ix, iy] = tmp.Center;
                }

            for (int iy = 0; iy < m - 1; iy++)
            {
                tmp = new Matrix(bitmap.Height - x * (n - 1), y);
                for (int i = 0; i < bitmap.Height - x * (n - 1); i++)
                    for (int j = 0; j < y; j++)
                        tmp[i, j] = bitmap.GetPixel(iy * y + j, (n - 1) * x + i).ToArgb();
                A[n - 1, iy] = tmp.Center;
            }

            for (int ix = 0; ix < n - 1; ix++)
            {
                tmp = new Matrix(x, bitmap.Width - y * (m - 1));
                for (int i = 0; i < x; i++)
                    for (int j = 0; j < bitmap.Width - y * (m - 1); j++)
                        tmp[i, j] = bitmap.GetPixel((m - 1) * y + j, ix * x + i).ToArgb();
                A[ix, m - 1] = tmp.Center;
            }


            tmp = new Matrix(bitmap.Height - x * (n - 1), bitmap.Width - y * (m - 1));
            for (int i = 0; i < bitmap.Height - x * (n - 1); i++)
                for (int j = 0; j < bitmap.Width - y * (m - 1); j++)
                    tmp[i, j] = bitmap.GetPixel((m - 1) * y + j, (n - 1) * x + i).ToArgb();
            A[n - 1, m - 1] = tmp.Center;

            return Vectors.Create(A, true)/Color.Black.ToArgb();
        }

        public static Bitmap VectorToBitmap(Vectors v, int colcount)
        {
            Matrix A = Matrix.Create(v, colcount)*Color.Black.ToArgb();

            Bitmap r = new Bitmap(A.ColCount, A.RowCount);
            for (int i = 0; i < A.RowCount; i++)
                for (int j = 0; j < A.ColCount; j++)
                    r.SetPixel(j, i, Color.FromArgb((int)A[i, j]));
            return r;
        }

        public static Vectors BitmapToVector(ref Bitmap bitmap, int x0, int X, int y0, int Y, int n, int m)
        {
            Matrix A = new Matrix(n, m), tmp;

            int Width = (Y - y0 + 1);
            int Height = (X - x0+ 1);

            int x = Height / n, y = Width / m;
            int xs = Height - x * n, ys = Width - y * m;

            Func<int, int> xx = (int ind) => (ind < xs) ? x + 1 : x;
            Func<int, int> yy = (int ind) => (ind < ys) ? y + 1 : y;

            Func<int,int> x0x=(int ind)=> x0+x*ind+Math.Min(ind,xs);
            Func<int, int> y0y = (int ind) => y0 +y*ind+ Math.Min(ind, ys);

            for (int ix = 0; ix < n/*n - 1*/; ix++)
                for (int iy = 0; iy <m /*m - 1*/; iy++)
                {
                    tmp = new Matrix(xx(ix), yy(iy));
                    for (int i = 0; i < xx(ix); i++)
                        for (int j = 0; j < yy(iy); j++)
                            tmp[i, j] = bitmap.GetPixel(y0y(iy) + j, x0x(ix) + i).ToArgb();
                    A[ix, iy] = tmp.Center;

                    for (int i = 0; i < xx(ix); i++)
                        for (int j = 0; j < yy(iy); j++)
                            bitmap.SetPixel(y0y(iy) + j, x0x(ix) + i, Color.FromArgb((int)A[ix, iy]));
                }


            return Vectors.Create(A, true) / Color.Black.ToArgb();
        }

        /// <summary>
        /// Возвращает набор векторов из изображения (для нейросети)
        /// </summary>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Vectors[] GetVectors(ref Bitmap b, int n, int m)
        {
            var set = GetPointSets(b);
            Vectors[] res = new Vectors[set.Length];
            //Parallel.For(0, res.Length, (int i) => { 
            for (int i = 0; i < res.Length; i++)
            {
                var c = set[i].Range;
                res[i] = BitmapToVector(ref b, c[0], c[1], c[2], c[3], n, m);
            }
            //});
            return res;
        }
    }

    /// <summary>
    /// Множество точек
    /// </summary>
    public class PointSet : IComparable
    {
        internal List<Point> Set = new List<Point>();
        public Point Center
        {
            get
            {
                double xmin = Set[0].x, xmax = xmin, ymin = Set[0].y, ymax = ymin;

                for (int i = 1; i < Set.Count; i++)
                {
                    if (Set[i].x < xmin)
                        xmin = Set[i].x;
                    if (Set[i].x > xmax)
                        xmax = Set[i].x;
                    if (Set[i].y < ymin)
                        ymin = Set[i].y;
                    if (Set[i].y < ymax)
                        ymax = Set[i].y;
                }

                return new Point((xmax + xmin) / 2, (ymax + ymin) / 2);
            }
        }
        public double Diametr
        {
            get
            {
                var s = this.Range;
                return Math.Sqrt((s[0] - s[1]) * (s[0] - s[1]) + (s[2] - s[3]) * (s[2] - s[3]));
            }
        }


        public PointSet(List<Point> list)
        {
            for (int i = 0; i < list.Count; i++)
                this.Set.Add(list[i].dup);
        }
        public PointSet(PointSet set)
        {
            for (int i = 0; i < set.Set.Count; i++)
                this.Set.Add(set.Set[i].dup);
            //this.Set.Count.Show();
        }
        public PointSet() => Set = new List<Point>();

        /// <summary>
        /// Возвращает вектор (min_x, max_x, min_y, max_y)
        /// </summary>
        public int[] Range
        {
            get
            {
                double xmin = Set[0].x, xmax = xmin, ymin = Set[0].y, ymax = ymin;

                for (int i = 1; i < Set.Count; i++)
                {
                    if (Set[i].x < xmin)
                        xmin = Set[i].x;
                    if (Set[i].x > xmax)
                        xmax = Set[i].x;
                    if (Set[i].y < ymin)
                        ymin = Set[i].y;
                    if (Set[i].y > ymax)
                        ymax = Set[i].y;
                }

                return new int[] { xmin.ToInt(), xmax.ToInt(), ymin.ToInt(), ymax.ToInt() };
            }
        }

        /// <summary>
        /// Вставляет во множество элемент, если его там ещё нет
        /// </summary>
        /// <param name="p"></param>
        public void TryToAdd(Point p)
        {
            if (!Set.Contains(p))
                Set.Add(p);
        }

        public bool Contains(Point p) => Set.Contains(p);

        public int CompareTo(object obj)
        {
            PointSet ps = (PointSet)obj;
            return this.Center.Swap.CompareTo(ps.Center.Swap);
        }

        public static void DrawRect(PointSet[] set, ref Bitmap bitmap)
        {
            for (int i = 0; i < set.Length; i++)
            {
                var r = set[i].Range;

                for (int k = r[0]; k <= r[1]; k++)
                {
                    bitmap.SetPixel(r[2], k, Color.Green);
                    bitmap.SetPixel(r[3], k, Color.Green);
                }
                for (int k = r[2]; k <= r[3]; k++)
                {
                    bitmap.SetPixel(k, r[0], Color.Green);
                    bitmap.SetPixel(k, r[1], Color.Green);
                }

                Graphics g = Graphics.FromImage(bitmap);
                g.DrawString($"{i + 1}", new Font("Arial", 10), Brushes.Red, (r[2] + r[3]) / 2, (r[0] + r[1]) / 2);
            }

            bitmap.Save($"res{++Expend.count}.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static Vectors[] GetCountCurves(PointSet[] set)
        {
            Program.form.toolStripLabel1.Text = "Вычисляется число контуров у цифр";
            Vectors[] res = new Vectors[set.Length];
            Parallel.For(0, res.Length, (int k) =>
            {
                //for(int k = 0; k < res.Length; k++)
                //{
                res[k] = new Vectors(1);
                PointSet tmp = new PointSet(set[k]);
                res[k][0] = tmp.CountCurves()/*+0.01*/;
                // }
            });
            return res;
        }

        public int DistantCoef(Point p)
        {
            if (this.Set.Count <= 1) return -1;
            int res = 0;
            if (Set.IndexOf(p) >= 0)
                while (Set[res] == p) res++;

            double d = Point.Eudistance(Set[res], p);
            for (int i = res + 1; i < Set.Count; i++)
            {
                double d2 = Point.Eudistance(Set[i], p);
                if (d2 != 0 && d2 < d)
                {
                    d = d2;
                    res = i;
                }
            }
            return res;
        }

        private int CountCurves()
        {
            int res = 0;
            Point beg;
            Point tmp;

            while (Set.Count > 1)//если поставить 0, может появиться лишняя кривая из одной точки или около того
            {
                beg = Set[0];
                int d = this.DistantCoef(beg);

                while (d >= 0 && Point.Eudistance(beg, Set[d]) <= 2)
                {
                    tmp = Set[d].dup;
                    Set.Remove(beg);
                    beg = tmp.dup;
                    d = this.DistantCoef(beg);
                }

                Set.Remove(beg);
                res++;
            }

            return res;
        }
    }

    /// <summary>
    /// Класс, обеспечивающий соответствие между bitmap и вектром цифр
    /// </summary>
    public class SymbolSamples
    {
       public Tuple<Vectors[], Vectors[]> vectors { get; private set; }
        List<char> chars;

        public int Count => vectors.Item1.Length;

        /// <summary>
        /// Конструктор по массивам
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="charss"></param>
        public SymbolSamples(Tuple<Vectors[], Vectors[]> vec, char[] charss)
        {
            chars = new List<char>();
            vectors = vec;
            chars.AddRange(charss);
        }

        public SymbolSamples(Vectors[] vec, char[] charss)
        {
            chars = new List<char>();
            vectors = new Tuple<Vectors[], Vectors[]>(vec, vec);
            chars.AddRange(charss);
        }

        /// <summary>
        /// Конструктор по именам файлов
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="c"></param>
        public SymbolSamples(string vec, string per, string c)
        {
            StreamReader vecc = new StreamReader(vec);
            StreamReader percent = new StreamReader(per);
            StreamReader cc = new StreamReader(c);
            chars = new List<char>();

            List<Vectors> vect = new List<Vectors>(), perc = new List<Vectors>();

            string s = vecc.ReadLine();
            while (s != null)
            {
                vect.Add(new Vectors(s));
                perc.Add(new Vectors(percent.ReadLine()));
                chars.Add(Convert.ToChar(cc.ReadLine()));
                s = vecc.ReadLine();
            }

            vectors = new Tuple<Vectors[], Vectors[]>(vect.ToArray(), perc.ToArray());

            vecc.Close();
            percent.Close();
            cc.Close();
        }

        public void ToFiles(string name = "")
        {
            StreamWriter vec = new StreamWriter($"{name} (vec).txt");
            StreamWriter per = new StreamWriter($"{name} (percent).txt");
            StreamWriter c = new StreamWriter($"{name} (char).txt");

            for (int i = 0; i < Count; i++)
            {
                vec.WriteLine(vectors.Item1[i].ToString());
                per.WriteLine(vectors.Item2[i].ToString());
                c.WriteLine(chars[i]);
            }

            vec.Close();
            per.Close();
            c.Close();
        }

        /// <summary>
        /// Сгенерировать объект по изображению (из которого возьмутся векторы) и соответствующему набору символов
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static SymbolSamples Create(Bitmap bitmap, char[] chars) => new SymbolSamples(Expend.GetVectors(bitmap), chars);

        public static SymbolSamples CreateHau(Bitmap bitmap, char[] chars, int n = 40, int len = 6, double eps = 1e-8) => new SymbolSamples(Expend.GetHauVectors(Expend.GetCurvePointSets(bitmap), n, len, eps), chars);

        public Tuple<char, char, char, string[]> GetResult(Vectors v, Vectors p)
        {
            string[] st = new string[Count];
            double[] eps = new double[Count];
            double[] per = new double[Count];
            double[] mult = new double[Count];

            for (int i = 0; i < Count; i++)
            {
                //v[14] = 0;vectors[i][14] = 0;
                eps[i] = (vectors.Item1[i] - v).DistNorm;
                per[i] = (vectors.Item2[i] - p).DistNorm;
                mult[i] = eps[i] * per[i];
                st[i] = $"Разница с эталоном для [{chars[i]}] равна \t{eps[i]}; \t{per[i]}";
            }

            return new Tuple<char, char, char, string[]>(chars[Array.IndexOf(eps, eps.Min())], chars[Array.IndexOf(per, per.Min())], chars[Array.IndexOf(mult, mult.Min())], st);
        }
    }


    /// <summary>
    /// Нейросеть
    /// </summary>
    public class NeuroNet
    {
        /// <summary>
        /// Число выходных нейронов
        /// </summary>
        private static readonly int LastCount = 10;

        /// <summary>
        /// Массив весовых матриц
        /// </summary>
        private Matrix[] W;
        /// <summary>
        /// Массив смещений персепронов
        /// </summary>
        private Vectors[] b;
        /// <summary>
        /// Массив результатов по слоям
        /// </summary>
        private Vectors[] a;
        /// <summary>
        /// Число слоёв (не считая входного)
        /// </summary>
        private int L;

        /// <summary>
        /// Количество переменных нейросети
        /// </summary>
        private int FullDim
        {
            get
            {
                int res = 0;

                for (int i = 0; i < L; i++)
                    res += W[i].RowCount * W[i].ColCount + b[i].Deg;

                return res;
            }
        }
        /// <summary>
        /// Количество чисто весов нейросети
        /// </summary>
        private int HalfFullDim
        {
            get
            {
                int res = 0;

                for (int i = 0; i < L; i++)
                    res += W[i].RowCount * W[i].ColCount;

                return res;
            }
        }


        private Matrix[] dW;
        private Vectors[] da;
        private Vectors[] db;

        /// <summary>
        /// Последний слой сети (результат)
        /// </summary>
        public Vectors aL => a[a.Length - 1];


        /// <summary>
        /// Масштабирующая функция сигмоиды
        /// </summary>
        public static Func<Vectors, Vectors> sigma = (Vectors v) =>
        {
            Contract.Requires(v.IsIn(0, 1));
            Vectors res = new Vectors(v.Deg);
            for (int i = 0; i < res.Deg; i++)
                res[i] = Math.Pow(1.0 + Math.Exp(-v[i]),-1);

            Contract.Ensures(res.IsIn(0, 1));
            return res;
        };
        /// <summary>
        /// Производная сигмоиды
        /// </summary>
        private static Func<Vectors, Vectors> dsigma = (Vectors v) =>
        {
            Contract.Requires(v.IsIn(0, 1));      

            Vectors res = new Vectors(v.Deg);
            for (int i = 0; i < res.Deg; i++)
            {
                double e = Math.Exp(-v[i]);
                res[i] = e / Math.Pow((1.0 + e), 2);
            }

            Contract.Ensures(res.IsIn(0, 1));
            return res;
        };

        /// <summary>
        /// Конструктор по входному массиву и массиву размерностей всех слоёв, кроме последнего (у того LastCount)
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="dims"></param>
        public NeuroNet(Vectors a0, double range = 1.0, double centerW = 0.5, double centerd = -0.5, params int[] dims)
        {
            MakeNeuroNet(a0.Deg, range, centerW, centerd, dims);
            Go(a0);
        }
        /// <summary>
        /// Конструктор по входному массиву и массиву размерностей всех слоёв, кроме последнего (у того LastCount)
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="dims"></param>
        private NeuroNet(Vectors a0, double[] range, double[] centerW, double[] centerd, params int[] dims)
        {
            MakeNeuroNet(a0.Deg, range, centerW, centerd, dims);
            Go(a0);
        }
        /// <summary>
        /// Конструктор по входному массиву и массиву размерностей всех слоёв, кроме последнего (у того LastCount)
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="dims"></param>
        public NeuroNet(Vectors a0,  params int[] dims)
        {
            MakeNeuroNet(a0.Deg, dims);
            Go(a0);
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="net"></param>
        private NeuroNet(NeuroNet net)
        {
            this.L = net.L;
            W = new Matrix[L];
            b = new Vectors[L];
            a = new Vectors[L];

            dW = new Matrix[L];
            da = new Vectors[L];
            db = new Vectors[L];

            for (int i = 0; i < L; i++)
            {
                W[i] = net.W[i].dup;
                b[i] = net.b[i].dup;
                a[i] = net.a[i].dup;
                dW[i] = net.dW[i].dup;
                da[i] = net.da[i].dup;
                db[i] = net.db[i].dup;
            }
        }

        private NeuroNet() { }

        private void Init(int m, params int[] dims)
        {
            L = dims.Length + 1;
            W = new Matrix[L];
            b = new Vectors[L];
            a = new Vectors[L];

            dW = new Matrix[L];
            da = new Vectors[L];
            db = new Vectors[L];

            //Первый слой
            W[0] = new Matrix(dims[0], m);
            dW[0] = new Matrix(dims[0], m);
            b[0] = new Vectors(dims[0]);
            a[0] = new Vectors(dims[0]);
            da[0] = new Vectors(dims[0]);
            db[0] = new Vectors(dims[0]);
            //Следующие слои
            for (int i = 1; i < dims.Length; i++)
            {
                W[i] = new Matrix(dims[i], dims[i - 1]);
                dW[i] = new Matrix(dims[i], dims[i - 1]);
                b[i] = new Vectors(dims[i]);
                a[i] = new Vectors(dims[i]);
                da[i] = new Vectors(dims[i]);
                db[i] = new Vectors(dims[i]);
            }
            //Последний слой
            W[L - 1] = new Matrix(LastCount, dims[L - 2]);
            dW[L - 1] = new Matrix(LastCount, dims[L - 2]);
            b[L - 1] = new Vectors(LastCount);
            a[L - 1] = new Vectors(LastCount);
            da[L - 1] = new Vectors(LastCount);
            db[L - 1] = new Vectors(LastCount);
        }

        /// <summary>
        /// Сгенерировать нейросеть по размерности входного вектора и размерности слоёв (за исключением последнего слоя, чья размерность LastCount)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="dims"></param>
        private void MakeNeuroNet(int m, double range = 1.0, double centerW = 0.5, double centerd = -0.5, params int[] dims)
        {
            Init(m, dims);

            //Заполнить случайными числами
            Rand(range, centerW, centerd);
        }
        /// <summary>
        /// Сгенерировать нейросеть по размерности входного вектора и размерности слоёв (за исключением последнего слоя, чья размерность LastCount)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="dims"></param>
        private void MakeNeuroNet(int m, double[] range, double[] centerW, double[] centerd, params int[] dims)
        {
            Init(m, dims);

            //Заполнить случайными числами
            Rand(range, centerW, centerd);
        }
        /// <summary>
        /// Сгенерировать нейросеть по размерности входного вектора и размерности слоёв (за исключением последнего слоя, чья размерность LastCount)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="dims"></param>
        private void MakeNeuroNet(int m,  params int[] dims)
        {
            Init(m, dims);

            //Заполнить случайными числами
            RandBase();
        }

        /// <summary>
        /// Заполнить веса и сдвиги случайными значениями, специально подобранными для начальной реализации сети
        /// </summary>
        /// <param name="s"></param>
        private void RandBase()
        {
            var r = new MathNet.Numerics.Random.CryptoRandomSource();
            double range = 0.1;
            for (int k = 0; k < L; k++)
            {
                range = 2 * Math.Sqrt(6) / Math.Sqrt(W[k].ColCount + W[k].RowCount);

                for (int i = 0; i < b[k].Deg; i++)
                {
                    for (int j = 0; j < W[k].ColCount; j++)
                        W[k][i, j] = r.NextDouble() * range - range / 2;
                    b[k][i] = -1;
                }
            }

        }
        /// <summary>
        /// Заполнить веса и сдвиги случайными значениями
        /// </summary>
        /// <param name="s"></param>
        private void Rand(double range = 1.0, double centerW = 0.5, double centerd = -0.5)
        {
            var r = new MathNet.Numerics.Random.CryptoRandomSource();
            for (int k = 0; k < L; k++)
                for (int i = 0; i < b[k].Deg; i++)
                {
                    for (int j = 0; j < W[k].ColCount; j++)
                        W[k][i, j] = r.NextDouble() * range - range / 2 + centerW;
                    b[k][i] = r.NextDouble() * range - range / 2 + centerd;
                }
        }
        /// <summary>
        /// Заполнить веса и сдвиги случайными значениями
        /// </summary>
        /// <param name="s"></param>
        private void Rand(double[] range, double[] centerW, double[] centerd)
        {
            Random r = new Random();
            for (int k = 0; k < L; k++)
                for (int i = 0; i < b[k].Deg; i++)
                {
                    for (int j = 0; j < W[k].ColCount; j++)
                        W[k][i, j] = r.NextDouble() * range[k] - range[k] / 2 + centerW[k];
                    b[k][i] = r.NextDouble() * range[k] - range[k] / 2 + centerd[k];
                }
        }


        /// <summary>
        /// Пройтись по сети
        /// </summary>
        private void Go(Vectors a0)
        {
            a[0] = sigma(Matrix.FastMult(W[0], a0) + b[0]);
            //if (Double.IsNaN(b[0][0])) throw new Exception("6");
            for (int i = 1; i < L; i++)
                a[i] = sigma(Matrix.FastMult(W[i], a[i - 1]) + b[i]);
        }
        //private void Go(Vectors a0)
        //{
        //    a[0] = sigma(Matrix.FastMult(W[0], a0) + b[0]);
        //    for (int i = 1; i < L-1; i++)
        //        a[i] = sigma(Matrix.FastMult(W[i], a[i - 1]) + b[i]);
        //    a[L-1] = Matrix.FastMult(W[L-1], a[L-2]) + b[L-1];
        //}

        /// <summary>
        /// Изменить текущий объект, переопределив ссылки другого
        /// </summary>
        /// <param name="net"></param>
        private void SetThis(NeuroNet net)
        {
            this.a = net.a;
            this.b = net.b;
            this.W = net.W;
            this.da = net.da;
            this.dW = net.dW;
            this.L = net.L;
        }

        /// <summary>
        /// Сгенерировать векторы и матрицы градиента
        /// </summary>
        /// <param name="a0">Входные данные (нужны для генерации градиентов на первом слое, так как обычно считаются первым слоем нейросети)</param>
        private void GenerateGrad(Vectors a0, Vectors y)
        {
            GenerateDA(a0, y);

            //матрицы градиента

            for (int i = L - 1; i >= 1; i--)
            {
                db[i] = Vectors.CompMult(dsigma(a[i]), da[i]);
                dW[i] = Matrix.Create(a[i - 1], db[i]);
            }

            db[0] = Vectors.CompMult(dsigma(a[0]), da[0]);
            dW[0] = Matrix.Create(a0, db[0]);
        }

        private void GenerateDA(Vectors a0, Vectors y)
        {
            //вспомогательные градиенты
            da[L - 1] = 2 * (a[L - 1] - y);
            for (int i = L - 2; i >= 0; i--)
                da[i] = Vectors.CompMult(dsigma(a[i + 1]), da[i + 1]) * W[i + 1];
        }

        /// <summary>
        /// Градиенты для множества примеров
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="y"></param>
        private void GenerateGrad(Vectors[] a0,Vectors[] y)
        {
            GenerateGrad(a0[0], y[0]);
            Matrix[] ddW=dW.dup();
            Vectors[] ddb=db.dup();
            for(int i = 1; i < a0.Length; i++)
            {
                GenerateGrad(a0[i], y[i]);
                Parallel.For(0, L, (int k) => {
                    ddW[k] += dW[k];
                    ddb[k] += db[k];
                });
            }
            Parallel.For(0, L, (int k) => {
                ddW[k] /= a0.Length;
                ddb[k] /= a0.Length;
            });

            this.dW = ddW.dup();
            this.db = ddb.dup();
        }


        /// <summary>
        /// Норма текущего градиента (как сумма норм градиентных матриц)
        /// </summary>
        private double GradientNorm
        {
            get
            {
                double sum = dW[0].CubeNorm;
                for (int i = 1; i < dW.Length; i++)
                    sum += dW[0].CubeNorm;
                return sum;
            }
        }

        /// <summary>
        /// Обучить сеть
        /// </summary>
        /// <param name="a0">Входные данные</param>
        /// <param name="y">Идеальный выход</param>
        /// <param name="nu">Начальный шаг градиента</param>
        /// <param name="eps">Предельная погрешность</param>
        private void Teach(Vectors a0, Vectors y, double nu = 0.5, double eps = 1e-10, int maxcount = 1000, double alpha = 0.8)
        {
            Func<double> norm = () => (aL - y).DistNorm;

            double e = norm(), tmp, gr = e;
            int k = 0, count = maxcount;
            Matrix[] Wn, Wnm = GetW();
            Vectors[] bn, bnm = Getb();

            while (e > eps && gr > eps && maxcount > 0)
            {
                Wn = GetW(); bn = Getb();
                GenerateGrad(a0, y);
                Slope(nu);
                AddW(alpha, Wn, Wnm, bn, bnm);

                Go(a0);
                tmp = norm();
                gr = GradientNorm;
                if (tmp >= e)
                {
                    Slope(-nu);
                    AddW(-alpha, Wn, Wnm, bn, bnm);

                    Go(a0);
                    nu /= 2;
                    k = 0;
                }
                else
                {
                    e = tmp;
                    k++;
                    System.Diagnostics.Debug.WriteLine($"eps = {e} \tgrad = {gr} \tnu = {nu}");
                    maxcount = count;
                }

                if (k > 3) nu = nu * 2;
                //if (nu < 1e-11) nu = 100;

                Wnm = Wn.dup();
                bnm = bn.dup();
                maxcount--;
            }
        }

        /// <summary>
        /// Обучить сеть
        /// </summary>
        /// <param name="a0">Входные данные</param>
        /// <param name="y">Идеальный выход</param>
        /// <param name="nu">Начальный шаг градиента</param>
        /// <param name="eps">Предельная погрешность</param>
        private void Teach(Vectors[] a0, Vectors[] y, double nu = 10, double eps = 1e-10, double alpha = 0.7, int maxcount = 100, int maxiter = 50)
        {
            //Debug.WriteLine($"Вход на градиент");

            double e = GlobalNorm(a0,y), tmp, gr = e, basenu = nu;
            double ran = 10;

            int k = 0, count = maxcount;
            Matrix[] Wn, Wnm = GetW();
            Vectors[] bn, bnm = Getb();
            int i = 0;

            //Vectors nun;

            while (e > eps && maxiter > 0)
            {
                while (e > eps && maxcount > 0 && ran >= 1)
                {
                    //Wn = GetW();
                    //bn = Getb();
                    Wn = dW.dup();
                    bn = db.dup();

                    GenerateGrad(a0[i], y[i]);//nun = GetNU();
                    AddW(alpha, Wn, bn);
                    Slope(nu);
                    //AddW(alpha, Wn, Wnm,bn,bnm);

                    tmp = GlobalNorm(a0, y);
                    if (tmp >= e)
                    {
                        Slope(-nu);
                        //AddW(-alpha, Wn, Wnm, bn, bnm);

                        nu /= 2;
                        k = 0;
                        maxcount--;
                    }
                    else
                    {
                        ran = Math.Abs(e - tmp) / tmp * 100;
                        e = tmp;
                        k++;
                        System.Diagnostics.Debug.WriteLine($"\tLocal gradient: (by sample {i+1}) eps = {e} \tnu = {nu}");
                        maxcount = count;
                        nu = 10;
                    }

                    if (k > 3) nu *= 2 /*+ (k - 3) * 100*/;
                    if (nu < 1e-8) break;//nu = 100000;

                    //Wnm = Wn.dup();
                    //bnm = bn.dup();
                }

                i++;
                if (i == a0.Length)
                {
                    i = 0;
                    maxiter--;
                }
                maxcount = count;
                nu = basenu;
                k = 0;
                ran = 1.0;
            }
        }

        /// <summary>
        /// Обучить сеть
        /// </summary>
        /// <param name="a0">Входные данные</param>
        /// <param name="y">Идеальный выход</param>
        /// <param name="nu">Начальный шаг градиента</param>
        /// <param name="eps">Предельная погрешность</param>
        private void TeachMany(Vectors[] a0, Vectors[] y, double nu = 10, double eps = 1e-10, double alpha = 0.7, int maxcount = 500, int maxiter = 100)
        {
           // Debug.WriteLine($"Вход на градиент");

            double e = GlobalNorm(a0,y), tmp, basenu = nu;
            double ran = 10;

            int k = 0, count = maxcount;
            Matrix[] Wn, Wnm = GetW();
            Vectors[] bn, bnm = Getb();

            while (e > eps && maxiter > 0)
            {
                while (e > eps && maxcount > 0 && ran >= 0.1)
                {
                    Wn = GetW();
                    bn = Getb();
                   // Wn = dW.dup();
                    //bn = db.dup();

                    GenerateGrad(a0, y);

                    //Vectors nun = GetNU(nu,a0,y);

                    Slope(nu);
                    AddW(alpha, Wn, Wnm, bn, bnm);

                    //MomentDown(nu, alpha, Wn, bn);

                    tmp = GlobalNorm(a0, y);
                    if (tmp >= e)
                    {
                        Slope(-nu);
                        AddW(-alpha, Wn, Wnm, bn, bnm);

                        //Slope(1);

                        nu /= 2;
                        k = 0;
                        maxcount--;
                    }
                    else
                    {
                        ran = Math.Abs(e - tmp) / tmp * 100;
                        e = tmp;
                        k++;
                        System.Diagnostics.Debug.WriteLine($"\tGlobal gradient: eps = {e} \tnu = {nu}");
                        maxcount = count;
                       // nu = 10;
                    }

                    if (k > 3) nu *= 2+k;

                    //if (nu < 1e-6) nu = 10000;

                    Wnm = Wn.dup();
                    bnm = bn.dup();
                    //Wn = dW.dup();
                    //bn = db.dup();
                }

                ran = 1;
                maxiter--;
               // nu = 100000;
            }
        }

        /// <summary>
        /// Градиентный спуск с коэффициентом (градиент считается как w(k+1)=wk-nu*delta)
        /// </summary>
        /// <param name="nu"></param>
        private void Slope(double nu)
        {
            Parallel.For(0, L, (int i) =>
            {
                W[i] -= dW[i] * nu;
                b[i]+= db[i] * nu;
            });
        }
        /// <summary>
        /// Градиентный спуск с коэффициентами на каждый слой (градиент считается как w(k+1)=wk-nu*delta)
        /// </summary>
        /// <param name="nu"></param>
        private void Slope(Vectors nu)
        {
            Parallel.For(0, L, (int i) =>
            {
                W[i] -= dW[i] * nu[i];
                b[i] += db[i] * nu[i];
            });
        }

        /// <summary>
        /// Получить массив адаптивных шагов
        /// </summary>
        /// <returns></returns>
        private Vectors GetNU(double nu, Vectors[] a0, Vectors[] y)
        {
            Vectors[] dda = new Vectors[L];
            for (int i = 0; i < L; i++)
                dda[i] = new Vectors(a[i].Deg);

            for (int i = 0; i < a0.Length; i++)
            {
                Go(a0[i]);
                GenerateDA(a0[i], y[i]);

                Parallel.For(0, L, (int j) => {
                    dda[j] += da[j];
                });
            }


            Vectors res = new Vectors(L);
            Parallel.For(0, L, (int i) =>
            {
                Vectors aj = Vectors.CompMult(a[i], 1 - a[i]);
                Vectors gj = Vectors.CompMult(dda[i], dda[i]);
                res[i] = 4 * Vectors.CompMult(aj,gj).Sum / ((1 + a[i].DistNorm) *  Vectors.CompMult(aj, dda[i]).DistNorm);
                if (Double.IsNaN(res[i]) || Double.IsInfinity(res[i])) res[i] = nu;
            });
           // res.Show();
            return res;
        }

        /// <summary>
        /// Результат работы нейросети
        /// </summary>
        /// <param name="a0"></param>
        /// <returns></returns>
        public Vectors Result(Vectors a0)
        {
            Go(a0);
            return aL.dup;
        }

        /// <summary>
        /// Возвращает индекс максимального результата на выходе
        /// </summary>
        /// <param name="a0"></param>
        /// <returns></returns>
        public int StatRes(Vectors a0)
        {
            var res = Result(a0);
            return Array.IndexOf(res.DoubleMas, res.Max);
        }

        /// <summary>
        /// Окончательный результат для нейросети
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Tuple< string,string> EndRes(Vectors a)
        {
            string s = Result(a).Norming.ToString();

            int ind = StatRes(a);
            if (ind == 9) return new Tuple<string, string>( "0".ToString(),s);
            return new Tuple<string, string>( $"{ind + 1}",s);
        }

        public static int StatRes(NeuroNet[] net, Vectors a0, double p = 0.9)
        {
            for (int i = 0; i < net.Length - 1; i++)
            {
                var res = net[i].Result(a0);
                if (res.Max >= p)
                    return Array.IndexOf(res.DoubleMas, res.Max);
            }
            return net[net.Length - 1].StatRes(a0);
        }

        /// <summary>
        /// Усреднение массива нейросетей
        /// </summary>
        /// <param name="mas"></param>
        /// <returns></returns>
        public static NeuroNet Averaging(NeuroNet[] mas)
        {
            NeuroNet r = new NeuroNet(mas[0]);
            for (int i = 1; i < mas.Length; i++)
            {
                Parallel.For(0, r.L, (int k) =>
                {
                    r.W[k] += mas[i].W[k];
                    r.b[k] += mas[i].b[k];
                });
            }

            Parallel.For(0, r.L, (int k) =>
            {
                r.W[k] /= mas.Length;
                r.b[k] /= mas.Length;
            });

            return r;
        }

        /// <summary>
        /// Получить градиентные матрицы
        /// </summary>
        /// <returns></returns>
        public Matrix[] GetGrad()
        {
            Matrix[] res = new Matrix[L];
            for (int i = 0; i < L; i++)
                res[i] = dW[i].dup;
            return res;
        }
        /// <summary>
        /// Получить весовые матрицы
        /// </summary>
        /// <returns></returns>
        public Matrix[] GetW()
        {
            Matrix[] res = new Matrix[L];
            for (int i = 0; i < L; i++)
                res[i] = W[i].dup;
            return res;
        }
        /// <summary>
        /// Получить весовые матрицы
        /// </summary>
        /// <returns></returns>
        public Vectors[] Getb()
        {
            Vectors[] res = new Vectors[L];
            for (int i = 0; i < L; i++)
                res[i] = b[i].dup;
            return res;
        }

        public void Show()
        {
            for (int i = 0; i < L; i++)
            {
                $"-------Слой {i + 1}".Show();
                $"--------W[{i + 1}]".Show();
                W[i].PrintMatrix();
                $"--------b[{i + 1}]".Show();
                b[i].Show();
            }

            $"-------------Нейросеть---------".Show();
            ShowMe();
        }

        public void ShowMe()
        {
            for (int i = 0; i < L; i++)
            {
                $"Слой {i+1}: {a[i]}".Show();
            }
        }


        public static NeuroNet RandomSearch(Vectors[] a0, Vectors[] y, double eps = 1e-10, double begrange = 1000, int count = 1000, params int[] dims)
        {
            double b = begrange,q;
            int c = count;

            NeuroNet net = new NeuroNet(a0[0], dims), tmp = new NeuroNet(net),delt=new NeuroNet(net);

            Debug.WriteLine($"Погрешность в начале: {net.GlobalNorm(a0,y)}");

            //Func<Vectors, double> f = (Vectors v) =>
            //  {
            //      net.VectorToWeigths(v);
            //      return net.GlobalNorm(a0, y);
            //  };
            //var tup = BeeHiveAlgorithm.GetGlobalMin(f, net.FullDim, -begrange, begrange, eps: eps, countpoints: count, maxcountstep: 20);
            //net.VectorToWeigths(tup.Item1);
            //net.TeachMany(a0, y, 0.9, eps);
            var tup = GetGlobalMin(a0, y, net, -begrange, begrange, eps: eps, countpoints: count, maxcountstep: 20);
            net.VectorToWeigths(tup.Item1);

            //double e = net.GlobalNorm(a0, y);
            //while (e > eps && begrange > eps/*begrange<Double.MaxValue*/)
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        delt.Rand(begrange, 0, 0);
            //        NeuroNet s = tmp + delt; //s.Teach(a0, y, eps: eps);
            //        q = net.GlobalNorm(a0, y);
            //        if (q < e)
            //        {
            //            Debug.WriteLine($"New result (random): {e} --> {q}");
            //            e = q;
            //            net = s;

            //            //net.Teach(a0, y, eps: eps);
            //            //q = net.GlobalNorm(a0,y);
            //            //if (q < e)
            //            //{
            //            //    Debug.WriteLine($"New result (global gradient): {e} --> {q}");
            //            //    e = q;
            //            //}
            //            //else
            //            //for (int j = 0; j < a0.Length; j++)
            //            //{
            //            //    NeuroNet tet = new NeuroNet(net);
            //            //    tet.Teach(a0[j], y[j]);
            //            //    q = net.GlobalNorm(a0, y);
            //            //    if (q < e)
            //            //    {
            //            //        Debug.WriteLine($"New result (local gradient): {e} --> {q}");
            //            //        e = q;
            //            //        net = tet;
            //            //    }
            //            //}


            //            begrange = b;
            //            count = c;

            //            // break;
            //        }
            //    }
            //    net.TeachMany(a0, y, eps: eps);
            //    q = net.GlobalNorm(a0, y);
            //    if (q < e)
            //    {
            //        Debug.WriteLine($"New result (global gradient): {e} --> {q}");
            //        e = q;
            //    }

            //    Debug.WriteLine($"New Loop. Range = {begrange}  eps = {e}");
            //    tmp = new NeuroNet(net);
            //    begrange /= 2;
            //    //begrange *= 2;

            //    count = (int)(count / 1.6);
            //}

            //net.TeachMany(a0, y, 0.5, eps);
            return net;
        }

        public static NeuroNet TeachTeach(Vectors[] a0, Vectors[] y, double eps = 1e-10, double begrange = 1000, int count = 1000, params int[] dims)
        {

            Vectors[] a0s, ys;

            NeuroNet net = new NeuroNet(a0[0], dims);

            Debug.WriteLine($"Погрешность в начале: {net.GlobalNorm(a0, y)}");

            for(int i = 0; i < a0.Length; i++)
            {
                Debug.WriteLine($"-------------------------Минимизация по {i+1} примерам");
                a0s = a0.Slice(0, i+1);
                ys = y.Slice(0,i+1);
                var tup = GetGlobalMin(a0s, ys, net, -begrange, begrange, eps: eps, countpoints: count, maxcountstep: 20);
                net.VectorToWeigths(tup.Item1);
                Debug.WriteLine($"------Новая погрешность: {net.GlobalNorm(a0, y)}");
            }
            return net;
        }

        public static NeuroNet operator +(NeuroNet a, NeuroNet b)
        {
            NeuroNet res = new NeuroNet(a);
            for (int i = 0; i < a.L; i++)
            {
                res.W[i] += b.W[i];
                res.b[i] += b.b[i];
            }
            return res;
        }

        /// <summary>
        /// Добавить момент к весам
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="Wn"></param>
        /// <param name="Wnm"></param>
        public void AddW(double alpha, Matrix[] Wn, Matrix[] Wnm,Vectors[] bn, Vectors[] bnm)
        {
            Parallel.For(0, L, (int i) => { 
            //for (int i = 0; i < L; i++)
            //{
                  W[i] += (Wn[i] - Wnm[i]) * alpha;
                b[i] += (bn[i] - bnm[i]) * alpha;
            //}
                });
        }
        /// <summary>
        /// Добавить момент к весам
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="Wn"></param>
        /// <param name="Wnm"></param>
        public void AddW(double alpha, Matrix[] Wn, Vectors[] bn)
        {
            Parallel.For(0, L, (int i) => {
                //for (int i = 0; i < L; i++)
                //{
                dW[i] -= (Wn[i]) * alpha;
                db[i] -= (bn[i]) * alpha;
                //}
            });
        }
        /// <summary>
        /// Добавить момент к весам
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="Wn"></param>
        /// <param name="Wnm"></param>
        public void MomentDown(double nu,double alpha, Matrix[] Wn, Vectors[] bn)
        {
            Parallel.For(0, L, (int i) => {
                //for (int i = 0; i < L; i++)
                //{
                dW[i] =-nu*dW[i]+ (Wn[i]) * alpha;
                db[i] =-nu*db[i]+ (bn[i]) * alpha;
                //}
            });
            Slope(-1);
        }

        /// <summary>
        /// Возвращает Якобиан нейросети
        /// </summary>
        private Matrix J
        {
            get
            {
                List<double> vec=new List<double>();

                Vectors[] m = new Vectors[L];
                for (int i = 0; i < L; i++)
                {
                    m[i] = Vectors.Create(dW[i]); //Debug.WriteLine($"{m[i].Deg} == {dW[i].ColCount * dW[i].RowCount}");
                    vec.AddRange(m[i].DoubleMas);
                }
                for (int i = 0; i < L; i++)
                    vec.AddRange(db[i].DoubleMas);

                return Matrix.Create(new Vectors(vec.ToArray()), 1/*0*/, false);
                    
            }
        }
        /// <summary>
        /// Приближённый Гессиан
        /// </summary>
        private Matrix H
        {
            get
            {
                Matrix j = J;
                return j.Transpose() * j;
            }
        }
        /// <summary>
        /// Вектор невязок нейросети
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private Vectors e(Vectors y) => (aL - y).AbsVector;

        /// <summary>
        /// Приближённый специальный Гессиан для метода Левенберга-Маркварта
        /// </summary>
        /// <param name="mu"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Vectors Leven(double mu,Vectors y)
        {
            Matrix j = J;
            Matrix jt = j.Transpose();
            SqMatrix A = (new SqMatrix(jt * j) + mu * SqMatrix.E(j.ColCount));

            return  A.Invert()* jt * new Vectors( e(y).DistNorm);
        }
        /// <summary>
        /// Приближённый специальный Гессиан для метода Левенберга-Маркварта
        /// </summary>
        /// <param name="mu"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Vectors Leven(double mu,Vectors[]a, Vectors[] y)
        {
            Matrix j = J;
            Matrix jt = j.Transpose();
            SqMatrix A = (new SqMatrix(jt * j) + mu * SqMatrix.E(j.ColCount));

            return A.Invert() * jt * new Vectors(GlobalNorm(a,y));
        }

        /// <summary>
        /// Произвести спуск методом Левенберга-Маркварта
        /// </summary>
        /// <param name="leven"></param>
        private void LevenSlope(Vectors leven)
        {
            int k = 0;
            for (int i = 0; i < L; i++)
                for (int n = 0; n < W[i].RowCount; n++)
                    for (int m = 0; m < W[i].ColCount; m++)
                        W[i][n, m] -= leven[k++];

            for (int i = 0; i < L; i++)
                for (int n = 0; n < b[i].Deg; n++)
                    b[i][n] -= leven[k++];
        }

        /// <summary>
        /// Перевести вектор в веса
        /// </summary>
        /// <param name="leven"></param>
        private void VectorToWeigths(Vectors leven)
        {
            int k = 0;
            for (int i = 0; i < L; i++)
                for (int n = 0; n < W[i].RowCount; n++)
                    for (int m = 0; m < W[i].ColCount; m++)
                        W[i][n, m] = leven[k++];

            for (int i = 0; i < L; i++)
                for (int n = 0; n < b[i].Deg; n++)
                    b[i][n] = leven[k++];
        }
        /// <summary>
        /// Перевести веса нейросети в вектор
        /// </summary>
        /// <returns></returns>
        private Vectors WeightsToVector()
        {
            Vectors leven = new Vectors(this.FullDim);
            int k = 0;
            for (int i = 0; i < L; i++)
                for (int n = 0; n < W[i].RowCount; n++)
                    for (int m = 0; m < W[i].ColCount; m++)
                          leven[k++]=W[i][n, m];

            for (int i = 0; i < L; i++)
                for (int n = 0; n < b[i].Deg; n++)
                  leven[k++] = b[i][n]  ;
            return leven;
        }

        public void TeachLeven(Vectors a,Vectors y, double mu = 0.5, double eps = 1e-10, int maxcount = 1000)
        {
            Go(a);
            double e = this.e(y).DistNorm,tmp;
            int c = maxcount;
            Vectors lev;

            while (e > eps && maxcount>0)
            {
                Go(a);
                GenerateGrad(a, y);               
                lev = Leven(mu, y);

                LevenSlope(lev);
                Go(a);
                tmp = this.e(y).DistNorm;

                if (tmp >= e)
                {
                    LevenSlope(-lev);
                    mu /= 2;
                    maxcount--;
                }
                else
                {
                    Debug.WriteLine($"Levenberg: {e} --> {tmp}");
                    e = tmp;
                    maxcount = c;
                }
            }
        }
        public void TeachLeven(Vectors[] a, Vectors[] y, double mu = 0.5, double eps = 1e-10, int maxcount = 1000)
        {

            double e = GlobalNorm(a,y), tmp,ran=1.0;
            int c = maxcount,k=0;
            Vectors lev;

            while (e > eps && maxcount > 0 && ran>=1.0)
            {
                k++;
                GenerateGrad(a, y);
                lev = Leven(mu, a,y);

                LevenSlope(lev);
                tmp = GlobalNorm(a, y);

                if (tmp >= e)
                {
                    LevenSlope(-lev);
                    mu /= 2;
                    maxcount--;
                }
                else
                {
                    ran = Math.Abs(e - tmp) / tmp * 100;
                    Debug.WriteLine($"\tLevenberg (iter {k}): {e} --> {tmp}");
                    e = tmp;
                    maxcount = c;
                }
            }
        }

        public double GlobalNorm(Vectors[] a0,Vectors[] y)
        {
            double sum = (Result(a0[0]) - y[0]).DistNorm;
            for (int t = 1; t < a0.Length; t++)
            {
                sum += (Result(a0[t]) - y[t]).DistNorm;
            }
            return sum;
        }

        #region Метод сопряжённых градиентов

        /// <summary>
        /// Градиент функции как вектор
        /// </summary>
        public Vectors GradVector
        {
            get
            {
                Vectors leven = new Vectors(this.FullDim);
                int k = 0;
                for (int i = 0; i < L; i++)
                    for (int n = 0; n < W[i].RowCount; n++)
                        for (int m = 0; m < W[i].ColCount; m++)
                            leven[k++] = dW[i][n, m];

                for (int i = 0; i < L; i++)
                    for (int n = 0; n < b[i].Deg; n++)
                        leven[k++] = db[i][n];
                return leven;
            }
        }

        /// <summary>
        /// Метод сопряжённых градиентов
        /// </summary>
        /// <param name="a0">Входы</param>
        /// <param name="y">Выходы</param>
        /// <param name="eps">Погрешность</param>
        /// <param name="maxcount">Максимальное число итераций</param>
        /// <param name="n">Параметр забывания</param>
        public void TeachConjGrad(Vectors[] a0,Vectors[] y, double eps=1e-7,int maxcount = 1000,int n=10)
        {
            Func<Vectors, double> f = (Vectors v) =>
            {
                this.VectorToWeigths(v);
                return this.GlobalNorm(a0, y);
            };

            GenerateGrad(a0, y);

            Vectors x = this.WeightsToVector();
            Vectors r0,r=-GradVector;
            Vectors d = r.dup;
            double a, s;
            int k = 0;

            while(GlobalNorm(a0,y)>eps && maxcount > 0)
            {
                k++;
                r0 = r.dup;
                a = -(GradVector * d) / (d * (H * d));
                x += a * d;

                this.VectorToWeigths(x);
                GenerateGrad(a0, y);
                r = -GradVector;

                s = r * r / (r0 * r0);
                if (k == n + 1) { k = 0; s = 0; }

                //s = r * (r - r0) / (r0 * r0);
                //s = Math.Max(s, 0);

                d = r + s * d;
            
                maxcount--;
                
                Debug.WriteLine($"Conjugate grad: {GlobalNorm(a0, y)}");
            }

        }

        #endregion


        /// <summary>
        /// Получить минимум функции, посчитанный роевым методом
        /// </summary>
        /// <param name="f">Целевая функция</param>
        /// <param name="n">Размерность области определения целевой функции</param>
        /// <param name="min">Минимальное возможное значение каждого аргумента</param>
        /// <param name="max">Максимальное возможное значение каждого аргумента</param>
        /// <param name="eps">Допустимая погрешность</param>
        /// <param name="countpoints">Количество пчёл в рое</param>
        /// <param name="maxcountstep">Максимальное число итераций метода</param>
        /// <returns></returns>
        public static Tuple<Vectors, double> GetGlobalMin(Vectors[] a, Vectors[] y,NeuroNet net, double min = -1e12, double max = 1e12, double eps = 1e-10, int countpoints = 1000, int maxcountstep = 100)
        {
            Vectors beg = net.WeightsToVector();
            Vectors minimum = beg+min;
            Vectors maximum = beg+max;
            Func<Vectors, double> f = (Vectors v) =>
            {
                net.VectorToWeigths(v);
                return net.GlobalNorm(a, y);
            };

            Vectors vec = BeeHiveAlgorithm.GetGlobalMin(f, minimum, maximum, net.HalfFullDim, countpoints, 400, 100, 50, 150, 3, eps, 10).Item1;
            net.VectorToWeigths(vec);

             beg = vec;
             minimum = beg + min;
             maximum = beg + max;

            //net.TeachConjGrad(a, y, eps);
            //net.TeachLeven(a, y, 0.5, eps);

            net.TeachMany(a, y, 10, eps);
            Debug.WriteLine($"Погрешность после первого спуска:  {net.GlobalNorm(a,y)}");

            Hive hive = new Hive(minimum, maximum, f, countpoints,net.WeightsToVector());
            double e = hive.val;
            int c = maxcountstep, k = 0;

            Debug.WriteLine($"Погрешность после инициализации пчёл:  {e}"); 
            while (e > eps && maxcountstep > 0)
            {
                hive.MakeStep();
                if (hive.val < e)
                {
                    Debug.WriteLine($"Hive method (iter {++k}):  {e} ---> {hive.val}");
                    e = hive.val;
                    maxcountstep = c;

                    //net.TeachLeven(a, y, 0.5, eps);

                    net.VectorToWeigths(hive.g);
                    net.TeachMany(a, y, 100, eps);
                    net.Teach(a, y, 10, eps,maxiter:20);
                    e = net.GlobalNorm(a, y);
                    hive.UpdateG(net.WeightsToVector());
                }
                else
                    maxcountstep--;
                //Debug.WriteLine( $"c = {maxcountstep}  val = {hive.val}");
                if (k == 30) break;
            }

            //net.VectorToWeigths(hive.g);
            //net.TeachMany(a, y, 100, eps);
            //e = net.GlobalNorm(a, y);
            //hive.UpdateG(net.WeightsToVector());

            //net.TeachLeven(a, y, 0.5, eps);

            return new Tuple<Vectors, double>(hive.g, hive.val);
        }

        /// <summary>
        /// Записать нейросеть в файл
        /// </summary>
        /// <param name="filename"></param>
        public void ToFile(string filename)
        {
            using(StreamWriter fs=new StreamWriter(filename))
            {
                for (int i = 0; i < L; i++)
                    fs.Write(W[i].ColCount+" ");
                fs.WriteLine(LastCount);

                for(int k=0;k<L;k++)
                {
                    for (int i = 0; i < W[k].RowCount; i++)
                        fs.WriteLine(W[k].GetLineString(i));
                    fs.WriteLine(b[k].ToString());
                }
            }
        }

        /// <summary>
        /// Прочесть нейросеть из файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static NeuroNet FromFile(string filename)
        {
            NeuroNet r=new NeuroNet();

            using(StreamReader f=new StreamReader(filename))
            {
                string[] st = f.ReadLine().Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries);
                int[] mas = new int[st.Length - 2];
                for (int i = 0; i < mas.Length; i++)
                    mas[i] = st[i + 1].ToInt32();
                r.Init(st[0].ToInt32(), mas);

                //Vectors v = Vectors.Union(new Vectors(st[0].ToDouble()), new Vectors(mas));
                for(int k = 0; k < mas.Length; k++)
                {
                    for(int i = 0; i < mas[k]; i++)
                    {
                        double[] line = f.ReadLine().ToDoubleMas();
                        for (int j = 0; j < line.Length; j++)
                            r.W[k][i, j] = line[j];
                    }
                    r.b[k] = new Vectors(f.ReadLine());
                }
                for (int i = 0; i < LastCount; i++)
                {
                    double[] line = f.ReadLine().ToDoubleMas();
                    for (int j = 0; j < line.Length; j++)
                        r.W[mas.Length][i, j] = line[j];
                }
                r.b[mas.Length] = new Vectors(f.ReadLine());

            }

            return r;
        }

        public static void Test1vec(Vectors a0, Vectors y, double nu = 0.5, double eps = 1e-10, params int[] dims)
        {
            NeuroNet net = new NeuroNet(a0, 1, 0.5, -0.5, dims);
            //net.Show();
            net.Teach(a0, y, nu, eps);

            $"Результат и эталон".Show();
            net.Result(a0).Show();
            y.Show();
        }
        public static void Testmanyvec(Vectors[] a0, Vectors[] y, double nu = 0.5, double eps = 1e-10, params int[] dims)
        {
            NeuroNet[] net = new NeuroNet[a0.Length];
            for (int i = 0; i < a0.Length; i++)
            {
                net[i] = new NeuroNet(a0[i], 1, 0.5, -0.5, dims);
                //net.Show();
                net[i].Teach(a0[i], y[i], nu, eps);

                $"Разность результата и эталона по норме {i + 1}".Show();
                (net[i].Result(a0[i]) - y[i]).DistNorm.Show();
                "".Show();

            }

            $"Нормы разности для усреднения".Show();
            NeuroNet n = NeuroNet.Averaging(net);
            for (int i = 0; i < net.Length; i++)
            {
                (n.Result(a0[i]) - y[i]).DistNorm.Show();
                n.StatRes(a0[i]).Show();
            }


        }
        public static void Testmanyvec2(Vectors[] a0, Vectors[] y, double nu = 0.5, double eps = 1e-10, params int[] dims)
        {
            NeuroNet net = new NeuroNet(a0[0], 1, 0.5, -0.5, dims);
            for (int i = 0; i < a0.Length; i++)
            {
                net = new NeuroNet(a0[i], 1, 0.5, -0.5, dims);
                //net.Show();
                net.Teach(a0[i], y[i], nu, eps);

                $"Разность результата и эталона по норме {i + 1}".Show();
                (net.Result(a0[i]) - y[i]).DistNorm.Show();
                "".Show();
            }

            $"Нормы разности для итоговой сети".Show();
            for (int i = 0; i < a0.Length; i++)
            {
                (net.Result(a0[i]) - y[i]).DistNorm.Show();
                net.StatRes(a0[i]).Show();
            }
        }
        public static void Testmanyvec3(Vectors[] a0, Vectors[] y, double nu = 0.5, double eps = 1e-10, params int[] dims)
        {
            NeuroNet net = new NeuroNet(a0[0], dims);
            //net.Teach(a0, y, nu, eps);
            net.TeachMany(a0, y, nu, eps);

            $"Нормы разности для усреднения".Show();
            for (int i = 0; i < a0.Length; i++)
            {
                $"Норма {(net.Result(a0[i]) - y[i]).DistNorm} вердикт {net.StatRes(a0[i])} (должно быть {i})".Show();
            }

        }

        public static void TestRand(Vectors[] a0, Vectors[] y, double begrange = 1000, int count = 100, double eps = 1e-10, params int[] dims)
        {
            NeuroNet net = RandomSearch(a0, y, eps, begrange, count, dims);

            int k = 0;

            $"Нормы разности для усреднения".Show();
            for (int i = 0; i < a0.Length; i++)
            {
                //$"".Show(); net.Result(a0[i]).Show();
                //net.Go(a0[i]);
                //net.ShowMe();

                //$"Результат: {net.Result(a0[i])}".Show();
                $"Норма {(net.Result(a0[i]) - y[i]).DistNorm}   \tвердикт {net.StatRes(a0[i])} (должно быть {i})".Show();
                "".Show();

                if (net.StatRes(a0[i]) != i)
                    k++;
            }

            net.ToFile($"jop ({k}) {DateTime.Now.ToString().Replace(':',' ')}.txt");
        }
        public static void TestRand2(Vectors[] a0, Vectors[] y, double begrange = 1000, int count = 100, double eps = 1e-10, params int[] dims)
        {
            Vectors[] a01 = new Vectors[] { a0[0], a0[1], a0[2], a0[4], a0[6] };
            Vectors[] a02 = new Vectors[] { a0[3], a0[5], a0[7], a0[8], a0[9] };
            Vectors[] y1 = new Vectors[] { y[0], y[1], y[2], y[4], y[6] };
            Vectors[] y2 = new Vectors[] { y[3], y[5], y[7], y[8], y[9] };

            NeuroNet net1 = RandomSearch(a01, y1, eps, begrange, count, dims);
            NeuroNet net2 = RandomSearch(a02, y2, eps, begrange, count, dims);

            int k = 0;

            $"Нормы разности для усреднения".Show();
            for (int i = 0; i < a0.Length; i++)
            {
                //$"".Show(); net.Result(a0[i]).Show();
                //net.Go(a0[i]);
                //net.ShowMe();

                SetOfVectors s1 = new SetOfVectors(a01);
                SetOfVectors s2 = new SetOfVectors(a02);
                NeuroNet net;
                double d1 = s1.Dist(a0[i]),d2=s2.Dist(a0[i]);
                if (d1 < d2)
                    net = new NeuroNet(net1);
                else
                    net = new NeuroNet(net2);

                //$"Результат: {net.Result(a0[i])}".Show();
                $"Норма {(net.Result(a0[i]) - y[i]).DistNorm}   \tвердикт {net.StatRes(a0[i])} (должно быть {i})".Show();
                "".Show();

                if (net.StatRes(a0[i]) != i)
                    k++;
            }

            net1.ToFile($"jop1 ({k}) {DateTime.Now.ToString().Replace(':', ' ')}.txt");
            net2.ToFile($"jop1 ({k}) {DateTime.Now.ToString().Replace(':', ' ')}.txt");
        }

        public static void LastTest(Vectors[] a0, Vectors[] y, double begrange = 1000, int count = 100, double eps = 1e-10, params int[] dims)
        {

            Vectors a = Vectors.Union(a0);//a.Show();
            Vectors d = Vectors.Union(y);//d.Show();
            NeuroNet net = new NeuroNet(a, dims);

            net.Teach(a, d, 0.5, eps);

            int k = 0;

            $"Нормы разности для усреднения".Show();
            for (int i = 0; i < a0.Length; i++)
            {
                //$"".Show(); net.Result(a0[i]).Show();
                //net.Go(a0[i]);
                //net.ShowMe();

                //$"Результат: {net.Result(a0[i])}".Show();

                Vectors aa = Vectors.Repeat(a0[i], 10);
                Vectors dd = Vectors.Repeat(y[i], 10);

                $"Норма {(net.Result(aa) - dd).DistNorm}   \tвердикт {net.StatRes(aa) % 10} (должно быть {i})".Show();
                "".Show();

                if (net.StatRes(aa) % 10 != i)
                    k++;
            }

            net.ToFile($"jop ({k}) {DateTime.Now.ToString().Replace(':', ' ')}.txt");
        }

        public static void Test1vecLeven(Vectors a0, Vectors y, double nu = 0.5, double eps = 1e-10,int maxcount=1000, params int[] dims)
        {
            NeuroNet net = new NeuroNet(a0, dims);
            //net.Show();
            net.TeachLeven(a0, y, nu, eps,maxcount);

            $"Результат и эталон".Show();
            net.Result(a0).Show();
            y.Show();
        }

        public static void TestRandBest(Vectors[] a0, Vectors[] y, double begrange = 1000, int count = 100, double eps = 1e-10, params int[] dims)
        {
            //NeuroNet net = RandomSearch(a0, y, eps, begrange, count, dims);

            //NeuroNet rr = new NeuroNet(a0[0], 55);rr.ToFile("ss.txt");


            NeuroNet net = NeuroNet.FromFile("jop(0).txt");

            //net.Show();
            Debug.WriteLine($"Погрешность в начале: {net.GlobalNorm(a0, y)}");
            var tup = GetGlobalMin(a0, y, net, -begrange, begrange, eps: eps, countpoints: count, maxcountstep: 20);
            net.VectorToWeigths(tup.Item1);

            int k = 0;

            $"Нормы разности для усреднения".Show();
            for (int i = 0; i < a0.Length; i++)
            {
                //$"".Show(); net.Result(a0[i]).Show();
                //net.Go(a0[i]);
                //net.ShowMe();

                //$"Результат: {net.Result(a0[i])}".Show();
                $"Норма {(net.Result(a0[i]) - y[i]).DistNorm}   \tвердикт {net.StatRes(a0[i])} (должно быть {i})".Show();
                "".Show();

                if (net.StatRes(a0[i]) != i)
                    k++;
            }

            net.Show();
            net.ToFile($"jop ({k}) {DateTime.Now.ToString().Replace(':', ' ')}.txt");
        }

    }
}
