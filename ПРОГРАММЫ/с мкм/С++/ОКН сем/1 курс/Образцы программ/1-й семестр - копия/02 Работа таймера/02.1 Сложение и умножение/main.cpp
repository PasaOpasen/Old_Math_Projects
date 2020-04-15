//				Учебная программа 
//				
//				Работа таймера
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
#include <ctime>
#include <math.h>

using namespace std;

int n;                                      		// размерность матрицы, номер строки
double eps;
//double *a,*b,ai,bi,Sum,SumP;
double a[16000000],b[16000000],ai,bi,Sum,SumP,SumSinP,SumSINP;
int timeSum,timeSumP,timeSumSinP,timeSumSINP;

void Vvod();
void Vyvod();
void Generation(int n);                          	// создание массивов a и b
void Res();
double	SIN(double x);

int main()
{
    	Vvod();
    	Generation(n);
    	Res();
	Vyvod();
}
void Vvod()
{
    	char pust[100];
    	float 	scan;
    	FILE *fp;								// указатель на файл
    	fp=fopen("in.txt","r");				    	// открыть файл для чтения
                                            		// чтение из файла
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);fscanf(fp,"%d",&n);
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);ai=scan;
    	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);bi=scan;
    	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);eps=scan;
//    printf("n =%6d ai =%2d  bi =%2d\n",n,ai,bi);
//
//    getchar();
    	fclose(fp);							// закрыть файл для чтения
}
void Vyvod()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                            	// запись в файл
				                            	
    	fprintf(fp,"            Результат          Время\n");
    	fprintf(fp,"                               вычисления\n\n");
    	fprintf(fp," Sum     %12.2f         %5d\n",Sum,timeSum);
    	fprintf(fp," SumP    %12.2f         %5d\n",SumP,timeSumP);
    	fprintf(fp," SumSinP %21.11f %4d\n",SumSinP,timeSumSinP);
	fprintf(fp," Вычисление sin с точностью eps = %2.1e\n",eps);
    	fprintf(fp," SumSINP %21.11f %4d\n",SumSINP,timeSumSINP);
				                            	
//    	fprintf(fp,"  Сумма элементов массивов a и b равна %12.2f\n",Sum);
//    	fprintf(fp,"  Время вычисления суммы равно %d\n",timeSum);
//    	fprintf(fp,"  Сумма произведений элементов массивов a и b равна %12.2f\n",SumP);
//    	fprintf(fp,"  Время вычисления суммы произведений равно %d\n",timeSumP);

    	fclose(fp);	
}
void Generation(int n)
{
//    	a = new double[n];
//   	b = new double[n];
    	for (int i=0;i<n;i++)
    	{
        	a[i]=ai;
        	b[i]=bi;
    	}
}
//
//----------
//
void Res()
{
    	clock_t	timeBegin,timeEnd;
    	timeBegin = clock();					// время начала выполнения
    	Sum=0;
    	for (int i=0;i<n;i++)
    		Sum+=(a[i]+b[i]);
    	timeEnd = clock();						// время окончания выполнения
    	timeSum=timeEnd-timeBegin;
    	timeBegin = clock();
    	SumP=0;
    	for (int i=0;i<n;i++)
    		SumP+=(a[i]*b[i]);
    	timeEnd = clock();
    	timeSumP=timeEnd-timeBegin;
    	timeBegin = clock();
    	SumSinP=0;
    	for (int i=0;i<n;i++)
    		SumSinP+=sin(a[i]*b[i]);
    	timeEnd = clock();
    	timeSumSinP=timeEnd-timeBegin;
    	timeBegin = clock();
    	SumSINP=0;
    	for (int i=0;i<n;i++)
    		SumSINP+=SIN(a[i]*b[i]);
    	timeEnd = clock();
    	timeSumSINP=timeEnd-timeBegin;
}
double SIN(double x)
{
	double s=0;
	double ai=x;
	
	for (int i=3; fabs(ai)>eps; i+=2)
	{
		s+=ai;
		ai*=-x*x/i/(i-1);
	}
	return(s);
}
