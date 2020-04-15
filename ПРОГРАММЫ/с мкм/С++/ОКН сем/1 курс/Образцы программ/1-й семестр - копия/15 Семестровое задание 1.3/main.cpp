//				Учебная программа 
//				
//				СЗ 1.3 №13 -- найти количество элементов массива, 
//				не имеющих равных
//				
//				06 ноября 2015 г.				
//---------------------------------------------------------------------------------------

//#include "stdafx.h"
//#include <stdio.h>
//#include <math.h>
//#include <conio.h>
//#include <ctime>

#include <cstdlib>
#include <iostream>
//#include <ctime>
//#include <math.h>

using namespace std;

int n=10;                                   		// размерность массива
int a[10];


void Vvod();
int Res();
void Vyvod();

int main()
{
    	Vvod();
    	Vyvod();
}
void Vvod()
{
    	char  pust[100];
    	int scan;
    	FILE *fp;								// указатель на файл
    	fp=fopen("in.txt","r");				    	// открыть файл для чтения
                                            		// чтение из файла
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);
    	for (int i=0; i<n; i++)
	    {
			fscanf(fp,"%d",&scan); 
			a[i]=scan;
		}
    	fclose(fp);							// закрыть файл для чтения
}
void Vyvod()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                            	// запись в файл
    	fprintf(fp,"  Исходный массив\n\n");
    	for (int i=0; i<n; i++) 
		fprintf(fp,"%3d",a[i]);
    	fprintf(fp,"\n\n  Количество элементов массива,\n");
    	fprintf(fp,"  не имеющих равных, равно %d\n\n", Res());
    	fclose(fp);	
}
int Res()
{
	int s=0;
	for (int i=0;i<n;i++)
	{
		int k=0;
		for (int j=0;j<n;j++)
		if (a[i]==a[j]) k++;
		if (k==1) s++;
	}
	return (s);
}
