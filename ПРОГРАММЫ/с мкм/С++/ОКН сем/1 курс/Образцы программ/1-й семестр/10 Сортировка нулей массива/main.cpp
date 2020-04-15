//				Программа сортировки массива
//				(нулевые элементы -- вправо; 
//				порядок ненулевых элементов сохраняется)
//				23 октября 2015 г.				
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

int n,k,param1,param2,param3,time0;						// 
int *a;
//int n=20;
//int a[20]={1,0,0,4,5,7,0,0,0,0,10,11,12,0,0,15,16,0,18,19};

#define	swap(x,y)	{int s=x;x=y;y=s;}

void Vvod();
void Gena();								// генерация массива
void Sort_0();								//
void Sort_1();	
void Vyvod_0();
void Vyvod_1();

int main()
{
    	Vvod();
    	Gena();
    	Sort_0();
    	Vyvod_0();
    	Gena();
   	Sort_1();
	Vyvod_1();
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
    	fscanf(fp,"%d",&param1);
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%d",&k);
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%d",&param2);
    	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    	fscanf(fp,"%d",&param3);
//	printf("%d  %d %d",param1,param2,param3);
//    	getchar();
    	fclose(fp);							// закрыть файл для чтения
}
void Gena()
{
	a = new int[n];
	if (param1==0)
	{
		for (int i=0;i<n;i++) a[i]=0;
		for (int i=0;i*k<n;i++) a[i*k+1]=i+1;
	}
	else
	{
		for (int i=0;i<n;i++) a[i]=i;
		for (int i=0;i*k<n;i++) a[i*k+1]=0;
		a[5]=a[6]=a[7]=0;
	}
}
void Sort_0()
{
	clock_t	timeBegin,timeEnd;
    	timeBegin = clock();					// время начала выполнения
	int m=0;
	for (int i=0; i<n;i++)
	{
		if (a[i]!=0)
		{
			swap(a[i],a[m]);
			m++;
		}
	}
    	timeEnd = clock();
    	time0=timeEnd-timeBegin;
}
void Sort_1()
{
	clock_t	timeBegin,timeEnd;
    	timeBegin = clock();					// время начала выполнения
	int m=0;
	for (m=n-1;m>=0;m--)
	if (a[m]!=0) break;						// m указывает на ненулеой элемент
	if (param2==0)
	for (int i=m-1; i>=0;i--)				// вариант без сохранения порядка
	{
		if (a[i]==0)
		{
			swap(a[i],a[m]);
			m--;
		}
	}
	else
	for (int i=m-1; i>=0;i--)				// вариант с сохранением порядка
	{
		if (a[i]==0)
		{
			for (int j=i;j<m;j++) 
			a[j]=a[j+1];
			a[m]=0;
			m--;
		}
	}
    	timeEnd = clock();
    	time0=timeEnd-timeBegin;
}
void Vyvod_0()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                            	// запись в файл
    	fprintf(fp,"  Сортировка массива, нулевые элементы -- вправо\n\n");
    	fprintf(fp,"  Размерность массива = %d\n",n);
	if (param1==0)
	fprintf(fp,"  Массив содержит много нулей\n\n");
	else
	fprintf(fp,"  Массив содержит мало нулей\n\n");
	fprintf(fp,"  Первая сортировка (проход слева направо),\n");
	fprintf(fp,"  порядок ненулевых элементов сохраняется\n\n");
	if (param3==0) ;
	else
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
void Vyvod_1()
{
    	FILE *fp;
    	fp=fopen("out.txt","a");		 			// открыть файл для записи
				                            	// запись в файл
    	fprintf(fp,"\n\n  Вторая сортировка (проход справа налево),\n");
	if (param2==0)
	fprintf(fp,"  порядок ненулевых элементов не сохраняется\n\n");
	else
	fprintf(fp,"  порядок ненулевых элементов сохраняется\n\n");
	if (param3==0) ;
	else
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
