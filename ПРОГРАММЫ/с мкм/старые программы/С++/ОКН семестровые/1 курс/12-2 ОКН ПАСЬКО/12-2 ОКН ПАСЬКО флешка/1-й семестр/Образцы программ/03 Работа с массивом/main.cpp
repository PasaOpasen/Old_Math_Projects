//				Учебная программа 
//				
//				Пример работы с массивом
//				(запись в файл в обратном порядке)
//				27 сентября 2015 г.				
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
void Res();


int main()
{
    	Vvod();
    	Res();
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
void Res()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                            	// запись в файл
    	fprintf(fp,"  Запись массива в файл в обратном порядке\n\n");
    	fprintf(fp,"  Исходный массив\n\n");
    	for (int i=0; i<n; i++) 
		fprintf(fp,"%3d",a[i]);
    	fprintf(fp,"\n\n  Массив в обратном порядке\n\n");
    	for (int i=0; i<n; i++)
    		fprintf(fp,"%3d",a[n-1-i]);
    	fclose(fp);	
}
