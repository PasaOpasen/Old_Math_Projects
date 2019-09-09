using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МатКлассы;
using static Курсач.KursMethods;
using System.IO;

namespace Курсач
{
    public class BiharmonicEquation
    {
        #region Поля
        internal SqMatrix D1, D2, S;
        internal Vectors R1, R2;

        private SqMatrix D1m, D2m, dl;
        private Vectors dr;

        public Vectors d, cnew;
        public Vectors c => D1m * (R1 + S * d);
        public double[] ErrorsMasL, ErrorsMasQ;
        public SLAU slau;

        public Func<Vectors, double> F;
        public double p1, p2;
        #endregion

        /// <summary>
        /// Размерность задачи
        /// </summary>
        public int dim => R1.Deg;

        /// <summary>
        /// Созадть систему матриц и векторов по базисным функция
        /// </summary>
        /// <param name="n">Число базисных функций. Если -1, то берутся все функции</param>
        public BiharmonicEquation(int n = -1)
        {
            if (n == -1) n = masPoints.Length;
            D1 = new SqMatrix(n);
            D2 = new SqMatrix(n);
            S = new SqMatrix(n);
            R1 = new Vectors(n);
            R2 = new Vectors(n);

            Parallel.For(0, n, (int i) =>
            {
                for (int j = i; j < n; j++)
                {
                    D1[i, j] = IntegralClass.IntegralCurve((Point x) => alpha(x, i) * alpha(x, j)+alphanu(x, i) * alphanu(x, j), CIRCLE - 1);
                    D2[i, j] = IntegralClass.IntegralCurve((Point x) => beta(x, i) * beta(x, j)+betanu(x, i) * betanu(x, j), CIRCLE - 1);
                    S[i, j] = IntegralClass.IntegralCurve((Point x) => alpha(x, i) * beta(x, j)+alphanu(x, i) * betanu(x, j), CIRCLE - 1);
                }
                R1[i] = IntegralClass.IntegralCurve((Point y) => alphanu(y, i) * Unu(y)+ alpha(y, i)*U(y), CIRCLE - 1);
                R2[i] = IntegralClass.IntegralCurve((Point y) => betanu(y, i) * Unu(y)+ beta(y, i)*U(y), CIRCLE - 1);

                for (int j = i + 1; j < n; j++)
                {
                    D1[j, i] = D1[i, j];
                    D2[j, i] = D2[i, j];
                    S[j, i] = S[i, j];
                }

            });


            d = new Vectors(n);
            cnew = new Vectors(n);

            ErrorsMasL = new double[n];
            ErrorsMasQ = new double[n];

          p1 = IntegralClass.IntegralCurve((Point y) => Unu(y).Sqr(), CIRCLE - 1);
          p2 = IntegralClass.IntegralCurve((Point y) => U(y).Sqr(), CIRCLE - 1);

          $"p1 = {p1}, p2 = {p2}".Show();

            var mems = new Memoize<Point, Vectors[]>((Point p) => {
                Vectors[] r = new Vectors[4];
                for (int i = 0; i < 4; i++) r[i] = new Vectors(dim);
                for (int i = 0; i < dim; i++)
                {
                    r[0][i] = alpha(p, i);
                    r[1][i] = alphanu(p, i);
                    r[2][i] = beta(p, i);
                    r[3][i] = betanu(p, i);
                }
                return r;
            }).Value;
            Func<Point, Vectors[]> AB = (Point p) => mems(p);

            Func<Vectors,double> FF = (Vectors v) =>
            {
                Vectors ce = new Vectors(this.dim);
                Vectors de = new Vectors(this.dim);
                for (int i = 0; i < v.Deg / 2; i++)
                {
                    ce[i] = v[i];
                    de[i] = v[i + v.Deg / 2];
                }

                Functional func = (Point p) => {
                    var vec = AB(p);
                    return (U(p) - ce * vec[0] - de * vec[2]).Sqr() + (Unu(p) - ce * vec[2] - de * vec[3]).Sqr();
                };
                return IntegralClass.IntegralCurve(func, CIRCLE - 1);

                Functional f1 = (Point y) => {
                    double sum = U(y);
                    for (int i = 0; i < dim; i++)
                        sum -= ce[i] * alpha(y, i) + de[i] * beta(y, i);
                    return sum * sum;
                };
                Functional f2 = (Point y) => {
                    double sum = Unu(y);
                    for (int i = 0; i < dim; i++)
                        sum -= ce[i] * alphanu(y, i) + de[i] * betanu(y, i);
                    return sum * sum;
                };

                return IntegralClass.IntegralCurve((Point p) => f1(p) + f2(p), CIRCLE - 1);
            };
            F = (Vectors v) =>
               {
return FF(v);
                   Vectors cc = new Vectors(this.dim);
                   Vectors dd = new Vectors(this.dim);
                   for (int i = 0; i < v.Deg / 2; i++)
                   {
                       cc[i] = v[i];
                       dd[i] = v[i + v.Deg / 2];
                   }

                   int k = this.dim;
                   double sum =p1 + p2 -2 * (cc * R1 + dd * R2);
                   for (int i = 0; i < k; i++)
                       for (int j = 0; j < k; j++)               
                           sum += 2 * cc[i] * dd[j] * S[i, j] + cc[i] * cc[j] * D1[i, j] + dd[i] * dd[j] * D2[i, j];
                       
                   return sum;
               };


            Vectors tmp =Vectors.Union2( new Vectors(dim),Vectors.Random(dim, -4, 4));
            $"Error! {FF(tmp)} != {F(tmp)}".Show();


    //        Parallel.Invoke(
    //() => D1m = D1.Invertion,
    //() => D2m = D2.Invertion
    //);

    //        dl = D2 - S * (D1m * S);
    //        dr = R2 + S * D1m * R1;
    //        slau = new SLAU(dl, dr);
        }

