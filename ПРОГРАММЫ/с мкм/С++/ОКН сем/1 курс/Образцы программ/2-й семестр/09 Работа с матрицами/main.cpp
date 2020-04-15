//				Учебная программа 
//				
//				Работа с матрицами
//
//				19 февраля 2015 г.				
//---------------------------------------------------------------------------------------

//#include "stdafx.h"
//#include <stdio.h>
//#include <math.h>
//#include <conio.h>
//#include <ctime>

#include <cstdlib>
#include <iostream>
//#include <ctime>
#include <math.h>

void Vvod(double *a,double *b,int *n,int s);
void Vyvod(double *a,double *b,int n,double x);
double Matrix(double *a,double *b,int n);

//---------------------------------------------------------------------------------------
int main()
{
	int n;									// размерность СЛАУ
	double *a,*b;
	Vvod(a,b,&n,0);							// читаем значение n
	a = new double [n*n];
	b = new double [n*n];
	Vvod(a,b,&n,1);							// читаем значения матриц A и B
	double x=Matrix(a,b,n);
	Vyvod(a,b,n,x);
}
//---------------------------------------------------------------------------------------
void Vvod(double *a,double *b,int *nn,int s)
{
#define A(i,j) a[(i)*n+j]
#define B(i,j) b[(i)*n+j]
	int n;
	char  pust[100];
	float scan;
	FILE *fp;
	fp=fopen("in.txt","r");

	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%d",&n);
	*nn=n;
	if (s==0) return;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	for (int i=0; i<n; i++) 
		for (int j=0; j<n; j++)
		{
			fscanf(fp,"%f",&scan);
			A(i,j)=scan;
		}
		fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
		for (int i=0; i<n; i++) 
			for (int j=0; j<n; j++)
			{
				fscanf(fp,"%f",&scan);
				B(i,j)=scan;
			}
			fclose(fp);
}
//---------------------------------------------------------------------------------------
double Matrix(double *a,double *b,int n)
{
#define sqr(x) (x)*(x)
#define A(i,j) a[(i)*n+j]
#define B(i,j) b[(i)*n+j]
#define C(i,j) c[(i)*n+j]
	double *c;
	c = new double[n*n];
	double s=0;
	for(int i=0;i<n;i++)
	{
		for(int j=0;j<n;j++)
		{
			double s=0;
			for(int k=0;k<n;k++)
				s+=A(i,k)*B(k,j);
			C(i,j)=s;
		}
	}
	for(int i=0;i<n;i++)
		for(int j=0;j<n;j++)
			s+=sqr(C(i,j));
	return(s);
	delete c;
}
//---------------------------------------------------------------------------------------
void Vyvod(double *a,double *b,int n,double sum)
{
#define A(i,j) a[(i)*n+j]
#define B(i,j) b[(i)*n+j]
	FILE *fp;
	fp=fopen("out.txt","w");

	fprintf(fp,"  ПРИМЕР РАБОТЫ С МАТРИЦАМИ\n\n");
	fprintf(fp,"  Исходные матрицы\n\n");
	fprintf(fp,"  Матрица А:\n\n");
	for (int i=0; i<n; i++)
	{
		for (int j=0; j<n; j++)
			fprintf(fp," %4.1f ",A(i,j));
		fprintf(fp,"\n");
	}
	fprintf(fp,"\n");
	fprintf(fp,"  Матрица B:\n\n");
	for (int i=0; i<n; i++)
	{
		for (int j=0; j<n; j++)
			fprintf(fp," %4.1f ",B(i,j));
		fprintf(fp,"\n");
	}
	fprintf(fp,"\n\n  Сумма квадратов элементов матрицы А*В");
	fprintf(fp,"\n  равна  %f",sum);
	fclose(fp);	
}	
