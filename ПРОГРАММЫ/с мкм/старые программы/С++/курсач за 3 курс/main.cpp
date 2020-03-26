//программа, которая находит приближённое решение задачи Дирихле для уравнения Лапласа методом базисных потенциалов
//написал ПАСЬКО Д. А.
//в последний раз программа редактировалась 27.02.2018

//стандартные директивы препроцессора
#include <iostream>
#include <fstream>
#include <algorithm>
#include <cstdlib>
#include <cmath>
#include <string>
#include "Graph2016.h"
#include "stringconvert.h"
#include <dir.h>

//стандартное пространство имён
using namespace std;

class BasisPoint {//точки на плоскости
	public:
	double x;//координаты точки в двумерном пространстве
	double y;
	//функция базисного потенциала в точке z, соответствующая исходной базисной точке
	double Potentialf(BasisPoint z)
	{
		return log(1 / sqrt((z.x - x)*(z.x - x) + (z.y - y)*(z.y - y)));
	}

}*masPoints;//указатель на массив точек плоскости

	
	double Eudistance(BasisPoint z, BasisPoint w)//расстояние между точками 
	{
		return sqrt((z.x - w.x)*(z.x - w.x) + (z.y - w.y)*(z.y - w.y));
	}

//объявление нужных после этого момента функций
double BasisFuncPow(int i, int j, BasisPoint z);
double Nev(double **a, double *y, double *f);

namespace Func_in_matrix //пространство функций, связанных с матрицами и векторами
{
	void Matrix_power(double *Ax, double **a, double *x, int k)//частичное произведение матрицы на вектор 
	{
		for (int i = 0; i < k; i++)
		{
			double sum = 0;
			for (int j = 0; j < k; j++) sum += a[i][j] * x[j];
			Ax[i] = sum;
		}
	}

	void Vector_difference(double *r, double *Ax, double *b, int t)//разность двух векторов
	{
		for (int i = 0; i < t; i++) r[i] = Ax[i] - b[i];
	}

	double Scalar_power(double *r, double *rr, int t) //классическое скалярное произведение двух векторов
	{
		double s = 0;
		for (int i = 0; i < t; i++) s += r[i] * rr[i];
		return s;
	}

	void Vector_on_scalar(double *s, double tau, double *r, int t)//умножение вектора на скаляр
	{
		for (int i = 0; i < t; i++) s[i] = tau*r[i];
	}

	void Vector_sum(double *sum, double *s, double *x, int t)//сумма векторов
	{
		for (int i = 0; i < t; i++) sum[i] = x[i] + s[i];
	}

	void Vector_assingment(double *x, double *s, int t)//присваивание одному вектору другого
	{
		Vector_on_scalar(x, 1, s, t);
	}

}


//глобальные переменные
int ITER_INTEG = 5000; //количество шагов при интегрировании
double(*fi)(BasisPoint);//указатель на граничную функцию от точки
double(*fi0)(BasisPoint);
int CIRCLE, GF, N;//номер области, граничной функции и количество базисных точек
bool zero;// наличие нулевых элементов в массиве
const int KGF = 8, MAXCIRCLE = 4;//всего граничных функций
const double EPS = 0.00001, EPSS = 0, CONSTANT = 1, pi = 3.14159;//используемая в некоторых местах погрешность
double RESULT, NEVA; //точность аппроксимации и невязка
//double *goldmas;//указатель на массив чисел для равномерного распределения
int cr = 250, cg = 0, cb = 0, mr = 0, mg = 250, mb = 0; //цветовые параметры для границ
char dir_Curve_name[50], dir_func_name[50], chstr[100];//имя внутренней папки и подпапки
string sl = "\\", bstr;

double VALUE_FOR_ULTRA = 10;//
double MIN_RADIUS = 0.5;//радиус области
double MAX_RADIUS = 3.5;//радиус вне области

//функция произведений базисных функций
double BasisFuncPow(int i, int j, BasisPoint z)
{
	if ((i == N) && (j == N)) return fi(z)*fi(z);
	if (i == N) return masPoints[j].Potentialf(z)*fi(z);
	if (j == N) return masPoints[i].Potentialf(z)*fi(z);
	return masPoints[i].Potentialf(z)*masPoints[j].Potentialf(z);
}

//класс плоских кривых 
class Curve
{
private:
	double(*u)(double);//параметризации координат
	double(*v)(double);
	double _h; //значение шага для этой кривой
	int M;//число шагов
	void Get_h(int MM) {
		M = MM;
		_h = (b - a) / M; //присвоение шагу конкретного значения
	}

public:
	BasisPoint Transfer(double t) {//возврат точки на кривой по значению параметра
		BasisPoint point;
		point.x = u(t);
		point.y = v(t);
		return point;
	}

	double a;//начальное значение параметра
	double b;//конечное значение параметра

	Curve(double a0, double b0, double(*uu)(double), double(*vv)(double)) {//полный конструктор
		a = a0; b = b0;
		u = uu; v = vv;
		//fi=fi0;
	}
	Curve() {//простейший конструктор
		a = 0; b = 0;
	}

	double Firstkind(double(*f)(BasisPoint)) {//вычисление криволинейного интеграла первого рода по этой кривой от функции f методом трапеции (в программе не используется)
		Get_h(ITER_INTEG);
		double value = 0;
		for (int k = 0; k <= M - 1; k++) value += f(Transfer((a + (k + 1)*_h + a + (k)*_h) / 2))*Eudistance(Transfer(a + (k + 1)*_h), Transfer(a + (k)*_h));
		return value;
	}

	double Firstkind(int i, int j) {//вычисление криволинейного интеграла первого рода по этой кривой от функции BasisFuncPow(int i,int j,BasisPoint z)
		Get_h(ITER_INTEG);
		double value = 0;
		//for (int k = 0; k <= M - 1; k++) value += BasisFuncPow(i, j, Transfer((a + (k + 1)*h + a + (k)*h) / 2))*Eudistance(Transfer(a + (k + 1)*h), Transfer(a + (k)*h));//метод прямоугольников
		for (int k = 1; k <= M; k++) value += (BasisFuncPow(i, j, Transfer(a + (k)*_h)) + BasisFuncPow(i, j, Transfer(a + (k - 1)*_h)))*Eudistance(Transfer(a + (k - 1)*_h), Transfer(a + (k)*_h)) / 2;//метод трапеций

		//МЕТОД СИМПСОНА ОТКАЗЫВАЕТСЯ РАБОТАТЬ
		/*double sum1=0,sum2=0;
		for (double k = 0; k <= M - 1; k+=0.5){
			sum1+=BasisFuncPow(i, j, Transfer((a + (2*k)*h)));
			sum2+=BasisFuncPow(i, j, Transfer((a + (2*k-1)*h)));
		}
		value=h/3*(BasisFuncPow(i, j, Transfer(a))+BasisFuncPow(i, j, Transfer(b))+4*sum2+2*sum1);*/

		/*double EPS=0.0001;
		double I=EPS+1, I1=0;//I-предыдущее вычисленное значение интеграла, I1-новое, с бо`льшим N.
		for (int N=2; (N<=8)||(fabs(I1-I)>EPS); N*=2)
		{
			double hh, sum2=0, sum4=0, sum=0,e=hh/3;
			hh=(b-a)/(2*N);//Шаг интегрирования.
			for (int i=1; i<=2*N-1; i+=2)
			{
				sum4+=BasisFuncPow(i, j, Transfer(a+hh*i));//Значения с нечётными индексами, которые нужно умножить на 4.
				sum2+=BasisFuncPow(i, j, Transfer(a+hh*(i+1)));//Значения с чётными индексами, которые нужно умножить на 2.
			}
			sum=BasisFuncPow(i, j, Transfer(a))+4*sum4+2*sum2-BasisFuncPow(i, j, Transfer(b));//Отнимаем значение f(b) так как ранее прибавили его дважды.
			I=I1;
			I1=e*sum;
			cout<<N<<" "<<I1<<" "<<fabs(I1-I)<<endl;
		}
		*/
		return value;
	}

	~Curve() {
		//cout<<"Object "<<this<<" has deleted"<<endl;
	}

}myCurve;//граница области, в которой решается диф. уравнение

double sgn(double a) //сигнум
{
	if (a > 0) return 1;
	if (a < 0) return -1;
	return 0;
}

//пространство тестовых функций и данных
namespace TestFuncAndCurve {
	//возможные параметризации для области
		//окружность радиуса MIN_RADIUS
	double u1(double t) {
		return MIN_RADIUS * cos(t);
	}
	double v1(double t) {
		return MIN_RADIUS * sin(t);
	}

	//соответствующая окружность радиуса MAX_RADIUS (около которой берутся базисные точки)
	double u1h(double t) {
		return MAX_RADIUS * cos(t);
	}
	double v1h(double t) {
		return MAX_RADIUS * sin(t);
	}

