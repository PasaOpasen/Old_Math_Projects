using System;
using МатКлассы;
using Point = МатКлассы.Point;


namespace Курсач
{
    /// <summary>
    /// Класс кривых для курсача - пришлось так сделать, чтобы добавить новый метод
    /// </summary>
    public class CurveK : Curve
    {
        public CurveK(double a0, double b0, RealFunc uu, RealFunc vv) : base(a0, b0, uu, vv) { }
        public CurveK(double a0, double b0, RealFunc uu, RealFunc vv,double radius) : base(a0, b0, uu, vv,radius) { }
        public CurveK(double a0, double b0, RealFunc uu, RealFunc vv, double radius,DRealFunc uuu,DRealFunc vvv,TripleFunc T,RealFunc end) : base(a0, b0, uu, vv, radius,uuu,vvv,T,end) { }
        public CurveK() : base() { }

        /// <summary>
        /// Вычисление криволинейного интеграла первого рода по этой кривой от функции BasisFuncPow(int i,int j,Point z)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double Firstkind(int i, int j)
        {
            //return this.Firstkind((Point x)=>KursMethods.BasisFuncPow(i,j,(Point)x));
            Functional f = (Point x) => KursMethods.BasisFuncPow(i, j, (Point)x);
            Func<double, double> func = (double t) => f(this.Transfer(t));
            return IntegralClass.Integral(func, this.a, this.b);
        }
    }
}
