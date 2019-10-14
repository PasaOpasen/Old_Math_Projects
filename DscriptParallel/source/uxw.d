module Dscript;
import std.stdio, std.math, std.concurrency, std.complex, std.typecons, std.parallelism;
import std.array, std.conv;
import bessel;
import std.datetime;

///Точка
struct Point
{
    double x, y;
}
///Нормаль в точке
struct Normal
{
    Point ///Позиция
    position, ///Сама нормаль
        n;

    public this(double px, double py, double nx, double ny)
    {
        position = Point(px, py);
        n = Point(nx, ny);
    }

    public void show()
    {
        writeln(position.x, " ", position.y, " ", n.x, " ", n.y);
    }
}
///Набор нормалей
struct Normals
{
    ///Массив нормалей
    public shared Normal[] mas;

    public this(uint k)
    {
        mas = new Normal[k];
    }
}

///Квадратная комплексная матрица
struct Matrix
{
    private cdouble[] mas;
    ///число строк
    private uint n;

    public this(cdouble[] t, uint k)
    {
        this(t);
        n = k;
    }

    public this(cdouble[] t)
    {
        mas = t;
        n = cast(uint) sqrt(cast(double) mas.length);
    }

    public this(uint k)
    {
        n = k;
        mas = new cdouble[n * n];
        mas[] = 0 + 0i;
    }

    public void show()
    {
        writeln();
        string s = "||";
        for (int i = 0; i < n; i++)
        {
            write(s);
            for (int j = 0; j < n - 1; j++)
                write(mas[i * n + j], " ");
            write(mas[i * n + n - 1]);
            writefln(s);
        }

    }

    ///Обратная матрица методом Гаусса
    public Matrix inv()
    {
        uint p;
        cdouble tp;
        auto E = Matrix(this.n);
        int i, j, k;
        for (i = 0; i < n * n; i += n + 1)
            E.mas[i] = 1.0 + 0i;

        //прямой ход
        for (i = 0; i < n - 1; i++)
        {
            for (k = i + 1; k < n; k++)
            {
                tp = mas[k * n + i] / mas[i * n + i];

                for (j = i; j < n; j++)
                {
                    mas[k * n + j] -= tp * mas[i * n + j];
                    E.mas[k * n + j] -= tp * E.mas[i * n + j];
                }
                for (j = 0; j < i; j++)
                {
                    E.mas[k * n + j] -= tp * E.mas[i * n + j];
                }
            }

        }

        //обратный
        for (i = n - 1; i >= 1; i--)
        {
            for (k = i - 1; k >= 0; k--)
            {
                tp = mas[k * n + i] / mas[i * n + i];

                for (j = n - 1; j >= 0; j--)
                {
                    E.mas[k * n + j] -= tp * E.mas[i * n + j];
                }

            }

        }

        //деление
        for (i = 0; i < n; i++)
        {
            p = i * n + i;
            for (j = 0; j < n; j++)
                E.mas[i * n + j] /= mas[p];
        }

        return E;
    }
}

///Функция Ханкеля
auto hankel(double x)
{
    return tuple(besselJ(x, 1.0) + besselY(x, 1.0) * 1i, besselJ(x, 0.0) + besselY(x, 0.0) * 1i);
}

///Начало и конец по X, Y, W
double xmin, ymin, X, Y, wbeg, wend;
///Число точек по пространству и частоте
int countS, countW;
///Массив частот
shared(double[]) w;
///Массив нормалей по источникам
shared(Normals[]) sources;
///Полюса
shared double[][] poles;

shared double lambda, mu, ro, h;
shared cdouble I2;
shared double ml2, mu2, k1coef, k2coef;
shared cdouble im;

///Выходной массив
cdouble[][Tuple!(double, double, double, short)] uxw;

void main()
{

    import std.datetime.stopwatch : benchmark, StopWatch;

    readFiles();

    auto sw = StopWatch(AutoStart.yes);
    calculate();
    sw.stop();
    writeln("Time: ", sw.peek.total!"usecs");

    writeFile();

}