	//равносторонний треугольник со стороной MIN_RADIUS
	double u2(double t) {
		if ((t >= 0) && (t <= 2 * MIN_RADIUS))return t / 2;
		if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))return 3 * MIN_RADIUS - 1 * t;
	}
	double v2(double t) {
		if ((t >= 0) && (t <= MIN_RADIUS))return t / 2 * sqrt(3);
		if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))return -t / 2 * sqrt(3) + MIN_RADIUS * sqrt(3);
		if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))return 0;
	}
	double dis = MAX_RADIUS - MIN_RADIUS, mdx = dis / 2, mdy = mdx / sqrt(3);//кажется, из-за глобальности этих переменных всегда происходит стягивание одной вершины в одну и ту же точку
		//соответствующий равносторонний треугольник со стороной MAX_RADIUS
	double u2h(double t) {
		dis = MAX_RADIUS - MIN_RADIUS; mdx = dis / 2;//но если внутри каждой вставить это,оставив те глобальные, всё получится
		if ((t >= 0) && (t <= 2 * MAX_RADIUS))return t / 2 - mdx;
		if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))return 3 * MAX_RADIUS - 1 * t - mdx;
	}
	double v2h(double t) {
		dis = MAX_RADIUS - MIN_RADIUS; mdx = dis / 2; mdy = mdx / sqrt(3);
		if ((t >= 0) && (t <= MAX_RADIUS))return t / 2 * sqrt(3) - mdy;
		if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))return -t / 2 * sqrt(3) + MAX_RADIUS * sqrt(3) - mdy;
		if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))return 0 - mdy;
	}

	//квадрат со стороной MIN_RADIUS
	double u3(double t) {
		if ((t >= 0) && (t <= MIN_RADIUS))return t;
		if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))return MIN_RADIUS;
		if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))return 3 * MIN_RADIUS - t;
		if ((t >= 3 * MIN_RADIUS) && (t <= 4 * MIN_RADIUS))return 0;
	}
	double v3(double t) {
		if ((t >= 0) && (t <= MIN_RADIUS))return 0;
		if ((t >= MIN_RADIUS) && (t <= 2 * MIN_RADIUS))return t - MIN_RADIUS;
		if ((t >= 2 * MIN_RADIUS) && (t <= 3 * MIN_RADIUS))return MIN_RADIUS;
		if ((t >= 3 * MIN_RADIUS) && (t <= 4 * MIN_RADIUS))return 4 * MIN_RADIUS - t;
	}
	//соответствующий квадрат со стороной MAX_RADIUS
	double u3h(double t) {
		dis = MAX_RADIUS - MIN_RADIUS; mdx = dis / 2;
		if ((t >= 0) && (t <= MAX_RADIUS))return t - mdx;
		if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))return MAX_RADIUS - mdx;
		if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))return 3 * MAX_RADIUS - t - mdx;
		if ((t >= 3 * MAX_RADIUS) && (t <= 4 * MAX_RADIUS))return 0 - mdx;
	}
	double v3h(double t) {
		dis = MAX_RADIUS - MIN_RADIUS;	mdx = dis / 2;
		if ((t >= 0) && (t <= MAX_RADIUS))return 0 - mdx;
		if ((t >= MAX_RADIUS) && (t <= 2 * MAX_RADIUS))return t - MAX_RADIUS - mdx;
		if ((t >= 2 * MAX_RADIUS) && (t <= 3 * MAX_RADIUS))return MAX_RADIUS - mdx;
		if ((t >= 3 * MAX_RADIUS) && (t <= 4 * MAX_RADIUS))return 4 * MAX_RADIUS - t - mdx;
	}

	//острие
	double u4(double t) {
		if ((t >= 0) && (t <= MIN_RADIUS))return t;
		if ((t >= MIN_RADIUS) && (t <= 1.5*MIN_RADIUS))return 3 * MIN_RADIUS - 2 * t;
	}
	double v4(double t) {
		if ((t >= 0) && (t <= 0.5*MIN_RADIUS))return sqrt(MIN_RADIUS*MIN_RADIUS - (t - MIN_RADIUS)*(t - MIN_RADIUS));
		if ((t >= 0.5*MIN_RADIUS) && (t <= MIN_RADIUS))return sqrt(MIN_RADIUS*MIN_RADIUS - t*t);
		if ((t >= MIN_RADIUS) && (t <= 1.5*MIN_RADIUS))return 0;
	}

	double u4h(double t) {
		dis = MAX_RADIUS - MIN_RADIUS; mdx = dis / 2; mdy = mdx / sqrt(3);
		if ((t >= 0) && (t <= MAX_RADIUS))return t - mdx;
		if ((t >= MAX_RADIUS) && (t <= 1.5*MAX_RADIUS))return 3 * MAX_RADIUS - 2 * t - mdx;
	}
	double v4h(double t) {
		dis = MAX_RADIUS - MIN_RADIUS; mdx = dis / 2; mdy = mdx*sqrt(3) / 2;
		if ((t >= 0) && (t <= 0.5*MAX_RADIUS))return sqrt(MAX_RADIUS*MAX_RADIUS - (t - MAX_RADIUS)*(t - MAX_RADIUS)) - mdy;
		if ((t >= 0.5*MAX_RADIUS) && (t <= MAX_RADIUS))return sqrt(MAX_RADIUS*MAX_RADIUS - t*t) - mdy;
		if ((t >= MAX_RADIUS) && (t <= 1.5*MAX_RADIUS))return 0 - mdy;
	}

	//граничные функции и массив граничных функций
	double g1(BasisPoint point)
	{
		return cos(point.x)*cos(point.y);
	}
	double g2(BasisPoint point)
	{
		return sin(point.y);
	}
	double g3(BasisPoint point)
	{
		return point.x*point.x + 4;
	}

	double g4(BasisPoint point) {
		return CONSTANT;
	}
	/*double g40(BasisPoint point)//та же самая функция, но уже для самой myCurve
{
	double dis = MAX_RADIUS - MIN_RADIUS, dx = dis / 2, dy = dx / 2 * sqrt(3);
	BasisPoint center1, center2;//центры двух окружностей
	center1.x = 0 - dx;
	center1.y = 0 - dy;
	center2.x = MIN_RADIUS + dis - dx;
	center2.y = 0 - dy;

	if (point.y < 0 - dy) return CONSTANT;
	else
	{
		if ((point.x >= -dx) && (point.x <= MIN_RADIUS + dis / 2))//если первая координата принадлежит нужному отрезку
		{
			if (point.y == 0-dy) return 0;
			if (Eudistance(center1, point) == MIN_RADIUS + dis) return 0.5;
			if (Eudistance(center2, point) == MIN_RADIUS + dis) return -0.5;
			else return CONSTANT;
		}
		else return CONSTANT;
	}*/
	/*
		BasisPoint center1, center2;//центры двух окружностей
		center1.x = 0;
		center1.y = 0;
		center2.x = MIN_RADIUS;
		center2.y = 0;
		if (point.y < 0) return CONSTANT;
		else
		{
			if ((point.x >= 0) && (point.x <= MIN_RADIUS))//если первая координата принадлежит нужному отрезку
			{
				if (point.y == 0) return 0;
				if (Eudistance(center1, point) == MIN_RADIUS) return 0.5;
				if (Eudistance(center2, point) == MIN_RADIUS) return -0.5;
				else return CONSTANT;
			}
			else return CONSTANT;
		}
	}*/

	double g5(BasisPoint point)
	{
		return masPoints[0].Potentialf(point);
	}

	double g6(BasisPoint point)
	{
		return log(abs(point.x*point.y) + 1) + 2 * point.x;
	}
	double g7(BasisPoint point)
	{
		if (cos(point.x*point.y) != 0)return cos(2 * point.x) / cos(point.x*point.y);
		return 1;
	}

	double g8(BasisPoint point)
	{
		double dx = MIN_RADIUS / 2, dy = MIN_RADIUS / 2 * sqrt(3), argument;
		BasisPoint d = point; //d.x=-0.25;d.y=-0.25;
		d.x -= dx; d.y -= dy;//сдвиг к началу координат

		if (d.x == 0) argument = pi / 2 * sgn(d.y);
		else
		{
			if (d.y == 0) argument = pi*sgn(-1 + sgn(d.x));
			else argument = atan(d.y / d.x) + sgn(fabs(d.x) - d.x)*sgn(d.y)*pi;
		}
		//return argument;
		//cout<<d.x<<" "<<d.y<<" аргумент в доли: "<<argument/pi<<endl;

		if ((-pi <= argument) && (argument < -2 * pi / 3)) return -1. / 2;
		if ((-2 * pi / 3 <= argument) && (argument <= -pi / 3)) return 0;//return 1; //единицы будут
		if ((-pi / 3 < argument) && (argument <= pi / 2)) return 1. / 2;//return 1; //уже не будут
		/*if ((pi / 2 < argument) && (argument <= pi))*/return -1. / 2;
	}

	double(*GFunctions[KGF])(BasisPoint) = { g1,g2,g3,g4,g5,g6,g7,g8 };
}

//класс СЛАУ с методами их решения
class SLAU {
private:
	int size;//размерность системы

public:
	double **a, *b, *x;//указатели на матрицу и векторы (свободный и решения)

	double Nev(double **a, double *x, double *b, int t)//функция частичной невязки
	{
		double *Ax = new double[t];
		Func_in_matrix::Matrix_power(Ax, a, x, t);
		double s = 0;
		for (int i = 0; i < t; i++) s += ((Ax[i] - b[i])*(Ax[i] - b[i]));
		return sqrt(s);
	}

	double Error(int k) //частичная погрешность
	{
		double p = myCurve.Firstkind(N, N), sum = 0;

		double *Ax = new double[N];
		Func_in_matrix::Matrix_power(Ax, a, x, k);
		for (int i = 0; i < k; i++) sum += x[i] * Ax[i];
		double EPS = abs(p - sum);
		return sqrt(EPS);
	}

	void Make(int k) {//создание двумерного и одномерных динамических массивов с заданной размерностью
		size = k;
		a = new double *[size];
		for (int i = 0; i < size; i++)
			a[i] = new double[size];
		b = new double[size];
		x = new double[size];
		for (int i = 0; i < size; i++)x[i] = 0;
	}

	void Gauss(int t)//метод Гаусса
	{
		//создание вспомогательной матрицы системы
		double **matrix = new double *[size];
		for (int i = 0; i < size; i++)
			matrix[i] = new double[size + 1];
		//присваивание её элементам нужных значений
		for (int i = 0; i < size; i++) {
			matrix[i][size + 1] = b[i];
			for (int j = 0; j < size; j++) matrix[i][j] = a[i][j];
		}

		double m;//промежуточный множитель

		//прямой ход (без вывода матрицы, потому что работает)
		for (int j = 0; j < t; j++) {

			for (int i = j + 1; i < t; i++)
			{
				m = matrix[i][j] / matrix[j][j];

				for (int _h = j; _h < t; _h++)
					matrix[i][_h] -= m*matrix[j][_h];
				matrix[i][size + 1] -= matrix[j][size + 1] * m;
			}
		}

		//обратный ход		
		for (int j = t - 1; j >= 0; j--)
		{
		z2:
			for (int i = j - 1; i >= 0; i--)
			{
				if (matrix[j][j] == 0)
				{
					j--;
					goto z2;
				}
				m = matrix[i][j] / matrix[j][j];

				matrix[i][size + 1] -= matrix[j][size + 1] * m;
				matrix[i][j] -= m*matrix[j][j];
			}
		}

		//заполнение решения
		for (int i = 0; i < t; i++)
		{
			x[i] = matrix[i][size + 1] / matrix[i][i];
		}

		NEVA = Nev(a, x, b, t);//невязка фиксируется
	}

