#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include "ReadImage.h"
#include <math.h>
using namespace std;

double	a,b,c,d,x0,eps;
float sc,sv,sb;
int h,w,n,i,j;
double q[100];
double r[100],g[100],t[100];
char *R,*G,*B;
char pust[100];
	float  scan;
	FILE *fp=fopen("in.txt","r");	
   	FILE *pf=fopen("out.txt","w");

void Vvod();									
void line();									
double f(double x);
double df(double x);
double fy(double x,double xk,double y);
double f1(double x,double y);
void linef();
void risf();
void risk(double xk);


int main()
{
	Vvod();
	
	GetSize("input.bmp", &w, &h);	// (w -- ширина, h -- высота)
	R = new char[h*w];
	G = new char[h*w];
	B = new char[h*w];
	ReadImageFromFile("input.bmp",B,G,R);
			
    line();
    
    WriteImage("output.bmp",B,G,R);		
}


void Vvod()
{
						
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan); a=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);
	b=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);
	c=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);
	d=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    fscanf(fp,"%f",&scan);
	x0=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);
	eps=scan;
	
	//fprintf(pf,"%f %f %f %f %f %f\n",a,b,c,d,x0,eps);
	

	fclose(fp);
    							
}


double f(double x)
{
	return sin(x)-pow(x,7)-1;
	//return x*x-x;
}
double f1(double x,double y)
{
	return f(x)-y;
}
double df(double x)
{ return cos(x)-7*pow(x,6);
}
double fy(double x,double xk,double y)
{
	return df(xk)*(x-xk)+f(xk)-y;
}

void risf()
{double dx=(b-a)/w, dy=(d-c)/h;
	int m=h*w;

	for (double x=b;x>a+dx;x-=dx)
	{	
		for (double y=d;y>c+dy;y-=dy)
		{ 
		if((f1(x,y)*f1(x-dx,y)>=0)&(f1(x,y)*f1(x,y-dy))>=0)
		{R[m]=255;
		 G[m]=255;
		 B[m]=255;
		 
		}
        else
        {R[m]=0;
		 G[m]=0;
		 B[m]=0;
        }
    			m--;
		}
		m--;
	}

}


void risk(double xk)
{double dx=(b-a)/w, dy=(d-c)/h;
	int m=h*w;

	for (double x=b;x>a+dx;x-=dx)
	{	
		for (double y=d;y>c+dy;y-=dy)
		{ 
		if((fy(x,xk,y)*fy(x-dx,xk,y)>=0)&(fy(x,xk,y)*fy(x,xk,y-dy))>=0)
		{R[m]=255;
		 G[m]=255;
		 B[m]=255;
		 
		}
        else
        {R[m]=0;
		 G[m]=0;
		 B[m]=0;
        }
    			m--;
		}
		m--;
	}

}




void line()
{   
     risf();
	 
	 
	 double xk=x0;
	 int k=0;
	 
	 while(fabs(f(xk))>=eps)
	 {
	 	xk=xk-f(xk)/df(xk);	
	 	k++;
	 	if(k%7==0) risk(xk);
	 }
	 
	 fprintf(pf,"номер итерации "); fprintf(pf,"%d \n",k);
	 fprintf(pf,"погрешность ");fprintf(pf,"%f \n",eps);
	 fprintf(pf,"корень ");fprintf(pf,"%f \n",xk);
	
fclose(pf);		

}