        /// <summary>
        /// Вывести показатели качества обращения матриц
        /// </summary>
        private void NevaskShow()
        {
            $"Невязка D1: {(D1 * D1m - SqMatrix.E(D1.RowCount)).OctNorn}".Show();
            $"Невязка D2: {(D2 * D2m - SqMatrix.E(D1.RowCount)).OctNorn}".Show();
        }

        /// <summary>
        /// Вывести матрицы и векторы системы
        /// </summary>
        private void Shows()
        {
            "D1".Show(); D1.PrintMatrix();
            "D2".Show(); D2.PrintMatrix();
            "S".Show(); S.PrintMatrix();
            "R1".Show(); R1.Show();
            "R2".Show(); R2.Show();
        }

        /// <summary>
        /// Погрешность на данном этапе k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public double Error => F(Vectors.Union2(cnew, d));


        #region Ультра-гибрид
        private double ultraval = -1;
        public double VALUE_FOR_ULTRA
        {
            get
            {
                if (ultraval == -1) ultraval = IntegralClass.Integral((Point y) => (TestFuncAndCurve.Grads[GF - 1](y) * TestFuncAndCurve.Norm[CIRCLE - 1](y)).Sqr(), CIRCLE - 1) + IntegralClass.Integral((Point y) => (U(y)).Sqr(), CIRCLE - 1);
                return ultraval;
            }
            set { ultraval = value; }
        }

        /// <summary>
        /// Число, которое показывает, какая часть системы уже была решена ультра-гибридом
        /// </summary>
        public int UltraCount = 0;

        public void UltraHybrid(int t)
        {
            Functional f = (Point x) =>
                {
                    double sum = 0;
                    for (int i = 0; i < dim; i++)
                        sum += c[i] * alpha(x, i) + d[i] * beta(x, i);
                    return Math.Abs(sum - KursMethods.U(x));
                };

            if (UltraCount == 0)//если вообще не решалось
            {
                ErrorsMasQ = new double[dim];
                ErrorsMasL = new double[dim];

                //"Вошло".Show();
                d[0] = dr[0] / dl[0, 0];  //x[0].Show();
                VALUE_FOR_ULTRA = Error;
                ErrorsMasL[0] = VALUE_FOR_ULTRA;
                ErrorsMasQ[0] = IntegralClass.Integral(f, CIRCLE - 1);
                //BeeDown(-20, 20, 1000, 1);

                UltraCount++;
                for (int i = UltraCount + 1; i <= t; i++)
                    UltraHybridLast(i);
                UltraCount = t;
            }
            else if (UltraCount == t - 1)//если надо решить только по последней координате
            {
                UltraHybridLast(t);
                UltraCount++;
            }
            else
            {
                for (int i = UltraCount + 1; i <= t; i++)
                    UltraHybridLast(i);
                UltraCount = t;
            }
        }