	void Holets(int z) //решение уравнения Ах=b методом Холецкого, присвоение вектору х значений решени
	{
		//создание вспомогательной матрицы
		double **t = new double *[z];
		for (int i = 0; i < z; i++)
			t[i] = new double[z];

		//прямой ход метода
		t[0][0] = sqrt(a[0][0]);
		for (int j = 1; j < z; j++)	t[0][j] = /*sqrt(fabs(*/a[0][j]/*))*/ / t[0][0];

		for (int i = 1; i < z; i++)
			for (int j = 0; j < z; j++)
			{
				if (j < i) t[i][j] = 0;
				else if (i == j)
				{
					double sum = 0;
					for (int k = 0; k <= j - 1; k++) sum += t[k][j] * t[k][j];
					t[j][j] = sqrt(fabs(a[j][j] - sum));//без модуля не получается
				}
				else
				{
					double sum = 0;
					for (int k = 0; k <= i - 1; k++) sum += t[k][i] * t[k][j];
					t[i][j] = (a[i][j] - sum) / t[i][i];
				}

			}
		/*
		for (int j = 1; j < size; j++) {
			double sum = 0;
			for (int k = 0; k <= j - 1; k++) sum += t[k][j] * t[k][j];
			t[j][j] = sqrt(fabs(a[j][j] - sum));
		}

		for (int i = 0; i < size; i++)
			for (int j = 0; j < size; j++)
			{
				if (i < j) {
					double sum = 0;
					for (int k = 0; k <= i - 1; k++) sum += t[k][i] * t[k][j];
					t[i][j] = (a[i][j] - sum) / t[i][i];
				}
				else if (j < i) {
					t[i][j] = 0;
				}
			}*/

			/*
						//исходная матрица
						cout << "матрица a:" << endl;
						for (int i = 0; i < z; i++)
						{
							for (int j = 0; j < z; j++)cout << a[i][j] << " ";
							cout << endl;
						}
						//произведение матрицы t на транспонированную
						double **c = new double *[z];
						for (int i = 0; i < z; i++)
							c[i] = new double[z];

						for (int i = 0; i < z; i++)
							for (int j = 0; j < z; j++) {
								double s = 0;
								for (int k = 0; k < size; k++)s += t[k][i] * t[k][j];
								c[i][j] = s;
							}

						cout << endl;
						cout << "матрица c=(t)T*t:" << endl;
						for (int i = 0; i < z; i++)
						{

							for (int j = 0; j < z; j++)cout << c[i][j] << " ";
							cout << endl;
						}

						cout << endl;
						cout << "матрица t:" << endl;
						for (int i = 0; i < z; i++)
						{

							for (int j = 0; j < z; j++)cout << t[i][j] << " ";
							cout << endl;
						}
			*/
			//обратный ход метода     
		double *y = new double[z];
		y[0] = b[0] / t[0][0];
		//cout << y[0]<< endl;
		for (int i = 1; i < z; i++)
		{
			double sum = 0;
			for (int k = 0; k <= i - 1; k++) sum += t[k][i] * y[k];
			y[i] = (b[i] - sum) / t[i][i];
			//cout << y[i]<< endl;
		}

		x[z - 1] = y[z - 1] / t[z - 1][z - 1];
		for (int i = z - 2; i >= 0; i--)
		{
			double sum = 0;
			for (int k = i + 1; k < z; k++) sum += t[i][k] * x[k];
			x[i] = (y[i] - sum) / t[i][i];
		}

		NEVA = Nev(a, x, b, z);
	}

	void Jak(int t)//метод Якоби 
	{
		for (int i = 0; i < t; i++) x[i] = 0/*b[i]*/;//первое приближение - свободный вектор
		double E, EPSJ = 0.000001, NE = Nev(a, x, b, t);
		int num = 0, maxI = t*t;//переменные, связанные с количеством итераций
		while ((Nev(a, x, b, t) > EPSJ) && (num <= maxI) /*&& (Nev(a, x, b, t) <= NE)*/)
		{
			NE = Nev(a, x, b, t);
			//cout<<NE<<endl;
			for (int i = 0; i < t; i++)
			{
				E = 0;
				for (int j = 0; j < t; j++)
				{
				z1:
					if (j == i)
					{
						j++;
						goto z1;
					}
					else
					{
						E += a[i][j] * x[j];
					}
				}
				x[i] = (b[i] - E) / a[i][i];
			}
			num++;

		}

		NEVA = Nev(a, x, b, t);
	}

	void Speedy(int t)//метод наискорейшего спуска
	{
		for (int i = 0; i < t; i++) x[i] = b[i];//первое приближение - свободный вектор

		double E = Nev(a, x, b, t), EPSJ = /*0.000001*/EPSS;
		int num = 0, maxI = t*t;//переменные, связанные с количеством итераций
		double *Ax = new double[t];
		double *r = new double[t];
		double *Ar = new double[t];
		double *s = new double[t];
		double *sum = new double[t];

		while ((Nev(a, x, b, t) > EPSJ) && (num <= maxI))//пока 1) невязка большая, 2) шагов ещё не много, 3) невязка убывает
		{
			Func_in_matrix::Matrix_power(Ax, a, x, t);//произведение матрицы на вектор Ax=a*x
			Func_in_matrix::Vector_difference(r, Ax, b, t);//разность двух векторов r=Ax-b
			Func_in_matrix::Matrix_power(Ar, a, r, t);//Ar=a*r
			double tau = Func_in_matrix::Scalar_power(r, r, t) / Func_in_matrix::Scalar_power(Ar, r, t);//скалярное произведение двух векторов tau=(r,r)/(Ar,r)
			Func_in_matrix::Vector_on_scalar(s, -tau, r, t);//умножение вектора на скаляр s=-tau*r
			Func_in_matrix::Vector_sum(sum, s, x, t);//сумма векторов sum=x+s=x-tau*r...

		   /*if(Nev(a, x, b, t)>Nev(a,sum,b,t))
		   {*/
			E = Nev(a, x, b, t);//фиксируем невязку
			Func_in_matrix::Vector_assingment(x, sum, t);//присваивание одному вектору другого
		/*}
		else break;*/

		//cout << E << " " << num << endl;

		/*Func_in_matrix::Vector_sum(sum, b, x, t);
		Func_in_matrix::Vector_on_scalar(s, 1/2, sum, t);
		Func_in_matrix::Vector_assingment(x, sum, t);*/

			num++;

		}
		NEVA = Nev(a, x, b, t);
	}

	void Speedy0(int t)//метод наискорейшего спуска (без начального присвоения)
	{
		double E = Nev(a, x, b, t), EPSJ = /*0.000001*/EPSS;
		int num = 0, maxI = t/**t*/;//переменные, связанные с количеством итераций
		double *Ax = new double[t];
		double *r = new double[t];
		double *Ar = new double[t];
		double *s = new double[t];
		double *sum = new double[t];

		while ((Nev(a, x, b, t) > EPSJ) && (num <= maxI))//пока 1) невязка большая, 2) шагов ещё не много, 3) невязка убывает
		{
			Func_in_matrix::Matrix_power(Ax, a, x, t);//произведение матрицы на вектор Ax=a*x
			Func_in_matrix::Vector_difference(r, Ax, b, t);//разность двух векторов r=Ax-b
			Func_in_matrix::Matrix_power(Ar, a, r, t);//Ar=a*r
			double tau = Func_in_matrix::Scalar_power(r, r, t) / Func_in_matrix::Scalar_power(Ar, r, t);//скалярное произведение двух векторов tau=(r,r)/(Ar,r)
			Func_in_matrix::Vector_on_scalar(s, -tau, r, t);//умножение вектора на скаляр s=-tau*r
			Func_in_matrix::Vector_sum(sum, s, x, t);//сумма векторов sum=x+s=x-tau*r...

		   /*if(Nev(a, x, b, t)>Nev(a,sum,b,t))
		   {*/
			E = Nev(a, x, b, t);//фиксируем невязку
			Func_in_matrix::Vector_assingment(x, sum, t);//присваивание одному вектору другого
		/*}
		else break;*/

		//cout << E << " " << num << endl;

		/*Func_in_matrix::Vector_sum(sum, b, x, t);
		Func_in_matrix::Vector_on_scalar(s, 1/2, sum, t);
		Func_in_matrix::Vector_assingment(x, sum, t);*/

			num++;

		}
	}

	void Minimize_coef(int t)//минимизация коэффициентов
	{
		cout << "Точность аппроксимации при числе функций " << t << " =" << endl;
		cout << "до использования покоординатной минимизации:\t" << Error(t) << endl;
		double sum = 0;
		int r = 5;
		for (int k = 1; k <= r; k++)
		{
			for (int i = t - 1; i >= 0; i--)
			{
				for (int j = 0; j < t; j++)
				{
					if (j != i) sum += x[j] * a[i][j];
				}
				x[i] = (b[i] - sum) / a[i][i];
				sum = 0;
			}
			cout << "после использования покоординатной минимизации " << k << " раз:\t" << Error(t) << endl;
		}

	}
	/*
		void Minimize_coef2(int t,double pog)//минимизация коэффициентов
		{
			double sum = 0;
	while(Error(t)>pog){
				for (int i = t - 1; i >= 0; i--)
				{
					for (int j = 0; j < t; j++)
					{
						if (j != i) sum += x[j] * a[i][j];
					}
					x[i] = (b[i] - sum) / a[i][i];
					sum = 0;
				}

	}
		}
	*/
	void GaussSpeedy(int t)
	{
		Gauss(t);
		Speedy0(t);
		NEVA = Nev(a, x, b, t);
	}

	void GaussSpeedyMinimize(int t)//гибридный с использованием покоординатной минимизации
	{
		Gauss(t);
		Speedy0(t);
		Minimize_coef(t);
		NEVA = Nev(a, x, b, t);
	}

	void UltraHybrid(int t)//гибридный с координатной минимизацией по последней координате
	{
		//double tmp=10;
		//if(t!=0) tmp=Error(t-1);
		double *c = new double[t];
		//if(t>=1){Gauss+Speedy(t-1);}
		for (int i = 0; i < t - 1; i++)c[i] = x[i];

		double sum = 0;
		GaussSpeedy(t);

		if ((VALUE_FOR_ULTRA < Error(t)) && (t >= 1))//если погрешность выросла - исправить это
		{
			for (int i = 0; i < t - 1; i++) x[i] = c[i]; x[t - 1] = 0;
			if (t != 0)
			{
				for (int j = 0; j < t - 1; j++)
				{
					sum += x[j] * a[t - 1][j];
				}
				x[t - 1] = (b[t - 1] - sum) / a[t - 1][t - 1];
				sum = 0;

				double tmp1 = Error(t);

				if (VALUE_FOR_ULTRA < tmp1)//погрешность опять выросла
				{
					for (int i = 0; i < t - 1; i++) x[i] = c[i]; x[t - 1] = 0;
				}
			}
		}
		VALUE_FOR_ULTRA = Error(t);

		NEVA = Nev(a, x, b, t);

	}

