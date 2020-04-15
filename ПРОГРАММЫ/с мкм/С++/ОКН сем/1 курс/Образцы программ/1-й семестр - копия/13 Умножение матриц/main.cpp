//              Учебная программа
//              Умножение матриц разными способами 
//              для проверки зависимости времени выполнения программы
//              от организации обращения к памяти
//
//              13 сентября 2014 г.

#include <cstdlib>
#include <iostream>
#include <ctime>
#include <math.h>

using namespace std;

											// МАКРОСЫ
#define N	(1024*1024)						// Максимальная размерность СЛАУ
											
typedef double matrix[N];
matrix 		A,B,C;

int			n,n_cikle;
double		a,b;
int         timeR,timeR1,timeR2;
clock_t		timeBegin,timeEnd;

void Vvod();
void Vyvod();
void Umn_ij();
void Umn_ji();
void Umn();     

int main()
{
    
    Vvod();    
    //
    //----------	Заполняем матрицы	
    //
    for (int i=0; i<n; i++)
    	for (int j=0; j<n; j++)
    	{
    		int k=i*n+j;
    		A[k]=a;
    		B[k]=b;
    	}
    Umn_ij();
    Umn_ji();
    Umn();	
   	Vyvod();
}
//
//----------	Ввод данных
//
void Vvod()
{
     char		pust[100];
     float		scan;
    FILE *fp;									// указатель на файл
    fp=fopen("file_in.txt","r");				// открыть файл для чтения
       											// чтение из файла
    fscanf(fp,"%s",pust);fscanf(fp,"%d",&n);
    fscanf(fp,"%s",pust);fscanf(fp,"%d",&n_cikle);
    fscanf(fp,"%s",pust);
    fscanf(fp,"%f",&scan);a=scan;
    fscanf(fp,"%f",&scan);b=scan;
    fclose(fp);									// закрыть файл для чтения
}
//
//----------	Вывод данных
//
void Vyvod()
{
     int pr;
    FILE *fp;
    fp=fopen("file_out.txt","w");		 		// открыть файл для записи
    											// запись в файл
    fprintf(fp,"  УМНОЖЕНИЕ МАТРИЦ\n\n");
	fprintf(fp,"  Размерность матрицы равна  %4d\n\n",n);
    fprintf(fp,"  Способ              Время           Отношение\n");
    fprintf(fp,"  умножения           выполнения      в процентах\n\n");
    pr=(100*timeR)/timeR;
    fprintf(fp,"  Обычное (i,j)     %8d           %4d\n\n",timeR,pr);
    pr=(100*timeR1)/timeR;
    fprintf(fp,"  Обычное (j,i)     %8d           %4d\n\n",timeR1,pr);
    pr=(100*timeR2)/timeR;
    fprintf(fp,"  По строкам (i,j)  %8d           %4d\n\n",timeR2,pr);
    fclose(fp);	
}
//
//----------	Обычное умножение (i,j)
//
void Umn_ij()
{    
    timeBegin = clock();						// время начала выполнения
    
    for (int m=0; m<n_cikle; m++)
    {
    for (int i=0; i<n; i++)
    	for (int j=0; j<n; j++)	C[i*n+j]=0;
    for (int i=0; i<n; i++)
    	for (int j=0; j<n; j++)
    		for (int k=0; k<n; k++) C[i*n+j]+=A[i*n+k]*B[k*n+j];
    }
    timeEnd = clock();							// время окончания выполнения
    int time=timeEnd-timeBegin;
    if (time<1) timeR=1;
	else timeR=time;
}
//
//----------	Обычное умножение (j,i)
//
void Umn_ji()
{
    timeBegin = clock();						// время начала выполнения
    for (int m=0; m<n_cikle; m++)
    {
    for (int i=0; i<n; i++)
    	for (int j=0; j<n; j++)	C[i*n+j]=0;
    for (int j=0; j<n; j++)
    	for (int i=0; i<n; i++)
    		for (int k=0; k<n; k++) C[i*n+j]+=A[i*n+k]*B[k*n+j];
    }
    timeEnd = clock();							// время окончания выполнения
    timeR1=timeEnd-timeBegin;
}
//
//----------	Умножение "по строкам"
//
void Umn()
{
    timeBegin = clock();						// время начала выполнения
    
    for (int m=0; m<n_cikle; m++)
    {
    for (int i=0; i<n; i++)
    	for (int j=0; j<n; j++)	C[i*n+j]=0;
    for (int i=0; i<n; i++)
    	for (int k=0; k<n; k++)
    			for (int j=0; j<n; j++) C[i*n+j]+=A[i*n+k]*B[k*n+j];
    }
    timeEnd = clock();							// время окончания выполнения
    timeR2=timeEnd-timeBegin;
}