        private void MINIMAKA(int t)
        {
            //double sum = 0;
            //for (int k = 0; k <= t - 1; k++)
            //{
            //    for (int j = 0; j < k; j++)
            //        sum +=c[j] * S[k, j];
            //    for (int j = 0; j <= t - 1; j++)
            //        if(j!=k)
            //        sum += d[j]*D2[k,j];
            //    slau.x[k] = (R2[k] - sum) / D2[k, k];
            //}

            //BeeDown(-20, 20, 400+30*t, t);
        }

        /// <summary>
        /// Ультра-гибридный метод суперского решения по последней координате
        /// </summary>
        /// <param name="t"></param>
        public void UltraHybridLast(int t) //гибридный с координатной минимизацией по последней координате
        {
            double[] c = new double[t], cn = new double[t];
            //cnew = this.c.dup;

            for (int i = 0; i < t - 1; i++)
            {
                c[i] = slau.x[i];
                cn[i] = cnew[i];
            }

            Vectors mk1 = new Vectors(c), mk2 = new Vectors(c);

            double sum = 0;
            slau.GaussSpeedy(t);
            cnew = this.c.dup;

            double tmp = Error;
            if (VALUE_FOR_ULTRA < tmp) //если погрешность выросла - исправить это, потому что новое решение не годится
            {
                $"{VALUE_FOR_ULTRA} < {tmp} при t = {t} (до покоординатной минимизации результата СПИДГАУССА)".Show();
                //покоординатная минимизация результата СПИДГАУССА
                MINIMAKA(t);

                tmp = Error;
                if (VALUE_FOR_ULTRA < tmp)
                {
                    $"{VALUE_FOR_ULTRA} < {tmp} при t = {t} (до полной покоординатной минимизации вектора с1 с2 ... 0)".Show();
                    for (int i = 0; i < t; i++)//исправили, теперь пробуем новый метод                 
                    {
                        slau.x[i] = c[i];
                        cnew[i] = cn[i];
                    }

                    //покоординатная минимизация
                    MINIMAKA(t);


                    double tmp1 = Error;
                    if (VALUE_FOR_ULTRA < tmp1) //погрешность опять выросла - тогда просто оставляем 0 на конце
                    {
                        $"{VALUE_FOR_ULTRA} < {tmp1} при t = {t} (до полной покоординатной минимизации на конце)".Show();
                        for (int i = 0; i < t; i++)//исправили, теперь пробуем новый метод                 
                        {
                            slau.x[i] = c[i];
                            cnew[i] = cn[i];
                        }

                        for (int j = 0; j < t - 1; j++)
                        {
                            sum += c[j] * S[t - 1, j];
                            sum += d[j] * D2[t - 1, j];
                        }
                        sum += c[t - 1] * S[t - 1, t - 1];
                        slau.x[t - 1] = (R2[t - 1] - sum) / slau.A[t - 1, t - 1];
                        sum = 0;

                        tmp = Error;
                        if (VALUE_FOR_ULTRA < tmp)
                        {
                            for (int i = 0; i < t - 1; i++)//исправили, теперь пробуем новый метод                 
                            {
                                slau.x[i] = c[i];
                                cnew[i] = cn[i];
                            }
                            slau.x[t - 1] = 0;
                            cnew[t - 1] = 0;
                        }
                        else
                        {
                            $"Погрешность уменьшена МИНИМАКОЙ НА КОНЦЕ на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} %".Show();
                            VALUE_FOR_ULTRA = tmp;
                        }

                    }
                    else
                    {
                        $"Погрешность уменьшена ПОЛНОЙ МИНИМАКОЙ на {(VALUE_FOR_ULTRA - tmp1) / VALUE_FOR_ULTRA * 100} %".Show();
                        VALUE_FOR_ULTRA = tmp1;
                    }
                }
                else
                {
                    $"Погрешность уменьшена МИНИМАКОЙ СПИДГАУССА на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} %".Show();
                    VALUE_FOR_ULTRA = tmp;
                }
            }
            else
            {
                $"Погрешность уменьшена СПИДГАУССОМ на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} %".Show();
                VALUE_FOR_ULTRA = tmp;
            }

            ErrorsMasL[t - 1] = VALUE_FOR_ULTRA;
            Functional f = (Point x) =>
            {
                double s = 0;
                for (int i = 0; i < t; i++)
                    s += c[i] * alpha(x, i) + d[i] * beta(x, i);
                return Math.Abs(s - KursMethods.U(x));
            };
            ErrorsMasQ[t - 1] = IntegralClass.Integral(f, CIRCLE - 1);

        }
        #endregion

