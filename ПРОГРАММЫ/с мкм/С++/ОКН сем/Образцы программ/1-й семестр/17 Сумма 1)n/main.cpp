//				Учебная программа 
//				
//				Сумма 1/т
//
//				15 декабря 2015 г.				
//---------------------------------------------------------------------------------------

//#include "stdafx.h"
//#include <stdio.h>
//#include <math.h>
//#include <conio.h>
//#include <ctime>

#include <cstdlib>
#include <iostream>
#include <ctime>
//#include <math.h>

//using namespace std;

double sum;								// сумма
double n_min,n_max;							// верхний предел суммы
int timeSum;

void Vvod();
void Vyvod();
void Res();

int main()
{
	Vvod();
	Res();
	Vyvod();
}
void Vvod()
{
	char  pust[100];
	float scan;
	FILE *fp;								// указатель на файл
	fp=fopen("in.txt","r");					// открыть файл для чтения
										// чтение из файла
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%e",&scan);n_min=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%e",&scan);n_max=scan;
										// проверка правильности чтения
//	printf("n_max = %e",n_max);
//	getchar();
	fclose(fp);							// закрыть файл для чтения
}
void Vyvod()
{
	FILE *fp;
	fp=fopen("out.txt","w");		 			// открыть файл для записи
										// запись в файл
	fprintf(fp,"  Вычисление суммы sum_{i=n_min}^n_max (1/i)\n\n");
	fprintf(fp,"  n_min = %e\n",n_min);
	fprintf(fp,"  n_max = %e\n",n_max);
	fprintf(fp,"  Значение суммы   = %18.16e\n",sum);
	//fprintf(fp,"  Значение 1/n_max = %18.16e\n",1./n_max);
    	fprintf(fp,"  Время вычисления = %d мс",timeSum);
	fclose(fp);	
}
//
//----------
//
void Res()
{
	sum=0;
	clock_t	timeBegin,timeEnd;
    	timeBegin = clock();					// время начала выполнения
//	for (long long int i=(long long int)n_min;i<=n_max;i++)
//		sum+=1./i;
	for (double i=n_min;i<=n_max;i+=1)
		sum+=1/i;
	timeEnd = clock();						// время окончания выполнения
    	timeSum=timeEnd-timeBegin;
}
