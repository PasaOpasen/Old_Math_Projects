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
using static Курсач.KursMethods.TestFuncAndCurve;
using System.Collections;
using Работа2019;
using static FSlib.testforbook;

namespace Консоль
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            // Point p_ = new Point(0,1);
            // Point p0 = new Point(2,7);
            // Point p1 = new Point(-1, -2);
            // Point[] s = { p_, p0, p1 };
            // //Point p2 = new Point(p0);
            // //Console.WriteLine("{0}   {1}   {2}  {3}",p_.x,p0.x,p1.y,p2.y);
            // Console.ReadLine();
            // double[] a = { 1, 2, 1 };
            // Polynom e = new Polynom(s);
            // Polynom p = new Polynom(2,-1);
            // Polynom q = new Polynom(1, 1);
            // p.Show();
            // q.Show();
            // Polynom e = p-q;
            //Polynom e = new Polynom(Math.Sin,8,-2,2);
            //e.Show();
            //Console.WriteLine("{0}  ", Math.Sin(0.4)-e.Value(0.4));
            //Console.ReadLine();

            //StreamReader fs = new StreamReader(@"C:\1.txt");
            //StreamReader fe = new StreamReader(@"C:\2.txt");
            //string s = "";
            //while (s != null)
            //{
            //    s = fs.ReadLine();
            //    Console.WriteLine(s);
            //}

            //Polynom p = new Polynom(fe);
            //Kind g = Kind.SecondKind;
            //Polynom p = new Polynom(Polynom.Cheb(g,3));
            //Polynom q = new Polynom(Polynom.Cheb(g,1));
            //Polynom a = p / q;
            //Polynom b = p % q;
            //Polynom c = b + a * q;
            //p.Show();
            //q.Show();
            //a.Show();
            //b.Show();
            //c.Show();
            //Console.WriteLine(DefInteg.MiddleRect(Math.Cos, 0, 2*Math.PI));
            //Console.WriteLine(DefInteg.Trapez(Math.Cos, 0, Math.PI*2));
            //Console.WriteLine(DefInteg.Simpson(Math.Cos, 0, Math.PI*2));

            //Kind g = Kind.SecondKind;
            //Polynom p = new Polynom(Polynom.Cheb(g, 3));
            //Polynom a = p |5;
            //p.Show();
            //a.Show();

            //SqMatrix M = new SqMatrix();
            //SqMatrix r =p.Value(M);r.PrintMatrix();


            //M.CreateMatrix();
            //M.PrintMatrix();

            ////StreamReader f1 = new StreamReader(@"F:\1.txt");

            ////Probability.DisRandVal p = new Probability.DisRandVal(f1);
            ////Probability.DisRandVal.NerCheb(p, 0.5);


            ////Console.WriteLine(Math.Atan(2)-Math.Atan(0));
            //Probability.DisRandVal q = p;
            //q.Show();
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.MatExp(q)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.Dispersion(q)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Math.Sqrt(Probability.DisRandVal.Dispersion(q))).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.BegM(q,2)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.BegM(q, 3)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.BegM(q, 4)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.CenM(q, 2)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.CenM(q, 3)).ToStringMixed());
            //Console.WriteLine(Number.Rational.ToRational(Probability.DisRandVal.CenM(q, 4)).ToStringMixed());
            //Probability.DisRandVal t = (q - q.M) ^ 1;t.Show();

            /*Number.Rational j = new Number.Rational(7.8);
            Console.WriteLine(j.ToDouble()); j.ShowWrong(); j.ShowMixed();*/

            //Console.WriteLine(Probability.DisRandVal.CenM(q, 1));
            //StreamReader f1 = new StreamReader(@"C:\1.txt");
            //StreamReader f2 = new StreamReader(@"C:\3.txt");
            //SqMatrix M = new SqMatrix(f1); SqMatrix N = new SqMatrix(f2);
            //Graphs g1 = new Graphs(M); Graphs g2 = new Graphs(N);
            //Console.WriteLine(Graphs.Isomorphism(g1, g2));
            //Console.WriteLine(Graphs.Gomeomorphism(g1, g2));
            //Console.WriteLine(Graphs.Connectivity(g1));

            /*SqMatrix M = new SqMatrix(fe);
            M.PrintMatrix();
            SqMatrix A = M.Reverse*M;
            A.PrintMatrix();*/
            //M.Reverse.PrintMatrix();
            //Console.WriteLine(M.Track());
            //Polynom.CharactPol(M).Show();
            //M.CharactPol.Show();
            /*StreamWriter fs = new StreamWriter(@"C:\1.txt");
            fs.WriteLine("1 2 3 4 5 6 5 4");
            fs.Close();

            StreamReader fe = new StreamReader(@"C:\1.txt");
            string s= fe.ReadLine();
            Console.WriteLine(s);
            fe.Close();
            SLAU M = new SLAU(fs);*/
            /*Polynom M = Polynom.Lezh(6)*16, N=M.ToLeadPolynom();
            M.Show();
            N.Show();
            
            StreamReader fs = new StreamReader(@"C:\1.txt");
            SLAU M = new SLAU(fs);
            M.Show();

            Console.WriteLine(M.A[1][1]);*/

            //double r=DefInteg.ImproperFirstKind((double x) => { return (1 / (x * x + 1)); });
            //Console.WriteLine(r);
            //Probability.ConRandVal e = new Probability.ConRandVal((double x) => { if (x < 1 || x > 13) return 0; return 1.0/12; });
            //Probability.ConRandVal r = e^3;
            //e.Show();
            //r.Show();

            //RealFunc f = (double x) => { return x * Math.Sin(x); };
            //SequenceFunc p = (double x, int n) => { return Polynom.Cheb(Kind.FirstKind ,n).Value(x); };

            //RealFunc g = FuncMethods.Approx(f, p, SequenceFundKind.Other, 4, -1, 1);

            //Console.WriteLine(FuncMethods.RealFuncMethods.NormDistance(f, g, -1, 1));

            //RealFunc w = (double x) => { return x; }, e = (double x) => { return 1 / x; };
            //Console.WriteLine(FuncMethods.RealFuncMethods.ScalarPower(w, e, -1, 1));
            //Console.WriteLine(g(1));

            //Point[] mas = new Point[5];
            //mas[0] = new Point(0, 1);
            //mas[1] = new Point(1, -5);
            //mas[2] = new Point(2, 0);
            //mas[3] = new Point(3, 1);
            //mas[4] = new Point(4, 1);
            //Polynom p = new Polynom(mas);p.Show();
            //Console.WriteLine(Polynom.CubeSpline(mas)(0));

            //Polynom.PolynomTestShow(Math.Cos, 16, -2, 2, 11, 4, 4);

            //double[] c = { -3,-2,0, 1, 2, 3, 6 };
            //Polynom.PolynomTestShow(Math.Cos, c, -2, 2, 3, 3, 4);

            //Polynom p = Polynom.Neu(mas);
            //p.Show();
            //Polynom q = Polynom.Lag(mas);
            //q.Show();
            //Console.WriteLine(Polynom.R(mas, 3, 1, 3)(1));
            //Polynom p = new Polynom(mas);
            //Polynom e = p.AddPoint(new Point(3, -4));

            //Point.Show(mas);
            //Point.Show(p.points);
            //Point.Show(e.points);
            //p.Show();
            //e.Show();
            //Console.WriteLine(e.Value(0));

            //Polynom r = new Polynom(1, 0, 1, 2);
            //r.Show();
            //Console.WriteLine(r.S(-6,2));



            // RealFunc f = (double t)=>t*t*t;

            //double x = 3.141592653;
            //double Mn;
            //Console.WriteLine("Введите Mn");
            //string s = Console.ReadLine();
            //Mn = Convert.ToDouble(s);


            //Console.WriteLine("Sin(x)={0} P(x)= {1}", f(x),r.Value(x));
            //Point a;

            //Polynom t = r.AddPoint();
            //Console.WriteLine("{0} <= {1}",Math.Abs(f(x)-r.Value(x)),Polynom.wn(f, 4, 2.8, 3.2, x,Mn));


            //Polynom.LagEstimateErr(f, 3, 2.8, 3.2, x, Mn);

            //StreamReader fs = new StreamReader(@"F:\00.txt");
            //Matrix t = new Matrix(fs);t.PrintMatrix();
            //Vectors v;
            //UnderUncertainty.AverageGain(t, out v);
            //UnderUncertainty.MiniMax(t, out v);
            //UnderUncertainty.MaxiMax(t, out v);
            //UnderUncertainty.Laplas(t, out v);
            //UnderUncertainty.Vald(t, out v);
            //UnderUncertainty.Savage(t, out v);
            //UnderUncertainty.Hurwitz(t, out v);
            //UnderUncertainty.HodgeLeman(t, out v);
            //UnderUncertainty.Germeier(t, out v);
            //UnderUncertainty.Powers(t, out v);

            //StreamReader fs = new StreamReader(@"D:\7.txt");
            //SLAU t = new SLAU(fs);
            //t.ShowErrors();
            //Polynom.PolynomTestShow(f, 3, 2.8, 3.2);
            //RealFunc g= Polynom.

            // Point[] p = Point.Points(f, 4, 2.8, 3.2);
            //Console.WriteLine("{0} <= {1}",Math.Abs(f(x)-r.Value(x)),Polynom.wn(f, 4, 2.8, 3.2, x,Mn));

            //Console.WriteLine();
            //Polynom.PolynomTestShow(f,4,2.8,3.2,1,2,1);

            //Vectors v = new Vectors(4, 2, 2, 2, 0, 2);
            //Polynom p = new Polynom(v);
            //Polynom q = p | -6;
            //Polynom w = q | 6;
            //p.Show();
            //q.Show();
            //w.Show();

            //Vectors p = new Vectors(0, 0, 1, 1,1,1);
            //StreamReader fs = new StreamReader(@"F:\3.txt");
            //StreamWriter sf = new StreamWriter(@"F:\3+.txt");

            //Console.WriteLine(Vectors.SimplexInteger(ref p, fs, sf));

            //StreamWriter sf = new StreamWriter(@"F:\Решен.txt");
            //Vectors.SimpleSimplex(ref p, fs);
            //double r = 98.12331;
            //Console.WriteLine(r==(int)r);
            //double k=Vectors.SimplexInteger(ref p, fs);


            //Polynom p = new Polynom(2, 0.11, 2, 3, 4.5, 0, 0); p.ShowRational();

            //Number.Rational t = new Number.Rational(-8.58799999999);
            //t.ShowMixed();

            //Vectors o = p / 20;o.Show();

            //Vectors p = new Vectors(0.01,0.0098,0.0098,0.0097,0.0094,0.0097);
            //Vectors q = new Vectors(3, 4, 5);
            //Vectors e = new Vectors(5, 6, 7);
            //Vectors r = new Vectors(7, 8,1);
            //Vectors.Merge(p, q, e,r).Show();
            //Console.WriteLine(p.Contain(89));
            //Console.WriteLine(Vectors.IsSimpleCycle(p));

            //Vectors p = new Vectors(0.01, 0.0098, 0.0098, 0.0097, 0.0094, 0.0097);
            //p.RelAcVec.Show();
            //p.RelAcSqr.Show();
            //p.TrueValShow();



            //t.ProRace();
            //t.Show();
            //Console.WriteLine(t.Discrep);
            //t.Show();

            //Graphs.Type s = Graphs.Type.Full;
            //SqMatrix t = new SqMatrix(fs);


            //MultipleKnot a=new MultipleKnot(1,new double[] { 1,2,3});
            //MultipleKnot b = new MultipleKnot(2, new double[] { 1});
            //MultipleKnot c = new MultipleKnot(3, new double[] { -1,7});
            //Polynom p = Polynom.Hermit(a, b, c);
            //Polynom q = p | 1;
            ////p.Show();
            //Console.WriteLine(p.Value(3));
            //Console.WriteLine(q.Value(3));




            //r.ShowInfoFile();

            //SqMatrix t = new SqMatrix(fs);
            //t.PrintMatrix();
            //t.SubMatrix(1,7,8).PrintMatrix();

            //Console.WriteLine(r.ContainCycle());
            //r.ShowAllCycles(4);
            // Console.WriteLine(r.GetCycleExample(4));

            //r.Addition.B.PrintMatrix();
            //r.Chain(1, 7).Show();

            //Number.Rational t = new Number.Rational(3);
            //t.ShowMixed();
            //Console.WriteLine(Number.Rational.IsFractional(t));

            //StreamReader fs = new StreamReader(@"F:\5.txt");
            //Vectors v = new Vectors(fs);
            //StreamWriter fe = new StreamWriter(@"F:\51.txt");
            //Vectors.ShowInfo(v, fe);

            //RealFunc f = Math.Cos;
            //FuncMethods.DefInteg.Demonstration(f, 0, Math.PI/2);

            //Polynom p = Polynom.Hermit(5);p.Show();
            //RealFunc a = Math.Exp;
            //RealFunc b = (double x) => 1.0 / Math.Exp(x);
            //Console.WriteLine(FuncMethods.RealFuncMethods.ScalarPower(a, b, 3, 5));

            //List<Point> P = new List<Point>();
            //P.Add(new Point(1, 2));
            //P.Add(new Point(1, 3));
            //FuncMethods.NetFunc f = new FuncMethods.NetFunc(P);
            //f.Show();

            //RealFunc p = f.Spline;
            //Console.WriteLine(p(2.1));

            //SequenceFunc t = FuncMethods.Lezhandrs(-1, 1);
            //RealFunc f = (double x) => t(x, 3);
            //RealFunc g = (double x) => t(x, 4);
            //Console.WriteLine(FuncMethods.RealFuncMethods.ScalarPower(f, g, -1, 1));
            //Console.WriteLine(FuncMethods.RealFuncMethods.ScalarPower(Polynom.Lezh(3).Value, Polynom.Lezh(4).Value, -1, 1));

            //Console.WriteLine(Polynom.ScalarP(new Polynom(new double[] {1,0,3 }),new Polynom(new double[] { 0, 0, 3 }),0,1));

            //SequenceFunc y = FuncMethods.Lezhandrs(-1, 1);
            //RealFunc u = (double x) => t(x, 3);
            //Polynom p = Polynom.Lezh(3);
            //Console.WriteLine(p.Value(0)-u(0));

            //Polynom p = new Polynom(new double[] { 1, 0, 2 }); p.Show();
            //Polynom q = new Polynom(new double[] { 1, 1 }); q.Show();
            //Polynom z = p.Value(q); z.Show();
            //SequencePol l = FuncMethods.Lezhandr(-3,5);
            //Console.WriteLine(Polynom.ScalarP(l(2), l(8), -3, 5));

            //RealFunc f = (double t) => t * t * t;
            //double x = 0.5;
            //double[] g = { 1, 2, 3, 4 };
            //Point[] h = Point.Points(f, g);
            //Polynom.ShowNeuNew(h, f, x);

            //StreamReader fs = new StreamReader(@"F:\2.txt");
            //Graphs r = new Graphs(fs);
            //r.ShowInfoConsole();

            //StreamReader fs = new StreamReader(@"F:\7.txt");
            //SLAU t = new SLAU(fs);
            //t.Show();
            //t.ShowErrors();

            //SequenceFunc p = new SequenceFunc((double x, int i) => { return Math.Pow(x, i); });
            //RealFunc f = Math.Sin;
            //FuncMethods.ShowApprox(f, new double[] { -1, -0.5,0,0.5, 1, 2,2.5, 3,4 }, p, SequenceFuncKind.Other,6);
            //RealFunc g = FuncMethods.Approx(f, FuncMethods.Monom, SequenceFuncKind.Other, 3, -2, 2);
            //RealFunc r = FuncMethods.Approx(f, FuncMethods.Monoms, SequenceFuncKind.Other, 3, -2, 2);


            //StreamReader fs = new StreamReader(@"F:\2.txt");
            //Matrix M = new Matrix(fs);

            //M.AddByColumn(2, -2);M.AddByColumn(3, -2); M.AddByColumn(5, -2); M.AddByColumn(7, -2);
            //M.AddByLine(2, 2); M.AddByLine(5, 2);

            //M.AddByColumn(4, -1); M.AddByColumn(5, -1); M.AddByColumn(7, -1);
            //M.AddByLine(2, 1); M.AddByLine(5, 1); M.AddByLine(7, 1);

            //M.AddByColumn(2, -1); M.AddByColumn(3, -1); M.AddByColumn(4, -1);
            //M.AddByLine(1, 1); M.AddByLine(6, 1); M.AddByLine(7, 1);

            //M.PrintMatrix();
            //-----------------------------------------------------------------------------------------------------------------------------------------------------


            //double a, b;
            //Console.WriteLine("Введите начало отрезка интегрирования ");
            //a = Convert.ToDouble(Console.ReadLine());
            //Console.WriteLine("Введите конец отрезка интегрирования ");
            //b = Convert.ToDouble(Console.ReadLine());
            //Console.WriteLine("Введите колическтво шагов n, где n - четное ");
            //int n = int.Parse(Console.ReadLine());
            //Console.WriteLine("Введите eps ");
            //double eps = Convert.ToDouble(Console.ReadLine());
            //RealFunc F = (double t) => t*t + t*t*t;//Math.Sin(t);
            ////double res = FuncMethods.DefInteg.Simpson(F, a, b);
            //double res = FuncMethods.DefInteg.DefIntegral(F, a, b, FuncMethods.DefInteg.Method.Simpson, 
            //    FuncMethods.DefInteg.Criterion.StepCount, n, eps);
            //Console.WriteLine(res);


            //double a = 0, b = Math.PI;
            //b.Show();
            //FuncMethods.DefInteg.MonteKarloEnum KK = FuncMethods.DefInteg.MonteKarloEnum.Usual;
            //МатКлассы2018.Point p = new МатКлассы2018.Point(a, b);
            //Point e = new Point(a, b);
            //MultiFunc F = (double[] t) => Math.Cos(t[0]) * Math.Sin(t[1]);
            //double res = FuncMethods.DefInteg.MonteKarlo(F, KK, p, e);
            //Console.WriteLine(res);

            //string y = "1,2,45,tr,we";
            //y.Replace("we", "x");
            //Console.WriteLine(y);

            //string s = "2*x+cos(x-2)+ln(x+12)";
            //Parser r = new Parser(8, s);
            //Func<double, double, double> f1 = (double x, double y) => x * y;
            //Func<double, double, double> f2 = (double x, double y) => x + y;
            //Func<double, double, double> f3 = (double x, double y) => x - y;
            //Func<double, double, double> f4 = (double x, double y) => x /y;
            //Func<double, double, double>[,] F = { { f1, f2 }, { f3, f4 } };
            //FuncMethods.MatrixFunc<double, double> G = new FuncMethods.MatrixFunc<double, double>(F);
            //(-G)[5, 2].PrintMatrix();

            //RealFunc f = (double x) => 1 / x / x;
            //FuncMethods.DefInteg.ImproperFirstKindInf(f, 1).Show();

            //double p = 1, w = 0.4, l = 1.94117647058824, m = 1;
            //double x1 = p * w * w / (l + 2 * m), x2 = p * w * w / m;

            //ComplexFunc f = (Complex z) => (z - 4) * (z - 2) * (z-3-2*Complex.I)*(z-4-4*Complex.I);
            //ComplexFunc porabol = (Complex z) => -(z - 2) * (z - 2);
            //ComplexFunc sin = Complex.Sin;
            //ComplexFunc t = (Complex z) => -Complex.Pow(z * z - 0.5 * x2,2) + z * z * Complex.Sqrt((z * z - x1)*(z * z - x2));
            //ComplexFunc d = (Complex z) =>
            //{
            //    Complex s1 = Complex.Sqrt(z * z - x1), s2 = Complex.Sqrt(z * z - x2), g2 = z * z - 0.5 * x2;
            //    double h = 1;
            //    return 2 * m * (-2 * z * z * s1 * s2 * g2 * g2
            //     - (Complex.Pow(g2, 3) + Complex.Pow(z, 4) * (z * z - x1) * (z * z - x2)) * Complex.Sh(s1) * h * Complex.Sh(s2) * h
            //     + 2 * z * z * s1 * s2 * g2 * g2 * h * h * Complex.Ch(s1) * Complex.Ch(s2));
            //};

            //Complex a = new Complex(-Math.PI, 0);
            //Complex b = new Complex(Math.PI*4, 0);

            //Complex z1 = new Complex(-2), z2 = new Complex(1, 1), z3 = new Complex(2, 3);
            //Complex q = Muller(d, z1, z2, z3);
            //Console.WriteLine($"q={q} |f(q)|={d(q).Abs}");

            //Presentaion(sin, a, b, 0.01,ModifyFunction.No);
            //Complex[] k = Bisec(sin, a, b); k.Show();
            //Complex[] k = FullMuller(d, a, b);
            //Complex[] k = Chord(d, a, b);
            //Complex[] k = Neu(d, a, b);
            //Complex[] k = ChordNeu(d, a, b);
            //Console.WriteLine("Найденные корни:");
            //ExistMin(sin, -Math.PI, 0).Show();

            //RealFuncOfCompArg module=(Complex z)=> sin(z).Re;
            //Complex[] k=MinSearch(module, a, b, MinimumVar.GoldSection,0.01);
            //k.Show();

            //SearchRoots(f,  new Complex(5, 5), new Complex(0, 5), new Complex(0),new Complex(5, 0), RootSearchMethod.Chord,0.01,0.1).Show();


            //string s = "В греческом письме он используется более тысячи лет. В латыни пробел иногда встречался в древности, потом исчезал и вернулся тоже около тысячи лет назад. В древнейших славянских памятниках пробел также отсутствует (как в глаголице, так и в кириллице); регулярно и в нынешнем смысле используется в кириллице только с";
            //Coding.Hacking(s).Show();
            //string t = Coding.Vert(s, true,"16324578");
            //t.Show();
            //Coding.Vert(t, false, "16324578").Show();
            //s.Swap('a', 'f').Show();
            //VRealFunc f = (double x, Vectors v) =>new Vectors(Math.Cos(x),2*x);
            //VectorNetFunc func = МатКлассы2018.FuncMethods.ODU.ODUsearch(f, new Vectors(0,0), 0, 1, 0.01, МатКлассы2018.FuncMethods.ODU.Method.E1, 0.001, false);
            //string s = "12345 ясен хуй сука блять";
            //Coding.Vernam(s, true).Show();

            //double rr = 5;
            //Curve c = new Curve(0, 2 * Math.PI, (double t, double r) => r * Math.Cos(t), (double t, double r) => r * Math.Sin(t), rr);
            //c.S = (double tx, double ty, double r) => tx * ty * r; c.End = (double r) => 2 * Math.PI;
            //Functional f = (Point t) => Math.Sqrt(t.x * t.x + t.y * t.y)/*t.x*t.y*//*Math.Exp(-t.x*t.x-t.y*t.y)*/;
            ////for (int k = 10; k <= 10000; k *= 10) Demonstration(f, c, c.S, out double[] pog,out TimeSpan[] time,0.001, k, true, 2 * Math.PI / 3, "(Point t) => Math.Sqrt(t.x*t.x+t.y*t.y)");
            //f = (Point t) => /*Math.Sqrt(t.x * t.x + t.y * t.y)*/t.x * t.y/*Math.Exp(-t.x*t.x-t.y*t.y)*/;
            ////for (int k = 10; k <= 10000; k *= 10) Demonstration(f, c, c.S, out double[] pog, out TimeSpan[] time, 0.001, k, true, 0, "(Point t) => t.x*t.y");
            ////f = (Point t) => /*Math.Sqrt(t.x * t.x + t.y * t.y)*//*t.x*t.y*/Math.Exp(-t.x * t.x - t.y * t.y);
            ////for (int k = 10; k <= 10000; k *= 10) Demonstration(f, c, c.S, out double[] pog, out TimeSpan[] time, 0.001, k, true, Math.PI * (1 - Math.Exp(-rr * rr)), "(Point t) => Math.Exp(-t.x*t.x-t.y*t.y)");
            ////double i = DoubleIntegral(f, c, c.S, Method.GaussKronrod15, 0.001, 1000);
            ////(/*2*Math.PI/3*/0/*Math.PI*(1-Math.Exp(-rr*rr))*/ - i).Show();
            ////f = (Point t) => 1;

            //int[] cy = new int[] { 10,50,100,250,500};
            ////DemostrationToExcel(f, c, c.S, cy);

            //string s = Console.ReadLine(),ss= Console.ReadLine();
            //string[] st =Expendator.Union( s.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries), ss.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            //double[] k = new double[st.Length];
            //for(int i=0;i<st.Length;i++)      
            //    k[i] = i + 1;
            ////st.Show();
            //ИнтеграцияСДругимиПрограммами.CreatTableInExcel(k,st.ToDoubleMas());

            //int k = 10;
            //double[] arg = new double[15], sin = new double[15], cos = new double[15];
            //for (int i = 10; i <= 150; i += k)
            //{
            //    arg[i / 10 - 1] = 10 * i;
            //    sin[i / 10 - 1] = (FuncMethods.DefInteg.DoubleIntegral(f, c, c.S, Method.GaussKronrod15, cy: 10 * i, makesort: false) - 0).Abs();
            //    cos[i / 10 - 1] = (FuncMethods.DefInteg.DoubleIntegral(f, c, c.S, Method.GaussKronrod15, cy: 10 * i, makesort: true) - 0).Abs();
            //}
            //ИнтеграцияСДругимиПрограммами.CreatTableInExcel(arg, new Vectors(sin), new Vectors(cos));

            //double r = 2;
            //Curve c = new Curve(0, Ends[3](r), (double t) => U[3](t, r), (double t) => V[3](t, r), r, U[3], V[3], T[3], Ends[3]);
            //double i = DoubleIntegral((Point p) => 1, c, c.S, Method.GaussKronrod15, 0.001, 100);
            //(i - r * r * (Math.PI / 3 - Math.Sqrt(3) / 4)).Show();

            //RealFunc f = (double x) => 8+2*x-x*x;
            //TimeSpan t = new TimeSpan();
            //FuncMethods.DefInteg.GaussKronrod.Integral(f,-2, 4).Show();(t - new TimeSpan()).Ticks.Show();t = new TimeSpan();
            //FuncMethods.DefInteg.GaussKronrod.MySimpleGaussKronrod(f,-2, 4,15).Show(); (t - new TimeSpan()).Ticks.Show(); t = new TimeSpan();

            //FuncMethods.IntegralTransformations.Test((double x) => Math.Sin(x)/Math.Abs(x*x+90), 4);
            //double res= FuncMethods.DefInteg.GaussKronrod.IntegralInf((Complex x) =>1.0/( x * x + 2 * x + 8), -5, 5);
            // (res - Math.PI / Math.Sqrt(7)).Show();

            //Complex res = FuncMethods.DefInteg.GaussKronrod.DINN_GK((Complex x) => 1.0/(x-4.2), 4, 4,4,4.5,0.002);
            //(res).Show();





            //Форма_консоли F = new Форма_консоли();
            ////F.chart1.Titles[0].Text= "Логарифмическая зависимость аппроксимации функции cos(x)*sin(x/2) системой мономов на отрезке [-4,2] и невязок при решении СЛАУ для разного количества функций (точки с нулевыми невязками не выводятся)";
            //////SequenceFunc p=(double x,int k)=>Math.Exp(k*x);
            //SequencePol p = FuncMethods.Monom;
            //RealFunc f = (double x) => Math.Cos(x);
            //int a = -1, b = 1, n1 = 1, n2 = 35;
            //double[] MAS = new double[n2 + 1];

            //F.chart1.Series[0].Name = $"Невязки системы";
            //F.chart1.Series[1].Name = $"Погрешность аппроксимации";
            //F.chart1.Series[2].IsVisibleInLegend = false;

            //SLAU T = new SLAU(p, f, n2, a, b); T.begin = a; T.end = b; T.f = f;// T.p = p;
            //T.p = (double x, int j) => p(j).Value(x);
            //for (n = n1; n <= n2; n++)
            //{
            //    RealFunc g = FuncMethods.Approx(f, p, SequenceFuncKind.Other, n, a, b);
            //    MAS[n] = FuncMethods.RealFuncMethods.NormDistance(f, g, a, b);
            //    F.chart1.Series[1].Points.AddXY(n, Math.Log10(MAS[n]));
            //    T.GaussSelection();
            //    //T.GaussSpeedyMinimize(n);
            //    double nev = T.Nev(n);//nev.Show();
            //    if (nev != 0) F.chart1.Series[0].Points.AddXY(n, Math.Log10(nev));
            //}
            //F.ShowDialog();

            //F.chart1.Series[0].Name = $"Для матрицы Гильберта при отрезке [{a},{b}]";
            //SLAU T = new SLAU(p, f, n, a, b);//T.Show();
            //var st = new Vectors(T.Size - 1);
            //for (int k = 1; k < T.Size; k++)
            //{
            //    st[k - 1] = (new SqMatrix(T.A, k)).Det; //"".Show(); st.Show();
            //    F.chart1.Series[0].Points.AddXY(k, Math.Sign(st[k - 1]));
            //}


            //a = -7;b = -3;
            //F.chart1.Series[1].Name = $"Для матрицы Гильберта при отрезке [{a},{b}]";
            // T = new SLAU(p, f, n, a, b);//T.Show();
            // st = new Vectors(T.Size - 1);
            //for (int k = 1; k < T.Size; k++)
            //{
            //    st[k - 1] = (new SqMatrix(T.A, k)).Det; //"".Show(); st.Show();
            //    F.chart1.Series[1].Points.AddXY(k, Math.Sign(st[k - 1]));
            //}

            //a = 11;b = 14;
            //F.chart1.Series[2].Name = $"Для матрицы Гильберта при отрезке [{a},{b}]";
            // T = new SLAU(p, f, n, a, b);//T.Show();
            // st = new Vectors(T.Size - 1);
            //for (int k = 1; k < T.Size; k++)
            //{
            //    st[k - 1] = (new SqMatrix(T.A, k)).Det; //"".Show(); st.Show();
            //    F.chart1.Series[2].Points.AddXY(k, Math.Sign(st[k - 1]));
            //}


            //F.chart1.SaveImage("image.bmp",System.Drawing.Imaging.ImageFormat.Bmp);
            //F.ShowDialog();


            //string s = Console.ReadLine();
            //string[] st = s.Split(' ').Where(n => n.Length > 0).ToArray();
            //double[] mas = new double[st.Length];
            //for (int i = 0; i < mas.Length; i++) mas[i] = Convert.ToDouble(st[i]);
            //string tmp = Coding.HoffmanNumberEncode(mas);tmp.Show();
            //double[] h = Coding.HoffmanNunderDecode(tmp);h.Show();

            //double pi3 = 3 * Math.PI / 2, t = 2*pi3; ;
            //(Math.Cosh(pi3 - t) / Math.Cosh(pi3)).Show();

            //RealFunc f = Parser.GetDelegate("1,0/(1+(x-1)^2)");
            //"".Show();
            //f(4).Show();

            //string s = "00101";
            //byte[] mas = System.Text.Encoding.UTF8.GetBytes(s);

            //BitArray bitArray = new BitArray(new bool[] {true,false,true});
            //string str2 = System.Convert.ToString(bitArray);
            //bitArray.ToString().Show();

            //Matrix A = new Matrix(new double[,] { { 1, 2, 3, 4 }, { 1, 3, 4, 5 }, { 0,0,4,1} }),U,VT;
            //double[] w;
            //Matrix.SVD(A, out U, out w, out VT);

            //U.PrintMatrix();"".Show();
            //Matrix E=new Matrix(w,U.RowCount,VT.RowCount);"".Show();
            //VT.PrintMatrix();
            //(U * E * VT).PrintMatrix();

            //Complex z = new Complex(0.2, 0);
            //Complex.Sqrt(z-2).Arg.Show();

            //ComplexFunc g = (Complex z) => z;
            //ComplexFunc d = (Complex z) => z * z + 1,q=(Complex z)=>1;

            //FuncMethods.DefInteg.Residue.ResSum(g,d,q,new Complex[] { Complex.I}).Show();


            //CSqMatrix m = new CSqMatrix(new Complex[,] { {2,8,30 },{new Complex(7,2),4,new Complex(10,11) },{ new Complex(9,60),new Complex(3,1),4} });
            //m.Show();
            ////m.DivByLine(1, 2);m.Show();
            //"".Show();
            //CSqMatrix inv = m.Invert();
            //inv.Show();
            //Expendator2.EmptyLine();
            //(m * inv).Show();
            //new Задача_Штурма_Лиувилля.Form1().ShowDialog();


            //double a = 12345e-2;
            //a.DimOfFractionalPath().Show();

            //Line2D a = new Line2D(-4, -12, -8),b=new Line2D(9,3,4);
            //a = new Line2D(new Point(1, 5), new Point(3, 9));
            //a.Show();
            //Line2D.InterSecPoint(a,b).Show();


            //Sums.Sum(1, (int n) => 1.0 / n ,eps:1e-10).Show();
            //Functions.KMatrix(2, 3, -2, 2).Show();

            //Polynom pol = new Polynom(1, new Vectors(new double[] { 1, 2, -2, -1, -0.5, 0.1 }));

            //double[] pv = new double[] { p(1), p(-1), p(2), p(-2), p(-0.5), p(0.1) };
            //new Vectors(pv).Show();

            //(-1.8).ToPeriod(2).Show();
            //МатКлассы2019.SpecialFunctions.MyBessel(1, 1450.7).Show();
            //new Форма_консоли().ShowDialog();

            //Waves.DCircle c = new Waves.DCircle(new Point(0, 0), 2,0.6,arg: 0);
            //c.GetNormal(new Point(1.0,0.1)).Show();
            new DC().ShowDialog();

            Console.ReadLine();
           // ИнтеграцияСДругимиПрограммами.Kill();
        }
    }
}
