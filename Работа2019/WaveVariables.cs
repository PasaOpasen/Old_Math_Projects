using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using МатКлассы2018;
using static МатКлассы2018.Number;
using static МатКлассы2018.FuncMethods.DefInteg;
using static МатКлассы2018.FuncMethods.Optimization;
using МатКлассы2019;
using VectorNetFunc = System.Collections.Generic.List<System.Tuple<double, МатКлассы2018.Vectors>>;
using System.Collections;
using Computator.NET.Core.Functions;
using static Functions;
using System.Windows.Forms;
using Практика_с_фортрана;

public static class Forms
{
    public static UGrafic UG=new UGrafic();
}

public static class РабКонсоль
{

    public static double w = 2;

    #region Параметры DINN
    public static bool DINNplay = false;

    private static double t11 = 0, t44 = 15;
    public static double t1
    {
        get
        {
            if (t11 < 0) t11 = Math.Min(k1(w), k2(w)) / 2;
            return t11;
        }
        set
        {
            t11 = value;
        }
    }
    public static double t4
    {
        get
        {
            if (t44 < 0) t44 = Math.Max(k1(w), k2(w)) * 2;
            return t44;
        }
        set
        {
            t44 = value;
        }
    }

    public static double /*t1=0,*/ t2 = t1, t3 = t1, /*t4=15,*/ tm = 0.14, tp = 0, eps = 1e-8, pr = 1e-2, gr = 1e4;
    public static GaussKronrod.NodesCount NodesCount = GaussKronrod.NodesCount.GK31;
    #endregion

    #region Параметры для дисперсионок
    public static Complex[] Poles;
    public static double steproot = 1e-3, polesBeg = 0.01, polesEnd = 15;
    public static double epsjump = 1e-1, epsroot = 1e-3;
    public static int countroot = 50;
    public static void SetPoles(double beg = 0.01, double end = 15, double step = 1e-3, double eps = 1e-4, int count = 50)
    {
        РабКонсоль.Poles = ((Complex[])FuncMethods.Optimization.Halfc(Deltas, beg, end, step, eps, count))/*.Where(c=>c!=0).ToArray()*/;
        List<Complex> value = new List<Complex>(), newmas = new List<Complex>();

        double wtf = Deltas(РабКонсоль.Poles[0]).Abs;
        if (wtf < 1e-3)
            newmas.Add(РабКонсоль.Poles[0]);
        for (int j = 1; j < РабКонсоль.Poles.Length; j++)
            newmas.Add(РабКонсоль.Poles[j]);
        РабКонсоль.Poles = newmas.ToArray();
    }
    public static void SetPolesDef() => SetPoles(РабКонсоль.polesBeg, РабКонсоль.polesEnd, РабКонсоль.steproot, РабКонсоль.epsroot, РабКонсоль.countroot);
    #endregion
}

public static class Functions
{

    static Functions()
    {
        AfterChaigeData();
        //var b1 = new Memoize< Complex, Complex>(( Complex t)=>littleBessel1(t)).Value;
        //LittleBessel1 = (Complex c) => b1(c);
        //var b2 = new Memoize<Complex, Complex>((Complex t) => littleBessel2(t)).Value;
        //LittleBessel2 = (Complex c) => b1(c);
    }
    public static void AfterChaigeData()
    {
        ml2 = 2 * mu + lamda;
        im = Complex.I * mu;
        h =z1 -z2  ;

        //var KMatr = new Memoize<Tuple<Complex, Complex, double, double>, CSqMatrix>((Tuple<Complex, Complex, double, double> t) => K(t.Item1, t.Item2, t.Item3, t.Item4)).Value;
        //KMatrix = (Complex a1, Complex a2, double z, double w) => KMatr(new Tuple<Complex, Complex, double, double>(a1, a2, z, w));

        var del = new Memoize<Tuple<Complex, Complex, double>, CSqMatrix>((Tuple<Complex, Complex, double> t) => delta(t.Item1, t.Item2, t.Item3)).Value;
        Delta = (Complex a1, Complex a2, double w) => del(new Tuple<Complex, Complex, double>(a1, a2, w));
        var det= new Memoize<Tuple<Complex, Complex, double>, Complex>((Tuple<Complex, Complex, double> t) => delta(t.Item1, t.Item2, t.Item3).Det).Value;
        DeltaDet = (Complex a1, Complex a2, double w) => det(new Tuple<Complex, Complex, double>(a1, a2, w));

        //var bes = new Memoize<Tuple<Complex,double, double>, Complex[]>((Tuple<Complex, double, double> t) => _Bessel(t.Item1, t.Item2,t.Item3)).Value;
        //Bessel = (Complex a,double x,double y) => bes(new Tuple<Complex, double, double>(a,x,y));
        Bessel = new Func<Complex, double, double, Complex[]>(_Bessel);

        var han = new Memoize<Tuple<double, double>, Complex>((Tuple<double, double> t) => Computator.NET.Core.Functions.SpecialFunctions.Hankel1(t.Item1, t.Item2)).Value;
        Hankel = (double n, double x) => han(new Tuple<double, double>(n, x));

        prmsnmem = new Memoize<Tuple<Complex, double, double>, Complex[]>((Tuple<Complex, double, double> t) => _PRMSN(t.Item1, t.Item2, t.Item3));
        var prmsn = prmsnmem.Value;
        PRMSN = (Complex a, double z, double w) => prmsn(new Tuple<Complex, double, double>(a, z, w));
        //PRMSN = new Func<Complex, double, double, Complex[]>(_PRMSN);

        var pol = new Memoize<double, Vectors>((double t) => _PolesPoles(t)).Value;
        PolesPoles = (double x) => pol(x);
    }
    public static Memoize<Tuple<Complex, double, double>, Complex[]> prmsnmem;

