using System;
using МатКлассы;
using Point = МатКлассы.Point;
using static System.Math;
using static Курсач.KursMethods.TestFuncAndCurve;


namespace Курсач
{

    public partial class KursMethods
    {
        /// <summary>
        /// alpha_i
        /// </summary>
      public static Func<Point, int, double> alpha = (Point p, int k) => masPoints[k].PotentialF(p);
        /// <summary>
        /// beta_i
        /// </summary>
        public  static Func<Point, int, double> beta = (Point p, int k) => alpha(p, k) * Point.Eudistance(masPoints[k], p).Sqr();
        /// <summary>
        /// partial alpha_i / partial nu
        /// </summary>
        public  static Func<Point, int, double> alphanu = (Point p, int k) => aig(p, k) * Norm[CIRCLE - 1](p);
        /// <summary>
        /// partial beta_i / partial nu
        /// </summary>
        public static Func<Point, int, double> betanu = (Point p, int k) => big(p, k) * Norm[CIRCLE - 1](p);

        public static Func<Point, Point, double> Exy = (Point x, Point y) =>
        {
            double dist = Point.Eudistance(x, y);
            return (dist==0)?0:-2 * Math.PI * Math.Log(dist);
        };
        public static Func<Point, Point, double> Exynu = (Point x, Point y) => 
        {
            double d = 4*Math.PI / Point.Eudistance(x, y).Sqr();
            Point p = new Point((x.x - y.x) * d, (x.y - y.y) * d);
            return p * TestFuncAndCurve.Norm[CIRCLE - 1](y);
        };

        /// <summary>
        /// Правая часть для решения бигармонического уравнения через ОЗГ-метод
        /// </summary>
        public static Functional right = (Point x) =>
          {
              Functional f = (Point y) => TestFuncAndCurve.Grads[GF - 1](y) * TestFuncAndCurve.Norm[CIRCLE - 1](y) * Exy(x, y) - U(y) * Exynu(x, y);
              return myCurveQ.Firstkind(f);
          };

        /// <summary>
        /// Производная u по нормали
        /// </summary>
        public static Functional Unu = (Point x) => TestFuncAndCurve.Grads[GF - 1](x) * TestFuncAndCurve.Norm[CIRCLE - 1](x);
        /// <summary>
        /// Функция U
        /// </summary>
        public static Functional U = (Point x) => TestFuncAndCurve.DFunctions[GF-1](x);

