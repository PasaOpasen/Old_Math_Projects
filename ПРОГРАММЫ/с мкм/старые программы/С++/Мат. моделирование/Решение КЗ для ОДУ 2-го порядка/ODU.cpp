//				Решение КЗ для ОДУ
//				сеточным методом
//				(с использованием метода 3-х диагональной прогонки)
//				
//				23.10.2017 г.
//---------------------------------------------------------------------------------------

//#include "stdafx.h"
//#include <stdio.h>
//#include <math.h>
//#include <conio.h>

#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include "Graph2016.h"
#include <math.h>
//
//using namespace std;


											// ОБЩИЕ КОНСТАНТЫ

double		a,b;							// границы отрезка
double		c,d,							// нижняя и верхняя границы рисунка
			Nx,Ny;							// шаг делений коорд. линий по х и у
int			n,k;							// *количество узлов сетки, параметр точного решения
const double pi=3.14159265360;
double		*f,*y;

											// ОБЪЯВЛЕНИЕ ФУНКЦИЙ

double F(double x);							// правая часть
double u(double x);							// точное решение
double sqr(double x);						//
void Vvod();								// чтение данных
void Vyvod(double x,double y);				// вывод результата
void Progon_3();							// метод 3-ти диагональной прогонки
double Nevas();
double PogRes();
void Ris();							    	// рисование графика
//
//-------------------------------------------------------------------
//
int main()
{
	Vvod();
	Progon_3();
	Vyvod(Nevas(),PogRes());
	Ris();
}
//-------------------------------------------------------------------
#define sqr(x) (x)*(x)
//---------- Правая часть
double F(double x){return(-pi*pi*k*k*cos(pi*k*x));}
//---------- Точное решение
double u(double x){return(cos(pi*k*x));}
//---------- Чтение данных
void Vvod()
{
	char		pust[100];
	FILE *fp;
	if ((fp=fopen("Данные.txt","r")) == 0)
	{
		printf("\n NO FILE");
		getchar();
		//exit(0);
	}
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%lf",&a);
	fscanf(fp,"%s",pust);fscanf(fp,"%lf",&b);
	fscanf(fp,"%s",pust);fscanf(fp,"%lf",&c);
	fscanf(fp,"%s",pust);fscanf(fp,"%lf",&d);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%lf",&Nx);
	fscanf(fp,"%s",pust);fscanf(fp,"%lf",&Ny);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&n);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&k);
	f = new double[n+1];
	y = new double[n+1];
	fclose(fp);
}
//---------- Вывод результата
void Vyvod(double x,double y)
{
	FILE *fp;
	fp=fopen("out.txt","w");
	fprintf(fp,"   РЕШЕНИЕ краевой задачи для ОДУ 2-го порядка:\n\n");
	fprintf(fp,"   Размерность приближённой задачи \n");
	fprintf(fp,"   равна                        %10d\n",n);
	fprintf(fp,"   Невязка решения СЛАУ равна   %10.8f\n",x);
	fprintf(fp,"   Погрешность приближенного решения\n");
	fprintf(fp,"   равна                        %10.8f",y);
	fclose(fp);
}
//---------- Метод 3-х диагональной прогонки
void Progon_3()
{
	double *alpha,*betta;
	alpha = new double[n];
	betta = new double[n];
	double 
		u0=u(0),
		u1=u(1);
	double	
		b0,c0,								// 0-я строка
		ai,bi,ci,							// i-я строка, i=1,n-1
		an,bn;								// n-я строка
	//---------- заполняем под 1-е краевые условия    
	b0=1;c0=0;
	ai=1;bi=-2;ci=1;
	an=0;bn=1;
	{
		double h=(b-a)/n;
		double x=h;
		double h2=h*h;
		for(int i=1; i<n; i++)
		{
			f[i]=h2*F(x);
			x+=h;
		}
		f[0]=u0;
		f[n]=u1;
	}
	//---------- Решение СЛАУ
	alpha[n]=-an/bn;
	betta[n]=f[n]/bn;
	for (int i=n-1; i>=1; i--)
	{
		double s=alpha[i+1]*ci+bi;
		alpha[i]=-ai/s;
		betta[i]=(f[i]-betta[i+1]*ci)/s;
	}
	y[0]=(f[0]-c0*betta[1])/(b0+alpha[1]*c0);
	for (int i=1; i<=n; i++) y[i]=alpha[i]*y[i-1]+betta[i];
}
//---------- Вычисление невязки решения СЛАУ
double Nevas()
{
#define sqr(x) (x)*(x)
	double	
		b0,c0,								// 0-я строка
		ai,bi,ci,							// i-я строка, i=1,n-1
		an,bn;								// n-я строка
	//---------- заполняем под 1-е краевые условия    
	b0=1;c0=0;
	ai=1;bi=-2;ci=1;
	an=0;bn=1;
	double s=0;
	for (int i=1; i<n; i++) 
		s+=sqr(ai*y[i-1]+bi*y[i]+ci*y[i+1]-f[i]);
	s+=sqr(b0*y[0]+c0*y[1]-f[0])+sqr(an*y[n-1]+bn*y[n]-f[n]);
	return s;
}
//---------- Вычисление погрешности приближённого решения
double PogRes()
{
	double s,max=0;
	double h=(b-a)/n;
	double x=a;
	for (int i=0; i<=n; i++)
	{
		s=fabs(y[i]-u(x)); 
		if (max<s) max=s;
		x+=h;
	}
	return max;
}
//---------- Рисование графиков
void Ris()
{
	const int	M=300;						// количество точек рисования
	double h_p=(b-a)/M;						// шаг рисования
	double h=(b-a)/n;						// шаг сетки
	//---------- Создание окна
	SetColor(250,250,255);
	SetWindow(a,b,c,d);
	SetColor(0,0,0);
	xyLine(0,0,Nx,Ny);

	//---------- график точного решения
	double x=a;
	double Y=u(0);
	SetColor(0,180,120);
	SetPoint(x,Y);
	for (int i=1; i<=M; i++)
	{
		x+=h_p;
		Y=u(x);
		Line3(x,Y);
	}
	//---------- график приближенного решения по явной РС
	x=a;
	Y=y[0];
	SetColor(200,0,50);
	SetPoint(x,Y);
	for (int i=1; i<=n; i++)
	{
		x+=h;
		Y=y[i];
		Line(x,Y);
	}
	CloseWindow("График.bmp");
}