    public static double lamda = 51, mu = 26.3, ro = 2.7,  h;
    private static double ml2,z1 = 0, z2 = -2;
    private static Complex im;

    public static RealFunc k1 = (double w) => ro * w*w / ml2;
    public static RealFunc k2 = (double w) => ro * w * w / mu;
    private static Func<Complex, Complex, Complex> als = (Complex al1, Complex al2) => al1.Sqr() + al2.Sqr();
    public static Func<Complex, double, Complex> sigma = (Complex a, double kw) =>
      {
          //if (a.Abs > kw) return Complex.Sqrt((a - kw));
          //Complex tmp = Complex.I * Complex.Sqrt((kw - a));
          //if (a.Im * a.Re > 0) return tmp;
          //return -tmp;

          //Complex tmp = a - kw;
          //if (a.Abs >= kw) return Complex.I * Complex.Sqrt(tmp);
          //return -Complex.Sqrt(-tmp);

          if (a.Abs > kw) return Complex.Sqrt((a - kw));
          Complex tmp = -Complex.I * Complex.Sqrt((kw - a));
          if (a.Im * a.Re > 0) return tmp;
          return -tmp;
      };

    private static Func<Complex, Complex, double, CSqMatrix> delta = (Complex a1, Complex a2, double w) =>
        {
            double kt1 = k1(w), kt2 = k2(w);
            Complex al = als(a1, a2);
            Complex s1s = al - kt1, s2s = al - kt2, s1 = sigma(al, kt1), s2 = sigma(al, kt2);

            Complex a = -lamda * al + ml2 * s1s;
            Complex b = im * al * 2 * s1;
            Complex c = 2 * mu * al * s2;
            Complex d = -im * al * (s2s + al);
            Complex e11 = Complex.Exp(s1 * z1), e12 = Complex.Exp(s1 * z2), e21 = Complex.Exp(s2 * z1), e22 = Complex.Exp(s2 * z2);

            return new CSqMatrix(new Complex[,] {
                 { a*e11,a/e11,c*e21,-c/e21},
                 { -b*e11,b/e11,d*e21,d/e21},
                 {a*e12,a/e12,c*e22,-c/e22 },
                 {-b*e12,b/e12,d*e22,d/e22 }
             });
        };
    /// <summary>
    /// Функция, возвращающая матрицу, чей определитель есть знаменатель delta
    /// </summary>
    public static Func<Complex, Complex, double, CSqMatrix> Delta;
    /// <summary>
    /// Функция, возвращающая матрицу, чей определитель есть знаменатель delta
    /// </summary>
    public static Func<Complex, Complex, double, Complex> DeltaDet;

