//              Учебная програма
//              для определения размеров данных разных форматов
//
//              13 сентября 2014 г.

#include <cstdlib>
#include <iostream>
#include <math.h>

using namespace std;

int	n;

void Vvod();

int main()
{
    	FILE *fp;
    	fp=fopen("out.txt","w");		 			// открыть файл для записи
				                         	// запись в файл
    	fprintf(fp,"  ВЫЧИСЛЕНИЕ ФАКТОРИАЛА\n\n");
    	fprintf(fp,"  Формат int\n\n");
    	Vvod();
    	int fakt=1;
    	for(int i=1; i<=n; i++)
    	{
          fakt*=i;
          fprintf(fp,"  i = %3d   %2d! = %12d\n",i,i,fakt);
    	}

    	fprintf(fp,"\n\n  Формат long int\n\n");
    	long int longfakt=1;
    	for(int i=1; i<=n; i++)
    	{
          longfakt*=i;
          fprintf(fp,"  i = %3d   %2d! = %12d\n",i,i,longfakt);
    	}
    
    	fprintf(fp,"\n\n  Формат float\n\n");
    	float floatfakt=1;
    	for(int i=1; i<=n; i++)
    	{
          floatfakt*=i;
          fprintf(fp,"  i = %3d   %2d! = %22.f\n",i,i,floatfakt);
    	}   
      
    	fprintf(fp,"\n\n  Формат double\n\n");
    	double doblefakt=1;
    	for(int i=1; i<=n; i++)
    	{
          doblefakt*=i;
          fprintf(fp,"  i = %3d   %2d! = %22.f\n",i,i,doblefakt);
    	}      
    	fclose(fp);  
}
void Vvod()
{
    	char		pust[100];
    	float		scan;
    	FILE *fp;								// указатель на файл
    	fp=fopen("in.txt","r");			     	// открыть файл для чтения
       									// чтение из файла
    	fscanf(fp,"%s",pust);
    	fscanf(fp,"%s",pust);fscanf(fp,"%d",&n);
    	fclose(fp);							// закрыть файл для чтения
}
