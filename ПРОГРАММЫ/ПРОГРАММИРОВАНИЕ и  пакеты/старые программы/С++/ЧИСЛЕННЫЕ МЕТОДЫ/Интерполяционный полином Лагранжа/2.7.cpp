#include <cstdlib>
#include <iostream>
#include <stdio.h>
//#include "ReadImage.h"
#include <math.h>
#include "Graph.h"
using namespace std;

double	a, b, c, d, eps, Nx, Ny;
double *p;
int	nRis, n;
char pust[100];
float  scan;
FILE *fp = fopen("in.txt", "r");
FILE *pf = fopen("out.txt", "w");

void Input();
void PolynomGrafic();
double f(double x);
void Ris();
double LagPolynom(double x);
void PolynomGrafic(double* xx, double* yy);
double LagPolynom(double* xx, double* yy, double x);

int main()
{
	Input();

	Ris();
	
	/*double *yy = new double[n+1];
	double *xx = new double[n+1];
	xx[0]=1; yy[0]=2;
	xx[1]=3; yy[1]=0;
	xx[2]=5; yy[2]=0;*/
	
	PolynomGrafic();

	fclose(fp);
	fclose(pf);
}


void Input()
{
	fscanf(fp, "%s", pust);
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%f", &scan); a = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%f", &scan); b = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%f", &scan); c = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%f", &scan); d = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%f", &scan); Nx = scan;
	fscanf(fp, "%s", pust);
	fscanf(fp, "%f", &scan); Ny = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%d", &nRis);
	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%d", &n); //degree of polynom
//n--;


	p = new double[n + 1];
}

void Ris()
{
	SetColor(250, 250, 250);						// задаем фоновый цвет окна
	SetWindow(a, b, c, d);							// создаём окно (создаем массивы R,G,B) с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);							// задаем цвет координатных осей
	xyLine(a, 0, Nx, Ny);							// строим оси, пересекающиеся в т. (a,0)
	// с шагом делений по х равным Nx и Ny -- по у
	// рисуем 1-й график (of function)

	SetColor(0, 0, 255);    			            // задаем цвет линии
	double step = (b - a) / nRis;
	double x = a;
	SetPoint(x, f(x));							// устанавливаем курсор
	for (int i = 0; i < nRis; i++)
	{
		x += step;
		Line2(x, f(x));							// строим отрезок, 
		// соединяющий курсор с точкой (х,у)											// и перемещаем курсор в эту точку
	}

	CloseWindow();								// закрываем окно (создаем bmp-файл)
}

void PolynomGrafic()
{
	double h = (b - a) / n;
	for (int i = 0; i <= n; i++) p[i] = a + i*h;

	SetColor(0, 255, 0);
	double step = (b - a) / nRis;
	double x = a;
	SetPoint(x, LagPolynom(x));
	for (int i = 0; i < nRis; i++)
	{
		x += step;
		Line2(x, LagPolynom(x));
	}

	CloseWindow();
}


void PolynomGrafic(double* xx, double* yy)
{
	//double h = (b - a) / n;
	//for (int i = 0; i <= n; i++) p[i] = a + i*h;

	SetColor(0, 255, 0);
	double step = (b - a) / nRis;
	double x = a;
	SetPoint(x, LagPolynom(xx, yy, x));
	for (int i = 0; i < nRis; i++)
	{
		x += step;
		Line2(x, LagPolynom(xx,yy,x));
	}

	CloseWindow();
}

double f(double x)
{
	return (sin(8 * x) + cos(x));

}

double LagPolynom(double x)
{
	double value = 0;
	double B;
	for (int i = 0; i <= n; i++)
	{
		B = 1;
		for (int j = 0; j <= n; j++)
		{
		z1:
			if (j == i)
			{
				j++;
				goto z1;
			}
			else B *= (x - p[j]) / (p[i] - p[j]);
		}
		value += f(p[i])*B; //fprintf(pf, "%f \n", B);
	}

	return value;
}

double LagPolynom(double* xx, double* yy, double x)
{
	double value = 0;
	double B;
	for (int i = 0; i <= n; i++)
	{
		B = 1;
		for (int j = 0; j <= n; j++)
		{
		z1:
			if (j == i)
			{
				j++;
				goto z1;
			}
			else B *= (x - xx[j]) / (xx[i] - xx[j]);
		}
		value += yy[i] * B; //fprintf(pf, "%f \n", B);
	}

	return value;
}
