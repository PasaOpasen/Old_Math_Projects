//              Учебная програма
//              для определения размеров данных разных форматов
//
//              13 сентября 2014 г.

#include <cstdlib>
#include <iostream>
#include <math.h>

using namespace std;

int a,n;

void Vvod();

int main()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                         	// запись в файл
    	Vvod();
    	fprintf(fp,"  ВЫЧИСЛЕНИЕ %d^n\n\n",a);
    	fprintf(fp,"  Формат int \n\n");
    	int P2n=1;
    	for(int i=1; i<=n; i++)
    	{
          P2n*=a;
          fprintf(fp,"  i = %3d   %d^%-2d = %12d\n",i,a,i,P2n);
    	}

    	fprintf(fp,"\n\n  Формат long int\n\n");
    	long int longP2n=1;
    	for(int i=1; i<=n; i++)
    	{
          longP2n*=a;
          fprintf(fp,"  i = %3d   %d^%-2d = %12d\n",i,a,i,longP2n);
    	}
    
    	fprintf(fp,"\n\n  Формат float\n\n");
    	float floatP2n=1;
    	for(int i=1; i<=n; i++)
    	{
          floatP2n*=a;
          fprintf(fp,"  i = %3d   %d^%-2d = %22.f\n",i,a,i,floatP2n);
    	}   
      
    	fprintf(fp,"\n\n  Формат double\n\n");
    	double dobleP2n=1;
    	for(int i=1; i<=n; i++)
    	{
          dobleP2n*=a;
          fprintf(fp,"  i = %3d   %d^%-2d = %22.f\n",i,a,i,dobleP2n);
    	}      
    	fclose(fp);  
}
void Vvod()
{
     char		pust[100];
     float	scan;
    	FILE *fp;								// указатель на файл
    	fp=fopen("in.txt","r");			     	// открыть файл для чтения
       									// чтение из файла
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);fscanf(fp,"%d",&a);
    	fscanf(fp,"%s",pust);fscanf(fp,"%d",&n);
    	fclose(fp);							// закрыть файл для чтения
}
