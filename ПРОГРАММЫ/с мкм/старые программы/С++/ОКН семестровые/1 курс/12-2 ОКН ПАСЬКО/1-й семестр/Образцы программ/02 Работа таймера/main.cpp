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
//double *a,*b,ai,bi,Sum,SumP;
double a[16000000],b[16000000],ai,bi,Sum,SumP,SumSinP;
int timeSum,timeSumP,timeSumSinP;

void Vvod();
void Vyvod();
void Generation(int n);                          	// создание массивов a и b
void Res();

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
				                            	
    	fprintf(fp,"            Результат      Время\n");
    	fprintf(fp,"                           вычисления\n\n");
    	fprintf(fp," Sum     %12.2f      %4d\n",Sum,timeSum);
    	fprintf(fp," SumP    %12.2f      %4d\n",SumP,timeSumP);
    	fprintf(fp," SumSinP %12.2f      %4d\n",SumSinP,timeSumSinP);
				                            	
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
}