///Присвоение глобальным переменным
void baseConctruct()
{
    I2 = 0.0 + 0.5 * 1i;
    ml2 = 2.0 * mu + lambda;
    im = 0.0 + mu * 1i;
    mu2 = 2.0 * mu;
    k2coef = ro / mu;
    k1coef = ro / ml2;

    poles = new shared double[][countW];
    w = new shared double[countW];
    double th = (wend - wbeg) / (countW - 1);
    foreach (i; 0 .. countW)
    {
        poles[i] = [0.0, 0.0, 0.0];
        w[i] = wbeg + i * th;
    }
}

///Прочесть исходные файлы
void readFiles()
{
    writeln("Reading data...");
    readSpace();

    readPoles();
    //auto t2 = task!readPoles();

    //t2.executeInNewThread();

    readNormals();

    //t2.yieldForce;
    writeln("Data read. Calculating...");
}

///Прочесть общие данные
void readSpace()
{
    string[] st;
    File f = File("Space.txt", "r");

    xmin = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("xmin = ", xmin);
    ymin = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("ymin = ", ymin);
    X = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("xmax = ", X);
    Y = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("ymax = ", Y);
    countS = to!int(f.readln().replace("\n", "").split(' ')[1]);
    writeln("count of space points = ", countS);
    countW = to!int(f.readln().replace("\n", "").split(' ')[1]);
    writeln("count of w points = ", countW);
    st = f.readln().replace("\n", "").split(' ');
    sources = new shared Normals[to!int(st[1])];
    for (int i = 0; i < sources.length; i++)
        sources[i] = Normals(to!int(st[i + 2]));

    wbeg = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("wbeg = ", wbeg);
    wend = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("wend = ", wend);

    lambda = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("lambda = ", lambda);
    mu = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("mu = ", mu);
    ro = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("ro = ", ro);
    h = to!double(f.readln().replace("\n", "").split(' ')[1]);
    writeln("h = ", h);

    f.close();

    baseConctruct();

}

///Считать полюса
void readPoles()
{
    File f = File("Poles.txt", "r");
    string[] s;
    foreach (k; 0 .. countW)
    {
        s = f.readln().replace("\n", "").split(" ");
        //writeln(s);
        for (int i = 0; i < s.length; i++)
            poles[k][i] = to!double(s[i]);
        //writeln(poles[k]);
    }
    f.close();
}

///Считать нормали
void readNormals()
{
    File f = File("Normals.txt", "r");
    string[] s;
    double px, py, nx, ny;

    auto tmp = f.readln();
    foreach (k; 0 .. sources.length)
        foreach (i; 0 .. sources[k].mas.length)
        {
            s = f.readln().replace("\n", "").split(" ");
            //writeln(s);
            px = to!double(s[1]);
            py = to!double(s[2]);
            nx = to!double(s[3]);
            ny = to!double(s[4]);
            sources[k].mas[i] = Normal(px, py, nx, ny);
            //sources[k].mas[i].show();
        }
    f.close();
}

///Записать значения в файл
void writeFile()
{
    File f = File("uxw.txt", "w");
    f.writefln("x y w c Re(ux) Im(ux) Re(uy) Im(uy) Re(uz) Im(uz)");
    foreach (i, cdouble[] e; uxw)
    {
        //f.writeln(i[0], " ", i[1], " ", i[2], " ", i[3] + 1, " ", e[0].re, " ",             e[0].im, " ", e[1].re, " ", e[1].im, " ", e[2].re, " ", e[2].im);
        f.writefln("%.8f %.8f %.8f %d %.20f %.20f %.20f %.20f %.20f %.20f",i[0],  i[1],  i[2],  i[3] + 1,  e[0].re,
                 e[0].im,  e[1].re,  e[1].im,  e[2].re,  e[2].im);
          //       f.writeln("");
    }

    f.close();
}

///Вычислить значения
void calculate()
{
    double x, y;

    double xh = (X - xmin) / (countS - 1), yh = (Y - ymin) / (countS - 1);
    short scount = cast(short)(sources.length);
    int all = countS * countS * scount, save = 0;

    cdouble[][] val = new cdouble[][countW];
    int[] ind = new int[countW];
    foreach (u; 0 .. countW)
        ind[u] = u;

    foreach (i; 0 .. countS)
    {
        x = xmin + i * xh;
        foreach (j; 0 .. countS)
        {
            y = ymin + j * xh;
            foreach (short s; 0 .. scount)
            {
                //writeln(x," <- x, y = ", y, " s = ",s);
                foreach (k, ref v;  /*ind*/ taskPool.parallel(val))
                {
                    // val[k] = сalcuxw(x, y, s, k);
                    v = сalcuxw(x, y, s, cast(int) k);
                }

                foreach (k; ind)
                {
                    uxw[tuple(x, y, cast(double) w[k], s)] = val[k];
                }

                save++;
                if (save % 25 == 0)
                    writeln("Ready ", save, " from ", all, " (",
                            cast(float)(save) / all * 100, "%)");
            }
        }
    }

}