	/*	void SuperSpeedy(int t)//спуск по окончательному элементу
		{
			double *c = new double[t]; for (int i = 0; i < t - 1; i++)c[i] = x[i];
			double sum = 0;
			//tmp=Error(t);
			//cout<<" f "<<endl;
			Gauss+Speedy(t);
			//cout<<bool(tmp<Error(t))<<" "<<tmp<<" "<<Error(t)<<endl;

			if (tmp < Error(t))//если погрешность выросла - исправить это
			{
				//cout<<Error(t)<<endl;
				for (int i = 0; i < t - 1; i++) x[i] = c[i]; x[t - 1] = 0;
				//cout<<Error(t)<<endl;
				//Minimize_coef2(t,tmp);
				//Minimize_coef(t);
				if (t != 0)
				{
					for (int j = 0; j < t - 1; j++)
					{
						sum += x[j] * a[t - 1][j];
					}
					x[t - 1] = (b[t - 1] - sum) / a[t - 1][t - 1];
					sum = 0;

					double tmp1 = Error(t);

					if (tmp < tmp1)//погрешность опять выросла
					{
						for (int i = 0; i < t - 1; i++) x[i] = c[i]; x[t - 1] = 0;
					}
				}

							double E = Nev(a, x, b, t), EPSJ = EPSS;
							int num = 0, maxI = t*t*t;//переменные, связанные с количеством итераций
							double *Ax = new double[t];
							double *r = new double[t];
							double *Ar = new double[t];
							double *s = new double[t];
							double *sum = new double[t];
							double pog1 = Error(t),pog2=pog1;

							while ((Nev(a, x, b, t) > EPSJ) && (num <= maxI) && (pog2 <= pog1))//пока 1) невязка большая, 2) шагов ещё не много, 3) погрешность убывает
							{
								pog1 = Error(t);
								Func_in_matrix::Matrix_power(Ax, a, x, t);//произведение матрицы на вектор Ax=a*x
								Func_in_matrix::Vector_difference(r, Ax, b, t);//разность двух векторов r=Ax-b
								Func_in_matrix::Matrix_power(Ar, a, r, t);//Ar=a*r
								double tau = Func_in_matrix::Scalar_power(r, r, t) / Func_in_matrix::Scalar_power(Ar, r, t);//скалярное произведение двух векторов tau=(r,r)/(Ar,r)
								Func_in_matrix::Vector_on_scalar(s, -tau, r, t);//умножение вектора на скаляр s=-tau*r
								Func_in_matrix::Vector_sum(sum, s, x, t);//сумма векторов sum=x+s=x-tau*r...

								E = Nev(a, x, b, t);//фиксируем невязку
								Func_in_matrix::Vector_assingment(x, sum, t);//присваивание одному вектору другого
								num++;
								pog2=Error(t);
							}

			}
			tmp = Error(t);

			NEVA = Nev(a, x, b, t);
		}*/

	~SLAU() {
		//cout<<"Object "<<this<<" has deleted"<<endl;
	}

}MySLAU;//система, которую придётся решить

//перечисление методов
enum Method { Gauss, Holets, Jak, Speedy, GaussSpeedy, GaussSpeedyMinimize, UltraHybrid } baseMethod = UltraHybrid;
void Method_des(int i, Method mett)
{
	switch (mett)
	{
	case Gauss:
		MySLAU.Gauss(i);
		break;
	case Holets:
		MySLAU.Holets(i);
		break;
	case Jak:
		MySLAU.Jak(i);
		break;
	case Speedy:
		MySLAU.Speedy(i);
		break;
	case GaussSpeedy:
		MySLAU.GaussSpeedy(i);
		break;
	case GaussSpeedyMinimize:
		MySLAU.GaussSpeedyMinimize(i);
		break;
	case UltraHybrid:
		MySLAU.UltraHybrid(i);
		break;
	}
}

namespace ForDesigion
{

	double Random_eps() //случайное отклонение точки от кривой
	{
		double e = rand() % 25 * EPS / 25, p = rand(), q = rand();
		if (sgn(p - q) < 0) return sgn(p - q)*min(MAX_RADIUS - MIN_RADIUS, e) / 100;
		return sgn(p - q)*e;
	}

	void FillMassiv(Curve c, int z) //заполнить массив базисных точек не через файл - около такой кривой и от стольки точек
	{
		N = z;
		//delete[] masPoints;//очистить память под массив
		masPoints = new BasisPoint[N];
		//goldmas = new double[N];//массив чисел для равномерного расположения точек
		//fillgoldmas(c,z,goldmas);

		for (int i = 0; i < z; i++)
		{
			masPoints[i] = c.Transfer(c.a + (c.b - c.a)*i / z);
			//masPoints[i] = c.Transfer(goldmas[i]);
			double l = Random_eps() / sqrt(2);
			masPoints[i].x += l;
			masPoints[i].y += l;

		}
	}

	void RandomSwapping(int p) //перемешать массив masPoints в р действий
	{
		for (int i = 1; i <= p; i++)
		{
			int a = rand() % N, b = rand() % N;
			swap(masPoints[a], masPoints[b]);
		}
		/*
		//рекурсивная функция для fillgoldmas
		double rek(Curve c, int i) {
			return 0;
		}
		//заполнить массив особенных чисел для равномерного распределения
		void fillgoldmas(Curve c, int k, double *m) {
			m[0] = c.a;
			m[1] = (c.a + c.b) / 2;
			int t = 2;
			while (t <= k) {
				m[t] = m[t / 2] / 2;//первый элемент слоя
				for (int i = 1; i <= (t - 1) / 2; i++) m[t + i] = m[t] + rek(c, i);//первая половина элементов слоя
				for (int i = (t - 1) / 2 + 1; i <= t - 1; i++) {//вторая половина элементов слоя
					int e = 1;
					m[t + i] = m[t + i - e];
					e++;
				}
				t *= 2;
			}

		}
		*/
	}

	void ReadFile()
	{
		char buff[150];//создание вспомогательного символьного массива
		int k = 0; //счётчик
		double r;//вспомогательное действительное число	
		ifstream fin("input.txt"); //объявление объекта для чтения из файла
		fin >> buff;//чтение ненужной текстовой информации
		fin >> CIRCLE;//чтение номера области
		fin >> buff;
		fin >> GF;//чтение номера граничной функции
		fin >> buff;

		while (fin >> r) k++;//пока координаты точек считываются, прибавлять к счётчику 
		N = k / 2;//вычисление мощности множества базисных точек
		masPoints = new BasisPoint[N];//создание массива точек

		fin.clear();
		fin.seekg(0, ios::beg);//поставить указатель на начало файла, чтобы считать значения заново, но уже с записью
		fin >> buff; fin >> CIRCLE; fin >> buff; fin >> GF; fin >> buff;
		for (int i = 0; i < N; i++) fin >> masPoints[i].x >> masPoints[i].y;//заполнение массива точек
		fin.close();
	}

	void read_rand()
	{
		char buff[150];//создание вспомогательного символьного массива
		int k = 0; //счётчик
		double r;//вспомогательное действительное число	
		ifstream fin("input.txt"); //объявление объекта для чтения из файла
		fin >> buff;//чтение ненужной текстовой информации
		fin >> CIRCLE;//чтение номера области
		fin >> buff;
		fin >> GF;//чтение номера граничной функции
		fin.close();
	}

	void DeleteElement(int i, BasisPoint *a) //удаление из массива элемента i
	{
		for (int j = i; j < N - 1; j++) a[j] = a[j + 1];
		N--;
	}

	void ExceptionMas(BasisPoint *masPoints)//отсеивание из массива повторяющихся элементов
	{
		for (int i = 1; i < N; i++)
		{
			if (masPoints[i - 1].x == masPoints[i].x && masPoints[i - 1].y == masPoints[i].y) { DeleteElement(i, masPoints);  i--; }//удаление из массива точек повторяющихся элементов
		}
	}
	bool Comporator(BasisPoint a, BasisPoint b)//функция компоратора
	{
		if (a.y < b.y)return true;//cравнение по второй координате
		else if (a.y > b.y) return false;
		else return a.x < b.x;//если вторые координаты равны, сравнение по первой координате
	}
	void Screening()
	{
		sort(masPoints, masPoints + N, Comporator);//сортировка точек по компаратору
		ExceptionMas(masPoints);//отсеивание повторяющихся точек
	}

	//сообщение об ошибке
	void WriteAboutError()
	{
		ofstream fout("output.txt");
		fout << "НЕСУЩЕСТВУЮЩИЙ НОМЕР ГРАНИЧНОЙ ФУНКЦИИ ИЛИ ОБЛАСТИ! Всего имеется " << CIRCLE << " границ области и " << KGF << " граничных функций!" << endl;
		exit(0);
		fout.close();
	}
	void Display() //сопоставление первым двум числам из файла области и граничной функци
	{
		if ((GF <= 0) || (GF > KGF) || (CIRCLE > MAXCIRCLE) || (CIRCLE <= 0)) WriteAboutError();//если выбранный номер больше числа граничных функций на кривую
		else
		{
			fi = TestFuncAndCurve::GFunctions[GF - 1];//граничная функция - функция с номером GF для кривой с номером CIRCLE

			/*if(GF=4) fi0=TestFuncAndCurve::g40;
			else fi0=fi;*/

			Curve c1(0, 2 * pi, TestFuncAndCurve::u1, TestFuncAndCurve::v1);
			Curve c2(0, 3 * MIN_RADIUS, TestFuncAndCurve::u2, TestFuncAndCurve::v2);
			Curve c3(0, 4 * MIN_RADIUS, TestFuncAndCurve::u3, TestFuncAndCurve::v3);
			Curve c4(0, 1.5 * MIN_RADIUS, TestFuncAndCurve::u4, TestFuncAndCurve::v4);
			switch (CIRCLE) {
			case 1:
				myCurve = c1;
				break;
			case 2:
				myCurve = c2;
				break;
			case 3:
				myCurve = c3;
				break;
			case 4:
				myCurve = c4;
				//fi = TestFuncAndCurve::g40;
				break;
			}
		}

	}

