using System;
using МатКлассы;
using Point = МатКлассы.Point;
using static МатКлассы.FuncMethods.DefInteg;

namespace Курсач
{
    public static class IntegralClass
    {
        //только 2-20, 32, 64, 96, 100, 128, 256, 512, 1024
       public static int n = 128;

        /// <summary>
        /// Двойной интеграл
        /// </summary>
        /// <param name="f"></param>
        /// <param name="Curve"></param>
        /// <returns></returns>
        public static double Integral(Functional f, int Curve)
        {
            Func<double, double, double> func = (double x, double y) =>
            {
                Point t = new Point(x, y);
                if (KursMethods.filters[Curve](t)) return f(t);
                return 0;
            };

            double a, b, c, d;
            switch (Curve)
            {
                case 0:
                    a =-KursMethods.MAX_RADIUS;
                    b= KursMethods.MAX_RADIUS;
                    c = a;
                    d = b;
                    break;
                case 1:
                    a = 0;
                    b = KursMethods.MIN_RADIUS;
                    c = a;
                    d = b * Math.Sqrt(3) / 2;
                    break;
                case 3:
                    a = 0;
                    b = KursMethods.MIN_RADIUS;
                    c = a;
                    d = b;
                    break;
                default:
                    a = 0;
                    b = KursMethods.MIN_RADIUS;
                    c = a;
                    d = b * Math.Sqrt(3) / 2;
                    break;
            }

            return MathNet.Numerics.Integration.GaussLegendreRule.Integrate(func, a, b, c, d, n);
        }

        public static double Integral2(Functional f, int Curve) => DoubleIntegral(f, KursMethods.myCurve, KursMethods.myCurve.S, FuncMethods.DefInteg.Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);

        /// <summary>
        /// Криволинейный интеграл интеграл
        /// </summary>
        /// <param name="f"></param>
        /// <param name="Curve"></param>
        /// <returns></returns>
        public static double IntegralCurve(Functional f, int Curve)
        {
            Func<double,  double> func = (double t) =>f(KursMethods.curvesQ[Curve].Transfer(t));

            double a= KursMethods.curvesQ[Curve].a, b= KursMethods.curvesQ[Curve].b;

            return MathNet.Numerics.Integration.GaussLegendreRule.Integrate(func, a, b, n);
        }

        /// <summary>
        /// Определённый интеграл
        /// </summary>
        /// <param name="func"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Integral(Func<double,double> func, double a,double b)
        {
            return MathNet.Numerics.Integration.GaussLegendreRule.Integrate(func, a, b, n);
        }
    }
}