        public void BeeDown(double min, double max, int count, int t,int maxmax=100)
        {

            Vectors old = Vectors.Union2( cnew.SubVector(t), d.SubVector(t) );

            //slau.GaussSelection(t);
            //cnew=

            var r = BeeHiveAlgorithm.GetGlobalMin(this.F, (t) * 2, min, max, 1e-15, count, 60, old,maxmax);
            //$"Hive method ---> {r.Item2}".Show();
            VecToCD(r.Item1);

            ErrorsMasL[t - 1] = r.Item2;
            Functional ff = (Point x) =>
            {
                double s = 0;
                for (int i = 0; i < t; i++)
                    s += cnew[i] * alpha(x, i) + d[i] * beta(x, i);
                return Math.Abs(s - KursMethods.U(x));
            };
            ErrorsMasQ[t - 1] = IntegralClass.Integral(ff, CIRCLE - 1);

        }
        /// <summary>
        /// Записывает общий вектор v в векторы cnew и D
        /// </summary>
        /// <param name="v"></param>
        private void VecToCD(Vectors v)
        {
            //cnew = new Vectors(this.dim);
            //d = new Vectors(this.dim);
            for (int i = 0; i < v.Deg / 2; i++)
            {
                cnew[i] = v[i];
                d[i] = v[i + v.Deg / 2];
            }
        }

        private Tuple<Vectors, double, double> HalfSolve(int len)
        {
            SqMatrix iD1 = D1.SubMatrix(len), iD2 = D2.SubMatrix(len), iS = S.SubMatrix(len);
            Vectors iR1 = R1.SubVector(len), iR2 = R2.SubVector(len);

            SqMatrix iD1m = D1m.SubMatrix(len), iD2m = D2m.SubMatrix(len), idl;
            Vectors idr;

            idl = (D2 -  S * (D1m * S)).SubMatrix(len);
            idr = (R2 + S * D1m * R1).SubVector(len);

            Vectors id=idl.Solve(idr), icnew = (D1m * (R1 + S * id)).SubVector(len);

            Vectors res = Vectors.Union2(icnew, id);

            double p1 = IntegralClass.IntegralCurve((Point y) => (TestFuncAndCurve.Grads[GF - 1](y) * TestFuncAndCurve.Norm[CIRCLE - 1](y)).Sqr(), CIRCLE - 1);
            double p2 = IntegralClass.IntegralCurve((Point y) => (U(y)).Sqr(), CIRCLE - 1);

            Functional fif = (Point x) =>
            {
                double s = 0;
                for (int i = 0; i < id.Deg; i++)
                    s += icnew[i] * alpha(x, i) + id[i] * beta(x, i);
                return Math.Abs(s - KursMethods.U(x));
            };
            double L = IntegralClass.Integral(fif, CIRCLE - 1);

            $"len = {len}, L = {this.F(res)}".Show();

            return new Tuple<Vectors, double, double>(res, F(res), L);
        }