	void Building(int t)//построение массива точек при чтении из файла или при генерировании
	{
		if (t > 0)//заполнение массива базисных точек не из файла 
		{
			read_rand();//чтение данных не из файла
			Display();//сопоставление первым двум числам из файла - области и граничной функции

			Curve c1(0, 2 * pi, TestFuncAndCurve::u1h, TestFuncAndCurve::v1h);//кривая, в окрестности которой эти точки берутся
			Curve c2(0, 3 * MAX_RADIUS, TestFuncAndCurve::u2h, TestFuncAndCurve::v2h);
			Curve c3(0, 4 * MAX_RADIUS, TestFuncAndCurve::u3h, TestFuncAndCurve::v3h);
			Curve c4(0, 1.5 * MAX_RADIUS, TestFuncAndCurve::u4h, TestFuncAndCurve::v4h);

			switch (CIRCLE) {
			case 1:
				FillMassiv(c1, t);//заполнить массив
				break;
			case 2:
				FillMassiv(c2, t);//заполнить массив
				break;
			case 3:
				FillMassiv(c3, t);//заполнить массив
				break;
			case 4:
				FillMassiv(c4, t);//заполнить массив
				//fi= TestFuncAndCurve::GFunctions[GF - 1];
				break;
			}

			//for (int i = 0; i < N; i++) cout<<masPoints[i].x <<" "<< masPoints[i].y<<endl;cout<<endl;
			RandomSwapping(N * 2);//перемешать массив в столько действий
			//for (int i = 0; i < N; i++) cout<<masPoints[i].x <<" "<< masPoints[i].y<<endl;cout<<endl;
		}
		else
		{
			ReadFile();//чтение данных из файла
			Display();//сопоставление первым двум числам из файла - области и граничной функции
			Screening();//отсеивание из массива точек одинаковых точек
			RandomSwapping(N * 2);//перемешать массив
		}
		/*cout<<CIRCLE<<" "<<GF<<endl;
		for (int i = 0; i < N; i++) cout<<masPoints[i].x <<" "<< masPoints[i].y<<endl;*/
	}
	void Building(int t, int g, int cu)//построение массива точек при чтении из файла или при генерировании
	{
		GF = g;
		CIRCLE = cu;
		Display();//сопоставление первым двум числам из файла - области и граничной функции
		Curve c1(0, 2 * pi, TestFuncAndCurve::u1h, TestFuncAndCurve::v1h);//кривая, в окрестности которой эти точки берутся
		Curve c2(0, 3 * MAX_RADIUS, TestFuncAndCurve::u2h, TestFuncAndCurve::v2h);
		Curve c3(0, 4 * MAX_RADIUS, TestFuncAndCurve::u3h, TestFuncAndCurve::v3h);
		Curve c4(0, 1.5 * MAX_RADIUS, TestFuncAndCurve::u4h, TestFuncAndCurve::v4h);

		switch (CIRCLE) {
		case 1:
			FillMassiv(c1, t);//заполнить массив
			break;
		case 2:
			FillMassiv(c2, t);//заполнить массив
			break;
		case 3:
			FillMassiv(c3, t);//заполнить массив
			break;
		case 4:
			FillMassiv(c4, t);//заполнить массив
			//fi= TestFuncAndCurve::GFunctions[GF - 1];
			break;
		}

		//for (int i = 0; i < N; i++) cout<<masPoints[i].x <<" "<< masPoints[i].y<<endl;cout<<endl;
		RandomSwapping(N * 2);//перемешать массив в столько действий
		//for (int i = 0; i < N; i++) cout<<masPoints[i].x <<" "<< masPoints[i].y<<endl;cout<<endl;
	}
	void WriteError(double *x, double EPS) //вывод точности (после поиска решения)
	{
		//запись в файл
		char buf[250];
		string str;
		memset(buf, 0, sizeof(buf));
		string d1 = toString(baseMethod), d2 = toString(N), d3 = toString(MIN_RADIUS), d4 = toString(MAX_RADIUS), d5 = toString(GF), d6 = toString(CIRCLE);
		str = "Вектор решения и точность аппроксимации для функции " + d5 + " методом " + d1 + " при числе функций " + d2 + ".txt";
		strncpy(buf, str.c_str(), sizeof(buf) - 1);

		ofstream fout(buf);
		fout << "Вектор решения (" << N << " точек):" << endl;
		for (int i = 0; i < N; i++) fout << x[i] << endl;
		fout << "Получившаяся невязка при количестве функций " << N << " равна " << NEVA << endl;//вывести невязку
		fout << "Разница приближённого решения с граничной функцией при числе базисных функций " << N << " равна " << EPS << endl;
		RESULT = EPS;
		fout.close();
	}

	void Matrix_power(double *Ax, double **a, double *x) //присвоение вектору Ax произведения a*x матрицы на вектор
	{
		for (int i = 0; i < N; i++)
		{
			double sum = 0;
			for (int j = 0; j < N; j++) sum += a[i][j] * x[j];
			Ax[i] = sum;
		}
	}

	//функция невязки
	double Nev(double **a, double *x, double *b)
	{
		double *Ax = new double[N];
		Matrix_power(Ax, a, x);
		double s = 0;
		for (int i = 0; i < N; i++) s += ((Ax[i] - b[i])*(Ax[i] - b[i]));
		return sqrt(s);
	}

	void Error()//нахождение и вывод точности по известному решению
	{
		double p = myCurve.Firstkind(N, N), sum = 0;
		double *Ax = new double[N];
		Matrix_power(Ax, MySLAU.a, MySLAU.x);
		for (int i = 0; i < N; i++) sum += MySLAU.x[i] * Ax[i];//myCurve.Firstkind(i,N);
		WriteError(MySLAU.x, sqrt(abs(p - sum)));
	}

	void Search()
	{
		MySLAU.Make(N);//создать систему порядка, равного числу базисных точек

		for (int i = 0; i < N; i++) //заполнить систему
		{
			MySLAU.b[i] = myCurve.Firstkind(i, N);
			MySLAU.a[i][i] = myCurve.Firstkind(i, i);
			for (int j = i + 1; j < N; j++) //так как матрица симметрическая
			{
				double tmp = myCurve.Firstkind(i, j);
				MySLAU.a[i][j] = tmp;
				MySLAU.a[j][i] = tmp;
			}
		}

		/*cout << "матрица системы и свободный вектор: " << endl;
		for (int i = 0; i < N; i++)
		{
			cout << endl;
			for (int j = 0; j < N; j++)cout << MySLAU.a[i][j] << " ";
		}
		cout << endl; cout << endl;
		for (int i = 0; i < N; i++) cout << MySLAU.b[i] << endl; cout << endl;*/


		//-----------------------------------------решить систему-----------------------------------
		Method_des(N, baseMethod);
		//cout << "невязка для "<<N<<" функций = " << Nev(MySLAU.a, MySLAU.x, MySLAU.b) << endl;

		//вывести решение, найти и вывести погрешность		
		Error();
	}
}

namespace ForFixity//для рисования графика зависимости аппроксимации от числа точек
{
	double Min(double a, double b)
	{
		if (a < b) return a;
		return b;
	}

	double ReturnError() //возврат погрешности
	{
		double p = myCurve.Firstkind(N, N), sum = 0;

		double *Ax = new double[N];
		ForDesigion::Matrix_power(Ax, MySLAU.a, MySLAU.x);
		for (int i = 0; i < N; i++) sum += MySLAU.x[i] * Ax[i];
		return sqrt(abs(p - sum));
	}

	void Show(double *Errors) //рисование самого графика на основе массива точек 
	{
		double max = log10(Errors[0]), min = log10(Errors[N - 1]);
		for (int i = 0; i < N; i++)//определение минимума и максимума
		{
			double tmp = log10(Errors[i]);
			if (tmp < min) min = tmp;
			if (tmp > max) max = tmp;
		}

		//задать пределы окна
		double	a0 = -1 + 0,
			b0 = 1 + N,
			c0 = -1 + min,
			d0 = 1 + max;

		SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
		SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
		SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
		xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х равным 1 и 0.1 по у

		SetColor(250, 0, 0); // задаем цвет линии (красный)
		SetPoint(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку 

		for (int i = 0; i < N; i++) Line2(i + 1, log10(Errors[i])); //cout<<i<<" "<<Errors[i]<<endl;

		char buf[250];
		string str;
		memset(buf, 0, sizeof(buf));
		string d2 = toString(N), d3 = toString(GF), d4 = toString(CIRCLE), d5 = toString(GF), d6 = toString(baseMethod);
		//str = "График 2 качества аппроксимации в зависимости от числа базисных функций (" + d2 + ").bmp";
		str = "График 2 качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").bmp";
		strncpy(buf, str.c_str(), sizeof(buf) - 1);
		CloseWindow(buf);
		//CloseWindow("График 2 качества аппроксимации в зависимости от числа базисных функций.bmp");// закрываем окно (создаем bmp-файл)
	}

