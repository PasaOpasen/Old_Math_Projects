//решение уравнения теплопроводности через неявную схему, ПАСЬКО Д. А., 03.12.2017
#include <fstream>
#include <iostream>
#include <cmath>
#include "Graph2016.h"
using namespace std;


const double pi = 3.1415;
double t, hh, T, X, *up,*upn, *down,*downn, eps, a0, b0, c0, d0,*y,*f,*a,*b,*c,*alp,*bet;
int kk,n,k=10;

//искомая функция
double u(double t, double x)
{
	//return exp(-16 * pi * pi * t)*sin(4 * pi*x) + 1;
	//return (cos(x)+sin(x))*t;
	return t*cos(k*x);
}
//свободный член
double func(double t, double x)
{
	//return 0;
	//return (1+t)*(cos(x)+sin(x));
	return cos(k*x) + t*k*k*cos(k*x);
}

//чтение из файла
void read()
{
	ifstream fin("input.txt");
	char buff[100];
	fin >> buff; fin >> t;//чтение шага по времени
	fin >> buff; fin >> hh;//шаг по расстоянию
	fin >> buff; fin >> T;//точка по времени
	fin >> buff; fin >> X;//чтение точки по расстоянию

	fin.close();
}

//кусок из решения у-я трёхдиагональной матрицей
void SLAU(double* down,double ti)//нахождение решения методом прогонки
{
	double con=t/hh/hh;
    //заполнение
	for (int i = 1; i <= n; i++) f[i] = down[i-1]+t*func(ti, (i-1)*hh);//заполнение свободного вектора значениями производной

	b[1] = 1+2*con;
	c[1] = 0;
	for (int i = 2; i < n; i++)
	{
		b[i] = 1+2*con;
		c[i] = -con;
		a[i] = c[i];
	}
	b[n] = 1+2*con;
	a[n] = 0;
 
    //прямой ход
	alp[2] = -c[1] / b[1];
	bet[2] = f[1] / b[1];
	for (int i = 2; i < n; i++)
	{
		double val = b[i] + a[i] * alp[i];
		alp[i + 1] = -c[i] / val;
		bet[i + 1] = (-a[i] * bet[i] + f[i]) / val;
	}

	//обратный ход
	y[n] = (-a[n] * bet[n] + f[n]) / (b[n] + a[n] * alp[n]);
	for (int i = n - 1; i >= 1; i--)
	{
		y[i] = alp[i + 1] * y[i + 1] + bet[i + 1];
	}
}

//вывод погрешности
void write(double eps)
{
	ofstream fout("output.txt");

	fout << eps << endl;//вывод погрешности

	fout.close();
}

//поиск решения в точке из файла
void search()
{
	kk = 1 / hh;

	up = new double[kk + 1];
	down = new double[kk + 1];
	
	upn = new double[kk + 1];
	downn = new double[kk + 1];
	
	//для трёхдиагональной матрицы  
	n = int(T / hh-1);
	y = new double[n + 1];
	f = new double[n + 1];
	a = new double[n + 1];
	b = new double[n + 1];
	c = new double[n + 1];
	alp = new double[n + 1];
	bet = new double[n + 1];

	double cc = t / hh / hh;//коэффициент для вычислений

	for (int j = 0; j <= kk; j++)
	{
		down[j] = u(0, j*hh);//заполнение самого нижнего слоя
		downn[j] = u(0, j*hh);//заполнение самого нижнего слоя
	}

	for (double k = t; k <= T; k += t)
	{
		up[0] = u(k, 0);// fout<<a[0]<<" \t";
		upn[0] = u(k, 0);// fout<<a[0]<<" \t";
		for (int j = 1; j <= kk - 1; j++)
		{
			up[j] = down[j] + cc*(down[j + 1] - 2 * down[j] + down[j - 1]/**/) + t*func(k, j*hh);//заполнение слоя выше
			
			SLAU(downn,k);
			upn[j] = y[j + 1];
		}
		up[kk] = u(k, 1);
        upn[kk] = u(k, 1);

		for (int j = 0; j <= kk; j++)
		{
			down[j] = up[j];//нижний слой сделать верхним
			downn[j] = upn[j];		
		}
	}

	eps = fabs(upn[int(X / hh)] - u(T, X));
}

//нарисовать функцию
void Risf(double tt)
{
	SetColor(0, 255, 0); // задаем цвет линии (зелёный)
	SetPoint(0, u(tt, 0)); // устанавливаем курсор на точку 

	const double eps = 0.0001;
	for (double i = eps; i <= 1; i += eps) Line2(i, u(tt, i));

}
//рисовать кривую
void RisL(double tt,double* uu)
{
	//SetColor(30, 5, 150); // задаем цвет линии 
	SetPoint(0, uu[0]); // устанавливаем курсор на точку
	for (int i = 1; i <= kk; i++) Line2(i*hh, uu[i]);
}

double minf(double z, double x)
{
	if (z < x) return z;
	return x;
}

//иллюстрирование
void illustrating()
{
	double tt = T;
	double max = u(tt, 0), min = max;
	eps = 0.001;
	//поиск пределов окна по y
	for (double i = eps; i <= 1; i += eps)
	{
		if (u(tt, i) < min) min = u(tt, i);
		if (u(tt, i) > max) max = u(tt, i);
	}

	//инициализация пределов окна
	a0 = -0.5;
	b0 = 1.5;
	c0 = -1 + minf(0, min);
	d0 = 1 + max;

	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х равным 1 и 1 по у

	// рисуем график
	Risf(tt);
	SetColor(30, 5, 150); // задаем цвет линии 
	RisL(tt,up);
	SetColor(200, 0, 0); // задаем цвет линии 
	RisL(tt,upn);

	CloseWindow("Решение.bmp");// закрываем окно (создаем bmp-файл)

}


int main()
{
	read();
	search();
	write(eps);
	illustrating();
	return 0;
}