        public static Tuple<double[], double[], int[], Func<Point, double>> LastMethod(int n, int g, int cu, SLAUpok SYSTEM = null, SLAUpok SYSTEMQ = null, int maxmax=150)
        {
            double[] ErrorsMasL = new double[n], ErrorsMasQ = new double[n];
            int[] Times = new int[n];
            bool Follow(int len, int it)
            {
                var d = ErrorsMasL.Where(nn => nn > 0).ToArray();
                return d.Length - d.Distinct().Count() <= len;
            }

            var tmp = new BiharmonicEquation(); //tmp.NevaskShow();
            for (int t = 1; t <= n; t++)
            {
                $"Now t = {t}".Show();
                double range = 20 + 2 * (n - t);

                DateTime time = DateTime.Now;
                if (Follow(5, t))
                {
                    //if (t==n)
                    //{
                    //    var s = tmp.HalfSolve(t);
                    //    if (s.Item2 < ErrorsMasL[t - 2])
                    //    {
                    //        ErrorsMasL[t - 1] = s.Item2;
                    //        ErrorsMasQ[t - 1] = s.Item3;

                    //        for (int i = 0; i < s.Item1.Deg / 2; i++)
                    //        {
                    //            tmp.cnew[i] = s.Item1[i];
                    //            tmp.d[i] = s.Item1[i + s.Item1.Deg / 2];
                    //        }
                    //        continue;
                    //    }
                    //}

                    tmp.BeeDown(-range, range, 300 + 10 * t, t, maxmax);
                    Times[t - 1] = (DateTime.Now - time).Milliseconds + ((t == 1) ? 0 : Times[t - 2]);
                    ErrorsMasL[t - 1] = tmp.ErrorsMasL[t - 1];
                    ErrorsMasQ[t - 1] = tmp.ErrorsMasQ[t - 1];
                }
                else
                {
                    Times[t - 1] = Times[t - 2];
                    ErrorsMasL[t - 1] = ErrorsMasL[t - 2];
                    ErrorsMasQ[t - 1] = ErrorsMasQ[t - 2];
                    tmp.ErrorsMasL[t - 1] = ErrorsMasL[t - 1];
                    tmp.ErrorsMasQ[t - 1] = ErrorsMasQ[t - 1];
                }

            }

            Func<Point, double> ff = (Point x) =>
             {
                 double s = 0;
                 for (int i = 0; i < tmp.dim; i++)
                     s += tmp.cnew[i] * alpha(x, i) + tmp.d[i] * beta(x, i);
                 return s;
             };
            return new Tuple<double[], double[], int[], Func<Point, double>>(ErrorsMasL, ErrorsMasQ, Times, ff);
        }

        public static Tuple<double[], double[], int[], Func<Point, double>> LastMethod2(int n, int g, int cu, SLAUpok SYSTEM = null, SLAUpok SYSTEMQ = null)
        {

            ForDesigion.Building(n, g, cu, SYSTEM, SYSTEMQ);
            double[] ErrorsMasL = new double[n], ErrorsMasQ = new double[n];
            int[] Times = new int[n];

            var tmp = new BiharmonicEquation();

            double range = 200;

            Vectors old = Vectors.Union2( tmp.cnew, tmp.d );

            var r = BeeHiveAlgorithm.GetGlobalMin(tmp.F, 2*n, -range, range, 1e-15, 1200, 60, old, -1);

            for (int t = 0; t < n; t++)
            {
                Functional fif = (Point x) =>
                {
                    double s = 0;
                    for (int i = 0; i < t; i++)
                        s += tmp.cnew[i] * alpha(x, i) + tmp.d[i] * beta(x, i);
                    return Math.Abs(s - KursMethods.U(x));
                };

                ErrorsMasQ[t] = IntegralClass.Integral(fif, CIRCLE - 1);
                ErrorsMasL[t] = tmp.F(Vectors.Union2( tmp.cnew.SubVector(t), tmp.d.SubVector(t) ));
            }

    
            Func<Point, double> ff = (Point x) =>
            {
                double s = 0;
                for (int i = 0; i < tmp.dim; i++)
                    s += tmp.cnew[i] * alpha(x, i) + tmp.d[i] * beta(x, i);
                return s;
            };
            return new Tuple<double[], double[], int[], Func<Point, double>>(ErrorsMasL, ErrorsMasQ, Times, ff);
        }