	void Show(double *ErrorsA, double *ErrorsB, Method A, Method B) //рисование самого графика на основе массива точек 
	{
		double max = log10(ErrorsA[0]), min = log10(ErrorsA[N - 1]);
		for (int i = 0; i < N; i++)//определение минимума и максимума
		{
			double tmp = log10(ErrorsA[i]);
			double tmpp = log10(ErrorsB[i]);
			if (tmp < min) min = tmp;
			if (tmp > max) max = tmp;
			if (tmpp < min) min = tmpp;
			if (tmpp > max) max = tmpp;
		}

		//задать пределы окна
		double	a0 = -1 + 0,
			b0 = 1 + N,
			c0 = -1 + min,
			d0 = 1 + max;

		SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
		SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
		SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
		xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х равным 1 и 0.1 по у

		SetColor(250, 0, 0); // задаем цвет линии (красный)
		SetPoint(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку 

		for (int i = 0; i < N; i++) Line2(i + 1, log10(ErrorsA[i])); //cout<<i<<" "<<Errors[i]<<endl;

		SetColor(0, 250, 0); // задаем цвет линии (зеленый)
		SetPoint(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку 
		for (int i = 0; i < N; i++) Line2(i + 1, log10(ErrorsB[i])); //cout<<i<<" "<<Errors[i]<<endl;

/*
		SetColor(0, 0, 250); // задаем цвет линии (синий)
		SetPoint(0, myCurve.Firstkind(N, N)); // устанавливаем курсор на точку
		for (int i = 0; i < N; i++) Line2(i + 1, Min(log10(ErrorsA[i]),log10(ErrorsB[i]))); //cout<<i<<" "<<Errors[i]<<endl;
*/

		char buf[250];
		string str;
		memset(buf, 0, sizeof(buf));
		string d2 = toString(N), d3 = toString(GF), d4 = toString(CIRCLE), d5 = toString(A), d6 = toString(B);
		//str = "График 2 качества аппроксимации в зависимости от числа базисных функций (" + d2 + ").bmp";
		str = "График 2 качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") в зависимости от числа базисных функций (" + d2 + ") при методах " + d5 + " (красный) и " + d6 + " (зелёный).bmp";
		strncpy(buf, str.c_str(), sizeof(buf) - 1);
		CloseWindow(buf);
		//CloseWindow("График 2 качества аппроксимации в зависимости от числа базисных функций.bmp");// закрываем окно (создаем bmp-файл)
	}

	double Error(int k) //частичная погрешность
	{
		double p = myCurve.Firstkind(N, N), sum = 0;

		double *Ax = new double[N];
		Func_in_matrix::Matrix_power(Ax, MySLAU.a, MySLAU.x, k);
		for (int i = 0; i < k; i++) sum += MySLAU.x[i] * Ax[i];
		double EPS = abs(p - sum);
		return sqrt(EPS);

	}

	void Create(double *Errors)
	{
		cout << "Для графика в зависимости от числа точек:" << endl;
		for (int i = 0; i < N/* - 1*/; i++)
		{
			Method_des(i, baseMethod);//решить частичную систему нужным методом
			Errors[i] = Error(i);//заполнить массив погрешностей
			/*
			if ((i > 0) && (Errors[i] > Errors[i - 1]))//если возникает скачок, перерешать методом наискорейшего спуска
			{
				Method_des(i, Gauss+Speedy);//решить частичную систему нужным методом
				Errors[i] = Error(i);//заполнить массив погрешностей
			}
			*/

			if (i % 10 == 0) cout << i + 1 << " -> " << Errors[i] << endl;//выводить каждую десятую
		}
		//Errors[N - 1] = RESULT;
	}

	void Create(double *ErrorsA, double *ErrorsB, Method A, Method B)
	{
		cout << "Для графика в зависимости от числа точек:" << endl;
		for (int i = 0; i < N; i++)
		{
			Method_des(i, A);//решить частичную систему нужным методом
			ErrorsA[i] = Error(i);//заполнить массив погрешностей
			if (i % 10 == 0) cout << i + 1 << " (A) -> " << ErrorsA[i] << endl;//выводить каждую десятую

		}//при использовании ультра-гибрида приходится разбивать на два цикла
		for (int i = 0; i < N; i++)
		{
			Method_des(i, B);//решить частичную систему нужным методом
			ErrorsB[i] = Error(i);//заполнить массив погрешностей
			if (i % 10 == 0) cout << i + 1 << " (B) -> " << ErrorsB[i] << endl;//выводить каждую десятую
		}
	}

	void WriteAboutZero(int NN, int GFf, int cCIRCLE, Method meth, int num)//сообщение об ошибке для графика 2
	{
		//запись в файл
		char buf[250];
		string str;
		memset(buf, 0, sizeof(buf));
		string d2 = toString(NN), d3 = toString(GFf), d4 = toString(cCIRCLE), d6 = toString(meth);
		str = "Сообщение об ошибке для графика 2 погрешностей (без log) качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").txt";
		strncpy(buf, str.c_str(), sizeof(buf) - 1);
		ofstream fout(buf);
		fout << "Невозможно построить логарифмический график, поскольку на элементе " << num << "(=0) функция принимает значение -infinity";
		fout.close();
	}

	void WriteMassiv(double *x) //вывод массива погрешностей
	{
		//запись в файл
		char buf[250];
		string str;
		memset(buf, 0, sizeof(buf));
		string d2 = toString(N), d3 = toString(GF), d4 = toString(CIRCLE), d6 = toString(baseMethod);
		str = "Файл 2 погрешностей (без log) качества аппроксимации функции (" + d3 + ") на области (" + d4 + ") методом " + d6 + " в зависимости от числа базисных функций (" + d2 + ").txt";
		strncpy(buf, str.c_str(), sizeof(buf) - 1);
		ofstream fout(buf);

		zero = true;//нулей нет 
		int j;//номер с нулём
		double min = x[0], max = x[0], mins = -1, maxs = -1;//минимальный элемент, максимальный, первый скачок, максимальный скачок
		int mini = 0, maxi = 0, mins1 = -1, maxs1 = -1, mins2 = -1, maxs2 = -1;//соответствующие номера
		for (int i = 0; i < N; i++)
		{
			fout << x[i] << endl;
			if (x[i] == 0)
			{
				zero = false;//есть нули
				j = i;//зафиксировать номер
			}
			//поиск минимального и максимального элемента
			if (x[i] < min) {
				min = x[i]; mini = i;
			}
			else if (x[i] > max) {
				max = x[i]; maxi = i;
			}
		}

		for (int i = 1; i < N; i++)
		{
			if (x[i] - x[i - 1] > 0)
			{
				mins = x[i] - x[i - 1];
				mins1 = i - 1;
				mins2 = i;
				maxs = mins;
				maxs1 = mins1;
				maxs2 = mins2;
				for (int j = i + 1; j < N; j++)
				{
					double p = x[j] - x[j - 1];
					if (p > 0)
					{
						if (p > maxs)
						{
							maxs = p;
							maxs1 = j - 1;
							maxs2 = j;
						}
					}
				}
				goto end1;
			}
		}
	end1:
		fout << endl;
		fout << "Анализ массива:" << endl;
		fout << "1)Минимальное значение " << min << " на элементе " << mini << endl;
		fout << "2)Максимальное значение " << max << " на элементе " << maxi << endl;
		if (mins > 0)
		{
			fout << "3)Первый скачок " << mins << " с элементa " << mins1 << " на элемент " << mins2 << endl;
			fout << "4)Максимальный скачок " << maxs << " с элементa " << maxs1 << " на элемент " << maxs2 << endl;
		}

		fout.close();

		if (!zero) WriteAboutZero(N, GF, CIRCLE, baseMethod, j);//написать сообщение об ошибке
	}

}

namespace ForQuality
{
	void Draw_CIRCLE(double radius, int R, int G, int B)//нарисовать окружность
	{
		SetColor(R, G, B); // задаем цвет линии 
		double d = radius - MIN_RADIUS;
		Curve c3(0, 3 * radius, TestFuncAndCurve::u2h, TestFuncAndCurve::v2h);
		switch (CIRCLE) {
		case 1:
			SetPoint(radius, 0); // устанавливаем курсор на точку 
			for (double i = EPS; i <= 2 * pi; i += EPS) Line2(radius*cos(i), radius*sin(i));//рисуем CIRCLE	
			break;
		case 2://треугольник
			SetPoint(-0.5*d, -0.5*d / sqrt(3));
			Line2(-0.5*d + radius, -0.5*d / sqrt(3));
			Line2(-0.5*d + 0.5*radius, -0.5*d / sqrt(3) + 0.5*radius*sqrt(3));
			Line2(-0.5*d, -0.5*d / sqrt(3));
			//Line2(c3.Transfer(radius).x, c3.Transfer(radius).y);
			//Line2(c3.Transfer(2*radius).x, c3.Transfer(2*radius).y);
			//Line2(c3.Transfer(3*radius).x, c3.Transfer(3*radius).y);

			/*cout<<-0.5*d<<" "<< -0.5*d/sqrt(3)<<endl;
			cout<<-0.5*d + radius<<" "<< -0.5*d/sqrt(3)<<endl;
			cout<<-0.5*d + 0.5*radius<<" "<< -0.5*d/sqrt(3) + 0.5*radius*sqrt(3)<<endl;
			cout<<-0.5*d<<" "<< -0.5*d/sqrt(3)<<endl;*/
			break;
		case 3://квадрат
			SetPoint(-0.5*d + 0, -0.5*d + 0);
			Line2(-0.5*d, -0.5*d + radius);
			Line2(-0.5*d + radius, -0.5*d + radius);
			Line2(-0.5*d + radius, -0.5*d + 0);
			Line2(-0.5*d + 0, -0.5*d + 0);
			break;
		case 4://острие
			SetPoint(-0.5*d + 0, -0.25*d*sqrt(3) + 0);
			Line2(-0.5*d + radius, -0.25*d*sqrt(3) + 0);

			for (double i = EPS; i <= pi / 3; i += EPS) Line2(radius*cos(i) - 0.5*d, radius*sin(i) - 0.25*d*sqrt(3));//рисуем CIRCLE	
			for (double i = 2 * pi / 3; i <= pi; i += EPS) Line2(radius*cos(i) - 0.5*d + radius, radius*sin(i) - 0.25*d*sqrt(3));//рисуем CIRCLE
			break;
		}


	}
	void Draw_mas(BasisPoint* r, int R, int G, int B)//нарисовать массив точек
	{
		SetColor(R, G, B); // задаем цвет линии
		double e = 0.01;
		for (int i = 0; i < N; i++)
		{
			SetPoint(r[i].x - 100 * EPS, r[i].y - e); // устанавливаем курсор на точку 
			Line2(r[i].x + 100 * EPS, r[i].y + e);//рисуем line
			SetPoint(r[i].x - 100 * EPS, r[i].y + e); // устанавливаем курсор на точку 
			Line2(r[i].x + 100 * EPS, r[i].y - e);//рисуем line
		}

	}
}

//прототипы вспомогательных функций
void Illustrating(double(*fi)(BasisPoint), BasisPoint *masPoints, double *x, Curve myCurve, int m);
void Fixity();
void Fixity(Method A, Method B);
void Quality(int s, int d, int g, int cu);
void Desigion(int s);
void Pictures_qua(int m, int minN, int maxN, int cu);
void Pictures_fix(int minN, int maxN, int cu);
void Pictures_ill(int minN, int maxN, int hh);