        /// <summary>
        /// Пространство тестовых функций и данных
        /// </summary>
        public class TestFuncAndCurve
        {
            /// <summary>
            /// Расширенные параметризации кривых
            /// </summary>
            public static Func<double,double,double>[] U = new Func<double,double,double>[CountCircle], V = new Func<double,double,double>[CountCircle];
            /// <summary>
            /// Площади сегментов кривых
            /// </summary>
            public static TripleFunc[] T = new TripleFunc[CountCircle];
            public static Func<double,double>[] Ends = new Func<double,double>[CountCircle];
            static TestFuncAndCurve()
            {
                U[0] = (double t, double r) => r * Math.Cos(t);
                V[0] = (double t, double r) => r * Math.Sin(t);
                U[1] = (double t, double r) =>
                  {
                      dis = r - MIN_RADIUS;
                      mdx = dis / 2; //но если внутри каждой вставить это,оставив те глобальные, всё получится
                      if ((t >= 0) && (t <= 2 * r))
                      {
                          return t / 2 - mdx;
                      }
                      if ((t >= 2 * r) && (t <= 3 * r))
                      {
                          return 3 * r - 1 * t - mdx;
                      }
                      throw new Exception("Выход за границы отрезка параметризации");
                  };
                V[1] = (double t, double r) =>
                {
                    dis = r - MIN_RADIUS;
                    mdx = dis / 2;
                    mdy = mdx / Math.Sqrt(3);
                    if ((t >= 0) && (t <= r))
                    {
                        return t / 2 * Math.Sqrt(3) - mdy;
                    }
                    if ((t >= r) && (t <= 2 * r))
                    {
                        return -t / 2 * Math.Sqrt(3) + r * Math.Sqrt(3) - mdy;
                    }
                    if ((t >= 2 * r) && (t <= 3 * r))
                    {
                        return 0 - mdy;
                    }
                    throw new Exception("Выход за границы отрезка параметризации");
                };
                U[2] = (double t, double r) =>
                {
                    dis = r - MIN_RADIUS;
                    mdx = dis / 2;
                    if ((t >= 0) && (t <= r))
                    {
                        return t - mdx;
                    }
                    if ((t >= r) && (t <= 2 * r))
                    {
                        return r - mdx;
                    }
                    if ((t >= 2 * r) && (t <= 3 * r))
                    {
                        return 3 * r - t - mdx;
                    }
                    if ((t >= 3 * r) && (t <= 4 * r))
                    {
                        return 0 - mdx;
                    }
                    throw new Exception("Выход за границы отрезка параметризации");
                };
                V[2] = (double t, double r) =>
                {
                    dis = r - MIN_RADIUS;
                    mdx = dis / 2;
                    if ((t >= 0) && (t <= r))
                    {
                        return 0 - mdx;
                    }
                    if ((t >= r) && (t <= 2 * r))
                    {
                        return t - r - mdx;
                    }
                    if ((t >= 2 * r) && (t <= 3 * r))
                    {
                        return r - mdx;
                    }
                    if ((t >= 3 * r) && (t <= 4 * r))
                    {
                        return 4 * r - t - mdx;
                    }
                    throw new Exception("Выход за границы отрезка параметризации");
                };
                U[3] = (double t, double r) =>
                {
                    dis = r - MIN_RADIUS;
                    mdx = dis / 2;
                    mdy = mdx / Math.Sqrt(3);
                    if ((t >= 0) && (t <= r))
                    {
                        return t - mdx;
                    }
                    if ((t >= r) && (t <= 1.5 * r))
                    {
                        return 3 * r - 2 * t - mdx;
                    }
                    throw new Exception("Выход за границы отрезка параметризации");
                };
                V[3] = (double t, double r) =>
                {
                    dis = r - MIN_RADIUS;
                    mdx = dis / 2;
                    mdy = mdx * Math.Sqrt(3) / 2;
                    if ((t >= 0) && (t <= 0.5 * r))
                    {
                        return Math.Sqrt(r * r - (t - r) * (t - r)) - mdy;
                    }
                    if ((t >= 0.5 * r) && (t <= r))
                    {
                        return Math.Sqrt(r * r - t * t) - mdy;
                    }
                    if ((t >= r) && (t <= 1.5 * r))
                    {
                        return 0 - mdy;
                    }
                    throw new Exception("Выход за границы отрезка параметризации");
                };

                T[0] = (double tx, double ty, double r) => (r > 0) ? tx * ty * r : 0; Ends[0] = (double r) => 2 * Math.PI;
                T[1] = (double tx, double ty, double r) => (r > 0) ? tx * ty / 2 / Math.Sqrt(3) : 0; Ends[1] = (double r) => 3 * r;
                T[2] = (double tx, double ty, double r) => (r > 0) ? tx * ty / 2 : 0; Ends[2] = (double r) => 4 * r;
                T[3] = (double tx, double ty, double r) => (r > 0) ? 2 * tx * ty * (Math.PI / 3 - Math.Sqrt(3) / 4) / 1.5 : 0/*(2 * Math.PI / 3+1)*/; Ends[3] = (double r) => /*(1+2*Math.PI/3)*/1.5 * r;

                //for (int i = 0; i < DFunctions.Length; i++)
                //    GFunctions[i] = (Point x) =>
                //{
                //    Functional f = (Point p) => DFunctions[i](p) * E(new Point(x.x - p.x, x.y - p.y));
                //    return DFunctions[i](x);
                //    return DoubleIntegral(f, myCurve, myCurve.S, Method.GaussKronrod15, 0.001, FuncMethods.DefInteg.countY);
                //};
                FuncName[0] = "x+y";
                FuncName[1] = "sin(y)(exp(x)+exp(-x))";
                FuncName[2] = "3x+6y+sqrt(x^2+y^2)+0,5y^2";
                FuncName[3] = "constant=1,5";
                FuncName[4] = "-ln(abs(point-z0))";
                FuncName[5] = "x^2-y^2";
                FuncName[6] = "(x+y)exp(x-y)";
                FuncName[7] = "0 or 0,5 or -0,5";
                FuncName[8] = "f7+f3";
                FuncName[9] = "exp(x)(cos(y)+3sin(y))";
                FuncName[10] = "f1+f2+f3+f4+f5";
            }

