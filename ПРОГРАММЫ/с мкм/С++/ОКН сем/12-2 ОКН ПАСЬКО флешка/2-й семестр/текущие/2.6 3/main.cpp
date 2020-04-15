//				–исование графиков с использованием библиотеки Graph
//				
//				21 марта 2015 г.
//---------------------------------------------------------------------------------------

//#include "stdafx.h"
//#include <stdio.h>
//#include <math.h>
//#include <conio.h>
//#include <ctime>

#include <cstdlib>
#include <iostream>
//#include <ctime>
#include <math.h>
#include "Graph.h"


												// ќЅў»≈  ќЌ—“јЌ“џ

double a,b,c,d,									// границы рисунка
Nx,Ny;											// шаги делений по х и по у
double pi=3.141592;
int	nRis;										// количество шагов по x
int rC[2],gC[2],bC[2];
												// ќЅЏя¬Ћ≈Ќ»≈ ‘”Ќ ÷»…
double f1(double x);
double f2(double x);
void Vvod();
void Ris();
//------------------------------------------
int main()
{
	Vvod();
	Ris();
}
//------------------------------------------
void Ris()
{
	SetColor(250,250,250);						// задаем фоновый цвет окна
	SetWindow(a,b,c,d);							// создаЄм окно (создаем массивы R,G,B)
												// с пределами [a,b]x[c,d]
	SetColor(0,0,0);							// задаем цвет координатных осей
	xyLine(a,0,Nx,Ny);							// строим оси, пересекающиес€ в т. (a,0)
												// с шагом делений по х равным Nx
												// и Ny -- по у
												// рисуем 1-й график
	SetColor(rC[0],gC[0],bC[0]);    			// задаем цвет линии
	double step=(b-a)/nRis;
	double x=a;
	SetPoint(x,f1(x));							// устанавливаем курсор
	for (int i=0; i<nRis; i++)
	{ 
		x+=step;
		Line2(x,f1(x));							// строим отрезок, 
												// соедин€ющий курсор с точкой (х,у)
												// и перемещаем курсор в эту точку
	}
												// рисуем 2-й график
	SetColor(rC[1],gC[1],bC[1]);    			// задаем цвет линии
	x=a;
	SetPoint(x,f2(x));							// устанавливаем курсор
	for (int i=0; i<nRis; i++)
	{ 
		x+=step;
		Line(x,f2(x));							// строим отрезок, 
												// соедин€ющий курсор с точкой (х,у)
												// и перемещаем курсор в эту точку
	}
	CloseWindow();								// закрываем окно (создаем bmp-файл)
}
//------------------------------------------
double f1(double x)
{
	return (sin(3*pi*x));
}
double f2(double x)
{
	return (sin(pi*x));
}
//------------------------------------------
void Vvod()
{
	char		pust[100];
	float		scan;							// вспомогательна€ переменна€ дл€ чтени€ из файла
	FILE *fp;									// указатель на файл
	fp=fopen("in.txt","r");						// открыть файл дл€ чтени€
												// чтение из файла
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);a=scan;
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);b=scan;
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);c=scan;
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);d=scan;
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);Nx=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);Ny=scan;
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&nRis);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%d",&rC[0]);fscanf(fp,"%d",&gC[0]);fscanf(fp,"%d",&bC[0]);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%d",&rC[1]);fscanf(fp,"%d",&gC[1]);fscanf(fp,"%d",&bC[1]);
	//    printf("nRis =%6d rC[0] =%3d  gC[1] =%3d\n",nRis,rC[0],gC[1]);
	//    printf("b =%6f c =%6f\n",b,c);
	//    getchar();


	fclose(fp);
}
