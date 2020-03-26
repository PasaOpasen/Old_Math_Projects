//программа Д. ПАСЬКО решения уравнения диффузии методом прогонки
#include <fstream>
#include <iostream>
#include <cmath>
#include "Graph.h"


using namespace std;

double **aa, *a, *c, *b, *f, *y, eps, *alp, *bet, T, hh, k = 1, a0, b0, c0, d0;
int n;

double u(double x)
{
	return cos(k*x);
}

double D2u(double x)
{
	return -k*k*cos(k*x);
}

void read()
{
	ifstream fin("input2.txt");

	char v[50];
	fin >> v; fin >> T; fin >> v; fin >> hh;//считать с файла конец отрезка и шаг

	n = int(T / hh-1);
	/*aa = new double *[n + 1];
	for (int i = 0; i <= n; i++)
		aa[i] = new double[n + 1];*/

	y = new double[n + 1];
	f = new double[n + 1];
	a = new double[n + 1];
	b = new double[n + 1];
	c = new double[n + 1];
	alp = new double[n + 1];
	bet = new double[n + 1];

/*	for (int i = 1; i <= n; i++)
		for (int j = 1; j <= n; j++)
		{
			aa[i][j] = 0;
			if (abs(i - j) <= 1) aa[i][j] = i % 2 + j % 2; //генерация невырожденной трёхдиагональной матрицы
		}*/
    f[1]=u(0);
    f[n]=u(T);
	for (int i = 2; i <n; i++) f[i] = hh*hh*D2u(0 + (i-1)*hh);//заполнение свободного вектора значениями производной

	//b[1] = aa[1][1];
	//c[1] = aa[1][2];
	//for (int i = 2; i < n; i++)
	//{
	//	b[i] = aa[i][i];
	//	c[i] = aa[i][i + 1];
	//	a[i] = aa[i][i - 1];
	//}
	//b[n] = aa[n][n];
	//a[n] = aa[n][n - 1];

	b[1] = 1;
	c[1] = 0;
	for (int i = 2; i < n; i++)
	{
		b[i] = -2;
		c[i] = 1;
		a[i] = 1;
	}
	b[n] = 1;
	a[n] = 0;

	fin.close();
}

void search()//нахождение решения методом прогонки
{   //прямой ход
	alp[2] = -c[1] / b[1];
	bet[2] = f[1] / b[1];
	for (int i = 2; i < n; i++)
	{
		double val = b[i] + a[i] * alp[i];
		alp[i + 1] = -c[i] / val;
		bet[i + 1] = (-a[i] * bet[i] + f[i]) / val;
	}

	//обратный ход
	y[n] = (-a[n] * bet[n] + f[n]) / (b[n] + a[n] * alp[n]);//cout<<y[n]<<endl;
	for (int i = n - 1; i >= 1; i--)
	{
		y[i] = alp[i + 1] * y[i + 1] + bet[i + 1];
	}
}

double maximum()//нахождение разницы между решениями
{
	double m = abs(y[1] - u(0));
	//cout << m << endl;

	for (int i = 2; i <= n; i++) {
		//cout << abs(y[i] - u((i-1)*hh)) << endl;
		if (abs(y[i] - u((i-1)*hh)) > m)
			m = abs(y[i] - u((i-1)*hh));
	}

	return m;
}

//вывод
void write()
{
	ofstream fout("output2.txt");

	fout << "вектор решения:" << endl;
	for (int i = 1; i <= n; i++) fout << y[i] << endl; fout << " " << endl;

	fout << "погрешность= " << maximum();

	fout.close();
}

void Risf()
{
	SetColor(0, 255, 0); // задаем цвет линии (зелёный)
	SetPoint(0, u(0)); // устанавливаем курсор на точку 

	const double eps = 0.0001;
	for (double i = eps; i <= int(T / hh-1)*hh; i += eps) Line2(i, u(i));

}
void RisL()
{
	SetColor(30, 5, 150); // задаем цвет линии 
	SetPoint(0, y[1]); // устанавливаем курсор на точку
	for (int i = 2; i <= n; i++) Line2(i*hh, (y[i]));

}

//иллюстрирование
void illustrating()
{
	double max = y[1], min = y[1];
	
	//поиск пределов окна по y
	for (int i = 2; i <= n ; i++)
	{
		if (y[i] < min) min = y[i];
		if (y[i] > max) max = y[i];
	}
	
	//инициализация пределов окна
	a0 = -1 + 0;
	b0 = 1 + T;
	c0 = -1 + min;
	d0 = 1 + max;

	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(0, 0, 1, 1);	// строим оси, пересекающиеся в т. (0,0), с шагом делений по х равным 1 и 1 по у

	// рисуем график
	Risf();
	RisL();

	CloseWindow();// закрываем окно (создаем bmp-файл)

}

int main()
{
	read();
	search();
	write();
	illustrating();
	return 0;
}