void FileGrafic()//создание графиков по данным в файле
{
	char buff[150], tmp;//создание вспомогательного символьного массива
	int a, b, c, d;
	ifstream fin("Создание графиков.txt"); //объявление объекта для чтения из файла
	fin >> buff;//чтение ненужной текстовой информации
	fin >> tmp;
	if (tmp == '+')//эта ещё недоработана
	{
		fin >> buff;
		fin >> CIRCLE;
		fin >> buff;
		fin >> GF;
		//ForDesigion::Display();
		fin >> buff;
		fin >> a;
		Desigion(a);//заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
		//Illustrating(fi, masPoints, MySLAU.x, myCurve, 1);// график граничной функции и приближения
		Fixity();//график зависимости погрешности аппроксимации от числа базисных точек
	}
	else for (int i = 1; i <= 6; i++) fin >> buff;

	fin >> buff;//чтение ненужной текстовой информации
	fin >> tmp;
	if (tmp == '+')
	{
		fin >> buff;
		fin >> a;
		fin >> buff;
		fin >> b;
		fin >> buff;
		fin >> c;
		Pictures_fix(a, b, c);
	}
	else for (int i = 1; i <= 6; i++) fin >> buff;

	fin >> buff;//чтение ненужной текстовой информации
	fin >> tmp;
	if (tmp == '+')
	{
		fin >> buff;
		fin >> a;
		fin >> buff;
		fin >> b;
		fin >> buff;
		fin >> c;
		fin >> buff;
		fin >> d;
		Pictures_qua(a, b, c, d);
	}
	else for (int i = 1; i <= 8; i++) fin >> buff;

	fin.close();
}

void Make_TestFuncAndCurve()//пробная тестовая функция
{
	//нарисовать границы и точки
	/*double	a0 = -MAX_RADIUS*1.1,
		b0 = -a0,
		c0 = a0,
		d0 = -a0;
	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х и по у
	ForQuality::Draw_CIRCLE(1, 255, 0, 0);

	CloseWindow("Результат.bmp");*/
	//MAX_RADIUS=MIN_RADIUS; //если сделать так, то почти заработает
	Desigion(7);

	//Curve q(0,1.5*MAX_RADIUS,TestFuncAndCurve::u4h,TestFuncAndCurve::v4h);
	//cout << TestFuncAndCurve::g4(q.Transfer(0.25*MAX_RADIUS)) << " " << TestFuncAndCurve::g4(q.Transfer(0.80*MAX_RADIUS)) << " " << TestFuncAndCurve::g4(q.Transfer(1.3*MAX_RADIUS)) << endl;
	/*BasisPoint a,b,c;
	a.x=-1;a.y=0;
	b.x=1;b.y=0;
	c.x=0;c.y=-1;
	cout << TestFuncAndCurve::g8(a) << " " << TestFuncAndCurve::g8(b) << " " << TestFuncAndCurve::g8(c) << endl;*/
	mkdir("name_dir");
	ofstream fout("name_dir\\new_file.txt");
	fout.close();
	/*cout << TestFuncAndCurve::g8(myCurve.Transfer(0.25*MIN_RADIUS)) << " " << TestFuncAndCurve::g8(myCurve.Transfer(0.80*MIN_RADIUS)) << " " << TestFuncAndCurve::g8(myCurve.Transfer(1.3*MIN_RADIUS)) << endl;
	cout << myCurve.Transfer(0.25*MIN_RADIUS).x << " " << myCurve.Transfer(0.25*MIN_RADIUS).y << endl;
	cout << myCurve.Transfer(0.8*MIN_RADIUS).x << " " << myCurve.Transfer(0.8*MIN_RADIUS).y << endl;
	cout << myCurve.Transfer(1.3*MIN_RADIUS).x << " " << myCurve.Transfer(1.3*MIN_RADIUS).y << endl;*/
	//cout<<atan((0.3-0.25*sqrt(3))/0.15)<<" "<<-pi/3<<endl;
	//cout<<myCurve.a<<" "<<myCurve.b<<endl;
}

//основная функция
int main()
{
	setlocale(LC_ALL, "Russian");

	//FileGrafic();
	//Make_TestFuncAndCurve();

	//Desigion(18);//заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
	//Illustrating(fi, masPoints, MySLAU.x, myCurve, 1);// график граничной функции и приближения
	//Fixity();//график зависимости погрешности аппроксимации от числа базисных точек

	//Quality(10, 8, 0, 0);//график зависимости погрешности от кривой, возле которой берутся базисные точки
	//Pictures_fix(30, 180, 30);//картинки зависимости погрешности аппроксимации для от 30 до 40 функций, шаг 30
	//Pictures_qua(40, 25, 105, 50);//картинки зависимости погрешности от кривой для 20 функций с кривыми от 40 до 100 и шагом 20
	Pictures_ill(4,10,3);//графики приближения для 4-10 функций с шагом 3

	return 0;
}

void Desigion(int s)
{
	ForDesigion::Building(s);//чтение и работа с данными
	ForDesigion::Search();//поиск решения и вывод погрешности
}
void Desigion(int s, int g, int cu) {
	ForDesigion::Building(s, g, cu);//чтение и работа с данными
	ForDesigion::Search();//поиск решения и вывод погрешности
}

//иллюстрирование
void Illustrating(double(*fi)(BasisPoint), BasisPoint *masPoints, double *x, Curve myCurve, int m)
{
	//fi=fi0;
	/*const double EPS = 0.0001;*/
	double max = fi(myCurve.Transfer(myCurve.a)), min = max;//поиск минимального и максимального значения граничной функции

	for (double i = myCurve.a + EPS; i <= myCurve.b; i += EPS)
	{
		double tmp = fi(myCurve.Transfer(i));
		if (tmp < min) min = tmp;
		if (tmp > max) max = tmp;
	}

	//инициализация пределов окна
	double	a0 = -myCurve.a - 1 + 0,
		b0 = 1 + myCurve.b,
		c0 = -1 + min,
		d0 = 1 + max;

	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B)
	// с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х равным 1 и 1 по у

	// рисуем график граничной функции
	SetColor(0, 255, 0); // задаем цвет линии (зелёный)
	SetPoint(myCurve.a, fi(myCurve.Transfer(myCurve.a))); // устанавливаем курсор на точку 
	for (double i = myCurve.a + EPS; i <= myCurve.b - EPS; i += EPS)Line2(i, fi(myCurve.Transfer(i)));

	// графики сумм m потенциальных функций

	double sum = 0;
	SetColor(101, 10, 150); // задаем цвет линии

	for (int i = 1; i <= N; i++) sum += x[i - 1] * masPoints[i - 1].Potentialf(myCurve.Transfer(myCurve.a));

	SetPoint(myCurve.a, sum); // устанавливаем курсор на точку 

	cout << "Начинается рисование графика для области.кривой " << CIRCLE << ". " << GF << endl;

	for (double j = myCurve.a + EPS; j <= myCurve.b; j += EPS)
	{
		sum = 0;
		for (int i = 1; (i <= N); i++) {
			sum += x[i - 1] * masPoints[i - 1].Potentialf(myCurve.Transfer(j));
		}
		Line2(j, sum);
	}

	//
	//	int k = abs(N-0*m);
	//	while (k <= N) {
	//
	//		double sum = 0;
	//		SetColor(20 * k, 10 * k, 15 * k); // задаем цвет линии
	//
	//		for (int i = 1; (i <= k) /*&& (i <= N)*/; i++) {
	//			sum += x[i - 1] * masPoints[i - 1].Potentialf(myCurve.Transfer(myCurve.a));
	//		}
	//		SetPoint(myCurve.a, sum); // устанавливаем курсор на точку 
	//
	//		for (double j = myCurve.a + EPS; j <= myCurve.b; j += EPS) {
	//			sum = 0;
	//			for (int i = 1; (i <= k) /*&& (i <= N)*/; i++) {
	//				sum += x[i - 1] * masPoints[i - 1].Potentialf(myCurve.Transfer(j));
	//			}
	//			Line2(j, sum);
	//		}
	//
	//		k += m;
	//	}


   //запись в файл bmp
	char buf[150];
	string str;
	memset(buf, 0, sizeof(buf));
	string d1 = toString(GF), d2 = toString(CIRCLE), d3 = toString(N);
	str = "График 1 граничной функции (" + d1 + ") и её приближения при числе базисных точек (" + d3 + ") на кривой (" + d2 + ").bmp";

	strncpy(buf, str.c_str(), sizeof(buf) - 1);
	CloseWindow(buf);
	//CloseWindow("График 1 граничной функции и её приближения.bmp");// закрываем окно (создаем bmp-файл)

//fi=TestFuncAndCurve::GFunctions[GF-1];
}

void Fixity() {//график функции (число потенциалов)->(расстояние до граничной функции)

	VALUE_FOR_ULTRA = 10;//?
	double *Errors = new double[N];//массив погрешностей
	ForFixity::Create(Errors);//заполнить массив ошибок
	ForFixity::WriteMassiv(Errors);//вывести погрешности
	if (zero)ForFixity::Show(Errors);//нарисовать ломанную ошибок
}

void Fixity(Method A, Method B) {//график функции (число потенциалов)->(расстояние до граничной функции) для методов A и B
	VALUE_FOR_ULTRA = 10;//?
	double *ErrorsA = new double[N];//массив погрешностей A
	double *ErrorsB = new double[N];//массив погрешностей A
	ForFixity::Create(ErrorsA, ErrorsB, A, B);//заполнить массив ошибок
	/*if (zero)*/ForFixity::Show(ErrorsA, ErrorsB, A, B);//нарисовать ломанную ошибок
}