    /// <summary>
    /// Функция знаменателя, выраженная явно
    /// </summary>
    public static Func<Complex, double, Complex> Deltass = (Complex alp, double w) =>
        {
            double kt1 = k1(w), kt2 = k2(w);
            Complex al = alp*alp;
            Complex s1 = sigma(al, kt1), s2 = sigma(al, kt2);

            Complex a = 2*mu*al-ml2*kt1;
            Complex b = /*im * al **/ 2 * s1;
            Complex c = 2 * mu * al * s2;
            Complex d = -/*im * al **/ (2*al-kt2);

            Complex ad = a * d,bc=b*c;

            //Complex e1 = Complex.Exp(h * s1), e2 = Complex.Exp(h * s2),e12=e1/e2,e21=1.0/e12,e12n=e1*e2,e21n=1.0/e12n;
            return 4 * ad * bc  - Complex.Ch(h*(s1+s2)) * (ad + bc).Sqr()+ Complex.Ch(h*(s1-s2))*(ad - bc).Sqr();

        };
    /// <summary>
    /// Дополнительный знаменатель, полученный от N
    /// </summary>
    public static Func<Complex, double, Complex> DeltassN = (Complex alp, double w) =>
    {
        double  kt2 = k2(w);
        Complex al = alp * alp;
        Complex  s2 = sigma(al, kt2);
        return  Complex.Sh(s2 * h);
    };
    /// <summary>
    /// Явные корни N
    /// </summary>
    /// <param name="omega"></param>
    /// <param name="tmin"></param>
    /// <param name="tmax"></param>
    /// <returns></returns>
    public static Vectors DeltassNPosRoots(double omega,double tmin, double tmax)
    {
        List<double> list = new List<double>();
        double alp,pi2=(Math.PI/h).Sqr(),kappa=k2(omega),s=kappa;//kappa.Show();
        int k = 1;
        do
        {
            alp = Math.Sqrt(s);
            list.Add(alp);
            s = kappa - pi2 * k * k;//if (omega > 13) s.Show();
            k++;
        }
        while (/*alp <= tmax && alp >= tmin &&*/ s >= 0);//new Vectors(list.ToArray()).Show();
        return new Vectors(list.Where(n=>n>=tmin&&n<=tmax).ToArray());
    }

    /// <summary>
    /// Функция знаменателя, выраженная явно (при глобальной частоте)
    /// </summary>
    public static ComplexFunc Deltas = (Complex a) => Deltass(a, РабКонсоль.w);


    /// <summary>
    /// Матрица Грина как функция от двух альф, z и частоты
    /// </summary>
    //public static Func<Complex, Complex, double, double, CSqMatrix> KMatrix;
    public static Func<Complex, double, double, double, double, CSqMatrix> K = (Complex a,double x,double y, double z, double w) =>
          {
              var c = PRMSN(a, z, w);
              var b = Bessel(a, x, y);

              Complex P = c[0], R = c[1], M = c[2], S = c[3], N = c[4];
              Complex jx = b[0], jy = b[1], jxx = b[2], jxy = b[3], jyy = b[4];
              Complex i = Complex.I;

              return new CSqMatrix(new Complex[,] {
                 {i*(M*jxx+N*jyy),i*(M-N)*jxy, -P*jxx},
             { i*(M-N)*jxy,i*(M*jyy+N*jxx),-P*jyy},
             { -i*S*jx,-i*S*jy,R}
              });
          };


    //private static Complex littleBessel(int v, Complex a) => МатКлассы2019.SpecialFunctions.MyBessel(v, a);
    private static Complex littleBessel1(Complex a) => МатКлассы2019.SpecialFunctions.MyBessel(1, a);
    private static Complex littleBessel2( Complex a) => МатКлассы2019.SpecialFunctions.MyBessel(2, a);
    private static Func<Complex, Complex> LittleBessel1,LittleBessel2;

    private static Complex[] _Bessel(Complex a,double x,double y)
    {
        Complex jx, jy, jxx, jxy, jyy;
        double x2=x*x,y2=y*y, r = Math.Sqrt(x2 + y2);
        Complex ar = a * r,ar3=ar*r*r;
        Complex j1ar =/*LittleBessel1(ar) */МатКлассы2019.SpecialFunctions.MyBessel(1, ar),j2ar=/*LittleBessel2(ar)*/ МатКлассы2019.SpecialFunctions.MyBessel(2, ar);

        //(a).Show();

        jx = -j1ar * x / r;
        jy = -j1ar * y / r;
        jxx = j2ar * x2 / r / r - j1ar * (x2 + a * y2) / ar3;
        jyy= j2ar * y2 / r / r - j1ar * (y2 + a * x2) / ar3;
        jxy = j2ar * x * y / r / r + j1ar / ar3 * (a - 1);
        //$"{jx} {jy} {jxx} {jxy} {jyy}".Show();
        return new Complex[] { jx, jy, jxx, jxy, jyy };
    }
    /// <summary>
    /// Возвращает первые и вторые производные функции J0(alpha,x,y), мемоизируется
    /// </summary>
    public static Func<Complex, double,double, Complex[]> Bessel;
    public static Func<double, double, Complex> Hankel;

