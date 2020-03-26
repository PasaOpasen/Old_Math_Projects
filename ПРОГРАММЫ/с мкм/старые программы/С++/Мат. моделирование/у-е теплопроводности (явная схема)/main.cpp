//решение уравнения теплопроводности через явную схему, ПАСЬКО Д. А., 20.11.2017
#include <fstream>
#include <iostream>
#include <cmath>
#include "Graph2016.h"
using namespace std;


const double pi = 3.1415;
double t, hh, T, X, *up, *b, eps, a0, b0, c0, d0;
int kk;
double k=10;

//искомая функция
double u(double t, double x)
{
	//return exp(-16 * pi * pi * t)*sin(4 * pi*x) + 1;
	//return (cos(x)+sin(x))*t;
	return t*cos(k*x);
}
//свободный член
double f(double t, double x)
{
	//return 0;
	//return (1+t)*(cos(x)+sin(x));
	return cos(k*x)+t*k*k*cos(k*x);
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
	b = new double[kk + 1];

	double c = t / hh / hh;//коэффициент для вычислений

	for (int j = 0; j <= kk; j++)
	{
		b[j] = u(0, j*hh);//заполнение самого нижнего слоя
	}

	for (double k = t; k <= T; k += t)
	{
		up[0] = u(k, 0);// fout<<a[0]<<" \t";
		for (int j = 1; j <= kk - 1; j++)
		{
			up[j] = b[j] + c*(b[j + 1] - 2 * b[j] + b[j - 1]) + t*f(k, j*hh);//заполнение слоя выше		
		}
		up[kk] = u(k, 1);

		for (int j = 0; j <= kk; j++)
		{
			b[j] = up[j];//нижний слой сделать верхним		
		}
	}


	eps = fabs(up[int(X / hh)] - u(T, X));
}

//поиск значения функции в конкретной точке
double search(double tt, double xx)
{
	T = tt; X = xx;
	kk = 1 / hh;

	up = new double[kk + 1];
	b = new double[kk + 1];

	double c = t / hh / hh;//коэффициент для вычислений

	for (int j = 0; j <= kk; j++)
	{
		b[j] = u(0, j*hh);//заполнение самого нижнего слоя
	}

	for (double k = t; k <= T; k += t)
	{
		up[0] = u(k, 0);// fout<<a[0]<<" \t";
		for (int j = 1; j <= kk - 1; j++)
		{
			up[j] = b[j] + c*(b[j + 1] - 2 * b[j] + b[j - 1]) + t*f(k, j*hh);//заполнение слоя выше		
		}
		up[kk] = u(k, 1);

		for (int j = 0; j <= kk; j++)
		{
			b[j] = up[j];//нижний слой сделать верхним		
		}
	}
	return up[int(X / hh)];
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
void RisL(double tt)
{
	SetColor(30, 5, 150); // задаем цвет линии 
	SetPoint(0,up[0]); // устанавливаем курсор на точку
	for (int i = 1; i <= kk; i ++) Line2(i*hh, up[i]);
	 
	//SetPoint(0,search(tt, 0)); // устанавливаем курсор на точку
	//double r = 0.2;
	//for (double i = r; i <= 1; i += r) Line2(i, search(tt, i));
}

double minf(double z, double x)
{
	if(z<x) return z;
	return x;
}

//иллюстрирование
void illustrating(double tt)
{ 
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
	c0 = -1 +minf(0,min); 
	d0 = 1 + max;

	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х равным 1 и 1 по у

	// рисуем график
	Risf(tt);
	RisL(tt);

	CloseWindow("Решение.bmp");// закрываем окно (создаем bmp-файл)

}


int main() 
{
	read();
	search();
	write(eps);
	
	illustrating(2);
	return 0;
}
