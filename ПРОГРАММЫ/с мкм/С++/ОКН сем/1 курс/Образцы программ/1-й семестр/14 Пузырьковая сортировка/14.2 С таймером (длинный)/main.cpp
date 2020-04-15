//				Учебная программа 
//				
//				Сортировка перестановкой ("пузырьковая")
//				(с таймером)
//				05 ноября 2015 г.				
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

using namespace std;

int n,k,param,time0;						// 
int *a;

#define	swap(x,y)	{int s=x;x=y;y=s;}

void Vvod();
void Gena();								// генерация массива
void sortPusyr1();								//
void sortPusyr2();	
void Vyvod0();
void Vyvod1();
void Vyvod2();

int main()
{
    	Vvod();
    	Gena();
    	Vyvod0();
    	sortPusyr1();
    	Vyvod1();
    	Gena();
   	sortPusyr2();
	Vyvod2();
}
void Vvod()
{
    	char  pust[100];
    	FILE *fp;								// указатель на файл
    	fp=fopen("in.txt","r");				    	// открыть файл для чтения
                                            		// чтение из файла
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);
    	fscanf(fp,"%d",&n);
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    	fscanf(fp,"%d",&param);
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%d",&k);
//	printf("%d  %d %d",param1,param2,param3);
//    	getchar();
    	fclose(fp);							// закрыть файл для чтения
}
void Gena()
{
	a = new int[n];
	if (param==0)
	{
		for (int i=0;i<n;i++) a[i]=0;
		for (int i=0;i*k<n;i++) a[i*k+1]=i+1;
	}
	else
	{
		for (int i=0;i<n;i+=2) {a[i]=-i;a[i+1]=i+1;}
	}
}
void sortPusyr1()
{
	#define swap(x,y) {int s=x;x=y;y=s;}
	clock_t	timeBegin,timeEnd;
    	timeBegin = clock();					// время начала выполнения
	for (int i=n-1;i>0;i--)
	for (int j=i-1; (j<n-1)&&(a[j]<a[j+1]);j++)
		swap(a[j],a[j+1]);
    	timeEnd = clock();
    	time0=timeEnd-timeBegin;
}
void sortPusyr2()
{
	#define swap(x,y) {int s=x;x=y;y=s;}
	clock_t	timeBegin,timeEnd;
    	timeBegin = clock();					// время начала выполнения
	for (int i=n-1;i>0;i--)
	for (int j=0; j<i;j++)
		if (a[j]<a[j+1]) swap(a[j],a[j+1]);
    	timeEnd = clock();
    	time0=timeEnd-timeBegin;
}
void Vyvod0()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                            	// запись в файл
    	fprintf(fp,"  Отсортировать массив в порядке убывания\n");
    	fprintf(fp,"  пузырьковым методом \n\n");
    	fprintf(fp,"  Размерность массива = %d\n",n);
	if (param==0)
	fprintf(fp,"  Массив содержит много нулей\n\n");
	else
	fprintf(fp,"  Массив содержит мало нулей\n\n");
	{
    		fprintf(fp,"  Первые 20 элементов исходного массива:\n\n");
    		int j=1;
    		for (int i=0; i < 20; i++,j++)
    		{    
			fprintf(fp,"%10d",a[i]);
			if (j==10)
			{
				j=0;
			fprintf(fp,"\n");
			}
		}
		fprintf(fp,"\n\n  Последние 40 элементов исходного массива:\n\n");
    		j=1;
    		for (int i=0; i < 40; i++,j++)
    		{    
			fprintf(fp,"%10d",a[n-40+i]);
			if (j==10)
			{
				j=0;
				fprintf(fp,"\n");
			}
		}
		fprintf(fp,"\n\n");
	}
    	fclose(fp);	
}
void Vyvod1()
{
    	FILE *fp;
    	fp=fopen("out.txt","a");		 			// открыть файл для записи
				                            	// запись в файл
	fprintf(fp,"  Первая сортировка (экономичная)\n\n");
	{
    		fprintf(fp,"  Первые 20 элементов отсортированного массива:\n\n");
    		int j=1;
    		for (int i=0; i < 20; i++,j++)
    		{    
			fprintf(fp,"%10d",a[i]);
			if (j==10)
			{
				j=0;
			fprintf(fp,"\n");
			}
		}
		fprintf(fp,"\n\n  Последние 40 элементов отсортированного массива:\n\n");
    		j=1;
    		for (int i=0; i < 40; i++,j++)
    		{    
			fprintf(fp,"%10d",a[n-40+i]);
			if (j==10)
			{
				j=0;
				fprintf(fp,"\n");
			}
		}
		fprintf(fp,"\n\n");
	}
    	fprintf(fp,"  Время выполнения = %5d мс",time0);
    	fclose(fp);	
}
void Vyvod2()
{
    	FILE *fp;
    	fp=fopen("out.txt","a");		 			// открыть файл для записи
				                            	// запись в файл
    	fprintf(fp,"\n\n  Вторая сортировка (неэкономичная)\n\n");
	{
    		fprintf(fp,"  Первые 20 элементов отсортированного массива:\n\n");
    		int j=1;
    		for (int i=0; i < 20; i++,j++)
    		{    
			fprintf(fp,"%10d",a[i]);
			if (j==10)
			{
				j=0;
				fprintf(fp,"\n");
			}
		}
		fprintf(fp,"\n\n  Последние 40 элементов отсортированного массива:\n\n");
    		j=1;
    		for (int i=0; i < 40; i++,j++)
    		{    
			fprintf(fp,"%10d",a[n-40+i]);
			if (j==10)
			{
				j=0;
				fprintf(fp,"\n");
			}
		}
		fprintf(fp,"\n\n");
	}
    	fprintf(fp,"  Время выполнения = %5d мс",time0);
    	fclose(fp);	
}