    /// <summary>
    /// Возвращает первые два столбца из обратной матрицы
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Tuple<CVectors,CVectors> Arev(Complex al,double w)
    {
        Complex alp = al * al,ima=Complex.I*mu*alp;
        Complex s1=sigma(alp,k1(w)),s2=sigma(alp,k2(w));
        Complex q = Complex.Exp(s1 * h),qp= Complex.Exp(s2 * h), qm=1.0/qp;
        Complex sh = (qp - qm) * 0.5, ch = (qp + qm) * 0.5;

        Complex a = 2 * mu * al - ml2 * k1(w);
        Complex b = 2 * s1;
        Complex c = 2 * mu * al * s2;
        Complex d = -(2 * al - k2(w));

        Complex ad = a * d;
        Complex bc = b * c;
        Complex cq = c / q,dq=d/q;

        Complex[] m1 = new Complex[4], m2 = new Complex[4];
        m1[0] = d * (bc - q*(ad*sh+bc*ch));
        m2[0] = dq * (-bc*q+(bc*ch-ad*sh));
        m1[1] =c*(ad-q*(ad*ch+bc*sh))*ima;
        m2[1] =cq*(ad*q+(bc*sh-ad*ch)) * ima;
        m1[2] =d*(bc*q+ad*sh-bc*ch);
        m2[2] =dq*(-bc+q*(ad*sh+bc*ch));
        m1[3] =c*(ad*q-ad*ch+bc*sh) * ima;
        m2[3] =cq*(ad-q*(ad*ch+bc*sh)) * ima;

        return new Tuple<CVectors, CVectors>(new CVectors(m1), new CVectors(m2));
    }

    /// <summary>
    /// Возвращает компоненты PRMSN, нужные для матрицы Грина
    /// </summary>
    /// <param name="al"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public static Complex[] _PRMSN(Complex al, double z,double w)
    {
        var c = Arev(al, w);
        CVectors v1 = c.Item1, v2 = c.Item2;
        Complex alp = al * al,s1=sigma(alp,k1(w)), s2 = sigma(alp, k2(w)),e1=Complex.Exp(s1*z),e2=Complex.Exp(s2*z);
        CVectors c1=new CVectors(new Complex[] { e1, 1.0 / e1, s2 * e2, -s2 / e2 }),c2=new CVectors(new Complex[] { s1*e1, -s1 / e1, alp* e2, alp / e2 });

        Complex P = v1 * c1;
        Complex R = v1 * c2;
        Complex M = v2 * c1;
        Complex S = v2 * c2;
        Complex N = Complex.Ch(s2 * (z + h))/s2/im/alp/Complex.Sh(s2*h);

        Complex del = Deltass(al, 2);

        return new Complex[] { P/del, R/del, M/del, S/del, N };
    }

    /// <summary>
    /// Возвращает компоненты PRMSN, нужные для матрицы Грина (мемоизированная)
    /// </summary>
    /// <param name="al"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public static Func<Complex, double, double, Complex[]> PRMSN;

    /// <summary>
    /// Возвращает массив полюсов при такой-то частоте
    /// </summary>
    private static Func<double, Vectors> _PolesPoles = (double w) =>
       {
           ComplexFunc del = (Complex a) => Deltass(a, w);
          Vectors v1= Roots.OtherMethod(del,  РабКонсоль.polesBeg, РабКонсоль.polesEnd, РабКонсоль.steproot, РабКонсоль.epsroot, Roots.MethodRoot.Brent, true);
           Vectors v2 = DeltassNPosRoots(w, РабКонсоль.polesBeg, РабКонсоль.polesEnd);
           return new Vectors(Expendator2.Distinct(v1.DoubleMas, v2.DoubleMas));
       };
    /// <summary>
    /// Мемоизированная _PolesPoles
    /// </summary>
    public static Func<double, Vectors> PolesPoles;

}