void Quality(int s, int d, int g, int cu)//график зависимости качества аппроксимации от радиуса при s функциях и d кривых
{

	//tmp=10;//?
	double *Errors = new double[d];//массив погрешностей
	double EPSs = (MAX_RADIUS - MIN_RADIUS) / d;

	//нарисовать границы и точки
	double	a0 = -MAX_RADIUS*1.1,
		b0 = -a0,
		c0 = a0,
		d0 = -a0;
	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х и по у

	ForQuality::Draw_CIRCLE(MIN_RADIUS, 250, 0, 0);//нарисовать краcным изначальную окружность //иногда не рисует

	//заполнение массива
	cout << cu/*CIRCLE*/ << ". " << g/*GF*/ << ". Для графика в зависимости от радиуса:" << endl;
	int i = 0;
	double tmp = MAX_RADIUS;
	bool isred = false;//наличие набора с чистым нулём
	for (MAX_RADIUS = MIN_RADIUS + EPSs; MAX_RADIUS <= tmp; MAX_RADIUS += EPSs)
	{
		VALUE_FOR_ULTRA = 10;
		if ((g != 0) && (cu != 0))Desigion(s, g, cu);
		else Desigion(s);
		Errors[i] = ForFixity::ReturnError();

		mr = 0;
		mg = int(fabs(250 - i - 1)) % 250;
		mb = int(fabs(i)) % 250;
		ForQuality::Draw_CIRCLE(MAX_RADIUS, mr, mg, 0);//нарисовать окружность
		if (Errors[i] == 0) { mr = 250; mg = 0; mb = 0; isred = true; }//если на этих точках точность максимально, нарисовать их красными
		ForQuality::Draw_mas(masPoints, mr, mg, mb);//нарисовать точки массива masPoints

		if (i % 10 == 0) cout << "Расстояние при радиусе области " << MIN_RADIUS << " и радиусе кривой " << MAX_RADIUS << " равно " << Errors[i] << endl;
		i++;
		if (i == d) break;
	}
	cout << "------Массив данных записан" << endl;

	//запись в файл массива и анализа
	char buf[250];
	string str, newstr;
	memset(buf, 0, sizeof(buf));
	string d1 = toString(s), d2 = toString(d), d3 = toString(MIN_RADIUS), d4 = toString(MAX_RADIUS), d5 = toString(GF), d6 = toString(CIRCLE), d7 = toString(baseMethod);
	//str = "Файл 3 погрешностей качества аппроксимации граничной функции (" + d5 + ") на кривой (" + d6 + ") методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").txt";
	str = "Файл 3 погрешностей качества аппроксимации методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").txt";
	newstr = bstr + sl + str;//полный адрес
	strncpy(buf, newstr.c_str(), sizeof(buf) - 1);
	ofstream fout(buf);


	if (isred) {

		//запись в файл bmp графика кривых и точек
		memset(buf, 0, sizeof(buf));
		//str = "График 3.2 кривых и базисных потенциалов для граничной функции (" + d5 + ") на кривой (" + d6 + ") методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").bmp";
		str = "График 3.2 кривых и базисных потенциалов методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").bmp";
		newstr = bstr + sl + str;//полный адрес
		strncpy(buf, newstr.c_str(), sizeof(buf) - 1);
		CloseWindow(buf);
	}

	zero = true;//нулей нет 
	int j;//номер нуля
	double min = Errors[0], max = Errors[0], mins = -1, maxs = -1;//минимальный элемент, максимальный, первый скачок, максимальный скачок
	int mini = 0, maxi = 0, mins1 = -1, maxs1 = -1, mins2 = -1, maxs2 = -1;//соответствующие номера
	for (int i = 0; i < d; i++)
	{
		fout << Errors[i] << " \t --> log10 = " << log10(Errors[i]) << endl;
		if ((Errors[i] <= 0) || (Errors[i] != Errors[i]))//проверка на 0 и на NaN
		{
			zero = false;
			j = i;
		}
		//поиск минимального и максимального элемента
		if (Errors[i] < min) {
			min = Errors[i]; mini = i;
		}
		else if (Errors[i] > max) {
			max = Errors[i]; maxi = i;
		}

	}

	for (int i = 1; i < d; i++)
	{
		if (Errors[i] - Errors[i - 1] > 0)
		{
			mins = Errors[i] - Errors[i - 1];
			mins1 = i - 1;
			mins2 = i;
			maxs = mins;
			maxs1 = mins1;
			maxs2 = mins2;
			for (int j = i + 1; j < d; j++)
			{
				double p = Errors[j] - Errors[j - 1];
				if (p > 0)
				{
					if (p > maxs)
					{
						maxs = p;
						maxs1 = j - 1;
						maxs2 = j;
					}
				}
			}
			goto end1;
		}
	}
end1:
	fout << endl;
	fout << "Анализ массива:" << endl;
	fout << "1)Минимальное значение " << min << " на элементе " << mini << endl;
	fout << "2)Максимальное значение " << max << " на элементе " << maxi << endl;
	if (mins > 0)
	{
		fout << "3)Первый скачок " << mins << " с элементa " << mins1 << " на элемент " << mins2 << endl;
		fout << "4)Максимальный скачок " << maxs << " с элементa " << maxs1 << " на элемент " << maxs2 << endl;
	}
	if (!zero) fout << "Невозможно построить логарифмический график, поскольку на элементе " << j + 1 << " (радиус " << MIN_RADIUS + j*EPSs << ") функция принимает значение " << log10(Errors[j]);
	/*if (!zero)//если есть нули, сделать вывод в файл
	{
		//запись в файл
		char buff[300];
		string strr;
		memset(buff, 0, sizeof(buff));
		strr = "Сообщение об ошибке для графика 3 погрешностей качества аппроксимации граничной функции (" + d5 + ") на кривой (" + d6 + ") методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").txt";
		strncpy(buff, strr.c_str(), sizeof(buff) - 1);
		ofstream fout(buff);//cout<<buff;
		fout << "Невозможно построить логарифмический график, поскольку на элементе " << j << "(радиус "<<j*EPSs<<") функция принимает значение -infinity";
		fout.close();
	}*/
	//else//иначе сделать график
	else //нарисовать график, если нет нулей
	{
		double maxx = log10(Errors[0]), minn = max;
		for (int i = 1; i < d; i++)
		{
			double tmp = log10(Errors[i]);
			if (tmp < minn) minn = tmp;

			if (tmp > maxx) maxx = tmp;
		}

		//минимальное из чисел (для того, чтобы начало координа попало в рисунок)
		//double kmin=0; if(min<kmin) kmin=min;
		double	a0 = 0 - MIN_RADIUS / 100,
			b0 = /*1+ d*/MAX_RADIUS*1.2 /*+ EPSs*/,
			c0 = /*-MIN_RADIUS / 100 +*/ minn,
			d0;
		if (maxx > 0) d0 = /*1 +*/ maxx; else d0 = -a0;
		//cout << a0 << " " << b0 << " " << c0 << " " << d0 << endl;

		SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
		SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
		SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
		xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х и по у

		SetColor(250, 0, 0); // задаем цвет линии (красный)
		SetPoint(MIN_RADIUS + EPSs, log10(Errors[0])); // устанавливаем курсор на точку 
		for (int i = 1; i < d; i++)
			Line2(MIN_RADIUS + (i + 1)*EPSs, log10(Errors[i]));//рисуем ломанную

		//запись в файл bmp
		memset(buf, 0, sizeof(buf));
		//str = "График 3.1 качества аппроксимации граничной функции (" + d5 + ") на кривой (" + d6 + ") методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").bmp";
		str = "График 3.1 качества аппроксимации методом " + d7 + " в зависимости от радиуса,(" + d1 + ") потенциальных функций, (" + d2 + ") кривых между окружностями радиусов (" + d3 + ") и (" + d4 + ").bmp";
		newstr = bstr + sl + str;//полный адрес
		strncpy(buf, newstr.c_str(), sizeof(buf) - 1);
		CloseWindow(buf);
	}
	fout.close();
}

//нарисовать систему картинок для от minN до maxN функций, cu кривых
void Pictures_fix(int minN, int maxN, int cu)
{
	int t = int(MAXCIRCLE*KGF*((maxN - minN) / cu + 1)), ind = 0;

	for (CIRCLE = 1; CIRCLE <= MAXCIRCLE; CIRCLE++)
	{
		for (GF = 1; GF <= KGF; GF++)
		{
			for (int m = minN; m <= maxN; m += cu)
			{
				ind++;
				Desigion(m, GF, CIRCLE);//заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
				Fixity();//график зависимости погрешности аппроксимации от числа базисных точек
				cout << "-------Осталось " << t - ind << endl;
			}
		}
	}
}
//картинки для m функций с кривыми от minN до maxN и шагом cu
void Pictures_qua(int m, int minN, int maxN, int cu)
{
	string str1, str2;
	int tt = int(MAXCIRCLE*KGF*((maxN - minN) / cu + 1)), ind = 0;
	for (CIRCLE = 1; CIRCLE <= MAXCIRCLE; CIRCLE++)
	{
		memset(dir_Curve_name, 0, sizeof(dir_Curve_name));
		string d5 = toString(GF), d6 = toString(CIRCLE), d7 = toString(baseMethod);
		str1 = "Данные для кривой " + d6;
		strncpy(dir_Curve_name, str1.c_str(), sizeof(dir_Curve_name) - 1);
		mkdir(dir_Curve_name);//создать папку для кривой

		for (GF = 1; GF <= KGF; GF++)
		{
			memset(dir_func_name, 0, sizeof(dir_func_name));
			string d5 = toString(GF), d6 = toString(CIRCLE), d7 = toString(baseMethod);
			str2 = "При граничной функции " + d5;
			strncpy(dir_func_name, str2.c_str(), sizeof(dir_func_name) - 1);
			bstr = str1 + sl + str2;
			strncpy(chstr, bstr.c_str(), sizeof(chstr) - 1);
			mkdir(chstr);//создать в ней папку для функции

			for (int t = minN; t <= maxN; t += cu)
			{
				ind++;
				Quality(m, t, GF, CIRCLE);//график зависимости погрешности от кривой, возле которой берутся базисные точки
				cout << "-------Осталось " << tt - ind << endl;
			}
		}
	}
}

void Pictures_ill(int minN, int maxN, int cu)//картинки приближения для minN-maxN функций c шагом cu
{
	int t = int(MAXCIRCLE*KGF*((maxN - minN) / cu + 1)), ind = 0;

		for (CIRCLE = 1; CIRCLE <= MAXCIRCLE; CIRCLE++)
		{
			for (GF = 1; GF <= KGF; GF++)
			{
	for (int m = minN; m <= maxN; m += cu)
	{
		ind++;
		Desigion(m/*, GF, CIRCLE*/);//заполнение массива из файла (0) или при генерировании (>0), решение и вывод решения
		Illustrating(fi, masPoints, MySLAU.x, myCurve, 1);// график граничной функции и приближения
		cout << "-------Осталось " << t - ind << endl;
	}
			}
		}
}