///Минимальное расстояние между элементами массива
pure double minDist(double[] mas)
{
    double r = mas[1] - mas[0], p;
    for (int i = 2; i < mas.length; i++)
    {
        p = mas[i] = mas[i - 1];
        if (p < r)
            r = p;
    }
    return r;
}
///евклидово расстояние между точками
pure nothrow double eudistance(Point a, Point b)
{
    return sqrt((a.x - b.x).pow(2.0) + (a.y - b.y).pow(2.0));
}

///Вычисление функции uxw по простанственным координатам, частоте и номеру источника
cdouble[] сalcuxw(double x, double y, short s, int ind)
{
    double ww = cast(double) w[ind];

    auto pols = poles[ind]; //poles[countUntil(w, ww)]; //writeln(pols);

    cdouble[][] c1 = new cdouble[][pols.length], c2 = new cdouble[][pols.length];
    cdouble[] res = [0.0 + 0i, 0.0 + 0i, 0.0 + 0i];
    Tuple!(cdouble, cdouble) tup;
    cdouble[] sum = res.dup;
    Point xy = Point(x, y);
    Point QQ;

    double dist = minDist([0.0] ~ pols), xp, yp;

    double eps = dist * 0.4, eps2 = 0.5 * eps;
    double[] plus = new double[pols.length], pminus = new double[pols.length];

    for (int k = 0; k < pols.length; k++)
    {
        plus[k] = pols[k] + eps;
        pminus[k] = pols[k] - eps;
        c1[k] = PRMSN(plus[k], ww);
        c2[k] = PRMSN(pminus[k], ww);
    }

    for (int i = 0; i < sources[s].mas.length; i++)
    {
        QQ = Point(eps2 * sources[s].mas[i].n.x, eps2 * sources[s].mas[i].n.y);
        xp = x - sources[s].mas[i].position.x;
        yp = y - sources[s].mas[i].position.y;

        for (int k = 0; k < pols.length; k++)
        {
            tup = hankel(pols[k] * eudistance(sources[s].mas[i].position, xy));

            res = inkTwice(plus[k], pminus[k], c1[k], c2[k], tup, xp, yp);
            //writeln(res);
            //writeln();
            sum[0] += res[0] * QQ.x + res[1] * QQ.y;
            sum[1] += res[4] * QQ.x + res[5] * QQ.y;
            sum[2] += res[7] * QQ.x + res[8] * QQ.y;
            // writeln(sum);
        }
    }
    sum[0] *= I2;
    sum[1] *= I2;
    sum[2] *= I2;
    return sum;
}