            #region возможные параметризации для области
            //окружность радиуса MIN_RADIUS
            public static double u1(double t)
            {
                return MIN_RADIUS * Math.Cos(t);
            }
            public static double v1(double t)
            {
                return MIN_RADIUS * Math.Sin(t);
            }

            //соответствующая окружность радиуса MAX_RADIUS (около которой берутся базисные точки)
            public static double u1h(double t)
            {
                return MAX_RADIUS * Math.Cos(t);
            }
            public static double v1h(double t)
            {
                return MAX_RADIUS * Math.Sin(t);
            }

            //равносторонний треугольник со стороной MIN_RADIUS
            public static double u2(double t)
            {
                if ((t >= 0) && (t <= 2 * MIN_RADIUS))
                {
                    return t / 2;
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return 3 * MIN_RADIUS - 1 * t;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v2(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return t / 2 * Math.Sqrt(3);
                }
                if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))
                {
                    return -t / 2 * Math.Sqrt(3) + MIN_RADIUS * Math.Sqrt(3);
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return 0;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double dis = MAX_RADIUS - MIN_RADIUS; //кажется, из-за глобальности этих переменных всегда происходит стягивание одной вершины в одну и ту же точку
            public static double mdx = dis / 2;
            public static double mdy = mdx / Math.Sqrt(3);
            //соответствующий равносторонний треугольник со стороной MAX_RADIUS
            public static double u2h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2; //но если внутри каждой вставить это,оставив те глобальные, всё получится
                if ((t >= 0) && (t <= 2 * MAX_RADIUS))
                {
                    return t / 2 - mdx;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return 3 * MAX_RADIUS - 1 * t - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v2h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                mdy = mdx / Math.Sqrt(3);
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return t / 2 * Math.Sqrt(3) - mdy;
                }
                if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))
                {
                    return -t / 2 * Math.Sqrt(3) + MAX_RADIUS * Math.Sqrt(3) - mdy;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return 0 - mdy;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }

            //квадрат со стороной MIN_RADIUS
            public static double u3(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return t;
                }
                if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))
                {
                    return MIN_RADIUS;
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return 3 * MIN_RADIUS - t;
                }
                if ((t >= 3 * MIN_RADIUS) && (t <= 4 * MIN_RADIUS))
                {
                    return 0;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v3(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return 0;
                }
                if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))
                {
                    return t - MIN_RADIUS;
                }
                if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))
                {
                    return MIN_RADIUS;
                }
                if ((t >= 3 * MIN_RADIUS) && (t <= 4 * MIN_RADIUS))
                {
                    return 4 * MIN_RADIUS - t;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            //соответствующий квадрат со стороной MAX_RADIUS
            public static double u3h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return t - mdx;
                }
                if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))
                {
                    return MAX_RADIUS - mdx;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return 3 * MAX_RADIUS - t - mdx;
                }
                if ((t >= 3 * MAX_RADIUS) && (t <= 4 * MAX_RADIUS))
                {
                    return 0 - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v3h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return 0 - mdx;
                }
                if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))
                {
                    return t - MAX_RADIUS - mdx;
                }
                if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))
                {
                    return MAX_RADIUS - mdx;
                }
                if ((t >= 3 * MAX_RADIUS) && (t <= 4 * MAX_RADIUS))
                {
                    return 4 * MAX_RADIUS - t - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            //острие
            public static double u4(double t)
            {
                if ((t >= 0) && (t <= MIN_RADIUS))
                {
                    return t;
                }
                if ((t >= MIN_RADIUS) && (t <= 1.5 * MIN_RADIUS))
                {
                    return 3 * MIN_RADIUS - 2 * t;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v4(double t)
            {
                if ((t >= 0) && (t <= 0.5 * MIN_RADIUS))
                {
                    return Math.Sqrt(MIN_RADIUS * MIN_RADIUS - (t - MIN_RADIUS) * (t - MIN_RADIUS));
                }
                if ((t >= 0.5 * MIN_RADIUS) && (t <= MIN_RADIUS))
                {
                    return Math.Sqrt(MIN_RADIUS * MIN_RADIUS - t * t);
                }
                if ((t >= MIN_RADIUS) && (t <= 1.5 * MIN_RADIUS))
                {
                    return 0;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }

            public static double u4h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                mdy = mdx / Math.Sqrt(3);
                if ((t >= 0) && (t <= MAX_RADIUS))
                {
                    return t - mdx;
                }
                if ((t >= MAX_RADIUS) && (t <= 1.5 * MAX_RADIUS))
                {
                    return 3 * MAX_RADIUS - 2 * t - mdx;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            public static double v4h(double t)
            {
                dis = MAX_RADIUS - MIN_RADIUS;
                mdx = dis / 2;
                mdy = mdx * Math.Sqrt(3) / 2;
                if ((t >= 0) && (t <= 0.5 * MAX_RADIUS))
                {
                    return Math.Sqrt(MAX_RADIUS * MAX_RADIUS - (t - MAX_RADIUS) * (t - MAX_RADIUS)) - mdy;
                }
                if ((t >= 0.5 * MAX_RADIUS) && (t <= MAX_RADIUS))
                {
                    return Math.Sqrt(MAX_RADIUS * MAX_RADIUS - t * t) - mdy;
                }
                if ((t >= MAX_RADIUS) && (t <= 1.5 * MAX_RADIUS))
                {
                    return 0 - mdy;
                }
                throw new Exception("Выход за границы отрезка параметризации");
            }
            #endregion

            #region граничные функции и массив граничных функций
            public static double g1(Point point) => point.x + point.y;
            public static double g2(Point point) => Math.Sin(point.y) * (Math.Exp(point.x) + Math.Exp(-point.x));
            public static double g3(Point point) => 3 * point.x + 6 * point.y + Math.Sqrt(point.x.Sqr() + point.y.Sqr()) + 0.5 * point.y.Sqr();
            public static double g4(Point point) => CONSTANT;
            public static double g5(Point point) => masPoints[0].PotentialF(point);
            public static double g6(Point point) => point.x * point.x - point.y * point.y;
            public static double g7(Point point) => g1(point) * Math.Exp(point.x - point.y);
            public static double g8(Point point)
            {
                double dx = MIN_RADIUS / 2;
                double dy = MIN_RADIUS / 2 * Math.Sqrt(3);
                double argument;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: Point d = point;
                Point d = new Point(point); //d.x=-0.25;d.y=-0.25;
                d.x -= dx;
                d.y -= dy; //сдвиг к началу координат

                if (d.x == 0)
                {
                    argument = pi / 2 * Math.Sign((sbyte)d.y);
                }
                else
                {
                    if (d.y == 0)
                    {
                        argument = pi * Math.Sign((sbyte)-1 + Math.Sign((sbyte)d.x));
                    }
                    else
                    {
                        argument = Math.Atan(d.y / d.x) + Math.Sign((sbyte)Math.Abs(d.x) - d.x) * Math.Sign((sbyte)d.y) * pi;
                    }
                }
                //return argument;
                //cout+d.x+" "+d.y+" аргумент в доли: "+argument/pi+endl;

                if ((-pi <= argument) && (argument < -2 * pi / 3))
                {
                    return -1.0 / 2;
                }
                if ((-2 * pi / 3 <= argument) && (argument <= -pi / 3))
                {
                    return 0; //return 1; //единицы будут
                }
                if ((-pi / 3 < argument) && (argument <= pi / 2))
                {
                    return 1.0 / 2; //return 1; //уже не будут
                }
                /*if ((pi / 2 < argument) && (argument <= pi))*/
                return -1.0 / 2;
            }
            public static double g9(Point point) => g8(point) + g3(point);
            public static double g10(Point p) => Math.Exp(p.x) * (Math.Cos(p.y) + 3 * Math.Sin(p.y));
            public static double g11(Point p) => g1(p) + g2(p) + g3(p) + g4(p) + g4(p);
            public static Functional[] DFunctions = { g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11 };
            public static Functional[] GFunctions = new Functional[DFunctions.Length];

            #endregion

            #region градиенты и нормали

            /// <summary>
            /// Градиенты функций
            /// </summary>
            public static Func<Point, Point>[] Grads = new Func<Point, Point>[]
            {
                (Point p)=>new Point(1,1),
                (Point p)=>new Point(Sin(p.y)*(Exp(p.x)-Exp(-p.x)),Cos(p.y)*(Exp(p.x)+Exp(-p.x))),
                (Point p)=>new Point(3+p.x/p.Abs,6+p.y*(1.0/p.Abs+1)),
                (Point p)=>new Point(0),
                (Point p)=>-aig(p,0),
                //{
                //    double d=-2/((p.x-masPoints[0].x).Sqr()+(p.y-masPoints[0].y).Sqr());
                //    return new Point(d*(p.x-masPoints[0].x).Pow(1/*3*/),d*(p.y-masPoints[0].y).Pow(/*3*/1));
                //},
                (Point p)=>new Point(2*p.x,-2*p.y),
                (Point p)=>new Point(Exp(p.x-p.y)*(p.x+p.y+1),Exp(p.x-p.y)*(-(p.x+p.y)+1)),
                (Point p)=>new Point(0),
                (Point p)=>new Point(3+p.x/p.Abs,6+p.y*(1.0/p.Abs+1)),
                (Point p)=>new Point(Exp(p.x)*(Cos(p.y)+3*Sin(p.y)),Exp(p.x)*(-Sin(p.y)+3*Cos(p.y)))
                //(Point p)=>new Point() последней функции не будет!!!!    
            };

            public static Func<Point, int, Point> aig = (Point p, int k) =>
            {
                //double d = -2 / ((p.x - masPoints[k].x).Sqr() + (p.y - masPoints[k].y).Sqr());
                //return new Point(d * (p.x - masPoints[k].x).Pow(3), d * (p.y - masPoints[k].y).Pow(3));
                double x = p.x - masPoints[k].x, y = p.y - masPoints[k].y, d = x * x + y * y;
                return new Point(x / d, y / d);
            };
            public static Func<Point, int, Point> big = (Point p, int k) =>
            {
                //double c = alpha(p, k);
                //return new Point(2 * ((p.x) - masPoints[k].x) * (c - ((p.x) - masPoints[k].x).Sqr()), 2 * ((p.y) - masPoints[k].y) * (c - ((p.y) - masPoints[k].y).Sqr()));
                double x = p.x - masPoints[k].x, y = p.y - masPoints[k].y, d = x * x + y * y;
                double al = alpha(p, k);
                Point gr = aig(p, k);
                return new Point(2 * x * al + d * gr.x, 2 * y * al + d * gr.y);
            };

            public static Func<Point, Point>[] Norm = new Func<Point, Point>[]
            {
                (Point p) =>
                {
                    double d=p.Abs;
                    return new Point(p.x/d,p.y/d);
                },
                (Point p)=>
                {
                    if(p.y<=0)
                        return new Point(0,-1);
                    if(p.x>=0.5*MIN_RADIUS)
                        return new Point(Sqrt(3)/2,0.5);
                    return new Point(-Sqrt(3)/2,0.5);
                },
                (Point p)=>
                {
                    if(p.x<=0) return new Point(-1,0);
                    if(p.x>=MIN_RADIUS) return new Point(1,0);
                   if(p.y<=0) return new Point(0,-1);
                    /*if(p.y==MIN_RADIUS)*/ return new Point(0,1);
                },
                (Point p) =>
                {
                    if(p.y<=0)
                        return new Point(0,-1);

                     double d;
                    if(p.x>=0.5*MIN_RADIUS)
                    {
                     d=p.Abs;
                    return new Point(p.x/d,p.y/d);
                    }
                    p.x-=MIN_RADIUS;
                    d=Math.Sqrt(p.x*p.x+p.y*p.y);
                    return new Point(p.x/d,p.y/d);
                }
            };
            #endregion

        }
    }
}