        /// <summary>
        /// Решает всё как надо, выдаёт массив погрешностей для границы (минимизируемый функционал) и вообще по области (цель решения задачи)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="g"></param>
        /// <param name="cu"></param>
        /// <param name="SYSTEM"></param>
        /// <param name="SYSTEMQ"></param>
        /// <returns></returns>
        public static Tuple<double[], double[]> CreateAndSolve(int n, int g, int cu, SLAUpok SYSTEM = null, SLAUpok SYSTEMQ = null)
        {
            //"вход".Show();
            ForDesigion.Building(n, g, cu, SYSTEM, SYSTEMQ);
            //"есть билд".Show();

            var tmp = new BiharmonicEquation();
            tmp.NevaskShow();

            tmp.UltraHybrid(tmp.dim);

            tmp.BeeDown(-20, 20, 1000, tmp.dim - 1);

            return new Tuple<double[], double[]>(tmp.ErrorsMasL, tmp.ErrorsMasQ);

        }

        public static void Test()
        {
            //ForDesigion.Building(10, 3, 3);
            //using (StreamWriter cir = new StreamWriter("Circles.txt"))
            //{
            //    for (int i = 0; i < CircleName.Length; i++)
            //        cir.WriteLine(CircleName[i]);
            //}
            //using (StreamWriter fir = new StreamWriter("Functions.txt"))
            //{
            //    for (int i = 0; i < KursMethods.FuncName.Length - 1; i++)
            //        fir.WriteLine(KursMethods.FuncName[i]);
            //}

            for (CIRCLE = 2; CIRCLE <= CountCircle; CIRCLE++)
                    for (GF = 1; GF <= KGF - 4; GF++)
                    {
                    //if (CIRCLE == 2) return;

                    //var t = BiharmonicEquation.CreateAndSolve(30, GF, CIRCLE);

                    $"Circle = {CIRCLE} \tGF = {GF}".Show();
                    ForDesigion.Building(40, GF, CIRCLE);
                    var t = BiharmonicEquation.LastMethod(40, GF, CIRCLE);

                        string s = $"C={CircleName[CIRCLE - 1]} f={FuncName[GF - 1]}.txt";
                        StreamWriter fs = new StreamWriter(s);
                        for (int i = 0; i < masPoints.Length; i++)
                            fs.WriteLine($"{i + 1} {t.Item1[i]} {t.Item2[i]} {t.Item3[i]}");

                        fs.Close(); 
                    }
            System.Diagnostics.Process.Start("cmd", "/c shutdown -s -f -t 00");
        }


        public void TestInteg()
        {
            Vectors ce = Vectors.Random(dim, -5, 9);
            Vectors de= Vectors.Random(dim, -5, 5);

            Func<double> FF = () =>
              {
                  Functional f1 = (Point y) => {
                  double sum = U(y);
                  for (int i = 0; i < dim; i++)
                      sum -= ce[i]*alpha(y, i) + de[i]*beta(y, i);
                      return sum*sum;
                  };
                  Functional f2 = (Point y) => {
                      double sum = Unu(y);
                      for (int i = 0; i < dim; i++)
                          sum -= ce[i]*alphanu(y, i) + de[i]*betanu(y, i);
                      return sum*sum;
                  };

                  return IntegralClass.IntegralCurve((Point p)=>f1(p)+f2(p), CIRCLE - 1);
              };

            $"Погрешность представления 1){FF()} 2){F(Vectors.Union2(ce,de))}".Show();

        }

        public void StableTest(Point p, int k)
        {
            $"alpha = {alpha(p,k)} beta = {beta(p,k)} alphanu = {alphanu(p,k)} betanu = {betanu(p,k)}".Show();
        }
    }


}

