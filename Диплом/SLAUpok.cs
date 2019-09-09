using System;
using System.IO;
using МатКлассы;
using Point = МатКлассы.Point;
using static МатКлассы.FuncMethods.DefInteg;
using static Курсач.KursMethods.TestFuncAndCurve;


namespace Курсач
{
    /// <summary>
    /// Класс СЛАУ с методами их решения (продолжение)
    /// </summary>
    public class SLAUpok : SLAU
    {
        public SLAUpok() : base() { }
        public SLAUpok(StreamReader fs) : base(fs) { }
        public SLAUpok(SLAUpok M, int t) : base(M, t) { }
        public SLAUpok(SLAUpok M)
        {
            Make(M.Size);
            ultraval = -1;
            UltraCount = 0;
            this.curve = M.curve;
            for (int i = 0; i < this.Size; i++)
                for (int j = 0; j < this.Size; j++)
                    this.A[i, j] = M.A[i, j];
        }
        public CurveK curve;
        public double[] ErrorsMas,ErrorMasP,ErrorsM1;
        //public Functional f;

        public double Error(int k) //частичная погрешность
        {
            double p = curve.Firstkind(KursMethods.N, KursMethods.N);
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

        private double ultraval=-1;
        public double VALUE_FOR_ULTRA { get { if (ultraval == -1) ultraval = curve.Firstkind(KursMethods.N, KursMethods.N); return ultraval; } set {ultraval=value; } }
        public override void Make(int k)
        {
            base.Make(k);
            UltraCount = 0;
            ultraval = -1;
            ErrorsMas = new double[k];
            ErrorMasP = new double[k];
            ErrorsM1 = new double[k];
        }
        public void Make(int k,double[,] AMAS)
        {
            Make(k);
            for (int i = 0; i < this.Size; i++)
                for (int j = 0; j < this.Size; j++)
                    this.A[i, j] = AMAS[i, j];
        }

        /// <summary>
        /// Число, которое показывает, какая часть системы уже была решена ультра-гибридом
        /// </summary>
        public int UltraCount=0;
        public void UltraHybrid(int t)
        {
            //UltraCount.Show();
            if(UltraCount==0)//если вообще не решалось
            {
                //"Вошло".Show();
                x[0] = b[0] / A[0, 0];  //x[0].Show();
                VALUE_FOR_ULTRA = Error(1);
                ErrorsMas[0] = VALUE_FOR_ULTRA;
                Functional f = (Point x) =>
                {
                    double sum = this.x[0] * KursMethods.masPoints[0].PotentialF((Point)x);
                    double s = sum - KursMethods.fig(x);
                    return s * s;
                };
                Functional fM1 = (Point x) =>
                {
                    //это я ещё не использовал!!!
                    //Functional M1ApproxD = (Point u) =>
                    //{
                    //    Functional ro = (Point y) =>
                    //    {
                    //        double sss = this.x[0] * KursMethods.masPoints[0].PotentialF(u);
                    //        return sss * KursMethods.Exy(u, y);
                    //    };
                    //    return DoubleIntegral(ro, curve, curve.S, FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY) /*IntegralClass.Integral(ro, KursMethods.CIRCLE - 1)*/ /*- right(x)*/;
                    //};

                    double su = KursMethods.M1Approx(x);
                    double s = su - KursMethods.TestFuncAndCurve.DFunctions[KursMethods.GF - 1](x);
                    return s * s;
                };

                ErrorMasP[0] = Math.Sqrt(DoubleIntegral(f, curve, curve.S,  FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY));
                ErrorsM1[0] = Math.Sqrt(IntegralClass.Integral2(fM1,KursMethods.CIRCLE-1));
                $"{1} = {ErrorsM1[0]}".Show();

                UltraCount++;
                for (int i = UltraCount + 1; i <= t; i++)
                    UltraHybridLast(i);
                UltraCount = t;
            }
            else if(UltraCount==t-1)//если надо решить только по последней координате
            {
                    UltraHybridLast(t);
                    UltraCount++;
            }
            else
            {
                for (int i = UltraCount+1; i <= t; i++)
                    UltraHybridLast(i);
                UltraCount = t;
            }
        }
        /// <summary>
        /// Ультра-гибридный метод суперского решения по последней координате
        /// </summary>
        /// <param name="t"></param>
        public void UltraHybridLast(int t) //гибридный с координатной минимизацией по последней координате
        {
            double[] c = new double[t];
            for (int i = 0; i < t - 1; i++)
                c[i] = x[i];
            Vectors mk1 = new Vectors(c), mk2 = new Vectors(c);
            bool existres = true;

            double sum = 0;
            GaussSpeedy(t);

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
                        $"{VALUE_FOR_ULTRA} < {tmp1} при t = {t} (до полной покоординатной минимизации на конце)".Show();
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
                            existres = false;
                        }
                        else
                        {
                            $"Погрешность уменьшена МИНИМАКОЙ НА КОНЦЕ на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} % (now {tmp})".Show();
                            VALUE_FOR_ULTRA = tmp;
                        }

                    }
                    else
                    {
                        $"Погрешность уменьшена ПОЛНОЙ МИНИМАКОЙ на {(VALUE_FOR_ULTRA - tmp1) / VALUE_FOR_ULTRA * 100}% (now {tmp1})".Show();
                        VALUE_FOR_ULTRA = tmp1;
                    }
                }
                else
                {
                    $"Погрешность уменьшена МИНИМАКОЙ СПИДГАУССА на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} % (now {tmp})".Show();
                    VALUE_FOR_ULTRA = tmp;
                }
            }
            else
            {
                $"Погрешность уменьшена СПИДГАУССОМ на {(VALUE_FOR_ULTRA - tmp) / VALUE_FOR_ULTRA * 100} % (now {tmp})".Show();
                VALUE_FOR_ULTRA = tmp;
            }

            ErrorsMas[t - 1] = VALUE_FOR_ULTRA;
            Functional f = (Point x) =>
            {
                sum = 0;

                for (int ii = 1; ii <= t; ii++)
                {
                    sum += this.x[ii - 1] * KursMethods.masPoints[ii - 1].PotentialF(x);
                }
                double s = sum - KursMethods.fig(x);
                return s * s;
            };
            ErrorMasP[t-1] = Math.Sqrt(DoubleIntegral(f, curve, curve.S, FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY));
            //UltraCount = t;
            NEVA = Nev(A, x, b, t);

            Functional fM1 = (Point x) =>
            {

            //    Functional M1ApproxD = (Point u) =>
            //{
            //    Functional ro = (Point y) =>
            //    {
            //        double sss = 0; for (int i = 0; i < t; i++) sss += this.x[i] * KursMethods.masPoints[i].PotentialF(u);
            //        return sss * KursMethods.Exy(u, y);
            //    };
            //    //return DoubleIntegral(ro, curve, curve.S, FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY) /*IntegralClass.Integral(ro, KursMethods.CIRCLE - 1)*/ /*- right(x)*/;
            //    return IntegralClass.Integral(ro, KursMethods.CIRCLE - 1);
            //};

                double su = KursMethods.M1Approx(x);
                double s = su - KursMethods.U(x);
                return s * s;
            };
            if (existres)
            {
                $"Считается погрешность метода 1".Show();
                ErrorsM1[t - 1] = Math.Sqrt(IntegralClass.Integral2(fM1, KursMethods.CIRCLE - 1));
            }
            else
                ErrorsM1[t - 1] = ErrorsM1[t - 2];


            $"{t} = {ErrorsM1[t - 1]}".Show();
        }
    }
}