///Cпециальная разность матриц Грина
cdouble[] inkTwice(double a1, double a2, cdouble[] PRMSN1, cdouble[] PRMSN2,
        Tuple!(cdouble, cdouble) beshank, double x, double y)
{

    double x2 = x * x, y2 = y * y, r2 = x2 + y2, r = sqrt(r2), xy = x * y;
    double ar1 = a1 * r, a21 = a1 * a1;
    double ar2 = a2 * r, a22 = a2 * a2;

    cdouble j1ar = beshank[0], j0ar = beshank[1];

    cdouble P = PRMSN1[0] * a21 - PRMSN2[0] * a22, R1 = PRMSN1[1],
        Mi1 = PRMSN1[2] * 1i, Si = (PRMSN1[3] - PRMSN2[3]) * 1i, Ni1 = PRMSN1[4] * 1i;
    cdouble R2 = PRMSN2[1], Mi2 = PRMSN2[2] * 1i, Ni2 = PRMSN2[4] * 1i;

    cdouble j1arr = j1ar / r, jx = -x * j1arr, jy = -y * j1arr, j0ara1 = j0ar * a1,
        j0ara2 = j0ar * a2, jtmp = j1arr * (x2 - y2), jxx1 = -(j0ara1 * x2 - jtmp),
        jxy1 = -xy * (j0ara1 - 2 * j1arr), jyy1 = -(j0ara1 * y2 + jtmp),
        jxx2 = -(j0ara2 * x2 - jtmp), jxy2 = -xy * (j0ara2 - 2 * j1arr), jyy2 = -(j0ara2 * y2 + jtmp);

    cdouble K12 = ((Mi1 - Ni1) * jxy1 - (Mi2 - Ni2) * jxy2) / r2;

    // writeln((Mi1 * jxx1 + Ni1 * jyy1), " ", Mi2 * jxx2, " ", Ni2 * jyy2, " ", (Mi1 * jxx1 + Ni1 * jyy1 - Mi2 * jxx2 - Ni2 * jyy2));     
    //  writeln((Mi1 * jxx1 + Ni1 * jyy1), " ", Mi2 * jxx2, " ", Ni2 * jyy2, " ", (Mi1 * jxx1 + Ni1 * jyy1) - (Mi2 * jxx2 + Ni2 * jyy2));
    //writeln();

    return [(Mi1 * jxx1 + Ni1 * jyy1 - Mi2 * jxx2 - Ni2 * jyy2) / r2, K12,
        P * jx, K12, (Mi1 * jyy1 + Ni1 * jxx1 - Mi2 * jyy2 - Ni2 * jxx2) / r2,
        P * jy, Si * jx, Si * jy, R1 * j0ara1 - R2 * j0ara2];
}

cdouble[] PRMSN(double al, double ww)
{
    auto c = reversOfA(al, ww);
    cdouble[] v1 = c[0], v2 = c[1];
    double alp = al * al;
    cdouble s1 = sigma(alp, k1(ww)), s2 = sigma(alp, k2(ww)), ai = 0.0 + (alp * 1i);
    cdouble e1 = 1.0 + 0i, e3 = 1.0 + 0i, e2 = exp(-s1 * (h)), e4 = exp(-s2 * (h));

    cdouble[] c1 = [e1, e2, s2 * e3, -s2 * e4], c2 = [s1 * e1, -s1 * e2, alp * e3, alp * e4];

    cdouble P = scPower(v1, c1) / mu2;
    cdouble R = scPower(v1, c2) / mu2;
    cdouble M = scPower(v2, c1) / mu2;
    cdouble S = scPower(v2, c2) / mu2;
    cdouble N = 1i * cth(s2 * h) / (mu * s2);

    return [P, R, M, S, N];
}

///Котангенс
pure nothrow cdouble cth(cdouble c)
{
    import std.math : sin, cos;

    return cos(c) / sin(c);
}

pure nothrow cdouble exp(cdouble c)
{
    import std.math : exp, expi;

    return exp(c.re) * expi(c.im);
}

///Скалярное произведение комплексных векторов
pure nothrow cdouble scPower(cdouble[] a, cdouble[] b)
{
    cdouble res = 0.0 + 0i;
    for (int i = 0; i < a.length; i++)
        res += a[i] * b[i];
    return res;
}

Tuple!(cdouble[], cdouble[]) reversOfA(double alp, double w0)
{
    double kt1 = k1(w0), kt2 = k2(w0);
    double al = alp * alp;
    cdouble s1 = sigma(al, kt1), s2 = sigma(al, kt2);
    cdouble a = cast(cdouble)(al - 0.5 * kt2); //*mu2;
    cdouble b = s1 * 1i; // * mu2;// *();
    cdouble c = al * s2; // * mu2;
    cdouble d = -a * 1i; // * (al * cdouble.I);
    //cdouble q = cdouble.Exp(s1 * h), ww = cdouble.Exp(s2 * h);
    cdouble q = exp(-s1 * h), ww = exp(-s2 * h);

    Matrix res = new Matrix([a, a * q, c, -c * ww, -b, b * q, d, d * ww, a * q,
            a, c * ww, -c, -b * q, b, d * ww, d]).inv();

    return tuple([res.mas[0], res.mas[4], res.mas[8], res.mas[12]],
            [res.mas[1], res.mas[5], res.mas[9], res.mas[13]]);

}

double k1(double w0)
{
    return w0 * w0 * k1coef;
}

double k2(double w0)
{
    return w0 * w0 * k2coef;
}

cdouble sigma(double a, double kw)
{
    return sqrt(cast(cdouble)(a - kw)) * sgn(abs(a) - kw);
}
