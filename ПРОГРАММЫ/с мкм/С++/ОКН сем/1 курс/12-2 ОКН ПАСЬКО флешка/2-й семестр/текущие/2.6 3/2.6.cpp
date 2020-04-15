#include <cstdlib>
#include <iostream>
#include <stdio.h>
//#include "ReadImage.h"
#include <math.h>
#include "Graph.h"
using namespace std;

double	a,b,c,d,x0,eps,Nx,Ny,xn,xk;
int	nRis;	
char pust[100];
float  scan;
FILE *fp=fopen("in.txt","r");	
FILE *pf=fopen("out.txt","w");

void Vvod();									
void line();									
double f(double x);
double df(double x);
double fy(double x,double xk);
void Ris();
void risk(double xk,int k);


int main()
{
	Vvod();

	Ris();		
	line();

}


void Vvod()
{

	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan); a=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);b=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);c=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);d=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);Nx=scan;
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);Ny=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);x0=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);eps=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&scan);nRis=(int)scan;

	//fprintf(pf,"%f %f %f %f %f %f %f %f %d\n",a,b,c,d,Nx,Ny,x0,eps,nRis);


	fclose(fp);

}



void line()
{   int k=0;


double x=x0;
//h=0.00001;
//double du=(f(x+h)-f(x))/h;
//du =df(x);

while(fabs(f(x))>=eps)
{
//x=x-f(x)/du;
x=x-f(x)/df(x);
k++;

if (k==1) risk(x,k);
if (k==2) risk(x,k);
}



//	 if (f(a)*f(b)>0) // если знаки функции на краях отрезка одинаковые, то здесь нет корня
//                  fprintf(pf,"нет корня");
//                           else
//                           {
//                           if (f(a)*d2f(a)>0) xk  = a; // для выбора начальной точки проверяем f(x0)*d2f(x0)>0 ?
//                                  else xk = b;
//                           xn = xk-f(xk)/df(xk); // считаем первое приближение
//                        
//                           while(fabs(xk-xn) > eps) // пока не достигнем необходимой точности, будет продолжать вычислять
//                           {
//                                  xk = xn;
//                                  xn = xk-f(xk)/df(xk); // непосредственно формула Ньютона
//                           }
//                    fprintf(pf,"корень ");fprintf(pf,"%f \n",xn);
//                    }


fprintf(pf,"погрешность ");fprintf(pf,"%f \n",eps);
fprintf(pf,"номер итерации "); fprintf(pf,"%d \n",k);
fprintf(pf,"корень ");fprintf(pf,"%f \n",x);
x0=x;

fclose(pf);		

}


void Ris()
{  
	SetColor(250,250,250);						// задаем фоновый цвет окна
	SetWindow(a,b,c,d);							// создаём окно (создаем массивы R,G,B)
	// с пределами [a,b]x[c,d]
	SetColor(0,0,0);							// задаем цвет координатных осей
	xyLine(a,0,Nx,Ny);							// строим оси, пересекающиеся в т. (a,0)
	// с шагом делений по х равным Nx
	// и Ny -- по у
	// рисуем 1-й график
	SetColor(0,255,0);    			            // задаем цвет линии
	double step=(b-a)/nRis;
	double x=b;
	SetPoint(x,f(x));
						// устанавливаем курсор
	for (int i=0; i<nRis; i++)
	{ 
		x-=step;
		if ((f(x)>c+eps)&(f(x)<d-eps))Line2(x,f(x));
		//if(f(x)<=c+eps) break;								// строим отрезок, 
		// соединяющий курсор с точкой (х,у)											// и перемещаем курсор в эту точку
	}

	CloseWindow();								// закрываем окно (создаем bmp-файл)
}



void risk(double xk, int k)
{											
	SetColor(30*k,10*k,50);    			            // задаем цвет линии
	double step=(b-a)/nRis;
	double x=b;
	SetPoint(x,fy(x,xk));							// устанавливаем курсор
	for (int i=0; i<nRis; i++)
	{ 
		x-=step;
		Line2(x,fy(x,xk));
		if(fy(x,xk)<=0) break;							// строим отрезок, 
		// соединяющий курсор с точкой (х,у)											// и перемещаем курсор в эту точку
	}

	CloseWindow();

}





double f(double x)
{
	return (sin(x)+pow(x,7)-1);
	//return x*x-2;
}

double df(double x)
{    return (cos(x)+7*pow(x,6));
	//return 2*x;
}

double fy(double x,double xk)
{
double h=0.00001;
double du=(f(xk+h)-f(xk))/h;

//return (du*(x-xk)+f(xk));
return (df(xk)*(x-xk)+f(xk));
}



